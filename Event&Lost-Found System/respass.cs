using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace Event_Lost_Found_System
{
    public partial class respass : Form
    {
        private string userEmailOrPhone;
        private string username;

        // Update the constructor to accept email/phone and username
        public respass(string emailOrPhone, string user)
        {
            InitializeComponent();
            userEmailOrPhone = emailOrPhone;
            username = user; // Store the username
            lblUsername.Text = $"Resetting password for: {username}"; // Display the username
        }

        // Method to update the password in the database
        private void UpdatePassword(string username, string newPassword)
        {
            // Connection string for the Access database
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb";

            using (OleDbConnection con = new OleDbConnection(connectionString))
            {
                try
                {
                    con.Open();
                    string updateQuery = "UPDATE BPC SET [Password] = @Password WHERE [Username] = @Username";
                    using (OleDbCommand cmd = new OleDbCommand(updateQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@Password", newPassword);
                        cmd.Parameters.AddWithValue("@Username", username);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Password has been updated successfully.");

                            // Redirect to the login form
                            Form1 loginForm = new Form1(); // Replace with the actual name of your login form class
                            loginForm.Show(); // Show the login form
                            this.Close(); // Close the current form
                        }
                        else
                        {
                            MessageBox.Show("Username not found. Please check your username.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred while updating the password: " + ex.Message);
                }
            }
        }

        private void btnsubmit_Click_1(object sender, EventArgs e)
        {
            string newPassword = textBoxNewPassword.Text; // Get the new password
            string confirmPassword = textBoxConfirmPassword.Text; // Get the confirmation password

            if (newPassword == confirmPassword)
            {
                // Call the method to update the password in the database
                UpdatePassword(username, newPassword);
            }
            else
            {
                MessageBox.Show("Passwords do not match. Please try again.");
            }
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
