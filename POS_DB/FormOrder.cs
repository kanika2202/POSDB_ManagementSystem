using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_DB
{
    public partial class FormOrder : Form
    {
        public FormOrder()
        {
            InitializeComponent();
        }
       
        private void button1_Click(object sender, EventArgs e)
        {
            pos.dbConnect("localhost", "DB_POS");
            // ---- CHECK EMPTY ----
            if (TextBoxCustomerID.Text == "" ||
                    TextBoxEmployee.Text == "" ||
                    TextBoxOrderDate.Text == "")
                {
                    MessageBox.Show("Please fill in all fields before inserting!");
                    return;
                }
                // ----------------------
                // ---- CHECK SUB TOTAL NOT EMPTY ----
                if (TextBoxTotal.Text == "")
                {
                    MessageBox.Show("Total cannot be empty! Please press 0 first.");
                    return;
                }
           
            string[] columns = { "CustomerID", "EmployeeID", "OrderDate", "Total" };
            int total = Int32.Parse(TextBoxTotal.Text);
            string[] datas = { TextBoxCustomerID.Text, TextBoxEmployee.Text, TextBoxOrderDate.Text, total.ToString() };
            pos.dbInsert("TableOrder", columns, datas);

        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            

        }

        private void button5_Click(object sender, EventArgs e)
        {
           
        }
       CDB pos = new CDB();
        private void FormOrder_Load(object sender, EventArgs e)
        {
            pos.dbConnect("localhost", "DB_POS");
        }

        private void TextBoxTotal_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            string[] data = pos.searchID(
                    "TableOrder",
                    "OrderID",
                    Int16.Parse(TextBoxOrderID.Text));
            // ៣. ពិនិត្យមើលលទ្ធផល (Check for null)
            if (data == null)
            {
                // បើ data == null មានន័យថា រកមិនឃើញទិន្នន័យក្នុង Database ទេ
                MessageBox.Show("Data is emty");

                // សម្អាត TextBox ផ្សេងៗឱ្យទទេ
                TextBoxCustomerID.Clear();
                TextBoxEmployee.Clear();
                TextBoxOrderDate.Clear();
                TextBoxTotal.Clear();
            }
            else
            {
                // ៤. បើមានទិន្នន័យ (data != null) ទើបយកមកបង្ហាញ
                TextBoxOrderID.Text = data[0];
                TextBoxCustomerID.Text = data[1];
                TextBoxEmployee.Text = data[2];
                string InputDate = data[3];
                DateTime create = DateTime.Parse(InputDate);

                // ប្តូរវាទៅជាទម្រង់ dd,MM,yyyy រួចដាក់ចូល TextBox វិញ
                TextBoxOrderDate.Text = create.ToString("yyyy-MM-dd");
                TextBoxTotal.Text=data[4];
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] columns = { "OrderID", "CustomerID", "EmployeeID", "OrderDate", "Total" };
            string[] datas = { TextBoxOrderID.Text, TextBoxCustomerID.Text, TextBoxEmployee.Text, TextBoxOrderDate.Text, TextBoxTotal.Text };
            pos.dbUpdate("TableOrder", columns, datas);
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            StartupForm st=new StartupForm();
            st.Show();
            this.Hide();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            pos.dbDelete("TableOrder", "OrderID", Int16.Parse(TextBoxOrderID.Text));
        }
    
    }

}
