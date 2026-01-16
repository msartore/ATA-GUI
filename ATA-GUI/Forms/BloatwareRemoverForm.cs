using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using ATA_GUI.Classes;
using ATA_GUI.Utils;

namespace ATA_GUI
{
    public partial class BloatwareRemoverForm : Form
    {
        private readonly List<string> packageList = new();
        private readonly HashSet<string> nonSystemAppSet;
        private readonly HashSet<string> systemAppSet;
        private readonly HashSet<string> foundPackageList = new();
        public List<DataGridViewRow> selectedRows { get; set; }

        public string CurrentDevice { get; set; }

        public BloatwareRemoverForm(List<string> nonSystemApp, List<string> systemApp)
        {
            InitializeComponent();
            this.nonSystemAppSet = new HashSet<string>(nonSystemApp.Select(it => it.Trim()));
            this.systemAppSet = new HashSet<string>(systemApp.Select(it => it.Trim()));
            selectedRows = new List<DataGridViewRow>();
        }

        public string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray().Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        private bool IsValidPackageName(string packageName)
        {
            if (string.IsNullOrWhiteSpace(packageName))
                return false;

            // Android package names must follow Java package naming conventions
            // They should be in the format com.example.name
            // Consist of lowercase letters, numbers, dots, and underscores
            // Cannot start with a number or dot

            string[] parts = packageName.Split('.');
            if (parts.Length == 0)
                return false;

            foreach (string part in parts)
            {
                if (string.IsNullOrEmpty(part))
                    return false;

                // Check if the part starts with a digit
                if (char.IsDigit(part[0]))
                    return false;

                // Each part should contain only valid identifier characters
                foreach (char c in part)
                {
                    if (!(char.IsLetterOrDigit(c) || c == '_'))
                        return false;
                }
            }

            return true;
        }

        private async void BloatwareRemover_Shown(object sender, EventArgs e)
        {
            comboBoxActionMode.SelectedIndex = 0;
            comboBoxScanLevel.SelectedIndex = 0;

            // Configure the DataGridView for optimal display
            ConfigureDataGridView();

            await LoadAppAsync();
            MainForm.MessageShowBox("Warning: Be careful before disabling/removing any system app or service. You must ensure that the package is not used by system " +
                "to function. Disabling a critical system app may result in bricking your phone. So always double check before disabling/removing any system app.", 1);
        }

        private void ConfigureDataGridView()
        {
            // Set up the DataGridView for better display of multiline content
            // Don't use AutoSizeRowsMode.AllCells as it causes performance issues during resize
            dataGridViewBloatwareList.DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Set a reasonable fixed row height to avoid constant recalculation during resize
            dataGridViewBloatwareList.RowTemplate.Height = 60;

            // Set column headers to wrap text if needed
            dataGridViewBloatwareList.EnableHeadersVisualStyles = false;
            dataGridViewBloatwareList.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            // Ensure proper grid styling for better column separation
            dataGridViewBloatwareList.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridViewBloatwareList.GridColor = System.Drawing.SystemColors.ControlLight;
        }


