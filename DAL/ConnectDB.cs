// DAL/ConnectDB.cs
using System;
using System.Data;
using System.Data.SqlClient;

namespace DACK_ITPROJECT.DAL
{
    public class ConnectDB
    {
        public readonly string strCon = "Data Source= (local);Initial Catalog=PhoneStore_V6;Integrated Security=True;TrustServerCertificate=True";

        public SqlConnection conn = null;
        public SqlCommand comm = null;
        public SqlDataAdapter da = null;
        public SqlTransaction tran = null;

        public ConnectDB()
        {
            conn = new SqlConnection(strCon);
            comm = conn.CreateCommand();
        }

        public void OpenConnection()
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Lỗi khi mở kết nối cơ sở dữ liệu: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi không xác định khi mở kết nối: " + ex.Message, ex);
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Lỗi khi đóng kết nối cơ sở dữ liệu: " + ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi không xác định khi đóng kết nối: " + ex.Message, ex);
            }
        }

        public void BeginTransaction()
        {
            OpenConnection();
            tran = conn.BeginTransaction();
            comm.Transaction = tran;
        }

        public void CommitTransaction()
        {
            if (tran != null)
            {
                tran.Commit();
                tran = null;
            }
            CloseConnection();
        }

        public void RollbackTransaction()
        {
            if (tran != null)
            {
                tran.Rollback();
                tran = null;
            }
            CloseConnection();
        }

        public DataSet ExecuteQueryDataSet(string strSQL, CommandType ct, SqlParameter[] param = null)
        {
            bool wasClosed = conn.State == ConnectionState.Closed;
            if (wasClosed && tran == null) OpenConnection();

            try
            {
                comm.CommandText = strSQL;
                comm.CommandType = ct;
                comm.Connection = conn;

                // Clear previous parameters and add new ones
                comm.Parameters.Clear();
                if (param != null)
                {
                    foreach (SqlParameter p in param)
                        comm.Parameters.Add(p);
                }

                if (tran != null) comm.Transaction = tran;

                da = new SqlDataAdapter(comm);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
            catch (SqlException ex)
            {
                throw new Exception("Lỗi truy vấn dữ liệu: " + ex.Message, ex);
            }
            finally
            {
                if (wasClosed && tran == null) CloseConnection();
            }
        }

        public bool MyExecuteNonQuery(string strSQL, CommandType ct, ref string error, SqlParameter[] param = null)
        {
            bool f = false;
            bool wasClosed = conn.State == ConnectionState.Closed;
            if (wasClosed && tran == null) OpenConnection();

            try
            {
                comm.CommandText = strSQL;
                comm.CommandType = ct;
                comm.Connection = conn;

                // Clear previous parameters and add new ones
                comm.Parameters.Clear();
                if (param != null)
                {
                    foreach (SqlParameter p in param)
                        comm.Parameters.Add(p);
                }

                if (tran != null) comm.Transaction = tran;

                comm.ExecuteNonQuery();
                f = true;
            }
            catch (SqlException ex)
            {
                error = ex.Message;
            }
            finally
            {
                if (wasClosed && tran == null) CloseConnection();
            }
            return f;
        }
    }
}