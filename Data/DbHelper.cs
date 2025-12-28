using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DACK_ITPROJECT.Data
{
    public class DbHelper
    {
        private readonly string _conn;
        public DbHelper()
        {
            _conn = ConfigurationManager.ConnectionStrings["PhoneStore"]?.ConnectionString;
            if (string.IsNullOrEmpty(_conn)) throw new InvalidOperationException("Connection string 'PhoneStore' not found in App.config");
        }

        public async Task<DataRow> AuthenticateAsync(string maNV, string matKhau)
        {
            using (var cn = new SqlConnection(_conn))
            using (var cmd = new SqlCommand("sp_DangNhap", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaNV", maNV);
                cmd.Parameters.AddWithValue("@MatKhau", matKhau);
                var da = new SqlDataAdapter(cmd);
                var dt = new DataTable();
                await Task.Run(() => da.Fill(dt));
                if (dt.Rows.Count == 0) return null;
                return dt.Rows[0];
            }
        }
    }
}
