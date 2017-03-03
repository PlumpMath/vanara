using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.Design.Behavior;
using Vanara.Extensions;
using Vanara.Windows.Forms.Design;

namespace Vanara.Extensions
{
	public static partial class ExtensionMethods
	{
		public static T GetService<T>(this IServiceProvider sp) where T : class => (T)sp.GetService(typeof(T));
	}
}

namespace System.ComponentModel.Design
{
	internal static class ComponentDesignerExtension
	{
		public const Reflection.BindingFlags allInstBind = Reflection.BindingFlags.NonPublic | Reflection.BindingFlags.Public | Reflection.BindingFlags.Instance;

		public static object EditValue(this ComponentDesigner designer, object objectToChange, string propName)
		{
			var prop = TypeDescriptor.GetProperties(objectToChange)[propName];
			var context = new EditorServiceContext(designer, prop);
			var editor = prop.GetEditor(typeof(UITypeEditor)) as UITypeEditor;
			var curVal = prop.GetValue(objectToChange);
			var newVal = editor?.EditValue(context, context, curVal);
			if (newVal != curVal)
				try { prop.SetValue(objectToChange, newVal); }
				catch (CheckoutException) { }
			return newVal;
		}

		public static List<DesignerActionItem> GetAllAttributedActionItems(this DesignerActionList actionList)
		{
			var fullAIList = new List<DesignerActionItem>();
			foreach (var mbr in actionList.GetType().GetMethods(allInstBind))
			{
				foreach (IActionGetItem attr in mbr.GetCustomAttributes(typeof(DesignerActionMethodAttribute), false))
				{
					if (mbr.ReturnType == typeof(void) && mbr.GetParameters().Length == 0)
						fullAIList.Add(attr.GetItem(actionList, mbr));
					else
						throw new FormatException("DesignerActionMethodAttribute must be applied to a method returning void and having no parameters.");
				}
			}
			foreach (var mbr in actionList.GetType().GetProperties(allInstBind))
			{
				foreach (IActionGetItem attr in mbr.GetCustomAttributes(typeof(DesignerActionPropertyAttribute), false))
					fullAIList.Add(attr.GetItem(actionList, mbr));
			}
			fullAIList.Sort(CompareItems);
			return fullAIList;
		}

		public static DesignerVerbCollection GetAttributedVerbs(this ComponentDesigner designer)
		{
			var verbs = new DesignerVerbCollection();
			foreach (var m in designer.GetType().GetMethods(allInstBind))
			{
				foreach (DesignerVerbAttribute attr in m.GetCustomAttributes(typeof(DesignerVerbAttribute), true))
				{
					verbs.Add(attr.GetDesignerVerb(designer, m));
				}
			}
			return verbs;
		}

		public static DesignerActionItemCollection GetFilteredActionItems(this DesignerActionList actionList, List<DesignerActionItem> fullAIList)
		{
			var col = new DesignerActionItemCollection();
			fullAIList.ForEach(ai => { if (CheckCondition(actionList, ai)) { col.Add(ai); } });

			// Add header items for displayed items
			var i = 0; string cat = null;
			while (i < col.Count)
			{
				var curCat = col[i].Category;
				if (string.Compare(curCat, cat, true) != 0)
				{
					col.Insert(i++, new DesignerActionHeaderItem(curCat));
					cat = curCat;
				}
				i++;
			}

			return col;
		}

		public static IDictionary<string, List<Attribute>> GetRedirectedProperties(this ComponentDesigner d)
		{
			var ret = new Dictionary<string, List<Attribute>>();
			foreach (var prop in d.GetType().GetProperties(allInstBind))
			{
				foreach (RedirectedDesignerPropertyAttribute attr in prop.GetCustomAttributes(typeof(RedirectedDesignerPropertyAttribute), false))
				{
					List<Attribute> attributes;
					if (attr.ApplyOtherAttributes)
					{
						attributes = new List<Attribute>(Array.ConvertAll(prop.GetCustomAttributes(false), o => o as Attribute));
						attributes.RemoveAll(a => a is RedirectedDesignerPropertyAttribute);
					}
					else
						attributes = new List<Attribute>();
					ret.Add(prop.Name, attributes);
				}
			}
			return ret;
		}

