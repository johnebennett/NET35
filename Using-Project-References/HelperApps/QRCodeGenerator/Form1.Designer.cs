
namespace WindowsFormsApp1
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSha1 = new System.Windows.Forms.Button();
            this.btnSha256 = new System.Windows.Forms.Button();
            this.btnSha512 = new System.Windows.Forms.Button();
            this.btnGenerateAll = new System.Windows.Forms.Button();
            this.txtSha1 = new System.Windows.Forms.TextBox();
            this.txtSha256 = new System.Windows.Forms.TextBox();
            this.txtSha512 = new System.Windows.Forms.TextBox();
            this.txtSecret = new System.Windows.Forms.TextBox();
            this.txtUniqueId = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnMakeEntry = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblRemaining = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(713, 415);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "CLose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSha1
            // 
            this.btnSha1.Location = new System.Drawing.Point(12, 78);
            this.btnSha1.Name = "btnSha1";
            this.btnSha1.Size = new System.Drawing.Size(75, 23);
            this.btnSha1.TabIndex = 1;
            this.btnSha1.Text = "Sha1";
            this.btnSha1.UseVisualStyleBackColor = true;
            this.btnSha1.Click += new System.EventHandler(this.btnSha1_Click);
            // 
            // btnSha256
            // 
            this.btnSha256.Location = new System.Drawing.Point(12, 107);
            this.btnSha256.Name = "btnSha256";
            this.btnSha256.Size = new System.Drawing.Size(75, 23);
            this.btnSha256.TabIndex = 2;
            this.btnSha256.Text = "Sha256";
            this.btnSha256.UseVisualStyleBackColor = true;
            this.btnSha256.Click += new System.EventHandler(this.btnSha256_Click);
            // 
            // btnSha512
            // 
            this.btnSha512.Location = new System.Drawing.Point(12, 136);
            this.btnSha512.Name = "btnSha512";
            this.btnSha512.Size = new System.Drawing.Size(75, 23);
            this.btnSha512.TabIndex = 3;
            this.btnSha512.Text = "Sha512";
            this.btnSha512.UseVisualStyleBackColor = true;
            this.btnSha512.Click += new System.EventHandler(this.btnSha512_Click);
            // 
            // btnGenerateAll
            // 
            this.btnGenerateAll.Location = new System.Drawing.Point(12, 165);
            this.btnGenerateAll.Name = "btnGenerateAll";
            this.btnGenerateAll.Size = new System.Drawing.Size(75, 23);
            this.btnGenerateAll.TabIndex = 4;
            this.btnGenerateAll.Text = "Gen ALL";
            this.btnGenerateAll.UseVisualStyleBackColor = true;
            this.btnGenerateAll.Click += new System.EventHandler(this.btnGenerateAll_Click);
            // 
            // txtSha1
            // 
            this.txtSha1.Location = new System.Drawing.Point(92, 80);
            this.txtSha1.Name = "txtSha1";
            this.txtSha1.Size = new System.Drawing.Size(120, 20);
            this.txtSha1.TabIndex = 5;
            // 
            // txtSha256
            // 
            this.txtSha256.Location = new System.Drawing.Point(93, 109);
            this.txtSha256.Name = "txtSha256";
            this.txtSha256.Size = new System.Drawing.Size(120, 20);
            this.txtSha256.TabIndex = 6;
            // 
            // txtSha512
            // 
            this.txtSha512.Location = new System.Drawing.Point(93, 138);
            this.txtSha512.Name = "txtSha512";
            this.txtSha512.Size = new System.Drawing.Size(120, 20);
            this.txtSha512.TabIndex = 7;
            // 
            // txtSecret
            // 
            this.txtSecret.Location = new System.Drawing.Point(93, 12);
            this.txtSecret.Name = "txtSecret";
            this.txtSecret.Size = new System.Drawing.Size(150, 20);
            this.txtSecret.TabIndex = 8;
            this.txtSecret.Text = "GOATPM";
            // 
            // txtUniqueId
            // 
            this.txtUniqueId.Location = new System.Drawing.Point(93, 47);
            this.txtUniqueId.Name = "txtUniqueId";
            this.txtUniqueId.Size = new System.Drawing.Size(150, 20);
            this.txtUniqueId.TabIndex = 9;
            this.txtUniqueId.Text = "QVWApdFTByyM";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(249, 45);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 10;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Secret";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Unique ID";
            // 
            // btnMakeEntry
            // 
            this.btnMakeEntry.Location = new System.Drawing.Point(330, 44);
            this.btnMakeEntry.Name = "btnMakeEntry";
            this.btnMakeEntry.Size = new System.Drawing.Size(75, 23);
            this.btnMakeEntry.TabIndex = 13;
            this.btnMakeEntry.Text = "Make Entry";
            this.btnMakeEntry.UseVisualStyleBackColor = true;
            this.btnMakeEntry.Click += new System.EventHandler(this.btnMakeEntry_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblRemaining
            // 
            this.lblRemaining.AutoSize = true;
            this.lblRemaining.Location = new System.Drawing.Point(249, 87);
            this.lblRemaining.Name = "lblRemaining";
            this.lblRemaining.Size = new System.Drawing.Size(0, 13);
            this.lblRemaining.TabIndex = 14;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(429, 44);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(300, 300);
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblRemaining);
            this.Controls.Add(this.btnMakeEntry);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.txtUniqueId);
            this.Controls.Add(this.txtSecret);
            this.Controls.Add(this.txtSha512);
            this.Controls.Add(this.txtSha256);
            this.Controls.Add(this.txtSha1);
            this.Controls.Add(this.btnGenerateAll);
            this.Controls.Add(this.btnSha512);
            this.Controls.Add(this.btnSha256);
            this.Controls.Add(this.btnSha1);
            this.Controls.Add(this.btnClose);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSha1;
        private System.Windows.Forms.Button btnSha256;
        private System.Windows.Forms.Button btnSha512;
        private System.Windows.Forms.Button btnGenerateAll;
        private System.Windows.Forms.TextBox txtSha1;
        private System.Windows.Forms.TextBox txtSha256;
        private System.Windows.Forms.TextBox txtSha512;
        private System.Windows.Forms.TextBox txtSecret;
        private System.Windows.Forms.TextBox txtUniqueId;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnMakeEntry;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblRemaining;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

