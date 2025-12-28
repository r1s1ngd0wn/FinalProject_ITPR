using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using DACK_ITPROJECT.Data;

namespace DACK_ITPROJECT
{
    public partial class UC_AddNewPhone : UserControl
    {
        private readonly string connectionString = DACK_ITPROJECT.Data.DbConfig.ConnectionString;

        public UC_AddNewPhone()
        {
            InitializeComponent();
            this.Load += UC_AddNewPhone_Load;

            // --- TAB 1 EVENTS (Phone) ---
            if (btnSave != null) btnSave.Click += btnSave_Click;

            // Re-bind Undo to ensure it doesn't fire twice if added in designer
            if (btnUndo != null)
            {
                btnUndo.Click -= btnUndo_Click;
                btnUndo.Click += btnUndo_Click;
            }

            // --- TAB 2 EVENTS (Accessory) ---
            if (cboAccType != null) cboAccType.SelectedIndexChanged += CboAccType_SelectedIndexChanged;
            if (cboConnectionType != null) cboConnectionType.SelectedIndexChanged += CboConnectionType_SelectedIndexChanged;
            if (btnAddAccessory != null) btnAddAccessory.Click += BtnAddAccessory_Click;
            if (btnUndoAll != null) btnUndoAll.Click += BtnUndoAll_Click;
        }

        private void UC_AddNewPhone_Load(object sender, EventArgs e)
        {
            // 1. Rename Tabs
            if (tabControl1 != null && tabControl1.TabPages.Count >= 2)
            {
                tabControl1.TabPages[0].Text = "Add phone/tablet";
                tabControl1.TabPages[1].Text = "Add accessory";
            }

            // 2. Load Data for Tab 1
            LoadComboBoxData();
            ResetFormFields();

            // 3. Initialize Tab 2
            InitAccessoryDropdowns();
        }

        private void InitAccessoryDropdowns()
        {
            if (label30 != null) label30.Location = new Point(6, 100);            
            if (txtAccessoryName != null) txtAccessoryName.Location = new Point(182, 100); 

            if (label31 != null) label31.Location = new Point(6, 140);             
            if (txtAccessoryID != null) txtAccessoryID.Location = new Point(182, 140);    

            // Ensure they are visible
            if (label30 != null) label30.Visible = true;
            if (label31 != null) label31.Visible = true;
            if (txtAccessoryName != null) txtAccessoryName.Visible = true;
            if (txtAccessoryID != null) txtAccessoryID.Visible = true;

            // Setup Dropdowns
            cboAccType.Items.Clear();
            cboAccType.Items.AddRange(new string[] { "Charger Combo", "Power Bank", "Headphones", "Sạc + Cáp", "Sạc dự phòng", "Tai nghe" });

            cboCableLength.Items.Clear();
            cboCableLength.Items.AddRange(new string[] { "0.5m", "1m", "1.5m", "2m", "3m" });

            cboWattage.Items.Clear();
            cboWattage.Items.AddRange(new string[] { "5W", "10W", "12W", "15W", "18W", "20W", "25W", "30W", "45W", "65W", "100W", "120W" });

            // Initialize Dynamic Layout
            HideAllAccessoryControls();
            UpdateAccessoryLayout();
        }

        private void CboAccType_SelectedIndexChanged(object sender, EventArgs e)
        {
            HideAllAccessoryControls();
            string type = cboAccType.SelectedItem?.ToString() ?? "";

            // normalize to lower for checks
            string t = type.ToLowerInvariant();

            if (t.Contains("sac") || t.Contains("charger"))
            {
                // Charger Combo: Show Length, Wattage, Connection
                SetControlVisibility(label26, cboCableLength, true);      // Length
                SetControlVisibility(label27, cboWattage, true);          // Wattage
                SetControlVisibility(label28, cboConnectionType, true);   // Connection

                UpdateConnectionList(new string[] { "Type-C", "Lightning", "Micro-USB", "Type-C to Lightning" });
            }
            else if (t.Contains("power") || t.Contains("dự phòng") || t.Contains("sacduphong") || t.Contains("sạc dự phòng"))
            {
                // Power Bank: Show Connection, Capacity
                SetControlVisibility(label28, cboConnectionType, true);   // Connection
                SetControlVisibility(label29, txtCapacity, true);         // Capacity

                UpdateConnectionList(new string[] { "Type-A", "Type-C", "Wireless (MagSafe)" });
            }
            else if (t.Contains("head") || t.Contains("tai") || t.Contains("ear") || t.Contains("tai nghe"))
            {
                // Headphones: Always show Connection first
                SetControlVisibility(label28, cboConnectionType, true);

                UpdateConnectionList(new string[] { "Wired (Có dây)", "Wireless (Bluetooth)" });

                // Trigger logic to show Capacity or Length based on current selection
                CboConnectionType_SelectedIndexChanged(null, null);
            }

            UpdateAccessoryLayout(); // Auto-adjust spacing
        }

