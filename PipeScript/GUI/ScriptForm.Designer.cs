using PipeScript.UI;

namespace PipeScript.GUI;

sealed partial class ScriptForm
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
        _richTextBoxNoSmoothScrollOutput = new RichTextBoxNoSmoothScroll();
        label1 = new System.Windows.Forms.Label();
        buttonStop = new System.Windows.Forms.Button();
        buttonStart = new System.Windows.Forms.Button();
        buttonPauseResume = new System.Windows.Forms.Button();
        buttonStep = new System.Windows.Forms.Button();
        scriptCodeView1 = new ScriptCodeView();
        label2 = new System.Windows.Forms.Label();
        SuspendLayout();
        // 
        // _richTextBoxNoSmoothScrollOutput
        // 
        _richTextBoxNoSmoothScrollOutput.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        _richTextBoxNoSmoothScrollOutput.Location = new System.Drawing.Point(12, 35);
        _richTextBoxNoSmoothScrollOutput.Name = "_richTextBoxNoSmoothScrollOutput";
        _richTextBoxNoSmoothScrollOutput.Size = new System.Drawing.Size(801, 600);
        _richTextBoxNoSmoothScrollOutput.TabIndex = 0;
        _richTextBoxNoSmoothScrollOutput.Text = "";
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
        buttonStop.Location = new System.Drawing.Point(159, 641);
        buttonStop.Name = "buttonStop";
        buttonStop.Size = new System.Drawing.Size(141, 48);
        buttonStop.TabIndex = 2;
        buttonStop.Text = "Стоп";
        buttonStop.UseVisualStyleBackColor = true;
        buttonStop.Click += buttonStop_Click;
        // 
        // buttonStart
        // 
        buttonStart.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        buttonStart.Location = new System.Drawing.Point(12, 641);
        buttonStart.Name = "buttonStart";
        buttonStart.Size = new System.Drawing.Size(141, 48);
        buttonStart.TabIndex = 3;
        buttonStart.Text = "Старт";
        buttonStart.UseVisualStyleBackColor = true;
        buttonStart.Click += buttonStart_Click;
        // 
        // buttonPauseResume
        // 
        buttonPauseResume.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        buttonPauseResume.Location = new System.Drawing.Point(306, 641);
        buttonPauseResume.Name = "buttonPauseResume";
        buttonPauseResume.Size = new System.Drawing.Size(141, 48);
        buttonPauseResume.TabIndex = 5;
        buttonPauseResume.Text = "Пауза";
        buttonPauseResume.UseVisualStyleBackColor = true;
        buttonPauseResume.Click += buttonPauseResume_Click;
        // 
        // buttonStep
        // 
        buttonStep.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
        buttonStep.Enabled = false;
        buttonStep.Location = new System.Drawing.Point(453, 641);
        buttonStep.Name = "buttonStep";
        buttonStep.Size = new System.Drawing.Size(141, 48);
        buttonStep.TabIndex = 6;
        buttonStep.Text = "Шаг";
        buttonStep.UseVisualStyleBackColor = true;
        buttonStep.Click += buttonStep_Click;
        // 
        // scriptCodeView1
        // 
        scriptCodeView1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        scriptCodeView1.DetectUrls = false;
        scriptCodeView1.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
        scriptCodeView1.Location = new System.Drawing.Point(819, 35);
        scriptCodeView1.Name = "scriptCodeView1";
        scriptCodeView1.Size = new System.Drawing.Size(567, 600);
        scriptCodeView1.TabIndex = 7;
        scriptCodeView1.Text = "";
        scriptCodeView1.WordWrap = false;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Location = new System.Drawing.Point(819, 9);
        label2.Name = "label2";
        label2.Size = new System.Drawing.Size(51, 23);
        label2.TabIndex = 8;
        label2.Text = "Код:";
        // 
        // ScriptForm
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(11F, 23F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(1398, 698);
        Controls.Add(label2);
        Controls.Add(scriptCodeView1);
        Controls.Add(buttonStep);
        Controls.Add(buttonPauseResume);
        Controls.Add(buttonStart);
        Controls.Add(buttonStop);
        Controls.Add(label1);
        Controls.Add(_richTextBoxNoSmoothScrollOutput);
        Font = new System.Drawing.Font("Arial", 10F);
        Name = "ScriptForm";
        Text = "Form1";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private RichTextBoxNoSmoothScroll _richTextBoxNoSmoothScrollOutput;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button buttonStop;
    private System.Windows.Forms.Button buttonStart;
    private System.Windows.Forms.Button buttonPauseResume;
    private System.Windows.Forms.Button buttonStep;
    private ScriptCodeView scriptCodeView1;
    private System.Windows.Forms.Label label2;
}