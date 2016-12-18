using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Advanced_Combat_Tracker;
using System.IO;
using System.Reflection;
using System.Xml;

namespace ACTWebSocket_Plugin
{
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using System.Timers;
    using WebSocketSharp;
    using WebSocketSharp.Net;
    using WebSocketSharp.Server;
    using static ACTWebSocketCore;

    public interface PluginDirectory
    {
        void SetPluginDirectory(String path);
        String GetPluginDirectory();
    }
    public class ACTWebSocketMain : UserControl, IActPluginV1, PluginDirectory
    {
        private ACTWebSocketCore core;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label2;
        private TextBox port;
        private Label label3;
        private CheckBox autostart;
        private TabControl tabControl1;
        private TabPage tabMiniParse;
        private TableLayoutPanel tableLayoutPanel3;
        private CheckBox MiniParseUse;
        private Label label7;
        private Label label5;
        private Label label6;
        private TextBox MiniParseSortKey;
        private ComboBox sortType;
        private TabPage tabLogLineRead;
        private TableLayoutPanel tableLayoutPanel4;
        private CheckBox BeforeLogLineReadUse;
        private Label label8;
        private Label label1;
        private CheckBox localhostOnly;
        private TextBox hostname;
        private Label label4;
        private CheckBox OnLogLineReadUse;
        private Label label10;
        private Label server;
        private TableLayoutPanel tableLayoutPanel2;
        private Button buttonOff;
        private Button buttonOn;
        private Button copyURL;
        private Label label9;
        private CheckBox randomURL;
        private TabControl tabControl2;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private TableLayoutPanel tableLayoutPanel5;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button button1;
        private Button button2;
        private ListBox listBox2;
        private ListBox listBox1;
        private FlowLayoutPanel flowLayoutPanel2;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private Label label11;
        private Label label12;
        private Button button3;
        private Button button4;
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.port = new System.Windows.Forms.TextBox();
            this.server = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.autostart = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabMiniParse = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.MiniParseUse = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.MiniParseSortKey = new System.Windows.Forms.TextBox();
            this.sortType = new System.Windows.Forms.ComboBox();
            this.tabLogLineRead = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.BeforeLogLineReadUse = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.OnLogLineReadUse = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOff = new System.Windows.Forms.Button();
            this.buttonOn = new System.Windows.Forms.Button();
            this.copyURL = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.localhostOnly = new System.Windows.Forms.CheckBox();
            this.hostname = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.randomURL = new System.Windows.Forms.CheckBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabMiniParse.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabLogLineRead.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.port, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.server, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.autostart, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.localhostOnly, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.hostname, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.randomURL, 1, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // port
            // 
            resources.ApplyResources(this.port, "port");
            this.port.Name = "port";
            // 
            // server
            // 
            resources.ApplyResources(this.server, "server");
            this.server.Name = "server";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // autostart
            // 
            resources.ApplyResources(this.autostart, "autostart");
            this.autostart.Name = "autostart";
            this.autostart.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Controls.Add(this.tabMiniParse);
            this.tabControl1.Controls.Add(this.tabLogLineRead);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabMiniParse
            // 
            this.tabMiniParse.Controls.Add(this.tableLayoutPanel3);
            resources.ApplyResources(this.tabMiniParse, "tabMiniParse");
            this.tabMiniParse.Name = "tabMiniParse";
            this.tabMiniParse.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.MiniParseUse, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.MiniParseSortKey, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.sortType, 1, 2);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // MiniParseUse
            // 
            resources.ApplyResources(this.MiniParseUse, "MiniParseUse");
            this.MiniParseUse.Checked = true;
            this.MiniParseUse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MiniParseUse.Name = "MiniParseUse";
            this.MiniParseUse.UseVisualStyleBackColor = true;
            this.MiniParseUse.CheckedChanged += new System.EventHandler(this.MiniParseUse_CheckedChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // MiniParseSortKey
            // 
            resources.ApplyResources(this.MiniParseSortKey, "MiniParseSortKey");
            this.MiniParseSortKey.Name = "MiniParseSortKey";
            this.MiniParseSortKey.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // sortType
            // 
            resources.ApplyResources(this.sortType, "sortType");
            this.sortType.Cursor = System.Windows.Forms.Cursors.Default;
            this.sortType.DisplayMember = "Key";
            this.sortType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sortType.FormattingEnabled = true;
            this.sortType.Name = "sortType";
            this.sortType.ValueMember = "Value";
            this.sortType.SelectedIndexChanged += new System.EventHandler(this.MiniParseSortType_SelectedIndexChanged);
            // 
            // tabLogLineRead
            // 
            this.tabLogLineRead.Controls.Add(this.tableLayoutPanel4);
            resources.ApplyResources(this.tabLogLineRead, "tabLogLineRead");
            this.tabLogLineRead.Name = "tabLogLineRead";
            this.tabLogLineRead.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.BeforeLogLineReadUse, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.OnLogLineReadUse, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.label10, 0, 1);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // BeforeLogLineReadUse
            // 
            resources.ApplyResources(this.BeforeLogLineReadUse, "BeforeLogLineReadUse");
            this.BeforeLogLineReadUse.Name = "BeforeLogLineReadUse";
            this.BeforeLogLineReadUse.UseVisualStyleBackColor = true;
            this.BeforeLogLineReadUse.CheckedChanged += new System.EventHandler(this.LogLineReadUse_CheckedChanged);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // OnLogLineReadUse
            // 
            resources.ApplyResources(this.OnLogLineReadUse, "OnLogLineReadUse");
            this.OnLogLineReadUse.Name = "OnLogLineReadUse";
            this.OnLogLineReadUse.UseVisualStyleBackColor = true;
            this.OnLogLineReadUse.CheckedChanged += new System.EventHandler(this.OnLogLineReadUse_CheckedChanged);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.buttonOff, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonOn, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.copyURL, 2, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
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
            // copyURL
            // 
            resources.ApplyResources(this.copyURL, "copyURL");
            this.copyURL.Name = "copyURL";
            this.copyURL.UseVisualStyleBackColor = true;
            this.copyURL.Click += new System.EventHandler(this.copyURL_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // localhostOnly
            // 
            resources.ApplyResources(this.localhostOnly, "localhostOnly");
            this.localhostOnly.Checked = true;
            this.localhostOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.localhostOnly.Name = "localhostOnly";
            this.localhostOnly.UseVisualStyleBackColor = true;
            // 
            // hostname
            // 
            resources.ApplyResources(this.hostname, "hostname");
            this.hostname.Name = "hostname";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // randomURL
            // 
            resources.ApplyResources(this.randomURL, "randomURL");
            this.randomURL.Name = "randomURL";
            this.randomURL.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            resources.ApplyResources(this.tabControl2, "tabControl2");
            this.tabControl2.Controls.Add(this.tabPage1);
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.Transparent;
            this.tabPage2.Controls.Add(this.tableLayoutPanel5);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Click += new System.EventHandler(this.tabPage2_Click);
            // 
            // tableLayoutPanel5
            // 
            resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
            this.tableLayoutPanel5.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.listBox2, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.listBox1, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.flowLayoutPanel2, 1, 2);
            this.tableLayoutPanel5.Controls.Add(this.label11, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.label12, 1, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Controls.Add(this.button2);
            this.flowLayoutPanel1.Controls.Add(this.button4);
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // listBox2
            // 
            resources.ApplyResources(this.listBox2, "listBox2");
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Name = "listBox2";
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // listBox1
            // 
            resources.ApplyResources(this.listBox1, "listBox1");
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Name = "listBox1";
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.checkBox1);
            this.flowLayoutPanel2.Controls.Add(this.checkBox2);
            this.flowLayoutPanel2.Controls.Add(this.button3);
            resources.ApplyResources(this.flowLayoutPanel2, "flowLayoutPanel2");
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            resources.ApplyResources(this.checkBox2, "checkBox2");
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // button3
            // 
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // ACTWebSocketMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl2);
            this.Name = "ACTWebSocketMain";
            this.Load += new System.EventHandler(this.ACTWebSocket_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabMiniParse.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tabLogLineRead.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        #endregion
        public ACTWebSocketMain()
        {
            InitializeComponent();
            ((Control)this.tabPage2).Enabled = false;
        }

        ~ACTWebSocketMain()
        {
            SaveSettings();
        }

        Label lblStatus;    // The status label that appears in ACT's Plugin tab
        string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\ACTWebSocket.config.xml");
        SettingsSerializer xmlSettings;

        #region IActPluginV1 Members

        static readonly List<KeyValuePair<string, MiniParseSortType>> sortTypeDict = new List<KeyValuePair<string, MiniParseSortType>>()
        {
            new KeyValuePair<string, MiniParseSortType>("DoNotSort", MiniParseSortType.None),
            new KeyValuePair<string, MiniParseSortType>("SortStringAscending", MiniParseSortType.StringAscending),
            new KeyValuePair<string, MiniParseSortType>("SortStringDescending", MiniParseSortType.StringDescending),
            new KeyValuePair<string, MiniParseSortType>("SortNumberAscending", MiniParseSortType.NumericAscending),
            new KeyValuePair<string, MiniParseSortType>("SortNumberDescending", MiniParseSortType.NumericDescending)
        };

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            if (core == null)
            {
                core = new ACTWebSocketCore();
                core.pluginDirectory = pluginDirectory;
            }
            lblStatus = pluginStatusText;   // Hand the status label's reference to our local var
            pluginScreenSpace.Controls.Add(this);   // Add this UserControl to the tab ACT provides
            this.Dock = DockStyle.Fill; // Expand the UserControl to fill the tab's client space
            xmlSettings = new SettingsSerializer(this);	// Create a new settings serializer and pass it this instance
            sortType.SelectedIndex = -1;

            sortType.DataSource = sortTypeDict;
            LoadSettings();



            if (core != null)
            {
                core.Filters["/BeforeLogLineRead"] = BeforeLogLineReadUse.Checked;
                core.Filters["/OnLogLineRead"] = OnLogLineReadUse.Checked;
                core.Filters["/MiniParse"] = MiniParseUse.Checked;
                core.Config.SortKey = MiniParseSortKey.Text.Trim();
                core.Config.SortType = (MiniParseSortType)sortType.SelectedIndex;
            }
            if (autostart.Checked)
            {
                StartServer();
            }
            else
            {
                StopServer();
            }
            // Create some sort of parsing event handler.  After the "+=" hit TAB twice and the code will be generated for you.
            ActGlobals.oFormActMain.BeforeLogLineRead += this.oFormActMain_BeforeLogLineRead;
            ActGlobals.oFormActMain.OnLogLineRead += this.oFormActMain_OnLogLineRead;
            var s = ActGlobals.oFormActMain.ActPlugins;
            lblStatus.Text = "Plugin Started";
        }

