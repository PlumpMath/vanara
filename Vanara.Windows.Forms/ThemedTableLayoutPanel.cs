﻿using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Vanara.Extensions;

namespace Vanara.Windows.Forms
{
	/// <summary>
	/// A table layout panel that supports a glass overlay.
	/// </summary>
	[ToolboxItem(true), System.Drawing.ToolboxBitmap(typeof(ThemedTableLayoutPanel), "ThemedTableLayoutPanel.bmp")]
	public class ThemedTableLayoutPanel : TableLayoutPanel
	{
		private VisualStyleRenderer rnd;
		private string styleClass;
		private int stylePart;
		private int styleState;
		private bool supportGlass;

		/// <summary>
		/// Initializes a new instance of the <see cref="ThemedTableLayoutPanel"/> class.
		/// </summary>
		public ThemedTableLayoutPanel()
		{
			SetTheme("WINDOW", 29, 1);
		}

		/// <summary>Gets or sets the style class.</summary>
		/// <value>The style class.</value>
		[DefaultValue("WINDOW"), Category("Appearance")]
		public string StyleClass
		{
			get { return styleClass; }
			set { styleClass = value; ResetTheme(); Invalidate(); }
		}

		/// <summary>Gets or sets the style part.</summary>
		/// <value>The style part.</value>
		[DefaultValue(29), Category("Appearance")]
		public int StylePart
		{
			get { return stylePart; }
			set { stylePart = value; ResetTheme(); Invalidate(); }
		}

		/// <summary>Gets or sets the style part.</summary>
		/// <value>The style part.</value>
		[DefaultValue(1), Category("Appearance")]
		public int StyleState
		{
			get { return styleState; }
			set { styleState = value; ResetTheme(); Invalidate(); }
		}

		/// <summary>Gets or sets a value indicating whether this table supports glass (can be enclosed in the glass margin).</summary>
		/// <value><c>true</c> if supports glass; otherwise, <c>false</c>.</value>
		[DefaultValue(false), Category("Appearance")]
		public bool SupportGlass
		{
			get { return supportGlass; }
			set { supportGlass = value; Invalidate(); }
		}

		/// <summary>
		/// Sets the theme using theme class information.
		/// </summary>
		/// <param name="className">Name of the theme class.</param>
		/// <param name="part">The theme part.</param>
		/// <param name="state">The theme state.</param>
		public void SetTheme(string className, int part, int state)
		{
			styleClass = className;
			stylePart = part;
			styleState = state;
			ResetTheme();
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Paint" /> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (!this.IsDesignMode() && SupportGlass && DesktopWindowManager.IsCompositionEnabled())
				try { e.Graphics.Clear(Color.Black); } catch { }
			else
			{
				if (rnd == null || !Application.RenderWithVisualStyles)
					try { e.Graphics.Clear(BackColor); } catch { }
				else
					rnd.DrawBackground(e.Graphics, ClientRectangle, e.ClipRectangle);
			}
			base.OnPaint(e);
		}

		private void ResetTheme()
		{
			if (VisualStyleRenderer.IsSupported)
			{
				try
				{
					if (rnd == null)
						rnd = new VisualStyleRenderer(styleClass, stylePart, styleState);
					else
						rnd.SetParameters(styleClass, stylePart, styleState);
				}
				catch
				{
					rnd = null;
				}
			}
			else
				rnd = null;
		}
	}
}