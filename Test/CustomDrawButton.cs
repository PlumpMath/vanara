using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Vanara.Drawing;
using Vanara.Extensions;
using Vanara.Windows.Forms;

namespace Test
{
	public partial class CustomDrawButton : CustomDrawBase
	{
		private static int[,] durs;
		private readonly VisualStyleRenderer rnd;

		public CustomDrawButton()
		{
			InitializeComponent();
			SetStyle(ControlStyles.Opaque, true);
			rnd = new VisualStyleRenderer(VisualStyleElement.Button.PushButton.Normal);
			if (durs == null) durs = rnd.GetTransitionMatrix();
		}

		protected PushButtonState ButtonState
		{
			get { var st = GetButtonState(State); System.Diagnostics.Debug.WriteLine($"GetButtonState[{Name}]={State}=>{st}"); return st; }
		}

		protected PushButtonState LastButtonState => GetButtonState(LastState);

		private Font StyledFont => rnd.GetFont2(null, Font);

		private Color StyledForeColor { get { try { return rnd.GetColor(ColorProperty.TextColor); } catch { return ForeColor; } } }

		protected override void OnPaint(PaintEventArgs pe)
		{
			BufferedPaint.PaintAnimation(pe.Graphics, this, pe.ClipRectangle, PaintAction, LastButtonState, ButtonState, GetDuration, this.BuildTextFormatFlags());
			base.OnPaint(pe);
		}

		private static int GetDuration(PushButtonState oldstate, PushButtonState newstate) => durs[(int)oldstate - 1, (int)newstate - 1];

		private PushButtonState GetButtonState(ControlState cstate)
		{
			EnumFlagIndexer<ControlState> state = cstate;
			if (state[ControlState.Disabled]) return PushButtonState.Disabled;
			if (state[ControlState.Pressed]) return PushButtonState.Pressed;
			if (state[ControlState.Hot]) return PushButtonState.Hot;
			if (state[ControlState.Defaulted]) return PushButtonState.Default;
			return PushButtonState.Normal;
		}

		private void PaintAction(Graphics graphics, Rectangle bounds, PushButtonState currentstate, TextFormatFlags tff)
		{
			ButtonRenderer.DrawParentBackground(graphics, bounds, this);
			ButtonRenderer.DrawButton(graphics, bounds, currentstate);
			rnd.SetState((int)currentstate);
			graphics.DrawImageAndText(bounds, Text, StyledFont, Image, TextAlign, ImageAlign, TextImageRelation, StyledForeColor, false, 0, Enabled, tff);
			if (Focused)
				ControlPaint.DrawFocusRectangle(graphics, Rectangle.Inflate(bounds, -3, -3));
			System.Diagnostics.Debug.WriteLine($"Painting CustomBtn: {currentstate}");
		}
	}
}
