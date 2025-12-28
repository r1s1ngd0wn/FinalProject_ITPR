using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using DACK_ITPROJECT.Data;

namespace DACK_ITPROJECT.GUI
{
    public partial class UC_Statistic : UserControl
    {
        private readonly string connectionString = DACK_ITPROJECT.Data.DbConfig.ConnectionString;

        public UC_Statistic()
        {
            InitializeComponent();
            this.Load += UC_Statistic_Load;

            // --- LINK EVENTS ---
            // Tab 1: Refresh Button
            Control[] btns = this.Controls.Find("btnRefresh", true);
            if (btns.Length > 0) btns[0].Click += btnRefresh_Click;

            // Tab 2: Time Filter ComboBox
            Control[] cboTime = this.Controls.Find("cboTimeFilter", true);
            if (cboTime.Length > 0)
                ((ComboBox)cboTime[0]).SelectedIndexChanged += cboTimeFilter_SelectedIndexChanged;

            // Tab 3: Sort Items ComboBox
            Control[] cboSort = this.Controls.Find("cboSortItems", true);
            if (cboSort.Length > 0)
                ((ComboBox)cboSort[0]).SelectedIndexChanged += cboSortItems_SelectedIndexChanged;

            // Tab Control Change (Auto-refresh tab content)
            TabControl tabParams = GetTabControl();
            if (tabParams != null) tabParams.SelectedIndexChanged += TabControl_SelectedIndexChanged;
        }

        private TabControl GetTabControl()
        {
            foreach (Control c in this.Controls)
            {
                if (c is TabControl tc) return tc;
            }
            return null;
        }

        private void UC_Statistic_Load(object sender, EventArgs e)
        {
            // 1. Setup Tab 2 Dropdown (Time)
            Control[] cboTime = this.Controls.Find("cboTimeFilter", true);
            if (cboTime.Length > 0)
            {
                ComboBox c = (ComboBox)cboTime[0];
                c.Items.Clear();
                c.Items.AddRange(new object[] { "Last 7 Days", "Last 30 Days", "This Year", "All Time" });
                c.SelectedIndex = 0; // Default: Last 7 Days
            }

            // 2. Setup Tab 3 Dropdown (Sort)
            Control[] cboSort = this.Controls.Find("cboSortItems", true);
            if (cboSort.Length > 0)
            {
                ComboBox c = (ComboBox)cboSort[0];
                c.Items.Clear();
                c.Items.AddRange(new object[] { "Most Sold (Quantity)", "Highest Revenue" });
                c.SelectedIndex = 0; // Default: Quantity
            }

            LoadOverview(); // Load Tab 1 by default
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tc = (TabControl)sender;
            if (tc == null) return;

            if (tc.SelectedIndex == 0) LoadOverview();
            if (tc.SelectedIndex == 1) TriggerTimeLoad();
            if (tc.SelectedIndex == 2) TriggerCategoryLoad();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadOverview();
        }

        // =========================================================
        // TAB 1: OVERVIEW (fixed to populate overview grid with all sales)
        // =========================================================
        private void LoadOverview()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // 1. Total Revenue
                    SqlCommand cmdRev = new SqlCommand("SELECT ISNULL(SUM(TongTien), 0) FROM HOA_DON", conn);
                    decimal revenue = Convert.ToDecimal(cmdRev.ExecuteScalar());

                    // 2. Total Orders
                    SqlCommand cmdCount = new SqlCommand("SELECT COUNT(*) FROM HOA_DON", conn);
                    int orders = Convert.ToInt32(cmdCount.ExecuteScalar());

                    SetLabelText("lblTotalRevenue", revenue.ToString("N0") + " VND");
                    SetLabelText("lblTotalOrders", orders.ToString() + " Orders");

                    // 3. Populate overview grid with all sales (most recent first)
                    string q = @"
                        SELECT MaHoaDon AS [Invoice ID],
                               NgayLap AS [Date],
                               TongTien AS [Total],
                               MaNhanVien AS [Staff]
                        FROM HOA_DON
                        ORDER BY NgayLap DESC, MaHoaDon DESC";

