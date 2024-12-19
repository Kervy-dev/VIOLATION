using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions; // Required for regex validation
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Event_Lost_Found_System
{
    public partial class add : Form
    {
        public string Username { get; private set; } // Property to store the username for passing back

        public add()
        {
            InitializeComponent();
        }

        OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb");
        OleDbCommand cmd = new OleDbCommand();

        private void bntclear_Click(object sender, EventArgs e)
        {
            new acc().Show();
            this.Hide(); // Simply close this form to return to the previous one
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Exit the application
        }

        private void cb2_CheckedChanged_1(object sender, EventArgs e)
        {
            // Show or hide password characters based on checkbox state
            if (cb2.Checked)
            {
                sign_pass.PasswordChar = '\0';  // Show the password
                sign_re.PasswordChar = '\0';    // Show the password
            }
            else
            {
                sign_pass.PasswordChar = '*';  // Hide the password with '*'
                sign_re.PasswordChar = '*';    // Hide the password with '*'
            }
        }

        private void signup_create_Click_1(object sender, EventArgs e)
        {
            // Validate that all required fields are filled
            if (string.IsNullOrWhiteSpace(sign_ID.Text) || string.IsNullOrWhiteSpace(sign_un.Text) ||
                string.IsNullOrWhiteSpace(sign_pass.Text) || string.IsNullOrWhiteSpace(sign_re.Text))
            {
                MessageBox.Show("All fields are required. Please fill them in.", "Create Account Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (sign_pass.Text.Length < 8)
            {
                // Validate password minimum length
                MessageBox.Show("Password must be at least 8 characters long.", "Invalid Password Length", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                sign_pass.Focus(); // Focus on the password field for re-entry
            }
            else if (!IsPasswordValid(sign_pass.Text))
            {
                // Validate password format
                MessageBox.Show("Password must contain both letters and numbers.", "Invalid Password Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                sign_pass.Focus(); // Focus on the password field for re-entry
            }
            else if (sign_pass.Text == sign_re.Text)
            {
                try
                {
                    con.Open();

                    // Determine the user type based on ID # prefix
                    string userType = sign_ID.Text.StartsWith("MA") ? "Student" : "Admin";

                    // Prepare the SQL command for inserting the user data into the database
                    string signup = "INSERT INTO BPC ([ID #], [Username], [Password], [Type]) VALUES (@ID, @Username, @Password, @Type)";
                    cmd = new OleDbCommand(signup, con);

                    // Add parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@ID", sign_ID.Text);
                    cmd.Parameters.AddWithValue("@Username", sign_un.Text);
                    cmd.Parameters.AddWithValue("@Password", sign_pass.Text);
                    cmd.Parameters.AddWithValue("@Type", userType);

                    // Execute the query to insert data
                    cmd.ExecuteNonQuery();

                    // Show success message
                    MessageBox.Show("Your account has been successfully created!", "Registration Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear the input fields for a new registration
                    ClearInputFields();
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during the process
                    MessageBox.Show("An error occurred: " + ex.Message, "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close(); // Close the database connection
                }
            }
            else
            {
                // If the passwords do not match, prompt the user to re-enter
                MessageBox.Show("Passwords do not match, please re-enter", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                sign_pass.Text = "";
                sign_re.Text = "";
                sign_pass.Focus(); // Focus the password field for re-entry
            }
        }

        // Method to validate password format
        private bool IsPasswordValid(string password)
        {
            // Regex to ensure the password contains at least one letter, one number, and is at least 8 characters long
            return Regex.IsMatch(password, @"^(?=.*[A-Za-z])(?=.*\d).{8,}$");
        }

        // Method to clear input fields
        private void ClearInputFields()
        {
            sign_ID.Text = "";
            sign_un.Text = "";
            sign_pass.Text = "";
            sign_re.Text = "";
            sign_ID.Focus(); // Set focus back to the first field
        }
    }
}
