﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Vanara.Windows.Forms
{
	/// <summary>
	/// An input dialog that automatically creates controls to collect the values of the object supplied via the <see cref="Data"/> property.
	/// </summary>
	public class InputDialog : CommonDialog
	{
		private object data;

		/// <summary>
		/// Gets or sets the data for the input dialog box. The data type will determine the type of input mechanism displayed. For simple types, a <see
		/// cref="TextBox"/> with validation, or a <see cref="CheckBox"/> or a <see cref="ComboBox"/> will be displayed. For classes and structures, all of the
		/// public, top-level, fields and properties will have input mechanisms shown for each. See Remarks for more detail.
		/// </summary>
		/// <value>The data for the input dialog box.</value>
		/// <remarks>TBD</remarks>
		[DefaultValue(null), Browsable(false), Category("Data"), Description("The data for the input dialog box.")]
		public object Data
		{
			get { return data; }
			set { data = value; }
		}

		/// <summary>Gets or sets the image to display on the top left corner of the dialog. This value can be <c>null</c> to display no image.</summary>
		/// <value>The image to display on the top left corner of the dialog.</value>
		[DefaultValue(null), Category("Appearance"), Description("The image to display on the top left corner of the dialog.")]
		public Image Image { get; set; }

		/// <summary>Gets or sets the text prompt to display above all input options. This value can be <c>null</c>.</summary>
		/// <value>The text prompt to display above all input options.</value>
		[DefaultValue(null), Category("Appearance"), Description("The text prompt to display above all input options.")]
		public string Prompt { get; set; }

		/// <summary>Gets or sets the input dialog box title.</summary>
		/// <value>The input dialog box title. The default value is an empty string ("").</value>
		[DefaultValue(""), Category("Window"), Description("The input dialog box title.")]
		public string Title { get; set; } = "";

		/// <summary>Displays an input dialog in front of the specified object and with the specified prompt, caption, data, and image.</summary>
		/// <param name="owner">An implementation of <see cref="IWin32Window"/> that will own the modal dialog box.</param>
		/// <param name="prompt">The text prompt to display above all input options. This value can be <c>null</c>.</param>
		/// <param name="caption">The caption for the dialog.</param>
		/// <param name="data">
		/// The data for the input. The data type will determine the type of input mechanism displayed. For simple types, a <see cref="TextBox"/> with
		/// validation, or a <see cref="CheckBox"/> or a <see cref="ComboBox"/> will be displayed. For classes and structures, all of the public, top-level,
		/// fields and properties will have input mechanisms shown for each. See Remarks for more detail.
		/// </param>
		/// <param name="image">The image to display on the top left corner of the dialog. This value can be <c>null</c> to display no image.</param>
		/// <param name="width">The desired width of the <see cref="InternalInputDialog"/>. A value of <c>0</c> indicates a default width.</param>
		/// <returns>
		/// Either <see cref="DialogResult.OK"/> or <see cref="DialogResult.Cancel"/>. On OK, the <paramref name="data"/> parameter will include the updated
		/// values from the <see cref="InternalInputDialog"/>.
		/// </returns>
		/// <remarks></remarks>
		public static DialogResult Show(IWin32Window owner, string prompt, string caption, ref object data, Image image = null, int width = 0)
		{
			using (var dlg = new InternalInputDialog(prompt, caption, image, data, width))
			{
				var ret = owner == null ? dlg.ShowDialog() : dlg.ShowDialog(owner);
				if (ret == DialogResult.OK)
					data = dlg.Data;
				return ret;
			}
		}

		/// <summary>Displays an input dialog with the specified prompt, caption, data, and image.</summary>
		/// <param name="prompt">The text prompt to display above all input options. This value can be <c>null</c>.</param>
		/// <param name="caption">The caption for the dialog.</param>
		/// <param name="data">
		/// The data for the input. The data type will determine the type of input mechanism displayed. For simple types, a <see cref="TextBox"/> with
		/// validation, or a <see cref="CheckBox"/> or a <see cref="ComboBox"/> will be displayed. For classes and structures, all of the public, top-level,
		/// fields and properties will have input mechanisms shown for each. See Remarks for more detail.
		/// </param>
		/// <param name="image">The image to display on the top left corner of the dialog. This value can be <c>null</c> to display no image.</param>
		/// <param name="width">The desired width of the <see cref="InternalInputDialog"/>. A value of <c>0</c> indicates a default width.</param>
		/// <returns>
		/// Either <see cref="DialogResult.OK"/> or <see cref="DialogResult.Cancel"/>. On OK, the <paramref name="data"/> parameter will include the updated
		/// values from the <see cref="InternalInputDialog"/>.
		/// </returns>
		/// <remarks></remarks>
		public static DialogResult Show(string prompt, string caption, ref object data, Image image = null, int width = 0) => Show(null, prompt, caption, ref data, image, width);

		/// <summary>
		/// Resets all properties to their default values.
		/// </summary>
		public override void Reset() { }

		/// <summary>
		/// <para>This API supports the.NET Framework infrastructure and is not intended to be used directly from your code.</para>
		/// <para>Specifies a common dialog box.</para>
		/// </summary>
		/// <param name="hwndOwner">A value that represents the window handle of the owner window for the common dialog box.</param>
		/// <returns><c>true</c> if the data was collected; otherwise, <c>false</c>.</returns>
		protected override bool RunDialog(IntPtr hwndOwner) => Show(NativeWindow.FromHandle(hwndOwner), Prompt, Title, ref data, Image) == DialogResult.OK;

		/// <summary>
		/// Get input based on automatic interpretation of Data object.
		/// </summary>
		internal class InternalInputDialog : Form
		{
			private const int prefWidth = 340;

			private static readonly Size minSize = new Size(193, 104);

			private static readonly Dictionary<Type, char[]> keyPressValidChars = new Dictionary<Type, char[]>
			{
				[typeof(byte)] = GetCultureChars(true, false, true),
				[typeof(sbyte)] = GetCultureChars(true, true, true),
				[typeof(short)] = GetCultureChars(true, true, true),
				[typeof(ushort)] = GetCultureChars(true, false, true),
				[typeof(int)] = GetCultureChars(true, true, true),
				[typeof(uint)] = GetCultureChars(true, false, true),
				[typeof(long)] = GetCultureChars(true, true, true),
				[typeof(ulong)] = GetCultureChars(true, false, true),
				[typeof(double)] = GetCultureChars(true, true, true, true, true, true),
				[typeof(float)] = GetCultureChars(true, true, true, true, true, true),
				[typeof(decimal)] = GetCultureChars(true, true, true, true, true),
				[typeof(TimeSpan)] = GetCultureChars(true, true, false, new[] { '-' }),
				[typeof(Guid)] = GetCultureChars(true, false, false, "-{}()".ToCharArray()),
			};

			private static readonly Type[] simpleTypes = new[] { typeof(Enum), typeof(Decimal), typeof(DateTime),
			typeof(DateTimeOffset), typeof(String), typeof(TimeSpan), typeof(Guid) };

			private static readonly Dictionary<Type, Predicate<string>> validations = new Dictionary<Type, Predicate<string>>
			{
				[typeof(byte)] = s => { byte n; return byte.TryParse(s, out n); },
				[typeof(sbyte)] = s => { sbyte n; return sbyte.TryParse(s, out n); },
				[typeof(short)] = s => { short n; return short.TryParse(s, out n); },
				[typeof(ushort)] = s => { ushort n; return ushort.TryParse(s, out n); },
				[typeof(int)] = s => { int n; return int.TryParse(s, out n); },
				[typeof(uint)] = s => { uint n; return uint.TryParse(s, out n); },
				[typeof(long)] = s => { long n; return long.TryParse(s, out n); },
				[typeof(ulong)] = s => { ulong n; return ulong.TryParse(s, out n); },
				[typeof(char)] = s => { char n; return char.TryParse(s, out n); },
				[typeof(double)] = s => { double n; return double.TryParse(s, out n); },
				[typeof(float)] = s => { float n; return float.TryParse(s, out n); },
				[typeof(decimal)] = s => { decimal n; return decimal.TryParse(s, out n); },
				[typeof(DateTime)] = s => { DateTime n; return DateTime.TryParse(s, out n); },
				[typeof(TimeSpan)] = s => { TimeSpan n; return TimeSpan.TryParse(s, out n); },
				[typeof(Guid)] = s => { try { var n = new Guid(s); return true; } catch { return false; } },
			};

			private Panel borderPanel;
			private TableLayoutPanel buttonPanel;
			private Button cancelBtn;
			private IContainer components;
			private object dataObj;
			private ErrorProvider errorProvider;
			private Image image;
			private readonly List<MemberInfo> items = new List<MemberInfo>();
			private Button okBtn;
			private string prompt;
			private TableLayoutPanel table;

			/// <summary>
			/// Initializes a new instance of the <see cref="InternalInputDialog"/> class.
			/// </summary>
			public InternalInputDialog()
			{
				InitializeComponent();
			}

			internal InternalInputDialog(string prompt, string caption, Image image, object data, int width) : this()
			{
				Width = width;
				this.prompt = prompt;
				base.Text = caption;
				this.image = image;
				Data = data;
			}

			/// <summary>Gets or sets the data.</summary>
			/// <value>The data.</value>
			[DefaultValue(null), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public object Data
			{
				get { return dataObj; }
				set
				{
					if (value == null)
						throw new ArgumentNullException(nameof(Data));

					items.Clear();

					if (IsSimpleType(value.GetType()))
						items.Add(null);
					else
					{
						foreach (var mi in value.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public))
						{
							if (GetAttr(mi)?.Hidden ?? false)
								continue;
							var fi = mi as FieldInfo;
							var pi = mi as PropertyInfo;
							if (fi != null && IsSupportedType(fi.FieldType))
							{
								items.Add(fi);
							}
							else if (pi != null && IsSupportedType(pi.PropertyType) && pi.GetIndexParameters().Length == 0 && pi.CanWrite)
							{
								items.Add(pi);
							}
						}

						items.Sort((x, y) => (GetAttr(x)?.Order ?? int.MaxValue) - (GetAttr(y)?.Order ?? int.MaxValue));
					}

					dataObj = value;

					BuildTable();
				}
			}

			/// <summary>Gets or sets the image.</summary>
			/// <value>The image.</value>
			[DefaultValue(null)]
			public Image Image
			{
				get { return image; }
				set
				{
					if (image != value)
					{
						image = value;
						BuildTable();
					}
				}
			}

			/// <summary>Gets or sets the prompt.</summary>
			/// <value>The prompt.</value>
			[DefaultValue(null)]
			public string Prompt
			{
				get { return prompt; }
				set
				{
					if (prompt != value)
					{
						prompt = value;
						BuildTable();
					}
				}
			}

			/// <summary>
			/// Gets or sets the width of the control.
			/// </summary>
			public new int Width
			{
				get { return base.Width; }
				set
				{
					if (value == 0) value = prefWidth;
					value = Math.Max(minSize.Width, value);
					MinimumSize = new Size(value, minSize.Height);
					MaximumSize = new Size(value, int.MaxValue);
				}
			}

			private bool HasPrompt => !string.IsNullOrEmpty(Prompt);

			private static object ConvertFromStr(string value, Type destType)
			{
				if (destType == typeof(string))
					return value;
				if (value.Trim() == string.Empty)
					return destType.IsValueType ? Activator.CreateInstance(destType) : null;
				if (typeof(IConvertible).IsAssignableFrom(destType))
					try { return Convert.ChangeType(value, destType); } catch { }
				return TypeDescriptor.GetConverter(destType).ConvertFrom(value);
			}

			private static string ConvertToStr(object value)
			{
				if (value == null)
					return string.Empty;
				var conv = value as IConvertible;
				if (conv != null)
					return value.ToString();
				return (string)TypeDescriptor.GetConverter(value).ConvertTo(value, typeof(string));
			}

			private static int GetBestHeight(Control c)
			{
				using (var g = c.CreateGraphics())
					return TextRenderer.MeasureText(g, c.Text, c.Font, new Size(c.Width, 0), TextFormatFlags.WordBreak).Height;
			}

			private static char[] GetCultureChars(bool digits, bool neg, bool pos, bool dec = false, bool grp = false, bool e = false)
			{
				var c = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
				var l = new List<string>();
				if (digits) l.AddRange(c.NativeDigits);
				if (neg) l.Add(c.NegativeSign);
				if (pos) l.Add(c.PositiveSign);
				if (dec) l.Add(c.NumberDecimalSeparator);
				if (grp) l.Add(c.NumberGroupSeparator);
				if (e) l.Add("Ee");
				var sb = new System.Text.StringBuilder();
				foreach (var s in l)
					sb.Append(s);
				var ca = sb.ToString().ToCharArray();
				Array.Sort(ca);
				return ca;
			}

			private static char[] GetCultureChars(bool timeChars, bool timeSep, bool dateSep, char[] other)
			{
				var c = System.Globalization.CultureInfo.CurrentCulture;
				var l = new List<string>();
				if (timeChars) l.AddRange(c.NumberFormat.NativeDigits);
				if (timeSep) { l.Add(c.DateTimeFormat.TimeSeparator); l.Add(c.NumberFormat.NumberDecimalSeparator); }
				if (dateSep) l.Add(c.DateTimeFormat.DateSeparator);
				if (other != null && other.Length > 0) l.Add(new string(other));
				var sb = new System.Text.StringBuilder();
				foreach (var s in l)
					sb.Append(s);
				var ca = sb.ToString().ToCharArray();
				Array.Sort(ca);
				return ca;
			}

			private static bool IsSimpleType(Type type) => type.IsPrimitive || type.IsEnum || Array.Exists(simpleTypes, t => t == type) || Convert.GetTypeCode(type) != TypeCode.Object ||
				(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(type.GetGenericArguments()[0]));

			private static bool IsSupportedType(Type type)
			{
				if (typeof(IConvertible).IsAssignableFrom(type))
					return true;
				var cvtr = TypeDescriptor.GetConverter(type);
				if (cvtr.CanConvertFrom(typeof(string)) && cvtr.CanConvertTo(typeof(string)))
					return true;
				return false;
			}

			/// <summary>
			/// Binds input text values back to the Data object.
			/// </summary>
			private void BindToData()
			{
				for (var i = 0; i < items.Count; i++)
				{
					var item = items[i];
					var itemType = GetItemType(item);

					// Get value from control
					var c = table.Controls[$"input{i}"];
					var box = c as CheckBox;
					var val = box?.Checked ?? ConvertFromStr(c.Text, itemType);

					// Apply value to dataObj
					if (item == null)
						dataObj = val;
					else if (item is PropertyInfo)
						((PropertyInfo)item).SetValue(dataObj, val, null);
					else
						((FieldInfo)item).SetValue(dataObj, val);
				}
			}

			private Control BuildInputForItem(int i)
			{
				var item = items[i];
				var itemType = GetItemType(item);

				// Get default text value
				object val;
				if (item == null)
					val = dataObj;
				else if (item is PropertyInfo)
					val = ((PropertyInfo)item).GetValue(dataObj, null);
				else
					val = ((FieldInfo)item).GetValue(dataObj);
				var t = ConvertToStr(val);

				// Build control type
				Control retVal;
				if (itemType == typeof(bool))
				{
					retVal = new CheckBox { AutoSize = true, Checked = (bool)val, Margin = new Padding(0, 7, 0, 0), MinimumSize = new Size(0, 20) };
				}
				else if (itemType.IsEnum)
				{
					var cb = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
					cb.Items.AddRange(Enum.GetNames(itemType));
					cb.Text = t;
					retVal = cb;
				}
				else
				{
					var tb = new TextBox { CausesValidation = true, Dock = DockStyle.Fill, Text = t };
					tb.Enter += (s, e) => tb.SelectAll();
					if (itemType == typeof(char))
						tb.KeyPress += (s, e) => e.Handled = !char.IsControl(e.KeyChar) && tb.TextLength > 0;
					else
						tb.KeyPress += (s, e) => e.Handled = IsInvalidKey(e.KeyChar, itemType);
					tb.Validating += (s, e) =>
					{
						var invalid = TextIsInvalid(tb, itemType);
						e.Cancel = invalid;
						errorProvider.SetError(tb, invalid ? $"Text must be in a valid format for {itemType.Name}." : "");
					};
					tb.Validated += (s, e) => errorProvider.SetError(tb, "");
					errorProvider.SetIconPadding(tb, -18);
					retVal = tb;
				}

				// Set standard props
				// TODO: Change out '7' for DPI specific spacing
				retVal.Margin = new Padding(items.Count == 1 && HasPrompt && items[0] == null ? 4 : 0, 7, 0, 0);
				retVal.Name = $"input{i}";
				return retVal;
			}

			private Label BuildLabelForItem(int i)
			{
				var item = items[i];
				var lbl = new Label { AutoSize = true, Dock = DockStyle.Left, Margin = new Padding(0, 0, 1, 0) };
				if (item != null)
				{
					lbl.Text = (GetAttr(item)?.Label ?? item.Name) + ":";
					// TODO: Change out '10' for spacing needed to align label text with TextBox and '4' for DPI specific spacing
					lbl.Margin = new Padding(0, 10, 4, 0);
				}
				return lbl;
			}

			private void BuildTable()
			{
				table.SuspendLayout();

				// Clear out last layout
				table.Controls.Clear();
				while (table.RowStyles.Count > 1)
					table.RowStyles.RemoveAt(1);

				table.RowCount = items.Count + (HasPrompt ? 1 : 0);

				// Icon
				if (Image != null)
				{
					table.Controls.Add(new PictureBox { Image = Image, Size = Image.Size, Margin = new Padding(0, 0, 7, 0), TabStop = false }, 0, 0);
					table.SetRowSpan(table.GetControlFromPosition(0, 0), table.RowCount);
				}

				var hrow = 0;

				// Add header row if needed
				Label lbl = null;
				if (HasPrompt)
				{
					lbl = new Label
					{
						AutoSize = true,
						Text = Prompt,
						Dock = DockStyle.Top,
						UseMnemonic = false,
						Font = new Font(Font.FontFamily, Font.Size*4/3),
						ForeColor = Color.FromArgb(19, 112, 171),
						Margin = new Padding(items.Count == 1 && items[0] == null ? 1 : 0, 0, 0, 0)
					};
					table.Controls.Add(lbl, 1, hrow++);
					table.SetColumnSpan(lbl, 2);
				}

				// Build rows for each item
				for (var i = 0; i < items.Count; i++)
				{
					if (i + hrow > 0)
						table.RowStyles.Add(new RowStyle(SizeType.AutoSize));
					table.Controls.Add(BuildLabelForItem(i), 1, i + hrow);
					table.Controls.Add(BuildInputForItem(i), 2, i + hrow);
				}

				table.ResumeLayout();

				if (HasPrompt)
				{
					if (lbl != null && lbl.PreferredWidth > lbl.Width)
						lbl.MinimumSize = lbl.Size;
				}
			}

			private void cancelBtn_Click(object sender, EventArgs e)
			{
				Close();
			}

			private InputDialogItemAttribute GetAttr(MemberInfo mi) => (InputDialogItemAttribute)Attribute.GetCustomAttribute(mi, typeof(InputDialogItemAttribute), true);

			private Type GetItemType(MemberInfo mi) => mi == null ? dataObj.GetType() : ((mi as PropertyInfo)?.PropertyType ?? ((FieldInfo)mi).FieldType);

			private void InitializeComponent()
			{
				components = new Container();
				buttonPanel = new TableLayoutPanel();
				okBtn = new Button();
				cancelBtn = new Button();
				borderPanel = new Panel();
				table = new TableLayoutPanel();
				errorProvider = new ErrorProvider(components);
				buttonPanel.SuspendLayout();
				((ISupportInitialize)errorProvider).BeginInit();
				SuspendLayout();
				//
				// buttonPanel
				//
				buttonPanel.AutoSize = true;
				buttonPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
				buttonPanel.BackColor = SystemColors.Control;
				buttonPanel.ColumnCount = 3;
				buttonPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
				buttonPanel.ColumnStyles.Add(new ColumnStyle());
				buttonPanel.ColumnStyles.Add(new ColumnStyle());
				buttonPanel.Controls.Add(okBtn, 1, 0);
				buttonPanel.Controls.Add(cancelBtn, 2, 0);
				buttonPanel.Dock = DockStyle.Bottom;
				buttonPanel.Location = new Point(0, 25);
				buttonPanel.Margin = new Padding(0);
				buttonPanel.MinimumSize = new Size(177, 40);
				buttonPanel.Name = "buttonPanel";
				buttonPanel.Padding = new Padding(10, 8, 10, 9);
				buttonPanel.RowCount = 1;
				buttonPanel.RowStyles.Add(new RowStyle());
				buttonPanel.Size = new Size(177, 40);
				buttonPanel.TabIndex = 1;
				//
				// okBtn
				//
				okBtn.Location = new Point(10, 8);
				okBtn.Margin = new Padding(0, 0, 7, 0);
				okBtn.MinimumSize = new Size(75, 23);
				okBtn.Name = "okBtn";
				okBtn.Size = new Size(75, 23);
				okBtn.TabIndex = 0;
				okBtn.Text = "OK";
				okBtn.UseVisualStyleBackColor = true;
				okBtn.Click += new EventHandler(okBtn_Click);
				//
				// cancelBtn
				//
				cancelBtn.DialogResult = DialogResult.Cancel;
				cancelBtn.Location = new Point(92, 8);
				cancelBtn.Margin = new Padding(0);
				cancelBtn.MinimumSize = new Size(75, 23);
				cancelBtn.Name = "cancelBtn";
				cancelBtn.Size = new Size(75, 23);
				cancelBtn.TabIndex = 1;
				cancelBtn.Text = "&Cancel";
				cancelBtn.UseVisualStyleBackColor = true;
				cancelBtn.Click += new EventHandler(cancelBtn_Click);
				//
				// borderPanel
				//
				borderPanel.BackColor = Color.FromArgb((int)(byte)223, (int)(byte)223, (int)(byte)223);
				borderPanel.Dock = DockStyle.Bottom;
				borderPanel.Location = new Point(0, 24);
				borderPanel.Margin = new Padding(0);
				borderPanel.MinimumSize = new Size(0, 1);
				borderPanel.Name = "borderPanel";
				borderPanel.Size = new Size(177, 1);
				borderPanel.TabIndex = 2;
				//
				// table
				//
				table.AutoSize = true;
				table.AutoSizeMode = AutoSizeMode.GrowAndShrink;
				table.ColumnCount = 3;
				table.ColumnStyles.Add(new ColumnStyle());
				table.ColumnStyles.Add(new ColumnStyle());
				table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
				table.Dock = DockStyle.Fill;
				table.Location = new Point(0, 0);
				table.Margin = new Padding(0);
				table.Name = "table";
				table.Padding = new Padding(10);
				table.RowCount = 1;
				table.RowStyles.Add(new RowStyle());
				table.Size = new Size(177, 24);
				table.TabIndex = 3;
				//
				// errorProvider
				//
				errorProvider.ContainerControl = this;
				//
				// InternalInputDialog
				//
				AcceptButton = okBtn;
				AutoSize = true;
				AutoSizeMode = AutoSizeMode.GrowAndShrink;
				BackColor = SystemColors.Window;
				CancelButton = cancelBtn;
				ClientSize = new Size(prefWidth, 65);
				Controls.Add(table);
				Controls.Add(borderPanel);
				Controls.Add(buttonPanel);
				Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
				FormBorderStyle = FormBorderStyle.FixedDialog;
				MinimumSize = new Size(prefWidth, minSize.Height);
				MaximumSize = new Size(prefWidth, int.MaxValue);
				Name = "InternalInputDialog";
				StartPosition = FormStartPosition.CenterParent;
				buttonPanel.ResumeLayout(false);
				((ISupportInitialize)errorProvider).EndInit();
				ResumeLayout(false);
				PerformLayout();
			}

			private bool IsInvalidKey(char keyChar, Type itemType)
			{
				if (char.IsControl(keyChar))
					return false;
				char[] chars;
				keyPressValidChars.TryGetValue(itemType, out chars);
				if (chars != null)
				{
					var si = Array.BinarySearch(chars, keyChar);
					System.Diagnostics.Debug.WriteLine($"Processed key {keyChar} as {si} position.");
					if (si < 0)
						return true;
				}
				return false;
			}

			private void okBtn_Click(object sender, EventArgs e)
			{
				BindToData();
				DialogResult = DialogResult.OK;
				Close();
			}

			private bool TextIsInvalid(TextBox tb, Type itemType)
			{
				if (string.IsNullOrEmpty(tb.Text))
					return false;
				Predicate<string> p;
				validations.TryGetValue(itemType, out p);
				if (p != null)
					return !p(tb.Text);
				return false;
			}

			private class RegexTextBox : TextBox
			{
				public string RegexPattern { get; set; }

				protected override void OnKeyPress(KeyPressEventArgs e)
				{
					//System.Text.RegularExpressions.Regex.IsMatch()
					base.OnKeyPress(e);
				}
			}
		}
	}

	/// <summary>
	/// Allows a developer to attribute a property or field with text that gets shown instead of the field or property name in an <see cref="InputDialog"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
	public sealed class InputDialogItemAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="InputDialogItemAttribute" /> class.
		/// </summary>
		public InputDialogItemAttribute() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="InputDialogItemAttribute" /> class.
		/// </summary>
		/// <param name="label">The label to use in the <see cref="InputDialog"/> as the label for this field or property.</param>
		public InputDialogItemAttribute(string label)
		{
			Label = label;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this item is hidden and not displayed by the <see cref="InputDialog"/>.
		/// </summary>
		/// <value>
		/// <c>true</c> if hidden; otherwise, <c>false</c>.
		/// </value>
		public bool Hidden { get; set; } = false;

		/// <summary>
		/// Gets or sets the label to use in the <see cref="InputDialog"/> as the label for this field or property.
		/// </summary>
		/// <value>
		/// The label for this item.
		/// </value>
		public string Label { get; }

		/// <summary>
		/// Gets or sets the order in which to display the input for this field or property within the <see cref="InputDialog"/>.
		/// </summary>
		/// <value>
		/// The display order for this item.
		/// </value>
		public int Order { get; set; } = int.MaxValue;
	}
}