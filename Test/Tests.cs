using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows.Forms;
using Microsoft.Win32;
using Vanara.Extensions;
using Vanara.PInvoke;
using Vanara.Security.AccessControl;
using Vanara.Windows.Forms;
using Vanara.Windows.Shell;

namespace Test
{
	internal static class Tests
	{
		public static event EventHandler<TestResultEventArgs> TestResult;

		[TestName("ACL Editor - Dir")]
		public static void AclEditorDialogDir(Form parentForm)
		{
			using (var aclEditorDialog1 = new AccessControlEditorDialog())
			{
				aclEditorDialog1.Flags = AclUI.SI_OBJECT_INFO_Flags.SI_EDIT_OWNER | AclUI.SI_OBJECT_INFO_Flags.SI_EDIT_AUDITS | AclUI.SI_OBJECT_INFO_Flags.SI_CONTAINER | AclUI.SI_OBJECT_INFO_Flags.SI_ADVANCED | AclUI.SI_OBJECT_INFO_Flags.SI_EDIT_EFFECTIVE | AclUI.SI_OBJECT_INFO_Flags.SI_VIEW_ONLY;
				aclEditorDialog1.Initialize(@"C:\HP", null, ResourceType.FileObject);
				var res = aclEditorDialog1.ShowDialog(parentForm);
				AddTestResult(null, "Run dir", $"{res}: {aclEditorDialog1.ObjectIsContainer}:{aclEditorDialog1.Sddl}");
			}
		}

		[TestName("ACL Editor - Registry")]
		public static void AclEditorDialogReg(Form parentForm)
		{
			using (var aclEditorDialog1 = new AccessControlEditorDialog())
			{
				using (var key = Registry.CurrentUser.OpenSubKey("Software", RegistryKeyPermissionCheck.Default, RegistryRights.ReadKey))
				{
					aclEditorDialog1.Initialize(key);
					var res = aclEditorDialog1.ShowDialog(parentForm);
					AddTestResult(null, "Run reg", $"{res}: {aclEditorDialog1.ObjectIsContainer}:{aclEditorDialog1.Sddl}");
				}
			}
		}

		[TestName("Credentials")]
		public static void CredentialsDialog(Form parentForm)
		{
			using (var credentialsDialog1 = new CredentialsDialog())
			{
				var res = credentialsDialog1.ShowDialog(parentForm);
				AddTestResult(null, "Run", $"{res}: {credentialsDialog1.UserName}:{credentialsDialog1.Password}");
			}
		}

		public static void DiskFreeSpace()
		{
			int sectors, sectorBytes, freeClusters, clusters;
			Kernel32.GetDiskFreeSpace(null, out sectors, out sectorBytes, out freeClusters, out clusters);
			AddTestResult(null , "Root disk", $"Sect:{sectors};BxSec{sectorBytes};Clust:{freeClusters}/{clusters}");
		}

		public static void FileCompression()
		{
			var fileName = System.IO.Path.GetTempFileName();
			System.IO.File.WriteAllText(fileName, new string('1', 4096));
			var fi = new System.IO.FileInfo(fileName);
			if (!fi.GetNtfsCompression())
				fi.SetNtfsCompression(true);
			//var gt = fi.GetNtfsCompressionAsync();
			//gt.Wait();
			//AddTestResult("AsyncGet", "Compr", gt.Result);
			//var t = fi.SetNtfsCompressionAsync(false);
			//t.Wait();
			//AddTestResult("AsyncSet", "Compr", t.Status);
			AddTestResult(null + "Compr", fileName, $"{fi.GetNtfsCompression()}");
			fi.Delete();
		}

		[TestName("Folder Browser")]
		public static void FolderBrowser(Form parentForm)
		{
			using (var folderBrowserDialog1 = new Vanara.Windows.Forms.FolderBrowserDialog())
			{
				folderBrowserDialog1.BrowseOption = FolderBrowserDialogOptions.FoldersAndFiles;
				folderBrowserDialog1.Caption = @"My Caption";
				folderBrowserDialog1.Description = @"Some longer text";

				var res = folderBrowserDialog1.ShowDialog(parentForm);
				AddTestResult(null, "Run", $"{res}: {folderBrowserDialog1.SelectedItem}");
			}
		}

