using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace Event_Lost_Found_System
{
    public partial class Verification_Code : Form
    {
        private string generatedOTP;
        private string userEmailOrPhone;
        private string username; // New variable for storing the username

        // Updated constructor to accept email/phone, OTP, and username
        public Verification_Code(string emailOrPhone, string otp, string user)
        {
            InitializeComponent();
            userEmailOrPhone = emailOrPhone;
            generatedOTP = otp;
            username = user; // Store the username
            lblVerificationMessage.Text = $"Verification code is sent to {emailOrPhone}"; // Update label to show the email or phone
            lblUsername.Text = $"Resetting password for: {username}"; // Update to show the username
        }

        // Method to generate OTP
        private string GenerateOTP()
        {
            Random rand = new Random();
            return rand.Next(100000, 999999).ToString(); // Generate a random 6-digit OTP
        }

        // Method to send OTP via email
        private void SendOTPEmail(string email)
        {
            try
            {
                // Generate the OTP
                generatedOTP = GenerateOTP();

                // Setup the email
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("acadease49@gmail.com"); // Your Gmail address
                mail.To.Add(email);
                mail.Subject = "Your OTP Code";
                mail.Body = $"Your OTP code is {generatedOTP}";

                SmtpClient smtpServer = new SmtpClient("smtp.gmail.com"); // Gmail's SMTP server
                smtpServer.Port = 587; // Use port 587 for TLS
                smtpServer.Credentials = new NetworkCredential("acadease49@gmail.com", "uiys tuip qbtt yhyh"); // Use your Gmail credentials
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

        // Method to resend OTP
        private void ResendOTP()
        {
            // Generate a new OTP
            generatedOTP = GenerateOTP();

            // Send the new OTP to the email
            SendOTPEmail(userEmailOrPhone);
        }


        // Submit button click event to verify OTP
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // Check if the entered OTP matches the generated OTP
            if (textBoxOTP.Text == generatedOTP)
            {
                MessageBox.Show("OTP verification successful!");

                // Show the respass form and pass the userEmailOrPhone
                Reset_Password_A mainForm = new Reset_Password_A(userEmailOrPhone, username); // Pass the email and username
                mainForm.Show();  // Show the respass form

                this.Hide();  // Hide the current reset form
            }
            else
            {
                MessageBox.Show("Invalid OTP. Please try again.");
            }
        }

        // Close application button
        private void btn1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Additional logic for Resend OTP button (already implemented in btnResend_Click)
        private void btnResendOTP_Click(object sender, EventArgs e)
        {
            ResendOTP();
        }
    }
}
