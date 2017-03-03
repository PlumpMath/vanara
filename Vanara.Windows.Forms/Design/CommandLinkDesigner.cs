using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Vanara.Windows.Forms.Design
{
	[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	internal class CommandLinkDesigner : RichControlDesigner<CommandLink>, IToolboxUser
	{
		private static readonly string[] propsToRemove = new string[] { "AllowDrop", "AutoEllipsis", "BackColor",
			"BackgroundImage", "BackgroundImageLayout", "ContextMenuStrip", "Cursor", "FlatStyle", "FlatAppearance", "Font",
			"ForeColor", "ImageAlign", "ImageIndex", "ImageKey", "ImageList", "Padding", "TextAlign", "TextImageRelation",
			"UseCompatibleTextRendering", "UseVisualStyleBackColor", "UseWaitCursor" };

		public CommandLinkDesigner() { }

		public override SelectionRules SelectionRules => (SelectionRules.Visible | SelectionRules.AllSizeable | SelectionRules.Moveable);

		protected override System.Collections.Generic.IEnumerable<string> PropertiesToRemove => propsToRemove;

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			AutoResizeHandles = true;
		}

		bool IToolboxUser.GetToolSupported(ToolboxItem tool) => true;

		void IToolboxUser.ToolPicked(ToolboxItem tool) { }
	}
}