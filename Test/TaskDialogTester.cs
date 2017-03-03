using System;
using System.Windows.Forms;
using Vanara.Windows.Forms;

namespace TaskDialogDemo
{
	public partial class TaskDialogTester : Form
	{
		public TaskDialogTester()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			ReturnedButtonLabel.Text = string.Empty;
			/*
			if ( !TaskDialog.IsAvailableOnThisOS )
			{
				this.TaskDialogButton.Enabled = false;
				this.ReturnedButtonLabel.Text = "Requires OS version " + TaskDialog.RequiredOSVersion + " or later.";
			}
			 */
			progressWithTimerCheckBox.Checked = true;
			UpdateEnabledState();

			//SampleUsage ( );
			//SampleUsageComplex ( );
		}

		private void progressWithTimerCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			UpdateEnabledState();
		}

		private void SampleUsage(object sender, EventArgs e)
		{
			var result = TaskDialog.Show(this, "Do you want to do this?", null, "My Application", TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No, TaskDialogIcon.None);
			if (result == (int)DialogResult.Yes)
			{
				result = TaskDialog.Show(this, "Here is the question:", "", new string[] { "Finish\nComplete all actions and terminate", "Do nothing" });
				if (result != (int)DialogResult.Cancel)
				{
					result = TaskDialog.Show(this, "How many apples?", "Let us know how many apples you and your family want.", "", new string[] { "One", "Five", "Ten" });
					if (result != 0)
						TaskDialog.Show(this, $"Selected radio button: {result}");
				}
			}
		}

		private void SampleUsageComplex(object sender, EventArgs e)
		{
			var result = (int)taskDialog.ShowDialog();
			if (taskDialog.Result.VerificationFlagChecked)
			{
				// Suppress future asks.
			}
			if (result == 101)
			{
				// Do it.
			}
		}

		private void showProgressBarCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			UpdateEnabledState();
		}

		private void TaskDialogButton_Click(object sender, EventArgs e)
		{
			complexBtn.Enabled = false;
			taskDialog.Reset();
			taskDialog.WindowTitle = windowTitle.Text;
			taskDialog.MainInstruction = mainInstructionTextBox.Text;
			taskDialog.Content = contentTextBox.Text;
			taskDialog.Footer = footerTextBox.Text;
			taskDialog.ExpandedInformation = expandedInfoTextBox.Text;

			// Common buttons
			taskDialog.CommonButtons = (TaskDialogCommonButtons)Convert.ToInt32(commonBtnsCombo.SelectedValue);

			// Custom Buttons
			if (buttonIDTextBox1.TextLength != 0 && buttonTextBox1.TextLength != 0)
			{
				try
				{
					taskDialog.Buttons.Add(new TaskDialogButton(buttonTextBox1.Text, Convert.ToInt32(buttonIDTextBox1.Text, 10)));
				}
				catch (FormatException)
				{
				}
			}

			if (buttonIDTextBox2.TextLength != 0 && buttonTextBox2.TextLength != 0)
			{
				try
				{
					taskDialog.Buttons.Add(new TaskDialogButton(buttonTextBox2.Text, Convert.ToInt32(buttonIDTextBox2.Text, 10)) { CloseOnClick = false });
				}
				catch (FormatException)
				{
				}
			}

			if (buttonIDTextBox3.TextLength != 0 && buttonTextBox3.TextLength != 0)
			{
				try
				{
					taskDialog.Buttons.Add(new TaskDialogButton(buttonTextBox3.Text, Convert.ToInt32(buttonIDTextBox3.Text, 10)) { Enabled = false, ElevatedStateRequired = true });
				}
				catch (FormatException)
				{
				}
			}

			// DefaultButton
			if (defaultButtonTextBox.TextLength != 0)
			{
				try
				{
					taskDialog.DefaultButton = Convert.ToInt32(defaultButtonTextBox.Text, 10);
				}
				catch (FormatException)
				{
				}
			}

			// Radio Buttons
			if (radioButtonIDTextBox1.TextLength != 0 && radioButtonTextBox1.TextLength != 0)
			{
				try
				{
					taskDialog.RadioButtons.Add(new TaskDialogRadioButton(radioButtonTextBox1.Text, Convert.ToInt32(radioButtonIDTextBox1.Text, 10)));
				}
				catch (FormatException)
				{
				}
			}

			if (radioButtonIDTextBox2.TextLength != 0 && radioButtonTextBox2.TextLength != 0)
			{
				try
				{
					taskDialog.RadioButtons.Add(new TaskDialogRadioButton(radioButtonTextBox2.Text, Convert.ToInt32(radioButtonIDTextBox2.Text, 10)));
				}
				catch (FormatException)
				{
				}
			}

			if (radioButtonIDTextBox3.TextLength != 0 && radioButtonTextBox3.TextLength != 0)
			{
				try
				{
					taskDialog.RadioButtons.Add(new TaskDialogRadioButton(radioButtonTextBox3.Text, Convert.ToInt32(radioButtonIDTextBox3.Text, 10)));
				}
				catch (FormatException)
				{
				}
			}

			// DefaultRadioButton
			if (defaultRadioButtonTextBox.TextLength != 0)
			{
				try
				{
					taskDialog.DefaultRadioButton = Convert.ToInt32(defaultRadioButtonTextBox.Text, 10);
				}
				catch (FormatException)
				{
				}
			}

			// Main Icon
			if (mainIconPath.TextLength == 0)
				taskDialog.MainIcon = (TaskDialogIcon)mainIconComboBox.SelectedValue;
			else
				taskDialog.CustomMainIcon = new System.Drawing.Icon(mainIconPath.Text);

			// Footer Icon
			if (footerIconPath.TextLength == 0)
				taskDialog.FooterIcon = (TaskDialogIcon)footerIconComboBox.SelectedValue;
			else
				taskDialog.CustomFooterIcon = new System.Drawing.Icon(footerIconPath.Text);

			taskDialog.EnableHyperlinks = enableHyperlinksCheckBox.Checked;
			taskDialog.ProgressBar.Visible = showProgressBarCheckBox.Checked || showMarqueeCheckBox.Checked;
			if (showMarqueeCheckBox.Checked)
				taskDialog.ProgressBar.Style = ProgressBarStyle.Marquee;
			taskDialog.AllowDialogCancellation = allowCancelCheckBox.Checked;
			taskDialog.CallbackTimer = progressWithTimerCheckBox.Checked;
			taskDialog.ExpandedByDefault = expandedByDefaultCheckBox.Checked;
			taskDialog.ExpandFooterArea = expandedFooterCheckBox.Checked;
			taskDialog.PositionRelativeToWindow = positionRelativeToWindowCheckBox.Checked;
			taskDialog.RightToLeftLayout = RightToLeftLayoutCheckbox.Checked;
			taskDialog.NoDefaultRadioButton = NoDefaultRadioButtonCheckBox.Checked;
			taskDialog.CanBeMinimized = CanBeMinimizedCheckBox.Checked;
			taskDialog.ButtonDisplay = (TaskDialogButtonDisplay)buttonPlacementCombo.SelectedValue;
			taskDialog.VerificationText = verficationFlagTextBox.Text;
			taskDialog.VerificationFlagChecked = verifyFlagCheckBox.Checked;
			taskDialog.ExpandedControlText = expandedControlTextBox.Text;
			taskDialog.CollapsedControlText = collapsedControlTextBox.Text;

			//
			// Show the Dialog
			//
			taskDialog.ShowDialog((taskDialog.CanBeMinimized ? null : this));

			ReturnedButtonLabel.Text = $"Button Selected: {taskDialog.Result.DialogResult}   Verification: {(taskDialog.Result.VerificationFlagChecked ? "checked" : "clear")}   Radio Button: {taskDialog.Result.SelectedRadioButton}";
		}

		private void UpdateEnabledState()
		{
			progressWithTimerCheckBox.Enabled = showProgressBarCheckBox.Checked;
			autoCancelCheckBox.Enabled = showProgressBarCheckBox.Checked &&
				progressWithTimerCheckBox.Checked;
		}

		private void taskDialog_Timer(object sender, TaskDialog.TimerEventArgs e)
		{
			if (showProgressBarCheckBox.Checked)
			{
				if (e.TickCount < 10000)
					taskDialog.ProgressBar.Value = (int)e.TickCount / 100;
				else if (e.TickCount < 11000)
					taskDialog.ProgressBar.Value = 100;
				else if (e.TickCount < 12000 && autoCancelCheckBox.Checked)
					taskDialog.PerformButtonClick((int)DialogResult.Cancel);
				else
					e.Reset = true;
			}
		}

		private void taskDialog_Load(object sender, EventArgs e)
		{
			if (showMarqueeCheckBox.Checked)
				taskDialog.ProgressBar.MarqueeAnimationSpeed = 100;
		}

		private void taskDialog_LinkClicked(object sender, LinkClickedEventArgs e)
		{
			if (e.LinkText.StartsWith("http:", StringComparison.InvariantCultureIgnoreCase))
			{
				var psi = new System.Diagnostics.ProcessStartInfo
				{
					FileName = e.LinkText,
					UseShellExecute = true
				};
				System.Diagnostics.Process.Start(psi);
			}
			else
			{
				MessageBox.Show((IWin32Window)taskDialog, $"Link clicked. Hyperlink: {e.LinkText}", "Got callback");
			}
		}

		private void insertHyperlinkToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private bool onAssignment;

		private void mainIconComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!onAssignment)
			{
				onAssignment = true;
				mainIconPath.Text = null;
				// mainIconPic.Image = TaskDialog.
				onAssignment = false;
			}
		}

		private void mainIconPath_TextChanged(object sender, EventArgs e)
		{
			if (!onAssignment)
			{
				onAssignment = true;
				mainIconComboBox.SelectedValue = TaskDialogIcon.None;
				//mainIconPic.Image = TaskDialog.
				onAssignment = false;
			}
		}

		private void mainIconBrowse_Click(object sender, EventArgs e)
		{
			if (iconOpenFileDlg.ShowDialog(this) == DialogResult.OK)
				mainIconPath.Text = iconOpenFileDlg.FileName;
		}

		private void footerIconComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!onAssignment)
			{
				onAssignment = true;
				footerIconPath.Text = null;
				// footerIconPic.Image = TaskDialog.
				onAssignment = false;
			}
		}

		private void footerIconPath_TextChanged(object sender, EventArgs e)
		{
			if (!onAssignment)
			{
				onAssignment = true;
				footerIconComboBox.SelectedValue = TaskDialogIcon.None;
				// footerIconPic.Image = TaskDialog.
				onAssignment = false;
			}
		}

		private void footerIconBrowse_Click(object sender, EventArgs e)
		{
			if (iconOpenFileDlg.ShowDialog(this) == DialogResult.OK)
				footerIconPath.Text = iconOpenFileDlg.FileName;
		}
	}
}