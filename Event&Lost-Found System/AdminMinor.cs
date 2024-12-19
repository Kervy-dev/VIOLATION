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
    public partial class AdminMinor : Form
    {
        public AdminMinor()
        {
            InitializeComponent();
        }

        private void AdminMinor_Load(object sender, EventArgs e)
        {
            // Ensure that the ComboBox is populated with "Minor" and "Major"
            offensesComboBox.Items.Clear();
            offensesComboBox.Items.Add("Minor");
            offensesComboBox.Items.Add("Major");

            // Set the default value for ComboBox to Minor
            offensesComboBox.SelectedIndex = 0;

            // Enable custom drawing for tab control
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl1.Padding = new Point(20, 5);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Change tab names based on selection
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

        private void offensesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Check the selected item in the ComboBox
            string selectedValue = offensesComboBox.SelectedItem.ToString();

            if (selectedValue == "Minor")
            {
                // Stay in Minor view
                ShowMinorOffenses();
            }
            else if (selectedValue == "Major")
            {
                // Open clubs form if Major is selected
                OpenClubsForm();
            }
        }

        private void ShowMinorOffenses()
        {
            // Logic to display Minor offenses
            tabPage1.Text = "Gambling/Vandalism";
            tabPage2.Text = "Indecent Conduct";
            tabPage3.Text = "Plagiarism";
            tabPage4.Text = "Threats/Intimidation";
            tabPage5.Text = "Theft";
            tabPage6.Text = "Drinking on Campus";
        }

        private void OpenClubsForm()
        {
            // Hide the current form and open the clubs form
            this.Hide(); // Hide the AdminMinor form
            clubs clubsForm = new clubs(); // Create a new instance of the Clubs form
            clubsForm.Show(); // Show the clubs form
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

        private void button25_Click(object sender, EventArgs e)
        {
            new StatisticAdmin().Show();
            this.Hide();
        }
    }
}
