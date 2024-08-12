﻿namespace ATA_GUI.Forms
{
    partial class ShellCommandsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShellCommandsForm));
            comboBoxMethod = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            comboBoxPermission = new System.Windows.Forms.ComboBox();
            buttonRun = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // comboBoxMethod
            // 
            comboBoxMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxMethod.FormattingEnabled = true;
            comboBoxMethod.Items.AddRange(new object[] { "grant", "revoke" });
            comboBoxMethod.Location = new System.Drawing.Point(31, 47);
            comboBoxMethod.Name = "comboBoxMethod";
            comboBoxMethod.Size = new System.Drawing.Size(104, 23);
            comboBoxMethod.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(31, 29);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(49, 15);
            label1.TabIndex = 1;
            label1.Text = "Method";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(154, 29);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(65, 15);
            label2.TabIndex = 2;
            label2.Text = "Permission";
            // 
            // comboBoxPermission
            // 
            comboBoxPermission.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxPermission.FormattingEnabled = true;
            comboBoxPermission.Items.AddRange(new object[] { "ACCESS_ADSERVICES_AD_ID", "ACCESS_ADSERVICES_ATTRIBUTION", "ACCESS_ADSERVICES_CUSTOM_AUDIENCE", "ACCESS_ADSERVICES_MANAGER", "ACCESS_ADSERVICES_STATE", "ACCESS_ADSERVICES_TOPICS", "ACCESS_ALL_DOWNLOADS", "ACCESS_AMBIENT_CONTEXT_EVENT", "ACCESS_AMBIENT_LIGHT_STATS", "ACCESS_BLOBS_ACROSS_USERS", "ACCESS_BLUETOOTH_SHARE", "ACCESS_BROADCAST_RADIO", "ACCESS_BROADCAST_RESPONSE_STATS", "ACCESS_CACHE_FILESYSTEM", "ACCESS_CHECKIN_PROPERTIES", "ACCESS_CONTENT_PROVIDERS_EXTERNALLY", "ACCESS_CONTEXT_HUB", "ACCESS_DOWNLOAD_MANAGER", "ACCESS_DOWNLOAD_MANAGER_ADVANCED", "ACCESS_DRM_CERTIFICATES", "ACCESS_FM_RADIO", "ACCESS_FPS_COUNTER", "ACCESS_GPU_SERVICE", "ACCESS_IMS_CALL_SERVICE", "ACCESS_INPUT_FLINGER", "ACCESS_INSTANT_APPS", "ACCESS_KEYGUARD_SECURE_STORAGE", "ACCESS_LOCATION_EXTRA_COMMANDS", "ACCESS_LOCUS_ID_USAGE_STATS", "ACCESS_LOWPAN_STATE", "ACCESS_MESSAGES_ON_ICC", "ACCESS_MOCK_LOCATION", "ACCESS_MTP", "ACCESS_NETWORK_CONDITIONS", "ACCESS_NETWORK_STATE", "ACCESS_NOTIFICATIONS", "ACCESS_NOTIFICATION_POLICY", "ACCESS_PDB_STATE", "ACCESS_PRIVILEGED_AD_ID", "ACCESS_PRIVILEGED_APP_SET_ID", "ACCESS_RCS_USER_CAPABILITY_EXCHANGE", "ACCESS_SHARED_LIBRARIES", "ACCESS_SHORTCUTS", "ACCESS_SURFACE_FLINGER", "ACCESS_TELEPHONY_SIMINFO_DB", "ACCESS_TUNED_INFO", "ACCESS_TV_DESCRAMBLER", "ACCESS_TV_SHARED_FILTER", "ACCESS_TV_TUNER", "ACCESS_ULTRASOUND", "ACCESS_VIBRATOR_STATE", "ACCESS_VOICE_INTERACTION_SERVICE", "ACCESS_VR_MANAGER", "ACCESS_VR_STATE", "ACCESS_WIFI_STATE", "ACCOUNT_MANAGER", "ACTIVITY_EMBEDDING", "ACT_AS_PACKAGE_FOR_ACCESSIBILITY", "ADD_ALWAYS_UNLOCKED_DISPLAY", "ADD_TRUSTED_DISPLAY", "ADJUST_RUNTIME_PERMISSIONS_POLICY", "ALLOCATE_AGGRESSIVE", "ALLOW_ANY_CODEC_FOR_PLAYBACK", "ALLOW_PLACE_IN_MULTI_PANE_SETTINGS", "ALLOW_SLIPPERY_TOUCHES", "AMBIENT_WALLPAPER", "APPROVE_INCIDENT_REPORTS", "ASEC_ACCESS", "ASEC_CREATE", "ASEC_DESTROY", "ASEC_MOUNT_UNMOUNT", "ASEC_RENAME", "ASSOCIATE_COMPANION_DEVICES", "ASSOCIATE_INPUT_DEVICE_TO_DISPLAY", "AUTHENTICATE_ACCOUNTS", "BACKUP", "BATTERY_PREDICTION", "BATTERY_STATS", "BIND_ACCESSIBILITY_SERVICE", "BIND_AMBIENT_CONTEXT_DETECTION_SERVICE", "BIND_APPWIDGET", "BIND_ATTENTION_SERVICE", "BIND_ATTESTATION_VERIFICATION_SERVICE", "BIND_AUGMENTED_AUTOFILL_SERVICE", "BIND_AUTOFILL", "BIND_AUTOFILL_FIELD_CLASSIFICATION_SERVICE", "BIND_AUTOFILL_SERVICE", "BIND_CACHE_QUOTA_SERVICE", "BIND_CALL_DIAGNOSTIC_SERVICE", "BIND_CALL_REDIRECTION_SERVICE", "BIND_CALL_STREAMING_SERVICE", "BIND_CARRIER_MESSAGING_CLIENT_SERVICE", "BIND_CARRIER_MESSAGING_SERVICE", "BIND_CARRIER_SERVICES", "BIND_CELL_BROADCAST_SERVICE", "BIND_COMPANION_DEVICE_MANAGER_SERVICE", "BIND_CHOOSER_TARGET_SERVICE", "BIND_COMPANION_DEVICE_SERVICE", "BIND_CONDITION_PROVIDER_SERVICE", "BIND_CONNECTION_SERVICE", "BIND_CONTENT_CAPTURE_SERVICE", "BIND_CONTENT_SUGGESTIONS_SERVICE", "BIND_CONTROLS", "BIND_CREDENTIAL_PROVIDER_SERVICE", "BIND_DEVICE_ADMIN", "BIND_DIRECTORY_SEARCH", "BIND_DISPLAY_HASHING_SERVICE", "BIND_DOMAIN_VERIFICATION_AGENT", "BIND_DREAM_SERVICE", "BIND_EUICC_SERVICE", "BIND_EXPLICIT_HEALTH_CHECK_SERVICE", "BIND_EXTERNAL_STORAGE_SERVICE", "BIND_FIELD_CLASSIFICATION_SERVICE", "BIND_GAME_SERVICE", "BIND_GBA_SERVICE", "BIND_HOTWORD_DETECTION_SERVICE", "BIND_IMS_SERVICE", "BIND_INCALL_SERVICE", "BIND_INLINE_SUGGESTION_RENDER_SERVICE", "BIND_INPUT_METHOD", "BIND_INTENT_FILTER_VERIFIER", "BIND_JOB_SERVICE", "BIND_KEYGUARD_APPWIDGET", "BIND_MIDI_DEVICE_SERVICE", "BIND_MUSIC_RECOGNITION_SERVICE", "BIND_NETWORK_RECOMMENDATION_SERVICE", "BIND_NFC_SERVICE", "BIND_NOTIFICATION_ASSISTANT_SERVICE", "BIND_NOTIFICATION_LISTENER_SERVICE", "BIND_PACKAGE_VERIFIER", "BIND_PHONE_ACCOUNT_SUGGESTION_SERVICE", "BIND_PRINT_RECOMMENDATION_SERVICE", "BIND_PRINT_SERVICE", "BIND_PRINT_SPOOLER_SERVICE", "BIND_QUICK_ACCESS_WALLET_SERVICE", "BIND_QUICK_SETTINGS_TILE", "BIND_REMOTEVIEWS", "BIND_REMOTE_DISPLAY", "BIND_REMOTE_LOCKSCREEN_VALIDATION_SERVICE", "BIND_RESOLVER_RANKER_SERVICE", "BIND_RESUME_ON_REBOOT_SERVICE", "BIND_ROTATION_RESOLVER_SERVICE", "BIND_ROUTE_PROVIDER", "BIND_RUNTIME_PERMISSION_PRESENTER_SERVICE", "BIND_SATELLITE_GATEWAY_SERVICE", "BIND_SATELLITE_SERVICE", "BIND_SCREENING_SERVICE", "BIND_SELECTION_TOOLBAR_RENDER_SERVICE", "BIND_SETTINGS_SUGGESTIONS_SERVICE", "BIND_SOUND_TRIGGER_DETECTION_SERVICE", "BIND_TELECOM_CONNECTION_SERVICE", "BIND_TELEPHONY_DATA_SERVICE", "BIND_TELEPHONY_NETWORK_SERVICE", "BIND_TEXTCLASSIFIER_SERVICE", "BIND_TEXT_SERVICE", "BIND_TIME_ZONE_PROVIDER_SERVICE", "BIND_TRACE_REPORT_SERVICE", "BIND_TRANSLATION_SERVICE", "BIND_TRUST_AGENT", "BIND_TV_INPUT", "BIND_TV_INTERACTIVE_APP", "BIND_TV_REMOTE_SERVICE", "BIND_VISUAL_QUERY_DETECTION_SERVICE", "BIND_VISUAL_VOICEMAIL_SERVICE", "BIND_VOICE_INTERACTION", "BIND_VPN_SERVICE", "BIND_VR_LISTENER_SERVICE", "BIND_WALLPAPER", "BIND_WALLPAPER_EFFECTS_GENERATION_SERVICE", "BIND_WEARABLE_SENSING_SERVICE", "BLUETOOTH", "BLUETOOTH_ADMIN", "BLUETOOTH_MAP", "BLUETOOTH_PRIVILEGED", "BLUETOOTH_STACK", "BRICK", "BRIGHTNESS_SLIDER_USAGE", "BROADCAST_CALLLOG_INFO", "BROADCAST_CLOSE_SYSTEM_DIALOGS", "BROADCAST_NETWORK_PRIVILEGED", "BROADCAST_OPTION_INTERACTIVE", "BROADCAST_PACKAGE_REMOVED", "BROADCAST_PHONE_ACCOUNT_REGISTRATION", "BROADCAST_SMS", "BROADCAST_STICKY", "BROADCAST_WAP_PUSH", "BYPASS_ROLE_QUALIFICATION", "CACHE_CONTENT", "CALL_AUDIO_INTERCEPTION", "CALL_COMPANION_APP", "CALL_PRIVILEGED", "CAMERA_DISABLE_TRANSMIT_LED", "CAMERA_INJECT_EXTERNAL_CAMERA", "CAMERA_SEND_SYSTEM_EVENTS", "CAPTURE_AUDIO_HOTWORD", "CAPTURE_AUDIO_OUTPUT", "CAPTURE_BLACKOUT_CONTENT", "CAPTURE_CONSENTLESS_BUGREPORT_ON_USERDEBUG_BUILD", "CAPTURE_MEDIA_OUTPUT", "CAPTURE_SECURE_VIDEO_OUTPUT", "CAPTURE_TUNER_AUDIO_INPUT", "CAPTURE_TV_INPUT", "CAPTURE_VIDEO_OUTPUT", "CAPTURE_VOICE_COMMUNICATION_OUTPUT", "CARRIER_FILTER_SMS", "CHANGE_ACCESSIBILITY_VOLUME", "CHANGE_APP_IDLE_STATE", "CHANGE_APP_LAUNCH_TIME_ESTIMATE", "CHANGE_BACKGROUND_DATA_SETTING", "CHANGE_COMPONENT_ENABLED_STATE", "CHANGE_CONFIGURATION", "CHANGE_DEVICE_IDLE_TEMP_WHITELIST", "CHANGE_HDMI_CEC_ACTIVE_SOURCE", "CHANGE_LOWPAN_STATE", "CHANGE_NETWORK_STATE", "CHANGE_OVERLAY_PACKAGES", "CHANGE_WIFI_MULTICAST_STATE", "CHANGE_WIFI_STATE", "CHECK_REMOTE_LOCKSCREEN", "CLEAR_APP_CACHE", "CLEAR_APP_GRANTED_URI_PERMISSIONS", "CLEAR_APP_USER_DATA", "CLEAR_FREEZE_PERIOD", "COMPANION_APPROVE_WIFI_CONNECTIONS", "CONFIGURE_DISPLAY_BRIGHTNESS", "CONFIGURE_DISPLAY_COLOR_MODE", "CONFIGURE_INTERACT_ACROSS_PROFILES", "CONFIGURE_WIFI_DISPLAY", "CONFIRM_FULL_BACKUP", "CONNECTIVITY_INTERNAL", "CONNECTIVITY_USE_RESTRICTED_NETWORKS", "CONTROL_ALWAYS_ON_VPN", "CONTROL_AUTOMOTIVE_GNSS", "CONTROL_DEVICE_LIGHTS", "CONTROL_DEVICE_STATE", "CONTROL_DISPLAY_BRIGHTNESS", "CONTROL_DISPLAY_COLOR_TRANSFORMS", "CONTROL_DISPLAY_SATURATION", "CONTROL_INCALL_EXPERIENCE", "CONTROL_KEYGUARD", "CONTROL_KEYGUARD_SECURE_NOTIFICATIONS", "CONTROL_LOCATION_UPDATES", "CONTROL_OEM_PAID_NETWORK_PREFERENCE", "CONTROL_REMOTE_APP_TRANSITION_ANIMATIONS", "CONTROL_UI_TRACING", "CONTROL_VPN", "CONTROL_WIFI_DISPLAY", "COPY_PROTECTED_DATA", "CREATE_USERS", "CREATE_VIRTUAL_DEVICE", "CREDENTIAL_MANAGER_QUERY_CANDIDATE_CREDENTIALS", "CREDENTIAL_MANAGER_SET_ALLOWED_PROVIDERS", "CRYPT_KEEPER", "CREDENTIAL_MANAGER_SET_ORIGIN", "CUSTOMIZE_SYSTEM_UI", "DEBUG_VIRTUAL_MACHINE", "DELETE_CACHE_FILES", "DELETE_PACKAGES", "DELETE_STAGED_HEALTH_CONNECT_REMOTE_DATA", "DELIVER_COMPANION_MESSAGES", "DETECT_SCREEN_CAPTURE", "DEVICE_POWER", "DISABLE_HIDDEN_API_CHECKS", "DIAGNOSTIC", "DISABLE_INPUT_DEVICE", "DISABLE_KEYGUARD", "DISABLE_SYSTEM_SOUND_EFFECTS", "DISPATCH_NFC_MESSAGE", "DISPATCH_PROVISIONING_MESSAGE", "DOMAIN_VERIFICATION_AGENT", "DOWNLOAD_CACHE_NON_PURGEABLE", "DUMP", "DVB_DEVICE", "ENABLE_TEST_HARNESS_MODE", "ENFORCE_UPDATE_OWNERSHIP", "ENTER_CAR_MODE_PRIORITIZED", "EXECUTE_APP_ACTION", "EXEMPT_FROM_AUDIO_RECORD_RESTRICTIONS", "EXPAND_STATUS_BAR", "FACTORY_TEST", "FILTER_EVENTS", "FLASHLIGHT", "FORCE_BACK", "FORCE_DEVICE_POLICY_MANAGER_LOGS", "FORCE_PERSISTABLE_URI_PERMISSIONS", "FORCE_STOP_PACKAGES", "FOREGROUND_SERVICE", "FOREGROUND_SERVICE_CAMERA", "FOREGROUND_SERVICE_CONNECTED_DEVICE", "FOREGROUND_SERVICE_DATA_SYNC", "FOREGROUND_SERVICE_FILE_MANAGEMENT", "FOREGROUND_SERVICE_HEALTH", "FOREGROUND_SERVICE_LOCATION", "FOREGROUND_SERVICE_MEDIA_PLAYBACK", "FOREGROUND_SERVICE_MEDIA_PROJECTION", "FOREGROUND_SERVICE_MICROPHONE", "FOREGROUND_SERVICE_PHONE_CALL", "FOREGROUND_SERVICE_REMOTE_MESSAGING", "FOREGROUND_SERVICE_SPECIAL_USE", "FOREGROUND_SERVICE_SYSTEM_EXEMPTED", "FRAME_STATS", "FREEZE_SCREEN", "GET_ACCOUNTS_PRIVILEGED", "GET_ANY_PROVIDER_TYPE", "GET_APP_GRANTED_URI_PERMISSIONS", "GET_APP_METADATA", "GET_APP_OPS_STATS", "GET_DETAILED_TASKS", "GET_HISTORICAL_APP_OPS_STATS", "GET_INTENT_SENDER_INTENT", "GET_PACKAGE_SIZE", "GET_PASSWORD", "GET_PEOPLE_TILE_PREVIEW", "GET_PROCESS_STATE_AND_OOM_SCORE", "GET_RUNTIME_PERMISSIONS", "GET_TASKS", "GET_TOP_ACTIVITY_INFO", "GLOBAL_SEARCH", "GLOBAL_SEARCH_CONTROL", "GRANT_PROFILE_OWNER_DEVICE_IDS_ACCESS", "GRANT_RUNTIME_PERMISSIONS", "GRANT_RUNTIME_PERMISSIONS_TO_TELEPHONY_DEFAULTS", "HANDLE_CALL_INTENT", "HANDLE_CAR_MODE_CHANGES", "HANDLE_QUERY_PACKAGE_RESTART", "HARDWARE_TEST", "HDMI_CEC", "HEALTH_CONNECT_BACKUP_INTER_AGENT", "HIDE_NON_SYSTEM_OVERLAY_WINDOWS", "HIDE_OVERLAY_WINDOWS", "INJECT_EVENTS", "INPUT_CONSUMER", "INSTALL_DPC_PACKAGES", "INSTALL_DYNAMIC_SYSTEM", "INSTALL_GRANT_RUNTIME_PERMISSIONS", "INSTALL_LOCATION_PROVIDER", "INSTALL_LOCATION_TIME_ZONE_PROVIDER_SERVICE", "INSTALL_PACKAGES", "INSTALL_PACKAGE_UPDATES", "INSTALL_SELF_UPDATES", "INSTALL_TEST_ONLY_PACKAGE", "INSTANT_APP_FOREGROUND_SERVICE", "INTENT_FILTER_VERIFICATION_AGENT", "INTERACT_ACROSS_PROFILES", "INTERACT_ACROSS_USERS", "INTERACT_ACROSS_USERS_FULL", "INTERNAL_DELETE_CACHE_FILES", "INTERNAL_SYSTEM_WINDOW", "INTERNET", "INVOKE_CARRIER_SETUP", "KEEP_UNINSTALLED_PACKAGES", "KEYPHRASE_ENROLLMENT_APPLICATION", "KILL_ALL_BACKGROUND_PROCESSES", "KILL_BACKGROUND_PROCESSES", "KILL_UID", "LAUNCH_CAPTURE_CONTENT_ACTIVITY_FOR_NOTE", "LAUNCH_CREDENTIAL_SELECTOR", "LAUNCH_DEVICE_MANAGER_SETUP", "LAUNCH_MULTI_PANE_SETTINGS_DEEP_LINK", "LAUNCH_TRUST_AGENT_SETTINGS", "LISTEN_ALWAYS_REPORTED_SIGNAL_STRENGTH", "LIST_ENABLED_CREDENTIAL_PROVIDERS", "LOADER_USAGE_STATS", "LOCAL_MAC_ADDRESS", "LOCATION_BYPASS", "LOCATION_HARDWARE", "LOCK_DEVICE", "LOG_COMPAT_CHANGE", "LOG_FOREGROUND_RESOURCE_USE", "LOOP_RADIO", "MAINLINE_NETWORK_STACK", "MAKE_UID_VISIBLE", "MANAGE_ACCESSIBILITY", "MANAGE_ACCOUNTS", "MANAGE_ACTIVITY_STACKS", "MANAGE_ACTIVITY_TASKS", "MANAGE_APPOPS", "MANAGE_APP_HIBERNATION", "MANAGE_APP_OPS_MODES", "MANAGE_APP_OPS_RESTRICTIONS", "MANAGE_APP_PREDICTIONS", "MANAGE_APP_TOKENS", "MANAGE_AUDIO_POLICY", "MANAGE_AUTO_FILL", "MANAGE_BIND_INSTANT_SERVICE", "MANAGE_BIOMETRIC", "MANAGE_BIOMETRIC_DIALOG", "MANAGE_BLUETOOTH_WHEN_WIRELESS_CONSENT_REQUIRED", "MANAGE_CAMERA", "MANAGE_CARRIER_OEM_UNLOCK_STATE", "MANAGE_CA_CERTIFICATES", "MANAGE_CLIPBOARD_ACCESS_NOTIFICATION", "MANAGE_CLOUDSEARCH", "MANAGE_COMPANION_DEVICES", "MANAGE_CONTENT_CAPTURE", "MANAGE_CONTENT_SUGGESTIONS", "MANAGE_CRATES", "MANAGE_CREDENTIAL_MANAGEMENT_APP", "MANAGE_DEBUGGING", "MANAGE_DEFAULT_APPLICATIONS", "MANAGE_DEVICE_ADMINS", "MANAGE_DEVICE_LOCK_STATE", "MANAGE_DEVICE_POLICY_ACCESSIBILITY", "MANAGE_DEVICE_POLICY_ACCOUNT_MANAGEMENT", "MANAGE_DEVICE_POLICY_ACROSS_USERS", "MANAGE_DEVICE_POLICY_ACROSS_USERS_FULL", "MANAGE_DEVICE_POLICY_ACROSS_USERS_SECURITY_CRITICAL", "MANAGE_DEVICE_POLICY_AIRPLANE_MODE", "MANAGE_DEVICE_POLICY_APPS_CONTROL", "MANAGE_DEVICE_POLICY_APP_EXEMPTIONS", "MANAGE_DEVICE_POLICY_APP_RESTRICTIONS", "MANAGE_DEVICE_POLICY_APP_USER_DATA", "MANAGE_DEVICE_POLICY_AUDIO_OUTPUT", "MANAGE_DEVICE_POLICY_AUTOFILL", "MANAGE_DEVICE_POLICY_BACKUP_SERVICE", "MANAGE_DEVICE_POLICY_BLUETOOTH", "MANAGE_DEVICE_POLICY_BUGREPORT", "MANAGE_DEVICE_POLICY_CALLS", "MANAGE_DEVICE_POLICY_CAMERA", "MANAGE_DEVICE_POLICY_CERTIFICATES", "MANAGE_DEVICE_POLICY_COMMON_CRITERIA_MODE", "MANAGE_DEVICE_POLICY_DEBUGGING_FEATURES", "MANAGE_DEVICE_POLICY_DEFAULT_SMS", "MANAGE_DEVICE_POLICY_DEVICE_IDENTIFIERS", "MANAGE_DEVICE_POLICY_DISPLAY", "MANAGE_DEVICE_POLICY_FACTORY_RESET", "MANAGE_DEVICE_POLICY_FUN", "MANAGE_DEVICE_POLICY_INPUT_METHODS", "MANAGE_DEVICE_POLICY_INSTALL_UNKNOWN_SOURCES", "MANAGE_DEVICE_POLICY_KEEP_UNINSTALLED_PACKAGES", "MANAGE_DEVICE_POLICY_KEYGUARD", "MANAGE_DEVICE_POLICY_LOCALE", "MANAGE_DEVICE_POLICY_LOCATION", "MANAGE_DEVICE_POLICY_LOCK", "MANAGE_DEVICE_POLICY_LOCK_CREDENTIALS", "MANAGE_DEVICE_POLICY_LOCK_TASK", "MANAGE_DEVICE_POLICY_METERED_DATA", "MANAGE_DEVICE_POLICY_MICROPHONE", "MANAGE_DEVICE_POLICY_MOBILE_NETWORK", "MANAGE_DEVICE_POLICY_MODIFY_USERS", "MANAGE_DEVICE_POLICY_MTE", "MANAGE_DEVICE_POLICY_NEARBY_COMMUNICATION", "MANAGE_DEVICE_POLICY_NETWORK_LOGGING", "MANAGE_DEVICE_POLICY_ORGANIZATION_IDENTITY", "MANAGE_DEVICE_POLICY_OVERRIDE_APN", "MANAGE_DEVICE_POLICY_PACKAGE_STATE", "MANAGE_DEVICE_POLICY_PHYSICAL_MEDIA", "MANAGE_DEVICE_POLICY_PRINTING", "MANAGE_DEVICE_POLICY_PRIVATE_DNS", "MANAGE_DEVICE_POLICY_PROFILES", "MANAGE_DEVICE_POLICY_PROFILE_INTERACTION", "MANAGE_DEVICE_POLICY_PROXY", "MANAGE_DEVICE_POLICY_QUERY_SYSTEM_UPDATES", "MANAGE_DEVICE_POLICY_RESET_PASSWORD", "MANAGE_DEVICE_POLICY_RESTRICT_PRIVATE_DNS", "MANAGE_DEVICE_POLICY_RUNTIME_PERMISSIONS", "MANAGE_DEVICE_POLICY_RUN_IN_BACKGROUND", "MANAGE_DEVICE_POLICY_SAFE_BOOT", "MANAGE_DEVICE_POLICY_SCREEN_CAPTURE", "MANAGE_DEVICE_POLICY_SCREEN_CONTENT", "MANAGE_DEVICE_POLICY_SECURITY_LOGGING", "MANAGE_DEVICE_POLICY_SETTINGS", "MANAGE_DEVICE_POLICY_SMS", "MANAGE_DEVICE_POLICY_STATUS_BAR", "MANAGE_DEVICE_POLICY_SUPPORT_MESSAGE", "MANAGE_DEVICE_POLICY_SUSPEND_PERSONAL_APPS", "MANAGE_DEVICE_POLICY_SYSTEM_APPS", "MANAGE_DEVICE_POLICY_SYSTEM_DIALOGS", "MANAGE_DEVICE_POLICY_SYSTEM_UPDATES", "MANAGE_DEVICE_POLICY_TIME", "MANAGE_DEVICE_POLICY_USB_DATA_SIGNALLING", "MANAGE_DEVICE_POLICY_USB_FILE_TRANSFER", "MANAGE_DEVICE_POLICY_USERS", "MANAGE_DEVICE_POLICY_VPN", "MANAGE_DEVICE_POLICY_WALLPAPER", "MANAGE_DEVICE_POLICY_WIFI", "MANAGE_DEVICE_POLICY_WINDOWS", "MANAGE_DEVICE_POLICY_WIPE_DATA", "MANAGE_DOCUMENTS", "MANAGE_DYNAMIC_SYSTEM", "MANAGE_ETHERNET_NETWORKS", "MANAGE_FACE", "MANAGE_FACTORY_RESET_PROTECTION", "MANAGE_FINGERPRINT", "MANAGE_GAME_ACTIVITY", "MANAGE_GAME_MODE", "MANAGE_HEALTH_DATA", "MANAGE_HEALTH_PERMISSIONS", "MANAGE_HOTWORD_DETECTION", "MANAGE_IPSEC_TUNNELS", "MANAGE_LOWPAN_INTERFACES", "MANAGE_LOW_POWER_STANDBY", "MANAGE_MEDIA", "MANAGE_MEDIA_PROJECTION", "MANAGE_MUSIC_RECOGNITION", "MANAGE_NETWORK_POLICY", "MANAGE_NOTIFICATIONS", "MANAGE_NOTIFICATION_LISTENERS", "MANAGE_ONE_TIME_PERMISSION_SESSIONS", "MANAGE_ONGOING_CALLS", "MANAGE_OWN_CALLS", "MANAGE_PROFILE_AND_DEVICE_OWNERS", "MANAGE_ROLE_HOLDERS", "MANAGE_ROLLBACKS", "MANAGE_ROTATION_RESOLVER", "MANAGE_SAFETY_CENTER", "MANAGE_SCOPED_ACCESS_DIRECTORY_PERMISSIONS", "MANAGE_SEARCH_UI", "MANAGE_SENSORS", "MANAGE_SENSOR_PRIVACY", "MANAGE_SLICE_PERMISSIONS", "MANAGE_SMARTSPACE", "MANAGE_SOUND_TRIGGER", "MANAGE_SPEECH_RECOGNITION", "MANAGE_SUBSCRIPTION_PLANS", "MANAGE_SUBSCRIPTION_USER_ASSOCIATION", "MANAGE_TEST_NETWORKS", "MANAGE_TIME_AND_ZONE_DETECTION", "MANAGE_TOAST_RATE_LIMITING", "MANAGE_UI_TRANSLATION", "MANAGE_USB", "MANAGE_USERS", "MANAGE_USER_OEM_UNLOCK_STATE", "MANAGE_VIRTUAL_MACHINE", "MANAGE_VOICE_KEYPHRASES", "MANAGE_WALLPAPER_EFFECTS_GENERATION", "MANAGE_WEAK_ESCROW_TOKEN", "MANAGE_WEARABLE_SENSING_SERVICE", "MANAGE_WIFI_COUNTRY_CODE", "MANAGE_WIFI_INTERFACES", "MANAGE_WIFI_NETWORK_SELECTION", "MARK_DEVICE_ORGANIZATION_OWNED", "MASTER_CLEAR", "MEDIA_CONTENT_CONTROL", "MEDIA_RESOURCE_OVERRIDE_PID", "MIGRATE_HEALTH_CONNECT_DATA", "MODIFY_ACCESSIBILITY_DATA", "MODIFY_ADSERVICES_STATE", "MODIFY_APPWIDGET_BIND_PERMISSIONS", "MODIFY_AUDIO_ROUTING", "MODIFY_AUDIO_SETTINGS", "MODIFY_AUDIO_SETTINGS_PRIVILEGED", "MODIFY_CELL_BROADCASTS", "MODIFY_DAY_NIGHT_MODE", "MODIFY_DEFAULT_AUDIO_EFFECTS", "MODIFY_HDR_CONVERSION_MODE", "MODIFY_NETWORK_ACCOUNTING", "MODIFY_PARENTAL_CONTROLS", "MODIFY_PHONE_STATE", "MODIFY_QUIET_MODE", "MODIFY_REFRESH_RATE_SWITCHING_TYPE", "MODIFY_SETTINGS_OVERRIDEABLE_BY_RESTORE", "MODIFY_THEME_OVERLAY", "MODIFY_TOUCH_MODE_STATE", "MODIFY_USER_PREFERRED_DISPLAY_MODE", "MONITOR_DEFAULT_SMS_PACKAGE", "MONITOR_DEVICE_CONFIG_ACCESS", "MONITOR_INPUT", "MONITOR_KEYBOARD_BACKLIGHT", "MOUNT_FORMAT_FILESYSTEMS", "MOUNT_UNMOUNT_FILESYSTEMS", "MOVE_PACKAGE", "NETWORK_AIRPLANE_MODE", "NETWORK_BYPASS_PRIVATE_DNS", "NETWORK_CARRIER_PROVISIONING", "NETWORK_FACTORY", "NETWORK_MANAGED_PROVISIONING", "NETWORK_SCAN", "NETWORK_SETTINGS", "NETWORK_SETUP_WIZARD", "NETWORK_SIGNAL_STRENGTH_WAKEUP", "NETWORK_STACK", "NETWORK_STATS_PROVIDER", "NET_ADMIN", "NET_TUNNELING", "NFC", "NFC_HANDOVER_STATUS", "NFC_PREFERRED_PAYMENT_INFO", "NFC_SET_CONTROLLER_ALWAYS_ON", "NFC_TRANSACTION_EVENT", "NOTIFICATION_DURING_SETUP", "NOTIFY_PENDING_SYSTEM_UPDATE", "NOTIFY_TV_INPUTS", "OBSERVE_APP_USAGE", "OBSERVE_GRANT_REVOKE_PERMISSIONS", "OBSERVE_NETWORK_POLICY", "OBSERVE_ROLE_HOLDERS", "OBSERVE_SENSOR_PRIVACY", "OEM_UNLOCK_STATE", "OPEN_ACCESSIBILITY_DETAILS_SETTINGS", "OVERRIDE_COMPAT_CHANGE_CONFIG", "OVERRIDE_COMPAT_CHANGE_CONFIG_ON_RELEASE_BUILD", "OVERRIDE_DISPLAY_MODE_REQUESTS", "OVERRIDE_WIFI_CONFIG", "PACKAGE_ROLLBACK_AGENT", "PACKAGE_USAGE_STATS", "PACKAGE_VERIFICATION_AGENT", "PACKET_KEEPALIVE_OFFLOAD", "PEEK_DROPBOX_DATA", "PEERS_MAC_ADDRESS", "PERFORM_CDMA_PROVISIONING", "PERFORM_IMS_SINGLE_REGISTRATION", "PERFORM_SIM_ACTIVATION", "PERSISTENT_ACTIVITY", "POWER_SAVER", "PROCESS_CALLLOG_INFO", "PROCESS_PHONE_ACCOUNT_REGISTRATION", "PROVIDE_DEFAULT_ENABLED_CREDENTIAL_SERVICE", "PROVIDE_OWN_AUTOFILL_SUGGESTIONS", "PROVIDE_REMOTE_CREDENTIALS", "PROVIDE_RESOLVER_RANKER_SERVICE", "PROVIDE_TRUST_AGENT", "PROVISION_DEMO_DEVICE", "PROVISION_MANAGED_DEVICE_SILENTLY", "QUERY_ADMIN_POLICY", "QUERY_ALL_PACKAGES", "QUERY_AUDIO_STATE", "QUERY_CLONED_APPS", "QUERY_DO_NOT_ASK_CREDENTIALS_ON_BOOT", "QUERY_TIME_ZONE_RULES", "QUERY_USERS", "RADIO_SCAN_WITHOUT_LOCATION", "READ_ACTIVE_EMERGENCY_SESSION", "READ_APP_SPECIFIC_LOCALES", "READ_ASSISTANT_APP_SEARCH_DATA", "READ_BLOCKED_NUMBERS", "READ_CARRIER_APP_INFO", "READ_CLIPBOARD_IN_BACKGROUND", "READ_COMPAT_CHANGE_CONFIG", "READ_CONTENT_RATING_SYSTEMS", "READ_DEVICE_CONFIG", "READ_DREAM_STATE", "READ_DREAM_SUPPRESSION", "READ_FRAME_BUFFER", "READ_GLOBAL_APP_SEARCH_DATA", "READ_HOME_APP_SEARCH_DATA", "READ_INPUT_STATE", "READ_INSTALLED_SESSION_PATHS", "READ_INSTALL_SESSIONS", "READ_LOGS", "READ_LOWPAN_CREDENTIAL", "READ_NEARBY_STREAMING_POLICY", "READ_NETWORK_USAGE_HISTORY", "READ_OEM_UNLOCK_STATE", "READ_PEOPLE_DATA", "READ_PRECISE_PHONE_STATE", "READ_PRINT_SERVICES", "READ_PRINT_SERVICE_RECOMMENDATIONS", "READ_PRIVILEGED_PHONE_STATE", "READ_PROFILE", "READ_PROJECTION_STATE", "READ_RESTRICTED_STATS", "READ_RUNTIME_PROFILES", "READ_SAFETY_CENTER_STATUS", "READ_SEARCH_INDEXABLES", "READ_SOCIAL_STREAM", "READ_SYNC_SETTINGS", "READ_SYNC_STATS", "READ_SYSTEM_UPDATE_INFO", "READ_USER_DICTIONARY", "READ_WALLPAPER_INTERNAL", "READ_WIFI_CREDENTIAL", "READ_WRITE_SYNC_DISABLED_MODE_CONFIG", "REAL_GET_TASKS", "REBOOT", "RECEIVE_BLUETOOTH_MAP", "RECEIVE_BOOT_COMPLETED", "RECEIVE_DATA_ACTIVITY_CHANGE", "RECEIVE_DEVICE_CUSTOMIZATION_READY", "RECEIVE_EMERGENCY_BROADCAST", "RECEIVE_MEDIA_RESOURCE_USAGE", "RECEIVE_STK_COMMANDS", "RECEIVE_WIFI_CREDENTIAL_CHANGE", "RECOVERY", "RECOVER_KEYSTORE", "REGISTER_CALL_PROVIDER", "REGISTER_CONNECTION_MANAGER", "REGISTER_MEDIA_RESOURCE_OBSERVER", "REGISTER_SIM_SUBSCRIPTION", "REGISTER_STATS_PULL_ATOM", "REGISTER_WINDOW_MANAGER_LISTENERS", "REMAP_MODIFIER_KEYS", "REMOTE_AUDIO_PLAYBACK", "REMOTE_DISPLAY_PROVIDER", "REMOVE_DRM_CERTIFICATES", "REMOVE_TASKS", "RENOUNCE_PERMISSIONS", "REORDER_TASKS", "REQUEST_COMPANION_PROFILE_APP_STREAMING", "REQUEST_COMPANION_PROFILE_AUTOMOTIVE_PROJECTION", "REQUEST_COMPANION_PROFILE_COMPUTER", "REQUEST_COMPANION_PROFILE_GLASSES", "REQUEST_COMPANION_PROFILE_NEARBY_DEVICE_STREAMING", "REQUEST_COMPANION_PROFILE_WATCH", "REQUEST_COMPANION_RUN_IN_BACKGROUND", "REQUEST_COMPANION_SELF_MANAGED", "REQUEST_COMPANION_START_FOREGROUND_SERVICES_FROM_BACKGROUND", "REQUEST_COMPANION_USE_DATA_IN_BACKGROUND", "REQUEST_DELETE_PACKAGES", "REQUEST_IGNORE_BATTERY_OPTIMIZATIONS", "REQUEST_INCIDENT_REPORT_APPROVAL", "REQUEST_INSTALL_PACKAGES", "REQUEST_NETWORK_SCORES", "REQUEST_NOTIFICATION_ASSISTANT_SERVICE", "REQUEST_OBSERVE_COMPANION_DEVICE_PRESENCE", "REQUEST_PASSWORD_COMPLEXITY", "REQUEST_UNIQUE_ID_ATTESTATION", "RESET_APP_ERRORS", "RESET_FINGERPRINT_LOCKOUT", "RESET_PASSWORD", "RESET_SHORTCUT_MANAGER_THROTTLING", "RESTART_PACKAGES", "RESTART_WIFI_SUBSYSTEM", "RESTORE_RUNTIME_PERMISSIONS", "RESTRICTED_VR_ACCESS", "RETRIEVE_WINDOW_CONTENT", "RETRIEVE_WINDOW_TOKEN", "REVIEW_ACCESSIBILITY_SERVICES", "REVOKE_POST_NOTIFICATIONS_WITHOUT_KILL", "REVOKE_RUNTIME_PERMISSIONS", "ROTATE_SURFACE_FLINGER", "RUN_IN_BACKGROUND", "RUN_USER_INITIATED_JOBS", "SATELLITE_COMMUNICATION", "SCHEDULE_EXACT_ALARM", "SCHEDULE_PRIORITIZED_ALARM", "SCORE_NETWORKS", "SECURE_ELEMENT_PRIVILEGED_OPERATION", "SEND_CALL_LOG_CHANGE", "SEND_CATEGORY_CAR_NOTIFICATIONS", "SEND_DEVICE_CUSTOMIZATION_READY", "SEND_DOWNLOAD_COMPLETED_INTENTS", "SEND_EMBMS_INTENTS", "SEND_RESPOND_VIA_MESSAGE", "SEND_SAFETY_CENTER_UPDATE", "SEND_SHOW_SUSPENDED_APP_DETAILS", "SEND_SMS_NO_CONFIRMATION", "SERIAL_PORT", "SET_ACTIVITY_WATCHER", "SET_ALWAYS_FINISH", "SET_AND_VERIFY_LOCKSCREEN_CREDENTIALS", "SET_ANIMATION_SCALE", "SET_APP_SPECIFIC_LOCALECONFIG", "SET_CLIP_SOURCE", "SET_DEBUG_APP", "SET_DEFAULT_ACCOUNT_FOR_CONTACTS", "SET_DISPLAY_OFFSET", "SET_GAME_SERVICE", "SET_HARMFUL_APP_WARNINGS", "SET_INITIAL_LOCK", "SET_INPUT_CALIBRATION", "SET_KEYBOARD_LAYOUT", "SET_LOW_POWER_STANDBY_PORTS", "SET_MEDIA_KEY_LISTENER", "SET_ORIENTATION", "SET_POINTER_SPEED", "SET_PREFERRED_APPLICATIONS", "SET_PROCESS_LIMIT", "SET_SCREEN_COMPATIBILITY", "SET_SYSTEM_AUDIO_CAPTION", "SET_TIME", "SET_TIME_ZONE", "SET_UNRESTRICTED_GESTURE_EXCLUSION", "SET_UNRESTRICTED_KEEP_CLEAR_AREAS", "SET_VOLUME_KEY_LONG_PRESS_LISTENER", "SET_WALLPAPER", "SET_WALLPAPER_COMPONENT", "SET_WALLPAPER_DIM_AMOUNT", "SET_WALLPAPER_HINTS", "SHOW_KEYGUARD_MESSAGE", "SHUTDOWN", "SIGNAL_PERSISTENT_PROCESSES", "SIGNAL_REBOOT_READINESS", "SMS_FINANCIAL_TRANSACTIONS", "SOUNDTRIGGER_DELEGATE_IDENTITY", "SOUND_TRIGGER_RUN_IN_BATTERY_SAVER", "STAGE_HEALTH_CONNECT_REMOTE_DATA", "START_ACTIVITIES_FROM_BACKGROUND", "START_ACTIVITY_AS_CALLER", "START_ANY_ACTIVITY", "START_CROSS_PROFILE_ACTIVITIES", "START_FOREGROUND_SERVICES_FROM_BACKGROUND", "START_PRINT_SERVICE_CONFIG_ACTIVITY", "START_REVIEW_PERMISSION_DECISIONS", "START_TASKS_FROM_RECENTS", "START_VIEW_APP_FEATURES", "START_VIEW_PERMISSION_USAGE", "STATSCOMPANION", "STATUS_BAR", "STATUS_BAR_SERVICE", "STOP_APP_SWITCHES", "STORAGE_INTERNAL", "SUBSCRIBED_FEEDS_READ", "SUBSCRIBED_FEEDS_WRITE", "SUBSCRIBE_TO_KEYGUARD_LOCKED_STATE", "SUBSTITUTE_NOTIFICATION_APP_NAME", "SUBSTITUTE_SHARE_TARGET_APP_NAME_AND_ICON", "SUGGEST_EXTERNAL_TIME", "SUGGEST_MANUAL_TIME_AND_ZONE", "SUGGEST_TELEPHONY_TIME_AND_ZONE", "SUPPRESS_CLIPBOARD_ACCESS_NOTIFICATION", "SUSPEND_APPS", "SYSTEM_ALERT_WINDOW", "SYSTEM_APPLICATION_OVERLAY", "TABLET_MODE", "TEMPORARY_ENABLE_ACCESSIBILITY", "TEST_BIOMETRIC", "TEST_BLACKLISTED_PASSWORD", "TEST_INPUT_METHOD", "TEST_MANAGE_ROLLBACKS", "TETHER_PRIVILEGED", "TIS_EXTENSION_INTERFACE", "TOGGLE_AUTOMOTIVE_PROJECTION", "TRANSMIT_IR", "TRIGGER_LOST_MODE", "TRIGGER_SHELL_BUGREPORT", "TRIGGER_SHELL_PROFCOLLECT_UPLOAD", "TRUST_LISTENER", "TUNER_RESOURCE_ACCESS", "TURN_SCREEN_ON", "TV_INPUT_HARDWARE", "TV_VIRTUAL_REMOTE_CONTROLLER", "UNLIMITED_SHORTCUTS_API_CALLS", "UNLIMITED_TOASTS", "UPDATE_CONFIG", "UPDATE_DEVICE_MANAGEMENT_RESOURCES", "UPDATE_DEVICE_STATS", "UPDATE_DOMAIN_VERIFICATION_USER_SELECTION", "UPDATE_FONTS", "UPDATE_APP_OPS_STATS", "UPDATE_LOCK", "UPDATE_LOCK_TASK_PACKAGES", "UPDATE_PACKAGES_WITHOUT_USER_ACTION", "UPDATE_TIME_ZONE_RULES", "UPGRADE_RUNTIME_PERMISSIONS", "USER_ACTIVITY", "USE_ATTESTATION_VERIFICATION_SERVICE", "USE_BIOMETRIC_INTERNAL", "USE_COLORIZED_NOTIFICATIONS", "USE_CREDENTIALS", "USE_CUSTOM_VIRTUAL_MACHINE", "USE_DATA_IN_BACKGROUND", "USE_EXACT_ALARM", "USE_FULL_SCREEN_INTENT", "USE_ICC_AUTH_WITH_DEVICE_IDENTIFIER", "USE_RESERVED_DISK", "UWB_PRIVILEGED", "VERIFY_ATTESTATION", "VIBRATE", "VIBRATE_ALWAYS_ON", "VIEW_INSTANT_APPS", "VIRTUAL_INPUT_DEVICE", "WAKEUP_SURFACE_FLINGER", "WAKE_LOCK", "WATCH_APPOPS", "WHITELIST_AUTO_REVOKE_PERMISSIONS", "WHITELIST_RESTRICTED_PERMISSIONS", "WIFI_ACCESS_COEX_UNSAFE_CHANNELS", "WIFI_SET_DEVICE_MOBILITY_STATE", "WIFI_UPDATE_COEX_UNSAFE_CHANNELS", "WIFI_UPDATE_USABILITY_STATS_SCORE", "WRITE_ALLOWLISTED_DEVICE_CONFIG", "WRITE_APN_SETTINGS", "WRITE_BLOCKED_NUMBERS", "WRITE_DEVICE_CONFIG", "WRITE_DREAM_STATE", "WRITE_EMBEDDED_SUBSCRIPTIONS", "WRITE_GSERVICES", "WRITE_MEDIA_STORAGE", "WRITE_OBB", "WRITE_PROFILE", "WRITE_SECURE_SETTINGS", "WRITE_SECURITY_LOG", "WRITE_SETTINGS", "WRITE_SETTINGS_HOMEPAGE_DATA", "WRITE_SMS", "WRITE_SOCIAL_STREAM", "WRITE_SYNC_SETTINGS", "WRITE_USER_DICTIONARY", "health.READ_EXERCISE_ROUTE" });
            comboBoxPermission.Location = new System.Drawing.Point(154, 47);
            comboBoxPermission.Name = "comboBoxPermission";
            comboBoxPermission.Size = new System.Drawing.Size(194, 23);
            comboBoxPermission.TabIndex = 3;
            // 
            // buttonRun
            // 
            buttonRun.Location = new System.Drawing.Point(305, 97);
            buttonRun.Name = "buttonRun";
            buttonRun.Size = new System.Drawing.Size(75, 23);
            buttonRun.TabIndex = 4;
            buttonRun.Text = "Run";
            buttonRun.UseVisualStyleBackColor = true;
            buttonRun.Click += ButtonRun_Click;
            // 
            // ShellCommandsForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            ClientSize = new System.Drawing.Size(392, 132);
            Controls.Add(buttonRun);
            Controls.Add(comboBoxPermission);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(comboBoxMethod);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ShellCommandsForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Shell Permission";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxMethod;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxPermission;
        private System.Windows.Forms.Button buttonRun;
    }
}