		[TestName("Form Functions")]
		public static void Form(Form parentForm)
		{
			var childControl = parentForm.Controls.OfType<Button>().FirstOrDefault();
			var f = childControl?.GetParent<Form>() ?? parentForm;
			f.EnableChildren(false);
			AddTestResult(null, "GetProperty=Text", f.GetPropertyValue<string>("Text"));
			AddTestResult(null, "Btn.BuildFormatFlags", childControl.BuildTextFormatFlags());
			AddTestResult(null, "Btn.GetRightToLeftProperty", childControl.GetRightToLeftProperty());
			AddTestResult(null, "GetStyle", f.GetStyle());
			f.SetStyle(0x00010000, false);
			AddTestResult(null, "SetStyle", f.GetStyle());
			f.EnableChildren(true);
			AddTestResult(null, "CursorBounds", f.Cursor.Bounds());
		}

		[ReqPriv]
		public static void LsaEnumAcctsWithRight()
		{
			using (var h = new LocalSecurityAuthority(null, LocalSecurityAuthority.DesiredAccess.LookupNames | LocalSecurityAuthority.DesiredAccess.ViewLocalInformation))
			{
				//const string r = "SeBackupPrivilege";
				var right = SystemPrivilege.Debug;
				foreach (var t in h.EnumerateAccountsWithRight(right))
					AddTestResult(null, $"Acct w/{right}", ((NTAccount)t.Translate(typeof(NTAccount))).Value);
			}
		}

		[ReqPriv]
		public static void LsaLookupNames()
		{
			var accts = new[] { "SYSTEM", "Administrator", "AMERICAS", "david.a.hall@hpe.com", Environment.MachineName, "brad.spivack@hpe.com" };
			using (var h = new LocalSecurityAuthority(null, LocalSecurityAuthority.DesiredAccess.LookupNames))
			{
				var ret1 = h.LookupNames(false, accts).ToArray();
				for (var i = 0; i < ret1.Length; i++)
					AddTestResult(null, $"Lookup:{accts[i]}", ret1[i].ToString());
			}
		}

		[ReqPriv]
		public static void LsaUserLogonRights()
		{
			var un = $"{Environment.UserDomainName}\\{Environment.UserName}";

			using (var h = new LocalSecurityAuthority(null, LocalSecurityAuthority.DesiredAccess.LookupNames | LocalSecurityAuthority.DesiredAccess.ViewLocalInformation))
			{
				AddTestResult(null, $"LogonRight:{un}", h.UserLogonRights(un).Select(r => r.ToString()));
			}
		}

		[ReqPriv]
		public static void LsaUserPrivileges()
		{
			var un = $"{Environment.UserDomainName}\\{Environment.UserName}";

			using (var h = new LocalSecurityAuthority(null, LocalSecurityAuthority.DesiredAccess.LookupNames | LocalSecurityAuthority.DesiredAccess.ViewLocalInformation))
			{
				AddTestResult(null, $"Priv:{un}", h.UserPrivileges(un).Select(r => r.ToString()));
			}
		}

		public static void Process()
		{
			var cp = System.Diagnostics.Process.GetCurrentProcess();
			AddTestResult(null, "Integrity Level", cp.GetIntegrityLevel());
			var pp = cp.GetParentProcess();
			AddTestResult(null, "Parent Process ID", pp.Id);
			AddTestResult(null, "Sibling Process IDs", pp.GetChildProcesses(true).Select(p => p.Id.ToString()));
			AddTestResult(null, "Enabled Privs", cp.GetPrivileges().Where(p => (p.Attributes & AdvApi32.PrivilegeAttributes.SE_PRIVILEGE_ENABLED) > 0).Select(p => p.Privilege.ToString()));
			using (new PrivilegedCodeBlock(cp, SystemPrivilege.Backup))
				AddTestResult(null, "Has code block privilege", cp.HasPrivilege(SystemPrivilege.Backup));
			AddTestResult(null, "Has privileges", cp.HasPrivileges(true, SystemPrivilege.Debug, SystemPrivilege.Impersonate));
		}

