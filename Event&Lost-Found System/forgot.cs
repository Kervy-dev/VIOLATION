using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace Event_Lost_Found_System
{
    public partial class forgot : Form
    {
        private string generatedOTP;
        private string userEmailOrPhone;
        private string username; // New variable for storing the username

        public forgot()
        {
            InitializeComponent();
        }

        // Method to generate OTP
        private string GenerateOTP()
        {
            Random rand = new Random();
            generatedOTP = rand.Next(100000, 999999).ToString(); // Generate a random 6-digit OTP
            return generatedOTP;
        }

        // Method to send OTP via email
        private void SendOTPEmail(string email)
        {
            try
            {
                // Generate the OTP
                string otp = GenerateOTP();

                // Setup the email
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("petwussy533@gmail.com"); // Your Gmail address
                mail.To.Add(email);
                mail.Subject = "Your OTP Code";
                mail.Body = $"Your OTP code is {otp}";

                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com"); // Correct SMTP server for Gmail
                smtpServer.Port = 587; // Use port 587 for TLS
                smtpServer.Credentials = new NetworkCredential("acadease49@gmail.com", "uiys tuip qbtt yhyh"); // Use your App Password here
                smtpServer.EnableSsl = true;

                // Send the email
                smtpServer.Send(mail);
                MessageBox.Show("OTP has been sent to your email.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send OTP: {ex.Message}");
            }
        }

        // Method to simulate sending OTP via phone
        private void SendOTPSMS(string phone)
        {
            // Simulate sending OTP
            string otp = GenerateOTP();
            MessageBox.Show($"OTP has been sent to your phone: {phone}. OTP: {otp}");
        }

        private void btnlog_Click(object sender, EventArgs e)
        {
            userEmailOrPhone = textBoxEmailOrPhone.Text; // Get the user's email or phone number input
            username = textBoxUsername.Text; // Get the username input

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please enter your username.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Exit if the username is not provided
            }

            if (userEmailOrPhone.Contains("@"))
            {
                // Send OTP via email
                SendOTPEmail(userEmailOrPhone);
            }
            else
            {
                // Send OTP via phone (placeholder for SMS integration)
                SendOTPSMS(userEmailOrPhone);
            }

            // Show the reset form and pass the username and generated OTP
            reset resetForm = new reset(userEmailOrPhone, generatedOTP, username); // Pass the username
            resetForm.Show();
            this.Hide();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
