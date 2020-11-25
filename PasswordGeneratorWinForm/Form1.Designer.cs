namespace PasswordGeneratorWinForm
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.txtExistingPassw = new System.Windows.Forms.TextBox();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Items.AddRange(new object[] {
            "Include Symbols ( e.g. @#$% )",
            "Include Numbers ( e.g. 123456 )",
            "Include Lowercase Characters ( e.g. abcdefgh )",
            "Include Uppercase Characters ( e.g. ABCDEFGH )",
            "Exclude Similar Characters ( e.g. i, l, 1, L, o, 0, O )",
            "Exclude Ambiguous Characters ( { } [ ] ( ) / \\ \' \" ` ~ , ; : . < > )",
            "Generate On Your Device ( do NOT send across the Internet )",
            "Auto-Select ( select the password automatically )",
            "Save My Preference ( save all the settings above for later use )",
            "Load My Settings Anywhere - URL to load my settings on other computers quickly"});
            this.listBox1.Location = new System.Drawing.Point(60, 179);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(481, 169);
            this.listBox1.TabIndex = 0;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(60, 13);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(521, 19);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Use an existing password (the new password will have the same length and the same" +
    " symbols)";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(60, 39);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(188, 19);
            this.radioButton2.TabIndex = 2;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Don\'t use an existing password";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // txtExistingPassw
            // 
            this.txtExistingPassw.Location = new System.Drawing.Point(60, 93);
            this.txtExistingPassw.Name = "txtExistingPassw";
            this.txtExistingPassw.Size = new System.Drawing.Size(680, 23);
            this.txtExistingPassw.TabIndex = 3;
            this.txtExistingPassw.TextChanged += new System.EventHandler(this.txtExistingPassword_TextChanged);
            // 
            // txtNewPassword
            // 
            this.txtNewPassword.Location = new System.Drawing.Point(60, 141);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.Size = new System.Drawing.Size(680, 23);
            this.txtNewPassword.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtNewPassword);
            this.Controls.Add(this.txtExistingPassw);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.listBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.TextBox txtExistingPassw;
        private System.Windows.Forms.TextBox txtNewPassword;
    }
}

