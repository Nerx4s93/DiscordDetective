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
        buttonUpdateProxyList = new System.Windows.Forms.Button();
        buttonBuy = new System.Windows.Forms.Button();
        buttonDelete = new System.Windows.Forms.Button();
        buttonChangeType = new System.Windows.Forms.Button();
        buttonProlong = new System.Windows.Forms.Button();
        proxyListView = new DiscordDetective.UI.ProxyListView();
        TabPageBots = new System.Windows.Forms.TabPage();
        listViewBots = new System.Windows.Forms.ListView();
        ContextMenuStripBots = new System.Windows.Forms.ContextMenuStrip(components);
        AddBotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        UpdateListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        UpdateBotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        DeleteBotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        tabPage1 = new System.Windows.Forms.TabPage();
        label4 = new System.Windows.Forms.Label();
        listView4 = new System.Windows.Forms.ListView();
        label3 = new System.Windows.Forms.Label();
        listView3 = new System.Windows.Forms.ListView();
        listView2 = new System.Windows.Forms.ListView();
        label2 = new System.Windows.Forms.Label();
        label1 = new System.Windows.Forms.Label();
        listView1 = new System.Windows.Forms.ListView();
        button5 = new System.Windows.Forms.Button();
        contextMenuStripProlong = new System.Windows.Forms.ContextMenuStrip(components);
        buttonProlong3Days = new System.Windows.Forms.ToolStripMenuItem();
        buttonProlongWeek = new System.Windows.Forms.ToolStripMenuItem();
        buttonProlong2Weeks = new System.Windows.Forms.ToolStripMenuItem();
        buttonProlongMonth = new System.Windows.Forms.ToolStripMenuItem();
        buttonProlong2Month = new System.Windows.Forms.ToolStripMenuItem();
        buttonProlong3Month = new System.Windows.Forms.ToolStripMenuItem();
        contextMenuStripChangeType = new System.Windows.Forms.ContextMenuStrip(components);
        buttonTypeSocks5 = new System.Windows.Forms.ToolStripMenuItem();
        buttonTypeHttp = new System.Windows.Forms.ToolStripMenuItem();
        tabControlMain.SuspendLayout();
        tabPageProxy.SuspendLayout();
        TabPageBots.SuspendLayout();
        ContextMenuStripBots.SuspendLayout();
        tabPage1.SuspendLayout();
        contextMenuStripProlong.SuspendLayout();
        contextMenuStripChangeType.SuspendLayout();
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
        tabPageProxy.Controls.Add(buttonUpdateProxyList);
        tabPageProxy.Controls.Add(buttonBuy);
        tabPageProxy.Controls.Add(buttonDelete);
        tabPageProxy.Controls.Add(buttonChangeType);
        tabPageProxy.Controls.Add(buttonProlong);
        tabPageProxy.Controls.Add(proxyListView);
        tabPageProxy.Location = new System.Drawing.Point(4, 32);
        tabPageProxy.Name = "tabPageProxy";
        tabPageProxy.Size = new System.Drawing.Size(1448, 690);
        tabPageProxy.TabIndex = 1;
        tabPageProxy.Text = "Прокси";
        tabPageProxy.UseVisualStyleBackColor = true;
        // 
        // buttonUpdateProxyList
        // 
        buttonUpdateProxyList.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        buttonUpdateProxyList.Location = new System.Drawing.Point(1242, 578);
        buttonUpdateProxyList.Name = "buttonUpdateProxyList";
        buttonUpdateProxyList.Size = new System.Drawing.Size(194, 52);
        buttonUpdateProxyList.TabIndex = 6;
        buttonUpdateProxyList.Text = "Обновить список";
        buttonUpdateProxyList.UseVisualStyleBackColor = true;
        buttonUpdateProxyList.Click += buttonUpdateProxyList_Click;
        // 
        // buttonBuy
        // 
        buttonBuy.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        buttonBuy.Location = new System.Drawing.Point(1242, 636);
        buttonBuy.Name = "buttonBuy";
        buttonBuy.Size = new System.Drawing.Size(194, 52);
        buttonBuy.TabIndex = 5;
        buttonBuy.Text = "Купить";
        buttonBuy.UseVisualStyleBackColor = true;
        buttonBuy.Click += buttonBuy_Click;
        // 
        // buttonDelete
        // 
        buttonDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        buttonDelete.Enabled = false;
        buttonDelete.Location = new System.Drawing.Point(1242, 520);
        buttonDelete.Name = "buttonDelete";
        buttonDelete.Size = new System.Drawing.Size(194, 52);
        buttonDelete.TabIndex = 4;
        buttonDelete.Text = "Удалить";
        buttonDelete.UseVisualStyleBackColor = true;
        buttonDelete.Click += buttonDelete_Click;
        // 
        // buttonChangeType
        // 
        buttonChangeType.Enabled = false;
        buttonChangeType.Location = new System.Drawing.Point(1242, 61);
        buttonChangeType.Name = "buttonChangeType";
        buttonChangeType.Size = new System.Drawing.Size(194, 52);
        buttonChangeType.TabIndex = 3;
        buttonChangeType.Text = "Изменить тип";
        buttonChangeType.UseVisualStyleBackColor = true;
        buttonChangeType.Click += buttonChangeType_Click;
        // 
        // buttonProlong
        // 
        buttonProlong.Enabled = false;
        buttonProlong.Location = new System.Drawing.Point(1242, 3);
        buttonProlong.Name = "buttonProlong";
        buttonProlong.Size = new System.Drawing.Size(194, 52);
        buttonProlong.TabIndex = 1;
        buttonProlong.Text = "Продлить";
        buttonProlong.UseVisualStyleBackColor = true;
        buttonProlong.Click += buttonProlong_Click;
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
        proxyListView.Size = new System.Drawing.Size(1236, 730);
        proxyListView.TabIndex = 0;
        proxyListView.SelectedIndexChanged += proxyListView_SelectedIndexChanged;
        // 
        // TabPageBots
        // 
        TabPageBots.Controls.Add(listViewBots);
        TabPageBots.Location = new System.Drawing.Point(4, 34);
        TabPageBots.Name = "TabPageBots";
        TabPageBots.Padding = new System.Windows.Forms.Padding(3);
        TabPageBots.Size = new System.Drawing.Size(1448, 688);
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
        listViewBots.Size = new System.Drawing.Size(1442, 682);
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
        tabPage1.Controls.Add(label4);
        tabPage1.Controls.Add(listView4);
        tabPage1.Controls.Add(label3);
        tabPage1.Controls.Add(listView3);
        tabPage1.Controls.Add(listView2);
        tabPage1.Controls.Add(label2);
        tabPage1.Controls.Add(label1);
        tabPage1.Controls.Add(listView1);
        tabPage1.Controls.Add(button5);
        tabPage1.Location = new System.Drawing.Point(4, 34);
        tabPage1.Name = "tabPage1";
        tabPage1.Size = new System.Drawing.Size(1448, 688);
        tabPage1.TabIndex = 2;
        tabPage1.Text = "Выкачивание";
        tabPage1.UseVisualStyleBackColor = true;
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Location = new System.Drawing.Point(931, 9);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(198, 23);
        label4.TabIndex = 8;
        label4.Text = "Этап 4 - Сохранение";
        // 
        // listView4
        // 
        listView4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        listView4.FullRowSelect = true;
        listView4.Location = new System.Drawing.Point(931, 35);
        listView4.Name = "listView4";
        listView4.Size = new System.Drawing.Size(377, 647);
        listView4.TabIndex = 7;
        listView4.UseCompatibleStateImageBehavior = false;
        listView4.View = System.Windows.Forms.View.List;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new System.Drawing.Point(546, 9);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(219, 23);
        label3.TabIndex = 6;
        label3.Text = "Этап 3 - ИИ обработка";
        // 
        // listView3
        // 
        listView3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        listView3.FullRowSelect = true;
        listView3.Location = new System.Drawing.Point(546, 35);
        listView3.Name = "listView3";
        listView3.Size = new System.Drawing.Size(377, 647);
        listView3.TabIndex = 5;
        listView3.UseCompatibleStateImageBehavior = false;
        listView3.View = System.Windows.Forms.View.List;
        // 
        // listView2
        // 
        listView2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        listView2.FullRowSelect = true;
        listView2.Location = new System.Drawing.Point(235, 35);
        listView2.Name = "listView2";
        listView2.Size = new System.Drawing.Size(305, 647);
        listView2.TabIndex = 4;
        listView2.UseCompatibleStateImageBehavior = false;
        listView2.View = System.Windows.Forms.View.List;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(235, 9);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(137, 23);
        label2.TabIndex = 3;
        label2.Text = "Этап 2 - Чаты";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(8, 9);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(159, 23);
        label1.TabIndex = 2;
        label1.Text = "Этап 1 - Сервер";
        // 
        // listView1
        // 
        listView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        listView1.FullRowSelect = true;
        listView1.Location = new System.Drawing.Point(8, 35);
        listView1.Name = "listView1";
        listView1.Size = new System.Drawing.Size(221, 647);
        listView1.TabIndex = 1;
        listView1.UseCompatibleStateImageBehavior = false;
        listView1.View = System.Windows.Forms.View.List;
        // 
        // button5
        // 
        button5.Location = new System.Drawing.Point(1314, 35);
        button5.Name = "button5";
        button5.Size = new System.Drawing.Size(126, 34);
        button5.TabIndex = 0;
        button5.Text = "Start";
        button5.UseVisualStyleBackColor = true;
        button5.Click += button5_Click;
        // 
        // contextMenuStripProlong
        // 
        contextMenuStripProlong.Font = new System.Drawing.Font("Arial", 12F);
        contextMenuStripProlong.ImageScalingSize = new System.Drawing.Size(24, 24);
        contextMenuStripProlong.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { buttonProlong3Days, buttonProlongWeek, buttonProlong2Weeks, buttonProlongMonth, buttonProlong2Month, buttonProlong3Month });
        contextMenuStripProlong.Name = "contextMenuStripProlong";
        contextMenuStripProlong.Size = new System.Drawing.Size(187, 208);
        // 
        // buttonProlong3Days
        // 
        buttonProlong3Days.Name = "buttonProlong3Days";
        buttonProlong3Days.Size = new System.Drawing.Size(186, 34);
        buttonProlong3Days.Text = "3 дня";
        buttonProlong3Days.Click += buttonProlong3Days_Click;
        // 
        // buttonProlongWeek
        // 
        buttonProlongWeek.Name = "buttonProlongWeek";
        buttonProlongWeek.Size = new System.Drawing.Size(186, 34);
        buttonProlongWeek.Text = "1 неделя";
        buttonProlongWeek.Click += buttonProlongWeek_Click;
        // 
        // buttonProlong2Weeks
        // 
        buttonProlong2Weeks.Name = "buttonProlong2Weeks";
        buttonProlong2Weeks.Size = new System.Drawing.Size(186, 34);
        buttonProlong2Weeks.Text = "2 недели";
        buttonProlong2Weeks.Click += buttonProlong2Weeks_Click;
        // 
        // buttonProlongMonth
        // 
        buttonProlongMonth.Name = "buttonProlongMonth";
        buttonProlongMonth.Size = new System.Drawing.Size(186, 34);
        buttonProlongMonth.Text = "1 месяц";
        buttonProlongMonth.Click += buttonProlongMonth_Click;
        // 
        // buttonProlong2Month
        // 
        buttonProlong2Month.Name = "buttonProlong2Month";
        buttonProlong2Month.Size = new System.Drawing.Size(186, 34);
        buttonProlong2Month.Text = "2 месяца";
        buttonProlong2Month.Click += buttonProlong2Month_Click;
        // 
        // buttonProlong3Month
        // 
        buttonProlong3Month.Name = "buttonProlong3Month";
        buttonProlong3Month.Size = new System.Drawing.Size(186, 34);
        buttonProlong3Month.Text = "3 месяца";
        buttonProlong3Month.Click += buttonProlong3Month_Click;
        // 
        // contextMenuStripChangeType
        // 
        contextMenuStripChangeType.Font = new System.Drawing.Font("Arial", 12F);
        contextMenuStripChangeType.ImageScalingSize = new System.Drawing.Size(24, 24);
        contextMenuStripChangeType.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { buttonTypeSocks5, buttonTypeHttp });
        contextMenuStripChangeType.Name = "contextMenuStrip1";
        contextMenuStripChangeType.Size = new System.Drawing.Size(182, 72);
        // 
        // buttonTypeSocks5
        // 
        buttonTypeSocks5.Name = "buttonTypeSocks5";
        buttonTypeSocks5.Size = new System.Drawing.Size(181, 34);
        buttonTypeSocks5.Text = "SOCKS5";
        buttonTypeSocks5.Click += buttonTypeSocks5_Click;
        // 
        // buttonTypeHttp
        // 
        buttonTypeHttp.Name = "buttonTypeHttp";
        buttonTypeHttp.Size = new System.Drawing.Size(181, 34);
        buttonTypeHttp.Text = "HTTP(s)";
        buttonTypeHttp.Click += buttonTypeHttp_Click;
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
        tabPage1.ResumeLayout(false);
        tabPage1.PerformLayout();
        contextMenuStripProlong.ResumeLayout(false);
        contextMenuStripChangeType.ResumeLayout(false);
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
    private System.Windows.Forms.Button buttonDelete;
    private System.Windows.Forms.Button buttonChangeType;
    private System.Windows.Forms.Button buttonProlong;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.Button button5;
    private System.Windows.Forms.ListView listView1;
    private System.Windows.Forms.ListView listView2;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ListView listView3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.ListView listView4;
    private System.Windows.Forms.Button buttonBuy;
    private System.Windows.Forms.ContextMenuStrip contextMenuStripProlong;
    private System.Windows.Forms.ContextMenuStrip contextMenuStripChangeType;
    private System.Windows.Forms.ToolStripMenuItem buttonTypeSocks5;
    private System.Windows.Forms.ToolStripMenuItem buttonTypeHttp;
    private System.Windows.Forms.ToolStripMenuItem buttonProlong3Days;
    private System.Windows.Forms.ToolStripMenuItem buttonProlongWeek;
    private System.Windows.Forms.ToolStripMenuItem buttonProlong2Weeks;
    private System.Windows.Forms.ToolStripMenuItem buttonProlongMonth;
    private System.Windows.Forms.ToolStripMenuItem buttonProlong2Month;
    private System.Windows.Forms.ToolStripMenuItem buttonProlong3Month;
    private System.Windows.Forms.Button buttonUpdateProxyList;
}