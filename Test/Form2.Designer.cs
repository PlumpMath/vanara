namespace Test
{
	partial class Form2
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
			this.ipAddressBox1 = new Vanara.Windows.Forms.IPAddressBox();
			this.SuspendLayout();
			// 
			// ipAddressBox1
			// 
			this.ipAddressBox1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
			this.ipAddressBox1.Location = new System.Drawing.Point(11, 22);
			this.ipAddressBox1.Name = "ipAddressBox1";
			this.ipAddressBox1.Size = new System.Drawing.Size(103, 22);
			this.ipAddressBox1.TabIndex = 0;
			this.ipAddressBox1.Text = "192.168.0.1";
			// 
			// Form2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(248, 245);
			this.Controls.Add(this.ipAddressBox1);
			this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "Form2";
			this.Text = "Form2";
			this.ResumeLayout(false);

		}

		#endregion

		private Vanara.Windows.Forms.IPAddressBox ipAddressBox1;
	}
}