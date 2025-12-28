using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using DACK_ITPROJECT.Data;

namespace DACK_ITPROJECT.GUI
{
    public partial class UC_ManageBrand : UserControl
    {
        private readonly string connectionString = DACK_ITPROJECT.Data.DbConfig.ConnectionString;

        public UC_ManageBrand()
        {
            InitializeComponent();
            this.Load += UC_ManageBrand_Load;

            // Link Buttons
            if (this.Controls.Find("btnAddBrand", true).Length > 0)
                this.Controls.Find("btnAddBrand", true)[0].Click += btnAddBrand_Click;

            if (this.Controls.Find("btnRemoveBrand", true).Length > 0)
                this.Controls.Find("btnRemoveBrand", true)[0].Click += btnRemoveBrand_Click;

            if (this.Controls.Find("textBox2", true).Length > 0)
                this.Controls.Find("textBox2", true)[0].TextChanged += txtSearchRemove_TextChanged;
        }

        private void UC_ManageBrand_Load(object sender, EventArgs e)
        {
            RefreshAllGrids();
        }

        private void RefreshAllGrids()
        {
            // Load data for both tabs
            LoadBrandsTab1();
            LoadBrandsTab2("");
        }

        // ==========================================
        // TAB 1: ADD BRAND (dgvAddBrand)
        // ==========================================
        private void LoadBrandsTab1()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT MaThuongHieu AS [ID], TenThuongHieu AS [Name] FROM THUONG_HIEU ORDER BY MaThuongHieu DESC";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvAddBrand.DataSource = dt;
                    dgvAddBrand.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex) { MessageBox.Show("Error loading Tab 1: " + ex.Message); }
        }

        private void btnAddBrand_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddBrand.Text))
            {
                MessageBox.Show("Please enter a Brand Name.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "INSERT INTO THUONG_HIEU (TenThuongHieu) VALUES (@Name)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", txtAddBrand.Text.Trim());
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Brand Added Successfully!");
                    txtAddBrand.Clear();

                    // Refresh BOTH tabs so the new brand shows up in the Remove list too
                    RefreshAllGrids();
                }
            }
            catch (Exception ex) { MessageBox.Show("Error adding brand: " + ex.Message); }
        }

        // ==========================================
        // TAB 2: REMOVE BRAND (dataGridView2)
        // ==========================================
        private void LoadBrandsTab2(string keyword)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT MaThuongHieu AS [ID], TenThuongHieu AS [Name] FROM THUONG_HIEU WHERE TenThuongHieu LIKE @Keyword ORDER BY MaThuongHieu DESC";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dataGridView2.DataSource = dt;
                    dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            catch (Exception ex) { MessageBox.Show("Error loading Tab 2: " + ex.Message); }
        }

        private void txtSearchRemove_TextChanged(object sender, EventArgs e)
        {
            // Live Search in Tab 2
            LoadBrandsTab2(textBox2.Text.Trim());
        }

        private void btnRemoveBrand_Click(object sender, EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a brand from the list to remove.");
                return;
            }

            if (MessageBox.Show("Are you sure? This might cause errors if products are linked to this brand.", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    // Get ID from the selected row in Tab 2
                    string id = dataGridView2.SelectedRows[0].Cells[0].Value.ToString();

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "DELETE FROM THUONG_HIEU WHERE MaThuongHieu = @ID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@ID", id);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Brand Removed.");
                        RefreshAllGrids(); // Refresh both lists
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not delete. The brand might be in use by existing products.\n\nError: " + ex.Message);
                }
            }
        }
    }
}