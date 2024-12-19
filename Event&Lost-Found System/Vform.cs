using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Event_Lost_Found_System
{
    public partial class Vform : Form
    {
        public Vform()
        {
            InitializeComponent();
        }
        private int userId;

        // Navigate to the 'user' form
        private void btnH_Click(object sender, EventArgs e)
        {
            new user().Show();
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
            new Vform().Show();
            this.Hide();
        }

        // Navigate to the 'prof' form
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            try
            {
                prof profileForm = new prof(userId);
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
            string selectedViolation = cbViolation.SelectedItem.ToString();

            string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb";

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Check if the violation already exists for the student
                    string checkQuery = "SELECT COUNT(*) FROM Violation WHERE StudentID = @StudentID AND Violation = @Violation";
                    using (OleDbCommand checkCmd = new OleDbCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@StudentID", studentID);
                        checkCmd.Parameters.AddWithValue("@Violation", selectedViolation);

                        int existingCount = (int)checkCmd.ExecuteScalar();

                        if (existingCount > 0)
                        {
                            // Update the counts if the violation already exists
                            string updateQuery = "UPDATE Violation SET counts = counts + 1 WHERE StudentID = @StudentID AND Violation = @Violation";
                            using (OleDbCommand updateCmd = new OleDbCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@StudentID", studentID);
                                updateCmd.Parameters.AddWithValue("@Violation", selectedViolation);
                                updateCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Violation already exists. Counts updated successfully!");
                        }
                        else
                        {
                            // Insert a new record if the violation does not exist
                            string insertQuery = "INSERT INTO Violation (StudentID, Violation, counts) VALUES (@StudentID, @Violation, 1)";
                            using (OleDbCommand insertCmd = new OleDbCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@StudentID", studentID);
                                insertCmd.Parameters.AddWithValue("@Violation", selectedViolation);
                                insertCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("New violation recorded successfully!");
                        }
                    }

                    // Notify the admin
                    string adminNotificationMessage = $"New violation committed by Student ID {studentID}: {selectedViolation}";
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
                if (form is violation violationForm)
                {
                    violationForm.LoadData();
                    break;
                }
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            new Statistic().Show();
            this.Hide();
        }

        private void txtStudentID_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