        private void CboConnectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Logic specifically for Headphones
            string accType = cboAccType.SelectedItem?.ToString() ?? "";
            if (accType.ToLowerInvariant().Contains("head") || accType.ToLowerInvariant().Contains("tai"))
            {
                string conn = cboConnectionType.SelectedItem?.ToString();

                // Reset sub-fields
                SetControlVisibility(label26, cboCableLength, false);
                SetControlVisibility(label29, txtCapacity, false);

                if (!string.IsNullOrEmpty(conn))
                {
                    if (conn.ToLowerInvariant().Contains("wired") || conn.Contains("Có dây"))
                    {
                        // Wired -> Show Cable Length
                        SetControlVisibility(label26, cboCableLength, true);
                    }
                    else
                    {
                        // Wireless -> Show Capacity (Battery)
                        SetControlVisibility(label29, txtCapacity, true);
                    }
                }
                UpdateAccessoryLayout(); // Auto-adjust spacing
            }
        }

        // DYNAMIC LAYOUT ENGINE
        private void UpdateAccessoryLayout()
        {
            // 1. Start Dynamic Y at 180 to clear the moved Static Fields (which now end at ~160)
            int currentY = 180;
            int gap = 40;

            // 2. Only dynamic fields in this list (Static Name/ID are handled in Init)
            var rows = new List<(Control label, Control input)>
            {
                (label25, cboAccType),        // 1. Accessory Type
                (label28, cboConnectionType), // 2. Connection
                (label26, cboCableLength),    // 3. Length
                (label27, cboWattage),        // 4. Wattage
                (label29, txtCapacity)        // 5. Capacity
            };

            foreach (var row in rows)
            {
                if (row.input != null && row.input.Visible)
                {
                    if (row.label != null)
                    {
                        row.label.Location = new Point(row.label.Location.X, currentY + 3);
                        row.label.Visible = true;
                    }
                    row.input.Location = new Point(row.input.Location.X, currentY);
                    currentY += gap;
                }
                else
                {
                    if (row.label != null) row.label.Visible = false;
                }
            }

            // 3. Move Buttons to the bottom
            int buttonY = currentY + 20;
            if (btnAddAccessory != null) btnAddAccessory.Location = new Point(btnAddAccessory.Location.X, buttonY);
            if (btnUndoAll != null) btnUndoAll.Location = new Point(btnUndoAll.Location.X, buttonY);
        }

        private void HideAllAccessoryControls()
        {
            SetControlVisibility(label26, cboCableLength, false);
            SetControlVisibility(label27, cboWattage, false);
            SetControlVisibility(label28, cboConnectionType, false);
            SetControlVisibility(label29, txtCapacity, false);
            // Product name/id remain visible by default; do not hide them here
            if (txtAccessoryName != null) txtAccessoryName.Visible = true;
            if (txtAccessoryID != null) txtAccessoryID.Visible = true;
            if (label30 != null) label30.Visible = true;
            if (label31 != null) label31.Visible = true;
        }

        private void SetControlVisibility(Label lbl, Control ctrl, bool visible)
        {
            if (lbl != null) lbl.Visible = visible;
            if (ctrl != null) ctrl.Visible = visible;
        }

        private void UpdateConnectionList(string[] items)
        {
            cboConnectionType.Items.Clear();
            cboConnectionType.Items.AddRange(items);
            // Don't auto-select index 0 to force user to choose
            cboConnectionType.SelectedIndex = -1;
            cboConnectionType.Text = "";
        }

