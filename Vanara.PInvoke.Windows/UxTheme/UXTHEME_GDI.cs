using System.Runtime.InteropServices;
using static Vanara.PInvoke.Gdi32;

namespace Vanara.PInvoke
{
	public static partial class UxTheme
	{
		[DllImport(nameof(UxTheme), ExactSpelling = true, CharSet = CharSet.Unicode)]
		public static extern int GetThemeFont(SafeThemeHandle hTheme, Gdi32.SafeDCHandle hdc, int iPartId, int iStateId, int iPropId, out Gdi32.LOGFONT pFont);

		[DllImport(nameof(UxTheme), ExactSpelling = true, CharSet = CharSet.Unicode)]
		public static extern int GetThemeSysFont(SafeThemeHandle hTheme, int iFontId, out Gdi32.LOGFONT pFont);
	}
}