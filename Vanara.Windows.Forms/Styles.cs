using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Vanara.Windows.Forms
{
	public enum RenderStyle
	{
		SystemTheme,
		Custom
	}

	public interface IDrawingStyle<in TCtrl, in TEnum> where TEnum : struct, IConvertible where TCtrl : Control
	{
		void Draw(TCtrl ctrl, TEnum state, PaintEventArgs e);
		Size Measure(TCtrl ctrl, TEnum state, Graphics g = null);
	}
}
