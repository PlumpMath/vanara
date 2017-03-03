using System;
using System.Drawing;
using System.Windows.Forms;
using Vanara.Drawing;

namespace Test
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			TestUtil.RunEachMultiple(Write);
			Console.Write(@"\nPress any key to continue: ");
			Console.ReadKey();
			//Application.EnableVisualStyles();
			//Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new Form1());
			//var rf = new Vanara.Resources.ResourceFile(@"C:\Windows\System32\imageres.dll");
			//Application.Run(new ImageDialog { ClientIcon = rf.GroupIcons[3] });
		}

		private static int i;

		private static void Write(object s, TestResultEventArgs e) { Console.WriteLine(@"{3}:{0,-25}{1,-20}{2}", e.Test, e.Param, e.Result, i++); }
	}
}