		private static void PropVariant()
		{
			Ole32.PROPVARIANT pv;
			// SafeArray
			pv = new Ole32.PROPVARIANT(new object[] { 1, DateTime.Now, Guid.NewGuid() });
			AddTestResult(null, pv.ToString(), pv.parray);
			pv = new Ole32.PROPVARIANT(new object[] { 1, 2, 3 });
			AddTestResult(null, pv.ToString(), pv.parray);

			// Struct
			var ms = new SafeCoTaskMemString("TESTFMT");
			pv = new Ole32.PROPVARIANT(new[] { new Ole32.CLIPDATA(ms.DangerousGetHandle(), ms.Length), new Ole32.CLIPDATA(0xCFF) });
			AddTestResult(null, pv.ToString(), pv.caclipdata.ToArray()[0].ulClipFmt);

			// Dates
			pv = new Ole32.PROPVARIANT((DateTime?)null);
			AddTestResult(null, pv.ToString(), pv.pdate);
			pv = new Ole32.PROPVARIANT(DateTime.Now.AddYears(-500));
			AddTestResult(null, pv.ToString(), pv.Value);
			pv = new Ole32.PROPVARIANT(DateTime.Now);
			AddTestResult(null, pv.ToString(), new[] { pv.Value, pv.date, pv.pdate });
			pv = new Ole32.PROPVARIANT(DateTime.Now, VarEnum.VT_FILETIME);
			AddTestResult(null, pv.ToString(), pv.filetime.ToString(null));
			pv = new Ole32.PROPVARIANT(new[] { DateTime.Now, DateTime.Today, DateTime.MaxValue });
			AddTestResult(null, pv.ToString(), pv.cadate);

			// FILETIME
			var ft = DateTime.Now.ToFileTimeStruct();
			pv = new Ole32.PROPVARIANT(ft);
			AddTestResult(null, pv.ToString(), pv.filetime.ToString(null));
			pv = new Ole32.PROPVARIANT(new[] { ft, ft });
			AddTestResult(null, pv.ToString(), pv.cafiletime?.Select(f => f.ToString("G")));
			pv = new Ole32.PROPVARIANT(new[] { DateTime.Now, DateTime.Today, DateTime.MaxValue }, VarEnum.VT_FILETIME);
			AddTestResult(null, pv.ToString(), pv.cafiletime?.Select(f => f.ToString("G")));

			// Bool
			pv = new Ole32.PROPVARIANT(true);
			AddTestResult(null, pv.ToString(), new[] { pv.pboolVal, pv.boolVal });
			pv = new Ole32.PROPVARIANT(new[] { true, false, true, false });
			AddTestResult(null, pv.ToString(), pv.cabool);
			PropSys.InitPropVariantFromBooleanVector(new[] { true, false, true, false }, 4, pv);
			AddTestResult(null, pv.ToString(), pv.cabool);
			AddTestResult(null, pv.ToString(), pv.cai);

			// Int
			pv = new Ole32.PROPVARIANT((int?)null);
			AddTestResult(null, pv.ToString(), pv.plVal);
			pv = new Ole32.PROPVARIANT(3);
			AddTestResult(null, pv.ToString(), new[] { pv.Value, pv.bVal, pv.cVal, pv.iVal, pv.uiVal, pv.intVal, pv.lVal, pv.uintVal, pv.ulVal, pv.hVal, pv.uhVal, pv.scode });
			pv = new Ole32.PROPVARIANT(new Win32Error(5));
			AddTestResult(null, pv.ToString(), $"{pv.lVal}/{(int)pv.scode}");
			pv = new Ole32.PROPVARIANT(new[] { 0, 1, 2 });
			AddTestResult(null, pv.ToString(), pv.cal);

			// Decimal
			pv = new Ole32.PROPVARIANT(3.3m);
			AddTestResult(null, pv.ToString(), new[] { pv.Value, pv.pdecVal, pv.bVal, pv.cVal, pv.iVal, pv.uiVal, pv.intVal, pv.lVal, pv.uintVal, pv.ulVal, pv.hVal, pv.uhVal, pv.fltVal, pv.dblVal });
			pv = new Ole32.PROPVARIANT(3.3m, VarEnum.VT_CY);
			AddTestResult(null, pv.ToString(), new[] { pv.Value, pv.pdecVal, pv.cyVal, pv.pcyVal });
			pv = new Ole32.PROPVARIANT(new[] { 0m, 1m, 2m }, VarEnum.VT_CY);
			AddTestResult(null, pv.ToString(), pv.cacy);

			// Single/double precision
			pv = new Ole32.PROPVARIANT(3.3f);
			AddTestResult(null, pv.ToString(), new[] { pv.Value, pv.fltVal, pv.dblVal, pv.pdecVal, pv.bVal, pv.cVal, pv.iVal, pv.uiVal, pv.intVal, pv.lVal, pv.uintVal, pv.ulVal, pv.hVal, pv.uhVal });
			pv = new Ole32.PROPVARIANT(3.3);
			AddTestResult(null, pv.ToString(), new[] { pv.Value, pv.fltVal, pv.dblVal });
			pv = new Ole32.PROPVARIANT(new[] { 0.0f, 1.1f, 2.2f });
			AddTestResult(null, pv.ToString(), pv.caflt);
			pv = new Ole32.PROPVARIANT(new[] { 0.0, 1.1, 2.2 });
			AddTestResult(null, pv.ToString(), pv.cadbl);

			// Byte
			pv = new Ole32.PROPVARIANT((byte)3);
			AddTestResult(null, pv.ToString(), pv.bVal);
			pv = new Ole32.PROPVARIANT(new byte[] { 0, 1, 2 });
			AddTestResult(null, pv.ToString(), pv.caub);

			// Guid
			pv = new Ole32.PROPVARIANT(Guid.NewGuid());
			AddTestResult(null, pv.ToString(), pv.puuid);
			pv = new Ole32.PROPVARIANT(new[] { Guid.NewGuid(), Guid.NewGuid() });
			AddTestResult(null, pv.ToString(), pv.cauuid);

			// String
			pv = new Ole32.PROPVARIANT("TestNoVTVal");
			AddTestResult(null, pv.ToString(), new[] { pv.Value, pv.pszVal, pv.pwszVal, pv.bstrVal, pv.pbstrVal });
			pv = new Ole32.PROPVARIANT("TestBSTR", VarEnum.VT_BSTR);
			AddTestResult(null, pv.ToString(), new[] { pv.Value, pv.pszVal, pv.pwszVal, pv.bstrVal, pv.pbstrVal });
			pv = new Ole32.PROPVARIANT("TestLPSTR", VarEnum.VT_LPSTR);
			AddTestResult(null, pv.ToString(), new[] { pv.Value, pv.pszVal, pv.pwszVal, pv.bstrVal, pv.pbstrVal });
			pv = new Ole32.PROPVARIANT("TestLPWSTR", VarEnum.VT_LPWSTR);
			AddTestResult(null, pv.ToString(), new[] { pv.Value, pv.pszVal, pv.pwszVal, pv.bstrVal, pv.pbstrVal });
			pv = new Ole32.PROPVARIANT(new[] { "TestStrVec", "X" });
			AddTestResult(null, pv.ToString(), pv.calpwstr);
			pv = new Ole32.PROPVARIANT(new[] { 1, 2, 3 }, VarEnum.VT_LPWSTR);
			AddTestResult(null, pv.ToString(), pv.calpwstr);
			PropSys.InitPropVariantFromStringVector(new[] { "TestStrVec", "X" }, 2, pv);
			AddTestResult(null, pv.ToString(), pv.calpwstr);
		}

