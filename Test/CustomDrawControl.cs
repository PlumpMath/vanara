using System.Drawing;
using System.Windows.Forms.VisualStyles;
using Vanara.Windows.Forms;

namespace Test
{
	public partial class CustomButtonControl : CustomButton
	{
		public CustomButtonControl()
		{
			InitializeComponent();
			CornerRadius = 4;
			PaintPattern.Add(PushButtonState.Normal, new DrawPattern(Color.Blue, Color.Blue, Color.White));
			PaintPattern.Add(PushButtonState.Hot, new DrawPattern(Color.Aqua, Color.Aqua, Color.Black));
			PaintPattern.Add(PushButtonState.Pressed, new DrawPattern(Color.DarkBlue, Color.DarkBlue, Color.White));
			PaintPattern.Add(PushButtonState.Default, new DrawPattern(Color.Blue, Color.DarkBlue, Color.White));
		}
	}
}
