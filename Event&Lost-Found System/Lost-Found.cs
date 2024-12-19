using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Event_Lost_Found_System
{
    public partial class Lost_Found : Form
    {
        public Lost_Found()
        {
            InitializeComponent();
        }

        OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\petwu\\source\\repos\\Event&Lost-Found System\\bin\\Debug\\Monitoring.accdb");
        OleDbCommand cmd = new OleDbCommand();
        OleDbDataAdapter da = new OleDbDataAdapter();

        private void signup_create_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            this.Hide();

            if (signup_ID.Text == "" || signup_mail.Text == "" || signup_contact.Text == "" || signup_un.Text == "" || signup_pass.Text == "" || signup_re.Text == "")
            {
                MessageBox.Show("ID#, E-mail, Contact #, Username, and Password fields are empty", "Create account Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (signup_pass.Text == signup_re.Text)
            {
                try
                {
                    con.Open();
                    // Updated SQL statement with ID #, Email, and Contact fields
                    string signup = "INSERT INTO BPC ([ID #], [Username], [Password], [Email], [Contact #]) VALUES(@ID, @Username, @Password, @Email, @Contact)";
                    cmd = new OleDbCommand(signup, con);

                    // Adding parameters for each field
                    cmd.Parameters.AddWithValue("@ID", signup_ID.Text);
                    cmd.Parameters.AddWithValue("@Username", signup_un.Text);
                    cmd.Parameters.AddWithValue("@Password", signup_pass.Text);
                    cmd.Parameters.AddWithValue("@Email", signup_mail.Text);
                    cmd.Parameters.AddWithValue("@Contact", signup_contact.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Your Account has successfully been created", "Registration Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
            }
            else
            {
                MessageBox.Show("Passwords do not match, please re-enter", "Registration Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                signup_pass.Text = "";
                signup_re.Text = "";
                signup_pass.Focus();
            }
        }

        private void cb2_CheckedChanged(object sender, EventArgs e)
        {
            if (cb2.Checked)
            {
                signup_pass.PasswordChar = '\0';
                signup_re.PasswordChar = '\0';
            }
            else
            {
                signup_pass.PasswordChar = '*';
                signup_re.PasswordChar = '*';
            }
        }

        private void bntclear_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            this.Hide();
        }

        private void cb1_CheckedChanged(object sender, EventArgs e)
        {
                if (cb1.Checked)
                {
                    MessageBox.Show("Thank you for agreeing to the terms and conditions.", "Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);


                signup_create.Enabled = true;
                }
                else
                {
                    MessageBox.Show("You need to agree to the terms and conditions to proceed.", "Reminder", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                signup_create.Enabled = false;
                }

        }

        private void btn1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
