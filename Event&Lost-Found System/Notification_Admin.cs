using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace Event_Lost_Found_System
{
    public partial class Notification_Admin : Form
    {
        public Notification_Admin()
        {
            InitializeComponent();

            // Set up the ListBox for custom drawing
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.DrawItem += ListBox1_DrawItem;

            LoadNotifications(); // Load notifications when the form opens
        }

        // Database connection string
        private readonly string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb";
        private int userId;
        // Load notifications into the listBox1
        private void LoadNotifications()
        {
            listBox1.Items.Clear(); // Clear existing items

            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Query to get all notifications
                    string query = "SELECT ID, Message, IsRead FROM Notifications ORDER BY ID DESC";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string message = reader["Message"].ToString();
                            bool isRead = Convert.ToBoolean(reader["IsRead"]);

                            // Add the notification to the list box
                            listBox1.Items.Add(new ListBoxItem(message, reader["ID"].ToString(), !isRead)); // Unread = highlight
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading notifications: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Custom drawing for ListBox items
        private void ListBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            // Get the current item
            ListBoxItem item = listBox1.Items[e.Index] as ListBoxItem;

            if (item != null)
            {
                // Set background color for unread notifications
                if (item.IsUnread)
                {
                    e.Graphics.FillRectangle(Brushes.Yellow, e.Bounds); // Yellow background
                }
                else
                {
                    e.Graphics.FillRectangle(Brushes.White, e.Bounds); // White background for read
                }

                // Draw the text
                e.Graphics.DrawString(
                    item.Message,
                    e.Font,
                    Brushes.Black, // Text color
                    e.Bounds,
                    StringFormat.GenericDefault
                );

                // Draw the focus rectangle if the item is selected
                e.DrawFocusRectangle();
            }
        }

        // Mark notification as read when clicked
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem is ListBoxItem selectedItem)
            {
                string notificationID = selectedItem.ID;

                using (OleDbConnection conn = new OleDbConnection(connectionString))
                {
                    try
                    {
                        conn.Open();

                        // Update the notification as read in the database
                        string query = "UPDATE Notifications SET IsRead = True WHERE ID = @ID";
                        using (OleDbCommand cmd = new OleDbCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ID", notificationID);
                            cmd.ExecuteNonQuery();
                        }

                        // Reload notifications to reflect the changes
                        LoadNotifications();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error marking notification as read: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // Navigate to other forms (other handlers remain unchanged)
        // Navigate to the 'Records' form
        private void lb5_Click(object sender, EventArgs e)
        {
            new Records_Admin().Show();
            this.Hide();
        }

        private void lb4_Click(object sender, EventArgs e)
        {
            new Accounts_Admin().Show();
            this.Hide();
        }

        private void lb3_Click(object sender, EventArgs e)
        {
            new Notification_Admin().Show();
            this.Hide();
        }

        private void lb2_Click(object sender, EventArgs e)
        {
            new About_us_Admin().Show();
            this.Hide();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            new Dashboard_Admin().Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            new Home_Admin().Show();
            this.Hide();
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
                new Form1().Show();
                this.Hide();
            }
        }
    }

    // Helper class to represent a notification with metadata
    public class ListBoxItem
    {
        public string Message { get; }
        public string ID { get; }
        public bool IsUnread { get; }

        public ListBoxItem(string message, string id, bool isUnread)
        {
            Message = message;
            ID = id;
            IsUnread = isUnread;
        }

        public override string ToString()
        {
            return Message; // Display only the message in the list box
        }
    }
}