        private async Task LoadAppAsync()
        {
            string jsonFilePath = Path.Combine(Application.StartupPath, "uad_lists.json");
            JObject json = null;

            // Check if the file exists locally
            if (!File.Exists(jsonFilePath))
            {
                // Ask user if they want to download the file using a simple dialog
                DialogResult result = MessageBox.Show(
                    "The bloatware list file was not found. Would you like to download it from the Universal Android Debloater repository?",
                    "Download Bloatware List",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Download the file
                    await DownloadBloatwareListAsync(jsonFilePath);
                }
                else
                {
                    // If user declines, show error message
                    MainForm.MessageShowBox("Cannot proceed without bloatware list. Please ensure the file is available or allow download.", 0);
                    return;
                }
            }

            // If we haven't loaded JSON yet (because file was downloaded or user declined download)
            if (json == null && File.Exists(jsonFilePath))
            {
                string jsonString = File.ReadAllText(jsonFilePath);
                json = JObject.Parse(jsonString);
            }

            if (json == null || !File.Exists(jsonFilePath))
            {
                // If still no JSON, show error and return
                MainForm.MessageShowBox("Could not load bloatware list. The application may not function properly.", 0);
                return;
            }

            // Run the rest of the processing on a background thread
            await Task.Run(() =>
            {
                try
                {
                    foundPackageList.Clear();
                    packageList.Clear();

                    // Use Invoke to update UI from background thread
                    Invoke(new Action(() =>
                    {
                        dataGridViewBloatwareList.Rows.Clear();
                    }));

                    int counter = 0;
                    int totalPackages = json.Count;

                    // Get all packages from the JSON
                    foreach (var kvp in json)
                    {
                        string packageId = kvp.Key;
                        var packageData = (JObject)kvp.Value;

                        string listType = packageData["list"]?.ToString();
                        string description = packageData["description"]?.ToString();
                        string removal = packageData["removal"]?.ToString();

                        // Validate package ID format (Android package names follow specific rules)
                        if (!IsValidPackageName(packageId))
                        {
                            continue; // Skip invalid package names
                        }

                        // Determine if this package should be included based on scan level
                        string scanLevel = "";
                        Invoke(new Action(() =>
                        {
                            scanLevel = comboBoxScanLevel.Text;
                        }));

                        bool includePackage = false;
                        switch (scanLevel)
                        {
                            case "basic":
                                // For basic level, include only recommended packages
                                includePackage = removal == "Recommended";
                                break;
                            case "medium":
                                // For medium level, include recommended and advanced packages
                                includePackage = removal == "Recommended" || removal == "Advanced";
                                break;
                            case "advance":
                                // For advance level, include all packages except Expert
                                includePackage = removal != "Expert";
                                break;
                            case "expert":
                                // For expert level, include all packages
                                includePackage = true;
                                break;
                            default:
                                Invoke(new Action(() =>
                                {
                                    MainForm.MessageShowBox("Error, this value can't be set", 0);
                                }));
                                return;
                        }

                        if (includePackage &&
                            !string.IsNullOrEmpty(packageId) &&
                            (nonSystemAppSet.Contains(packageId) || systemAppSet.Contains(packageId)))
                        {
                            // Check if this package already exists in the DataGridView to avoid duplicates
                            // and add it if it doesn't exist - all in one UI thread operation
                            Invoke(new Action(() =>
                            {
                                bool packageExists = false;
                                foreach (DataGridViewRow row in dataGridViewBloatwareList.Rows)
                                {
                                    if (row.Cells[0].Value?.ToString() == packageId)
                                    {
                                        packageExists = true;
                                        break;
                                    }
                                }

                                if (!packageExists)
                                {
                                    int rowIndex = dataGridViewBloatwareList.Rows.Add(packageId, description);

                                    // Color the row based on severity level
                                    DataGridViewCellStyle cellStyle = new DataGridViewCellStyle();

                                    switch (removal?.ToLower())
                                    {
                                        case "recommended":
                                            cellStyle.BackColor = Color.LightGreen;
                                            break;
                                        case "advanced":
                                            cellStyle.BackColor = Color.Orange;
                                            break;
                                        case "unsafe":
                                            cellStyle.BackColor = Color.Red;
                                            cellStyle.ForeColor = Color.White;
                                            break;
                                        case "expert":
                                            cellStyle.BackColor = Color.Purple;
                                            cellStyle.ForeColor = Color.White;
                                            break;
                                        default:
                                            // Use default color for unknown severity levels
                                            break;
                                    }

                                    // Apply the style to all cells in the row
                                    if (dataGridViewBloatwareList.Rows[rowIndex].Cells != null)
                                    {
                                        foreach (DataGridViewCell cell in dataGridViewBloatwareList.Rows[rowIndex].Cells)
                                        {
                                            if (cell != null)
                                            {
                                                cell.Style = cellStyle;
                                            }
                                        }
                                    }
                                    counter++; // Increment counter in the UI thread
                                }
                            }));
                        }
                    }

                    Invoke(new Action(() =>
                    {
                        labelBC.Text = "Bloatware found: " + counter;
                        // Resize rows after all data is loaded to improve appearance without affecting resize performance
                        dataGridViewBloatwareList.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);

                        // Apply search filter if there's text in the search box
                        if (textBoxSearch != null && !string.IsNullOrEmpty(textBoxSearch.Text))
                        {
                            // Trigger the search to filter the newly loaded data
                            textBoxSearch_TextChanged(null, EventArgs.Empty);
                        }
                    }));
                }
                catch (Exception ex)
                {
                    Invoke(new Action(() =>
                    {
                        MainForm.MessageShowBox($"Error loading bloatware list: {ex.Message}", 0);
                    }));
                }
            });
        }

        private async Task DownloadBloatwareListAsync(string filePath)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    // Set a user agent to avoid being blocked by some servers
                    client.DefaultRequestHeaders.Add("User-Agent", "ATA-GUI");

                    // Download the JSON content
                    string jsonContent = await client.GetStringAsync("https://raw.githubusercontent.com/Universal-Debloater-Alliance/universal-android-debloater-next-generation/refs/heads/main/resources/assets/uad_lists.json");

