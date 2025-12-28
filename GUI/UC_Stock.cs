using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Collections.Generic;

namespace DACK_ITPROJECT
{
    public partial class UC_Stock : UserControl
    {
        // ⚠️ CONFIRM CONNECTION STRING
        private string connectionString = @"Data Source=.;Initial Catalog=PhoneStore_V6;Integrated Security=True";

        public UC_Stock()
        {
            InitializeComponent();
            this.Load += UC_Stock_Load;

            // Link Grid Click Event manually if not done in designer
            if (this.Controls.Find("dgvStock", true).Length > 0)
                ((DataGridView)this.Controls.Find("dgvStock", true)[0]).CellClick += dgvStock_CellClick;
        }

        private void UC_Stock_Load(object sender, EventArgs e)
        {
            LoadStock();
            ResetLabels();
        }

        private void LoadStock()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Load only the basic specs requested for the grid
                    string query = @"
                        SELECT 
                            t.MaSanPham, t.TenSanPham, th.TenThuongHieu, 
                            t.RAM, t.ROM, t.KichThuocManHinh, t.GiaNhap, t.GiaBan
                        FROM THIET_BI t
                        LEFT JOIN THUONG_HIEU th ON t.MaThuongHieu = th.MaThuongHieu
                        WHERE t.TrangThai = 1";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    Control[] grids = this.Controls.Find("dgvStock", true);
                    if (grids.Length > 0)
                    {
                        DataGridView dgv = (DataGridView)grids[0];
                        dgv.DataSource = dt;
                        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                        // Rename columns to user-friendly headers
                        if (dgv.Columns["MaSanPham"] != null) dgv.Columns["MaSanPham"].HeaderText = "Product ID";
                        if (dgv.Columns["TenSanPham"] != null) dgv.Columns["TenSanPham"].HeaderText = "Phone Name";
                        if (dgv.Columns["TenThuongHieu"] != null) dgv.Columns["TenThuongHieu"].HeaderText = "Brand";
                        if (dgv.Columns["RAM"] != null) dgv.Columns["RAM"].HeaderText = "RAM";
                        if (dgv.Columns["ROM"] != null) dgv.Columns["ROM"].HeaderText = "ROM";
                        if (dgv.Columns["KichThuocManHinh"] != null) dgv.Columns["KichThuocManHinh"].HeaderText = "Screen Size";
                        if (dgv.Columns["GiaNhap"] != null)
                        {
                            dgv.Columns["GiaNhap"].HeaderText = "Cost Price";
                            dgv.Columns["GiaNhap"].DefaultCellStyle.Format = "N0";
                        }
                        if (dgv.Columns["GiaBan"] != null)
                        {
                            dgv.Columns["GiaBan"].HeaderText = "Price";
                            dgv.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error loading stock: " + ex.Message); }
        }

        private void dgvStock_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv.Rows[e.RowIndex];

                // Show quick items available in grid (safe: check for null)
                SetLabelText("lblCompany", row.Cells["TenThuongHieu"]?.Value?.ToString());
                SetLabelText("lblModel", row.Cells["TenSanPham"]?.Value?.ToString());
                SetLabelText("lblRam", row.Cells["RAM"]?.Value?.ToString());
                SetLabelText("lblMemory", row.Cells["ROM"]?.Value?.ToString());

                // If MaSanPham is present, load full specs from DB (safe and reliable)
                var idObj = row.Cells["MaSanPham"]?.Value;
                if (idObj != null)
                {
                    LoadSpecsFromDatabase(idObj.ToString());
                }
                else
                {
                    // Fallback: set remaining labels using available columns (guarded)
                    bool sdSupport = false;
                    if (dgv.Columns.Contains("HoTroTheNho") && row.Cells["HoTroTheNho"].Value != DBNull.Value)
                        sdSupport = Convert.ToBoolean(row.Cells["HoTroTheNho"].Value);
                    SetLabelText("lblExpandable", sdSupport ? "Yes" : "No");

                    string size = row.Cells["KichThuocManHinh"]?.Value?.ToString();
                    string tech = dgv.Columns.Contains("CongNgheManHinh") ? row.Cells["CongNgheManHinh"]?.Value?.ToString() : null;
                    SetLabelText("lblDisplay", $"{size} ({tech})");

                    SetLabelText("lblRear", dgv.Columns.Contains("CameraSau") ? row.Cells["CameraSau"]?.Value?.ToString() : null);
                    SetLabelText("lblFront", dgv.Columns.Contains("CameraTruoc") ? row.Cells["CameraTruoc"]?.Value?.ToString() : null);

                    SetLabelText("lblFinger", dgv.Columns.Contains("Chip") ? row.Cells["Chip"]?.Value?.ToString() : null);
                    SetLabelText("lblSim", dgv.Columns.Contains("SoKheSim") ? row.Cells["SoKheSim"]?.Value?.ToString() : null);
                    SetLabelText("lblNetwork", dgv.Columns.Contains("Pin") ? row.Cells["Pin"]?.Value?.ToString() : null);

                    if (dgv.Columns.Contains("GiaBan") && decimal.TryParse(row.Cells["GiaBan"]?.Value?.ToString(), out decimal price))
                        SetLabelText("lblPrice", price.ToString("N0") + " VND");
                }
            }
        }

