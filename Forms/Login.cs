using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using DACK_ITPROJECT.Data; // Ensure this matches your namespace for DbHelper

namespace DACK_ITPROJECT
{
    public partial class Login : Form
    {
        // ⚠️ CONFIRM CONNECTION STRING
        private string connectionString = @"Data Source=.;Initial Catalog=PhoneStore_V6;Integrated Security=True";

        public Login()
        {
            InitializeComponent();

            // Link the Login button click event
            if (this.btnLogin != null)
            {
                this.btnLogin.Click += new EventHandler(this.btnLogin_Click);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both Username and Password.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Use the Stored Procedure 'sp_DangNhap' from your V6 database
                    using (SqlCommand cmd = new SqlCommand("sp_DangNhap", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaNV", username);
                        cmd.Parameters.AddWithValue("@MatKhau", password);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                string role = reader["ChucVu"].ToString(); // Get role (Admin/Staff)
                                string name = reader["HoTen"].ToString();

                                MessageBox.Show($"Welcome back, {name}!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Open Main Form
                                Form1 mainForm = new Form1();
                                mainForm.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Invalid Username or Password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database connection error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}