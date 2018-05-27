namespace Zenseless.ExampleFramework
{
	partial class FormShaderException
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.listBox = new System.Windows.Forms.ListBox();
			this.richTextBox = new System.Windows.Forms.RichTextBox();
			this.buttonSave = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.listBox);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.buttonCancel);
			this.splitContainer1.Panel2.Controls.Add(this.buttonSave);
			this.splitContainer1.Panel2.Controls.Add(this.richTextBox);
			this.splitContainer1.Size = new System.Drawing.Size(726, 365);
			this.splitContainer1.SplitterDistance = 89;
			this.splitContainer1.SplitterWidth = 3;
			this.splitContainer1.TabIndex = 3;
			// 
			// listBox
			// 
			this.listBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listBox.Font = new System.Drawing.Font("Consolas", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listBox.FormattingEnabled = true;
			this.listBox.ItemHeight = 12;
			this.listBox.Location = new System.Drawing.Point(0, 0);
			this.listBox.Margin = new System.Windows.Forms.Padding(2);
			this.listBox.Name = "listBox";
			this.listBox.Size = new System.Drawing.Size(726, 89);
			this.listBox.TabIndex = 3;
			this.listBox.SelectedIndexChanged += new System.EventHandler(this.ListBox_SelectedIndexChanged);
			// 
			// richTextBox
			// 
			this.richTextBox.BackColor = System.Drawing.Color.Black;
			this.richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox.Font = new System.Drawing.Font("Consolas", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.richTextBox.ForeColor = System.Drawing.Color.White;
			this.richTextBox.Location = new System.Drawing.Point(0, 0);
			this.richTextBox.Margin = new System.Windows.Forms.Padding(2);
			this.richTextBox.Name = "richTextBox";
			this.richTextBox.Size = new System.Drawing.Size(726, 273);
			this.richTextBox.TabIndex = 2;
			this.richTextBox.Text = "";
			// 
			// buttonSave
			// 
			this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonSave.Location = new System.Drawing.Point(634, 221);
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.Size = new System.Drawing.Size(92, 23);
			this.buttonSave.TabIndex = 3;
			this.buttonSave.Text = "Save (CTRL+S)";
			this.buttonSave.UseVisualStyleBackColor = true;
			// 
			// buttonCancel
			// 
			this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(634, 250);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(92, 23);
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "Cancel (ESC)";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// FormShaderException
			// 
			this.AcceptButton = this.buttonSave;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(726, 365);
			this.Controls.Add(this.splitContainer1);
			this.KeyPreview = true;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "FormShaderException";
			this.Text = "FormShaderError";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormShaderException_FormClosing);
			this.Load += new System.EventHandler(this.FormShaderException_Load);
			this.Shown += new System.EventHandler(this.FormShaderException_Shown);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormShaderError_KeyDown);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ListBox listBox;
		private System.Windows.Forms.RichTextBox richTextBox;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonSave;
	}
}