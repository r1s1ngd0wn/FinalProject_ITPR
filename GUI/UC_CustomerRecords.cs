using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using DACK_ITPROJECT.Data;

namespace DACK_ITPROJECT
{
    public partial class UC_CustomerRecords : UserControl
    {
        private readonly string connectionString = DACK_ITPROJECT.Data.DbConfig.ConnectionString;

        public UC_CustomerRecords()
        {
            InitializeComponent();
            this.Load += UC_CustomerRecords_Load;

            if (this.Controls.Find("btnSearch", true).Length > 0)
                this.Controls.Find("btnSearch", true)[0].Click += btnSearch_Click;
        }

        private void UC_CustomerRecords_Load(object sender, EventArgs e)
        {
            // Fill Combo Box if empty
            Control[] combos = this.Controls.Find("cboSearchBy", true);
            if (combos.Length > 0)
            {
                ComboBox cbo = (ComboBox)combos[0];
                if (cbo.Items.Count == 0)
                {
                    cbo.Items.Add("Customer Name");
                    cbo.Items.Add("Phone Number");
                    cbo.SelectedIndex = 0;
                }
            }
            LoadRecords();
        }

        private void LoadRecords(string searchType = "All", string keyword = "")
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            hd.MaHoaDon AS [Invoice ID],
                            kh.TenKhachHang AS [Customer],
                            kh.SoDienThoai AS [Phone],
                            hd.NgayLap AS [Date],
                            hd.TongTien AS [Total],
                            nv.HoTen AS [Staff]
                        FROM HOA_DON hd
                        LEFT JOIN KHACH_HANG kh ON hd.SoDienThoaiKH = kh.SoDienThoai
                        LEFT JOIN NHAN_VIEN nv ON hd.MaNhanVien = nv.MaNhanVien
                        WHERE 1=1";

                    if (searchType == "Customer" && !string.IsNullOrWhiteSpace(keyword))
                        query += " AND kh.TenKhachHang LIKE @Keyword";
                    else if (searchType == "Phone" && !string.IsNullOrWhiteSpace(keyword))
                        query += " AND kh.SoDienThoai LIKE @Keyword";

                    query += " ORDER BY hd.NgayLap DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    if (!string.IsNullOrWhiteSpace(keyword))
                        cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    Control[] grids = this.Controls.Find("dgvCustomerRecords", true);
                    if (grids.Length > 0)
                    {
                        DataGridView dgv = (DataGridView)grids[0];
                        dgv.DataSource = dt;
                        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                        if (dgv.Columns["Total"] != null)
                            dgv.Columns["Total"].DefaultCellStyle.Format = "N0";
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error loading records: " + ex.Message); }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string type = "All";
            string keyword = "";

            Control[] combos = this.Controls.Find("cboSearchBy", true);
            if (combos.Length > 0)
            {
                ComboBox cbo = (ComboBox)combos[0];
                if (cbo.Text == "Customer Name") type = "Customer";
                if (cbo.Text == "Phone Number") type = "Phone";
            }

            Control[] txts = this.Controls.Find("txtSearch", true);
            if (txts.Length > 0) keyword = txts[0].Text.Trim();

            LoadRecords(type, keyword);
        }
    }
}