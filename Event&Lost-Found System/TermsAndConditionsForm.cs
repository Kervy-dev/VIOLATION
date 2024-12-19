using System;
using System.Drawing;
using System.Windows.Forms;

namespace Event_Lost_Found_System
{
    public partial class TermsAndConditionsForm : Form
    {
        public TermsAndConditionsForm()
        {
            InitializeComponent();
        }

        // Event handler for checkbox change
        private void cbt_CheckedChanged_1(object sender, EventArgs e)
        {
            // If the checkbox is unchecked, prevent proceeding
            if (!cbt.Checked)
            {
                // Disable the Confirm button if checkbox is unchecked
                btnConfirm.Enabled = false;
            }
            else
            {
                // Enable the Confirm button if checkbox is checked
                btnConfirm.Enabled = true;
            }
        }

        // Custom paint event for the checkbox
        private void cbt_Paint(object sender, PaintEventArgs e)
        {
            // Safely cast sender to CheckBox
            CheckBox cb = sender as CheckBox;

            if (cb != null)
            {
                int checkBoxSize = 20; // Set the desired checkbox size

                // Clear the background
                e.Graphics.Clear(cb.BackColor);

                // Draw the checkbox square
                Rectangle rect = new Rectangle(0, (cb.Height - checkBoxSize) / 2, checkBoxSize, checkBoxSize);
                ControlPaint.DrawCheckBox(e.Graphics, rect, cb.Checked ? ButtonState.Checked : ButtonState.Normal);

                // Draw the text
                TextRenderer.DrawText(e.Graphics, cb.Text, cb.Font,
                    new Point(checkBoxSize + 5, (cb.Height - cb.Font.Height) / 2), cb.ForeColor);
            }
        }

        // Load event for the Terms and Conditions form
        private void TermsAndConditionsForm_Load(object sender, EventArgs e)
        {
            // Attach the Paint event for the checkbox
            cbt.Paint += new PaintEventHandler(cbt_Paint);
        }

        private void btnConfirm_Click_1(object sender, EventArgs e)
        {

            // Close the form after confirming
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
