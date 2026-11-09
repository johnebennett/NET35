/*
 * Created by SharpDevelop.
 * User: John
 * Date: 03/07/2019
 * Time: 10:38 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace MakeItemClasses
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Panel pnlBottomButtons;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.MenuStrip mnuMain;
		private System.Windows.Forms.ToolStripMenuItem mnuMain_File;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem mnuMain_File_Exit;
		private System.Windows.Forms.ToolStripMenuItem mnuMain_File_Open;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.OpenFileDialog dlgOpenFile;
		private System.Windows.Forms.Panel pnlTop;
		private System.Windows.Forms.Panel pnlClient;
		private System.Windows.Forms.TextBox txtFileContent;
		private System.Windows.Forms.Panel pnlClientButtons;
		private System.Windows.Forms.Button btnGenerateContent;
		private System.Windows.Forms.Button btnSaveToFile;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.pnlBottomButtons = new System.Windows.Forms.Panel();
			this.btnExit = new System.Windows.Forms.Button();
			this.mnuMain = new System.Windows.Forms.MenuStrip();
			this.mnuMain_File = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuMain_File_Open = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuMain_File_Exit = new System.Windows.Forms.ToolStripMenuItem();
			this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
			this.pnlTop = new System.Windows.Forms.Panel();
			this.pnlClient = new System.Windows.Forms.Panel();
			this.txtFileContent = new System.Windows.Forms.TextBox();
			this.pnlClientButtons = new System.Windows.Forms.Panel();
			this.btnGenerateContent = new System.Windows.Forms.Button();
			this.btnSaveToFile = new System.Windows.Forms.Button();
			this.pnlBottomButtons.SuspendLayout();
			this.mnuMain.SuspendLayout();
			this.pnlClient.SuspendLayout();
			this.pnlClientButtons.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlBottomButtons
			// 
			this.pnlBottomButtons.Controls.Add(this.btnExit);
			this.pnlBottomButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlBottomButtons.Location = new System.Drawing.Point(0, 469);
			this.pnlBottomButtons.Name = "pnlBottomButtons";
			this.pnlBottomButtons.Size = new System.Drawing.Size(1280, 31);
			this.pnlBottomButtons.TabIndex = 0;
			// 
			// btnExit
			// 
			this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExit.Location = new System.Drawing.Point(1193, 4);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(75, 23);
			this.btnExit.TabIndex = 0;
			this.btnExit.Text = "E&xit";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.BtnExitClick);
			// 
			// mnuMain
			// 
			this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.mnuMain_File});
			this.mnuMain.Location = new System.Drawing.Point(0, 0);
			this.mnuMain.Name = "mnuMain";
			this.mnuMain.Size = new System.Drawing.Size(1280, 24);
			this.mnuMain.TabIndex = 1;
			this.mnuMain.Text = "menuStrip1";
			// 
			// mnuMain_File
			// 
			this.mnuMain_File.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
			this.toolStripMenuItem2,
			this.mnuMain_File_Open,
			this.toolStripSeparator1,
			this.mnuMain_File_Exit});
			this.mnuMain_File.Name = "mnuMain_File";
			this.mnuMain_File.Size = new System.Drawing.Size(37, 20);
			this.mnuMain_File.Text = "&File";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(100, 6);
			// 
			// mnuMain_File_Open
			// 
			this.mnuMain_File_Open.Name = "mnuMain_File_Open";
			this.mnuMain_File_Open.Size = new System.Drawing.Size(103, 22);
			this.mnuMain_File_Open.Text = "&Open";
			this.mnuMain_File_Open.Click += new System.EventHandler(this.MnuMain_File_OpenClick);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(100, 6);
			// 
			// mnuMain_File_Exit
			// 
			this.mnuMain_File_Exit.Name = "mnuMain_File_Exit";
			this.mnuMain_File_Exit.Size = new System.Drawing.Size(103, 22);
			this.mnuMain_File_Exit.Text = "E&xit";
			this.mnuMain_File_Exit.Click += new System.EventHandler(this.MnuMain_File_ExitClick);
			// 
			// dlgOpenFile
			// 
			this.dlgOpenFile.DefaultExt = "json";
			this.dlgOpenFile.Filter = "Json Settings Files (*.json)|*.json|All Files (*.*)|*.*";
			this.dlgOpenFile.Title = "Open Settings File";
			// 
			// pnlTop
			// 
			this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlTop.Location = new System.Drawing.Point(0, 24);
			this.pnlTop.Name = "pnlTop";
			this.pnlTop.Size = new System.Drawing.Size(1280, 31);
			this.pnlTop.TabIndex = 2;
			this.pnlTop.Visible = false;
			// 
			// pnlClient
			// 
			this.pnlClient.Controls.Add(this.txtFileContent);
			this.pnlClient.Controls.Add(this.pnlClientButtons);
			this.pnlClient.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlClient.Location = new System.Drawing.Point(0, 55);
			this.pnlClient.Name = "pnlClient";
			this.pnlClient.Size = new System.Drawing.Size(1280, 414);
			this.pnlClient.TabIndex = 3;
			// 
			// txtFileContent
			// 
			this.txtFileContent.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtFileContent.Location = new System.Drawing.Point(0, 0);
			this.txtFileContent.Multiline = true;
			this.txtFileContent.Name = "txtFileContent";
			this.txtFileContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtFileContent.Size = new System.Drawing.Size(1280, 383);
			this.txtFileContent.TabIndex = 1;
			// 
			// pnlClientButtons
			// 
			this.pnlClientButtons.Controls.Add(this.btnSaveToFile);
			this.pnlClientButtons.Controls.Add(this.btnGenerateContent);
			this.pnlClientButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pnlClientButtons.Location = new System.Drawing.Point(0, 383);
			this.pnlClientButtons.Name = "pnlClientButtons";
			this.pnlClientButtons.Size = new System.Drawing.Size(1280, 31);
			this.pnlClientButtons.TabIndex = 0;
			// 
			// btnGenerateContent
			// 
			this.btnGenerateContent.Location = new System.Drawing.Point(13, 4);
			this.btnGenerateContent.Name = "btnGenerateContent";
			this.btnGenerateContent.Size = new System.Drawing.Size(75, 23);
			this.btnGenerateContent.TabIndex = 0;
			this.btnGenerateContent.Text = "&Generate";
			this.btnGenerateContent.UseVisualStyleBackColor = true;
			this.btnGenerateContent.Click += new System.EventHandler(this.BtnGenerateContentClick);
			// 
			// btnSaveToFile
			// 
			this.btnSaveToFile.Location = new System.Drawing.Point(100, 4);
			this.btnSaveToFile.Name = "btnSaveToFile";
			this.btnSaveToFile.Size = new System.Drawing.Size(75, 23);
			this.btnSaveToFile.TabIndex = 1;
			this.btnSaveToFile.Text = "Save To File";
			this.btnSaveToFile.UseVisualStyleBackColor = true;
			this.btnSaveToFile.Click += new System.EventHandler(this.BtnSaveToFileClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1280, 500);
			this.Controls.Add(this.pnlClient);
			this.Controls.Add(this.pnlTop);
			this.Controls.Add(this.pnlBottomButtons);
			this.Controls.Add(this.mnuMain);
			this.MainMenuStrip = this.mnuMain;
			this.Name = "MainForm";
			this.Text = "Make Item Classes";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.pnlBottomButtons.ResumeLayout(false);
			this.mnuMain.ResumeLayout(false);
			this.mnuMain.PerformLayout();
			this.pnlClient.ResumeLayout(false);
			this.pnlClient.PerformLayout();
			this.pnlClientButtons.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
