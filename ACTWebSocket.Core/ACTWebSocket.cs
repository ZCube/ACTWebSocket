﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Advanced_Combat_Tracker;
using System.Xml;

#pragma warning disable 0168 // variable declared but not used. 

namespace ACTWebSocket_Plugin
{
    using static ACTWebSocketCore;
    using ACTWebSocket.Core;
    using Classes;
    using Newtonsoft.Json.Linq;
    using Open.Nat;
    using System.Globalization;
    using System.IO;
    using System.Threading.Tasks;
    using System.Threading;
    using System.Net;
    using System.Diagnostics;
    using System.ComponentModel;
    using Ionic.Zip;
    using System.Text.RegularExpressions;
    using Tmds.MDns;
    using System.Security.AccessControl;

    public interface PluginDirectory
    {
        void SetPluginDirectory(string path);
        string GetPluginDirectory();
    }
    public class ACTWebSocketMain : UserControl, IActPluginV1, PluginDirectory
    {
        public static string overlayCaption = "OverlayProcWMCOPYDATA";
        string overlaySkinDirectory { get; set; }
        string overlayScreenShotDirectory { get; set; }
        string pluginDirectory = "";
        private ComboBox hostnames;
        private Label label2;
        private TextBox uPnPPort;
        private Button buttonAddURL;
        private Button buttonURL;
        private ListView WebSkinListView;
        private Button buttonOpen;
        private CheckBox skinOnAct;
        private GroupBox groupBox2;
        private CheckBox autostartoverlay;
        private GroupBox groupBox1;
        private GroupBox groupBox3;
        private ListView FileSkinListView;
        private Button buttonOpenOverlayProcManager;
        private ColumnHeader Title2;
        private ColumnHeader Title;
        private Button buttonStartStopOverlayProc;
        private Button buttonOverlay;
        private ProgressBar progressBar;
        private Button buttonDownload;
        private GroupBox groupBox4;
        private Button buttonCopyCode;
        private Label label1;
        private TextBox hashCode;
        private CheckBox chatFilter;

        public void SetSkinDir(string path)
        {
            overlaySkinDirectory = path;
        }

        public void SetScreenShotDir(string path)
        {
            overlayScreenShotDirectory = path;
        }

        public string GetSkinDir()
        {
            return overlaySkinDirectory;
        }

        public void SetPluginDirectory(string path)
        {
            overlayProcDir = path + "\\overlay_proc";
            overlayProcExe = overlayProcDir + "\\overlay_proc.exe";
            UpdateOverlayProc();
            pluginDirectory = path;
        }

