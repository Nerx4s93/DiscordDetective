namespace DiscordDetective.GUI;

partial class FormMain
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
        components = new System.ComponentModel.Container();
        tabControlMain = new System.Windows.Forms.TabControl();
        tabPageProxy = new System.Windows.Forms.TabPage();
        button4 = new System.Windows.Forms.Button();
        button3 = new System.Windows.Forms.Button();
        button2 = new System.Windows.Forms.Button();
        button1 = new System.Windows.Forms.Button();
        proxyListView = new DiscordDetective.UI.ProxyListView();
        TabPageBots = new System.Windows.Forms.TabPage();
        listViewBots = new System.Windows.Forms.ListView();
        ContextMenuStripBots = new System.Windows.Forms.ContextMenuStrip(components);
        AddBotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        UpdateListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        UpdateBotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        DeleteBotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        tabPage1 = new System.Windows.Forms.TabPage();
        tabControlMain.SuspendLayout();
        tabPageProxy.SuspendLayout();
        TabPageBots.SuspendLayout();
        ContextMenuStripBots.SuspendLayout();
        SuspendLayout();
        // 
        // tabControlMain
        // 
        tabControlMain.Controls.Add(tabPageProxy);
        tabControlMain.Controls.Add(TabPageBots);
        tabControlMain.Controls.Add(tabPage1);
        tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
        tabControlMain.Location = new System.Drawing.Point(0, 0);
        tabControlMain.Name = "tabControlMain";
        tabControlMain.SelectedIndex = 0;
        tabControlMain.Size = new System.Drawing.Size(1456, 726);
        tabControlMain.TabIndex = 0;
        tabControlMain.TabStop = false;
        // 
        // tabPageProxy
        // 
        tabPageProxy.Controls.Add(button4);
        tabPageProxy.Controls.Add(button3);
        tabPageProxy.Controls.Add(button2);
        tabPageProxy.Controls.Add(button1);
        tabPageProxy.Controls.Add(proxyListView);
        tabPageProxy.Location = new System.Drawing.Point(4, 32);
        tabPageProxy.Name = "tabPageProxy";
        tabPageProxy.Size = new System.Drawing.Size(1448, 690);
        tabPageProxy.TabIndex = 1;
        tabPageProxy.Text = "Прокси";
        tabPageProxy.UseVisualStyleBackColor = true;
        // 
        // button4
        // 
        button4.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        button4.Enabled = false;
        button4.Location = new System.Drawing.Point(1242, 636);
        button4.Name = "button4";
        button4.Size = new System.Drawing.Size(194, 52);
        button4.TabIndex = 4;
        button4.Text = "Удалить";
        button4.UseVisualStyleBackColor = true;
        // 
        // button3
        // 
        button3.Enabled = false;
        button3.Location = new System.Drawing.Point(1242, 119);
        button3.Name = "button3";
        button3.Size = new System.Drawing.Size(194, 52);
        button3.TabIndex = 3;
        button3.Text = "Изменить тип";
        button3.UseVisualStyleBackColor = true;
        // 
        // button2
        // 
        button2.Enabled = false;
        button2.Location = new System.Drawing.Point(1242, 61);
        button2.Name = "button2";
        button2.Size = new System.Drawing.Size(194, 52);
        button2.TabIndex = 2;
        button2.Text = "Автопродление";
        button2.UseVisualStyleBackColor = true;
        // 
        // button1
        // 
        button1.Enabled = false;
        button1.Location = new System.Drawing.Point(1242, 3);
        button1.Name = "button1";
        button1.Size = new System.Drawing.Size(194, 52);
        button1.TabIndex = 1;
        button1.Text = "Продлить";
        button1.UseVisualStyleBackColor = true;
        // 
        // proxyListView
        // 
        proxyListView.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        proxyListView.BackColor = System.Drawing.Color.White;
        proxyListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        proxyListView.Font = new System.Drawing.Font("Arial", 10F);
        proxyListView.Location = new System.Drawing.Point(0, 0);
        proxyListView.MaximumSize = new System.Drawing.Size(1236, 0);
        proxyListView.MinimumSize = new System.Drawing.Size(1236, 500);
        proxyListView.Name = "proxyListView";
        proxyListView.Padding = new System.Windows.Forms.Padding(1);
        proxyListView.Size = new System.Drawing.Size(1236, 696);
        proxyListView.TabIndex = 0;
        // 
        // TabPageBots
        // 
        TabPageBots.Controls.Add(listViewBots);
        TabPageBots.Location = new System.Drawing.Point(4, 32);
        TabPageBots.Name = "TabPageBots";
        TabPageBots.Padding = new System.Windows.Forms.Padding(3);
        TabPageBots.Size = new System.Drawing.Size(1448, 690);
        TabPageBots.TabIndex = 0;
        TabPageBots.Text = "Боты";
        TabPageBots.UseVisualStyleBackColor = true;
        // 
        // listViewBots
        // 
        listViewBots.ContextMenuStrip = ContextMenuStripBots;
        listViewBots.Dock = System.Windows.Forms.DockStyle.Fill;
        listViewBots.Location = new System.Drawing.Point(3, 3);
        listViewBots.Name = "listViewBots";
        listViewBots.Size = new System.Drawing.Size(1442, 684);
        listViewBots.TabIndex = 0;
        listViewBots.UseCompatibleStateImageBehavior = false;
        listViewBots.SelectedIndexChanged += listViewBots_SelectedIndexChanged;
        listViewBots.DoubleClick += listViewBots_DoubleClick;
        // 
        // ContextMenuStripBots
        // 
        ContextMenuStripBots.ImageScalingSize = new System.Drawing.Size(24, 24);
        ContextMenuStripBots.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { AddBotToolStripMenuItem, UpdateListToolStripMenuItem, UpdateBotToolStripMenuItem, DeleteBotToolStripMenuItem });
        ContextMenuStripBots.Name = "ContextMenuStripBots";
        ContextMenuStripBots.Size = new System.Drawing.Size(283, 132);
        // 
        // AddBotToolStripMenuItem
        // 
        AddBotToolStripMenuItem.Name = "AddBotToolStripMenuItem";
        AddBotToolStripMenuItem.Size = new System.Drawing.Size(282, 32);
        AddBotToolStripMenuItem.Text = "Добавить";
        AddBotToolStripMenuItem.Click += AddBotToolStripMenuItem_Click;
        // 
        // UpdateListToolStripMenuItem
        // 
        UpdateListToolStripMenuItem.Name = "UpdateListToolStripMenuItem";
        UpdateListToolStripMenuItem.Size = new System.Drawing.Size(282, 32);
        UpdateListToolStripMenuItem.Text = "Обновить список";
        UpdateListToolStripMenuItem.Click += UpdateListToolStripMenuItem_Click;
        // 
        // UpdateBotToolStripMenuItem
        // 
        UpdateBotToolStripMenuItem.Enabled = false;
        UpdateBotToolStripMenuItem.Name = "UpdateBotToolStripMenuItem";
        UpdateBotToolStripMenuItem.Size = new System.Drawing.Size(282, 32);
        UpdateBotToolStripMenuItem.Text = "Обновить информацию";
        UpdateBotToolStripMenuItem.Click += UpdateBotToolStripMenuItem_Click;
        // 
        // DeleteBotToolStripMenuItem
        // 
        DeleteBotToolStripMenuItem.Enabled = false;
        DeleteBotToolStripMenuItem.Name = "DeleteBotToolStripMenuItem";
        DeleteBotToolStripMenuItem.Size = new System.Drawing.Size(282, 32);
        DeleteBotToolStripMenuItem.Text = "Удалить";
        DeleteBotToolStripMenuItem.Click += DeleteBotToolStripMenuItem_Click;
        // 
        // tabPage1
        // 
        tabPage1.Location = new System.Drawing.Point(4, 32);
        tabPage1.Name = "tabPage1";
        tabPage1.Size = new System.Drawing.Size(1448, 690);
        tabPage1.TabIndex = 2;
        tabPage1.Text = "Скрипты";
        tabPage1.UseVisualStyleBackColor = true;
        // 
        // FormMain
        // 
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
        ClientSize = new System.Drawing.Size(1456, 726);
        Controls.Add(tabControlMain);
        Font = new System.Drawing.Font("Arial", 10F);
        Name = "FormMain";
        Text = "FormMain";
        tabControlMain.ResumeLayout(false);
        tabPageProxy.ResumeLayout(false);
        TabPageBots.ResumeLayout(false);
        ContextMenuStripBots.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.TabControl tabControlMain;
    private System.Windows.Forms.TabPage TabPageBots;
    private System.Windows.Forms.ListView listViewBots;
    private System.Windows.Forms.ContextMenuStrip ContextMenuStripBots;
    private System.Windows.Forms.ToolStripMenuItem AddBotToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem DeleteBotToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem UpdateBotToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem UpdateListToolStripMenuItem;
    private System.Windows.Forms.TabPage tabPageProxy;
    private UI.ProxyListView proxyListView;
    private System.Windows.Forms.Button button4;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.TabPage tabPage1;
}