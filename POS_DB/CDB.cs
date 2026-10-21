using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_DB
{
    internal class CDB
    {
        public SqlConnection conn;
        public void dbConnect(string h, string d)
        {
            string ConString = "Data Source=" + h +
                ";Initial Catalog=" + d +
                ";Integrated Security=True;Encrypt=False";
            conn = new SqlConnection(ConString);
            try
            {
                conn.Open();
                MessageBox.Show("Connect Success");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public void dbInsert(string table, string[] columns, string[] datas)
        {
            string column = "";
            string data = "";
            for (int i = 0; i < columns.Length; i++)
            {
                column += columns[i] + ",";
                data += "\'" + datas[i] + "\',";
                //column = full_name,email,password, 
                //data = 'apple','apple@gmail.com','Admin1111',

            }
            column = column.Substring(0, column.Length - 1);
            data = data.Substring(0, data.Length - 1);
            string qry = "Insert into " + table + "(" + column + ")" + "values (" + data + ")";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(qry, conn);
                int rowsEffected = cmd.ExecuteNonQuery();
                MessageBox.Show(rowsEffected + "row(s) Insert sucessfull");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }

        }
        public string[] searchID(string table, string column, int id)
        {
            try
            {
                conn.Open();
                string qry = "select * from " + table + " where " + column + " = " + id;
                SqlCommand cmd = new SqlCommand(qry, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    string[] row = new string[reader.FieldCount];
                    for (int i = 0; i < row.Length; i++)
                    {
                        row[i] = reader[i].ToString();
                    }
                    reader.Close();
                    return row;
                }
                reader.Close();
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        public void dbDelete(string table, string column, int id)
        {
            try
            {
                conn.Open();
                string qry = "DELETE FROM " + table + " where " + column + " = " + id;
                SqlCommand cmd = new SqlCommand(qry, conn);
                int rowsEffected = cmd.ExecuteNonQuery();
                MessageBox.Show(rowsEffected + "rows delete successfully");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }
        }
        public void dbUpdate(string table, string[] columns, string[] datas)
        {
            try
            {
                conn.Open();

                string data = "";

                for (int i = 1; i < columns.Length; i++)
                {
                    data += columns[i] + " = '" + datas[i] + "',";
                }

                data = data.Substring(0, data.Length - 1);

                string qry = "UPDATE " + table + " SET " + data + " WHERE " + columns[0] + " = " + datas[0];

                SqlCommand cmd = new SqlCommand(qry, conn);
                int rowsEffected = cmd.ExecuteNonQuery();

                MessageBox.Show(rowsEffected + " row(s) updated successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public string[] select(string table, string col, string id)
        {
            try
            {
                conn.Open();

                string qry = "SELECT " + id + "," + col + " FROM " + table;
                SqlCommand cmd = new SqlCommand(qry, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                List<string> rows = new List<string>();

                while (reader.Read())
                {
                    rows.Add(reader[id].ToString() + "-" + reader[col].ToString());
                }

                reader.Close();
                return rows.ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        public void LoginSelect(string table, string[] col, string[] data, Form f)
        {
            try
            {
                conn.Open();

                // use condition WHERE to get data of Array
                // Result: AdminName = 'value' AND Password = 'value'
                string whereClause = "";
                for (int i = 0; i < col.Length; i++)
                {
                    whereClause += col[i] + " = '" + data[i] + "'";

                    // add word  AND if data not end of Array
                    if (i < col.Length - 1)
                    {
                        whereClause += " AND ";
                    }
                }

                //  SELECT COUNT(*) for ExecuteScalar can convert to number
                string qry = "SELECT COUNT(*) FROM " + table + " WHERE " + whereClause;

                SqlCommand cmd = new SqlCommand(qry, conn);
                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                    MessageBox.Show("correct");
                    
                    StartupForm st = new StartupForm(data[0]); // data[0] is Admin name
                    st.Show();
                    f.Hide();
                }
                else
                {
                    MessageBox.Show("Name or Password is Incorrect!.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
       
        public void UpdateOrderTotal(int OrderID)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }

                string sql = "UPDATE TableOrder SET Total = " +
                             "(SELECT ISNULL(SUM(SubTotal), 0) FROM TableOrderDetail WHERE OrderID = " + OrderID + ") " +
                             "WHERE OrderID = " + OrderID;

                SqlCommand cmdUpdate = new SqlCommand(sql, conn);
                cmdUpdate.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating total: " + ex.Message);
            }
            finally
            {
                conn.Close(); 
            }
         }
      
        public string getProductPrice(string id)
        {
            string price = "0";
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                // ទាញយក UnitPrice ពី TableProduct តាម ID
                string qry = "SELECT UnitPrice FROM TableProduct WHERE ProductID = " + id;
                SqlCommand cmd = new SqlCommand(qry, conn);
                object result = cmd.ExecuteScalar(); // use Scalar because want ine value

                if (result != null)
                {
                    price = result.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching price: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return price;
        }
        //check stock
        // Method remaining Stock
        public int CheckStock(string pID)
        {
            int count = 0;
            try
            {
                // ប្រើ Method searchID ដែលអ្នកមានស្រាប់ ដើម្បីទាញទិន្នន័យពី TableStock
                // data[0] = StockID, data[1] = ProductID, data[2] = Quantity
                string[] data = searchID("TableStock", "ProductID", int.Parse(pID));

                if (data != null)
                {
                    count = int.Parse(data[2]); // ទាញយកចំនួន Quantity មកទុកក្នុង variable count
                }
            }
            catch
            {
                count = 0;
            }
            return count;
        }
        public bool IsUserExists(string username)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                // ប្រើ COUNT ដើម្បីឆែកមើលថាមានឈ្មោះ AdminName ហ្នឹងក្នុង TableAdmin ឬនៅ
                string qry = "SELECT COUNT(*) FROM TableAdmin WHERE AdminName = '" + username + "'";
                SqlCommand cmd = new SqlCommand(qry, conn);

                int count = (int)cmd.ExecuteScalar();

                if (count > 0)
                {
                    return true; // if it have that name return true
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error checking user: " + ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
            return false; 
        }
        //dok stock auto
        public void DeductStock(string pID, int qty)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                // SQL sub Quantity in TableStock តាម ProductID
                string qry = "UPDATE TableStock SET Quantity = Quantity - " + qty +
                             " WHERE ProductID = '" + pID + "'";

                SqlCommand cmd = new SqlCommand(qry, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Stock Deduction Error: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        // add stock back for delete or update
        public void AddStockBack(string pID, int qty)
        {
            try
            {
                if (conn.State == ConnectionState.Closed) conn.Open();

                // SQL sum Quantity in TableStock 
                string qry = "UPDATE TableStock SET Quantity = Quantity + " + qty +
                             " WHERE ProductID = '" + pID + "'";

                SqlCommand cmd = new SqlCommand(qry, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Adding Stock Back: " + ex.Message);
            }
            finally { conn.Close(); }
        }
    }

}
