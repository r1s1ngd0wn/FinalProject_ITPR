using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using DACK_ITPROJECT.Data;

namespace DACK_ITPROJECT
{
    public partial class UC_Customer : UserControl
    {
        private readonly string connectionString = DACK_ITPROJECT.Data.DbConfig.ConnectionString;

        private DataTable cartTable;
        private decimal grandTotal = 0;

        public UC_Customer()
        {
            InitializeComponent();
            InitializeCart();

            this.Load += UC_Customer_Load;

            // Link Events
            if (this.Controls.Find("btnSearchItem", true).Length > 0)
                this.Controls.Find("btnSearchItem", true)[0].Click += btnSearchItem_Click;

            if (this.Controls.Find("btnPurchase", true).Length > 0)
                this.Controls.Find("btnPurchase", true)[0].Click += btnPurchase_Click;

            if (this.Controls.Find("btnClearCart", true).Length > 0)
                this.Controls.Find("btnClearCart", true)[0].Click += BtnClearCart_Click;
        }

        private void UC_Customer_Load(object sender, EventArgs e)
        {
            LoadProducts("");
            CalculateTotal();
        }

        // --- 1. SETUP CART ---
        private void InitializeCart()
        {
            cartTable = new DataTable();
            cartTable.Columns.Add("ProductID");
            cartTable.Columns.Add("ProductName");
            cartTable.Columns.Add("Price", typeof(decimal));
            cartTable.Columns.Add("Quantity", typeof(int));
            cartTable.Columns.Add("Total", typeof(decimal));
            cartTable.Columns.Add("Type"); // Stores 'ThietBi' or 'PhuKien'

            dgvCart.DataSource = cartTable;

            // Format & Hide Columns
            if (dgvCart.Columns["Price"] != null) dgvCart.Columns["Price"].DefaultCellStyle.Format = "N0";
            if (dgvCart.Columns["Total"] != null) dgvCart.Columns["Total"].DefaultCellStyle.Format = "N0";
            if (dgvCart.Columns["Type"] != null) dgvCart.Columns["Type"].Visible = false;

            dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // --- 2. SEARCH ---
        private void btnSearchItem_Click(object sender, EventArgs e)
        {
            LoadProducts(txtSearch.Text.Trim());
        }

        private void LoadProducts(string keyword)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    // Use the SP from your SQL Script
                    SqlCommand cmd = new SqlCommand("sp_TimKiemSanPham", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Keyword", keyword);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvProduct.DataSource = dt;
                    dgvProduct.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    if (dgvProduct.Columns["MaSanPham"] != null) dgvProduct.Columns["MaSanPham"].HeaderText = "ID";
                    if (dgvProduct.Columns["TenSanPham"] != null) dgvProduct.Columns["TenSanPham"].HeaderText = "Product Name";
                    if (dgvProduct.Columns["GiaBan"] != null)
                    {
                        dgvProduct.Columns["GiaBan"].HeaderText = "Price";
                        dgvProduct.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
                    }
                    if (dgvProduct.Columns["LoaiSanPham"] != null) dgvProduct.Columns["LoaiSanPham"].Visible = false; // Hide SP-level type
                    if (dgvProduct.Columns["HinhAnh"] != null) dgvProduct.Columns["HinhAnh"].Visible = false;     // Hide Image URL

                    // Add 'Add to Cart' Button
                    if (!dgvProduct.Columns.Contains("colBtn"))
                    {
                        DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn();
                        btnCol.Name = "colBtn";
                        btnCol.HeaderText = "Action";
                        btnCol.Text = "Add";
                        btnCol.UseColumnTextForButtonValue = true;
                        dgvProduct.Columns.Add(btnCol);
                    }

                    // Re-link Grid Events
                    dgvProduct.CellContentClick -= DgvProduct_CellContentClick;
                    dgvProduct.CellContentClick += DgvProduct_CellContentClick;

                    dgvProduct.SelectionChanged -= DgvProduct_SelectionChanged;
                    dgvProduct.SelectionChanged += DgvProduct_SelectionChanged;
                }
            }
            catch (Exception ex) { MessageBox.Show("Error loading items: " + ex.Message); }
        }

