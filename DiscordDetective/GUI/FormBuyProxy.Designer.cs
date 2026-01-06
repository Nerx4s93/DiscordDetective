namespace DiscordDetective.GUI;

partial class FormBuyProxy
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
        comboBoxType = new System.Windows.Forms.ComboBox();
        label1 = new System.Windows.Forms.Label();
        label2 = new System.Windows.Forms.Label();
        comboBoxCountry = new System.Windows.Forms.ComboBox();
        labelBuyCount = new System.Windows.Forms.Label();
        comboBoxChanging = new System.Windows.Forms.ComboBox();
        numericUpDownCount = new System.Windows.Forms.NumericUpDown();
        label3 = new System.Windows.Forms.Label();
        label4 = new System.Windows.Forms.Label();
        labelPrice = new System.Windows.Forms.Label();
        buttonBuy = new System.Windows.Forms.Button();
        priceTimer = new System.Windows.Forms.Timer(components);
        ((System.ComponentModel.ISupportInitialize)numericUpDownCount).BeginInit();
        SuspendLayout();
        // 
        // comboBoxType
        // 
        comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        comboBoxType.FormattingEnabled = true;
        comboBoxType.Items.AddRange(new object[] { "IPv6", "IPv4", "IPv4 Shared" });
        comboBoxType.Location = new System.Drawing.Point(12, 35);
        comboBoxType.Name = "comboBoxType";
        comboBoxType.Size = new System.Drawing.Size(403, 31);
        comboBoxType.TabIndex = 0;
        comboBoxType.SelectedIndexChanged += comboBoxType_SelectedIndexChanged;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(12, 9);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(108, 23);
        label1.TabIndex = 1;
        label1.Text = "Тип прокси";
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(12, 69);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(76, 23);
        label2.TabIndex = 2;
        label2.Text = "Страна";
        // 
        // comboBoxCountry
        // 
        comboBoxCountry.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
        comboBoxCountry.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        comboBoxCountry.FormattingEnabled = true;
        comboBoxCountry.Items.AddRange(new object[] { "IPv6", "IPv4", "IPv4 Shared" });
        comboBoxCountry.Location = new System.Drawing.Point(12, 95);
        comboBoxCountry.Name = "comboBoxCountry";
        comboBoxCountry.Size = new System.Drawing.Size(403, 31);
        comboBoxCountry.TabIndex = 3;
        comboBoxCountry.DrawItem += comboBoxCountry_DrawItem;
        comboBoxCountry.SelectedIndexChanged += comboBoxCountry_SelectedIndexChanged;
        // 
        // labelBuyCount
        // 
        labelBuyCount.AutoSize = true;
        labelBuyCount.Location = new System.Drawing.Point(12, 129);
        labelBuyCount.Name = "labelBuyCount";
        labelBuyCount.Size = new System.Drawing.Size(246, 23);
        labelBuyCount.TabIndex = 4;
        labelBuyCount.Text = "Доступно к покупке: 0  шт";
        // 
        // comboBoxChanging
        // 
        comboBoxChanging.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
        comboBoxChanging.FormattingEnabled = true;
        comboBoxChanging.Items.AddRange(new object[] { "1 неделя", "2 недели", "1 месяц", "2 месяца", "3 месяца" });
        comboBoxChanging.Location = new System.Drawing.Point(12, 255);
        comboBoxChanging.Name = "comboBoxChanging";
        comboBoxChanging.Size = new System.Drawing.Size(403, 31);
        comboBoxChanging.TabIndex = 5;
        comboBoxChanging.SelectedIndexChanged += comboBoxChanging_SelectedIndexChanged;
        // 
        // numericUpDownCount
        // 
        numericUpDownCount.Location = new System.Drawing.Point(12, 196);
        numericUpDownCount.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        numericUpDownCount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        numericUpDownCount.Name = "numericUpDownCount";
        numericUpDownCount.Size = new System.Drawing.Size(403, 30);
        numericUpDownCount.TabIndex = 6;
        numericUpDownCount.Value = new decimal(new int[] { 1, 0, 0, 0 });
        numericUpDownCount.ValueChanged += numericUpDownCount_ValueChanged;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Location = new System.Drawing.Point(12, 170);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(116, 23);
        label3.TabIndex = 7;
        label3.Text = "Количество";
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Location = new System.Drawing.Point(12, 229);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(78, 23);
        label4.TabIndex = 8;
        label4.Text = "Период";
        // 
        // labelPrice
        // 
        labelPrice.AutoSize = true;
        labelPrice.Location = new System.Drawing.Point(12, 304);
        labelPrice.Name = "labelPrice";
        labelPrice.Size = new System.Drawing.Size(97, 23);
        labelPrice.TabIndex = 9;
        labelPrice.Text = "Цена: 0 ₽";
        // 
        // buttonBuy
        // 
        buttonBuy.Enabled = false;
        buttonBuy.Location = new System.Drawing.Point(12, 330);
        buttonBuy.Name = "buttonBuy";
        buttonBuy.Size = new System.Drawing.Size(403, 56);
        buttonBuy.TabIndex = 10;
        buttonBuy.Text = "Купить";
        buttonBuy.UseVisualStyleBackColor = true;
        buttonBuy.Click += buttonBuy_Click;
        // 
        // priceTimer
        // 
        priceTimer.Interval = 800;
        priceTimer.Tick += priceTimer_Tick;
        // 
        // FormBuyProxy
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(11F, 23F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(427, 396);
        Controls.Add(buttonBuy);
        Controls.Add(labelPrice);
        Controls.Add(label4);
        Controls.Add(label3);
        Controls.Add(numericUpDownCount);
        Controls.Add(comboBoxChanging);
        Controls.Add(labelBuyCount);
        Controls.Add(comboBoxCountry);
        Controls.Add(label2);
        Controls.Add(label1);
        Controls.Add(comboBoxType);
        Font = new System.Drawing.Font("Arial", 10F);
        Name = "FormBuyProxy";
        Text = "Покупка прокси";
        ((System.ComponentModel.ISupportInitialize)numericUpDownCount).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.ComboBox comboBoxType;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox comboBoxCountry;
    private System.Windows.Forms.Label labelBuyCount;
    private System.Windows.Forms.ComboBox comboBoxChanging;
    private System.Windows.Forms.NumericUpDown numericUpDownCount;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label labelPrice;
    private System.Windows.Forms.Button buttonBuy;
    private System.Windows.Forms.Timer priceTimer;
}