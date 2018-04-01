
namespace App
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
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.tbHost = new MultiCtrls.LabeledTextBox();
			this.tbPort = new MultiCtrls.LabeledTextBox();
			this.tbUser = new MultiCtrls.LabeledTextBox();
			this.tbPass = new MultiCtrls.LabeledTextBox();
			this.tbDataBaseName = new MultiCtrls.LabeledTextBox();
			this.llOpenConnAndSelect = new System.Windows.Forms.LinkLabel();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// dataGridView1
			// 
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Location = new System.Drawing.Point(12, 96);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.Size = new System.Drawing.Size(629, 347);
			this.dataGridView1.TabIndex = 0;
			// 
			// tbHost
			// 
			this.tbHost.LabelText = "Host adress";
			this.tbHost.Location = new System.Drawing.Point(98, 12);
			this.tbHost.Name = "tbHost";
			this.tbHost.offset = 5;
			this.tbHost.Size = new System.Drawing.Size(100, 20);
			this.tbHost.TabIndex = 1;
            this.tbHost.Text = "localhost";
			// 
			// tbPort
			// 
			this.tbPort.LabelText = "Port";
			this.tbPort.Location = new System.Drawing.Point(98, 38);
			this.tbPort.Name = "tbPort";
			this.tbPort.offset = 5;
			this.tbPort.Size = new System.Drawing.Size(100, 20);
			this.tbPort.TabIndex = 4;
            this.tbPort.Text = "5432";
			// 
			// tbUser
			// 
			this.tbUser.LabelText = "User";
			this.tbUser.Location = new System.Drawing.Point(301, 12);
			this.tbUser.Name = "tbUser";
			this.tbUser.offset = 5;
			this.tbUser.Size = new System.Drawing.Size(100, 20);
			this.tbUser.TabIndex = 6;
            this.tbUser.Text = "postgres";
            // 
            // tbPass
            // 
            this.tbPass.LabelText = "Password";
			this.tbPass.Location = new System.Drawing.Point(541, 12);
			this.tbPass.Name = "tbPass";
			this.tbPass.offset = 5;
			this.tbPass.Size = new System.Drawing.Size(100, 20);
			this.tbPass.TabIndex = 7;
            this.tbPass.Text = "password";
            // 
            // tbDataBaseName
            // 
            this.tbDataBaseName.LabelText = "Database name";
			this.tbDataBaseName.Location = new System.Drawing.Point(301, 38);
			this.tbDataBaseName.Name = "tbDataBaseName";
			this.tbDataBaseName.offset = 5;
			this.tbDataBaseName.Size = new System.Drawing.Size(100, 20);
			this.tbDataBaseName.TabIndex = 10;
            this.tbDataBaseName.Text = "postgres";
            // 
            // llOpenConnAndSelect
            // 
            this.llOpenConnAndSelect.AutoSize = true;
			this.llOpenConnAndSelect.Location = new System.Drawing.Point(441, 45);
			this.llOpenConnAndSelect.Name = "llOpenConnAndSelect";
			this.llOpenConnAndSelect.Size = new System.Drawing.Size(200, 13);
			this.llOpenConnAndSelect.TabIndex = 11;
			this.llOpenConnAndSelect.TabStop = true;
			this.llOpenConnAndSelect.Text = "Open connection and bake simple select";
			this.llOpenConnAndSelect.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llOpenConnAndSelect_LinkClicked);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(662, 455);
			this.Controls.Add(this.llOpenConnAndSelect);
			this.Controls.Add(this.tbDataBaseName);
			this.Controls.Add(this.tbPass);
			this.Controls.Add(this.tbUser);
			this.Controls.Add(this.tbPort);
			this.Controls.Add(this.tbHost);
			this.Controls.Add(this.dataGridView1);
			this.Name = "Minesweeper";
			this.Text = "Minesweeper";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView dataGridView1;
		private MultiCtrls.LabeledTextBox tbHost;
		private MultiCtrls.LabeledTextBox tbPort;
		private MultiCtrls.LabeledTextBox tbUser;
		private MultiCtrls.LabeledTextBox tbPass;
		private MultiCtrls.LabeledTextBox tbDataBaseName;
		private System.Windows.Forms.LinkLabel llOpenConnAndSelect;
        private System.Windows.Forms.LinkLabel selectHra;
        private System.Windows.Forms.LinkLabel insertOblast;
        private System.Windows.Forms.LinkLabel insertTah;
        private System.Windows.Forms.LinkLabel insertMina;
        private System.Windows.Forms.LinkLabel deleteMina;
    }
}