        public void DeInitPlugin()
        {
            StopServer();
            // Unsubscribe from any events you listen to when exiting!
            ActGlobals.oFormActMain.BeforeLogLineRead -= this.oFormActMain_BeforeLogLineRead;
            ActGlobals.oFormActMain.OnLogLineRead -= this.oFormActMain_OnLogLineRead;

            SaveSettings();
            lblStatus.Text = "Plugin Exited";
        }

        #endregion

        void oFormActMain_AfterCombatAction(bool isImport, CombatActionEventArgs actionInfo)
        {
        }

        void LoadSettings()
        {
            xmlSettings.AddControlSetting(port.Name, port);
            xmlSettings.AddControlSetting(localhostOnly.Name, localhostOnly);
            xmlSettings.AddControlSetting(autostart.Name, autostart);
            xmlSettings.AddControlSetting(hostname.Name, hostname);
            xmlSettings.AddControlSetting(MiniParseSortKey.Name, MiniParseSortKey);
            xmlSettings.AddControlSetting(sortType.Name, sortType);
            xmlSettings.AddControlSetting(MiniParseUse.Name, MiniParseUse);
            xmlSettings.AddControlSetting(BeforeLogLineReadUse.Name, BeforeLogLineReadUse);
            xmlSettings.AddControlSetting(randomURL.Name, randomURL);

            if (File.Exists(settingsFile))
            {
                FileStream fs = new FileStream(settingsFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlTextReader xReader = new XmlTextReader(fs);

                try
                {
                    while (xReader.Read())
                    {
                        if (xReader.NodeType == XmlNodeType.Element)
                        {
                            if (xReader.LocalName == "SettingsSerializer")
                            {
                                xmlSettings.ImportFromXml(xReader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Error loading settings: " + ex.Message;
                }
                xReader.Close();
            }
        }
        void SaveSettings()
        {
            FileStream fs = new FileStream(settingsFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            XmlTextWriter xWriter = new XmlTextWriter(fs, Encoding.UTF8);
            xWriter.Formatting = Formatting.Indented;
            xWriter.Indentation = 1;
            xWriter.IndentChar = '\t';
            xWriter.WriteStartDocument(true);
            xWriter.WriteStartElement("Config");    // <Config>
            xWriter.WriteStartElement("SettingsSerializer");    // <Config><SettingsSerializer>
            xmlSettings.ExportToXml(xWriter);   // Fill the SettingsSerializer XML
            xWriter.WriteEndElement();  // </SettingsSerializer>
            xWriter.WriteEndElement();  // </Config>
            xWriter.WriteEndDocument(); // Tie up loose ends (shouldn't be any)
            xWriter.Flush();    // Flush the file buffer to disk
            xWriter.Close();
        }

        private void ACTWebSocket_Load(object sender, EventArgs e)
        {
            UpdateList();
        }

        private void oFormActMain_BeforeLogLineRead(
            bool isImport,
            LogLineEventArgs logInfo)
        {
            core.Broadcast("/BeforeLogLineRead", logInfo.logLine);
        }

        private void oFormActMain_OnLogLineRead(
            bool isImport,
            LogLineEventArgs logInfo)
        {
            core.Broadcast("/OnLogLineRead", logInfo.logLine);
        }


        private void StartServer()
        {
            if (randomURL.Checked)
            {
                core.randomDir = Guid.NewGuid().ToString();
            }
            else
            {
                core.randomDir = null;
            }
            core.StartServer(localhostOnly.Checked ? "127.0.0.1" : "0.0.0.0", Convert.ToInt16(port.Text), hostname.Text.Trim());
            localhostOnly.Enabled = false;
            port.Enabled = false;
            hostname.Enabled = false;
            buttonOn.Enabled = false;
            buttonOff.Enabled = true;
            randomURL.Enabled = false;
            ((Control)this.tabPage2).Enabled = true;
            //tabPage1
        }

        private void StopServer()
        {
            core.StopServer();
            localhostOnly.Enabled = true;
            port.Enabled = true;
            hostname.Enabled = true;
            buttonOn.Enabled = true;
            buttonOff.Enabled = false;
            randomURL.Enabled = true;
            ((Control)this.tabPage2).Enabled = false;
        }

        private void buttonOn_Click(object sender, EventArgs e)
        {
            StartServer();
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

        String pluginDirectory = "";
        public void SetPluginDirectory(string path)
        {
            pluginDirectory = path;
        }

        public string GetPluginDirectory()
        {
            return pluginDirectory;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (core != null)
            {
                core.Config.SortKey = MiniParseSortKey.Text.Trim();
            }
        }

        private void MiniParseSortType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (core != null)
            {
                core.Config.SortType = (MiniParseSortType)sortType.SelectedIndex;
            }
        }

        private void LogLineReadUse_CheckedChanged(object sender, EventArgs e)
        {
            if (core != null)
            {
                core.Filters["/BeforeLogLineRead"] = BeforeLogLineReadUse.Checked;
            }
        }

        private void OnLogLineReadUse_CheckedChanged(object sender, EventArgs e)
        {
            if (core != null)
            {
                core.Filters["/OnLogLineRead"] = OnLogLineReadUse.Checked;
            }
        }

        private void MiniParseUse_CheckedChanged(object sender, EventArgs e)
        {
            if (core != null)
            {
                core.Filters["/MiniParse"] = MiniParseUse.Checked;
            }
        }

        private void copyURL_Click(object sender, EventArgs e)
        {
            String url = "";
            if (localhostOnly.Checked)
            {
                url = "http://localhost:" + port.Text + "/";
            }
            else
            {
                url = "http://" + hostname.Text + ":" + port.Text + "/";
            }
            if (core.randomDir != null)
            {
                url += core.randomDir + "/";
            }
            Clipboard.SetText(url);
        }
        string GetRelativePath(string filespec, string folder)
        {
            Uri pathUri = new Uri(filespec);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        public static List<String> SearchForWindow(string title)
        {
            SearchData sd = new SearchData { TitleKeyword = title };
            EnumWindows(new EnumWindowsProc(EnumProc), ref sd);
            return sd.overlayWindowTitles;
        }

        public static bool EnumProc(IntPtr hWnd, ref SearchData data)
        {
            // Check classname and title 
            // This is different from FindWindow() in that the code below allows partial matches
            StringBuilder sb = new StringBuilder(1024);
            GetWindowText(hWnd, sb, sb.Capacity);
            if (sb.ToString().StartsWith(data.TitleKeyword))
            {
                data.overlayWindowTitles.Add(sb.ToString());
            }
            return true;
        }

        public class SearchData
        {
            public String TitleKeyword = "";
            public List<String> overlayWindowTitles = new List<String>();
        }

        private delegate bool EnumWindowsProc(IntPtr hWnd, ref SearchData data);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, ref SearchData data);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        public void UpdateList()
        {
            this.listBox1.Items.Clear();
            foreach (string file in Directory.EnumerateFiles(pluginDirectory, "*.html", SearchOption.AllDirectories))
            {
                this.listBox1.Items.Add(GetRelativePath(file, pluginDirectory));
            }
            List<String> titles = SearchForWindow("overlay_");
            this.listBox2.Items.Clear();
            this.listBox2.Sorted = true;
            foreach (string title in titles)
            {
                this.listBox2.Items.Add(title);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String overlayPath = pluginDirectory + "/overlay/overlaydemo.exe";
            if (File.Exists(overlayPath) && this.listBox1.SelectedIndex >= 0)
            {
                String url = "";
                if (localhostOnly.Checked)
                {
                    url = "http://localhost:" + port.Text + "/";
                }
                else
                {
                    url = "http://" + hostname.Text + ":" + port.Text + "/";
                }
                if (core.randomDir != null)
                {
                    url += core.randomDir + "/";
                }
                String param = url + System.Uri.EscapeDataString(this.listBox1.Items[this.listBox1.SelectedIndex].ToString());
                param = param.Replace("%5C", "/");
                String title = "overlay_" + Guid.NewGuid().ToString();
                this.listBox2.Items.Add(title);
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = overlayPath;
                startInfo.Arguments = param + " " + title;
                Process.Start(startInfo);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (this.listBox1.SelectedIndex >= 0)
            {
                String url = "";
                if (localhostOnly.Checked)
                {
                    url = "http://localhost:" + port.Text + "/";
                }
                else
                {
                    url = "http://" + hostname.Text + ":" + port.Text + "/";
                }
                if (core.randomDir != null)
                {
                    url += core.randomDir + "/";
                }
                String param = url + System.Uri.EscapeDataString(this.listBox1.Items[this.listBox1.SelectedIndex].ToString());
                param = param.Replace("%5C", "/");
                Clipboard.SetText(param);
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            UpdateList();
        }

        private static int ToIntPtr32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool CloseWindow(IntPtr hWnd);

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(int dwErrorCode);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", SetLastError = true)]
        public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int WS_EX_NOACTIVATE  = 0x08000000;

        public static IntPtr GetWindowLongFlag(IntPtr hWnd, IntPtr value)
        {
            int nIndex = GWL_EXSTYLE;
            int error = 0;
            IntPtr result = IntPtr.Zero;

            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                Int32 result32 = GetWindowLong(hWnd, nIndex);
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(result32);
            }
            else
            {
                result = GetWindowLongPtr(hWnd, nIndex);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            result = new IntPtr(result.ToInt64() & value.ToInt64());

            return result;
        }

        public static IntPtr SetWindowLongFlag(IntPtr hWnd, IntPtr value)
        {
            int nIndex = GWL_EXSTYLE;
            int error = 0;
            IntPtr result = IntPtr.Zero;

            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                Int32 result32 = GetWindowLong(hWnd, nIndex);
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(result32);
            }
            else
            {
                result = GetWindowLongPtr(hWnd, nIndex);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            result = new IntPtr(result.ToInt64() | value.ToInt64());

            if (IntPtr.Size == 4)
            {
                Int32 result32 = SetWindowLong(hWnd, nIndex, ToIntPtr32(result));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(result32);
            }
            else
            {
                result = SetWindowLongPtr(hWnd, nIndex, result);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }

        public static IntPtr UnsetWindowLongFlag(IntPtr hWnd, IntPtr value)
        {
            int nIndex = GWL_EXSTYLE;
            int error = 0;
            IntPtr result = IntPtr.Zero;

            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                Int32 result32 = GetWindowLong(hWnd, nIndex);
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(result32);
            }
            else
            {
                result = GetWindowLongPtr(hWnd, nIndex);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            result = new IntPtr(result.ToInt64() & ~value.ToInt64());

            if (IntPtr.Size == 4)
            {
                Int32 result32 = SetWindowLong(hWnd, nIndex, ToIntPtr32(result));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(result32);
            }
            else
            {
                result = SetWindowLongPtr(hWnd, nIndex, result);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }
        
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.listBox2.SelectedIndex >= 0)
            {
                String title = this.listBox2.Items[this.listBox2.SelectedIndex].ToString();
                IntPtr hwnd = FindWindow(null, title);
                if (hwnd == null || hwnd.ToInt64() == 0)
                {
                    //this.listBox2.Items.RemoveAt(this.listBox2.SelectedIndex);
                    UpdateList();
                }
                else
                {
                    if (checkBox1.Checked)
                        SetWindowLongFlag(hwnd, new IntPtr(WS_EX_TRANSPARENT));
                    else
                        UnsetWindowLongFlag(hwnd, new IntPtr(WS_EX_TRANSPARENT));
                }
            }

        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.listBox2.SelectedIndex >= 0)
            {
                String title = this.listBox2.Items[this.listBox2.SelectedIndex].ToString();
                IntPtr hwnd = FindWindow(null, title);
                if (hwnd == null || hwnd.ToInt64() == 0)
                {
                    //this.listBox2.Items.RemoveAt(this.listBox2.SelectedIndex);
                    UpdateList();
                }
                else
                {
                    if (checkBox2.Checked)
                        SetWindowLongFlag(hwnd, new IntPtr(WS_EX_NOACTIVATE));
                    else
                        UnsetWindowLongFlag(hwnd, new IntPtr(WS_EX_NOACTIVATE));
                }
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox2.SelectedIndex >= 0)
            {
                String title = this.listBox2.Items[this.listBox2.SelectedIndex].ToString();
                IntPtr hwnd = FindWindow(null, title);
                if (hwnd == null || hwnd.ToInt64() == 0)
                {
                    //this.listBox2.Items.RemoveAt(this.listBox2.SelectedIndex);
                    UpdateList();
                }
                else
                {
                    checkBox1.Checked = (GetWindowLongFlag(hwnd, new IntPtr(WS_EX_TRANSPARENT)).ToInt64() > 0);
                    checkBox2.Checked = (GetWindowLongFlag(hwnd, new IntPtr(WS_EX_NOACTIVATE)).ToInt64() > 0);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.listBox2.SelectedIndex >= 0)
            {
                String title = this.listBox2.Items[this.listBox2.SelectedIndex].ToString();
                IntPtr hwnd = FindWindow(null, title);
                if (hwnd == null || hwnd.ToInt64() == 0)
                {
                    //this.listBox2.Items.RemoveAt(this.listBox2.SelectedIndex);
                    UpdateList();
                }
                else
                {
                    SendMessage(hwnd, 0x0400 + 1, new IntPtr(0x08), new IntPtr(0x08));
                    CloseWindow(hwnd);
                    this.listBox2.Items.RemoveAt(this.listBox2.SelectedIndex);
                }
            }
        }
    }
}