		public static void RedirectRegisteredProperties(this ComponentDesigner d, IDictionary properties, IDictionary<string, List<Attribute>> redirectedProps)
		{
			foreach (var propName in redirectedProps.Keys)
			{
				var oldPropertyDescriptor = (PropertyDescriptor)properties[propName];
				if (oldPropertyDescriptor != null)
				{
					var attributes = redirectedProps[propName];
					properties[propName] = TypeDescriptor.CreateProperty(d.GetType(), oldPropertyDescriptor, attributes.ToArray());
				}
			}
		}

		public static void RemoveProperties(this ComponentDesigner d, IDictionary properties, IEnumerable<string> propertiesToRemove)
		{
			foreach (var p in propertiesToRemove)
				if (properties.Contains(p))
					properties.Remove(p);
		}

		public static void SetComponentProperty<T>(this ComponentDesigner d, string propName, T value)
		{
			var propDesc = TypeDescriptor.GetProperties(d.Component)[propName];
			if (propDesc != null && propDesc.PropertyType == typeof(T) && !propDesc.IsReadOnly && propDesc.IsBrowsable)
				propDesc.SetValue(d.Component, value);
		}

		public static DialogResult ShowDialog(this ComponentDesigner designer, Form dialog)
		{
			var context = new EditorServiceContext(designer);
			return context.ShowDialog(dialog);
		}

		private static bool CheckCondition(DesignerActionList actionList, DesignerActionItem ai)
		{
			if (ai.Properties["Condition"] != null)
			{
				var p = actionList.GetType().GetProperty((string)ai.Properties["Condition"], allInstBind, null, typeof(bool), Type.EmptyTypes, null);
				if (p != null)
					return (bool)p.GetValue(actionList, null);
			}
			return true;
		}

		private static int CompareItems(DesignerActionItem a, DesignerActionItem b)
		{
			var c = string.Compare(a?.Category ?? string.Empty, b?.Category ?? string.Empty, true);
			if (c != 0)
				return c;
			c = (int)(a?.Properties["Order"] ?? 0) - (int)(b?.Properties["Order"] ?? 0);
			if (c != 0)
				return c;
			return string.Compare(a?.DisplayName, b?.DisplayName, true);
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	internal sealed class RedirectedDesignerPropertyAttribute : Attribute
	{
		public RedirectedDesignerPropertyAttribute() { ApplyOtherAttributes = true; }

		public bool ApplyOtherAttributes { get; set; }
	}
}

namespace Vanara.Windows.Forms.Design
{
	internal interface IActionGetItem
	{
		string Category { get; }

		DesignerActionItem GetItem(DesignerActionList actions, System.Reflection.MemberInfo mbr);
	}

	[AttributeUsage(AttributeTargets.Method)]
	internal sealed class DesignerActionMethodAttribute : Attribute, IActionGetItem
	{
		public DesignerActionMethodAttribute(string displayName, int displayOrder = 0)
		{
			DisplayName = displayName;
			DisplayOrder = displayOrder;
		}

		public bool AllowAssociate { get; set; }

		public string Category { get; set; }

		public string Condition { get; set; }

		public string Description { get; set; }

		public string DisplayName { get; }

		public int DisplayOrder { get; }

		public bool IncludeAsDesignerVerb { get; set; }

		DesignerActionItem IActionGetItem.GetItem(DesignerActionList actions, System.Reflection.MemberInfo mbr)
		{
			var ret = new DesignerActionMethodItem(actions, mbr.Name, DisplayName, Category, Description, IncludeAsDesignerVerb)
				{ AllowAssociate = AllowAssociate };
			if (!string.IsNullOrEmpty(Condition))
				ret.Properties.Add("Condition", Condition);
			ret.Properties.Add("Order", DisplayOrder);
			return ret;
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	internal sealed class DesignerActionPropertyAttribute : Attribute, IActionGetItem
	{
		public DesignerActionPropertyAttribute(string displayName, int displayOrder = 0)
		{
			DisplayName = displayName;
			DisplayOrder = displayOrder;
		}

		public bool AllowAssociate { get; set; }

		public string Category { get; set; }

		public string Condition { get; set; }

		public string Description { get; set; }

		public string DisplayName { get; }

		public int DisplayOrder { get; }

		DesignerActionItem IActionGetItem.GetItem(DesignerActionList actions, System.Reflection.MemberInfo mbr)
		{
			var ret = new DesignerActionPropertyItem(mbr.Name, DisplayName, Category, Description)
				{ AllowAssociate = AllowAssociate };
			if (!string.IsNullOrEmpty(Condition))
				ret.Properties.Add("Condition", Condition);
			ret.Properties.Add("Order", DisplayOrder);
			return ret;
		}
	}

	[AttributeUsage(AttributeTargets.Method)]
	internal sealed class DesignerVerbAttribute : Attribute
	{
		private readonly CommandID cmdId;
		private readonly string menuText;

		public DesignerVerbAttribute(string menuText)
		{
			this.menuText = menuText;
		}

		public DesignerVerbAttribute(string menuText, Guid commandMenuGroup, int commandId)
		{
			this.menuText = menuText;
			cmdId = new CommandID(commandMenuGroup, commandId);
		}

		internal DesignerVerb GetDesignerVerb(object obj, System.Reflection.MethodInfo mi)
		{
			var handler = (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), obj, mi);
			if (cmdId != null)
				return new DesignerVerb(menuText, handler, cmdId);
			return new DesignerVerb(menuText, handler);
		}
	}

