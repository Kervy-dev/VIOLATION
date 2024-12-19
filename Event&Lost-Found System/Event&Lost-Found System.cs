using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Event_Lost_Found_System
{
    public partial class Form1 : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeft, int nTop, int nRight, int nBottom, int nWidthEllipse, int nHeightEllipse);

        public Form1()
        {
            InitializeComponent();
        }
        
        private int GetLoggedInUserId()
        {
            // Assume you have the username and password entered by the user
            string username = userID.Text;
            string password = pass.Text;


            return 0;
           
        }

        OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb");
        OleDbCommand cmd = new OleDbCommand();
        private int loginAttempts = 0; // Track login attempts

        private void userID_Click(object sender, EventArgs e)
        {
            userID.BackColor = Color.LightBlue;
            panel3.BackColor = Color.LightBlue;
            panel4.BackColor = SystemColors.Control;
            pass.BackColor = SystemColors.Control;
        }

        private void pass_Click(object sender, EventArgs e)
        {
            pass.BackColor = Color.LightBlue;
            panel4.BackColor = Color.LightBlue;
            userID.BackColor = SystemColors.Control;
            panel3.BackColor = SystemColors.Control;
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            pass.UseSystemPasswordChar = false;
        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            pass.UseSystemPasswordChar = true;
        }

        private void btnlog_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                try
                {
                    // Set up the database connection
                    using (OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb"))
                    {
                        // Open the connection
                        con.Open();
                        int userId = GetLoggedInUserId();


                        if (userId > 0) // Check if userId is valid
                        {
                            // Pass the userId to the profile form
                            prof profileForm = new prof(userId);
                            profileForm.Show();
                            this.Hide();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during login: " + ex.Message);
                }

                

                // SQL query to use UNION to fetch username and password from both BPC and SII tables
                string query = @"
            SELECT [Username], [Password], [Type], [HasAcceptedTerms] 
            FROM BPC 
            WHERE Username = @username AND Password = @password
            UNION
            SELECT [Username], [Password], [Type], NULL AS HasAcceptedTerms 
            FROM SII 
            WHERE Username = @username AND Password = @password
        ";

                // Command setup
                cmd = new OleDbCommand(query, con);
                cmd.Parameters.AddWithValue("@username", userID.Text);
                cmd.Parameters.AddWithValue("@password", pass.Text);

        
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string userType = reader["Type"].ToString();
                    bool hasAcceptedTerms = reader["HasAcceptedTerms"] != DBNull.Value && (bool)reader["HasAcceptedTerms"];

                    // Insert activity log entry for login time
                    string formattedTimeIn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); // Format time without milliseconds
                    string insertLogQuery = "INSERT INTO ActivityLog (Username, LoginTime) VALUES (@username, @timeIn)";
                    OleDbCommand insertCmd = new OleDbCommand(insertLogQuery, con);

                    // Assuming userId is part of the returned data or retrieved by another method
                    string username = reader["Username"].ToString(); // or any other unique identifier from the reader
                    insertCmd.Parameters.AddWithValue("@username", username);
                    insertCmd.Parameters.AddWithValue("@timeIn", formattedTimeIn); // Use formatted time without milliseconds
                                                                                  
                    // Execute the insert to record login
                    insertCmd.ExecuteNonQuery();

                    // Handle Terms and Conditions logic for students (if HasAcceptedTerms exists in BPC)
                    if (userType == "Student" && hasAcceptedTerms == false)
                    {
                        TermsAndConditionsForm termsForm = new TermsAndConditionsForm();
                        DialogResult result = termsForm.ShowDialog();

                        if (result == DialogResult.OK)
                        {
                            UpdateUserTermsStatus(userID.Text); // Update the terms status in the database
                            MessageBox.Show("Terms and Conditions accepted. You can now proceed.", "Success");
                        }
                        else
                        {
                            MessageBox.Show("You must accept the Terms and Conditions to continue.", "Notice");
                            return; // Stop execution if terms are not accepted
                        }
                    }

                    // Hide the current form and show the appropriate form based on user type
                    this.Hide();

                    // Display message to user (Optional, can be removed if not needed)
                    MessageBox.Show($"Welcome, {userID.Text}! You are logged in as a {userType}.");

                    if (userType == "Student")
                    {
                        user studentForm = new user();
                        studentForm.Show();
                    }
                    else if (userType == "Admin")
                    {
                        Form3 adminForm = new Form3();
                        adminForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Unrecognized user type.");
                        this.Show(); // Show login form again if user type is unrecognized
                    }
                }
                else
                {
                    // Increment login attempts and show error message
                    loginAttempts++;
                    MessageBox.Show("Invalid username or password.");

                    if (loginAttempts >= 3)
                    {
                        MessageBox.Show("Too many failed login attempts. The application will now exit.");
                        Application.Exit(); // Exit the application if 3 failed attempts

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void UpdateUserTermsStatus(string username)
        {
            try
            {
                // Close connection if it's open and reopen it for the update query
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                string updateQuery = "UPDATE BPC SET HasAcceptedTerms = True WHERE Username = @username";

                OleDbCommand updateCmd = new OleDbCommand(updateQuery, con);
                updateCmd.Parameters.AddWithValue("@username", username);

                con.Open();
                updateCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating terms status: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void login_fpass_Click(object sender, EventArgs e)
        {
            new forgot().Show();
            this.Hide();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Exit the application
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set rounded corners for the login button
            btnlog.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnlog.Width, btnlog.Height, 25, 25));
        }
    }
}
