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
    using ACTWebSocket.Core;
    using Newtonsoft.Json.Linq;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
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
        void SetPluginDirectory(string path);
        string GetPluginDirectory();
    }
    public class ACTWebSocketMain : UserControl, IActPluginV1, PluginDirectory
    {
        private ACTWebSocketCore core;
        private TextBox port;
        private CheckBox autostart;
        private CheckBox MiniParseUse;
        private TextBox MiniParseSortKey;
        private ComboBox sortType;
        private CheckBox BeforeLogLineReadUse;
        private CheckBox localhostOnly;
        private TextBox hostname;
        private CheckBox OnLogLineReadUse;
        private Button buttonOff;
        private Button buttonOn;
        private Button copyURL;
        private CheckBox randomURL;
        private Button button1;
        private Button button2;
        private ListBox listBox2;
        private ListBox listBox1;
        private CheckBox checkBox1;
        private CheckBox checkBox2;
        private Button close;
        private Label label13;
        private GroupBox startoption;
        private GroupBox hostdata;
        private Label label15;
        private Label label14;
        private GroupBox miniparse;
        private Label label17;
        private Label label16;
        private GroupBox othersets;
        private GroupBox serverStatus;
        private GroupBox groupBox1;
        private SplitContainer splitContainer1;
        private Button button4;
        private GroupBox groupBox2;
        private CheckBox checkBox6;
        private CheckBox checkBox5;
        private CheckBox checkBox4;
        private CheckBox checkBox3;
        private Panel panel1;
        private Panel panel2;
        private Label label1;
        private Label label2;
        private TextBox overlayTitle;
        private Label label3;
        private Label label4;
        private TrackBar opacity;
        private TextBox url;
        private Label label5;
        private Button button5;
        private TrackBar zoom;
        private Label label6;
        private TrackBar fps;
        private Label label7;
        private TextBox x;
        private Label label8;
        private TextBox height;
        private Label label11;
        private TextBox width;
        private Label label10;
        private TextBox y;
        private Label label9;
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.copyURL = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.port = new System.Windows.Forms.TextBox();
            this.autostart = new System.Windows.Forms.CheckBox();
            this.MiniParseUse = new System.Windows.Forms.CheckBox();
            this.MiniParseSortKey = new System.Windows.Forms.TextBox();
            this.sortType = new System.Windows.Forms.ComboBox();
            this.BeforeLogLineReadUse = new System.Windows.Forms.CheckBox();
            this.OnLogLineReadUse = new System.Windows.Forms.CheckBox();
            this.buttonOff = new System.Windows.Forms.Button();
            this.buttonOn = new System.Windows.Forms.Button();
            this.localhostOnly = new System.Windows.Forms.CheckBox();
            this.hostname = new System.Windows.Forms.TextBox();
            this.randomURL = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.close = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.startoption = new System.Windows.Forms.GroupBox();
            this.hostdata = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.miniparse = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.othersets = new System.Windows.Forms.GroupBox();
            this.serverStatus = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.height = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.width = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.y = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.x = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.fps = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.zoom = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.url = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.opacity = new System.Windows.Forms.TrackBar();
            this.overlayTitle = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.startoption.SuspendLayout();
            this.hostdata.SuspendLayout();
            this.miniparse.SuspendLayout();
            this.othersets.SuspendLayout();
            this.serverStatus.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.opacity)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.Controls.Add(this.listBox2);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Name = "panel1";
            // 
            // listBox1
            // 
            resources.ApplyResources(this.listBox1, "listBox1");
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Name = "listBox1";
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.copyURL);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Name = "panel2";
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // copyURL
            // 
            resources.ApplyResources(this.copyURL, "copyURL");
            this.copyURL.Name = "copyURL";
            this.copyURL.UseVisualStyleBackColor = true;
            this.copyURL.Click += new System.EventHandler(this.copyURL_Click);
            // 
            // button4
            // 
            resources.ApplyResources(this.button4, "button4");
            this.button4.Name = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Name = "label1";
            // 
            // listBox2
            // 
            resources.ApplyResources(this.listBox2, "listBox2");
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Name = "listBox2";
            this.listBox2.SelectedIndexChanged += new System.EventHandler(this.listBox2_SelectedIndexChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Name = "label2";
            // 
            // port
            // 
            resources.ApplyResources(this.port, "port");
            this.port.Name = "port";
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
            // BeforeLogLineReadUse
            // 
            resources.ApplyResources(this.BeforeLogLineReadUse, "BeforeLogLineReadUse");
            this.BeforeLogLineReadUse.BackColor = System.Drawing.Color.Transparent;
            this.BeforeLogLineReadUse.Name = "BeforeLogLineReadUse";
            this.BeforeLogLineReadUse.UseVisualStyleBackColor = false;
            this.BeforeLogLineReadUse.CheckedChanged += new System.EventHandler(this.LogLineReadUse_CheckedChanged);
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
            // localhostOnly
            // 
            resources.ApplyResources(this.localhostOnly, "localhostOnly");
            this.localhostOnly.BackColor = System.Drawing.Color.Transparent;
            this.localhostOnly.Checked = true;
            this.localhostOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.localhostOnly.Name = "localhostOnly";
            this.localhostOnly.UseVisualStyleBackColor = false;
            // 
            // hostname
            // 
            resources.ApplyResources(this.hostname, "hostname");
            this.hostname.Name = "hostname";
            // 
            // randomURL
            // 
            resources.ApplyResources(this.randomURL, "randomURL");
            this.randomURL.BackColor = System.Drawing.Color.Transparent;
            this.randomURL.Name = "randomURL";
            this.randomURL.UseVisualStyleBackColor = false;
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.BackColor = System.Drawing.Color.Transparent;
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.update_overlayWindow);
            // 
            // checkBox2
            // 
            resources.ApplyResources(this.checkBox2, "checkBox2");
            this.checkBox2.BackColor = System.Drawing.Color.Transparent;
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = false;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.update_overlayWindow);
            // 
            // close
            // 
            resources.ApplyResources(this.close, "close");
            this.close.Name = "close";
            this.close.UseVisualStyleBackColor = true;
            this.close.Click += new System.EventHandler(this.button3_Click);
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.BackColor = System.Drawing.Color.White;
            this.label13.Name = "label13";
            // 
            // startoption
            // 
            resources.ApplyResources(this.startoption, "startoption");
            this.startoption.BackColor = System.Drawing.Color.Transparent;
            this.startoption.Controls.Add(this.localhostOnly);
            this.startoption.Controls.Add(this.randomURL);
            this.startoption.Controls.Add(this.autostart);
            this.startoption.Name = "startoption";
            this.startoption.TabStop = false;
            // 
            // hostdata
            // 
            resources.ApplyResources(this.hostdata, "hostdata");
            this.hostdata.BackColor = System.Drawing.Color.Transparent;
            this.hostdata.Controls.Add(this.label15);
            this.hostdata.Controls.Add(this.label14);
            this.hostdata.Controls.Add(this.hostname);
            this.hostdata.Controls.Add(this.port);
            this.hostdata.Name = "hostdata";
            this.hostdata.TabStop = false;
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
            // miniparse
            // 
            resources.ApplyResources(this.miniparse, "miniparse");
            this.miniparse.BackColor = System.Drawing.Color.Transparent;
            this.miniparse.Controls.Add(this.label17);
            this.miniparse.Controls.Add(this.label16);
            this.miniparse.Controls.Add(this.MiniParseUse);
            this.miniparse.Controls.Add(this.sortType);
            this.miniparse.Controls.Add(this.MiniParseSortKey);
            this.miniparse.Name = "miniparse";
            this.miniparse.TabStop = false;
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Name = "label17";
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Name = "label16";
            // 
            // othersets
            // 
            resources.ApplyResources(this.othersets, "othersets");
            this.othersets.BackColor = System.Drawing.Color.Transparent;
            this.othersets.Controls.Add(this.BeforeLogLineReadUse);
            this.othersets.Controls.Add(this.OnLogLineReadUse);
            this.othersets.Name = "othersets";
            this.othersets.TabStop = false;
            // 
            // serverStatus
            // 
            resources.ApplyResources(this.serverStatus, "serverStatus");
            this.serverStatus.Controls.Add(this.buttonOn);
            this.serverStatus.Controls.Add(this.buttonOff);
            this.serverStatus.Name = "serverStatus";
            this.serverStatus.TabStop = false;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.splitContainer1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.BackColor = System.Drawing.Color.Transparent;
            this.groupBox2.Controls.Add(this.height);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.width);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.y);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.x);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.fps);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.zoom);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.button5);
            this.groupBox2.Controls.Add(this.url);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.opacity);
            this.groupBox2.Controls.Add(this.overlayTitle);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.checkBox6);
            this.groupBox2.Controls.Add(this.checkBox5);
            this.groupBox2.Controls.Add(this.checkBox4);
            this.groupBox2.Controls.Add(this.checkBox3);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Controls.Add(this.checkBox2);
            this.groupBox2.Controls.Add(this.close);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // height
            // 
            resources.ApplyResources(this.height, "height");
            this.height.Name = "height";
            this.height.TextChanged += new System.EventHandler(this.update_overlayWindowPosition);
            this.height.Enter += new System.EventHandler(this.x_Enter);
            this.height.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.digitOnly_KeyPress);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Name = "label11";
            // 
            // width
            // 
            resources.ApplyResources(this.width, "width");
            this.width.Name = "width";
            this.width.TextChanged += new System.EventHandler(this.update_overlayWindowPosition);
            this.width.Enter += new System.EventHandler(this.x_Enter);
            this.width.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.digitOnly_KeyPress);
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Name = "label10";
            // 
            // y
            // 
            resources.ApplyResources(this.y, "y");
            this.y.Name = "y";
            this.y.TextChanged += new System.EventHandler(this.update_overlayWindowPosition);
            this.y.Enter += new System.EventHandler(this.x_Enter);
            this.y.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.digitOnly_KeyPress);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Name = "label9";
            // 
            // x
            // 
            resources.ApplyResources(this.x, "x");
            this.x.Name = "x";
            this.x.TextChanged += new System.EventHandler(this.update_overlayWindowPosition);
            this.x.Enter += new System.EventHandler(this.x_Enter);
            this.x.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.digitOnly_KeyPress);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Name = "label8";
            // 
            // fps
            // 
            resources.ApplyResources(this.fps, "fps");
            this.fps.Maximum = 60;
            this.fps.Minimum = 1;
            this.fps.Name = "fps";
            this.fps.Value = 25;
            this.fps.Scroll += new System.EventHandler(this.update_overlayWindow);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Name = "label7";
            // 
            // zoom
            // 
            resources.ApplyResources(this.zoom, "zoom");
            this.zoom.LargeChange = 25;
            this.zoom.Maximum = 500;
            this.zoom.Minimum = 25;
            this.zoom.Name = "zoom";
            this.zoom.Value = 25;
            this.zoom.Scroll += new System.EventHandler(this.update_overlayWindow);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Name = "label6";
            // 
            // button5
            // 
            resources.ApplyResources(this.button5, "button5");
            this.button5.Name = "button5";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // url
            // 
            resources.ApplyResources(this.url, "url");
            this.url.Name = "url";
            this.url.TextChanged += new System.EventHandler(this.url_TextChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Name = "label5";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Name = "label4";
            // 
            // opacity
            // 
            resources.ApplyResources(this.opacity, "opacity");
            this.opacity.Maximum = 1000;
            this.opacity.Name = "opacity";
            this.opacity.Value = 1000;
            this.opacity.Scroll += new System.EventHandler(this.update_overlayWindow);
            // 
            // overlayTitle
            // 
            resources.ApplyResources(this.overlayTitle, "overlayTitle");
            this.overlayTitle.Name = "overlayTitle";
            this.overlayTitle.KeyUp += new System.Windows.Forms.KeyEventHandler(this.overlayTitle_KeyUp);
            this.overlayTitle.Leave += new System.EventHandler(this.overlayTitle_Leave);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Name = "label3";
            // 
            // checkBox6
            // 
            resources.ApplyResources(this.checkBox6, "checkBox6");
            this.checkBox6.BackColor = System.Drawing.Color.Transparent;
            this.checkBox6.Checked = true;
            this.checkBox6.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.UseVisualStyleBackColor = false;
            this.checkBox6.CheckedChanged += new System.EventHandler(this.update_overlayWindow);
            // 
            // checkBox5
            // 
            resources.ApplyResources(this.checkBox5, "checkBox5");
            this.checkBox5.BackColor = System.Drawing.Color.Transparent;
            this.checkBox5.Checked = true;
            this.checkBox5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.UseVisualStyleBackColor = false;
            this.checkBox5.CheckedChanged += new System.EventHandler(this.update_overlayWindow);
            // 
            // checkBox4
            // 
            resources.ApplyResources(this.checkBox4, "checkBox4");
            this.checkBox4.BackColor = System.Drawing.Color.Transparent;
            this.checkBox4.Checked = true;
            this.checkBox4.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.UseVisualStyleBackColor = false;
            this.checkBox4.CheckedChanged += new System.EventHandler(this.update_overlayWindow);
            // 
            // checkBox3
            // 
            resources.ApplyResources(this.checkBox3, "checkBox3");
            this.checkBox3.BackColor = System.Drawing.Color.Transparent;
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.UseVisualStyleBackColor = false;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.update_overlayWindow);
            // 
            // ACTWebSocketMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.serverStatus);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.othersets);
            this.Controls.Add(this.miniparse);
            this.Controls.Add(this.hostdata);
            this.Controls.Add(this.startoption);
            this.Controls.Add(this.label13);
            this.Name = "ACTWebSocketMain";
            this.Load += new System.EventHandler(this.ACTWebSocket_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.startoption.ResumeLayout(false);
            this.startoption.PerformLayout();
            this.hostdata.ResumeLayout(false);
            this.hostdata.PerformLayout();
            this.miniparse.ResumeLayout(false);
            this.miniparse.PerformLayout();
            this.othersets.ResumeLayout(false);
            this.othersets.PerformLayout();
            this.serverStatus.ResumeLayout(false);
            this.serverStatus.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.zoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.opacity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        #endregion
        public ACTWebSocketMain()
        {
            InitializeComponent();
        }

        ~ACTWebSocketMain()
        {
            CloseAll();
            SaveSettings();
        }

        string overlayWindowPrefix = "overlay_";

        Label lblStatus;    // The status label that appears in ACT's Plugin tab
        string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\ACTWebSocket.config.xml");
        SettingsSerializer xmlSettings;

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

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;
            string specName = "(none)";
            try { specName = CultureInfo.CreateSpecificCulture(ci.Name).Name; } catch { }
            if(specName == "ko-KR")
            {
                sortTypeDict = new List<KeyValuePair<string, MiniParseSortType>>()
                {
                    new KeyValuePair<string, MiniParseSortType>("정렬 안함", MiniParseSortType.None),
                    new KeyValuePair<string, MiniParseSortType>("문자열 올림차순 정렬", MiniParseSortType.StringAscending),
                    new KeyValuePair<string, MiniParseSortType>("문자열 내림차순 정렬", MiniParseSortType.StringDescending),
                    new KeyValuePair<string, MiniParseSortType>("숫자 올림차순 정렬", MiniParseSortType.NumericAscending),
                    new KeyValuePair<string, MiniParseSortType>("숫자 내림차순 정렬", MiniParseSortType.NumericDescending)
                };
            }
            else
            {
                sortTypeDict = new List<KeyValuePair<string, MiniParseSortType>>()
                {
                    new KeyValuePair<string, MiniParseSortType>("DoNotSort", MiniParseSortType.None),
                    new KeyValuePair<string, MiniParseSortType>("SortStringAscending", MiniParseSortType.StringAscending),
                    new KeyValuePair<string, MiniParseSortType>("SortStringDescending", MiniParseSortType.StringDescending),
                    new KeyValuePair<string, MiniParseSortType>("SortNumberAscending", MiniParseSortType.NumericAscending),
                    new KeyValuePair<string, MiniParseSortType>("SortNumberDescending", MiniParseSortType.NumericDescending)
                };
            }
            if (core == null)
            {
                core = new ACTWebSocketCore();
                core.pluginDirectory = pluginDirectory;
                core.overlaySkinDirectory = overlaySkinDirectory;
                NewUIWindow();
            }
            lblStatus = pluginStatusText;   // Hand the status label's reference to our local var
            pluginScreenSpace.Controls.Add(this);   // Add this UserControl to the tab ACT provides
            Dock = DockStyle.Fill; // Expand the UserControl to fill the tab's client space
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
            ActGlobals.oFormActMain.BeforeLogLineRead += oFormActMain_BeforeLogLineRead;
            ActGlobals.oFormActMain.OnLogLineRead += oFormActMain_OnLogLineRead;
            var s = ActGlobals.oFormActMain.ActPlugins;
            lblStatus.Text = "Plugin Started";
        }

        public void DeInitPlugin()
        {
            StopServer();
            // Unsubscribe from any events you listen to when exiting!
            ActGlobals.oFormActMain.BeforeLogLineRead -= oFormActMain_BeforeLogLineRead;
            ActGlobals.oFormActMain.OnLogLineRead -= oFormActMain_OnLogLineRead;

            SaveSettings();
            CloseAll();
            lblStatus.Text = "Plugin Exited";
        }

        #endregion

        void oFormActMain_AfterCombatAction(bool isImport, CombatActionEventArgs actionInfo)
        {
        }

        async void LoadSettings()
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

            // validate
            try
            {
                int p = Convert.ToInt16(port.Text);
            }
            catch (Exception e)
            {
                port.Text = "10501";
            }

            if (hostname.Text.Length == 0)
            {
                hostname.Text = "localhost";
            }

            UpdateList();
            if (File.Exists(pluginDirectory + "\\overlayconfig.json"))
            {
                //overlayWindows = new JObject();
                try
                {
                    JObject overlayWindowsLoaded = JObject.Parse(File.ReadAllText(pluginDirectory + "\\overlayconfig.json"));
                    IList<string> keys = overlayWindows.Properties().Select(p => p.Name).ToList();
                    IList<string> keysLoaded = overlayWindowsLoaded.Properties().Select(p => p.Name).ToList();
                    foreach (String title in keysLoaded)
                    {
                        overlayWindows[title] = overlayWindowsLoaded[title];
                        JObject o = (JObject)overlayWindows[title];
                        JToken val = null;
                        if (!o.TryGetValue("zoom", out val))
                        {
                            o["zoom"] = 1.0;
                        }
                        if (!o.TryGetValue("opacity", out val))
                        {
                            o["opacity"] = 1.0;
                        }
                        if (title == overlayFullscreenName)
                        {
                            using (var client = new HttpClient())
                            {
                                try
                                {
                                    var response = await client.GetAsync("http://localhost:5088/res");
                                    var responseString = await response.Content.ReadAsStringAsync();
                                    JObject o2 = JObject.Parse(responseString);
                                    overlayWindows[title] = o2;
                                }
                                catch (Exception e)
                                {

                                }
                                try
                                {
                                    string json = overlayWindows[title].ToString();
                                    var content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(json));
                                    var response = await client.PostAsync("http://localhost:5088/req", content);
                                    var responseString = await response.Content.ReadAsStringAsync();
                                }
                                catch (Exception e2)
                                {

                                }
                            }
                        }
                    }
                    foreach (string title in keysLoaded)
                    {
                        if (title == overlayFullscreenName)
                        {
                            continue;
                        }
                        IntPtr hwnd = Native.FindWindow(null, overlayWindowPrefix + title);
                        if (hwnd == null || hwnd.ToInt64() == 0)
                        {
                            if (NewOverlayWindow((JObject)overlayWindows[title]))
                            {
                                listBox2.Items.Add(title);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Parsing Error", "ActWebSocket");
                }
                //                = File.ReadAllText(Environment.CurrentDirectory + "\\overlayconfig.json");
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

            IList<string> keys = overlayWindows.Properties().Select(p => p.Name).ToList();
            foreach (string title in keys)
            {
                IntPtr hwnd = Native.FindWindow(null, overlayWindowPrefix + title);
                if (hwnd == null || hwnd.ToInt64() == 0)
                {
                }
                else
                {
                    Native.RECT rect = new Native.RECT();
                    if(Native.GetWindowRect(hwnd, out rect))
                    {
                        overlayWindows[title]["x"] = rect.Left;
                        overlayWindows[title]["y"] = rect.Top;
                        overlayWindows[title]["width"] = rect.Right - rect.Left;
                        overlayWindows[title]["height"] = rect.Bottom - rect.Top;
                    }
                }
            }
            File.WriteAllText(pluginDirectory+ "\\overlayconfig.json", overlayWindows.ToString());
        }

        void SaveSettingXML(string url, JObject o)
        {
            XmlDocument doc = new XmlDocument();
            // Root node : Overlays
            // Child node rule : Overlays/Overlay
            // Child node settings : Overlays/Overlay/*
            string emptySetting = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Overlays></Overlays>";

            if (File.Exists(Environment.CurrentDirectory + "\\overlayconfig.xml"))
                doc.Load(Environment.CurrentDirectory + "\\overlayconfig.xml");
            else
                doc.LoadXml(emptySetting);

            XmlElement setting = null;

            foreach(XmlElement xe in doc.SelectNodes("/Overlays/Overlay"))
            {
                if(xe.SelectSingleNode("/Overlay/Url").InnerText == url)
                {
                    setting = xe as XmlElement;
                    break;
                }
            }

            string[] objects = 
                { "Url", "x", "y", "width", "height", "useDragFilter", "useDragMove", "NoActive", "Transparent", "hide"};

            if(setting == null)
            {
                setting = doc.CreateElement("Overlay");
                foreach(string s in objects)
                {
                    XmlElement elem = doc.CreateElement(s);
                    if(s == "Url")
                    {
                        elem.InnerText = url;
                    }
                    else
                    {
                        elem.InnerText = o[s].ToString();
                    }
                    setting.AppendChild(elem);
                }
                doc.AppendChild(setting);
            }
            else
            {
                foreach(string s in objects)
                {
                    if (s == "Url") continue;
                    setting.SelectSingleNode("/Overlay/" + s).InnerText = o[s].ToString();
                }
            }

            doc.Save(Environment.CurrentDirectory + "\\overlayconfig.xml");
        }

        JObject LoadSettingXml(string url)
        {
            XmlDocument doc = new XmlDocument();
            JObject o = new JObject();
            // default values
            o["useDragFilter"] = true;
            o["useDragMove"] = true;
            o["hide"] = false;
            o["width"] = 100;
            o["height"] = 100;
            o["x"] = 0;
            o["y"] = 0;
            o["Transparent"] = true;
            o["NoActivate"] = true;


            string[] objects =
                { "x", "y", "width", "height", "useDragFilter", "useDragMove", "NoActive", "Transparent", "hide"};

            if (File.Exists(Environment.CurrentDirectory + "\\overlayconfig.xml"))
            {
                doc.Load(Environment.CurrentDirectory + "\\overlayconfig.xml");
                foreach(XmlElement xe in doc.SelectNodes("/Overlays/Overlay"))
                {
                    if(xe.SelectSingleNode("/Overlay/Url").InnerText == url)
                    {
                        foreach (string s in objects)
                        {
                            if(s == "x" || s == "y" || s == "width" || s == "height")
                            {
                                o[s] = int.Parse(xe.SelectSingleNode("/Overlay/" + s).InnerText);
                            }
                            else
                            {
                                o[s] = (xe.SelectSingleNode("/Overlay/" + s).InnerText.ToLower() == "true" ? true : false);
                            }
                        }
                        break;
                    }
                }
            }

            return o;
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
            try
            {
                core.StartServer(localhostOnly.Checked ? "127.0.0.1" : "0.0.0.0", Convert.ToInt16(port.Text), hostname.Text.Trim());
                localhostOnly.Enabled = false;
                port.Enabled = false;
                hostname.Enabled = false;
                buttonOn.Enabled = false;
                buttonOff.Enabled = true;
                randomURL.Enabled = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                core.StopServer();
            }
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

        string overlaySkinDirectory { get; set; }
        string pluginDirectory = "";
        public void SetSkinDir(string path)
        {
            overlaySkinDirectory = path;
        }

        public string GetSkinDir()
        {
            return overlaySkinDirectory;
        }

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

        public void CloseAll()
        {
            UpdateList(false);
            List<string> full_titles = Native.SearchForWindow(overlayWindowPrefix);
            for (int i=0;i< full_titles.Count;++i)
            {
                string title = full_titles[i];
                IntPtr hwnd = Native.FindWindow(null, title);
                if (hwnd == null || hwnd.ToInt64() == 0)
                {
                }
                else
                {
                    Native.SendMessage(hwnd, 0x0400 + 1, new IntPtr(0x08), new IntPtr(0x08));
                    Native.CloseWindow(hwnd);
                }
            }
        }

        static public string overlayFullscreenName = "FullScreen_Overlay";

        public void UpdateList(bool updateInfo = true)
        {
            listBox1.Items.Clear();
            foreach (string file in Directory.EnumerateFiles(overlaySkinDirectory, "*.html", SearchOption.AllDirectories))
            {
                listBox1.Items.Add(Utility.GetRelativePath(file, overlaySkinDirectory));
            }
            List<string> titles = Native.SearchForWindow(overlayWindowPrefix);
            listBox2.Items.Clear();
            listBox2.Sorted = true;

            // Fullscreen Overlay..
            //titles.Add(overlayWindowPrefix+overlayFullscreenName);
            foreach (string fulltitle in titles)
            {
                string title = fulltitle.Substring(overlayWindowPrefix.Length);
                listBox2.Items.Add(title);

                if (updateInfo)
                {
                    IList<string> keys = overlayWindows.Properties().Select(p => p.Name).ToList();
                    if (!keys.Contains(title))
                    {
                        overlayWindows[title] = new JObject();
                        IntPtr hwnd = Native.FindWindow(null, overlayWindowPrefix + title);
                        if (hwnd == null || hwnd.ToInt64() == 0)
                        {
                            overlayWindows[title]["Transparent"] = true;
                            overlayWindows[title]["NoActivate"] = true;
                        }
                        else
                        {
                            overlayWindows[title]["Transparent"] = (Native.GetWindowLongFlag(hwnd, new IntPtr(Native.WS_EX_TRANSPARENT)).ToInt64() > 0);
                            overlayWindows[title]["NoActivate"] = (Native.GetWindowLongFlag(hwnd, new IntPtr(Native.WS_EX_NOACTIVATE)).ToInt64() > 0);
                        }
                        overlayWindows[title]["hide"] = false;
                        overlayWindows[title]["useDragFilter"] = true;
                        overlayWindows[title]["useDragMove"] = true;
                        overlayWindows[title]["useResizeGrip"] = true;
                        overlayWindows[title]["opacity"] = 1.0;
                        overlayWindows[title]["zoom"] = 1.0;
                        overlayWindows[title]["title"] = title;
                    }
                    else
                    {
                        IntPtr hwnd = Native.FindWindow(null, overlayWindowPrefix + title);
                        if (hwnd == null || hwnd.ToInt64() == 0)
                        {
                            overlayWindows[title]["Transparent"] = true;
                            overlayWindows[title]["NoActivate"] = true;
                        }
                        else
                        {
                            overlayWindows[title]["Transparent"] = (Native.GetWindowLongFlag(hwnd, new IntPtr(Native.WS_EX_TRANSPARENT)).ToInt64() > 0);
                            overlayWindows[title]["NoActivate"] = (Native.GetWindowLongFlag(hwnd, new IntPtr(Native.WS_EX_NOACTIVATE)).ToInt64() > 0);
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateList();
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                string url = "";
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
                string param = url + Uri.EscapeDataString(listBox1.Items[listBox1.SelectedIndex].ToString());
                param = param.Replace("%5C", "/");
                string title = Guid.NewGuid().ToString();
                if(NewOverlayWindow(param, title))
                {
                    listBox2.Items.Add(title);
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                string url = "";
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
                string param = url + Uri.EscapeDataString(listBox1.Items[listBox1.SelectedIndex].ToString());
                param = param.Replace("%5C", "/");
                string title = Guid.NewGuid().ToString();
                if (NewOverlayWindow(param, title))
                {
                    this.listBox2.Items.Add(title);
                }
            }
        }
        private bool NewUIWindow()
        {
            {
                IntPtr hwnd = Native.FindWindow(null, "ui_title");
                Native.CloseWindow(hwnd);
            }
            string overlayPath = pluginDirectory + "/overlay/overlay_proc.exe";
            if (File.Exists(overlayPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                string uri = new Uri(pluginDirectory + "\\WS_SKIN\\MainForm.html").AbsoluteUri;
                startInfo.FileName = overlayPath;
                JObject o = new JObject();
                o["Transparent"] = false;
                o["NoActivate"] = false;
                o["hide"] = false;
                o["useDragFilter"] = true;
                o["useDragMove"] = true;
                o["useResizeGrip"] = true;
                o["opacity"] = 1.0;
                o["zoom"] = 1.0;
                o["url"] = uri;
                o["fps"] = 30.0;
                o["title"] = "ui_title";


                string json = json = o.ToString();
                startInfo.Arguments = Utility.Base64Encoding(json) + " 9992 " + Handle.ToString();
                var p = Process.Start(startInfo);
                //p.WaitForInputIdle(1000); //wait for the window to be ready for input;
                //p.Refresh();
                //IntPtr hwnd = Native.FindWindow(null, "ui_title");
                //IntPtr ghwnd = Native.GetParent(hwnd);
                //Native.SetWindowLong(hwnd, Native.GWL_STYLE, Native.WS_CHILD);
                //Native.SetWindowLong(hwnd, Native.GWL_EXSTYLE, 0);
                //Native.SetParent(hwnd, this.Handle);
                //SizeChanged += new EventHandler(UpdateUISize);
                //UpdateUISize(new Object(), new EventArgs());
                return true;
            }
            return false;
        }
        void UpdateUISize(object sender, EventArgs e)
        {
            IntPtr hwnd = Native.FindWindow(null, "ui_title");
            //Native.SetParent(hwnd, this.Handle);
            Native.RECT prect = new Native.RECT();
            Native.GetWindowRect(Handle, out prect);
            JObject o = new JObject();
            o["x"] = prect.Left;
            o["y"] = prect.Top;
            o["width"] = prect.Right - prect.Left;
            o["height"] = prect.Bottom - prect.Top;
            o["hide"] = false;
            String json = o.ToString();
            Native.SendMessageToWindow(hwnd, 1, json);
        }

        private bool NewOverlayWindow(string url, string title)
        {
            string overlayPath = pluginDirectory + "/overlay/overlay_proc.exe";
            if (File.Exists(overlayPath) && listBox1.SelectedIndex >= 0)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = overlayPath;
                JObject o = new JObject();
                o["Transparent"] = false;
                o["NoActivate"] = false;
                o["hide"] = false;
                o["useDragFilter"] = true;
                o["useDragMove"] = true;
                o["useResizeGrip"] = true;
                o["opacity"] = 1.0;
                o["zoom"] = 1.0;
                o["url"] = url;
                o["title"] = title;
                o["fps"] = 30.0;
                //o["x"] = Convert.ToInt32(x.Text);
                //o["y"] = Convert.ToInt32(y.Text);
                //o["width"] = Convert.ToInt32(width.Text) <= 0 ? 1 : Convert.ToInt32(width.Text);
                //o["height"] = Convert.ToInt32(height.Text) <= 0 ? 1 : Convert.ToInt32(height.Text);
                //o["width"] = 100;
                //o["height"] = 100;

                return NewOverlayWindow(o);
            }
            return false;
        }

        private bool NewOverlayWindow(JObject obj)
        {
            string overlayPath = pluginDirectory + "/overlay/overlay_proc.exe";
            if (File.Exists(overlayPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = overlayPath;
                JObject o = (JObject)obj.DeepClone();
                string title = o["title"].Value<string>();
                o["title"] = overlayWindowPrefix + o["title"];

                if (overlayWindows[title] == null)
                    overlayWindows[title] = obj;

                string json = json = o.ToString();
                startInfo.Arguments = Utility.Base64Encoding(json);
                Process.Start(startInfo);
                return true;
            }
            return false;
        }

        private void copyURL_Click(object sender, EventArgs e)
        {
            string url = "";
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
            if (listBox1.SelectedIndex >= 0)
            {
                string fullURL = url + Uri.EscapeDataString(listBox1.Items[listBox1.SelectedIndex].ToString());
                fullURL = fullURL.Replace("%5C", "/");
                Clipboard.SetText(fullURL);
            }
            else
            {
                Clipboard.SetText(url);
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            UpdateList();
        }

        private async void update_overlayWindow(object sender, EventArgs e)
        {
            checkBox5.Enabled = checkBox4.Checked;
            if (listBox2.SelectedIndex >= 0)
            {
                string title = listBox2.Items[listBox2.SelectedIndex].ToString();
                IntPtr hwnd = Native.FindWindow(null, overlayWindowPrefix + title);
                if ((hwnd == null || hwnd.ToInt64() == 0) && title != overlayFullscreenName)
                {
                    UpdateList();
                }
                else
                {
                    JObject o = new JObject();
                    o["Transparent"] = checkBox1.Checked;
                    o["NoActivate"] = checkBox2.Checked;
                    o["hide"] = checkBox3.Checked;
                    o["useDragFilter"] = checkBox4.Checked;
                    o["useDragMove"] = checkBox4.Checked && checkBox5.Checked;
                    o["useResizeGrip"] = checkBox6.Checked;
                    o["opacity"] = (double)opacity.Value / opacity.Maximum;
                    o["zoom"] = zoom.Value / 100.0;
                    o["fps"] = (double)fps.Value;
                    checkBox5.Enabled = checkBox4.Checked;
                    string json = o.ToString();
                    if (title == overlayFullscreenName)
                    {
                        using (var client = new HttpClient())
                        {
                            try
                            {
                                var content = new ByteArrayContent(Encoding.UTF8.GetBytes(json));
                                var response = await client.PostAsync("http://localhost:5088/req", content);
                                var responseString = await response.Content.ReadAsStringAsync();
                            }
                            catch(Exception e2)
                            {

                            }
                        }
                    }
                    else
                    {
                        Native.SendMessageToWindow(hwnd, 1, json);
                    }

                    IList<string> keys = o.Properties().Select(p => p.Name).ToList();
                    foreach (string key in keys)
                    {
                        overlayWindows[title][key] = o[key].DeepClone();
                    }
                }
            }
        }

        private async void update_overlayWindowPosition(object sender, EventArgs e)
        {
            if (updateUIWithoutMove)
                return;
            if (listBox2.SelectedIndex >= 0)
            {
                string title = listBox2.Items[listBox2.SelectedIndex].ToString();
                IntPtr hwnd = Native.FindWindow(null, overlayWindowPrefix + title);
                if ((hwnd == null || hwnd.ToInt64() == 0) && title != overlayFullscreenName)
                {
                    UpdateList();
                }
                else
                {
                    JObject o = new JObject();
                    try
                    {
                        o["x"] = Convert.ToInt32(x.Text);
                    }
                    catch (Exception e2)
                    {
                    }
                    try
                    {
                        o["y"] = Convert.ToInt32(y.Text);
                    }
                    catch (Exception e2)
                    {
                    }
                    try
                    {
                        o["width"] = Convert.ToInt32(width.Text);
                    }
                    catch (Exception e2)
                    {
                    }
                    try
                    {
                        o["height"] = Convert.ToInt32(height.Text);
                    }
                    catch (Exception e2)
                    {
                    }
                    string json = o.ToString();
                    if (title == overlayFullscreenName)
                    {
                        using (var client = new HttpClient())
                        {
                            try
                            {
                                var content = new ByteArrayContent(Encoding.UTF8.GetBytes(json));
                                var response = await client.PostAsync("http://localhost:5088/req", content);
                                var responseString = await response.Content.ReadAsStringAsync();
                            }
                            catch (Exception e2)
                            {

                            }
                        }
                    }
                    else
                    {
                        Native.SendMessageToWindow(hwnd, 1, json);
                    }

                    IList<string> keys = o.Properties().Select(p => p.Name).ToList();
                    foreach (string key in keys)
                    {
                        overlayWindows[title][key] = o[key].DeepClone();
                    }
                }
            }
        }

        bool updateUIWithoutMove = false;
        private async void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex >= 0)
            {
                string title = listBox2.Items[listBox2.SelectedIndex].ToString();
                IntPtr hwnd = Native.FindWindow(null, overlayWindowPrefix + title);
                if ((hwnd == null || hwnd.ToInt64() == 0) && title != overlayFullscreenName)
                {
                    //this.listBox2.Items.RemoveAt(this.listBox2.SelectedIndex);
                    UpdateList();
                }
                else
                {
                    overlayTitle.Enabled = title != overlayFullscreenName;
                    close.Enabled = title != overlayFullscreenName;

                    overlayTitle.Text = title;
                    IList<string> keys = overlayWindows.Properties().Select(p => p.Name).ToList();
                    if (title == overlayFullscreenName)
                    {
                        using (var client = new HttpClient())
                        {
                            try
                            {
                                var response = await client.GetAsync("http://localhost:5088/res");
                                var responseString = await response.Content.ReadAsStringAsync();
                                JObject o2 = JObject.Parse(responseString);
                                overlayWindows[title] = o2;
                            }
                            catch (Exception e2)
                            {
                                return;
                            }
                        }
                    }

                    if (keys.Contains(title))
                    {
                        checkBox1.Checked = overlayWindows[title].Value<bool>("Transparent");
                        checkBox2.Checked = overlayWindows[title].Value<bool>("NoActivate");
                        checkBox3.Checked = overlayWindows[title].Value<bool>("hide");
                        checkBox4.Checked = overlayWindows[title].Value<bool>("useDragFilter");
                        checkBox5.Checked = overlayWindows[title].Value<bool>("useDragMove");
                        checkBox6.Checked = overlayWindows[title].Value<bool>("useResizeGrip");
                        url.Text = overlayWindows[title].Value<string>("url");
                        try
                        {
                            opacity.Value = (int)(overlayWindows[title].Value<double>("opacity") * opacity.Maximum);
                        }
                        catch (Exception e2) { }
                        finally { }
                        try
                        {
                            zoom.Value = (int)(overlayWindows[title].Value<double>("zoom") * 100);
                        }
                        catch (Exception e2) { }
                        finally { }
                        try
                        {
                            fps.Value = (int)overlayWindows[title].Value<double>("fps");
                        }
                        catch (Exception e2) { }
                        finally { }

                        Native.RECT rect = new Native.RECT();
                        Native.GetWindowRect(hwnd, out rect);

                        updateUIWithoutMove = true;
                        x.Text = rect.Left.ToString();
                        y.Text = rect.Top.ToString();
                        width.Text = (rect.Right - rect.Left).ToString();
                        height.Text = (rect.Bottom - rect.Top).ToString();
                        updateUIWithoutMove = false;

                        //x.Text = overlayWindows[title].Value<Int32>("x").ToString();
                        //y.Text = overlayWindows[title].Value<Int32>("y").ToString();
                        //width.Text = overlayWindows[title].Value<Int32>("width").ToString();
                        //height.Text = overlayWindows[title].Value<Int32>("height").ToString();

                        if (title == overlayFullscreenName)
                        {
                        }
                        else
                        {
                            // get windows rect
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex >= 0)
            {
                JObject o = new JObject();
                string title = listBox2.Items[listBox2.SelectedIndex].ToString();
                o["title"] = title;
                core.APIOverlayWindowClose(o);
                //IntPtr hwnd = Native.FindWindow(null, overlayWindowPrefix + title);
                //if (hwnd == null || hwnd.ToInt64() == 0)
                //{
                //    //this.listBox2.Items.RemoveAt(this.listBox2.SelectedIndex);
                //    UpdateList();
                //}
                //else
                //{
                //    Native.SendMessage(hwnd, 0x0400 + 1, new IntPtr(0x08), new IntPtr(0x08));
                //    Native.CloseWindow(hwnd);

                //    overlayWindows.Remove(title);
                //    this.listBox2.Items.RemoveAt(this.listBox2.SelectedIndex);
                //}
            }
        }

        private void overlayTitle_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (listBox2.SelectedIndex >= 0)
                {
                    string title = listBox2.Items[listBox2.SelectedIndex].ToString();
                    IntPtr hwnd = Native.FindWindow(null, overlayWindowPrefix + title);
                    if (hwnd == null || hwnd.ToInt64() == 0)
                    {
                        UpdateList();
                    }
                    else
                    {
                        if (title.CompareTo(overlayTitle.Text) != 0)
                        {
                            JObject o = new JObject();
                            o["title"] = overlayWindowPrefix + overlayTitle.Text;
                            string json = o.ToString();
                            Native.SendMessageToWindow(hwnd, 1, json);
                            overlayWindows[overlayTitle.Text] = overlayWindows[title].DeepClone();
                            overlayWindows[overlayTitle.Text]["title"] = overlayTitle.Text;
                            overlayWindows.Remove(title);
                            UpdateList();
                        }
                    }
                }
            }
        }

        private void overlayTitle_Leave(object sender, EventArgs e)
        {
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex >= 0)
            {
                string title = listBox2.Items[listBox2.SelectedIndex].ToString();
                IntPtr hwnd = Native.FindWindow(null, overlayWindowPrefix + title);
                if ((hwnd == null || hwnd.ToInt64() == 0) && title != overlayFullscreenName)
                {
                    UpdateList();
                }
                else
                {
                    JObject o = new JObject();
                    o["url"] = url.Text;
                    string json = o.ToString();
                    if (title == overlayFullscreenName)
                    {
                        using (var client = new HttpClient())
                        {
                            var content = new ByteArrayContent(Encoding.UTF8.GetBytes(json));
                            var response = await client.PostAsync("http://localhost:5088/req", content);
                            var responseString = await response.Content.ReadAsStringAsync();
                        }
                    }
                    else
                    {
                        Native.SendMessageToWindow(hwnd, 1, json);
                    }

                    IList<string> keys = o.Properties().Select(p => p.Name).ToList();
                    foreach (string key in keys)
                    {
                        overlayWindows[title][key] = o[key].DeepClone();
                    }
                }
            }
        }

        private void digitOnly_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private async void x_Enter(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex >= 0)
            {
                string title = listBox2.Items[listBox2.SelectedIndex].ToString();
                IntPtr hwnd = Native.FindWindow(null, overlayWindowPrefix + title);
                if ((hwnd == null || hwnd.ToInt64() == 0) && title != overlayFullscreenName)
                {
                    //this.listBox2.Items.RemoveAt(this.listBox2.SelectedIndex);
                    UpdateList();
                }
                else
                {
                    overlayTitle.Enabled = title != overlayFullscreenName;
                    close.Enabled = title != overlayFullscreenName;

                    overlayTitle.Text = title;
                    IList<string> keys = overlayWindows.Properties().Select(p => p.Name).ToList();
                    if (title == overlayFullscreenName)
                    {
                        JObject o = (JObject)overlayWindows[title];
                        using (var client = new HttpClient())
                        {
                            try
                            {
                                var response = await client.GetAsync("http://localhost:5088/res");
                                var responseString = await response.Content.ReadAsStringAsync();
                                JObject o2 = JObject.Parse(responseString);
                                overlayWindows[title] = o2;
                            }
                            catch (Exception e2)
                            {
                                return;
                            }
                        }
                    }

                    if (keys.Contains(title))
                    {
                        Native.RECT rect = new Native.RECT();
                        Native.GetWindowRect(hwnd, out rect);

                        updateUIWithoutMove = true;
                        x.Text = rect.Left.ToString();
                        y.Text = rect.Top.ToString();
                        width.Text = (rect.Right - rect.Left).ToString();
                        height.Text = (rect.Bottom - rect.Top).ToString();
                        updateUIWithoutMove = false;

                        //x.Text = overlayWindows[title].Value<Int32>("x").ToString();
                        //y.Text = overlayWindows[title].Value<Int32>("y").ToString();
                        //width.Text = overlayWindows[title].Value<Int32>("width").ToString();
                        //height.Text = overlayWindows[title].Value<Int32>("height").ToString();

                        if (title == overlayFullscreenName)
                        {
                        }
                        else
                        {
                            // get windows rect
                        }
                    }
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void url_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