	internal class EditorServiceContext : IWindowsFormsEditorService, ITypeDescriptorContext
	{
		private IComponentChangeService componentChangeSvc;
		private readonly ComponentDesigner designer;
		private readonly PropertyDescriptor targetProperty;

		internal EditorServiceContext(ComponentDesigner designer)
		{
			this.designer = designer;
		}

		internal EditorServiceContext(ComponentDesigner designer, PropertyDescriptor prop)
		{
			this.designer = designer;
			targetProperty = prop;
			if (prop == null)
			{
				prop = TypeDescriptor.GetDefaultProperty(designer.Component);
				if ((prop != null) && typeof(ICollection).IsAssignableFrom(prop.PropertyType))
					targetProperty = prop;
			}
		}

		internal EditorServiceContext(ComponentDesigner designer, PropertyDescriptor prop, string newVerbText)
			: this(designer, prop)
		{
			this.designer.Verbs.Add(new DesignerVerb(newVerbText, OnEditItems));
		}

		private T GetService<T>() where T : class => ((IServiceProvider)this).GetService<T>();

		private void OnEditItems(object sender, EventArgs e)
		{
			var component = targetProperty.GetValue(designer.Component);
			if (component != null)
			{
				var editor = TypeDescriptor.GetEditor(component, typeof(UITypeEditor)) as CollectionEditor;
				editor?.EditValue(this, this, component);
			}
		}

		void ITypeDescriptorContext.OnComponentChanged()
		{
			ChangeService.OnComponentChanged(designer.Component, targetProperty, null, null);
		}

		bool ITypeDescriptorContext.OnComponentChanging()
		{
			try
			{
				ChangeService.OnComponentChanging(designer.Component, targetProperty);
			}
			catch (CheckoutException exception)
			{
				if (exception != CheckoutException.Canceled)
					throw;
				return false;
			}
			return true;
		}

		object IServiceProvider.GetService(Type serviceType)
		{
			if ((serviceType == typeof(ITypeDescriptorContext)) || (serviceType == typeof(IWindowsFormsEditorService)))
				return this;
			return designer.Component?.Site?.GetService(serviceType);
		}

		void IWindowsFormsEditorService.CloseDropDown()
		{
		}

		void IWindowsFormsEditorService.DropDownControl(Control control)
		{
		}

		public DialogResult ShowDialog(Form dialog)
		{
			var service = GetService<IUIService>();
			if (service != null)
				return service.ShowDialog(dialog);
			return dialog.ShowDialog(designer.Component as IWin32Window);
		}

		private IComponentChangeService ChangeService => componentChangeSvc ?? (componentChangeSvc = GetService<IComponentChangeService>());

		IContainer ITypeDescriptorContext.Container => designer.Component.Site?.Container;

		object ITypeDescriptorContext.Instance => designer.Component;

		PropertyDescriptor ITypeDescriptorContext.PropertyDescriptor => targetProperty;
	}

	internal abstract class RichBehavior<TD> : Behavior where TD : ControlDesigner
	{
		protected RichBehavior(TD designer)
		{
			Designer = designer;
		}

		public TD Designer { get; }
	}

