using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using DACK_ITPROJECT.Data;
namespace DACK_ITPROJECT.GUI
{
    public partial class UC_ManageEmployee : UserControl
    {
        private readonly string connectionString = DACK_ITPROJECT.Data.DbConfig.ConnectionString;

        // Variable to store the ID of the employee we are currently editing
        private string selectedId = null;

        public UC_ManageEmployee()
        {
            InitializeComponent();
            this.Load += UC_ManageEmployee_Load;

            // Ensure we attach when the control gets a parent (tabpage) at runtime
            this.ParentChanged += (s, e) => AttachTabControlHandler();
            this.VisibleChanged += (s, e) =>
            {
                // When the UserControl becomes visible (tab selected), refresh grid
                if (this.Visible)
                    LoadEmployees();
            };

            // Link Events
            if (this.Controls.Find("btnToggleStatus", true).Length > 0)
                this.Controls.Find("btnToggleStatus", true)[0].Click += btnToggleStatus_Click;

            if (this.Controls.Find("btnAdd", true).Length > 0)
                this.Controls.Find("btnAdd", true)[0].Click += btnAdd_Click;

            if (this.Controls.Find("btnUpdate", true).Length > 0)
                this.Controls.Find("btnUpdate", true)[0].Click += btnUpdate_Click;

            if (this.Controls.Find("btnClear", true).Length > 0)
                this.Controls.Find("btnClear", true)[0].Click += btnClear_Click;

            if (this.Controls.Find("dgvEmployees", true).Length > 0)
                ((DataGridView)this.Controls.Find("dgvEmployees", true)[0]).CellClick += dgvEmployees_CellClick;
        }

        private void UC_ManageEmployee_Load(object sender, EventArgs e)
        {
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Load basic info.
                    string query = @"
                        SELECT MaNhanVien AS [ID], HoTen AS [Name], 
                               SoDienThoai AS [Phone], ChucVu AS [Role], 
                               TrangThai AS [Active] 
                        FROM NHAN_VIEN";
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    Control[] grids = this.Controls.Find("dgvEmployees", true);
                    if (grids.Length > 0)
                    {
                        DataGridView dgv = (DataGridView)grids[0];
                        dgv.DataSource = dt;
                        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error loading list: " + ex.Message); }
        }

        // --- 1. UPDATED AUTO-INCREMENT ID LOGIC ---
        private string GenerateNewID()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT TOP 1 MaNhanVien FROM NHAN_VIEN WHERE MaNhanVien LIKE 'NV%' ORDER BY LEN(MaNhanVien) DESC, MaNhanVien DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string lastId = result.ToString(); // e.g., "NV02"
                        string numberPart = lastId.Substring(2);
                        if (int.TryParse(numberPart, out int number))
                        {
                            // Increment: NV02 -> NV03
                            return "NV" + (number + 1).ToString("D2");
                        }
                    }

                    return "NV02";
                }
            }
            catch { return "NV02"; } // Fallback
        }

        // --- 2. ADD BUTTON (Always Adds as 'Staff') ---
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Name and Password are required.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Generate ID Automatically (Starting from NV02)
                    string newID = GenerateNewID();

                    // Hardcode 'Staff'
                    string query = @"INSERT INTO NHAN_VIEN (MaNhanVien, HoTen, SoDienThoai, MatKhau, ChucVu, TrangThai) 
                                     VALUES (@ID, @Name, @Phone, @Pass, 'Staff', 1)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", newID);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@Pass", txtPassword.Text.Trim());

                    cmd.ExecuteNonQuery();
                    MessageBox.Show($"Staff Added! Assigned ID: {newID}");

                    LoadEmployees();
                    ClearFields();
                }
            }
            catch (Exception ex) { MessageBox.Show("Error adding: " + ex.Message); }
        }

        // --- 3. GRID CLICK (Select for Edit) ---
        private void dgvEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv.Rows[e.RowIndex];

                // Store the ID in the background variable
                selectedId = row.Cells["ID"].Value.ToString();

                // Fill visible fields
                txtName.Text = row.Cells["Name"].Value.ToString();
                txtPhone.Text = row.Cells["Phone"].Value.ToString();

                txtPassword.Clear();
            }
        }

        // --- 4. UPDATE BUTTON ---
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedId))
            {
                MessageBox.Show("Please select an employee from the list first.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"UPDATE NHAN_VIEN 
                                     SET HoTen=@Name, SoDienThoai=@Phone, MatKhau=@Pass 
                                     WHERE MaNhanVien=@ID";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", selectedId);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@Pass", txtPassword.Text.Trim());

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Information Updated!");

                    LoadEmployees();
                    ClearFields();
                }
            }
            catch (Exception ex) { MessageBox.Show("Error updating: " + ex.Message); }
        }

        // --- 5. TOGGLE STATUS (Disable/Enable) ---
        private void btnToggleStatus_Click(object sender, EventArgs e)
        {
            Control[] grids = this.Controls.Find("dgvEmployees", true);
            if (grids.Length == 0) return;
            DataGridView dgv = (DataGridView)grids[0];

            if (dgv.SelectedRows.Count == 0) return;

            string id = dgv.SelectedRows[0].Cells["ID"].Value.ToString();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE NHAN_VIEN SET TrangThai = CASE WHEN TrangThai = 1 THEN 0 ELSE 1 END WHERE MaNhanVien = @ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", id);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Status Updated.");
                    LoadEmployees();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            selectedId = null;
            txtPhone.Clear();
            txtName.Clear();
            txtPassword.Clear();
        }

        // --- 6. TAB SELECTION HANDLING: auto-refresh when its tab is selected ---
        private void AttachTabControlHandler()
        {
            try
            {
                Control p = this.Parent;
                while (p != null)
                {
                    if (p is TabPage tabPage)
                    {
                        Control parentOfTab = tabPage.Parent;
                        if (parentOfTab is TabControl tabControl)
                        {
                            // detach first to avoid duplicate subscriptions
                            tabControl.SelectedIndexChanged -= TabControl_SelectedIndexChanged;
                            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
                            break;
                        }
                    }
                    p = p.Parent;
                }
            }
            catch { /* swallow any attach errors */ }
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tabControl = sender as TabControl;
            if (tabControl == null) return;

            // if our TabPage is selected, refresh the grid
            if (this.Parent is TabPage tabPage && tabControl.SelectedTab == tabPage)
            {
                // use BeginInvoke so UI is stable before loading data
                this.BeginInvoke((Action)(() => LoadEmployees()));
            }
        }
    }
}