using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Event_Lost_Found_System
{
    public partial class Change_Password : Form
    {
        // Database connection setup
        OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb");
        OleDbCommand cmd = new OleDbCommand();

        public Change_Password()
        {
            InitializeComponent();
        }
        private int userId;

        // Event handler for the Submit button click
        private void btnSub_Click(object sender, EventArgs e)
        {
            // Get the values entered by the user
            string username = tbUsername.Text;
            string currentPassword = textBox2.Text;
            string newPassword = tbpass.Text;
            string confirmPassword = tbre.Text;

            // Basic validation checks
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(currentPassword) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("All fields are required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("New password and confirm password do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Verify the username and current password in the database
            try
            {
                con.Open();
                string query = "SELECT [Password] FROM BPC WHERE [Username] = @username";
                cmd = new OleDbCommand(query, con);
                cmd.Parameters.AddWithValue("@username", username);

                // Execute the query and check if the user exists
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string storedPassword = reader["Password"].ToString();

                    // Check if the current password matches
                    if (storedPassword == currentPassword)
                    {
                        // Update the password in the database
                        string updateQuery = "UPDATE BPC SET [Password] = @newPassword WHERE [Username] = @username";
                        OleDbCommand updateCmd = new OleDbCommand(updateQuery, con);
                        updateCmd.Parameters.AddWithValue("@newPassword", newPassword);
                        updateCmd.Parameters.AddWithValue("@username", username);

                        int rowsAffected = updateCmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Password updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            // Optionally, clear the fields after successful update
                            tbUsername.Clear();
                            textBox2.Clear();
                            tbpass.Clear();
                            tbre.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Error updating password. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Current password is incorrect.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Username not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.Close();
            }
        }

        // Optional event handlers for textbox changes if you need them
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void tbpass_TextChanged(object sender, EventArgs e) { }
        private void tbre_TextChanged(object sender, EventArgs e) { }
        private void tbUsername_TextChanged(object sender, EventArgs e) { }

        private void bntclear_Click(object sender, EventArgs e)
        {
            Dashboard_User profileForm = new Dashboard_User(userId);
            profileForm.Show(); // Show the profile form
            this.Hide();        // Hide the login form
        }
    }
}