                    // Write the content to the local file
                    File.WriteAllText(filePath, jsonContent);

                    Invoke(new Action(() =>
                    {
                        MainForm.MessageShowBox("Bloatware list downloaded successfully!", 2);
                    }));
                }
            }
            catch (Exception ex)
            {
                Invoke(new Action(() =>
                {
                    MainForm.MessageShowBox($"Error downloading bloatware list: {ex.Message}", 0);
                }));
            }
        }

        // Legacy LoadApp method kept for backward compatibility if needed
        private void LoadApp()
        {
            // Call the async version instead
            _ = LoadAppAsync();
        }

        private void buttonAction_Click(object sender, EventArgs e)
        {
            if (selectedRows.Count > 0)
            {
                List<string> listFailed = new();
                List<string> listSuccess = new();

                foreach (DataGridViewRow row in selectedRows)
                {
                    if (row.IsNewRow) continue; // Skip the new row template

                    string app = row.Cells[0].Value?.ToString(); // Package column
                    if (string.IsNullOrEmpty(app)) continue;

                    // Validate the package name to prevent command injection
                    if (!IsValidPackageName(app))
                    {
                        listFailed.Add($"{app} (invalid package name)");
                        continue;
                    }

                    if (nonSystemAppSet.Contains(app) || comboBoxActionMode.SelectedIndex == 1)
                    {
                        string command = $"shell pm uninstall -k --user {ATA.CurrentDeviceSelected.User} {app}";
                        string result = ConsoleProcess.AdbFastbootCommandR(MainForm.commandAssemblerF(command), 0);

                        if (result != null && result.Contains("Success"))
                        {
                            listSuccess.Add(app);
                        }
                        else
                        {
                            listFailed.Add(app);
                        }
                    }
                    else
                    {
                        string command = $"shell pm disable-user --user {ATA.CurrentDeviceSelected.User} {app}";
                        string result = ConsoleProcess.AdbFastbootCommandR(MainForm.commandAssemblerF(command), 0);

                        if (result != null && result.Contains(app + " new state: disabled-user"))
                        {
                            listSuccess.Add(app);
                        }
                        else
                        {
                            listFailed.Add(app);
                        }
                    }
                }

                string successMessage = string.Format("Following apps {0}:\n{1}\n{2}",
                    comboBoxActionMode.Text,
                    string.Join("\n", listSuccess),
                    listFailed.Count > 0 ? $"ATA was not able to {comboBoxActionMode.SelectedText} the following apps:\n{string.Join("\n", listFailed)}\nSome system application can be only removed and not disabled" : "");

                MainForm.MessageShowBox(successMessage, 2);
                Close();
            }
            else
            {
                MainForm.MessageShowBox("No app selected!", 1);
            }
        }

        private async void comboBoxScanLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkBoxSelectAll.Checked = false;
            await LoadAppAsync();
        }

        private void checkBoxSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSelectAll.Checked)
            {
                dataGridViewBloatwareList.SelectAll();
                setWantedRows();
            }
            else
            {
                dataGridViewBloatwareList.ClearSelection();
                selectedRows.Clear();
            }
        }

        private void setWantedRows()
        {
            selectedRows.Clear();

            foreach (DataGridViewRow row in dataGridViewBloatwareList.SelectedRows)
            {
                if (row.Visible && !row.IsNewRow)
                {
                    selectedRows.Add(row);
                }
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            // Check if controls are properly initialized
            if (textBoxSearch == null || dataGridViewBloatwareList == null || labelBC == null)
                return;

            string searchText = textBoxSearch.Text?.ToLowerInvariant() ?? "";

            foreach (DataGridViewRow row in dataGridViewBloatwareList.Rows)
            {
                // Check if the search text matches either the package name or description
                string packageName = row.Cells[0]?.Value?.ToString()?.ToLowerInvariant() ?? "";
                string description = row.Cells[1]?.Value?.ToString()?.ToLowerInvariant() ?? "";

                bool isVisible = packageName.Contains(searchText) || description.Contains(searchText);
                row.Visible = isVisible;
            }

            // Update the count of visible rows
            int visibleCount = dataGridViewBloatwareList.Rows.Cast<DataGridViewRow>()
                .Count(row => row.Visible && !row.IsNewRow);
            labelBC.Text = $"Bloatware found: {visibleCount}";
        }

        public async void RefreshList()
        {
            await LoadAppAsync();
        }

        private void dataGridViewBloatwareList_SelectionChanged(object sender, EventArgs e)
        {
            setWantedRows();
        }
    }
}