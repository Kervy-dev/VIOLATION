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
    public partial class offense : Form
    {
        public offense()
        {
            InitializeComponent();
        }
        private int userId;

        private void btnvio_Click(object sender, EventArgs e)
        {
            new Vform().Show();
            this.Hide();
        }

        private void btnH_Click(object sender, EventArgs e)
        {
            new user().Show();
            this.Hide();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void offensesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (offensesComboBox.SelectedIndex > 0) // Avoid index 0 which is "Offenses"
            {
                string selectedCategory = offensesComboBox.SelectedItem.ToString();

                // Perform actions based on the selected item
                if (selectedCategory == "Major")
                {
                    // Show the Major form
                    ShowMajorForm();
                }
                else if (selectedCategory == "Minor")
                {
                    // Show the Minor form
                    ShowMinorForm();
                }

                // Reset the ComboBox back to "Offenses" without keeping the selection
                offensesComboBox.SelectedIndex = 0;
            }
        }

        private void ShowMajorForm()
        {
            // Open the form for Major Offenses
            major majorForm = new major();
            majorForm.Show();
            this.Hide(); // Hide the current form
        }

        private void ShowMinorForm()
        {
            // Open the form for Minor Offenses
            Minor minorForm = new Minor();
            minorForm.Show();
            this.Hide(); // Hide the current form

        }

        private void offense_Load(object sender, EventArgs e)
        {
            offensesComboBox.Items.Add("Offenses");
            offensesComboBox.SelectedIndex = 0; // Default to "Offenses"
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            prof profileForm = new prof(userId);
            profileForm.Show(); // Show the profile form
            this.Hide();        // Hide the login form
        }

        private void button25_Click(object sender, EventArgs e)
        {
            new Statistic().Show();
            this.Hide();
        }
    }
}
