using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32.SafeHandles;

// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable InconsistentNaming

namespace Vanara.PInvoke
{
	public static partial class Kernel32
	{
		[Flags]
		public enum ActCtxFlags
		{
			ACTCTX_FLAG_NONE = 0x00000000,
			ACTCTX_FLAG_PROCESSOR_ARCHITECTURE_VALID = 0x00000001,
			ACTCTX_FLAG_LANGID_VALID = 0x00000002,
			ACTCTX_FLAG_ASSEMBLY_DIRECTORY_VALID = 0x00000004,
			ACTCTX_FLAG_RESOURCE_NAME_VALID = 0x00000008,
			ACTCTX_FLAG_SET_PROCESS_DEFAULT = 0x00000010,
			ACTCTX_FLAG_APPLICATION_NAME_VALID = 0x00000020,
			ACTCTX_FLAG_SOURCE_IS_ASSEMBLYREF = 0x00000040,
			ACTCTX_FLAG_HMODULE_VALID = 0x00000080
		}

		public enum DeactivateActCtxFlag
		{
			/// <summary>
			/// If this value is set and the cookie specified in the ulCookie parameter is in the top frame of the activation stack, the activation context is popped from the stack and thereby deactivated.
			/// <para>If this value is set and the cookie specified in the ulCookie parameter is not in the top frame of the activation stack, this function searches down the stack for the cookie.</para>
			/// <para>If the cookie is found, a STATUS_SXS_EARLY_DEACTIVATION exception is thrown.</para>
			/// <para>If the cookie is not found, a STATUS_SXS_INVALID_DEACTIVATION exception is thrown.</para>
			/// <para>This value should be specified in most cases.</para>
			/// </summary>
			None = 0,
			/// <summary>
			/// If this value is set and the cookie specified in the ulCookie parameter is in the top frame of the activation stack, the function returns an ERROR_INVALID_PARAMETER error code. Call GetLastError to obtain this code.
			/// <para>If this value is set and the cookie is not on the activation stack, a STATUS_SXS_INVALID_DEACTIVATION exception will be thrown.</para>
			/// <para>If this value is set and the cookie is in a lower frame of the activation stack, all of the frames down to and including the frame the cookie is in is popped from the stack.</para>
			/// </summary>
			DEACTIVATE_ACTCTX_FLAG_FORCE_EARLY_DEACTIVATION = 1
		}

		public const int ACTCTX_FLAG_ASSEMBLY_DIRECTORY_VALID = 0x004;

		/// <summary>
		/// The ActivateActCtx function activates the specified activation context. It does this by pushing the specified
		/// activation context to the top of the
		/// activation stack. The specified activation context is thus associated with the current thread and any appropriate
		/// side-by-side API functions.
		/// </summary>
		/// <param name="hActCtx">
		/// Handle to an ACTCTX structure that contains information on the activation context that is to be
		/// made active.
		/// </param>
		/// <param name="lpCookie">
		/// Pointer to a ULONG_PTR that functions as a cookie, uniquely identifying a specific, activated
		/// activation context.
		/// </param>
		/// <returns>
		/// If the function succeeds, it returns TRUE. Otherwise, it returns FALSE. This function sets errors that can be
		/// retrieved by calling GetLastError.
		/// </returns>
		[DllImport(nameof(Kernel32), ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ActivateActCtx(ActCtxSafeHandle hActCtx, out IntPtr lpCookie);

		/// <summary>
		/// The CreateActCtx function creates an activation context.
		/// </summary>
		/// <param name="actctx">Pointer to an ACTCTX structure that contains information about the activation context to be created.</param>
		/// <returns>If the function succeeds, it returns a handle to the returned activation context. Otherwise, it returns INVALID_HANDLE_VALUE.</returns>
		[DllImport(nameof(Kernel32), CharSet = CharSet.Auto, SetLastError = true)]
		public static extern ActCtxSafeHandle CreateActCtx(ref ACTCTX actctx);

		/// <summary>The DeactivateActCtx function deactivates the activation context corresponding to the specified cookie.</summary>
		/// <param name="dwFlags">Flags that indicate how the deactivation is to occur.</param>
		/// <param name="lpCookie">
		/// The ULONG_PTR that was passed into the call to ActivateActCtx. This value is used as a cookie to identify a specific
		/// activated activation context.
		/// </param>
		/// <returns>
		/// If the function succeeds, it returns TRUE. Otherwise, it returns FALSE. This function sets errors that can be
		/// retrieved by calling GetLastError.
		/// </returns>
		[DllImport(nameof(Kernel32), ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeactivateActCtx(DeactivateActCtxFlag dwFlags, IntPtr lpCookie);

		/// <summary>The GetCurrentActCtx function returns the handle to the active activation context of the calling thread.</summary>
		/// <param name="handle">Pointer to the returned ACTCTX structure that contains information on the active activation context.</param>
		/// <returns>If the function succeeds, it returns TRUE. Otherwise, it returns FALSE. This function sets errors that can be retrieved by calling GetLastError.</returns>
		[DllImport(nameof(Kernel32), ExactSpelling = true, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetCurrentActCtx(out ActCtxSafeHandle handle);

		[DllImport(nameof(Kernel32), ExactSpelling = true, SetLastError = true)]
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		public static extern void ReleaseActCtx(IntPtr hActCtx);

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public struct ACTCTX
		{
			public int cbSize;
			public ActCtxFlags dwFlags;
			[MarshalAs(UnmanagedType.LPTStr)] public string lpSource;
			public ProcessorArchitecture wProcessorArchitecture;
			public ushort wLangId;
			[MarshalAs(UnmanagedType.LPTStr)] public string lpAssemblyDirectory;
			[MarshalAs(UnmanagedType.LPTStr)] public string lpResourceName;
			[MarshalAs(UnmanagedType.LPTStr)] public string lpApplicationName;
			public IntPtr hModule;

			public ACTCTX(string source) : this()
			{
				cbSize = Marshal.SizeOf(typeof(ACTCTX));
				lpSource = source;
			}

			public static ACTCTX Empty = new ACTCTX { cbSize = Marshal.SizeOf(typeof(ACTCTX)) };
		}

		public class ActCtxSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
		{
			public ActCtxSafeHandle() : base(true) { }

			public ActCtxSafeHandle(IntPtr hActCtx, bool ownsHandle) : base(ownsHandle) { SetHandle(hActCtx); }

			protected override bool ReleaseHandle()
			{
				ReleaseActCtx(handle);
				return true;
			}
		}
	}
}