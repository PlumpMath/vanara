using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Test
{
	public partial class ImageDialog : Form
	{
		private int zoomIdx = 5;
		private static readonly int[] zoomVals = { 25, 33, 50, 67, 75, 100, 125, 150, 200, 300, 400, 800 };

		public ImageDialog()
		{
			InitializeComponent();
		}

		[DefaultValue(null)]
		public Icon ClientIcon
		{
			get { return imageViewer1.Icon; }
			set { imageViewer1.Icon = value; }
		}

		[DefaultValue(null)]
		public Image ClientImage
		{
			get { return imageViewer1.Image; }
			set { imageViewer1.Image = value; }
		}

		private void ImageDialog_Load(object sender, EventArgs e)
		{

		}

		private void zoomOutButton_Click(object sender, EventArgs e) { zoomIdx = Math.Max(0, zoomIdx - 1); ApplyZoom(); }

		private void ApplyZoom()
		{
			imageViewer1.Magnification = zoomVals[zoomIdx] / 100f;
			toolStripLabel1.Text = $"{zoomVals[zoomIdx]}%";
			Refresh();
		}

		private void zoomInButton_Click(object sender, EventArgs e) { zoomIdx = Math.Min(11, zoomIdx + 1); ApplyZoom(); }
	}
}
