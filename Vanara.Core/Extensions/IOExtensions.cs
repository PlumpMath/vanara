using System;
using System.IO;
using System.Runtime.InteropServices;
using Vanara.PInvoke;

namespace Vanara.Extensions
{
	public static class IOExtensions
	{
		/// <summary>Writes the specified structure value of type <typeparamref name="T"/> into a binary stream.</summary>
		/// <typeparam name="T">The type of the structure value to write.</typeparam>
		/// <param name="writer">The <see cref="BinaryWriter"/> instance to write into.</param>
		/// <param name="value">The value to write.</param>
		public static void Write<T>(this BinaryWriter writer, T value) where T : struct
		{
			var sz = Marshal.SizeOf(value);
			var bytes = new byte[sz];
			using (var ptr = new PinnedObject(value))
				Marshal.Copy(ptr, bytes, 0, sz);
			writer.Write(bytes);
		}

		/// <summary>Reads the specified structure value of type <typeparamref name="T"/> from a binary stream.</summary>
		/// <typeparam name="T">The type of the structure value to read.</typeparam>
		/// <param name="reader">The <see cref="BinaryReader"/> instance to read from.</param>
		/// <returns>The value to read from the stream.</returns>
		public static T Read<T>(this BinaryReader reader) where T : struct
		{
			var sz = Marshal.SizeOf(typeof(T));
			var bytes = reader.ReadBytes(sz);
			using (var ptr = new PinnedObject(bytes))
				return ((IntPtr)ptr).ToStructure<T>();
		}
	}
}