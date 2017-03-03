using System;
using System.Reflection;

namespace Vanara.Extensions
{
	public static partial class ExtensionMethods
	{
		public static T GetPropertyValue<T>(this object obj, string propertyName, T defaultValue = default(T))
		{
			var prop = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, typeof(T), Type.EmptyTypes, null);
			if (prop == null) return defaultValue;
			var val = prop.GetValue(obj, null);
			if (val == null) return default(T);
			try { return (T)val; } catch { }
			try
			{
				var icv = val as IConvertible;
				if (icv != null) return (T)Convert.ChangeType(val, typeof(T));
			}
			catch { }
			return defaultValue;
		}

		public static T InvokeMethod<T>(this Type type, string methodName, params object[] args)
		{
			var o = Activator.CreateInstance(type);
			return InvokeMethod<T>(o, methodName, args);
		}

		public static T InvokeMethod<T>(this Type type, object[] instArgs, string methodName, params object[] args)
		{
			var o = Activator.CreateInstance(type, instArgs);
			return InvokeMethod<T>(o, methodName, args);
		}

		public static void InvokeMethod(this object obj, string methodName, params object[] args)
		{
			var argTypes = args == null || args.Length == 0 ? Type.EmptyTypes : Array.ConvertAll(args,
				o => o?.GetType() ?? typeof(object));
			InvokeMethod(obj, methodName, argTypes, args);
		}

		public static T InvokeMethod<T>(this object obj, string methodName, params object[] args)
		{
			var argTypes = args == null || args.Length == 0 ? Type.EmptyTypes : Array.ConvertAll(args,
				o => o?.GetType() ?? typeof(object));
			return InvokeMethod<T>(obj, methodName, argTypes, args);
		}

		public static void InvokeMethod(this object obj, string methodName, Type[] argTypes, object[] args)
		{
			var mi = obj?.GetType().GetMethod(methodName, argTypes);
			if (mi != null && mi.ReturnType == typeof(void))
				mi.Invoke(obj, args);
		}

		public static T InvokeMethod<T>(this object obj, string methodName, Type[] argTypes, object[] args)
		{
			var mi = obj?.GetType().GetMethod(methodName, argTypes);
			if (mi != null)
			{
				var tt = typeof(T);
				if (tt == typeof(object) || mi.ReturnType == tt || mi.ReturnType.IsSubclassOf(tt))
					return (T)mi.Invoke(obj, args);
				if (mi.ReturnType.GetInterface("IConvertible") != null)
					return (T)Convert.ChangeType(mi.Invoke(obj, args), tt);
			}
			return default(T);
		}

		public static Type LoadType(string typeName, string asmRef)
		{
			Type ret = null;
			if (!TryGetType(Assembly.LoadFrom(asmRef), typeName, ref ret))
				if (!TryGetType(Assembly.GetExecutingAssembly(), typeName, ref ret))
					if (!TryGetType(Assembly.GetCallingAssembly(), typeName, ref ret))
						TryGetType(Assembly.GetEntryAssembly(), typeName, ref ret);
			return ret;
		}

		private static bool TryGetType(Assembly asm, string typeName, ref Type type)
		{
			if (asm == null) return false;
			type = asm.GetType(typeName, false, false);
			return type != null;
		}
	}
}