        // --- 3. ADD TO CART ---
        private void DgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvProduct.Columns[e.ColumnIndex].Name == "colBtn")
            {
                // Map columns from sp_TimKiemSanPham
                string id = dgvProduct.Rows[e.RowIndex].Cells["MaSanPham"].Value.ToString();
                string name = dgvProduct.Rows[e.RowIndex].Cells["TenSanPham"].Value.ToString();
                decimal price = Convert.ToDecimal(dgvProduct.Rows[e.RowIndex].Cells["GiaBan"].Value);
                // LoaiSanPham (returned by SP) is 'ThietBi' or 'PhuKien' stored in column LoaiSanPham
                string loai = dgvProduct.Rows[e.RowIndex].Cells["LoaiSanPham"].Value.ToString();

                AddToCart(id, name, price, 1, loai);
            }
        }

        private void AddToCart(string id, string name, decimal price, int quantity, string type)
        {
            foreach (DataRow row in cartTable.Rows)
            {
                if (row["ProductID"].ToString() == id && row["Type"].ToString() == type)
                {
                    row["Quantity"] = (int)row["Quantity"] + quantity;
                    row["Total"] = (int)row["Quantity"] * price;
                    CalculateTotal();
                    return;
                }
            }
            cartTable.Rows.Add(id, name, price, quantity, price * quantity, type);
            CalculateTotal();
        }

        private void BtnClearCart_Click(object sender, EventArgs e)
        {
            cartTable.Rows.Clear();
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            grandTotal = 0;
            foreach (DataRow row in cartTable.Rows)
            {
                grandTotal += Convert.ToDecimal(row["Total"]);
            }
            if (label16 != null) label16.Text = "Total : " + grandTotal.ToString("N0") + " VND";
        }

        // --- 4. CHECKOUT ---
        private void btnPurchase_Click(object sender, EventArgs e)
        {
            if (cartTable.Rows.Count == 0) { MessageBox.Show("Cart is empty."); return; }
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtContact.Text))
            {
                MessageBox.Show("Customer Name and Phone are required.");
                return;
            }

            // Retrieve the current employee ID at purchase time to ensure we have the latest session value
            string currentEmployeeId = SessionManager.CurrentLoggedInEmployeeId;
            if (string.IsNullOrWhiteSpace(currentEmployeeId))
            {
                MessageBox.Show("No logged-in employee found. Please log in before making a purchase.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    // 1. Add Customer (Ignore if exists)
                    SqlCommand cmdCust = new SqlCommand("sp_ThemKhachHang", conn, transaction);
                    cmdCust.CommandType = CommandType.StoredProcedure;
                    cmdCust.Parameters.AddWithValue("@SoDienThoai", txtContact.Text);
                    cmdCust.Parameters.AddWithValue("@TenKH", txtName.Text);
                    cmdCust.Parameters.AddWithValue("@NgaySinh", DateTime.Now);
                    cmdCust.Parameters.AddWithValue("@DiaChi", txtAddress.Text);
                    cmdCust.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmdCust.ExecuteNonQuery();

                    // 2. Create Invoice
                    string invoiceID = GenerateInvoiceId();
                    SqlCommand cmdHD = new SqlCommand("sp_TaoHoaDon", conn, transaction);
                    cmdHD.CommandType = CommandType.StoredProcedure;
                    cmdHD.Parameters.AddWithValue("@MaHD", invoiceID);
                    // Use the fresh employee id
                    cmdHD.Parameters.AddWithValue("@MaNV", currentEmployeeId);
                    cmdHD.Parameters.AddWithValue("@SdtKH", txtContact.Text);
                    cmdHD.Parameters.AddWithValue("@TongTien", grandTotal);
                    cmdHD.Parameters.AddWithValue("@GiamGia", 0);
                    cmdHD.Parameters.AddWithValue("@ThanhToan", grandTotal);
                    cmdHD.Parameters.AddWithValue("@GhiChu", "Purchase via App");
                    cmdHD.ExecuteNonQuery();

                    // 3. Add Details & Update Stock
                    foreach (DataRow row in cartTable.Rows)
                    {
                        SqlCommand cmdDet = new SqlCommand("sp_ThemChiTietHoaDon", conn, transaction);
                        cmdDet.CommandType = CommandType.StoredProcedure;
                        cmdDet.Parameters.AddWithValue("@MaHD", invoiceID);
                        cmdDet.Parameters.AddWithValue("@MaSP", row["ProductID"]);
                        cmdDet.Parameters.AddWithValue("@LoaiSanPham", row["Type"]); // 'ThietBi' or 'PhuKien'
                        cmdDet.Parameters.AddWithValue("@SoLuong", row["Quantity"]);
                        cmdDet.Parameters.AddWithValue("@DonGia", row["Price"]);
                        cmdDet.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    MessageBox.Show("Purchase Successful! Invoice: " + invoiceID);

                    cartTable.Clear();
                    CalculateTotal();
                    LoadProducts(txtSearch.Text.Trim());
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Transaction Failed: " + ex.Message);
                }
            }
        }

        private string GenerateInvoiceId()
        {
            string baseId = "HD" + DateTime.Now.ToString("yyMMddHHmm");
            var rnd = new Random();
            return baseId + rnd.Next(0, 99).ToString("D2");
        }

        // --- 5. SPEC PANEL (Dynamic)
        // When the selection changes, fetch full details from THIET_BI or PHU_KIEN
        private void DgvProduct_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProduct.CurrentRow == null) return;

            // Safe accessor for current row
            string GetGrid(string col) =>
                dgvProduct.Columns.Contains(col) && dgvProduct.CurrentRow.Cells[col].Value != null
                ? dgvProduct.CurrentRow.Cells[col].Value.ToString() : null;

            string id = GetGrid("MaSanPham");
            string loaiSanPham = GetGrid("LoaiSanPham"); // 'ThietBi' or 'PhuKien'
            decimal price = 0; decimal.TryParse(GetGrid("GiaBan") ?? "0", out price);

            // Set price always
            lblPrice.Text = price.ToString("N0");

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(loaiSanPham))
            {
                // clear
                lblRam.Text = lblInternalStorage.Text = lblExpandable.Text = "----";
                lblRam.Visible = lblInternalStorage.Visible = lblExpandable.Visible = true;
                return;
            }

            if (loaiSanPham == "ThietBi")
            {
                LoadDeviceDetails(id);
            }
            else // PhuKien
            {
                LoadAccessoryDetails(id);
            }
        }

        // Fetch device (THIET_BI) details and map to labels
        private void LoadDeviceDetails(string maSP)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string q = @"SELECT RAM, ROM, Pin, KichThuocManHinh, DoPhanGiai, CongNgheManHinh, MauSac, HeDieuHanh
                                 FROM THIET_BI WHERE MaSanPham = @id";
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@id", maSP);
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            string ram = r["RAM"] as string ?? "----";
                            string rom = r["ROM"] as string ?? "----";
                            string pin = r["Pin"] as string ?? "----";
                            string screen = r["KichThuocManHinh"] as string ?? null;
                            string res = r["DoPhanGiai"] as string ?? null;

                            lblRam.Text = $"RAM: {ram}";
                            lblInternalStorage.Text = $"ROM: {rom}";
                            lblExpandable.Text = $"Battery: {pin}";
                            lblRam.Visible = lblInternalStorage.Visible = lblExpandable.Visible = true;
                        }
                        else
                        {
                            lblRam.Text = lblInternalStorage.Text = lblExpandable.Text = "----";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading device details: " + ex.Message);
            }
        }

        // Fetch accessory (PHU_KIEN) details and map based on subtype
        private void LoadAccessoryDetails(string maSP)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string q = @"SELECT LoaiPhuKien, ChieuDaiDay, CongSuat, DungLuong, KieuKetNoi
                                 FROM PHU_KIEN WHERE MaSanPham = @id";
                    SqlCommand cmd = new SqlCommand(q, conn);
                    cmd.Parameters.AddWithValue("@id", maSP);
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            string loai = (r["LoaiPhuKien"] as string ?? "").ToLowerInvariant();
                            string length = r["ChieuDaiDay"] as string ?? null;
                            string watt = r["CongSuat"] as string ?? null;
                            string cap = r["DungLuong"] as string ?? null;
                            string connType = r["KieuKetNoi"] as string ?? null;

                            // Default visibility
                            lblRam.Visible = lblInternalStorage.Visible = lblExpandable.Visible = true;

                            if (loai.Contains("sac") || loai.Contains("sacduphong") || loai.Contains("charger") || loai.Contains("sạc"))
                            {
                                // Charger or adapter: show wattage and cable length (if present)
                                lblRam.Text = !string.IsNullOrEmpty(watt) ? $"Wattage: {watt}" : "Wattage: ----";
                                lblInternalStorage.Text = !string.IsNullOrEmpty(length) ? $"Cable: {length}" : "Cable: ----";
                                lblExpandable.Text = "----";
                                lblExpandable.Visible = false;
                            }
                            else if (loai.Contains("tainghe") || loai.Contains("tai") || loai.Contains("head") || loai.Contains("ear"))
                            {
                                // Headphones: show capacity (battery) and connection type
                                lblRam.Text = !string.IsNullOrEmpty(cap) ? $"Capacity: {cap}" : "Capacity: ----";
                                lblInternalStorage.Text = !string.IsNullOrEmpty(connType) ? $"Connection: {connType}" : "Connection: ----";
                                lblExpandable.Text = "----";
                                lblExpandable.Visible = false;
                            }
                            else if (loai.Contains("sacduphong") || loai.Contains("power") || loai.Contains("pin") || loai.Contains("powerbank"))
                            {
                                // Power bank: connection type, wattage and capacity
                                lblRam.Text = !string.IsNullOrEmpty(connType) ? $"Connection: {connType}" : "Connection: ----";
                                lblInternalStorage.Text = !string.IsNullOrEmpty(watt) ? $"Wattage: {watt}" : "Wattage: ----";
                                lblExpandable.Text = !string.IsNullOrEmpty(cap) ? $"Capacity: {cap}" : "Capacity: ----";
                                lblExpandable.Visible = true;
                            }
                            else
                            {
                                // Unknown accessory: best-effort mapping
                                lblRam.Text = !string.IsNullOrEmpty(loai) ? $"Type: {loai}" : "Type: ----";
                                lblInternalStorage.Text = !string.IsNullOrEmpty(connType) ? $"Conn: {connType}" : "Conn: ----";
                                lblExpandable.Text = !string.IsNullOrEmpty(cap) ? $"Spec: {cap}" : "Spec: ----";
                                lblExpandable.Visible = !string.IsNullOrEmpty(lblExpandable.Text) && lblExpandable.Text != "----";
                            }
                        }
                        else
                        {
                            lblRam.Text = lblInternalStorage.Text = lblExpandable.Text = "----";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading accessory details: " + ex.Message);
            }
        }
    }
}