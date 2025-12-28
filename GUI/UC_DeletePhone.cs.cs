using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DACK_ITPROJECT
{
    public partial class UC_DeletePhone : UserControl
    {
        private string connectionString = @"Data Source=.;Initial Catalog=PhoneStore_V6;Integrated Security=True";

        public UC_DeletePhone()
        {
            InitializeComponent();
            this.Load += UC_DeletePhone_Load;

            // Link Search Box
            if (this.Controls.Find("txtSearch", true).Length > 0)
                this.Controls.Find("txtSearch", true)[0].TextChanged += txtSearch_TextChanged;

            // Link Delete Button (Checking for 'btnAddToCart' as seen in your Designer)
            if (this.Controls.Find("btnAddToCart", true).Length > 0)
                this.Controls.Find("btnAddToCart", true)[0].Click += btnDelete_Click;
        }

        private void UC_DeletePhone_Load(object sender, EventArgs e)
        {
            LoadData("");
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            LoadData(txt.Text.Trim());
        }

        private void LoadData(string keyword)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("sp_TimKiemSanPham", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Keyword", keyword); // Ensure your SP uses @Keyword

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    Control[] grids = this.Controls.Find("dgvDeletePhone", true);
                    if (grids.Length > 0)
                    {
                        DataGridView dgv = (DataGridView)grids[0];
                        dgv.DataSource = dt;
                        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error loading data: " + ex.Message); }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Control[] grids = this.Controls.Find("dgvDeletePhone", true);
            if (grids.Length == 0) return;
            DataGridView dgv = (DataGridView)grids[0];

            if (dgv.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an item to delete.");
                return;
            }

            string name = dgv.SelectedRows[0].Cells["TenSanPham"].Value.ToString();
            string id = dgv.SelectedRows[0].Cells["MaSanPham"].Value.ToString();

            // Get Type ('ThietBi' or 'PhuKien') to call correct Delete SP
            string type = "ThietBi";
            if (dgv.SelectedRows[0].Cells["LoaiSanPham"].Value != null)
                type = dgv.SelectedRows[0].Cells["LoaiSanPham"].Value.ToString();

            if (MessageBox.Show($"Are you sure you want to delete '{name}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        SqlCommand cmd;

                        if (type == "PhuKien")
                            cmd = new SqlCommand("sp_XoaPhuKien", conn);
                        else
                            cmd = new SqlCommand("sp_XoaThietBi", conn);

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaSP", id);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Item Deleted.");

                        // Refresh List
                        Control[] txts = this.Controls.Find("txtSearch", true);
                        string kw = txts.Length > 0 ? txts[0].Text : "";
                        LoadData(kw);
                    }
                }
                catch (Exception ex) { MessageBox.Show("Error deleting item: " + ex.Message); }
            }
        }
    }
}