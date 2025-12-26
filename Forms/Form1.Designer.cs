namespace DACK_ITPROJECT
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelMenu = new System.Windows.Forms.Panel();
            this.panelCenter = new System.Windows.Forms.Panel();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panelPassword = new System.Windows.Forms.Panel();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.linkVerify = new System.Windows.Forms.LinkLabel();
            this.linkCancel = new System.Windows.Forms.LinkLabel();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCustomerRecords = new System.Windows.Forms.Button();
            this.btnStock = new System.Windows.Forms.Button();
            this.btnCustomer = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnAddPhone = new System.Windows.Forms.Button();
            this.panelMenu.SuspendLayout();
            this.panelCenter.SuspendLayout();
            this.panelPassword.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMenu
            // 
            this.panelMenu.BackColor = System.Drawing.Color.CadetBlue;
            this.panelMenu.Controls.Add(this.panelPassword);
            this.panelMenu.Controls.Add(this.button1);
            this.panelMenu.Controls.Add(this.btnCustomerRecords);
            this.panelMenu.Controls.Add(this.btnStock);
            this.panelMenu.Controls.Add(this.btnCustomer);
            this.panelMenu.Controls.Add(this.btnExit);
            this.panelMenu.Controls.Add(this.btnAddPhone);
            this.panelMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenu.ForeColor = System.Drawing.Color.White;
            this.panelMenu.Location = new System.Drawing.Point(0, 0);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(250, 718);
            this.panelMenu.TabIndex = 0;
            // 
            // panelCenter
            // 
            this.panelCenter.BackColor = System.Drawing.Color.Azure;
            this.panelCenter.Controls.Add(this.btnMinimize);
            this.panelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCenter.Location = new System.Drawing.Point(250, 0);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(1073, 718);
            this.panelCenter.TabIndex = 1;
            // 
            // panelPassword
            // 
            this.panelPassword.Controls.Add(this.linkCancel);
            this.panelPassword.Controls.Add(this.linkVerify);
            this.panelPassword.Controls.Add(this.txtPassword);
            this.panelPassword.Location = new System.Drawing.Point(26, 402);
            this.panelPassword.Name = "panelPassword";
            this.panelPassword.Size = new System.Drawing.Size(200, 100);
            this.panelPassword.TabIndex = 7;
            this.panelPassword.Visible = false;
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.CadetBlue;
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Location = new System.Drawing.Point(3, 15);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(194, 16);
            this.txtPassword.TabIndex = 0;
            // 
            // linkVerify
            // 
            this.linkVerify.AutoSize = true;
            this.linkVerify.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkVerify.LinkColor = System.Drawing.Color.White;
            this.linkVerify.Location = new System.Drawing.Point(18, 65);
            this.linkVerify.Name = "linkVerify";
            this.linkVerify.Size = new System.Drawing.Size(52, 19);
            this.linkVerify.TabIndex = 1;
            this.linkVerify.TabStop = true;
            this.linkVerify.Text = "Verify";
            // 
            // linkCancel
            // 
            this.linkCancel.AutoSize = true;
            this.linkCancel.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkCancel.LinkColor = System.Drawing.Color.White;
            this.linkCancel.Location = new System.Drawing.Point(116, 65);
            this.linkCancel.Name = "linkCancel";
            this.linkCancel.Size = new System.Drawing.Size(66, 19);
            this.linkCancel.TabIndex = 1;
            this.linkCancel.TabStop = true;
            this.linkCancel.Text = "Cancel";
            // 
            // btnMinimize
            // 
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.ForeColor = System.Drawing.Color.Transparent;
            this.btnMinimize.Image = global::DACK_ITPROJECT.Properties.Resources.minimize;
            this.btnMinimize.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMinimize.Location = new System.Drawing.Point(751, 0);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(45, 42);
            this.btnMinimize.TabIndex = 2;
            this.btnMinimize.UseVisualStyleBackColor = true;
            this.btnMinimize.Click += new System.EventHandler(this.btnMinimize_Click);
            // 
            // button1
            // 
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Image = global::DACK_ITPROJECT.Properties.Resources.delete;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(23, 335);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(233, 61);
            this.button1.TabIndex = 6;
            this.button1.Text = "Delete Phone Record";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnCustomerRecords
            // 
            this.btnCustomerRecords.FlatAppearance.BorderSize = 0;
            this.btnCustomerRecords.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomerRecords.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomerRecords.ForeColor = System.Drawing.Color.Black;
            this.btnCustomerRecords.Image = global::DACK_ITPROJECT.Properties.Resources.search;
            this.btnCustomerRecords.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCustomerRecords.Location = new System.Drawing.Point(26, 277);
            this.btnCustomerRecords.Name = "btnCustomerRecords";
            this.btnCustomerRecords.Size = new System.Drawing.Size(221, 52);
            this.btnCustomerRecords.TabIndex = 5;
            this.btnCustomerRecords.Text = "Customer Record\'s";
            this.btnCustomerRecords.UseVisualStyleBackColor = true;
            // 
            // btnStock
            // 
            this.btnStock.FlatAppearance.BorderSize = 0;
            this.btnStock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStock.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStock.ForeColor = System.Drawing.Color.Black;
            this.btnStock.Image = global::DACK_ITPROJECT.Properties.Resources.ready_stock;
            this.btnStock.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStock.Location = new System.Drawing.Point(26, 213);
            this.btnStock.Name = "btnStock";
            this.btnStock.Size = new System.Drawing.Size(181, 49);
            this.btnStock.TabIndex = 4;
            this.btnStock.Text = "Stock";
            this.btnStock.UseVisualStyleBackColor = true;
            // 
            // btnCustomer
            // 
            this.btnCustomer.FlatAppearance.BorderSize = 0;
            this.btnCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCustomer.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCustomer.ForeColor = System.Drawing.Color.Black;
            this.btnCustomer.Image = global::DACK_ITPROJECT.Properties.Resources.customer_service;
            this.btnCustomer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCustomer.Location = new System.Drawing.Point(26, 148);
            this.btnCustomer.Name = "btnCustomer";
            this.btnCustomer.Size = new System.Drawing.Size(181, 37);
            this.btnCustomer.TabIndex = 3;
            this.btnCustomer.Text = "Customers ";
            this.btnCustomer.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Image = global::DACK_ITPROJECT.Properties.Resources.close;
            this.btnExit.Location = new System.Drawing.Point(0, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(51, 40);
            this.btnExit.TabIndex = 1;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnAddPhone
            // 
            this.btnAddPhone.FlatAppearance.BorderSize = 0;
            this.btnAddPhone.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddPhone.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddPhone.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddPhone.Image = global::DACK_ITPROJECT.Properties.Resources.add_to_cart__1_;
            this.btnAddPhone.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddPhone.Location = new System.Drawing.Point(23, 59);
            this.btnAddPhone.Name = "btnAddPhone";
            this.btnAddPhone.Size = new System.Drawing.Size(181, 59);
            this.btnAddPhone.TabIndex = 0;
            this.btnAddPhone.Text = "AddPhone";
            this.btnAddPhone.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Teal;
            this.ClientSize = new System.Drawing.Size(1323, 718);
            this.Controls.Add(this.panelCenter);
            this.Controls.Add(this.panelMenu);
            this.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.panelMenu.ResumeLayout(false);
            this.panelCenter.ResumeLayout(false);
            this.panelPassword.ResumeLayout(false);
            this.panelPassword.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMenu;
        private System.Windows.Forms.Panel panelCenter;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button btnAddPhone;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnCustomer;
        private System.Windows.Forms.Button btnStock;
        private System.Windows.Forms.Button btnCustomerRecords;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panelPassword;
        private System.Windows.Forms.LinkLabel linkCancel;
        private System.Windows.Forms.LinkLabel linkVerify;
        private System.Windows.Forms.TextBox txtPassword;
    }
}

