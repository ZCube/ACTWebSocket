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
    using System.Linq;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
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
        private TableLayoutPanel tableLayoutPanel1;
        private CheckBox localhostOnly;
        private Label label1;
        private Label label2;
        private TextBox port;
        private Label server;
        private Label label3;
        private CheckBox autostart;
        private TabControl tabControl1;
        private TabPage tabMiniParse;
        private TableLayoutPanel tableLayoutPanel3;
        private TabPage tabLogLineRead;
        private TableLayoutPanel tableLayoutPanel2;
        private Button buttonOff;
        private Button buttonOn;
        private TextBox hostname;
        private Label label4;
        private Label label5;
        private Label label6;
        private TextBox MiniParseSortKey;
        private ComboBox sortType;
        private CheckBox MiniParseUse;
        private Label label7;
        private TableLayoutPanel tableLayoutPanel4;
        private CheckBox LogLineReadUse;
        private Label label8;
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.port = new System.Windows.Forms.TextBox();
            this.server = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.autostart = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabMiniParse = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.MiniParseSortKey = new System.Windows.Forms.TextBox();
            this.sortType = new System.Windows.Forms.ComboBox();
            this.tabLogLineRead = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonOff = new System.Windows.Forms.Button();
            this.buttonOn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.localhostOnly = new System.Windows.Forms.CheckBox();
            this.hostname = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.MiniParseUse = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.LogLineReadUse = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabMiniParse.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tabLogLineRead.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.94228F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 82.05772F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.port, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.server, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.autostart, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.localhostOnly, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.hostname, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 89.97996F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.02004F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(794, 354);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 22);
            this.label2.TabIndex = 2;
            this.label2.Text = "port";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // port
            // 
            this.port.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.port.Location = new System.Drawing.Point(145, 67);
            this.port.Name = "port";
            this.port.Size = new System.Drawing.Size(646, 21);
            this.port.TabIndex = 3;
            this.port.Text = "10501";
            // 
            // server
            // 
            this.server.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.server.AutoSize = true;
            this.server.Location = new System.Drawing.Point(3, 313);
            this.server.Name = "server";
            this.server.Size = new System.Drawing.Size(136, 41);
            this.server.TabIndex = 5;
            this.server.Text = "server";
            this.server.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 22);
            this.label3.TabIndex = 7;
            this.label3.Text = "auto start";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // autostart
            // 
            this.autostart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autostart.AutoSize = true;
            this.autostart.Checked = true;
            this.autostart.CheckState = System.Windows.Forms.CheckState.Checked;
            this.autostart.Location = new System.Drawing.Point(145, 3);
            this.autostart.Name = "autostart";
            this.autostart.Size = new System.Drawing.Size(646, 16);
            this.autostart.TabIndex = 8;
            this.autostart.Text = "auto start";
            this.autostart.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabMiniParse);
            this.tabControl1.Controls.Add(this.tabLogLineRead);
            this.tabControl1.Location = new System.Drawing.Point(145, 89);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(646, 199);
            this.tabControl1.TabIndex = 9;
            // 
            // tabMiniParse
            // 
            this.tabMiniParse.Controls.Add(this.tableLayoutPanel3);
            this.tabMiniParse.Location = new System.Drawing.Point(4, 22);
            this.tabMiniParse.Name = "tabMiniParse";
            this.tabMiniParse.Padding = new System.Windows.Forms.Padding(3);
            this.tabMiniParse.Size = new System.Drawing.Size(638, 173);
            this.tabMiniParse.TabIndex = 0;
            this.tabMiniParse.Text = "MiniParse";
            this.tabMiniParse.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.93671F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.06329F));
            this.tableLayoutPanel3.Controls.Add(this.MiniParseUse, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label5, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label6, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.MiniParseSortKey, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.sortType, 1, 2);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.Size = new System.Drawing.Size(626, 161);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(118, 30);
            this.label5.TabIndex = 0;
            this.label5.Text = "Sort Key";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 30);
            this.label6.TabIndex = 1;
            this.label6.Text = "Sort Type";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MiniParseSortKey
            // 
            this.MiniParseSortKey.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MiniParseSortKey.Location = new System.Drawing.Point(127, 33);
            this.MiniParseSortKey.Name = "MiniParseSortKey";
            this.MiniParseSortKey.Size = new System.Drawing.Size(496, 21);
            this.MiniParseSortKey.TabIndex = 2;
            this.MiniParseSortKey.Text = "encdps";
            this.MiniParseSortKey.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // sortType
            // 
            this.sortType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sortType.FormattingEnabled = true;
            this.sortType.Location = new System.Drawing.Point(127, 63);
            this.sortType.Name = "sortType";
            this.sortType.Size = new System.Drawing.Size(496, 20);
            this.sortType.TabIndex = 3;
            this.sortType.SelectedIndexChanged += new System.EventHandler(this.MiniParseSortType_SelectedIndexChanged);
            // 
            // tabLogLineRead
            // 
            this.tabLogLineRead.Controls.Add(this.tableLayoutPanel4);
            this.tabLogLineRead.Location = new System.Drawing.Point(4, 22);
            this.tabLogLineRead.Name = "tabLogLineRead";
            this.tabLogLineRead.Padding = new System.Windows.Forms.Padding(3);
            this.tabLogLineRead.Size = new System.Drawing.Size(638, 173);
            this.tabLogLineRead.TabIndex = 1;
            this.tabLogLineRead.Text = "LogLineRead";
            this.tabLogLineRead.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.buttonOff, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.buttonOn, 0, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(145, 316);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(646, 35);
            this.tableLayoutPanel2.TabIndex = 10;
            // 
            // buttonOff
            // 
            this.buttonOff.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOff.AutoSize = true;
            this.buttonOff.Location = new System.Drawing.Point(326, 3);
            this.buttonOff.Name = "buttonOff";
            this.buttonOff.Size = new System.Drawing.Size(317, 29);
            this.buttonOff.TabIndex = 1;
            this.buttonOff.Text = "Off";
            this.buttonOff.UseVisualStyleBackColor = true;
            this.buttonOff.Click += new System.EventHandler(this.buttonOff_Click);
            // 
            // buttonOn
            // 
            this.buttonOn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOn.AutoSize = true;
            this.buttonOn.Location = new System.Drawing.Point(3, 3);
            this.buttonOn.Name = "buttonOn";
            this.buttonOn.Size = new System.Drawing.Size(317, 29);
            this.buttonOn.TabIndex = 0;
            this.buttonOn.Text = "On";
            this.buttonOn.UseVisualStyleBackColor = true;
            this.buttonOn.Click += new System.EventHandler(this.buttonOn_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "localhost";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // localhostOnly
            // 
            this.localhostOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.localhostOnly.AutoSize = true;
            this.localhostOnly.Checked = true;
            this.localhostOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.localhostOnly.Location = new System.Drawing.Point(145, 25);
            this.localhostOnly.Name = "localhostOnly";
            this.localhostOnly.Size = new System.Drawing.Size(646, 14);
            this.localhostOnly.TabIndex = 0;
            this.localhostOnly.Text = "localhost Only";
            this.localhostOnly.UseVisualStyleBackColor = true;
            // 
            // hostname
            // 
            this.hostname.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hostname.Location = new System.Drawing.Point(145, 45);
            this.hostname.Name = "hostname";
            this.hostname.Size = new System.Drawing.Size(646, 21);
            this.hostname.TabIndex = 11;
            this.hostname.Text = "localhost";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 22);
            this.label4.TabIndex = 12;
            this.label4.Text = "hostname";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(118, 30);
            this.label7.TabIndex = 4;
            this.label7.Text = "Use MiniParse";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MiniParseUse
            // 
            this.MiniParseUse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MiniParseUse.AutoSize = true;
            this.MiniParseUse.Checked = true;
            this.MiniParseUse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.MiniParseUse.Location = new System.Drawing.Point(127, 3);
            this.MiniParseUse.Name = "MiniParseUse";
            this.MiniParseUse.Size = new System.Drawing.Size(496, 24);
            this.MiniParseUse.TabIndex = 13;
            this.MiniParseUse.Text = "Use";
            this.MiniParseUse.UseVisualStyleBackColor = true;
            this.MiniParseUse.CheckedChanged += new System.EventHandler(this.MiniParseUse_CheckedChanged);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.93671F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80.06329F));
            this.tableLayoutPanel4.Controls.Add(this.LogLineReadUse, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(626, 161);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // LogLineReadUse
            // 
            this.LogLineReadUse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LogLineReadUse.AutoSize = true;
            this.LogLineReadUse.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.LogLineReadUse.Location = new System.Drawing.Point(127, 3);
            this.LogLineReadUse.Name = "LogLineReadUse";
            this.LogLineReadUse.Size = new System.Drawing.Size(496, 24);
            this.LogLineReadUse.TabIndex = 13;
            this.LogLineReadUse.Text = "Use";
            this.LogLineReadUse.UseVisualStyleBackColor = true;
            this.LogLineReadUse.CheckedChanged += new System.EventHandler(this.LogLineReadUse_CheckedChanged);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(118, 30);
            this.label8.TabIndex = 4;
            this.label8.Text = "Use LogLineRead";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ACTWebSocketMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ACTWebSocketMain";
            this.Size = new System.Drawing.Size(800, 354);
            this.Load += new System.EventHandler(this.ACTWebSocket_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabMiniParse.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tabLogLineRead.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		
		#endregion
		public ACTWebSocketMain()
		{
			InitializeComponent();
        }

        ~ACTWebSocketMain()
        {
        }

		Label lblStatus;	// The status label that appears in ACT's Plugin tab
		string settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\PluginSample.config.xml");
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
            if(core == null)
            {
                core = new ACTWebSocketCore();
                core.pluginDirectory = pluginDirectory;
            }
            lblStatus = pluginStatusText;	// Hand the status label's reference to our local var
			pluginScreenSpace.Controls.Add(this);	// Add this UserControl to the tab ACT provides
			this.Dock = DockStyle.Fill;	// Expand the UserControl to fill the tab's client space
			xmlSettings = new SettingsSerializer(this);	// Create a new settings serializer and pass it this instance
			LoadSettings();

            sortType.DataSource = sortTypeDict;

            if (core != null)
            {
                core.Filters["/BeforeLogLineRead"] = LogLineReadUse.Checked;
                core.Filters["/MiniParse"] = MiniParseUse.Checked;
            }
            if (autostart.Checked)
            {
                core.StartServer(localhostOnly.Checked ? "127.0.0.1" : "0.0.0.0", Convert.ToInt16(port.Text), hostname.Text.Trim());
                localhostOnly.Enabled = false;
                port.Enabled = false;
                hostname.Enabled = false;
                buttonOn.Enabled = false;
                buttonOff.Enabled = true;
            }
            else {
                localhostOnly.Enabled = true;
                port.Enabled = true;
                hostname.Enabled = true;
                buttonOn.Enabled = true;
                buttonOff.Enabled = false;
            }
            // Create some sort of parsing event handler.  After the "+=" hit TAB twice and the code will be generated for you.
            ActGlobals.oFormActMain.BeforeLogLineRead += this.oFormActMain_OnLogLineRead;
            var s = ActGlobals.oFormActMain.ActPlugins;
            lblStatus.Text = "Plugin Started";
        }

		public void DeInitPlugin()
		{
            core.StopServer();
			// Unsubscribe from any events you listen to when exiting!
            ActGlobals.oFormActMain.BeforeLogLineRead -= this.oFormActMain_OnLogLineRead;
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
            xmlSettings.AddControlSetting(LogLineReadUse.Name, LogLineReadUse);

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
			xWriter.WriteStartElement("Config");	// <Config>
			xWriter.WriteStartElement("SettingsSerializer");	// <Config><SettingsSerializer>
			xmlSettings.ExportToXml(xWriter);	// Fill the SettingsSerializer XML
			xWriter.WriteEndElement();	// </SettingsSerializer>
			xWriter.WriteEndElement();	// </Config>
			xWriter.WriteEndDocument();	// Tie up loose ends (shouldn't be any)
			xWriter.Flush();	// Flush the file buffer to disk
			xWriter.Close();
		}

        private void ACTWebSocket_Load(object sender, EventArgs e)
        {
        }



        private void oFormActMain_OnLogLineRead(
            bool isImport,
            LogLineEventArgs logInfo)
        {
            core.Broadcast("/BeforeLogLineRead", logInfo.logLine);
        }


        private void buttonOn_Click(object sender, EventArgs e)
        {
            core.StartServer(localhostOnly.Checked ? "127.0.0.1" : "0.0.0.0", Convert.ToInt16(port.Text), hostname.Text.Trim());
            localhostOnly.Enabled = false;
            port.Enabled = false;
            hostname.Enabled = false;
            buttonOn.Enabled = false;
            buttonOff.Enabled = true;
        }

        private void buttonOff_Click(object sender, EventArgs e)
        {
            core.StopServer();
            localhostOnly.Enabled = true;
            port.Enabled = true;
            hostname.Enabled = true;
            buttonOn.Enabled = true;
            buttonOff.Enabled = false;
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
            if(core != null)
            {
                var config = core.Config;
                config.SortKey = MiniParseSortKey.Text.Trim();
                core.Config = config;
            }
        }

        private void MiniParseSortType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (core != null)
            {
                var config = core.Config;
                config.SortType = (MiniParseSortType)sortType.SelectedIndex;
                core.Config = config;
            }
        }

        private void LogLineReadUse_CheckedChanged(object sender, EventArgs e)
        {
            if(core != null)
            {
                core.Filters["/BeforeLogLineRead"] = LogLineReadUse.Checked;
            }
        }

        private void MiniParseUse_CheckedChanged(object sender, EventArgs e)
        {
            if (core != null)
            {
                core.Filters["/MiniParse"] = MiniParseUse.Checked;
            }
        }
    }
}
