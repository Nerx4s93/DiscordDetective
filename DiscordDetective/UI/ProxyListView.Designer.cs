namespace DiscordDetective.UI;

partial class ProxyListView
{
    /// <summary> 
    /// Обязательная переменная конструктора.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Освободить все используемые ресурсы.
    /// </summary>
    /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Код, автоматически созданный конструктором компонентов

    /// <summary> 
    /// Требуемый метод для поддержки конструктора — не изменяйте 
    /// содержимое этого метода с помощью редактора кода.
    /// </summary>
    private void InitializeComponent()
    {
        titlePanel = new System.Windows.Forms.Panel();
        container = new System.Windows.Forms.FlowLayoutPanel();
        SuspendLayout();
        // 
        // titlePanel
        // 
        titlePanel.Dock = System.Windows.Forms.DockStyle.Top;
        titlePanel.Location = new System.Drawing.Point(1, 1);
        titlePanel.Name = "titlePanel";
        titlePanel.Size = new System.Drawing.Size(1232, 60);
        titlePanel.TabIndex = 0;
        // 
        // container
        // 
        container.AutoScroll = true;
        container.Dock = System.Windows.Forms.DockStyle.Fill;
        container.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
        container.Location = new System.Drawing.Point(1, 61);
        container.Name = "container";
        container.Size = new System.Drawing.Size(1232, 736);
        container.TabIndex = 1;
        container.WrapContents = false;
        // 
        // UserControl1
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        BackColor = System.Drawing.Color.White;
        BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        Controls.Add(container);
        Controls.Add(titlePanel);
        Name = "UserControl1";
        Padding = new System.Windows.Forms.Padding(1);
        Size = new System.Drawing.Size(1234, 798);
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Panel titlePanel;
    private System.Windows.Forms.FlowLayoutPanel container;
}