        private void BtnAddAccessory_Click(object sender, EventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrWhiteSpace(txtAccessoryID.Text) || string.IsNullOrWhiteSpace(txtAccessoryName.Text))
            {
                MessageBox.Show("Accessory Product ID and Name are required.");
                return;
            }
            if (cboAccType.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an Accessory Type.");
                return;
            }

            // Map UI selection to DB LoaiPhuKien values: 'Sac', 'SacDuPhong', 'TaiNghe'
            string sel = cboAccType.SelectedItem?.ToString() ?? "";
            string selLow = sel.ToLowerInvariant();
            string dbLoai;
            if (selLow.Contains("power") || selLow.Contains("dự") || selLow.Contains("duphong") || selLow.Contains("sacduphong") || selLow.Contains("sạc dự phòng"))
                dbLoai = "SacDuPhong";
            else if (selLow.Contains("sac") || selLow.Contains("charger"))
                dbLoai = "Sac";
            else
                dbLoai = "TaiNghe";

            // Collect accessory specs (use null or empty if hidden)
            string chieuDai = cboCableLength.Visible ? cboCableLength.Text : null;
            string congSuat = cboWattage.Visible ? cboWattage.Text : null;
            string dungLuong = txtCapacity.Visible ? txtCapacity.Text : null;
            string kieuKetNoi = cboConnectionType.Visible ? cboConnectionType.Text : null;

            // Stock and prices
            int soLuong = 0;
            int.TryParse(txtStockAccessory.Text, out soLuong);

            decimal giaBan = 0;
            decimal.TryParse(txtSellingPriceAccesscory.Text, out giaBan);

            decimal giaNhap = 0;
            decimal.TryParse(txtImportPriceAccesscory.Text, out giaNhap);

