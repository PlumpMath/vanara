using System.Drawing;
using System.Runtime.InteropServices;

namespace Vanara.PInvoke
{
	/// <summary>The SIZE structure specifies the width and height of a rectangle.</summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct SIZE
	{
		/// <summary>Specifies the rectangle's width. The units depend on which function uses this.</summary>
		public int cx;
		/// <summary>Specifies the rectangle's height. The units depend on which function uses this.</summary>
		public int cy;

		/// <summary>Initializes a new instance of the <see cref="SIZE"/> struct.</summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		public SIZE(int width, int height)
		{
			cx = width;
			cy = height;
		}

		/// <summary>Converts this structure to a <see cref="System.Drawing.Size"/> structure.</summary>
		/// <returns>An equivalent <see cref="System.Drawing.Size"/> structure.</returns>
		public Size ToSize() => this;

		/// <summary>Performs an implicit conversion from <see cref="SIZE"/> to <see cref="Size"/>.</summary>
		/// <param name="s">The <see cref="SIZE"/>.</param>
		/// <returns>The <see cref="Size"/> result of the conversion.</returns>
		public static implicit operator Size(SIZE s) => new Size(s.cx, s.cy);

		/// <summary>Performs an implicit conversion from <see cref="Size"/> to <see cref="SIZE"/>.</summary>
		/// <param name="s">The <see cref="Size"/>.</param>
		/// <returns>The <see cref="SIZE"/> result of the conversion.</returns>
		public static implicit operator SIZE(Size s) => new SIZE(s.Width, s.Height);
	}
}