	internal class RichComponentDesigner<TC> : ComponentDesigner
		where TC : Component
	{
		private Adorner adorner;
		private IDictionary<string, List<Attribute>> redirectedProps;
		private DesignerVerbCollection verbs;

		public BehaviorService BehaviorService { get; private set; }

		public IComponentChangeService ComponentChangeService { get; private set; }

		public new TC Component => (TC)base.Component;

		public virtual GlyphCollection Glyphs => Adorner.Glyphs;

		public ISelectionService SelectionService { get; private set; }

		public override DesignerVerbCollection Verbs => verbs ?? (verbs = this.GetAttributedVerbs());

		internal Adorner Adorner
		{
			get
			{
				if (adorner == null)
					BehaviorService.Adorners.Add(adorner = new Adorner());
				return adorner;
			}
		}

		protected virtual IEnumerable<string> PropertiesToRemove => new string[0];

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			BehaviorService = GetService<BehaviorService>();
			SelectionService = GetService<ISelectionService>();
			if (SelectionService != null)
				SelectionService.SelectionChanged += OnSelectionChanged;
			ComponentChangeService = GetService<IComponentChangeService>();
			if (ComponentChangeService != null)
				ComponentChangeService.ComponentChanged += OnComponentChanged;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (BehaviorService != null & adorner != null)
					BehaviorService.Adorners.Remove(adorner);
				var ss = SelectionService;
				if (ss != null)
					ss.SelectionChanged -= OnSelectionChanged;
				var cs = ComponentChangeService;
				if (cs != null)
					cs.ComponentChanged -= OnComponentChanged;
			}
			base.Dispose(disposing);
		}

		protected virtual TS GetService<TS>() where TS : class => (TS)GetService(typeof(TS));

		protected virtual void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
		}

		protected virtual void OnSelectionChanged(object sender, EventArgs e)
		{
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);

			// RedirectRegisteredProperties
			if (redirectedProps == null)
				redirectedProps = this.GetRedirectedProperties();
			this.RedirectRegisteredProperties(properties, redirectedProps);