            // Execute stored procedure sp_ThemPhuKien
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ThemPhuKien", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaSP", txtAccessoryID.Text.Trim());
                        cmd.Parameters.AddWithValue("@TenSP", txtAccessoryName.Text.Trim());
                        cmd.Parameters.AddWithValue("@MaThuongHieu", DBNull.Value);
                        cmd.Parameters.AddWithValue("@LoaiPhuKien", dbLoai);
                        cmd.Parameters.AddWithValue("@ChieuDaiDay", (object)chieuDai ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@CongSuat", (object)congSuat ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DungLuong", (object)dungLuong ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@KieuKetNoi", (object)kieuKetNoi ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SoLuong", soLuong);
                        cmd.Parameters.AddWithValue("@GiaBan", giaBan);
                        cmd.Parameters.AddWithValue("@GiaNhap", giaNhap);
                        //cmd.Parameters.AddWithValue("@HinhAnh", DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Accessory saved successfully.");
                BtnUndoAll_Click(null, null); // Reset fields
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving accessory: " + ex.Message);
            }
        }

        private void BtnUndoAll_Click(object sender, EventArgs e)
        {
            // Reset accessory inputs including Product Name / ID
            txtAccessoryName.Text = "";
            txtAccessoryID.Text = "";
            cboAccType.SelectedIndex = -1;
            cboConnectionType.SelectedIndex = -1;
            cboCableLength.SelectedIndex = -1;
            cboWattage.SelectedIndex = -1;
            txtCapacity.Text = "";
            HideAllAccessoryControls();
            UpdateAccessoryLayout();
        }

        private void LoadComboBoxData()
        {
            try
            {
                cboDeviceType.Items.Clear(); cboDeviceType.Items.AddRange(new string[] { "DienThoai", "MayTinhBang" }); cboDeviceType.SelectedIndex = 0;
                cboRam.Items.Clear(); cboRam.Items.AddRange(new string[] { "4GB", "6GB", "8GB", "12GB", "16GB", "24GB" });
                cboInternalStorage.Items.Clear(); cboInternalStorage.Items.AddRange(new string[] { "64GB", "128GB", "256GB", "512GB", "1TB" });
                cboDisplayType.Items.Clear(); cboDisplayType.Items.AddRange(new string[] { "IPS LCD", "OLED", "AMOLED", "Dynamic AMOLED", "Super Retina XDR" }); cboDisplayType.SelectedIndex = 0;
                cboSimSlot.Items.Clear(); cboSimSlot.Items.AddRange(new string[] { "0", "1", "2" }); cboSimSlot.SelectedIndex = 2;
                cboOS.Items.Clear(); cboOS.Items.AddRange(new string[] { "Android", "iOS", "iPadOS" });

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter("SELECT MaThuongHieu, TenThuongHieu FROM THUONG_HIEU", conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cboBrand.DataSource = dt;
                    cboBrand.DisplayMember = "TenThuongHieu";
                    cboBrand.ValueMember = "MaThuongHieu";
                    cboBrand.SelectedIndex = -1;
                }
            }
            catch (Exception ex) { MessageBox.Show("Error loading dropdowns: " + ex.Message); }
        }

        private void ResetFormFields()
        {
            txtProductID.Text = ""; txtModelName.Text = "";
            if (cboBrand.DataSource != null) cboBrand.SelectedIndex = -1;
            cboDeviceType.SelectedIndex = 0; cboRam.SelectedIndex = -1;
            cboInternalStorage.SelectedIndex = -1; cboOS.SelectedIndex = -1;
            txtScreenSize.Text = ""; txtResolution.Text = ""; txtRearCamera.Text = ""; txtFrontCamera.Text = "";
            txtSoC.Text = ""; txtImportPrice.Text = ""; txtSellPrice.Text = ""; txtStock.Text = ""; txtColor.Text = ""; txtBattery.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtProductID.Text) || string.IsNullOrWhiteSpace(txtModelName.Text))
            {
                MessageBox.Show("Product ID and Name are required!");
                return;
            }
            if (cboBrand.SelectedValue == null)
            {
                MessageBox.Show("Please select a Brand.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ThemThietBi", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaSP", txtProductID.Text.Trim());
                        cmd.Parameters.AddWithValue("@TenSP", txtModelName.Text.Trim());
                        cmd.Parameters.AddWithValue("@MaThuongHieu", cboBrand.SelectedValue);
                        cmd.Parameters.AddWithValue("@LoaiThietBi", cboDeviceType.Text);
                        cmd.Parameters.AddWithValue("@KichThuocManHinh", txtScreenSize.Text.Trim());
                        cmd.Parameters.AddWithValue("@DoPhanGiai", txtResolution.Text.Trim());
                        cmd.Parameters.AddWithValue("@CongNgheManHinh", cboDisplayType.Text);
                        cmd.Parameters.AddWithValue("@HeDieuHanh", cboOS.Text);
                        cmd.Parameters.AddWithValue("@Chip", txtSoC.Text.Trim());
                        cmd.Parameters.AddWithValue("@RAM", cboRam.Text);
                        cmd.Parameters.AddWithValue("@ROM", cboInternalStorage.Text);
                        cmd.Parameters.AddWithValue("@Pin", txtBattery.Text.Trim());
                        cmd.Parameters.AddWithValue("@CameraSau", txtRearCamera.Text.Trim());
                        cmd.Parameters.AddWithValue("@CameraTruoc", txtFrontCamera.Text.Trim());
                        int simSlots = 0; int.TryParse(cboSimSlot.Text, out simSlots);
                        cmd.Parameters.AddWithValue("@SoKheSim", simSlots);
                        cmd.Parameters.AddWithValue("@HoTroTheNho", chkSDSupported.Checked);
                        cmd.Parameters.AddWithValue("@JackTaiNghe35", chkHDJackSupported.Checked);
                        cmd.Parameters.AddWithValue("@MauSac", txtColor.Text.Trim());
                        int stock = 0; decimal importPrice = 0, sellPrice = 0;
                        int.TryParse(txtStock.Text, out stock);
                        decimal.TryParse(txtImportPrice.Text, out importPrice);
                        decimal.TryParse(txtSellPrice.Text, out sellPrice);
                        cmd.Parameters.AddWithValue("@SoLuong", stock);
                        cmd.Parameters.AddWithValue("@GiaNhap", importPrice);
                        cmd.Parameters.AddWithValue("@GiaBan", sellPrice);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Product added successfully!");
                        ResetFormFields();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Error saving product: " + ex.Message); }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clear all input fields?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                ResetFormFields();
        }

        private void label7_Click(object sender, EventArgs e) { }
    }
}