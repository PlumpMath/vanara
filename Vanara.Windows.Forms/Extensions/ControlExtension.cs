﻿using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static Vanara.PInvoke.User32;

namespace Vanara.Extensions
{
	public static partial class ExtensionMethods
	{
		/// <summary>
		/// Performs an action on a control after its handle has been created. If the control's handle has already been created, the action is executed immediately.
		/// </summary>
		/// <param name="ctrl">This control.</param>
		/// <param name="action">The action to execute.</param>
		public static void CallWhenHandleValid(this Control ctrl, Action<Control> action)
		{
			if (ctrl.IsHandleCreated)
			{
				action(ctrl);
			}
			else if (!ctrl.IsDesignMode())
			{
				EventHandler handler = null;
				handler = (sender, e) =>
				{
					if (!ctrl.IsHandleCreated) return;
					ctrl.HandleCreated -= handler;
					action(ctrl);
				};
				ctrl.HandleCreated += handler;
			}
		}

		/// <summary>Enables all children of a control.</summary>
		/// <param name="ctl">This control.</param>
		/// <param name="enabled">If set to <c>true</c> enable all children, otherwise disable all children.</param>
		public static void EnableChildren(this Control ctl, bool enabled)
		{
			foreach (Control sub in ctl.Controls)
			{
				if (sub is ButtonBase || sub is ListControl || sub is TextBoxBase)
					sub.Enabled = enabled;
				sub.EnableChildren(enabled);
			}
		}

		/// <summary>Gets the control in the list of parents of type <c>T</c>.</summary>
		/// <typeparam name="T">The <see cref="Control"/> based <see cref="Type"/> of the parent control to retrieve.</typeparam>
		/// <param name="ctrl">This control.</param>
		/// <returns>The parent control matching T or null if not found.</returns>
		public static T GetParent<T>(this Control ctrl) where T : Control, new()
		{
			var p = ctrl.Parent;
			while (p != null & !(p is T))
				p = p.Parent;
			return p as T;
		}

		/// <summary>Gets the top-most control in the list of parents of type <c>T</c>.</summary>
		/// <typeparam name="T">The <see cref="Control"/> based <see cref="Type"/> of the parent control to retrieve.</typeparam>
		/// <param name="ctrl">This control.</param>
		/// <returns>The top-most parent control matching T or null if not found.</returns>
		public static T GetTopMostParent<T>(this Control ctrl) where T : Control, new()
		{
			var stack = new System.Collections.Generic.Stack<Control>();
			var p = ctrl.Parent;
			while (p != null)
			{
				stack.Push(p);
				p = p.Parent;
			}
			while (stack.Count > 0)
				if ((p = stack.Pop()) is T)
					return p as T;
			return null;
		}

		/// <summary>Gets the right to left property.</summary>
		/// <param name="ctrl">This control.</param>
		/// <returns>Culture defined direction of text for this control.</returns>
		public static RightToLeft GetRightToLeftProperty(this Control ctrl)
		{
			while (ctrl != null)
			{
				if (ctrl.RightToLeft != RightToLeft.Inherit)
					return ctrl.RightToLeft;
				ctrl = ctrl.Parent;
			}
			return RightToLeft.No;
		}

		/// <summary>Determines whether this control is in design mode.</summary>
		/// <param name="ctrl">This control.</param>
		/// <returns><c>true</c> if in design mode; otherwise, <c>false</c>.</returns>
		public static bool IsDesignMode(this Control ctrl)
		{
			var p = ctrl;
			while (p != null)
			{
				var site = p.Site;
				if (site != null && site.DesignMode)
					return true;
				p = p.Parent;
			}
			return false;
		}

		/// <summary>
		/// Gets a string using a message pattern that asks for the string length by sending a GetXXXLen message and then a GetXXXText message.
		/// </summary>
		/// <param name="ctrl">The control.</param>
		/// <param name="getLenMsg">The window message identifier for retrieving the string length.</param>
		/// <param name="getTextMsg">The window message identifier for retrieving the string.</param>
		/// <returns>The string result from the message call.</returns>
		public static string GetMessageString(this Control ctrl, uint getLenMsg, uint getTextMsg)
		{
			if (!ctrl.IsHandleCreated) return null;
			var cp = SendMessage(new HandleRef(ctrl, ctrl.Handle), getLenMsg).ToInt32() + 1;
			var sb = new System.Text.StringBuilder(cp);
			SendMessage(new HandleRef(ctrl, ctrl.Handle), getTextMsg, ref cp, sb);
			return sb.ToString();
		}

		/// <summary>Retrieves the window styles.</summary>
		/// <param name="ctrl">The control.</param>
		/// <returns>The window styles</returns>
		public static int GetStyle(this Control ctrl) => GetWindowLong(new HandleRef(ctrl, ctrl.Handle), (int)WindowLongFlags.GWL_STYLE).ToInt32();

		/// <summary>Removes the mnemonic, if one exists, from the string.</summary>
		/// <param name="str">The string.</param>
		/// <returns>A mnemonic free string.</returns>
		public static string RemoveMnemonic(this string str)
		{
			if (str == null) return null;
			var idx = str?.IndexOf('&');
			if (idx >= 0)
			{
				var sb = new System.Text.StringBuilder(str);
				sb.Remove(idx.Value, 1);
				return sb.ToString();
			}
			return str;
		}

		/// <summary>Sets the windows styles.</summary>
		/// <param name="ctrl">The control.</param>
		/// <param name="style">The style flags.</param>
		/// <param name="on">if set to <c>true</c> add the style, otherwise remove it.</param>
		public static void SetStyle(this Control ctrl, int style, bool on = true)
		{
			var href = new HandleRef(ctrl, ctrl.Handle);
			int oldstyle = GetWindowLong(href, (int)WindowLongFlags.GWL_STYLE).ToInt32();
			if ((oldstyle & style) != style && on)
				SetWindowLong(href, (int)WindowLongFlags.GWL_STYLE, new IntPtr(oldstyle | style));
			else if ((oldstyle & style) == style && !on)
				SetWindowLong(href, (int)WindowLongFlags.GWL_STYLE, new IntPtr(oldstyle & ~style));
			ctrl.Refresh();
		}
	}
}
