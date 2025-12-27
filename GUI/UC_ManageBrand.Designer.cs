namespace DACK_ITPROJECT.GUI
{
    partial class UC_ManageBrand
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtRemoveBrand = new System.Windows.Forms.TabPage();
            this.dgvAddBrand = new System.Windows.Forms.DataGridView();
            this.txtAddBrand = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAddBrand = new System.Windows.Forms.Button();
            this.btnRemoveBrand = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.txtRemoveBrand.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAddBrand)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.txtRemoveBrand);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(850, 600);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnAddBrand);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txtAddBrand);
            this.tabPage1.Controls.Add(this.dgvAddBrand);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(842, 574);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtRemoveBrand
            // 
            this.txtRemoveBrand.Controls.Add(this.btnRemoveBrand);
            this.txtRemoveBrand.Controls.Add(this.label2);
            this.txtRemoveBrand.Controls.Add(this.textBox2);
            this.txtRemoveBrand.Controls.Add(this.dataGridView2);
            this.txtRemoveBrand.Location = new System.Drawing.Point(4, 22);
            this.txtRemoveBrand.Name = "txtRemoveBrand";
            this.txtRemoveBrand.Padding = new System.Windows.Forms.Padding(3);
            this.txtRemoveBrand.Size = new System.Drawing.Size(842, 574);
            this.txtRemoveBrand.TabIndex = 1;
            this.txtRemoveBrand.Text = "tabPage2";
            this.txtRemoveBrand.UseVisualStyleBackColor = true;
            // 
            // dgvAddBrand
            // 
            this.dgvAddBrand.BackgroundColor = System.Drawing.Color.White;
            this.dgvAddBrand.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAddBrand.Location = new System.Drawing.Point(0, 212);
            this.dgvAddBrand.Name = "dgvAddBrand";
            this.dgvAddBrand.Size = new System.Drawing.Size(842, 356);
            this.dgvAddBrand.TabIndex = 0;
            // 
            // txtAddBrand
            // 
            this.txtAddBrand.Location = new System.Drawing.Point(526, 90);
            this.txtAddBrand.Name = "txtAddBrand";
            this.txtAddBrand.Size = new System.Drawing.Size(221, 20);
            this.txtAddBrand.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(66, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "Brand : ";
            // 
            // btnAddBrand
            // 
            this.btnAddBrand.FlatAppearance.BorderSize = 3;
            this.btnAddBrand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddBrand.ForeColor = System.Drawing.Color.Teal;
            this.btnAddBrand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddBrand.Location = new System.Drawing.Point(631, 155);
            this.btnAddBrand.Name = "btnAddBrand";
            this.btnAddBrand.Size = new System.Drawing.Size(116, 35);
            this.btnAddBrand.TabIndex = 14;
            this.btnAddBrand.Text = "Add";
            this.btnAddBrand.UseVisualStyleBackColor = true;
            // 
            // btnRemoveBrand
            // 
            this.btnRemoveBrand.FlatAppearance.BorderSize = 3;
            this.btnRemoveBrand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveBrand.ForeColor = System.Drawing.Color.Teal;
            this.btnRemoveBrand.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRemoveBrand.Location = new System.Drawing.Point(631, 152);
            this.btnRemoveBrand.Name = "btnRemoveBrand";
            this.btnRemoveBrand.Size = new System.Drawing.Size(116, 35);
            this.btnRemoveBrand.TabIndex = 18;
            this.btnRemoveBrand.Text = "Remove";
            this.btnRemoveBrand.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(66, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 17;
            this.label2.Text = "Brand : ";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(526, 87);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(221, 20);
            this.textBox2.TabIndex = 16;
            // 
            // dataGridView2
            // 
            this.dataGridView2.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(0, 209);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(842, 359);
            this.dataGridView2.TabIndex = 15;
            // 
            // UC_ManageBrand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "UC_ManageBrand";
            this.Size = new System.Drawing.Size(850, 600);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.txtRemoveBrand.ResumeLayout(false);
            this.txtRemoveBrand.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAddBrand)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dgvAddBrand;
        private System.Windows.Forms.TabPage txtRemoveBrand;
        private System.Windows.Forms.TextBox txtAddBrand;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddBrand;
        private System.Windows.Forms.Button btnRemoveBrand;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.DataGridView dataGridView2;
    }
}
