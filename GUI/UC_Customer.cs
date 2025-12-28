using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace DACK_ITPROJECT
{
    public partial class UC_Customer : UserControl
    {
        // ⚠️ CONNECTION STRING
        private string connectionString = @"Data Source=.;Initial Catalog=PhoneStore_V6;Integrated Security=True";

        private DataTable cartTable;
        private decimal grandTotal = 0;

        public UC_Customer()
        {
            InitializeComponent();
            InitializeCart();

            // Hook up events
            this.Load += UC_Customer_Load;

            // Search Button
            if (this.Controls.Find("btnSearchItem", true).Length > 0)
                this.Controls.Find("btnSearchItem", true)[0].Click += btnSearchItem_Click;

            // Checkout Button (btnPurchase)
            if (this.Controls.Find("btnPurchase", true).Length > 0)
                this.Controls.Find("btnPurchase", true)[0].Click += btnPurchase_Click;

            // Clear Cart Button
            if (btnClearCart != null)
                btnClearCart.Click += BtnClearCart_Click;

            // Product selection -> update spec panel
            if (this.Controls.Find("dgvProduct", true).Length > 0)
            {
                var grid = (DataGridView)this.Controls.Find("dgvProduct", true)[0];
                grid.SelectionChanged -= DgvProduct_SelectionChanged;
                grid.SelectionChanged += DgvProduct_SelectionChanged;
            }
        }

        private void UC_Customer_Load(object sender, EventArgs e)
        {
            LoadProducts("");
            CalculateTotal(); // Initialize label
        }

        // --- 1. SETUP CART GRID ---
        private void InitializeCart()
        {
            cartTable = new DataTable();
            cartTable.Columns.Add("ProductID");
            cartTable.Columns.Add("ProductName");
            cartTable.Columns.Add("Price", typeof(decimal));
            cartTable.Columns.Add("Quantity", typeof(int));
            cartTable.Columns.Add("Total", typeof(decimal));

            dgvCart.DataSource = cartTable;

            if (dgvCart.Columns["Price"] != null) dgvCart.Columns["Price"].DefaultCellStyle.Format = "N0";
            if (dgvCart.Columns["Total"] != null) dgvCart.Columns["Total"].DefaultCellStyle.Format = "N0";
            dgvCart.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // --- 2. SEARCH PRODUCTS ---
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
                    SqlCommand cmd = new SqlCommand("sp_TimKiemSanPham", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Keyword", keyword);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dgvProduct.DataSource = null;
                    dgvProduct.Columns.Clear();
                    dgvProduct.DataSource = dt;
                    dgvProduct.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                    var keep = new[] { "TenSanPham", "TenThuongHieu", "RAM", "ROM", "MauSac", "GiaBan", "MaSanPham" };
                    foreach (DataGridViewColumn col in dgvProduct.Columns.Cast<DataGridViewColumn>().ToArray())
                    {
                        if (Array.IndexOf(keep, col.Name) < 0)
                            dgvProduct.Columns.Remove(col.Name);
                    }

                    if (dgvProduct.Columns["TenSanPham"] != null) dgvProduct.Columns["TenSanPham"].HeaderText = "Phone Name";
                    if (dgvProduct.Columns["TenThuongHieu"] != null) dgvProduct.Columns["TenThuongHieu"].HeaderText = "Brand";
                    if (dgvProduct.Columns["RAM"] != null) dgvProduct.Columns["RAM"].HeaderText = "RAM";
                    if (dgvProduct.Columns["ROM"] != null) dgvProduct.Columns["ROM"].HeaderText = "ROM";
                    if (dgvProduct.Columns["MauSac"] != null) dgvProduct.Columns["MauSac"].HeaderText = "Color";
                    if (dgvProduct.Columns["GiaBan"] != null)
                    {
                        dgvProduct.Columns["GiaBan"].HeaderText = "Price";
                        dgvProduct.Columns["GiaBan"].DefaultCellStyle.Format = "N0";
                    }

                    if (!dgvProduct.Columns.Contains("colBtn"))
                    {
                        DataGridViewButtonColumn btnCol = new DataGridViewButtonColumn();
                        btnCol.Name = "colBtn";
                        btnCol.HeaderText = "Action";
                        btnCol.Text = "Add";
                        btnCol.UseColumnTextForButtonValue = true;
                        dgvProduct.Columns.Add(btnCol);
                    }

                    dgvProduct.CellContentClick -= DgvProduct_CellContentClick;
                    dgvProduct.CellContentClick += DgvProduct_CellContentClick;

                    dgvProduct.SelectionChanged -= DgvProduct_SelectionChanged;
                    dgvProduct.SelectionChanged += DgvProduct_SelectionChanged;

                    if (dgvProduct.Rows.Count > 0 && dgvProduct.CurrentRow != null)
                    {
                        var idObj = dgvProduct.CurrentRow.Cells["MaSanPham"]?.Value;
                        if (idObj != null) LoadSpecsFromDatabase(idObj.ToString());
                        else UpdateSpecPanelFromRow(dgvProduct.CurrentRow);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error loading products: " + ex.Message); }
        }

        // --- 3. ADD TO CART ---
        private void DgvProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvProduct.Columns[e.ColumnIndex].Name == "colBtn")
            {
                string id = dgvProduct.Rows[e.RowIndex].Cells["MaSanPham"].Value.ToString();
                string name = dgvProduct.Rows[e.RowIndex].Cells["TenSanPham"].Value.ToString();
                decimal price = Convert.ToDecimal(dgvProduct.Rows[e.RowIndex].Cells["GiaBan"].Value);

                AddToCart(id, name, price, 1);
            }
        }

        private void AddToCart(string id, string name, decimal price, int quantity)
        {
            foreach (DataRow row in cartTable.Rows)
            {
                if (row["ProductID"].ToString() == id)
                {
                    row["Quantity"] = (int)row["Quantity"] + quantity;
                    row["Total"] = (int)row["Quantity"] * price;
                    CalculateTotal();
                    return;
                }
            }
            cartTable.Rows.Add(id, name, price, quantity, price * quantity);
            CalculateTotal();
        }

        // --- 4. CLEAR CART LOGIC ---
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

            // Update Label16 (Total) directly
            if (label16 != null)
            {
                label16.Text = "Total : " + grandTotal.ToString("N0") + " VND";
            }
        }

        // --- 5. CHECKOUT ---
        private void btnPurchase_Click(object sender, EventArgs e)
        {
            if (cartTable.Rows.Count == 0) { MessageBox.Show("Cart is empty."); return; }
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtContact.Text))
            {
                MessageBox.Show("Customer Name and Phone are required.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    SqlCommand cmdCust = new SqlCommand("sp_ThemKhachHang", conn, transaction);
                    cmdCust.CommandType = CommandType.StoredProcedure;
                    cmdCust.Parameters.AddWithValue("@SoDienThoai", txtContact.Text);
                    cmdCust.Parameters.AddWithValue("@TenKH", txtName.Text);
                    cmdCust.Parameters.AddWithValue("@NgaySinh", DateTime.Now);
                    cmdCust.Parameters.AddWithValue("@DiaChi", txtAddress.Text);
                    cmdCust.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmdCust.ExecuteNonQuery();

                    string invoiceID = GenerateInvoiceId();
                    SqlCommand cmdHD = new SqlCommand("sp_TaoHoaDon", conn, transaction);
                    cmdHD.CommandType = CommandType.StoredProcedure;
                    cmdHD.Parameters.AddWithValue("@MaHD", invoiceID);
                    cmdHD.Parameters.AddWithValue("@MaNV", "NV01");
                    cmdHD.Parameters.AddWithValue("@SdtKH", txtContact.Text);
                    cmdHD.Parameters.AddWithValue("@TongTien", grandTotal);
                    cmdHD.Parameters.AddWithValue("@GiamGia", 0);
                    cmdHD.Parameters.AddWithValue("@ThanhToan", grandTotal);
                    cmdHD.Parameters.AddWithValue("@GhiChu", "");
                    cmdHD.ExecuteNonQuery();

                    foreach (DataRow row in cartTable.Rows)
                    {
                        SqlCommand cmdDet = new SqlCommand("sp_ThemChiTietHoaDon", conn, transaction);
                        cmdDet.CommandType = CommandType.StoredProcedure;
                        cmdDet.Parameters.AddWithValue("@MaHD", invoiceID);
                        cmdDet.Parameters.AddWithValue("@MaSP", row["ProductID"]);
                        cmdDet.Parameters.AddWithValue("@LoaiSanPham", "ThietBi");
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
            string baseId = "hdb" + DateTime.Now.ToString("yyyyMMddHHmmss");
            var rnd = new Random();
            string suffix = rnd.Next(0, 99).ToString("D2");
            return baseId + suffix;
        }

        // --- SPEC PANEL LOGIC ---
        private void DgvProduct_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProduct.CurrentRow == null) return;
            var idObj = dgvProduct.CurrentRow.Cells["MaSanPham"]?.Value;
            var id = idObj != null ? idObj.ToString() : null;

            if (!string.IsNullOrWhiteSpace(id)) LoadSpecsFromDatabase(id);
            else UpdateSpecPanelFromRow(dgvProduct.CurrentRow);
        }

        private void LoadSpecsFromDatabase(string maSanPham)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string sql = @"SELECT RAM, ROM, MauSac, CameraSau, CameraTruoc, GiaBan, Chip FROM THIET_BI WHERE MaSanPham = @id";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", maSanPham);
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                Func<string, string> read = col =>
                                {
                                    try { var idx = rdr.GetOrdinal(col); return rdr.IsDBNull(idx) ? "----" : rdr.GetValue(idx).ToString(); }
                                    catch { return "----"; }
                                };

                                lblRam.Text = read("RAM");
                                lblInternalStorage.Text = read("ROM");
                                lblExpandable.Text = read("MauSac");
                                lblRearCamera.Text = read("CameraSau");
                                lblFrontCamera.Text = read("CameraTruoc");
                                try { label17.Text = read("Chip"); } catch { }

                                if (!rdr.IsDBNull(rdr.GetOrdinal("GiaBan")))
                                {
                                    decimal price = Convert.ToDecimal(rdr["GiaBan"]);
                                    lblPrice.Text = price.ToString("N0");
                                }
                                else lblPrice.Text = "----";
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Failed to load specs: " + ex.Message); }
        }

        private void UpdateSpecPanelFromRow(DataGridViewRow row)
        {
            if (row == null) return;
            string Get(params string[] c) { foreach (var k in c) if (dgvProduct.Columns.Contains(k) && row.Cells[k].Value != null) return row.Cells[k].Value.ToString(); return "----"; }

            lblRam.Text = Get("RAM", "ram");
            lblInternalStorage.Text = Get("ROM", "rom");
            lblExpandable.Text = Get("MauSac", "Color");
            lblRearCamera.Text = Get("CameraSau", "RearCamera");
            lblFrontCamera.Text = Get("CameraTruoc", "FrontCamera");
            try { label17.Text = Get("Chip", "SoC"); } catch { }

            var pText = Get("GiaBan", "Price");
            if (decimal.TryParse(pText, out var p)) lblPrice.Text = p.ToString("N0");
            else lblPrice.Text = pText;
        }
    }
}