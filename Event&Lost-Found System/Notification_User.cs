using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace Event_Lost_Found_System
{
    public partial class Notification_User : Form
    {
        public Notification_User()
        {
            InitializeComponent();
            LoadNotifications(); // Load existing notifications when the form opens
        }

        // Database connection string
        private readonly string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb";
        private int userId;
        // Load notifications into the ListBox
        private void LoadNotifications()
        {
            lb1.Items.Clear(); // Clear existing items

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Query to get all notifications
                    string query = "SELECT Message FROM UserNotification ORDER BY ID DESC";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string message = reader["Message"].ToString();
                            lb1.Items.Add(message); // Add each notification to the ListBox
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading notifications: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void lb2_Click(object sender, EventArgs e)
        {
            new About_us_User().Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Dashboard_User profileForm = new Dashboard_User(userId);
            profileForm.Show(); // Show the profile form
            this.Hide();        // Hide the login form
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new Home_User().Show();
            this.Hide();
        }

        private void lb1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Optional: Handle notification selection if needed
        }

        private void Logout_Click_1(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("Are you sure you want to Logout?", "Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {

                    // Set up the database connection
                    using (OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb"))
                    {
                        // Open the connection
                        con.Open();

                        // Format DateTime.Now to remove milliseconds
                        string formattedTimeOut = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        // Update the ActivityLog with the logout time
                        string updateLogQuery = "UPDATE ActivityLog SET LogoutTime = @timeOut WHERE UserID = @userId AND LogoutTime IS NULL";
                        OleDbCommand updateCmd = new OleDbCommand(updateLogQuery, con);
                        updateCmd.Parameters.AddWithValue("@timeOut", formattedTimeOut);
                        updateCmd.Parameters.AddWithValue("@userId", userId);

                        // Execute the update query
                        updateCmd.ExecuteNonQuery();
                    }

                    // Optionally, show a message saying logout was successful
                    MessageBox.Show("You have successfully logged out.");

                    // Show the login form again (assuming Form1 is the login form)
                    new Form1().Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    // Handle any errors that may occur during the logout process
                    MessageBox.Show("Error during logout: " + ex.Message);
                }
            }
        }
    }
}
