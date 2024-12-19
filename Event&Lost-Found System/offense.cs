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
            new Violation_Form_User().Show();
            this.Hide();
        }

        private void btnH_Click(object sender, EventArgs e)
        {
            new Home_User().Show();
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
            Major_Offenses_User majorForm = new Major_Offenses_User();
            majorForm.Show();
            this.Hide(); // Hide the current form
        }

        private void ShowMinorForm()
        {
            // Open the form for Minor Offenses
            Minor_Offenses_User minorForm = new Minor_Offenses_User();
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
            Dashboard_User profileForm = new Dashboard_User(userId);
            profileForm.Show(); // Show the profile form
            this.Hide();        // Hide the login form
        }

        private void button25_Click(object sender, EventArgs e)
        {
            new Statistic_User().Show();
            this.Hide();
        }
    }
}
