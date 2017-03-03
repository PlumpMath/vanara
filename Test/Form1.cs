using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Vanara.Diagnostics;
using Vanara.Extensions;
using Vanara.Windows.Forms;

namespace Test
{
	public partial class Form1 : Form
	{
		//private readonly TaskDialogIcon[] icons;
		//private int iconIdx = 0;

		public Form1()
		{
			InitializeComponent();
			//icons = Enum.GetValues(typeof(TaskDialogIcon)).Cast<TaskDialogIcon>().ToArray();
		}

		private void abortShutdownToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SystemShutdown.AbortShutdown();
		}

		private void blockShutdownToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ShutdownBlockReasonSet("Can't kill me cuz I'm already dead.");
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var dict = TestUtil.GetUIMethods();
			var ret = TaskDialog.Show(this, @"Select a set of dialogs to test:", "UI Test", dict.Keys.ToArray());
			if (ret == (int)DialogResult.Cancel) return;
			dict.Values.ToArray()[ret - 101].Invoke(null, new object[] { this });
		}

		private void customDrawButton2_Click(object sender, EventArgs e)
		{
			var ip = new IPAddressBox { Text = @"192.168.0.1", TabStop = true };
			ip.SetFieldRange(0, 192, 192);
			themedTableLayoutPanel1.Controls.Add(ip);
		}

		private void customDrawControl1_Click(object sender, EventArgs e)
		{
			new TaskDialogDemo.TaskDialogTester().ShowDialog();
		}

		private void exitWindowsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SystemShutdown.ExitWindows();
		}

		private void Form1_Activated(object sender, EventArgs e) { themedPanel1.StyleState = 1; }

		private void Form1_Deactivate(object sender, EventArgs e) { themedPanel1.StyleState = 2; }

		private void Form1_Load(object sender, EventArgs e)
		{
			TestUtil.Run((s, args) => listView1.Items.Add(new ListViewItem(new[] { args.Test, args.Param, args.Result })));
		}

		private void Form1_Shown(object sender, EventArgs e)
		{
			comboBox1.SetCueBanner("Testing cue");
		}

		private void initiateShutdownToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SystemShutdown.InitiateShutdown(null, "Need to close this baby down", TimeSpan.FromSeconds(30));
		}

		private void lockWorkstationToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SystemShutdown.LockWorkStation();
		}
	}
}
