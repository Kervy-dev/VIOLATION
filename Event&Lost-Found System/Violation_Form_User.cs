using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlTypes;
using System.Windows.Forms;

namespace Event_Lost_Found_System
{
    public partial class Violation_Form_User : Form
    {
        public Violation_Form_User()
        {
            InitializeComponent();
        }
        private int userId;

        // Navigate to the 'user' form
        private void btnH_Click(object sender, EventArgs e)
        {
            new Home_User().Show();
            this.Hide();
        }

        // Exit the application
        private void btn1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Navigate to the 'offense' form
        private void btnclubs_Click(object sender, EventArgs e)
        {
            new offense().Show();
            this.Hide();
        }

        // Reload the current form
        private void btnvio_Click(object sender, EventArgs e)
        {
            new Violation_Form_User().Show();
            this.Hide();
        }

        // Navigate to the 'prof' form
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                Dashboard_User profileForm = new Dashboard_User(userId);
                profileForm.Show(); // Show the profile form
                this.Hide();        // Hide the login form
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while opening the 'prof' form: " + ex.Message);
            }
        }

        // Insert selected category and StudentID into the database and notify admin/user
        private void btns_Click(object sender, EventArgs e)
        {
            if (cbViolation.SelectedItem == null)
            {
                MessageBox.Show("Please select a violation category.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtStudentID.Text))
            {
                MessageBox.Show("Please enter a valid Student ID.");
                return;
            }


            string studentID = txtStudentID.Text.Trim();
            int selectedViolationID = int.Parse(cbViolation.SelectedValue.ToString());

            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb";

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Check if violation record already exists
                    string checkQuery = "SELECT COUNT(*) FROM [Student Violation Records] WHERE Student_ID = @StudentID AND Offense_ID = @Violation";
                    using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@StudentID", studentID);
                        checkCmd.Parameters.AddWithValue("@Violation", selectedViolationID);

                        int existingCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (existingCount > 0)
                        {
                            // Update counts if already exists
                            string updateQuery = "UPDATE [Student Violation Records] SET counts = counts + 1 WHERE Student_ID = @StudentID AND Offense_ID = @Violation";
                            using (OleDbCommand updateCmd = new OleDbCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@StudentID", studentID);
                                updateCmd.Parameters.AddWithValue("@Violation", selectedViolationID);
                                updateCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Violation already recorded. Counts updated.");
                        }
                        else
                        {
                            // Insert a new record
                            string insertQuery = "INSERT INTO [Student Violation Records] (Student_ID, Offense_ID) VALUES (@StudentID, @Violation)";
                            using (OleDbCommand insertCmd = new OleDbCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@StudentID", studentID);
                                insertCmd.Parameters.AddWithValue("@Violation", selectedViolationID);
                                
                                insertCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Violation successfully recorded.");
                        }
                    }

                    // Notify the admin
                    string adminNotificationMessage = $"New violation committed by Student ID {studentID}: {selectedViolationID}";
                    string adminNotificationQuery = "INSERT INTO Notifications (Message, IsRead) VALUES (@Message, False)";
                    using (OleDbCommand cmdAdminNotification = new OleDbCommand(adminNotificationQuery, conn))
                    {
                        cmdAdminNotification.Parameters.AddWithValue("@Message", adminNotificationMessage);
                        cmdAdminNotification.ExecuteNonQuery();
                    }

                    // Notify the user
                    string userNotificationMessage = "Your violation has been submitted.";
                    string userNotificationQuery = "INSERT INTO UserNotification (Message) VALUES (@Message)";
                    using (OleDbCommand cmdUserNotification = new OleDbCommand(userNotificationQuery, conn))
                    {
                        cmdUserNotification.Parameters.AddWithValue("@Message", userNotificationMessage);
                        cmdUserNotification.ExecuteNonQuery();
                    }

                    RefreshViolationForm();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Database error: " + ex.Message);
                }
            }
        }

        // Refresh the Violation form if it's open
        private void RefreshViolationForm()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form is Violation_Crud_Admin violationForm)
                {
                    violationForm.LoadData();
                    break;
                }
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            new Statistic_User().Show();
            this.Hide();
        }

        private void txtStudentID_TextChanged(object sender, EventArgs e)
        {

        }

        private void LoadViolations()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Retrieve data from the database
                    OleDbDataAdapter offenseAdapter = new OleDbDataAdapter("SELECT Offenses_ID, Offenses FROM [Offenses]", conn);
                    DataTable offenseTable = new DataTable();
                    offenseAdapter.Fill(offenseTable);

                    // Assign data source to the ComboBox
                    cbViolation.DataSource = offenseTable;
                    cbViolation.DisplayMember = "Offenses";
                    cbViolation.ValueMember = "Offenses_ID";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data into dropdown: " + ex.Message);
                }
            }
        }

        private void cbViolation_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    
                    // Ensure a valid selection
                    if (cbViolation.SelectedValue != null && cbViolation.SelectedItem != null)
                    {
                        // Get the selected values
                        string selectedCategory = ((DataRowView)cbViolation.SelectedItem)["Offenses"].ToString();
                        int selectedViolationID = Convert.ToInt32(cbViolation.SelectedValue);

                        // Use the selected values as needed
                        MessageBox.Show($"Selected Offense: {selectedCategory}\nOffense ID: {selectedViolationID}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error handling selection: " + ex.Message);
                }
            }
        }

        private void cbViolation_DropDown(object sender, EventArgs e)
        {
            LoadViolations();
        }
    }
}