		public static void PSID()
		{
			AdvApi32.PSID pSid;
			string dn;
			AdvApi32.SID_NAME_USE snu;
			AdvApi32.LookupAccountName(null, "SYSTEM", out pSid, out dn, out snu);
			var newSid = AdvApi32.PSID.Init(AdvApi32.SECURITY_NT_AUTHORITY, 0x12); // S-1-5-18
			AddTestResult(null, "Lookup SYSTEM == Built SYSTEM", pSid == newSid);
		}

		public static void QueryDosDevice()
		{
			var qdd = Kernel32.QueryDosDevice("C:\\");
			AddTestResult(null, "Root disk", $"{qdd.FirstOrDefault()}");
		}

		[TestName("Resource Icon")]
		public static void ResourceFile(Form parentForm)
		{
			var rf = new Vanara.Resources.ResourceFile(@"C:\Windows\System32\imageres.dll");
			var names = rf.GetResourceNames(User32.ResourceType.RT_GROUP_ICON);
			AddTestResult(null, "Icons", names);
			using (var dlg = new ImageDialog { ClientIcon = rf.GroupIcons[3] })
				dlg.ShowDialog(parentForm);
		}

		public static void ShellItem()
		{
			var shi = new ShellItem(Shell32.KNOWNFOLDERID_Enum.FOLDERID_PrintersFolder);
			AddTestResult(null, "ToolTip", shi.ToolTipText);
			AddTestResult(null, "Icon", shi.IconFilePath);
			AddTestResult(null, "Children", shi.EnumerateChildren());
		}