        public string GetPluginDirectory()
        {
            return pluginDirectory;
        }
        private ACTWebSocketCore core;
        private TextBox port;
        private CheckBox autostart;
        private CheckBox MiniParseUse;
        private CheckBox BeforeLogLineReadUse;
        private CheckBox UPNPUse;
        private CheckBox OnLogLineReadUse;
        private Button buttonOff;
        private Button buttonOn;
        private CheckBox randomURL;
        private GroupBox startoption;
        private GroupBox hostdata;
        private Label label15;
        private Label label14;
        private GroupBox othersets;
        private GroupBox serverStatus;
        private Button copyURL;
        #region Designer Created Code (Avoid editing)
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ACTWebSocketMain));
            this.port = new System.Windows.Forms.TextBox();
            this.autostart = new System.Windows.Forms.CheckBox();
            this.MiniParseUse = new System.Windows.Forms.CheckBox();
            this.BeforeLogLineReadUse = new System.Windows.Forms.CheckBox();
            this.OnLogLineReadUse = new System.Windows.Forms.CheckBox();
            this.buttonOff = new System.Windows.Forms.Button();
            this.buttonOn = new System.Windows.Forms.Button();
            this.UPNPUse = new System.Windows.Forms.CheckBox();
            this.randomURL = new System.Windows.Forms.CheckBox();
            this.startoption = new System.Windows.Forms.GroupBox();
            this.skinOnAct = new System.Windows.Forms.CheckBox();
            this.hostdata = new System.Windows.Forms.GroupBox();
            this.hostnames = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.uPnPPort = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.othersets = new System.Windows.Forms.GroupBox();
            this.chatFilter = new System.Windows.Forms.CheckBox();
            this.serverStatus = new System.Windows.Forms.GroupBox();
            this.copyURL = new System.Windows.Forms.Button();
            this.buttonAddURL = new System.Windows.Forms.Button();
            this.buttonURL = new System.Windows.Forms.Button();
            this.WebSkinListView = new System.Windows.Forms.ListView();
            this.Title2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonOpen = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonOverlay = new System.Windows.Forms.Button();
            this.autostartoverlay = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.buttonStartStopOverlayProc = new System.Windows.Forms.Button();
            this.buttonOpenOverlayProcManager = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.FileSkinListView = new System.Windows.Forms.ListView();
            this.Title = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.buttonCopyCode = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.hashCode = new System.Windows.Forms.TextBox();
            this.startoption.SuspendLayout();
            this.hostdata.SuspendLayout();
            this.othersets.SuspendLayout();
            this.serverStatus.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // port
            // 
            resources.ApplyResources(this.port, "port");
            this.port.Name = "port";
            this.port.TextChanged += new System.EventHandler(this.port_TextChanged);
            this.port.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.port_KeyPress);
            // 
            // autostart
            // 
            resources.ApplyResources(this.autostart, "autostart");
            this.autostart.BackColor = System.Drawing.Color.Transparent;
            this.autostart.Name = "autostart";
            this.autostart.UseVisualStyleBackColor = false;
            // 
            // MiniParseUse
            // 
            resources.ApplyResources(this.MiniParseUse, "MiniParseUse");
            this.MiniParseUse.BackColor = System.Drawing.Color.Transparent;
            this.MiniParseUse.Checked = true;
            this.MiniParseUse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MiniParseUse.Name = "MiniParseUse";
            this.MiniParseUse.UseVisualStyleBackColor = false;
            this.MiniParseUse.CheckedChanged += new System.EventHandler(this.MiniParseUse_CheckedChanged);
            // 
            // BeforeLogLineReadUse
            // 
            resources.ApplyResources(this.BeforeLogLineReadUse, "BeforeLogLineReadUse");
            this.BeforeLogLineReadUse.BackColor = System.Drawing.Color.Transparent;
            this.BeforeLogLineReadUse.Name = "BeforeLogLineReadUse";
            this.BeforeLogLineReadUse.UseVisualStyleBackColor = false;
            this.BeforeLogLineReadUse.CheckedChanged += new System.EventHandler(this.BeforeLogLineReadUse_CheckedChanged);
            // 
            // OnLogLineReadUse
            // 
            resources.ApplyResources(this.OnLogLineReadUse, "OnLogLineReadUse");
            this.OnLogLineReadUse.BackColor = System.Drawing.Color.Transparent;
            this.OnLogLineReadUse.Name = "OnLogLineReadUse";
            this.OnLogLineReadUse.UseVisualStyleBackColor = false;
            this.OnLogLineReadUse.CheckedChanged += new System.EventHandler(this.OnLogLineReadUse_CheckedChanged);
            // 
            // buttonOff
            // 
            resources.ApplyResources(this.buttonOff, "buttonOff");
            this.buttonOff.Name = "buttonOff";
            this.buttonOff.UseVisualStyleBackColor = true;
            this.buttonOff.Click += new System.EventHandler(this.buttonOff_Click);
            // 
            // buttonOn
            // 
            resources.ApplyResources(this.buttonOn, "buttonOn");
            this.buttonOn.Name = "buttonOn";
            this.buttonOn.UseVisualStyleBackColor = true;
            this.buttonOn.Click += new System.EventHandler(this.buttonOn_Click);
            // 
            // UPNPUse
            // 
            resources.ApplyResources(this.UPNPUse, "UPNPUse");
            this.UPNPUse.BackColor = System.Drawing.Color.Transparent;
            this.UPNPUse.Checked = true;
            this.UPNPUse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UPNPUse.Name = "UPNPUse";
            this.UPNPUse.UseVisualStyleBackColor = false;
            this.UPNPUse.CheckedChanged += new System.EventHandler(this.UPNPUse_CheckedChanged);
            // 
            // randomURL
            // 
            resources.ApplyResources(this.randomURL, "randomURL");
            this.randomURL.BackColor = System.Drawing.Color.Transparent;
            this.randomURL.Name = "randomURL";
            this.randomURL.UseVisualStyleBackColor = false;
            this.randomURL.CheckedChanged += new System.EventHandler(this.randomURL_CheckedChanged);
            // 
            // startoption
            // 
            this.startoption.BackColor = System.Drawing.Color.Transparent;
            this.startoption.Controls.Add(this.skinOnAct);
            this.startoption.Controls.Add(this.UPNPUse);
            this.startoption.Controls.Add(this.randomURL);
            this.startoption.Controls.Add(this.autostart);
            resources.ApplyResources(this.startoption, "startoption");
            this.startoption.Name = "startoption";
            this.startoption.TabStop = false;
            // 
            // skinOnAct
            // 
            resources.ApplyResources(this.skinOnAct, "skinOnAct");
            this.skinOnAct.BackColor = System.Drawing.Color.Transparent;
            this.skinOnAct.Name = "skinOnAct";
            this.skinOnAct.UseVisualStyleBackColor = false;
            this.skinOnAct.CheckedChanged += new System.EventHandler(this.skinOnAct_CheckedChanged);
            // 
            // hostdata
            // 
            this.hostdata.BackColor = System.Drawing.Color.Transparent;
            this.hostdata.Controls.Add(this.hostnames);
            this.hostdata.Controls.Add(this.label2);
            this.hostdata.Controls.Add(this.uPnPPort);
            this.hostdata.Controls.Add(this.label15);
            this.hostdata.Controls.Add(this.label14);
            this.hostdata.Controls.Add(this.port);
            resources.ApplyResources(this.hostdata, "hostdata");
            this.hostdata.Name = "hostdata";
            this.hostdata.TabStop = false;
            // 
            // hostnames
            // 
            this.hostnames.FormattingEnabled = true;
            resources.ApplyResources(this.hostnames, "hostnames");
            this.hostnames.Name = "hostnames";
            this.hostnames.TextChanged += new System.EventHandler(this.hostnames_TextChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Name = "label2";
            // 
            // uPnPPort
            // 
            resources.ApplyResources(this.uPnPPort, "uPnPPort");
            this.uPnPPort.Name = "uPnPPort";
            this.uPnPPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.port_KeyPress);
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Name = "label15";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Name = "label14";
            // 
            // othersets
            // 
            this.othersets.BackColor = System.Drawing.Color.Transparent;
            this.othersets.Controls.Add(this.chatFilter);
            this.othersets.Controls.Add(this.BeforeLogLineReadUse);
            this.othersets.Controls.Add(this.OnLogLineReadUse);
            this.othersets.Controls.Add(this.MiniParseUse);
            resources.ApplyResources(this.othersets, "othersets");
            this.othersets.Name = "othersets";
            this.othersets.TabStop = false;
            // 
            // chatFilter
            // 
            resources.ApplyResources(this.chatFilter, "chatFilter");
            this.chatFilter.BackColor = System.Drawing.Color.Transparent;
            this.chatFilter.Name = "chatFilter";
            this.chatFilter.UseVisualStyleBackColor = false;
            this.chatFilter.CheckedChanged += new System.EventHandler(this.chatFilter_CheckedChanged);
            // 
            // serverStatus
            // 
            this.serverStatus.Controls.Add(this.buttonOn);
            this.serverStatus.Controls.Add(this.buttonOff);
            resources.ApplyResources(this.serverStatus, "serverStatus");
            this.serverStatus.Name = "serverStatus";
            this.serverStatus.TabStop = false;
            // 
            // copyURL
            // 
            resources.ApplyResources(this.copyURL, "copyURL");
            this.copyURL.Name = "copyURL";
            this.copyURL.UseVisualStyleBackColor = true;
            this.copyURL.Click += new System.EventHandler(this.copyURL_Click);
            // 
            // buttonAddURL
            // 
            resources.ApplyResources(this.buttonAddURL, "buttonAddURL");
            this.buttonAddURL.Name = "buttonAddURL";
            this.buttonAddURL.UseVisualStyleBackColor = true;
            this.buttonAddURL.Click += new System.EventHandler(this.buttonAddURL_Click);
            // 
            // buttonURL
            // 
            resources.ApplyResources(this.buttonURL, "buttonURL");
            this.buttonURL.Name = "buttonURL";
            this.buttonURL.UseVisualStyleBackColor = true;
            this.buttonURL.Click += new System.EventHandler(this.buttonURL_Click);
            // 
            // WebSkinListView
            // 
            resources.ApplyResources(this.WebSkinListView, "WebSkinListView");
            this.WebSkinListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Title2});
            this.WebSkinListView.FullRowSelect = true;
            this.WebSkinListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.WebSkinListView.HideSelection = false;
            this.WebSkinListView.MultiSelect = false;
            this.WebSkinListView.Name = "WebSkinListView";
            this.WebSkinListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.WebSkinListView.UseCompatibleStateImageBehavior = false;
            this.WebSkinListView.View = System.Windows.Forms.View.Details;
            this.WebSkinListView.SelectedIndexChanged += new System.EventHandler(this.skinList_SelectedIndexChanged);
            // 
            // Title2
            // 
            resources.ApplyResources(this.Title2, "Title2");
            // 
            // buttonOpen
            // 
            resources.ApplyResources(this.buttonOpen, "buttonOpen");
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonOverlay);
            this.groupBox2.Controls.Add(this.buttonOpen);
            this.groupBox2.Controls.Add(this.buttonURL);
            this.groupBox2.Controls.Add(this.WebSkinListView);
            this.groupBox2.Controls.Add(this.copyURL);
            this.groupBox2.Controls.Add(this.buttonAddURL);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // buttonOverlay
            // 
            resources.ApplyResources(this.buttonOverlay, "buttonOverlay");
            this.buttonOverlay.Name = "buttonOverlay";
            this.buttonOverlay.UseVisualStyleBackColor = true;
            this.buttonOverlay.Click += new System.EventHandler(this.buttonOverlay_Click);
            // 
            // autostartoverlay
            // 
            resources.ApplyResources(this.autostartoverlay, "autostartoverlay");
            this.autostartoverlay.BackColor = System.Drawing.Color.Transparent;
            this.autostartoverlay.Name = "autostartoverlay";
            this.autostartoverlay.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonDownload);
            this.groupBox1.Controls.Add(this.buttonStartStopOverlayProc);
            this.groupBox1.Controls.Add(this.buttonOpenOverlayProcManager);
            this.groupBox1.Controls.Add(this.autostartoverlay);
            this.groupBox1.Controls.Add(this.progressBar);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // buttonDownload
            // 
            resources.ApplyResources(this.buttonDownload, "buttonDownload");
            this.buttonDownload.Name = "buttonDownload";
            this.buttonDownload.UseVisualStyleBackColor = true;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);
            // 
            // buttonStartStopOverlayProc
            // 
            resources.ApplyResources(this.buttonStartStopOverlayProc, "buttonStartStopOverlayProc");
            this.buttonStartStopOverlayProc.Name = "buttonStartStopOverlayProc";
            this.buttonStartStopOverlayProc.UseVisualStyleBackColor = true;
            this.buttonStartStopOverlayProc.Click += new System.EventHandler(this.buttonStartStopOverlayProc_Click);
            // 
            // buttonOpenOverlayProcManager
            // 
            resources.ApplyResources(this.buttonOpenOverlayProcManager, "buttonOpenOverlayProcManager");
            this.buttonOpenOverlayProcManager.Name = "buttonOpenOverlayProcManager";
            this.buttonOpenOverlayProcManager.UseVisualStyleBackColor = true;
            this.buttonOpenOverlayProcManager.Click += new System.EventHandler(this.buttonOpenOverlayProcManager_Click);
            // 
            // progressBar
            // 
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.Name = "progressBar";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.FileSkinListView);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // FileSkinListView
            // 
            resources.ApplyResources(this.FileSkinListView, "FileSkinListView");
            this.FileSkinListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Title});
            this.FileSkinListView.FullRowSelect = true;
            this.FileSkinListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.FileSkinListView.HideSelection = false;
            this.FileSkinListView.MultiSelect = false;
            this.FileSkinListView.Name = "FileSkinListView";
            this.FileSkinListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.FileSkinListView.UseCompatibleStateImageBehavior = false;
            this.FileSkinListView.View = System.Windows.Forms.View.Details;
            this.FileSkinListView.SelectedIndexChanged += new System.EventHandler(this.FileSkinListView_SelectedIndexChanged);
            // 
            // Title
            // 
            resources.ApplyResources(this.Title, "Title");
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.buttonCopyCode);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.hashCode);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // buttonCopyCode
            // 
            resources.ApplyResources(this.buttonCopyCode, "buttonCopyCode");
            this.buttonCopyCode.Name = "buttonCopyCode";
            this.buttonCopyCode.UseVisualStyleBackColor = true;
            this.buttonCopyCode.Click += new System.EventHandler(this.buttonCopyCode_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // hashCode
            // 
            resources.ApplyResources(this.hashCode, "hashCode");
            this.hashCode.Name = "hashCode";
            // 
            // ACTWebSocketMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.serverStatus);
            this.Controls.Add(this.othersets);
            this.Controls.Add(this.hostdata);
            this.Controls.Add(this.startoption);
            resources.ApplyResources(this, "$this");
            this.Name = "ACTWebSocketMain";
            this.Load += new System.EventHandler(this.ACTWebSocket_Load);
            this.startoption.ResumeLayout(false);
            this.hostdata.ResumeLayout(false);
            this.hostdata.PerformLayout();
            this.othersets.ResumeLayout(false);
            this.serverStatus.ResumeLayout(false);
            this.serverStatus.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        #endregion

        System.Timers.Timer overlayProcCheckTimer = null;
        public ACTWebSocketMain()
        {
            ChatFilter = false;
            InitializeComponent();
            if (ipc == null)
            {
                ipc = new IPC_COPYDATA();
                ipc.Show();
                ipc.Text = "Client" + overlayCaption;
                ipc.onMessage = (code, message) =>
                {
                    if (message == ".")
                        return;
                    try
                    {
                        JObject obj = JObject.Parse(message);
                        JToken token;
                        if (obj.TryGetValue("cmd", out token))
                        {
                            UpdateOverlayProc();
                            String cmd = token.ToObject<String>();
                            switch (cmd)
                            {
                                case "capture":
                                    {
                                        JToken value = obj["value"];
                                        String id = value["id"].ToObject<String>();
                                        String pngBase64 = value["capture"].ToObject<String>();
                                        pngBase64 = pngBase64;
                                        
                                        byte[] data = Convert.FromBase64String(pngBase64);

                                        string dir = overlayScreenShotDirectory;
                                        try
                                        {
                                            if (!Directory.Exists(dir))
                                            {
                                                Directory.CreateDirectory(dir);
                                            }
                                            string filename;
                                            int i = 0;

                                            do
                                            {
                                                filename = "ScreenShot_" + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + i.ToString() + ".png";
                                                ++i;
                                            }
                                            while (File.Exists(dir + "/" + filename));

                                            System.IO.FileStream _FileStream =
                                               new System.IO.FileStream(dir + "/" + filename, System.IO.FileMode.Create,
                                                                        System.IO.FileAccess.Write);
                                            _FileStream.Write(data, 0, data.Length);
                                            _FileStream.Close();
                                        }
                                        catch(Exception e)
                                        {
                                            
                                        }
                                    }
                                    break;
                                case "get_urllist":
                                    ServerUrlChanged();
                                    break;
                                case "overlay_proc_status_changed":
                                    UpdateOverlayProc();
                                    break;
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        // TODO : What?
                    }
                };
            }
            overlayProcCheckTimer = new System.Timers.Timer();
            overlayProcCheckTimer.Interval = 2000;
            overlayProcCheckTimer.Elapsed += (o, e) =>
            {
                UpdateOverlayProc();
            };
            overlayProcCheckTimer.Start();
        }

        ~ACTWebSocketMain()
        {
            SaveSettings();
        }

        Label lblStatus;    // The status label that appears in ACT's Plugin tab
        string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\ACTWebSocket.config.json");
        string overlaySettingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\ACTWebSocket.overlay.json");

        #region IActPluginV1 Members

        static List<KeyValuePair<string, MiniParseSortType>> sortTypeDict = new List<KeyValuePair<string, MiniParseSortType>>()
        {
            new KeyValuePair<string, MiniParseSortType>("DoNotSort", MiniParseSortType.None),
            new KeyValuePair<string, MiniParseSortType>("SortStringAscending", MiniParseSortType.StringAscending),
            new KeyValuePair<string, MiniParseSortType>("SortStringDescending", MiniParseSortType.StringDescending),
            new KeyValuePair<string, MiniParseSortType>("SortNumberAscending", MiniParseSortType.NumericAscending),
            new KeyValuePair<string, MiniParseSortType>("SortNumberDescending", MiniParseSortType.NumericDescending)
        };

        JObject overlayWindows = new JObject(); // 설정 전부

        public static bool SendMessage(string caption, JObject obj)
        {
            if (ipc == null)
                return false;
            return ipc.SendMessage(caption, 0, obj.ToString());
        }

        public static bool SendMessage(JObject obj)
        {
            if (ipc == null)
                return false;
            return ipc.SendMessage(overlayCaption, 0, obj.ToString());
        }

        public static IPC_COPYDATA ipc = null;
        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            UpdateOverlayProc();
            if (core == null)
            {
                core = new ACTWebSocketCore(this);
                core.pluginDirectory = pluginDirectory;
                core.overlaySkinDirectory = overlaySkinDirectory;
                core.hwnd = Handle;
            }
            lblStatus = pluginStatusText;   // Hand the status label's reference to our local var
            pluginScreenSpace.Controls.Add(this);   // Add this UserControl to the tab ACT provides
            Dock = DockStyle.Fill; // Expand the UserControl to fill the tab's client space
            LoadSettings();
            BeforeLogLineReadUse.Checked = BeforeLogLineRead;
            OnLogLineReadUse.Checked = OnLogLineRead;
            MiniParseUse.Checked = MiniParse;
            //SSLUse.Checked = UseSSL;
            UPNPUse.Checked = UseUPnP;
            randomURL.Checked = RandomURL;
            skinOnAct.Checked = SkinOnAct;
            chatFilter.Checked = ChatFilter;
            autostart.Checked = AutoRun;
            autostartoverlay.Checked = AutoOverlay;
            progressBar.Hide();
            progressBar.BringToFront();
            StopServer();

            if (core != null)
            {
                core.Filters["/BeforeLogLineRead"] = BeforeLogLineReadUse.Checked;
                core.Filters["/OnLogLineRead"] = OnLogLineReadUse.Checked;
                core.Filters["/MiniParse"] = MiniParseUse.Checked;
                // not configurable ?
                core.Config.SortKey = "encdps";
                core.Config.SortType = MiniParseSortType.NumericDescending;
            }

            try
            {
                if (autostart.Checked)
                {
                    StartServer();
                }
                else
                {
                    StopServer();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                StopServer();
            }

            try
            {
                if (autostartoverlay.Checked)
                {
                    StartOverlayProc();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                StopServer();
            }

            // Create some sort of parsing event handler.  After the "+=" hit TAB twice and the code will be generated for you.
            ActGlobals.oFormActMain.BeforeLogLineRead += oFormActMain_BeforeLogLineRead;
            ActGlobals.oFormActMain.OnLogLineRead += oFormActMain_OnLogLineRead;
            var s = ActGlobals.oFormActMain.ActPlugins;
            lblStatus.Text = "Plugin Started";
        }

        public void DeInitPlugin()
        {
            //StopServer();
            // Unsubscribe from any events you listen to when exiting!
            ActGlobals.oFormActMain.BeforeLogLineRead -= oFormActMain_BeforeLogLineRead;
            ActGlobals.oFormActMain.OnLogLineRead -= oFormActMain_OnLogLineRead;

            SaveSettings();

            SendMessage(JObject.FromObject(new
            {
                cmd = "stop"
            }));
            lblStatus.Text = "Plugin Exited";
        }

        #endregion

        void oFormActMain_AfterCombatAction(bool isImport, CombatActionEventArgs actionInfo)
        {
        }

        void LoadSettings()
        {
            if (File.Exists(settingsFile))
            {
                JObject obj = new JObject();
                try {
                    String s = File.ReadAllText(settingsFile);
                    obj = JObject.Parse(s);
                    JToken token;
                    if (obj.TryGetValue("Port", out token))
                    {
                        Port = token.ToObject<int>();
                    }
                    else
                    {
                        Port = 10501;
                    }
                    if (obj.TryGetValue("UPnPPort", out token))
                    {
                        UPnPPort = token.ToObject<int>();
                    }
                    else
                    {
                        UPnPPort = Port;
                    }
                    if (obj.TryGetValue("Hostname", out token))
                    {
                        Hostname = token.ToObject<String>();
                    }
                    else
                    {
                        Hostname = "localhost";
                    }
                    if (obj.TryGetValue("RandomURL", out token))
                    {
                        RandomURL = token.ToObject<bool>();
                    }
                    else
                    {
                        RandomURL = false;
                    }
                    if (obj.TryGetValue("UseUPnP", out token))
                    {
                        UseUPnP = token.ToObject<bool>();
                    }
                    else
                    {
                        UseUPnP = false;
                    }
                    //if (obj.TryGetValue("UseSSL", out token))
                    //{
                    //    UseSSL = token.ToObject<bool>();
                    //}
                    //else
                    //{
                    //    UseSSL = false;
                    //}
                    if (obj.TryGetValue("SkinOnAct", out token))
                    {
                        SkinOnAct = token.ToObject<bool>();
                    }
                    else
                    {
                        SkinOnAct = true;
                    }
                    if (obj.TryGetValue("AutoRun", out token))
                    {
                        AutoRun = token.ToObject<bool>();
                    }
                    else
                    {
                        AutoRun = false;
                    }
                    if (obj.TryGetValue("AutoOverlay", out token))
                    {
                        AutoOverlay = token.ToObject<bool>();
                    }
                    else
                    {
                        AutoOverlay = false;
                    }
                    if (obj.TryGetValue("BeforeLogLineRead", out token))
                    {
                        BeforeLogLineRead = token.ToObject<bool>();
                    }
                    else
                    {
                        BeforeLogLineRead = false;
                    }
                    if (obj.TryGetValue("ChatFilter", out token))
                    {
                        ChatFilter = token.ToObject<bool>();
                    }
                    else
                    {
                        ChatFilter = false;
                    }
                    if (obj.TryGetValue("OnLogLineRead", out token))
                    {
                        OnLogLineRead = token.ToObject<bool>();
                    }
                    else
                    {
                        OnLogLineRead = false;
                    }
                    if (obj.TryGetValue("MiniParse", out token))
                    {
                        MiniParse = token.ToObject<bool>();
                    }
                    else
                    {
                        MiniParse = true;
                    }
                    if(obj.TryGetValue("SkinURLList", out token))
                    {
                        SkinURLList.Clear();
                        foreach (var a in token.Values<string>())
                        {
                            SkinURLList.Add(a);
                        }
                        buttonRefresh_Click(null, null);
                    }
                }
                catch (Exception e)
                {
                }
                hostnames.Text = Hostname;
            }
        }


        void SaveSettings()
        {
            UpdateFormSettings();
            JObject obj = new JObject();
            obj.Add("Port", Port);
            obj.Add("UPnPPort", UPnPPort);
            obj.Add("Hostname", Hostname);
            obj.Add("RandomURL", RandomURL);
            obj.Add("UseUPnP", UseUPnP);
            //obj.Add("UseSSL", UseSSL);
            obj.Add("AutoRun", AutoRun);
            obj.Add("AutoOverlay", AutoOverlay);
            obj.Add("BeforeLogLineRead", BeforeLogLineRead);
            obj.Add("OnLogLineRead", OnLogLineRead);
            obj.Add("MiniParse", MiniParse);
            obj.Add("ChatFilter", ChatFilter);
            obj.Add("SkinOnAct", SkinOnAct);
            JArray skins = new JArray();
            foreach (string a in SkinURLList)
            {
                skins.Add(a);
            }
            obj.Add("SkinURLList", skins);
            String s = obj.ToString();
            File.WriteAllText(settingsFile, s);
        }

        private List<String> addrs = new List<String>();
        private void ACTWebSocket_Load(object sender, EventArgs e)
        {
            String strHostName = string.Empty;
            strHostName = Dns.GetHostName();
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);

            addrs.Clear();
            addrs.Add("127.0.0.1");
            {
                IPAddress[] addr = ipEntry.AddressList;
                for (int i = 0; i < addr.Length; i++)
                {
                    if (addr[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        addrs.Add(addr[i].ToString());
                }
            }
            addrs = Utility.Distinct<String>(addrs);
            addrs.Sort();
            core.SetAddress(addrs);

            Task task = Task.Factory.StartNew(() =>
            {
                String ipaddress = Utility.GetExternalIp();
                if (ipaddress.Length > 0)
                    addrs.Add(ipaddress);

                addrs = Utility.Distinct<String>(addrs);
                addrs.Sort();
                core.SetAddress(addrs);

            });
            Task UITask = task.ContinueWith((t) =>
            {
                foreach (var addr in addrs)
                {
                    hostnames.Items.Add(addr);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private bool IsChattings(LogLineEventArgs e)
        {
            string[] data = e.logLine.Split('|');

            try
            {
                if (Convert.ToInt32(data[0]) == 0)
                {
                    if (Convert.ToInt32(data[2], 16) < 54)
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        private void oFormActMain_BeforeLogLineRead(
            bool isImport,
            LogLineEventArgs logInfo)
        {
            if (BeforeLogLineRead)
            {
                bool isChatting = IsChattings(logInfo);
                if(!isChatting || (isChatting && ChatFilter))
                {
                    core.Broadcast("/BeforeLogLineRead", "Chat", logInfo.logLine);
                }
            }
        }

        private void oFormActMain_OnLogLineRead(
            bool isImport,
            LogLineEventArgs logInfo)
        {
            if (OnLogLineRead)
            {
                bool isChatting = IsChattings(logInfo);
                if (!isChatting || (isChatting && ChatFilter))
                {
                    core.Broadcast("/OnLogLineRead", "Chat", logInfo.logLine);
                }
            }
        }

        #region Web JSObject Part
        public int Port { get; set; }
        public int UPnPPort { get; set; }
        public string Hostname { get; set; }
        public bool SkinOnAct { get; set; }
        public bool RandomURL
        {
            get
            {
                return core == null || ACTWebSocketCore.randomDir != null;
            }
            set
            {
                if (value)
                {
                    ACTWebSocketCore.randomDir = Guid.NewGuid().ToString();
                }
                else
                {
                    ACTWebSocketCore.randomDir = null;
                }
            }
        }

        //public bool UseSSL { get; set; }
        public bool UseUPnP { get; set; }
        public bool AutoRun { get; set; }
        public bool AutoOverlay { get; set; }
        public bool ChatFilter { get; set; }
        public List<String> SkinURLList = new List<String>();

        public void StartServer()
        {
            core.Filters["/BeforeLogLineRead"] = true;
            core.Filters["/OnLogLineRead"] = true;
            core.Filters["/MiniParse"] = true;

            if (UseUPnP)
            {
                Task upnpTask = new Task(async () =>
                {
                    try
                    {
                        var discoverer = new NatDiscoverer();
                        var cts = new CancellationTokenSource(10000); // 10secs
                        var device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);

                        // not registered when first invoke...
                        await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, Port, UPnPPort, "ACTWebSocket Port"));
                        await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, Port, UPnPPort, "ACTWebSocket Port"));
                        await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, Port, UPnPPort, "ACTWebSocket Port"));
                    }
                    catch(Exception e)
                    {

                    }
                });
                upnpTask.Start();
            }
            
            var addresses = Dns.GetHostAddresses(Hostname);

            bool localhostOnly = false;
            for(int i=0;i< addresses.Length;++i)
            {
                var ip = addresses[i];
                if (IPAddress.IsLoopback(ip))
                {
                    localhostOnly = true;
                    break;
                }
            }
            
            if (RandomURL)
            {
                ACTWebSocketCore.randomDir = Guid.NewGuid().ToString();
            }
            else
            {
                ACTWebSocketCore.randomDir = null;
            }
            try
            {
                if (UseUPnP)
                {
                    core.StartServer(localhostOnly ? "127.0.0.1" : "0.0.0.0", Port, UPnPPort, Hostname, SkinOnAct, false);
                }
                else
                {
                    core.StartServer(localhostOnly ? "127.0.0.1" : "0.0.0.0", Port, Port, Hostname, SkinOnAct, false);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                StopServer();
            }
            //BeforeLogLineReadUse.Enabled = false;
            //OnLogLineReadUse.Enabled = false;
            //MiniParseUse.Enabled = false;
            //chatFilter.Enabled = false;
            //SSLUse.Enabled = false;
            skinOnAct.Enabled = false;
            UPNPUse.Enabled = false;
            randomURL.Enabled = false;
            buttonOn.Enabled = false;
            port.Enabled = false;
            uPnPPort.Enabled = false;
            hostnames.Enabled = false;
            buttonOff.Enabled = true;


            Task serviceTask = new Task(() =>
            {
                try
                {
                    string serviceType = "_overlay._tcp";
                    lock (serviceLock)
                    {
                        if (serviceBrowser != null)
                        {
                            serviceBrowser.StopBrowse();
                            serviceBrowser = null;
                        }

                        {
                            serviceBrowser = new ServiceBrowser();
                            serviceBrowser.ServiceAdded += onServiceAdded;
                            serviceBrowser.ServiceRemoved += onServiceRemoved;
                            serviceBrowser.ServiceChanged += onServiceChanged;

                            Console.WriteLine("Browsing for type: {0}", serviceType);
                            serviceBrowser.StartBrowse(serviceType);
                            Console.ReadLine();
                        }
                    }


                    String url = ("http") + "://" + Hostname + ":" + Port + "/";
                    if (ACTWebSocketCore.randomDir != null)
                    {
                        url += ACTWebSocketCore.randomDir + "/";
                    }
                    string address = string.Format("http://zcube.kr:8585/shorten?longUrl={0}", Uri.EscapeDataString(url));

                    HttpWebRequest request = HttpWebRequest.CreateHttp(address);
                    WebResponse response = request.GetResponse();
                    String res = "";
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        res = reader.ReadToEnd();
                    }

                    res = res;
                    JObject obj = JObject.Parse(res);
                    JToken token;
                    int status_code = 0;
                    String hash = "";
                    if (obj.TryGetValue("status_code", out token))
                    {
                        status_code = token.ToObject<int>();
                    }

                    JObject data = (JObject)obj["data"];
                    if (data != null && data.TryGetValue("hash", out token))
                    {
                        hash = token.ToObject<String>();
                    }
                    hashCode.Text = hash;
                }
                catch (Exception e)
                {

                }
            });
            serviceTask.Start();
        }

        public void StopServer()
        {
            if (serviceBrowser != null)
            {
                lock (serviceLock)
                {
                    serviceBrowser.StopBrowse();
                    serviceBrowser = null;
                }
            }
            core.StopServer();
            //BeforeLogLineReadUse.Enabled = true;
            //OnLogLineReadUse.Enabled = true;
            //MiniParseUse.Enabled = true;
            //chatFilter.Enabled = true;
            //SSLUse.Enabled = true;
            skinOnAct.Enabled = true;
            UPNPUse.Enabled = true;
            randomURL.Enabled = true;
            buttonOn.Enabled = true;
            port.Enabled = true;
            uPnPPort.Enabled = false;
            hostnames.Enabled = true;
            buttonOff.Enabled = false;
        }

        public void consolelog(object s)
        {
            Console.WriteLine(s);
        }
        
        public bool BeforeLogLineRead
        {
            get {
                try
                {
                    if (core != null)
                    {
                        return core.Filters["/BeforeLogLineRead"];
                    }
                }
                catch(Exception)
                {
                }
                return false;
            }
            set
            {
                if (core != null)
                {
                    core.Filters["/BeforeLogLineRead"] = value;
                }
            }
        }

        public bool OnLogLineRead
        {
            get
            {
                try
                {
                    if (core != null)
                    {
                        return core.Filters["/OnLogLineRead"];
                    }
                }
                catch (Exception)
                {
                }
                return false;
            }
            set
            {
                if (core != null)
                {
                    core.Filters["/OnLogLineRead"] = value;
                }
            }
        }

        public bool MiniParse 
        {
            get
            {
                try
                {
                    if (core != null)
                    {
                        return core.Filters["/MiniParse"];
                    }
                }
                catch (Exception)
                {
                }
                return true;
            }
            set
            {
                if (core != null)
                {
                    core.Filters["/MiniParse"] = value;
                }
            }
        }

        public string overlayProcDir { get; private set; }
        public string overlayProcExe { get; private set; }

        public List<string> GetFileSkinList()
        {
            string dir = SkinOnAct ? overlaySkinDirectory : pluginDirectory;
            List<string> list = new List<string>();
            try
            {
                foreach (string file in Directory.EnumerateFiles(dir, "*.html", SearchOption.AllDirectories))
                {
                    list.Add(Utility.GetRelativePath(file, dir));
                }
            }
            catch(Exception e)
            {
                if(!Directory.Exists(dir))
                {
                    try
                    {
                        Directory.CreateDirectory(dir);
                    }
                    catch(Exception e2)
                    {
                    }
                }
            }
            return list;
        }

        public string getURLPath(string skinPath = "", bool withRandomURL = true)
        {
            string url = "";
            {
                url = "://" + Hostname + ":" + Port + "/";
            }
            if (withRandomURL)
            {
                if (ACTWebSocketCore.randomDir != null)
                {
                    url += ACTWebSocketCore.randomDir + "/";
                }
            }
            if (skinPath.ToLower().StartsWith("http"))
            {
                try
                {
                    Uri uri = new Uri(skinPath + "?HOST_PORT=" + "ws" + url);
                    return uri.ToString();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            else
            {
                if (skinPath.Length > 0)
                {
                    try
                    {
                        string fullURL = ("http") + url + Uri.EscapeDataString(skinPath);
                        fullURL = fullURL.Replace("%5C", "/");
                        fullURL = fullURL.Replace("%2F", "/");
                        return fullURL;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
                else
                {
                    return url;
                }
            }
            return null;
        }

        public void copyURLPath(string skinPath = "")
        {
            string url = getURLPath(skinPath);
            if(url != null)
            {
                Clipboard.SetText(url);
            }
        }
        #endregion Web JSObject Part End

        private void UpdateFormSettings()
        {
            BeforeLogLineRead = BeforeLogLineReadUse.Checked;
            OnLogLineRead = OnLogLineReadUse.Checked;
            MiniParse = MiniParseUse.Checked;
            //UseSSL = SSLUse.Checked;
            UseUPnP = UPNPUse.Checked;
            RandomURL = randomURL.Checked;
            SkinOnAct = skinOnAct.Checked;
            ChatFilter = chatFilter.Checked;
            AutoRun = autostart.Checked;
            AutoOverlay = autostartoverlay.Checked;
            Hostname = hostnames.Text;
            try
            {
                Port = Convert.ToInt32(port.Text);
            }
            catch(Exception ex)
            {
                Port = 10501;
                port.Text = Port.ToString();
            }
            try
            {
                UPnPPort = Convert.ToInt32(uPnPPort.Text);
            }
            catch (Exception ex)
            {
                UPnPPort = Port;
                uPnPPort.Text = UPnPPort.ToString();
            }
        }

        private void buttonOn_Click(object sender, EventArgs e)
        {
            UpdateFormSettings();
            StartServer();
        }

        void onServiceChanged(object sender, ServiceAnnouncementEventArgs e)
        {
            printService('~', e.Announcement);
        }

        void onServiceRemoved(object sender, ServiceAnnouncementEventArgs e)
        {
            printService('-', e.Announcement);
        }

        void onServiceAdded(object sender, ServiceAnnouncementEventArgs e)
        {
            printService('+', e.Announcement);
            Uri uri = new Uri("http://" + e.Announcement.Addresses[0].ToString() + ":" + e.Announcement.Port + "/");
            HttpWebRequest webRequest = WebRequest.CreateHttp(uri);
            webRequest.Headers.Add("Port", Port.ToString());
            webRequest.Headers.Add("IP", hostnames.Text.ToString());
            webRequest.Headers.Add("SCHEME", "http");
            webRequest.BeginGetResponse(null, webRequest);
        }

        void printService(char startChar, ServiceAnnouncement service)
        {
            Console.WriteLine("{0} '{1}' on {2}", startChar, service.Instance, service.NetworkInterface.Name);
            Console.WriteLine("\tHost: {0} ({1})", service.Hostname, string.Join(", ", service.Addresses));
            Console.WriteLine("\tPort: {0}", service.Port);
            Console.WriteLine("\tTxt : [{0}]", string.Join(", ", service.Txt));
        }

        private void buttonOff_Click(object sender, EventArgs e)
        {
            StopServer();
        }

        private void port_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        private void buttonAddURL_Click(object sender, EventArgs e)
        {
            string url = ShowDialog("Add URL", "Add URL").Trim() ;
            SkinURLList.Add(url);
            AddWebURL(url);
        }

        private void buttonURL_Click(object sender, EventArgs e)
        {
            if (WebSkinListView.SelectedItems == null) return;
            if(WebSkinListView.SelectedItems.Count > 0)
            {
                string url = (string)WebSkinListView.SelectedItems[0].Tag;
                SkinURLList.Remove(url);
                WebSkinListView.Items.RemoveAt(WebSkinListView.SelectedItems[0].Index);
                {
                    if (core != null)
                    {
                        lock (core.skinObject)
                        {
                            JArray urlConverted = new JArray();
                            JArray array = (JArray)core.skinObject["URLList"].DeepClone();
                            if (array != null)
                            {
                                foreach (JToken obj in array)
                                {
                                    if (obj["URL"].ToObject<String>() == url)
                                    {
                                        obj.Remove();
                                        break;
                                    }
                                }
                            }
                            core.skinObject["URLList"] = array;
                            SendMessage(JObject.FromObject(new
                            {
                                cmd = "urllist",
                                value = core.skinObject
                            }));
                        }
                    }
                }
            }
        }

        private void copyURL_Click(object sender, EventArgs e)
        {
            if (WebSkinListView.SelectedItems == null) return;
            if (WebSkinListView.SelectedItems.Count > 0)
            {
                string url = (string)WebSkinListView.SelectedItems[0].Tag;
                copyURLPath(url);
            }
            else if (FileSkinListView.SelectedItems.Count > 0)
            {
                string url = (string)FileSkinListView.SelectedItems[0].Tag;
                copyURLPath(url);
            }
            else
            {
                copyURLPath();
            }
        }
        
        string GetTitle(string path)
        {
            string dir = SkinOnAct ? overlaySkinDirectory : pluginDirectory;
            string title = null;
            try
            {
                if (File.Exists(path))
                {
                    string source = File.ReadAllText(path);
                    title = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
                }
                if (File.Exists(dir + "/" + path))
                {
                    string source = File.ReadAllText(dir + "/" + path);
                    title = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
                }
                else
                {
                    WebClient wc = new WebClient();
                    wc.Encoding = Encoding.UTF8;
                    string source = wc.DownloadString(path);
                    title = Regex.Match(source, @"\<title\b[^>]*\>\s*(?<Title>[\s\S]*?)\</title\>", RegexOptions.IgnoreCase).Groups["Title"].Value;
                }
                title = title.Trim();
            }
            catch (Exception ex)
            {

            }
            return title;
        }

        private void AddWebURL(string a)
        {
            string title = null;
            Task task = Task.Factory.StartNew(() =>
            {
                title = GetTitle(a);
            });
            Task UITask = task.ContinueWith((t) =>
            {
                {
                    bool find = false;
                    foreach (ListViewItem i in WebSkinListView.Items)
                    {
                        if (((string)i.Tag).CompareTo(a) == 0)
                        {
                            find = true;
                        }
                    }

                    if (!find)
                    {
                        title = (title == null || title == "") ? a : title;
                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = title;
                        lvi.Tag = a;
                        WebSkinListView.Items.Add(lvi);
                        if (core != null)
                        {
                            lock (core.skinObject)
                            {
                                JObject skinInfo = new JObject();
                                skinInfo["Title"] = title;
                                skinInfo["URL"] = a;
                                JArray array = (JArray)core.skinObject["URLList"];
                                if (array == null)
                                {
                                    array = new JArray();
                                    core.skinObject["URLList"] = array;
                                }
                                array.Add(skinInfo);
                                ServerUrlChanged();
                            }
                        }
                    }
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
        List<Task> tasklist = new List<Task>();
        private ServiceBrowser serviceBrowser;
        private Object serviceLock = new Object();

        private void AddFileURL(string a)
        {
            a = a.Replace("\\", "/");
            string title = null;
            Task task = Task.Factory.StartNew(() =>
            {
                title = GetTitle(a);
            });
            Task UITask = task.ContinueWith((t) =>
            {
                {
                    bool find = false;
                    foreach (ListViewItem i in FileSkinListView.Items)
                    {
                        if (((string)i.Tag).CompareTo(a) == 0)
                        {
                            find = true;
                            break;
                        }
                    }

                    if (!find)
                    {
                        title = (title == null || title == "") ? a : title;
                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = title;
                        lvi.Tag = a;
                        FileSkinListView.Items.Add(lvi);
                        if (core != null)
                        {
                            lock (core.skinObject)
                            {
                                JObject skinInfo = new JObject();
                                skinInfo["Title"] = title;
                                skinInfo["URL"] = a;
                                JArray array = (JArray)core.skinObject["URLList"];
                                if (array == null)
                                {
                                    array = new JArray();
                                    core.skinObject["URLList"] = array;
                                }
                                array.Add(skinInfo);
                                ServerUrlChanged();
                            }
                        }
                    }
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            Task finalTask = UITask.ContinueWith((t) =>
            {
                tasklist.Remove(UITask);
                tasklist.Remove(task);
            }, TaskScheduler.FromCurrentSynchronizationContext());
            lock(tasklist)
            {
                tasklist.Add(task);
                tasklist.Add(UITask);
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            lock (tasklist)
            {
                foreach(var task in tasklist)
                {
                    task.Wait();
                }
                tasklist.Clear();
            }

            lock (core.skinObject)
            {
                JArray array = (JArray)core.skinObject["URLList"];
                array = new JArray();
                core.skinObject["URLList"] = array;
            }
            FileSkinListView.Items.Clear();
            WebSkinListView.Items.Clear();
            foreach (var a in SkinURLList)
            {
                AddWebURL(a);
            }

            List<string> list =  GetFileSkinList();
            foreach(var a in list)
            {
                bool find = false;
                for(int i=0;i< FileSkinListView.Items.Count;++i)
                {
                    if(FileSkinListView.Items[i].Tag == a)
                    {
                        find = true;
                    }
                }
                if(!find)
                {
                    AddFileURL(a);
                }
            }
        }

        private void MiniParseUse_CheckedChanged(object sender, EventArgs e)
        {
            MiniParse = MiniParseUse.Checked;
        }

        private void OnLogLineReadUse_CheckedChanged(object sender, EventArgs e)
        {
            OnLogLineRead = OnLogLineReadUse.Checked;
        }

        private void BeforeLogLineReadUse_CheckedChanged(object sender, EventArgs e)
        {
            BeforeLogLineRead = BeforeLogLineReadUse.Checked;
        }

        private void chatFilter_CheckedChanged(object sender, EventArgs e)
        {
            ChatFilter = chatFilter.Checked;
        }

        private void UPNPUse_CheckedChanged(object sender, EventArgs e)
        {
            uPnPPort.Enabled = UPNPUse.Checked;
        }

        private void UseSSL_CheckedChanged(object sender, EventArgs e)
        {
            //UseSSL = SSLUse.Checked;
        }

        private void skinOnAct_CheckedChanged(object sender, EventArgs e)
        {
            SkinOnAct = skinOnAct.Checked;
            buttonRefresh_Click(sender, e);
        }

        private void buttonCopyCode_Click(object sender, EventArgs e)
        {
            String id = hashCode.Text;
            Task task = Task.Factory.StartNew(() =>
            {
                string url = ("http")  + getURLPath("");
                string address = string.Format("https://dev.zcube.kr/fcmhelper/v1/message?id={0}&message={1}", Uri.EscapeDataString(id), Uri.EscapeDataString(url));

                HttpWebRequest request = HttpWebRequest.CreateHttp(address);
                WebResponse response = request.GetResponse();
                String res = "";
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    res = reader.ReadToEnd();
                }
            });
        }

        void UpdateOverlayProc()
        {
            bool b = File.Exists(overlayProcExe);
            buttonOverlay.Enabled = b;
            if(SendMessage(JObject.FromObject(new
            {
                cmd = "check"
            })))
            {
                buttonStartStopOverlayProc.Enabled = b;
                buttonOpenOverlayProcManager.Enabled = true;
                buttonOverlay.Enabled = true;
            }
            else
            {
                buttonStartStopOverlayProc.Enabled = b;
                buttonOpenOverlayProcManager.Enabled = false;
                buttonOverlay.Enabled = false;
            }
        }
        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (WebSkinListView.SelectedItems.Count > 0)
            {
                string url = (string)WebSkinListView.SelectedItems[0].Tag;
                url = getURLPath(url);
                if(url != null)
                {
                    System.Diagnostics.Process.Start(url);
                }
            }
            else if (FileSkinListView.SelectedItems.Count > 0)
            {
                string url = (string)FileSkinListView.SelectedItems[0].Tag;
                url = getURLPath(url);
                if (url != null)
                {
                    System.Diagnostics.Process.Start(url);
                }
            }
            else
            {
                copyURLPath();
            }
        }

        private void port_TextChanged(object sender, EventArgs e)
        {
            uPnPPort.Text = port.Text;
        }

        private void skinList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (WebSkinListView.SelectedItems.Count > 0)
            {
                FileSkinListView.SelectedItems.Clear();
            }
        }

        private void FileSkinListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (FileSkinListView.SelectedItems.Count > 0)
            {
                WebSkinListView.SelectedItems.Clear();
            }
        }

        public bool StartOverlayProc()
        {
            UpdateOverlayProc();
            if(!buttonOpenOverlayProcManager.Enabled)
            {
                bool b = File.Exists(overlayProcExe);
                if (!b)
                {
                    return false;
                }
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo(overlayProcExe);
                    startInfo.WorkingDirectory = overlayProcDir;
                    startInfo.Arguments = "";
                    Process.Start(startInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }
            return true;
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            UpdateOverlayProc();
            try
            {
                if (File.Exists(overlayProcExe))
                {
                    SendMessage(JObject.FromObject(new
                    {
                        cmd = "stop"
                    }));
                }
                buttonDownload.Enabled = false;

                int idx = 0;
                string url;
                switch(idx)
                {
                    default:
                    case 0:
                        url = "https://www.dropbox.com/sh/ionr8nkmp49gr8d/AADzOjamXxPGjOzFuhBSthPHa?dl=1";
                        break;
                    case 1:
                        url = "https://www.dropbox.com/sh/7i07svs4ostoahd/AAA4Tn1Q1piI-m9ibwfsb8loa?dl=1";
                        break;
                }

                WebClient webClient = new WebClient();
                progressBar.Value = 0;
                progressBar.Minimum = 0;
                progressBar.Maximum = 100;
                progressBar.Show();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                webClient.DownloadFileAsync(new Uri(url), pluginDirectory + "/overlay_proc.zip");
            }
            catch (Exception ex)
            {
                progressBar.Hide();
                buttonDownload.Enabled = true;
                UpdateOverlayProc();
                MessageBox.Show(ex.Message);
            }
        }

        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.TotalBytesToReceive == -1)
            {
                if (progressBar.Style != ProgressBarStyle.Marquee)
                {
                    progressBar.Value = 100;
                    progressBar.Style = ProgressBarStyle.Marquee;
                }
            }
            else
            {
                progressBar.Value = e.ProgressPercentage;
                progressBar.Style = ProgressBarStyle.Blocks;
            }
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            Task task = Task.Factory.StartNew(() =>
            {
                try
                {
                    ZipFile z = new ZipFile(pluginDirectory + "/overlay_proc.zip");
                    z.ExtractExistingFile = ExtractExistingFileAction.OverwriteSilently;
                    z.ExtractAll(pluginDirectory + "/overlay_proc");
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            Task UITask = task.ContinueWith((t) =>
            {
                progressBar.Hide();
                buttonDownload.Enabled = true;
                UpdateOverlayProc();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void buttonStartStopOverlayProc_Click(object sender, EventArgs e)
        {
            bool b = File.Exists(overlayProcExe);
            if (!SendMessage(JObject.FromObject(new
            {
                cmd = "stop"
            })))
            {
                // run instance
                StartOverlayProc();
            }
        }

        private void buttonOpenOverlayProcManager_Click(object sender, EventArgs e)
        {
            bool b = File.Exists(overlayProcExe);
            if (!SendMessage(JObject.FromObject(new
            {
                cmd = "manager"
            })))
            {
                // run instance ?
            }
        }

        // sample capture request code
        public bool captureRequest(String id_)
        {
            if (!SendMessage(JObject.FromObject(new
            {
                cmd = "capture",
                value = new
                {
                    id = id_
                }
            })))
            {
                return true;
            }
            return false;
        }

        private void buttonOverlay_Click(object sender, EventArgs e)
        {
            if (FileSkinListView.SelectedItems.Count > 0)
            {
                string url = (string)FileSkinListView.SelectedItems[0].Tag;
                SendMessage(JObject.FromObject(new
                {
                    cmd = "set",
                    value = new
                    {
                        opacity = 1.0,
                        zoom = 1.0,
                        fps = 30.0,
                        hide = false,
                        useDragFilter = true,
                        useDragMove = true,
                        useResizeGrip = true,
                        NoActivate = false,
                        Transparent = false,
                        url = getURLPath(url, false),
                        title = FileSkinListView.SelectedItems[0].Text
                    }
                }));
            }
            else if (WebSkinListView.SelectedItems.Count > 0)
            {
                string url = (string)WebSkinListView.SelectedItems[0].Tag;
                SendMessage(JObject.FromObject(new
                {
                    cmd = "set",
                    value = new
                    {
                        opacity = 1.0,
                        zoom = 1.0,
                        fps = 30.0,
                        hide = false,
                        useDragFilter = true,
                        useDragMove = true,
                        useResizeGrip = true,
                        NoActivate = false,
                        Transparent = false,
                        url = getURLPath(url, false),
                        title = WebSkinListView.SelectedItems[0].Text
                    }
                }));
            }
        }

        private void hostnames_TextChanged(object sender, EventArgs e)
        {
            ServerUrlChanged();
        }

        private void randomURL_CheckedChanged(object sender, EventArgs e)
        {
            ServerUrlChanged();
        }

        private void ServerUrlChanged()
        {
            Hostname = hostnames.Text;
            if (core != null)
            {
                lock (core.skinObject)
                {
                    JToken skinObject = core.skinObject.DeepClone();
                    JArray urlConverted = new JArray();
                    JArray array = (JArray)skinObject["URLList"];
                    core.skinObject["Token"] = ACTWebSocketCore.randomDir;
                    if (array != null)
                    {
                        foreach (JToken obj in array)
                        {
                            obj["URL"] = getURLPath(obj["URL"].ToObject<String>(), false);
                        }
                    }
                    else
                    {
                        skinObject["URLList"] = new JArray();
                    }
                    SendMessage(JObject.FromObject(new
                    {
                        cmd = "urllist",
                        value = skinObject
                    }));
                }
            }
        }

        private void SSLUse_CheckedChanged(object sender, EventArgs e)
        {
            //UseSSL = SSLUse.Checked;
        }

    }
}
