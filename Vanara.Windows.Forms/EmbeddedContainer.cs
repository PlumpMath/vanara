using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Vanara.Windows.Forms
{
	[Designer(typeof(Design.EmbeddedContainerDesigner))]
	internal class EmbeddedContainer : ContainerControl
	{
		public EmbeddedContainer()
		{
			base.AutoScroll = false;
			base.Dock = DockStyle.Fill;
		}
	}

	namespace Design
	{

		[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
		[EditorBrowsable(EditorBrowsableState.Never)]
		internal class EmbeddedContainerDesigner : RichControlDesigner<EmbeddedContainer>
		{
			private static readonly string[] propsToRemove = new string[]
			{
				"AutoScroll", "AutoScrollOffset", "AutoSize", "BackColor",
				"BackgroundImage", "BackgroundImageLayout", "ContextMenuStrip", "Cursor", "Dock", "Enabled", "Font",
				"ForeColor", /*"Location",*/ "MaximumSize", "MinimumSize", "Padding", /*"Size",*/ "TabStop",
				"UseWaitCursor"
			};

			public EmbeddedContainerDesigner() { }

			protected override System.Collections.Generic.IEnumerable<string> PropertiesToRemove => propsToRemove;
		}
	}
}

