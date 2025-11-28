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
        tabControl1 = new System.Windows.Forms.TabControl();
        TabPageBots = new System.Windows.Forms.TabPage();
        listView1 = new System.Windows.Forms.ListView();
        ContextMenuStripBots = new System.Windows.Forms.ContextMenuStrip(components);
        добавитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        удалитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        tabControl1.SuspendLayout();
        TabPageBots.SuspendLayout();
        ContextMenuStripBots.SuspendLayout();
        SuspendLayout();
        // 
        // tabControl1
        // 
        tabControl1.Controls.Add(TabPageBots);
        tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
        tabControl1.Location = new System.Drawing.Point(0, 0);
        tabControl1.Name = "tabControl1";
        tabControl1.SelectedIndex = 0;
        tabControl1.Size = new System.Drawing.Size(1218, 726);
        tabControl1.TabIndex = 0;
        // 
        // TabPageBots
        // 
        TabPageBots.Controls.Add(listView1);
        TabPageBots.Location = new System.Drawing.Point(4, 34);
        TabPageBots.Name = "TabPageBots";
        TabPageBots.Padding = new System.Windows.Forms.Padding(3);
        TabPageBots.Size = new System.Drawing.Size(1210, 688);
        TabPageBots.TabIndex = 0;
        TabPageBots.Text = "Боты";
        TabPageBots.UseVisualStyleBackColor = true;
        // 
        // listView1
        // 
        listView1.ContextMenuStrip = ContextMenuStripBots;
        listView1.Dock = System.Windows.Forms.DockStyle.Fill;
        listView1.Location = new System.Drawing.Point(3, 3);
        listView1.Name = "listView1";
        listView1.Size = new System.Drawing.Size(1204, 682);
        listView1.TabIndex = 0;
        listView1.UseCompatibleStateImageBehavior = false;
        // 
        // ContextMenuStripBots
        // 
        ContextMenuStripBots.ImageScalingSize = new System.Drawing.Size(24, 24);
        ContextMenuStripBots.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { добавитьToolStripMenuItem, удалитьToolStripMenuItem });
        ContextMenuStripBots.Name = "ContextMenuStripBots";
        ContextMenuStripBots.Size = new System.Drawing.Size(163, 68);
        // 
        // добавитьToolStripMenuItem
        // 
        добавитьToolStripMenuItem.Name = "добавитьToolStripMenuItem";
        добавитьToolStripMenuItem.Size = new System.Drawing.Size(162, 32);
        добавитьToolStripMenuItem.Text = "Добавить";
        // 
        // удалитьToolStripMenuItem
        // 
        удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
        удалитьToolStripMenuItem.Size = new System.Drawing.Size(162, 32);
        удалитьToolStripMenuItem.Text = "Удалить";
        // 
        // FormMain
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1218, 726);
        Controls.Add(tabControl1);
        Name = "FormMain";
        Text = "FormMain";
        tabControl1.ResumeLayout(false);
        TabPageBots.ResumeLayout(false);
        ContextMenuStripBots.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage TabPageBots;
    private System.Windows.Forms.ListView listView1;
    private System.Windows.Forms.ContextMenuStrip ContextMenuStripBots;
    private System.Windows.Forms.ToolStripMenuItem добавитьToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem удалитьToolStripMenuItem;
}