        private void LoadSpecsFromDatabase(string maSanPham)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = @"
                        SELECT 
                            t.TenSanPham, th.TenThuongHieu, t.RAM, t.ROM, t.KichThuocManHinh, t.CongNgheManHinh,
                            t.CameraSau, t.CameraTruoc, t.HoTroTheNho, t.SoKheSim, t.Pin, t.Chip, t.GiaBan
                        FROM THIET_BI t
                        LEFT JOIN THUONG_HIEU th ON t.MaThuongHieu = th.MaThuongHieu
                        WHERE t.MaSanPham = @id
                    ";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", maSanPham);
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                Func<string, string> read = col =>
                                {
                                    int idx = -1;
                                    try { idx = rdr.GetOrdinal(col); }
                                    catch { return "----"; }
                                    return rdr.IsDBNull(idx) ? "----" : rdr.GetValue(idx).ToString();
                                };

                                SetLabelText("lblCompany", read("TenThuongHieu"));
                                SetLabelText("lblModel", read("TenSanPham"));
                                SetLabelText("lblRam", read("RAM"));
                                SetLabelText("lblMemory", read("ROM"));

                                bool sdSupport = false;
                                try
                                {
                                    var idx = rdr.GetOrdinal("HoTroTheNho");
                                    sdSupport = !rdr.IsDBNull(idx) && Convert.ToBoolean(rdr.GetValue(idx));
                                }
                                catch { sdSupport = false; }
                                SetLabelText("lblExpandable", sdSupport ? "Yes" : "No");

                                string size = read("KichThuocManHinh");
                                string tech = read("CongNgheManHinh");
                                SetLabelText("lblDisplay", $"{size} ({tech})");

                                SetLabelText("lblRear", read("CameraSau"));
                                SetLabelText("lblFront", read("CameraTruoc"));

                                SetLabelText("lblFinger", read("Chip"));
                                SetLabelText("lblSim", read("SoKheSim"));
                                SetLabelText("lblNetwork", read("Pin"));

                                if (decimal.TryParse(read("GiaBan"), out decimal price))
                                    SetLabelText("lblPrice", price.ToString("N0") + " VND");
                                else
                                    SetLabelText("lblPrice", read("GiaBan"));
                            }
                            else
                            {
                                ResetLabels();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load product specs: " + ex.Message);
            }
        }

        // Replace SetLabelText with a more robust version that tolerates different designer names
        private void SetLabelText(string labelName, string text)
        {
            // Try exact name first (search children)
            Control[] lbls = this.Controls.Find(labelName, true);
            if (lbls.Length > 0)
            {
                lbls[0].Text = text ?? "----";
                return;
            }

            // Fallback: search by keyword (strip "lbl" prefix) and match control names containing the keyword
            string keyword = labelName;
            if (labelName.StartsWith("lbl", StringComparison.OrdinalIgnoreCase) && labelName.Length > 3)
                keyword = labelName.Substring(3);

            keyword = keyword.ToLowerInvariant();

            Control found = FindControlByNameContains(this, keyword);
            if (found != null && found is Label)
            {
                found.Text = text ?? "----";
                return;
            }

            // Last resort: try to set any label whose Location.X is small (left column) and is empty/"----"
            foreach (Control c in GetAllControls(this))
            {
                if (c is Label lbl && (lbl.Text == "----" || string.IsNullOrWhiteSpace(lbl.Text)))
                {
                    lbl.Text = text ?? "----";
                    return;
                }
            }
        }

        // Recursive helper: find control whose name contains keyword (case-insensitive)
        private Control FindControlByNameContains(Control parent, string keyword)
        {
            foreach (Control c in parent.Controls)
            {
                if (!string.IsNullOrEmpty(c.Name) && c.Name.ToLowerInvariant().Contains(keyword))
                    return c;
                var sub = FindControlByNameContains(c, keyword);
                if (sub != null) return sub;
            }
            return null;
        }

        // Returns all nested controls
        private IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                yield return c;
                foreach (var sub in GetAllControls(c))
                    yield return sub;
            }
        }

        private void ResetLabels()
        {
            // Reset all labels to "----"
            string[] labels = { "lblCompany", "lblModel", "lblRam", "lblMemory", "lblExpandable",
                                "lblDisplay", "lblRear", "lblFront", "lblFinger", "lblSim", "lblNetwork", "lblPrice" };
            foreach (var l in labels) SetLabelText(l, "----");
        }
    }
}