		public static void ShellLink()
		{
			const string fn = @"C:\Users\dahall\Desktop\Test.lnk";
			const string sysfile =
				@"C:\Users\dahall\Documents\Visual Studio 2010\Projects\ThemeExplorer\bin\Debug\ThemeExplorer.exe";
			var lnk = Vanara.Windows.Shell.ShellLink.Create(fn, sysfile, "Opens text files", @"%USERPROFILE%", null);
			lnk.ShowState = FormWindowState.Maximized;
			lnk.HotKey = Keys.Alt | Keys.Control | Keys.N;
			lnk.RunAsAdministrator = true;
			AddTestResult(null, lnk.ToString(), lnk.Properties);
		}

		public static void VolumeInfo()
		{
			string volName, fsName;
			int volSn, compLen;
			Kernel32.FileSystemFlags fsFlags;
			Kernel32.GetVolumeInformation(null, out volName, out volSn, out compLen, out fsFlags, out fsName);
			AddTestResult(null, "Root disk", $"Vol:{volName};VolSN:{volSn};FnLen:{compLen};FsName{fsName};FsFlg:{fsFlags}");
		}

		private static void AddTestResult<T>(string test, string param, IEnumerable<T> result, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
		{
			TestResult?.Invoke(null, new TestResultEventArgs(test ?? memberName, param, string.Join(";", result)));
		}

		private static void AddTestResult(string test, string param, string result, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
		{
			TestResult?.Invoke(null, new TestResultEventArgs(test ?? memberName, param, result));
		}

		private static void AddTestResult(string test, string param, object result, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
		{
			TestResult?.Invoke(null, new TestResultEventArgs(test ?? memberName, param, result?.ToString()));
		}
	}

	internal static class TestUtil
	{
		internal static string GetTestName(MethodInfo mi) => mi.GetCustomAttributes(typeof(TestNameAttribute), false).Cast<TestNameAttribute>().FirstOrDefault()?.Name ?? mi.Name;

		private static IEnumerable<MethodInfo> GetSimpleMethods()
		{
			var meths = typeof(Tests).GetMethods(BindingFlags.Static | BindingFlags.Public);
			Array.Sort(meths, (a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
			var isElev = Process.GetCurrentProcess().IsElevated();
			return meths.Where(mi => mi.GetParameters().Length == 0 && (isElev || mi.GetCustomAttributes(typeof(ReqPrivAttribute), false).Length == 0));
		}

		public static IDictionary<string, MethodInfo> GetUIMethods()
		{
			var meths = typeof(Tests).GetMethods(BindingFlags.Static | BindingFlags.Public);
			Array.Sort(meths, (a, b) => string.Compare(a.Name, b.Name, StringComparison.Ordinal));
			return meths.Where(mi => mi.GetParameters().Length == 1).ToDictionary(GetTestName);
		}

		internal static void Run(EventHandler<TestResultEventArgs> h)
		{
			Tests.TestResult += h;
			foreach (var m in GetSimpleMethods())
			{
				try
				{
					m.Invoke(null, null);
				}
				catch (Exception exception)
				{
					h?.Invoke(null, new TestResultEventArgs("Error", m.Name, exception.ToString()));
				}
			}
			Tests.TestResult -= h;
		}

		internal static void RunEachMultiple(EventHandler<TestResultEventArgs> h, int count = 100)
		{
			Tests.TestResult += h;
			foreach (var m in GetSimpleMethods())
			{
				for (var i = 0; i < count; i++)
				{
					try
					{
						m.Invoke(null, null);
					}
					catch (Exception exception)
					{
						h?.Invoke(null, new TestResultEventArgs("Error", m.Name, exception.ToString()));
					}
				}
				GC.Collect();
			}
			Tests.TestResult -= h;
		}
	}

	internal class ReqPrivAttribute : Attribute
	{
	}

	internal class TestNameAttribute : Attribute
	{
		public TestNameAttribute(string name) { Name = name; }

		public string Name { get; }
	}

	internal class TestResultEventArgs : EventArgs
	{
		internal TestResultEventArgs(string test, string param, string result)
		{
			Test = test;
			Param = param;
			Result = result;
		}

		public string Param { get; }
		public string Result { get; }
		public string Test { get; }
	}
}