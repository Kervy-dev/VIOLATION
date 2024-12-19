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
    public partial class Minor_Offenses_User : Form
    {
        public Minor_Offenses_User()
        {
            InitializeComponent();
        }
        private int userId;

        private void btn2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void offensesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            
                string selectedCategory = offensesComboBox.SelectedItem.ToString();

                // Perform actions based on the selected item
                if (selectedCategory == "Major")
                {
                    // Show the Major form
                    tabControl1.SelectedIndex = 1; // Switch to TabPage at index 1
                    ShowMajorForm();
                }
                else if (selectedCategory == "Minor")
                {
                    // Show the Minor form
                    
                    ShowMinorForm();
                }

                // Reset the ComboBox back to "Offenses" without keeping the selection
                offensesComboBox.SelectedIndex = 0;

                void ShowMajorForm()
                {
                    // Open the form for Major Offenses
                    Major_Offenses_User majorForm = new Major_Offenses_User();
                    majorForm.Show();
                    this.Hide(); // Hide the current form
                }

                void ShowMinorForm()
                {
                    // Open the form for Minor Offenses
                    Minor_Offenses_User minorForm = new Minor_Offenses_User();
                    minorForm.Show();
                    this.Hide(); // Hide the current form

                }
            
        }

        private void Minor_Load(object sender, EventArgs e)
        {
          
            offensesComboBox.Items.Add("Major");
            offensesComboBox.Items.Add("Minor");
            
            tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed; // Enable custom drawing
            tabControl1.Padding = new Point(20, 5); // Set padding between tabs
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
        private void tabPage1_Click(object sender, EventArgs e)
        {
            tabPage1.Text = "No ID";
            tabPage2.Text = "Using Another's ID";
            tabPage3.Text = "Inappropriate Appearance";
            tabPage4.Text = "Improper Uniform";
            tabPage5.Text = "Gadget Use in Class";
            tabPage6.Text = "Skipping Class";
            tabPage1.BackColor = Color.Gray;
            tabPage2.BackColor = Color.Gray;
            tabPage3.BackColor = Color.Gray;
            tabPage4.BackColor = Color.Gray;
            tabPage5.BackColor = Color.Gray;
            tabPage6.BackColor = Color.Gray;
        }

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

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tabPage1.Text = "No ID";
            tabPage2.Text = "Using Another's ID";
            tabPage3.Text = "Inappropriate Appearance";
            tabPage4.Text = "Improper Uniform";
            tabPage5.Text = "Gadget Use in Class";
            tabPage6.Text = "Skipping Class";
            tabPage1.BackColor = Color.Gray;
            tabPage2.BackColor = Color.Gray;
            tabPage3.BackColor = Color.Gray;
            tabPage4.BackColor = Color.Gray;
            tabPage5.BackColor = Color.Gray;
            tabPage6.BackColor = Color.Gray;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Dashboard_User profileForm = new Dashboard_User(userId);
            profileForm.Show(); // Show the profile form
            this.Hide();        // Hide the login form
        }

        private void button31_Click(object sender, EventArgs e)
        {
            new Statistic_User().Show();
            this.Hide();
        }
    }
}
