using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Event_Lost_Found_System
{
    public partial class clubs : Form
    {
        public clubs()
        {
            InitializeComponent();
        }

        private void btnvio_Click(object sender, EventArgs e)
        {
            new violation().Show();
            this.Hide();
        }

        private void btnH_Click(object sender, EventArgs e)
        {
            new Form3().Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new home().Show();
            this.Hide();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabPage1.Text = "Gambling/Vandalism";
            tabPage2.Text = "Indecent Conduct";
            tabPage3.Text = "Plagiarism";
            tabPage4.Text = "Threats/Intimidation";
            tabPage5.Text = "Theft";
            tabPage6.Text = "Drinking on Campus";
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Draw the tab text with custom spacing
            string tabText = tabControl1.TabPages[e.Index].Text;
            using (Brush brush = new SolidBrush(Color.Black))
            {
                e.Graphics.DrawString(tabText, e.Font, brush, e.Bounds.X + 10, e.Bounds.Y + 5);
            }
        }

        private void clubs_Load(object sender, EventArgs e)
        {
            offensesComboBox.SelectedIndex = 0; // Set the default selected index for the combo box
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed; // Enable custom drawing for tabs
            tabControl1.Padding = new Point(20, 5); // Set padding between tabs
        }

        private void offensesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check if the selected item is "Minor"
            if (offensesComboBox.SelectedItem.ToString() == "Minor")
            {
                // Show the AdminMinor form
                new AdminMinor().Show();
                this.Hide(); // Optionally hide the current form (clubs)
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            new StatisticAdmin().Show();
            this.Hide();
        }
    }
}
