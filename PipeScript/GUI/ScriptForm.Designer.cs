namespace PipeScript.GUI;

partial class ScriptForm
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
        richTextBoxOutput = new System.Windows.Forms.RichTextBox();
        label1 = new System.Windows.Forms.Label();
        buttonStop = new System.Windows.Forms.Button();
        buttonPause = new System.Windows.Forms.Button();
        buttonRestart = new System.Windows.Forms.Button();
        SuspendLayout();
        // 
        // richTextBoxOutput
        // 
        richTextBoxOutput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        richTextBoxOutput.Location = new System.Drawing.Point(12, 35);
        richTextBoxOutput.Name = "richTextBoxOutput";
        richTextBoxOutput.Size = new System.Drawing.Size(1077, 475);
        richTextBoxOutput.TabIndex = 0;
        richTextBoxOutput.Text = "";
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new System.Drawing.Point(12, 9);
        label1.Name = "label1";
        label1.Size = new System.Drawing.Size(77, 23);
        label1.TabIndex = 1;
        label1.Text = "Вывод:";
        // 
        // buttonStop
        // 
        buttonStop.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        buttonStop.Enabled = false;
        buttonStop.Location = new System.Drawing.Point(159, 516);
        buttonStop.Name = "buttonStop";
        buttonStop.Size = new System.Drawing.Size(141, 48);
        buttonStop.TabIndex = 2;
        buttonStop.Text = "Стоп";
        buttonStop.UseVisualStyleBackColor = true;
        // 
        // buttonPause
        // 
        buttonPause.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        buttonPause.Enabled = false;
        buttonPause.Location = new System.Drawing.Point(12, 516);
        buttonPause.Name = "buttonPause";
        buttonPause.Size = new System.Drawing.Size(141, 48);
        buttonPause.TabIndex = 3;
        buttonPause.Text = "Пауза";
        buttonPause.UseVisualStyleBackColor = true;
        // 
        // buttonRestart
        // 
        buttonRestart.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        buttonRestart.Enabled = false;
        buttonRestart.Location = new System.Drawing.Point(306, 516);
        buttonRestart.Name = "buttonRestart";
        buttonRestart.Size = new System.Drawing.Size(141, 48);
        buttonRestart.TabIndex = 4;
        buttonRestart.Text = "Перезапуск";
        buttonRestart.UseVisualStyleBackColor = true;
        // 
        // ScriptForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(11F, 23F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1101, 573);
        Controls.Add(buttonRestart);
        Controls.Add(buttonPause);
        Controls.Add(buttonStop);
        Controls.Add(label1);
        Controls.Add(richTextBoxOutput);
        Font = new System.Drawing.Font("Arial", 10F);
        Name = "ScriptForm";
        Text = "Form1";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.RichTextBox richTextBoxOutput;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button buttonStop;
    private System.Windows.Forms.Button buttonPause;
    private System.Windows.Forms.Button buttonRestart;
}