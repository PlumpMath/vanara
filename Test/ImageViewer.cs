using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Vanara.Drawing;

namespace Test
{
	public class ImageViewer : ScrollableControl
	{
		private const int chkboxSz = 8;
		private const int icoSpacing = 4;
		private Bitmap checks;
		private TextureBrush checkBrush;
		private Icon icon;
		private Image image;
		private Size imageSz;
		private float magnification = 1.0f;
		private List<Bitmap> icons;

		public ImageViewer()
		{
			AutoScroll = true;
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
		}

		[DefaultValue(null)]
		public Icon Icon
		{
			get { return icon; }
			set
			{
				icon = value;
				image = null;
				var subIcons = icon?.Split();
				icons = new List<Bitmap>();
				if (subIcons != null && subIcons.Length > 0)
				{
					Array.Sort(subIcons, (a, b) => a.Height != b.Height ? a.Height - b.Height : a.GetBitCount() - b.GetBitCount());
					int w = 0, h = 0, ch = 0, rw = 0;
					foreach (var ico in subIcons)
					{
						if (ico.Height != ch) { rw = icoSpacing; h += ch + icoSpacing; ch = ico.Height; }
						rw += ico.Width + icoSpacing;
						w = Math.Max(w, rw);
						icons.Add(ico.ToAlphaBitmap());
					}
					imageSz = new Size(w, h + ch + icoSpacing);
					Debug.Write($"ImgVw.Icon: Sz={imageSz}; BmpClr={string.Join(",", icons.Select(i => i.PixelFormat.ToString()))};");
					Debug.WriteLine($"Icon heights={string.Join(",", subIcons.Select(i => i.Height.ToString()))}; Icon colors={string.Join(",", subIcons.Select(i => i.GetBitCount().ToString()))}");
				}
				else
					imageSz = Size.Empty;
				Refresh();
			}
		}

		[DefaultValue(null)]
		public Image Image
		{
			get { return image; }
			set
			{
				image = value;
				icon = null;
				imageSz = image?.Size ?? Size.Empty;
				Refresh();
			}
		}

		[DefaultValue(1.0f)]
		public float Magnification
		{
			get { return magnification; }
			// ReSharper disable once CompareOfFloatsByEqualityOperator
			set { if (value == magnification) return; magnification = value; Refresh(); }
		}

		[DefaultValue(true)]
		public bool UseCheckerboardBackground { get; set; } = true;

		protected override void OnPaintBackground(PaintEventArgs e) { }

		protected override void OnPaint(PaintEventArgs pe)
		{
			Debug.Write($"ImageViewer.OnPaint: Pos={AutoScrollPosition};Off={AutoScrollOffset}");
			AutoScrollMinSize = Scale(imageSz);
			Debug.WriteLine($";Sz={AutoScrollMinSize}");

			pe.Graphics.ScaleTransform(Magnification, Magnification);
			pe.Graphics.TranslateTransform(AutoScrollPosition.X, AutoScrollPosition.Y);

			var r = new Rectangle(Point.Empty, imageSz);
			if (UseCheckerboardBackground)
				PaintBackgroundCheckerboard(pe.Graphics, r);
			else
				pe.Graphics.Clear(BackColor);

			if (Image != null)
				pe.Graphics.DrawImage(Image, r);
			else if (Icon != null)
			{
				int x = 0, y = 0, ch = 0;
				foreach (var ico in icons)
				{
					if (ico.Height != ch) { x = icoSpacing; y += ch + icoSpacing; ch = ico.Height; }
					pe.Graphics.DrawImage(ico, x, y);
					x += ico.Width + icoSpacing;
				}
			}
			base.OnPaint(pe);
		}

		protected override void OnResize(EventArgs e) { base.OnResize(e); Invalidate(); }

		protected override void OnScroll(ScrollEventArgs se) { base.OnScroll(se); Invalidate(); }

		private void MakeChecks()
		{
			checks = new Bitmap(chkboxSz * 2, chkboxSz * 2);
			using (var g = Graphics.FromImage(checks))
			{
				g.Clear(Color.White);
				g.FillRectangle(SystemBrushes.ControlLight, chkboxSz, 0, chkboxSz, chkboxSz);
				g.FillRectangle(SystemBrushes.ControlLight, 0, chkboxSz, chkboxSz, chkboxSz);
			}
			checkBrush = new TextureBrush(checks);
		}

		private void PaintBackgroundCheckerboard(Graphics g, Rectangle r)
		{
			if (checks == null) MakeChecks();
			g.FillRectangle(checkBrush, r);
		}

		private Size Scale(Size inSz) => new Size((int)Math.Round(inSz.Width * Magnification), (int)Math.Round(inSz.Height * Magnification));
	}
}