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
        checkBoxItemSelected = new System.Windows.Forms.CheckBox();
        label2 = new System.Windows.Forms.Label();
        label1 = new System.Windows.Forms.Label();
        label3 = new System.Windows.Forms.Label();
        titlePanel.SuspendLayout();
        SuspendLayout();
        // 
        // titlePanel
        // 
        titlePanel.BackColor = System.Drawing.Color.WhiteSmoke;
        titlePanel.Controls.Add(label3);
        titlePanel.Controls.Add(label1);
        titlePanel.Controls.Add(label2);
        titlePanel.Controls.Add(checkBoxItemSelected);
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
        container.Size = new System.Drawing.Size(1232, 738);
        container.TabIndex = 1;
        container.WrapContents = false;
        // 
        // checkBoxItemSelected
        // 
        checkBoxItemSelected.Location = new System.Drawing.Point(12, 18);
        checkBoxItemSelected.Name = "checkBoxItemSelected";
        checkBoxItemSelected.Size = new System.Drawing.Size(26, 25);
        checkBoxItemSelected.TabIndex = 1;
        checkBoxItemSelected.Text = "checkBox1";
        checkBoxItemSelected.UseVisualStyleBackColor = true;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
        label2.Location = new System.Drawing.Point(129, 18);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(87, 24);
        label2.TabIndex = 2;
        label2.Text = "Данные";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
        label1.Location = new System.Drawing.Point(648, 18);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(166, 24);
        label1.TabIndex = 3;
        label1.Text = "Дата окончания";
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 204);
        label3.Location = new System.Drawing.Point(868, 19);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(143, 24);
        label3.TabIndex = 4;
        label3.Text = "Комментарий";
        // 
        // ProxyListView
        // 
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
        BackColor = System.Drawing.Color.White;
        BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        Controls.Add(container);
        Controls.Add(titlePanel);
        Font = new System.Drawing.Font("Arial", 10F);
        MaximumSize = new System.Drawing.Size(1236, 0);
        MinimumSize = new System.Drawing.Size(1236, 800);
        Name = "ProxyListView";
        Padding = new System.Windows.Forms.Padding(1);
        Size = new System.Drawing.Size(1234, 800);
        titlePanel.ResumeLayout(false);
        titlePanel.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Panel titlePanel;
    private System.Windows.Forms.FlowLayoutPanel container;
    private System.Windows.Forms.CheckBox checkBoxItemSelected;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label1;
}