			// Remove properties
			this.RemoveProperties(properties, PropertiesToRemove);
		}
	}

	internal class RichComponentDesignerA<TC, TA> : RichComponentDesigner<TC>
		where TC : Component
		where TA : BaseDesignerActionList
	{
		private TA actions;

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (actions == null)
					actions = Activator.CreateInstance(typeof(TA), this, Component) as TA;
				return new DesignerActionListCollection(new DesignerActionList[] {actions});
			}
		}
	}

	internal class RichControlDesigner<TC> : ControlDesigner
		where TC : Control
	{
		private Adorner adorner;
		private IDictionary<string, List<Attribute>> redirectedProps;
		private DesignerVerbCollection verbs;

		public new BehaviorService BehaviorService => base.BehaviorService;

		public IComponentChangeService ComponentChangeService { get; private set; }

		public new TC Control => (TC)base.Control;

		public virtual GlyphCollection Glyphs => Adorner.Glyphs;

		public ISelectionService SelectionService { get; private set; }

		public override DesignerVerbCollection Verbs => verbs ?? (verbs = this.GetAttributedVerbs());

		internal Adorner Adorner
		{
			get
			{
				if (adorner == null)
					BehaviorService.Adorners.Add(adorner = new Adorner());
				return adorner;
			}
		}

		protected virtual IEnumerable<string> PropertiesToRemove => new string[0];

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			SelectionService = GetService<ISelectionService>();
			if (SelectionService != null)
				SelectionService.SelectionChanged += OnSelectionChanged;
			ComponentChangeService = GetService<IComponentChangeService>();
			if (ComponentChangeService != null)
				ComponentChangeService.ComponentChanged += OnComponentChanged;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				BehaviorService?.Adorners.Remove(adorner);
				var ss = SelectionService;
				if (ss != null)
					ss.SelectionChanged -= OnSelectionChanged;
				var cs = ComponentChangeService;
				if (cs != null)
					cs.ComponentChanged -= OnComponentChanged;
			}
			base.Dispose(disposing);
		}

		protected virtual TS GetService<TS>() where TS : class => (TS)GetService(typeof(TS));

		protected virtual void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
		}

		protected virtual void OnSelectionChanged(object sender, EventArgs e)
		{
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);

			// RedirectRegisteredProperties
			if (redirectedProps == null)
				redirectedProps = this.GetRedirectedProperties();
			this.RedirectRegisteredProperties(properties, redirectedProps);

			// Remove properties
			this.RemoveProperties(properties, PropertiesToRemove);
		}
	}

	internal class RichControlDesignerA<TC, TA> : RichControlDesigner<TC>
		where TC : Control
		where TA : BaseDesignerActionList
	{
		private TA actions;
		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (actions == null)
					actions = Activator.CreateInstance(typeof(TA), this, Component) as TA;
				return new DesignerActionListCollection(new DesignerActionList[] { actions });
			}
		}
	}

	internal abstract class BaseDesignerActionList : DesignerActionList
	{
		private List<DesignerActionItem> fullAIList;

		protected BaseDesignerActionList(ComponentDesigner designer, IComponent component)
			: base(component)
		{
			ParentDesigner = designer;
			AutoShow = true;
		}

		public ComponentDesigner ParentDesigner { get; }

		public override DesignerActionItemCollection GetSortedActionItems()
		{
			// Retrieve all attributed methods and properties
			if (fullAIList == null)
				fullAIList = this.GetAllAttributedActionItems();

			// Filter for conditions and load
			return this.GetFilteredActionItems(fullAIList);
		}

		protected T GetComponentProperty<T>(string propName)
		{
			var p = ComponentProp(propName, typeof(T));
			if (p != null)
				return (T)p.GetValue(Component, null);
			return default(T);
		}

		protected void SetComponentProperty<T>(string propName, T value)
		{
			ComponentProp(propName, typeof(T))?.SetValue(Component, value, null);
		}

		private System.Reflection.PropertyInfo ComponentProp(string propName, Type retType) => Component.GetType().GetProperty(propName, ComponentDesignerExtension.allInstBind, null, retType, Type.EmptyTypes, null);
	}

	internal abstract class RichDesignerActionList<TD, TC> : BaseDesignerActionList where TD : ComponentDesigner where TC : Component
	{
		protected RichDesignerActionList(TD designer, TC component) : base(designer, component)
		{
			ParentDesigner = designer;
		}

		public new TC Component => (TC)base.Component;

		public new TD ParentDesigner { get; }
	}

	internal abstract class RichGlyph<TD> : Glyph, IDisposable where TD : ControlDesigner
	{
		protected RichGlyph(TD designer, Behavior behavior)
			: base(behavior)
		{
			Designer = designer;
		}

		public TD Designer { get; }

		public virtual void Dispose()
		{
		}

		public void SetBehavior(RichBehavior<TD> b) { base.SetBehavior(b); }
	}

	internal class RichParentControlDesigner<TC> : ParentControlDesigner
		where TC : Control
	{
		private Adorner adorner;
		private IDictionary<string, List<Attribute>> redirectedProps;
		private DesignerVerbCollection verbs;

		public new BehaviorService BehaviorService => base.BehaviorService;

		public IComponentChangeService ComponentChangeService { get; private set; }

		public new TC Control => (TC)base.Control;

		public virtual GlyphCollection Glyphs => Adorner.Glyphs;

		public ISelectionService SelectionService { get; private set; }

		public override DesignerVerbCollection Verbs => verbs ?? (verbs = this.GetAttributedVerbs());

		internal Adorner Adorner
		{
			get
			{
				if (adorner == null)
					BehaviorService.Adorners.Add(adorner = new Adorner());
				return adorner;
			}
		}

		protected virtual IEnumerable<string> PropertiesToRemove => new string[0];

		public override void Initialize(IComponent component)
		{
			base.Initialize(component);
			SelectionService = GetService<ISelectionService>();
			if (SelectionService != null)
				SelectionService.SelectionChanged += OnSelectionChanged;
			ComponentChangeService = GetService<IComponentChangeService>();
			if (ComponentChangeService != null)
				ComponentChangeService.ComponentChanged += OnComponentChanged;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (BehaviorService != null & adorner != null)
					BehaviorService.Adorners.Remove(adorner);
				var ss = SelectionService;
				if (ss != null)
					ss.SelectionChanged -= OnSelectionChanged;
				var cs = ComponentChangeService;
				if (cs != null)
					cs.ComponentChanged -= OnComponentChanged;
			}
			base.Dispose(disposing);
		}

		protected virtual TS GetService<TS>() where TS : class => (TS)GetService(typeof(TS));

		protected virtual void OnComponentChanged(object sender, ComponentChangedEventArgs e)
		{
		}

		protected virtual void OnSelectionChanged(object sender, EventArgs e)
		{
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);

			// RedirectRegisteredProperties
			if (redirectedProps == null)
				redirectedProps = this.GetRedirectedProperties();
			this.RedirectRegisteredProperties(properties, redirectedProps);

			// Remove properties
			this.RemoveProperties(properties, PropertiesToRemove);
		}
	}

	internal class RichParentControlDesignerA<TC, TA> : RichParentControlDesigner<TC>
		where TC : Control
		where TA : BaseDesignerActionList
	{
		private TA actions;

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (actions == null)
					actions = Activator.CreateInstance(typeof(TA), this, Component) as TA;
				return new DesignerActionListCollection(new DesignerActionList[] { actions });
			}
		}
	}
}