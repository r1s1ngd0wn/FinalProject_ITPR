using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DACK_ITPROJECT.GUI; // Ensure this namespace exists if you moved UCs into a GUI folder

namespace DACK_ITPROJECT
{
    public partial class Form1 : Form
    {
        private string currentUserRole;

        public Form1(string role)
        {
            InitializeComponent();
            this.currentUserRole = role;

            // 1. Setup Window Layout
            this.panelCenter.Dock = DockStyle.Fill;
            this.panelCenter.AutoScroll = true;
            this.panelCenter.Padding = Padding.Empty;
            this.panelCenter.Margin = Padding.Empty;
            this.panelCenter.AutoSize = false;

            // 2. Configure Permissions based on Role
            ConfigurePermissions();

            // 3. Link Navigation Buttons
            this.btnAddPhone.Click += new EventHandler(this.btnAddPhone_Click);
            this.btnCustomer.Click += new EventHandler(this.btnCustomer_Click);
            this.btnStock.Click += new EventHandler(this.btnStock_Click);
            this.btnCustomerRecords.Click += new EventHandler(this.btnCustomerRecords_Click);

            // Delete Button (Mapped to 'btnDelPhoneRec' or 'button1' depending on designer)
            if (this.btnDelPhoneRec != null)
                this.btnDelPhoneRec.Click += new EventHandler(this.btnDelete_Click);

            this.btnManageBrand.Click += new EventHandler(this.btnManageBrand_Click);
            this.btnExit.Click += new EventHandler(this.btnExit_Click);
            this.btnMinimize.Click += new EventHandler(this.btnMinimize_Click);

            Control[] adminBtns = this.Controls.Find("btnManageEmployee", true);
            if (adminBtns.Length > 0)
            {
                adminBtns[0].Click += new EventHandler(this.btnManageEmployee_Click);
            }

            // 4. Load Default Screen
            LoadSubForm(new UC_Stock());
        }

        // --- Permission Logic ---
        private void ConfigurePermissions()
        {
            // If NOT Admin, hide sensitive buttons
            if (this.currentUserRole != "Admin")
            {
                // Hide Manage Employee Button
                Control[] adminBtns = this.Controls.Find("btnManageEmployee", true);
                if (adminBtns.Length > 0) adminBtns[0].Visible = false;

            }
            else
            {
                // If Admin, ensure buttons are visible
                Control[] adminBtns = this.Controls.Find("btnManageEmployee", true);
                if (adminBtns.Length > 0) adminBtns[0].Visible = true;
            }
        }

        // --- Helper to switch screens ---
        private void LoadSubForm(UserControl uc)
        {
            panelCenter.SuspendLayout();
            this.panelCenter.Controls.Clear();

            uc.Margin = Padding.Empty;
            uc.Padding = Padding.Empty;
            uc.AutoSize = false;
            uc.Dock = DockStyle.Fill;
            uc.AutoScroll = true;

            this.panelCenter.Controls.Add(uc);
            uc.BringToFront();
            panelCenter.ResumeLayout(false);
        }

        // --- Navigation Events ---

        private void btnAddPhone_Click(object sender, EventArgs e)
        {
            LoadSubForm(new UC_AddNewPhone());
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            LoadSubForm(new UC_Customer());
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            LoadSubForm(new UC_Stock());
        }

        private void btnCustomerRecords_Click(object sender, EventArgs e)
        {
            LoadSubForm(new UC_CustomerRecords());
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            LoadSubForm(new UC_DeletePhone());
        }

        private void btnManageBrand_Click(object sender, EventArgs e)
        {
            LoadSubForm(new UC_ManageBrand());
        }

        private void btnManageEmployee_Click(object sender, EventArgs e)
        {
            // Ensure you have created this UserControl
            LoadSubForm(new UC_ManageEmployee());
        }

        private void btnStatistic_Click(object sender, EventArgs e)
        {
            LoadSubForm(new UC_Statistic());
        }
        // --- Window Controls ---

        private void btnExit_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure you want to exit?", "Exit Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // Add this method to your Form1 class

        private void btnDelPhoneRec_Click(object sender, EventArgs e)
        {

        }

        
    }
}