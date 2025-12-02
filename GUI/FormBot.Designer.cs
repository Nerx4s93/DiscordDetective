namespace DiscordDetective.GUI;

partial class FormBot
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
        tabControl1 = new System.Windows.Forms.TabControl();
        tabPage1 = new System.Windows.Forms.TabPage();
        label10 = new System.Windows.Forms.Label();
        richTextBoxBio = new System.Windows.Forms.RichTextBox();
        labelVerified = new System.Windows.Forms.Label();
        labelEmail = new System.Windows.Forms.Label();
        labelAccentColor = new System.Windows.Forms.Label();
        labelBanner = new System.Windows.Forms.Label();
        labelAvatar = new System.Windows.Forms.Label();
        labelUserDiscriminator = new System.Windows.Forms.Label();
        labelUserGlobalName = new System.Windows.Forms.Label();
        labelUserUsername = new System.Windows.Forms.Label();
        pictureBoxAvatar = new System.Windows.Forms.PictureBox();
        labelUserId = new System.Windows.Forms.Label();
        tabPage2 = new System.Windows.Forms.TabPage();
        tabControl1.SuspendLayout();
        tabPage1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBoxAvatar).BeginInit();
        SuspendLayout();
        // 
        // tabControl1
        // 
        tabControl1.Controls.Add(tabPage1);
        tabControl1.Controls.Add(tabPage2);
        tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
        tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
        tabControl1.Location = new System.Drawing.Point(0, 0);
        tabControl1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
        tabControl1.Multiline = true;
        tabControl1.Name = "tabControl1";
        tabControl1.SelectedIndex = 0;
        tabControl1.Size = new System.Drawing.Size(998, 441);
        tabControl1.TabIndex = 0;
        // 
        // tabPage1
        // 
        tabPage1.Controls.Add(label10);
        tabPage1.Controls.Add(richTextBoxBio);
        tabPage1.Controls.Add(labelVerified);
        tabPage1.Controls.Add(labelEmail);
        tabPage1.Controls.Add(labelAccentColor);
        tabPage1.Controls.Add(labelBanner);
        tabPage1.Controls.Add(labelAvatar);
        tabPage1.Controls.Add(labelUserDiscriminator);
        tabPage1.Controls.Add(labelUserGlobalName);
        tabPage1.Controls.Add(labelUserUsername);
        tabPage1.Controls.Add(pictureBoxAvatar);
        tabPage1.Controls.Add(labelUserId);
        tabPage1.Location = new System.Drawing.Point(4, 29);
        tabPage1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
        tabPage1.Name = "tabPage1";
        tabPage1.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
        tabPage1.Size = new System.Drawing.Size(990, 408);
        tabPage1.TabIndex = 0;
        tabPage1.Text = "Информация бота";
        tabPage1.UseVisualStyleBackColor = true;
        // 
        // label10
        // 
        label10.AutoSize = true;
        label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
        label10.Location = new System.Drawing.Point(8, 109);
        label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        label10.Name = "label10";
        label10.Size = new System.Drawing.Size(64, 32);
        label10.TabIndex = 11;
        label10.Text = "Bio:";
        // 
        // richTextBoxBio
        // 
        richTextBoxBio.Location = new System.Drawing.Point(8, 145);
        richTextBoxBio.Name = "richTextBoxBio";
        richTextBoxBio.Size = new System.Drawing.Size(358, 257);
        richTextBoxBio.TabIndex = 10;
        richTextBoxBio.Text = "";
        // 
        // labelVerified
        // 
        labelVerified.AutoSize = true;
        labelVerified.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
        labelVerified.Location = new System.Drawing.Point(371, 374);
        labelVerified.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        labelVerified.Name = "labelVerified";
        labelVerified.Size = new System.Drawing.Size(120, 32);
        labelVerified.TabIndex = 9;
        labelVerified.Text = "Verified:";
        // 
        // labelEmail
        // 
        labelEmail.AutoSize = true;
        labelEmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
        labelEmail.Location = new System.Drawing.Point(371, 342);
        labelEmail.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        labelEmail.Name = "labelEmail";
        labelEmail.Size = new System.Drawing.Size(94, 32);
        labelEmail.TabIndex = 8;
        labelEmail.Text = "Email:";
        // 
        // labelAccentColor
        // 
        labelAccentColor.AutoSize = true;
        labelAccentColor.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
        labelAccentColor.Location = new System.Drawing.Point(371, 310);
        labelAccentColor.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        labelAccentColor.Name = "labelAccentColor";
        labelAccentColor.Size = new System.Drawing.Size(177, 32);
        labelAccentColor.TabIndex = 7;
        labelAccentColor.Text = "AccentColor:";
        // 
        // labelBanner
        // 
        labelBanner.AutoSize = true;
        labelBanner.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
        labelBanner.Location = new System.Drawing.Point(371, 278);
        labelBanner.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        labelBanner.Name = "labelBanner";
        labelBanner.Size = new System.Drawing.Size(114, 32);
        labelBanner.TabIndex = 6;
        labelBanner.Text = "Banner:";
        // 
        // labelAvatar
        // 
        labelAvatar.AutoSize = true;
        labelAvatar.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
        labelAvatar.Location = new System.Drawing.Point(371, 246);
        labelAvatar.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        labelAvatar.Name = "labelAvatar";
        labelAvatar.Size = new System.Drawing.Size(104, 32);
        labelAvatar.TabIndex = 5;
        labelAvatar.Text = "Avatar:";
        // 
        // labelUserDiscriminator
        // 
        labelUserDiscriminator.AutoSize = true;
        labelUserDiscriminator.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
        labelUserDiscriminator.Location = new System.Drawing.Point(371, 214);
        labelUserDiscriminator.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        labelUserDiscriminator.Name = "labelUserDiscriminator";
        labelUserDiscriminator.Size = new System.Drawing.Size(188, 32);
        labelUserDiscriminator.TabIndex = 4;
        labelUserDiscriminator.Text = "Discriminator:";
        // 
        // labelUserGlobalName
        // 
        labelUserGlobalName.AutoSize = true;
        labelUserGlobalName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
        labelUserGlobalName.Location = new System.Drawing.Point(371, 182);
        labelUserGlobalName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        labelUserGlobalName.Name = "labelUserGlobalName";
        labelUserGlobalName.Size = new System.Drawing.Size(181, 32);
        labelUserGlobalName.TabIndex = 3;
        labelUserGlobalName.Text = "GlobalName:";
        // 
        // labelUserUsername
        // 
        labelUserUsername.AutoSize = true;
        labelUserUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
        labelUserUsername.Location = new System.Drawing.Point(371, 150);
        labelUserUsername.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        labelUserUsername.Name = "labelUserUsername";
        labelUserUsername.Size = new System.Drawing.Size(152, 32);
        labelUserUsername.TabIndex = 2;
        labelUserUsername.Text = "Username:";
        // 
        // pictureBoxAvatar
        // 
        pictureBoxAvatar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        pictureBoxAvatar.Location = new System.Drawing.Point(8, 6);
        pictureBoxAvatar.Name = "pictureBoxAvatar";
        pictureBoxAvatar.Size = new System.Drawing.Size(100, 100);
        pictureBoxAvatar.TabIndex = 1;
        pictureBoxAvatar.TabStop = false;
        // 
        // labelUserId
        // 
        labelUserId.AutoSize = true;
        labelUserId.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
        labelUserId.Location = new System.Drawing.Point(371, 118);
        labelUserId.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
        labelUserId.Name = "labelUserId";
        labelUserId.Size = new System.Drawing.Size(45, 32);
        labelUserId.TabIndex = 0;
        labelUserId.Text = "Id:";
        // 
        // tabPage2
        // 
        tabPage2.Location = new System.Drawing.Point(4, 29);
        tabPage2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
        tabPage2.Name = "tabPage2";
        tabPage2.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
        tabPage2.Size = new System.Drawing.Size(990, 408);
        tabPage2.TabIndex = 1;
        tabPage2.Text = "Сервера";
        tabPage2.UseVisualStyleBackColor = true;
        // 
        // FormBot
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(998, 441);
        Controls.Add(tabControl1);
        Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 204);
        Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
        Name = "FormBot";
        Text = "FormBot";
        tabControl1.ResumeLayout(false);
        tabPage1.ResumeLayout(false);
        tabPage1.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)pictureBoxAvatar).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.TabPage tabPage2;
    private System.Windows.Forms.Label labelUserId;
    private System.Windows.Forms.Label labelBanner;
    private System.Windows.Forms.Label labelAvatar;
    private System.Windows.Forms.Label labelUserDiscriminator;
    private System.Windows.Forms.Label labelUserGlobalName;
    private System.Windows.Forms.Label labelUserUsername;
    private System.Windows.Forms.PictureBox pictureBoxAvatar;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.RichTextBox richTextBoxBio;
    private System.Windows.Forms.Label labelVerified;
    private System.Windows.Forms.Label labelEmail;
    private System.Windows.Forms.Label labelAccentColor;
}