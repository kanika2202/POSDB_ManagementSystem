using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace POS_DB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
       CDB pos = new CDB(); 
        private void button1_Click(object sender, EventArgs e)
        {
            pos.dbConnect("localhost", "DB_POS");
            string[] columns = { "CustomerName", "Phone", "Address", "CreateAt" };
            string[] datas = { TextBoxCustomerName.Text,TextBoxPhone.Text,TextBoxAddress.Text,TextBoxCreateAt.Text };
            pos.dbInsert("TableCustomer", columns, datas);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pos.dbConnect("localhost", "DB_POS");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            StartupForm st=new StartupForm();
            st.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] columns = { "CustomerId", "CustomerName", "Phone", "Address", "CreateAt" };
            string[] datas = { TextBoxCustomerID.Text, TextBoxCustomerName.Text, TextBoxPhone.Text, TextBoxAddress.Text, TextBoxCreateAt.Text };
            pos.dbUpdate("TableCustomer", columns, datas);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string[] data = pos.searchID(
                     "TableCustomer",
                     "CustomerID",
                     Int16.Parse(TextBoxCustomerID.Text));
            // ៣. ពិនិត្យមើលលទ្ធផល (Check for null)
            if (data == null)
            {
                // បើ data == null មានន័យថា រកមិនឃើញទិន្នន័យក្នុង Database ទេ
                MessageBox.Show("Data is emty");

                // សម្អាត TextBox ផ្សេងៗឱ្យទទេ
                TextBoxCustomerName.Clear();
                TextBoxPhone.Clear();
                TextBoxAddress.Clear();
                TextBoxCreateAt.Clear();
            }
            else
            {
                // ៤. បើមានទិន្នន័យ (data != null) ទើបយកមកបង្ហាញ
                TextBoxCustomerID.Text = data[0];
                TextBoxCustomerName.Text = data[1];
                TextBoxPhone.Text = data[2];
                TextBoxAddress.Text = data[3];
                string InputDate = data[4];
                DateTime create = DateTime.Parse(InputDate);

                // ប្តូរវាទៅជាទម្រង់ dd,MM,yyyy រួចដាក់ចូល TextBox វិញ
                TextBoxCreateAt.Text = create.ToString("yyyy-MM-dd");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pos.dbDelete("TableCustomer", "CustomerID", Int16.Parse(TextBoxCustomerID.Text));
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void LabelCount_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
