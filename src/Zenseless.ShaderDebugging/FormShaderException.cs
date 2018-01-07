using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Zenseless.HLGL;

namespace Zenseless.ShaderDebugging
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="System.Windows.Forms.Form" />
	public partial class FormShaderException : Form
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FormShaderException"/> class.
		/// </summary>
		public FormShaderException()
		{
			InitializeComponent();
			listBox.DataSource = errors;
			listBox.MouseWheel += OnMouseWheel;
			richTextBox.MouseWheel += OnMouseWheel;
		}

		/// <summary>
		/// Gets the errors.
		/// </summary>
		/// <value>
		/// The errors.
		/// </value>
		public BindingList<ShaderLogLine> Errors { get { return errors; } }

		/// <summary>
		/// Gets or sets the shader source code.
		/// </summary>
		/// <value>
		/// The shader source code.
		/// </value>
		public string ShaderSourceCode { get { return richTextBox.Text; } set { richTextBox.Text = value; } }

		/// <summary>
		/// Selects the specified identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		public void Select(int id)
		{
			listBox.SelectedIndex = id;
			ListBox_SelectedIndexChanged(null, null);
		}

		/// <summary>
		/// Gets or sets the size of the font.
		/// </summary>
		/// <value>
		/// The size of the font.
		/// </value>
		public float FontSize
		{
			get
			{
				return listBox.Font.Size;
			}
			set
			{
				var size = Math.Max(6, value);
				var font = new Font(listBox.Font.FontFamily, size);
				listBox.Font = font;
				richTextBox.Font = font;
			}
		}

		/// <summary>
		/// The errors
		/// </summary>
		private BindingList<ShaderLogLine> errors = new BindingList<ShaderLogLine>();

		/// <summary>
		/// Called when [mouse wheel].
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="MouseEventArgs"/> instance containing the event data.</param>
		private void OnMouseWheel(object sender, MouseEventArgs e)
		{
			if (Keys.Control == ModifierKeys)
			{
				FontSize += Math.Sign(e.Delta) * 2;
			}
		}

		/// <summary>
		/// Handles the KeyDown event of the FormShaderError control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="KeyEventArgs"/> instance containing the event data.</param>
		private void FormShaderError_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Escape: buttonCancel.PerformClick(); break;
				case Keys.S: if (e.Control) buttonSave.PerformClick(); break;
			}
		}

		/// <summary>
		/// Handles the SelectedIndexChanged event of the listBox control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void ListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			richTextBox.Select(0, richTextBox.Text.Length);
			richTextBox.SelectionColor = Color.White;
			try
			{
				var logLine = errors[listBox.SelectedIndex];
				var nr = logLine.LineNumber - 1;
				int start = richTextBox.GetFirstCharIndexFromLine(nr);
				int length = richTextBox.Lines[nr].Length;
				richTextBox.Select(start, length);
				richTextBox.SelectionBackColor = Color.DarkRed;
				richTextBox.ScrollToCaret();
			}
			catch { }
		}

		/// <summary>
		/// Handles the FormClosing event of the FormShaderException control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
		private void FormShaderException_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.SaveLayout();
			RegistryLoader.SaveValue(Name, "fontSize", FontSize);
		}

		/// <summary>
		/// Handles the Load event of the FormShaderException control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void FormShaderException_Load(object sender, EventArgs e)
		{
			this.LoadLayout();
			FontSize = (float)Convert.ToDouble(RegistryLoader.LoadValue(Name, "fontSize", 12.0f));
		}

		/// <summary>
		/// Handles the Shown event of the FormShaderException control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void FormShaderException_Shown(object sender, EventArgs e)
		{
			TopMost = false;
		}
	}
}
