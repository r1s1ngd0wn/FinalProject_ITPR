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
        public Form1()
        {
            InitializeComponent();

            // Ensure container resizes and can show scrollbars instead of cropping child controls
            this.panelCenter.Dock = DockStyle.Fill;
            this.panelCenter.AutoScroll = true;
            this.panelCenter.Padding = Padding.Empty;
            this.panelCenter.Margin = Padding.Empty;
            this.panelCenter.AutoSize = false;

            // Link Navigation Buttons
            this.btnAddPhone.Click += new EventHandler(this.btnAddPhone_Click);
            this.btnCustomer.Click += new EventHandler(this.btnCustomer_Click);
            this.btnStock.Click += new EventHandler(this.btnStock_Click);
            this.btnCustomerRecords.Click += new EventHandler(this.btnCustomerRecords_Click);
            this.btnDelPhoneRec.Click += new EventHandler(this.btnDelete_Click);
            this.btnManageBrand.Click += new EventHandler(this.btnManageBrand_Click);
            this.btnExit.Click += new EventHandler(this.btnExit_Click);
            this.btnMinimize.Click += new EventHandler(this.btnMinimize_Click);

            // Optional: Load default screen (e.g., Stock) on startup
            LoadSubForm(new UC_Stock());
        }

        // --- Helper to switch screens ---
        private void LoadSubForm(UserControl uc)
        {
            panelCenter.SuspendLayout();
            this.panelCenter.Controls.Clear();

            // Remove extra spacing so UC can fill exactly
            uc.Margin = Padding.Empty;
            uc.Padding = Padding.Empty;
            uc.AutoSize = false;               // prevent UC from resizing itself beyond the panel
            uc.Dock = DockStyle.Fill;          // stretch to panel size
            uc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            uc.AutoScroll = true;              // allow UC to scroll internally if needed

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
            // This is the Sales / POS Screen
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
            // Mapped to 'button1' in designer
            LoadSubForm(new UC_DeletePhone());
        }

        private void btnDelPhoneRec_Click(object sender, EventArgs e)
        {
            LoadSubForm(new UC_DeletePhone());
        }

        private void btnManageBrand_Click(object sender, EventArgs e)
        {
            LoadSubForm(new UC_ManageBrand());
        }

        // --- Window Controls ---

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }


        // If you add a "Manage Brand" button later, simply add:
        // private void btnBrand_Click(object sender, EventArgs e) { LoadSubForm(new UC_ManageBrand()); }
    }
}