                    SqlDataAdapter da = new SqlDataAdapter(q, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    BindGrid("dgvOverview", dt);
                }
            }
            catch (Exception ex) { MessageBox.Show("Error loading overview: " + ex.Message); }
        }

        // =========================================================
        // TAB 2: TIME STATISTICS (ComboBox)
        // =========================================================
        private void cboTimeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            TriggerTimeLoad();
        }

        private void TriggerTimeLoad()
        {
            Control[] cbos = this.Controls.Find("cboTimeFilter", true);
            if (cbos.Length == 0) return;

            string selection = ((ComboBox)cbos[0]).Text;
            int days = 7;

            if (selection == "Last 30 Days") days = 30;
            else if (selection == "This Year") days = 365;
            else if (selection == "All Time") days = 36500; // 100 Years

            LoadTimeStats(days);
        }

        private void LoadTimeStats(int days)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            MaHoaDon AS [Invoice ID], 
                            NgayLap AS [Date], 
                            TongTien AS [Total], 
                            MaNhanVien AS [Staff]
                        FROM HOA_DON 
                        WHERE NgayLap >= DATEADD(day, -@Days, GETDATE())
                        ORDER BY NgayLap DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Days", days);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    BindGrid("dataGridView1", dt);
                }
            }
            catch (Exception ex) { MessageBox.Show("Error loading time stats: " + ex.Message); }
        }

        // =========================================================
        // TAB 3: CATEGORY & ITEMS (ComboBox Sort)
        // =========================================================
        private void cboSortItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            TriggerCategoryLoad();
        }

        private void TriggerCategoryLoad()
        {
            Control[] cbos = this.Controls.Find("cboSortItems", true);
            string sortOrder = "SUM(ct.SoLuong) DESC"; // Default: Quantity

            if (cbos.Length > 0 && ((ComboBox)cbos[0]).Text == "Highest Revenue")
            {
                sortOrder = "SUM(ct.DonGia * ct.SoLuong) DESC";
            }

            LoadCategoryStats(sortOrder);
        }

        private void LoadCategoryStats(string orderByClause)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = $@"
                        SELECT 
                            t.LoaiThietBi AS [Category], 
                            SUM(ct.SoLuong) AS [Total Sold], 
                            SUM(ct.DonGia * ct.SoLuong) AS [Total Revenue]
                        FROM CHI_TIET_HOA_DON ct
                        JOIN THIET_BI t ON ct.MaSanPham = t.MaSanPham 
                        GROUP BY t.LoaiThietBi
                        ORDER BY {orderByClause}";

                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    BindGrid("dgvCategoryStats", dt);
                }
            }
            catch (Exception ex) { MessageBox.Show("Error loading categories: " + ex.Message); }
        }

        // =========================================================
        // HELPERS
        // =========================================================
        private void SetLabelText(string name, string text)
        {
            Control[] lbls = this.Controls.Find(name, true);
            if (lbls.Length > 0) lbls[0].Text = text;
        }

        private void BindGrid(string gridName, DataTable dt)
        {
            // Allow both exact names and designer names: prefer exact match
            Control[] grids = this.Controls.Find(gridName, true);

            // Fallback mapping if designer names don't match code names
            if (grids.Length == 0)
            {
                if (gridName == "dgvTimeSales")
                    grids = this.Controls.Find("dataGridView1", true); // Check if Tab 2 grid is dataGridView1
                else if (gridName == "dgvCategoryStats")
                    grids = this.Controls.Find("dgvCategoryStats", true);
                else if (gridName == "dgvOverview")
                    grids = this.Controls.Find("dgvOverview", true);
            }

            if (grids.Length > 0)
            {
                DataGridView dgv = (DataGridView)grids[0];
                dgv.DataSource = dt;
                dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Format Currency Columns if they exist
                if (dgv.Columns["Total"] != null) dgv.Columns["Total"].DefaultCellStyle.Format = "N0";
                if (dgv.Columns["Total Revenue"] != null) dgv.Columns["Total Revenue"].DefaultCellStyle.Format = "N0";
                // Format Date column if present
                if (dgv.Columns["Date"] != null) dgv.Columns["Date"].DefaultCellStyle.Format = "g";
            }
        }
    }
}