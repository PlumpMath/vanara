﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;

// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	public static partial class Gdi32
	{
		/// <summary>The character set.</summary>
		public enum LogFontCharSet : byte
		{
			/// <summary>Specifies the English character set.</summary>
			ANSI_CHARSET = 0,

			/// <summary>
			/// Specifies a character set based on the current system locale; for example, when the system locale is United States English, the default character
			/// set is ANSI_CHARSET.
			/// </summary>
			DEFAULT_CHARSET = 1,

			/// <summary>Specifies a character set of symbols.</summary>
			SYMBOL_CHARSET = 2,

			/// <summary>Specifies the Japanese character set.</summary>
			SHIFTJIS_CHARSET = 128,

			/// <summary>Specifies the Hangul Korean character set.</summary>
			HANGEUL_CHARSET = 129,

			/// <summary>Also spelled "Hangeul". Specifies the Hangul Korean character set.</summary>
			HANGUL_CHARSET = 129,

			/// <summary>Specifies the "simplified" Chinese character set for People's Republic of China.</summary>
			GB2312_CHARSET = 134,

			/// <summary>Specifies the "traditional" Chinese character set, used mostly in Taiwan and in the Hong Kong and Macao Special Administrative Regions.</summary>
			CHINESEBIG5_CHARSET = 136,

			/// <summary>Specifies a mapping to one of the OEM code pages, according to the current system locale setting.</summary>
			OEM_CHARSET = 255,

			/// <summary>Also spelled "Johap". Specifies the Johab Korean character set.</summary>
			JOHAB_CHARSET = 130,

			/// <summary>Specifies the Hebrew character set.</summary>
			HEBREW_CHARSET = 177,

			/// <summary>Specifies the Arabic character set.</summary>
			ARABIC_CHARSET = 178,

			/// <summary>Specifies the Greek character set.</summary>
			GREEK_CHARSET = 161,

			/// <summary>Specifies the Turkish character set.</summary>
			TURKISH_CHARSET = 162,

			/// <summary>Specifies the Vietnamese character set.</summary>
			VIETNAMESE_CHARSET = 163,

			/// <summary>Specifies the Thai character set.</summary>
			THAI_CHARSET = 222,

			/// <summary>Specifies a Eastern European character set.</summary>
			EASTEUROPE_CHARSET = 238,

			/// <summary>Specifies the Russian Cyrillic character set.</summary>
			RUSSIAN_CHARSET = 204,

			/// <summary>Specifies the Apple Macintosh character set.</summary>
			MAC_CHARSET = 77,

			/// <summary>Specifies the Baltic (Northeastern European) character set.</summary>
			BALTIC_CHARSET = 186
		}

		/// <summary>The clipping precision defines how to clip characters that are partially outside the clipping region.</summary>
		public enum LogFontClippingPrecision : byte
		{
			/// <summary>Not used.</summary>
			CLIP_CHARACTER_PRECIS = 1,

			/// <summary>Specifies default clipping behavior.</summary>
			CLIP_DEFAULT_PRECIS = 0,

			/// <summary>
			/// Windows XP SP1: Turns off font association for the font. Note that this flag is not guaranteed to have any effect on any platform after Windows
			/// Server 2003.
			/// </summary>
			CLIP_DFA_DISABLE = 4 << 4,

			/// <summary>
			/// Turns off font association for the font. This is identical to CLIP_DFA_DISABLE, but it can have problems in some situations; the recommended flag
			/// to use is CLIP_DFA_DISABLE.
			/// </summary>
			CLIP_DFA_OVERRIDE = 64,

			/// <summary>You must specify this flag to use an embedded read-only font.</summary>
			CLIP_EMBEDDED = 8 << 4,

			/// <summary>
			/// When this value is used, the rotation for all fonts depends on whether the orientation of the coordinate system is left-handed or right-handed.
			/// If not used, device fonts always rotate counterclockwise, but the rotation of other fonts is dependent on the orientation of the coordinate system.
			/// </summary>
			CLIP_LH_ANGLES = 1 << 4,

			/// <summary>Not used.</summary>
			CLIP_MASK = 0xf,

			/// <summary>
			/// Not used by the font mapper, but is returned when raster, vector, or TrueType fonts are enumerated. For compatibility, this value is always
			/// returned when enumerating fonts.
			/// </summary>
			CLIP_STROKE_PRECIS = 2,

			/// <summary>Not used.</summary>
			CLIP_TT_ALWAYS = 2 << 4,
		}

		/// <summary>
		/// Font families describe the look of a font in a general way. They are intended for specifying fonts when the exact typeface desired is not available.
		/// </summary>
		public enum LogFontFontFamily : byte
		{
			/// <summary>Use default font.</summary>
			FF_DONTCARE = 0 << 4,

			/// <summary>Fonts with variable stroke width (proportional) and with serifs. MS Serif is an example.</summary>
			FF_ROMAN = 1 << 4,

			/// <summary>Fonts with variable stroke width (proportional) and without serifs. MS Sans Serif is an example.</summary>
			FF_SWISS = 2 << 4,

			/// <summary>
			/// Fonts with constant stroke width (monospace), with or without serifs. Monospace fonts are usually modern. Pica, Elite, and CourierNew are examples.
			/// </summary>
			FF_MODERN = 3 << 4,

			/// <summary>Fonts designed to look like handwriting. Script and Cursive are examples.</summary>
			FF_SCRIPT = 4 << 4,

			/// <summary>Novelty fonts. Old English is an example.</summary>
			FF_DECORATIVE = 5 << 4,
		}

		/// <summary>
		/// The output precision. The output precision defines how closely the output must match the requested font's height, width, character orientation,
		/// escapement, pitch, and font type.
		/// </summary>
		public enum LogFontOutputPrecision : byte
		{
			/// <summary>Not used.</summary>
			OUT_CHARACTER_PRECIS = 2,

			/// <summary>Specifies the default font mapper behavior.</summary>
			OUT_DEFAULT_PRECIS = 0,

			/// <summary>Instructs the font mapper to choose a Device font when the system contains multiple fonts with the same name.</summary>
			OUT_DEVICE_PRECIS = 5,

			/// <summary>This value instructs the font mapper to choose from TrueType and other outline-based fonts.</summary>
			OUT_OUTLINE_PRECIS = 8,

			/// <summary>
			/// Instructs the font mapper to choose from only PostScript fonts. If there are no PostScript fonts installed in the system, the font mapper returns
			/// to default behavior.
			/// </summary>
			OUT_PS_ONLY_PRECIS = 10,

			/// <summary>Instructs the font mapper to choose a raster font when the system contains multiple fonts with the same name.</summary>
			OUT_RASTER_PRECIS = 6,

			/// <summary>A value that specifies a preference for TrueType and other outline fonts.</summary>
			OUT_SCREEN_OUTLINE_PRECIS = 9,

			/// <summary>This value is not used by the font mapper, but it is returned when raster fonts are enumerated.</summary>
			OUT_STRING_PRECIS = 1,

			/// <summary>This value is not used by the font mapper, but it is returned when TrueType, other outline-based fonts, and vector fonts are enumerated.</summary>
			OUT_STROKE_PRECIS = 3,

			/// <summary>
			/// Instructs the font mapper to choose from only TrueType fonts. If there are no TrueType fonts installed in the system, the font mapper returns to
			/// default behavior.
			/// </summary>
			OUT_TT_ONLY_PRECIS = 7,

			/// <summary>Instructs the font mapper to choose a TrueType font when the system contains multiple fonts with the same name.</summary>
			OUT_TT_PRECIS = 4,
		}

		/// <summary>
		/// The output quality defines how carefully the graphics device interface (GDI) must attempt to match the logical-font attributes to those of an actual
		/// physical font.
		/// </summary>
		public enum LogFontOutputQuality : byte
		{
			/// <summary>Appearance of the font does not matter.</summary>
			DEFAULT_QUALITY = 0,

			/// <summary>
			/// Appearance of the font is less important than when PROOF_QUALITY is used. For GDI raster fonts, scaling is enabled, which means that more font
			/// sizes are available, but the quality may be lower. Bold, italic, underline, and strikeout fonts are synthesized if necessary.
			/// </summary>
			DRAFT_QUALITY = 1,

			/// <summary>
			/// Character quality of the font is more important than exact matching of the logical-font attributes. For GDI raster fonts, scaling is disabled and
			/// the font closest in size is chosen. Although the chosen font size may not be mapped exactly when PROOF_QUALITY is used, the quality of the font
			/// is high and there is no distortion of appearance. Bold, italic, underline, and strikeout fonts are synthesized if necessary.
			/// </summary>
			PROOF_QUALITY = 2,

			/// <summary>Font is never antialiased.</summary>
			NONANTIALIASED_QUALITY = 3,

			/// <summary>Font is always antialiased if the font supports it and the size of the font is not too small or too large.</summary>
			ANTIALIASED_QUALITY = 4,

			/// <summary>
			/// If set, text is rendered (when possible) using ClearType antialiasing method. The font quality is given less importance than maintaining the text size.
			/// </summary>
			CLEARTYPE_QUALITY = 5,

			/// <summary>
			/// If set, text is rendered (when possible) using ClearType antialiasing method. The font quality is given more importance than maintaining the text size.
			/// </summary>
			CLEARTYPE_NATURAL_QUALITY = 6
		}

		/// <summary>The pitch of the font.</summary>
		public enum LogFontPitch : byte
		{
			/// <summary>The default pitch.</summary>
			DEFAULT_PITCH = 0,

			/// <summary>The pitch is fixed.</summary>
			FIXED_PITCH = 1,

			/// <summary>The pitch is variable.</summary>
			VARIABLE_PITCH = 2
		}

		/// <summary>The LOGFONT structure defines the attributes of a font.</summary>
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct LOGFONT
		{
			/// <summary>
			/// The height, in logical units, of the font's character cell or character. The character height value (also known as the em height) is the
			/// character cell height value minus the internal-leading value. The font mapper interprets the value specified in lfHeight in the following manner.
			/// <list type="table">
			/// <listheader>
			/// <term>Value</term>
			/// <definition>Meaning</definition>
			/// </listheader>
			/// <item>
			/// <term>&gt; 0</term>
			/// <definition>The font mapper transforms this value into device units and matches it against the cell height of the available fonts.</definition>
			/// </item>
			/// <item>
			/// <term>0</term>
			/// <definition></definition> The font mapper uses a default height value when it searches for a match.
			/// </item>
			/// <item>
			/// <term>&lt; 0</term>
			/// <definition>The font mapper transforms this value into device units and matches its absolute value against the character height of the available fonts.</definition>
			/// </item>
			/// </list>
			/// <para>For all height comparisons, the font mapper looks for the largest font that does not exceed the requested size.</para>
			/// <para>This mapping occurs when the font is used for the first time.</para>
			/// <para>For the MM_TEXT mapping mode, you can use the following formula to specify a height for a font with a specified point size:</para>
			/// </summary>
			public int lfHeight;

			/// <summary>
			/// The average width, in logical units, of characters in the font. If lfWidth is zero, the aspect ratio of the device is matched against the
			/// digitization aspect ratio of the available fonts to find the closest match, determined by the absolute value of the difference.
			/// </summary>
			public int lfWidth;

			/// <summary>
			/// The angle, in tenths of degrees, between the escapement vector and the x-axis of the device. The escapement vector is parallel to the base line
			/// of a row of text.
			/// <para>
			/// When the graphics mode is set to GM_ADVANCED, you can specify the escapement angle of the string independently of the orientation angle of the
			/// string's characters.
			/// </para>
			/// <para>
			/// When the graphics mode is set to GM_COMPATIBLE, lfEscapement specifies both the escapement and orientation. You should set lfEscapement and
			/// lfOrientation to the same value.
			/// </para>
			/// </summary>
			public int lfEscapement;

			/// <summary>The angle, in tenths of degrees, between each character's base line and the x-axis of the device.</summary>
			public int lfOrientation;

			/// <summary>
			/// The weight of the font in the range 0 through 1000. For example, 400 is normal and 700 is bold. If this value is zero, a default weight is used.
			/// </summary>
			private int _lfWeight;

			/// <summary>An italic font if set to TRUE.</summary>
			private byte _lfItalic;

			/// <summary>An underlined font if set to TRUE.</summary>
			private byte _lfUnderline;

			/// <summary>A strikeout font if set to TRUE.</summary>
			private byte _lfStrikeOut;

			/// <summary>The character set.</summary>
			public LogFontCharSet lfCharSet;

			/// <summary>
			/// The output precision. The output precision defines how closely the output must match the requested font's height, width, character orientation,
			/// escapement, pitch, and font type.
			/// </summary>
			public LogFontOutputPrecision lfOutPrecision;

			/// <summary>The clipping precision. The clipping precision defines how to clip characters that are partially outside the clipping region.</summary>
			public LogFontClippingPrecision lfClipPrecision;

			/// <summary>
			/// The output quality. The output quality defines how carefully the graphics device interface (GDI) must attempt to match the logical-font
			/// attributes to those of an actual physical font.
			/// </summary>
			public LogFontOutputQuality lfQuality;

			/// <summary>The pitch and family of the font.</summary>
			private byte lfPitchAndFamily;

			/// <summary>
			/// A null-terminated string that specifies the typeface name of the font. The length of this string must not exceed 32 TCHAR values, including the
			/// terminating NULL. The EnumFontFamiliesEx function can be used to enumerate the typeface names of all currently available fonts. If lfFaceName is
			/// an empty string, GDI uses the first font that matches the other specified attributes.
			/// </summary>
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
			private string _lfFaceName;

			/// <summary>Gets or sets the font family.</summary>
			/// <value>The font family.</value>
			public LogFontFontFamily FontFamily
			{
				get { return (LogFontFontFamily)(lfPitchAndFamily & 0xF0); }
				set { lfPitchAndFamily = (byte)((lfPitchAndFamily & 0x0F) | (byte)value); }
			}

			/// <summary>
			/// A string that specifies the typeface name of the font. The length of this string must not exceed 31 characters. The EnumFontFamiliesEx function
			/// can be used to enumerate the typeface names of all currently available fonts. If lfFaceName is an empty string, GDI uses the first font that
			/// matches the other specified attributes.
			/// </summary>
			/// <value>The face name of the font.</value>
			public string lfFaceName
			{
				get { return _lfFaceName; }
				set
				{
					if (value?.Length > 31)
						throw new ArgumentException(@"The face name may not have more than 31 characters.", nameof(lfFaceName));
					_lfFaceName = value;
				}
			}

			/// <summary>Gets or sets a value indicating whether this <see cref="LOGFONT"/> is italicized.</summary>
			/// <value><c>true</c> if italicized; otherwise, <c>false</c>.</value>
			public bool lfItalic
			{
				get { return _lfItalic == 1; }
				set { _lfItalic = System.Convert.ToByte(value); }
			}

			/// <summary>Gets or sets a value indicating whether struck out.</summary>
			/// <value><c>true</c> if struck out; otherwise, <c>false</c>.</value>
			public bool lfStrikeOut
			{
				get { return _lfStrikeOut == 1; }
				set { _lfStrikeOut = System.Convert.ToByte(value); }
			}

			/// <summary>Gets or sets a value indicating whether this <see cref="LOGFONT"/> is underlined.</summary>
			/// <value><c>true</c> if underlined; otherwise, <c>false</c>.</value>
			public bool lfUnderline
			{
				get { return _lfUnderline == 1; }
				set { _lfUnderline = System.Convert.ToByte(value); }
			}

			/// <summary>
			/// The weight of the font in the range 0 through 1000. For example, 400 is normal and 700 is bold. If this value is zero, a default weight is used.
			/// </summary>
			public short lfWeight
			{
				get { return (short)_lfWeight; }
				set
				{
					if (value < 0 || value > 1000)
						throw new ArgumentOutOfRangeException(nameof(lfWeight), @"Font weight must be a value in the range 0 through 1000.");
					_lfWeight = value;
				}
			}

			/// <summary>Gets or sets the font pitch.</summary>
			/// <value>The pitch.</value>
			public LogFontPitch Pitch
			{
				get { return (LogFontPitch)(lfPitchAndFamily & 0x0F); }
				set { lfPitchAndFamily = (byte)((lfPitchAndFamily & 0xF0) | (byte)value); }
			}

			/// <summary>Converts this structure to a <see cref="Font"/>.</summary>
			/// <returns>A <see cref="Font"/> matching the values of this structure.</returns>
			public Font ToFont()
			{
				try { return Font.FromLogFont(this); }
				catch { return new Font(lfFaceName, lfHeight, FontStyle.Regular, GraphicsUnit.Display); }
			}

			/// <summary>Returns a <see cref="System.String"/> that represents this instance.</summary>
			/// <returns>A <see cref="System.String"/> that represents this instance.</returns>
			public override string ToString() => $"lfHeight={lfHeight}, lfWidth={lfWidth}, lfEscapement={lfEscapement}, lfOrientation={lfOrientation}, lfWeight={lfWeight}, lfItalic={lfItalic}, lfUnderline={lfUnderline}, lfStrikeOut={lfStrikeOut}, lfCharSet={lfCharSet}, lfOutPrecision={lfOutPrecision}, lfClipPrecision={lfClipPrecision}, lfQuality={lfQuality}, lfPitchAndFamily={lfPitchAndFamily}, lfFaceName={lfFaceName}";

			/// <summary>Gets a <see cref="LOGFONT"/> structure from a <see cref="Font"/>.</summary>
			/// <param name="font">The font.</param>
			/// <returns>A <see cref="LOGFONT"/> structure.</returns>
			public static LOGFONT FromFont(Font font)
			{
				if (font == null)
					throw new System.ArgumentNullException(nameof(font));
				var lf = default(LOGFONT);
				font.ToLogFont(lf);
				return lf;
			}
		}
	}
}