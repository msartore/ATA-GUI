using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ATA_GUI.Classes;
using ATA_GUI.Utils;

namespace ATA_GUI.Forms
{
    public partial class ScrcpyForm : Form
    {
        private List<EncoderData> audioEncoders;
        private List<EncoderData> videoEncoders;

        public ScrcpyForm()
        {
            InitializeComponent();
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            StringBuilder commandBuilder = new("-s ");

            commandBuilder.Append(ATA.CurrentDeviceSelected.ID);
            commandBuilder.Append(checkBoxTSOff.Checked ? " -Sw" : "");
            commandBuilder.Append(checkBoxPOOC.Checked ? " --power-off-on-close" : "");
            commandBuilder.Append(checkBoxST.Checked ? " -t" : "");
            commandBuilder.Append(checkBoxPOOS.Checked ? "" : " --no-power-on");

            int videoCodecIndex = comboBoxVideoCodec.SelectedIndex;
            int audioCodecIndex = comboBoxAudioCodec.SelectedIndex;

            commandBuilder.AppendFormat(" --video-codec={0} --video-encoder={1}",
                videoEncoders[videoCodecIndex].Codec,
                videoEncoders[videoCodecIndex].Encoder);
            commandBuilder.AppendFormat(" --audio-codec={0} --audio-encoder={1}",
                audioEncoders[audioCodecIndex].Codec,
                audioEncoders[audioCodecIndex].Encoder);

            ConsoleProcess.ScrcpyProcess(commandBuilder.ToString());
        }

        private async void ScrcpyForm_Shown(object sender, EventArgs e)
        {
            string encoders = await ConsoleProcess.ScrcpyAsk("-s " + ATA.CurrentDeviceSelected.ID + " --list-encoders");

            if (encoders != null)
            {
                videoEncoders = new List<EncoderData>();
                audioEncoders = new List<EncoderData>();

                videoEncoders.AddRange(EncoderData.ExtractEncoders(encoders, MEDIA.VIDEO));
                audioEncoders.AddRange(EncoderData.ExtractEncoders(encoders, MEDIA.AUDIO));

                foreach (EncoderData item in videoEncoders)
                {
                    comboBoxVideoCodec.Items.Add(item.Codec + " by " + item.Encoder);
                }

                foreach (EncoderData item in audioEncoders)
                {
                    comboBoxAudioCodec.Items.Add(item.Codec + " by " + item.Encoder);
                }

                comboBoxVideoCodec.SelectedIndex = 0;
                comboBoxAudioCodec.SelectedIndex = 0;
            }
        }
    }
}
