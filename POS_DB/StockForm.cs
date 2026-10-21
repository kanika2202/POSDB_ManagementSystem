using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_DB
{
    public partial class StockForm : Form
    {
        public StockForm()
        {
            InitializeComponent();
        }
        CDB pos = new CDB();
        private void ButtonInsert_Click(object sender, EventArgs e)
        {
            pos.dbConnect("localhost", "DB_POS");
            string[] columns = { "ProductID","Quantity"};
            string product = ComboBoxProductID.Text.Split('-')[0];
            string[] datas = { product,TextBoxQuantity.Text.ToString()};
            pos.dbInsert("TableStock", columns, datas);
        }

        private void StockForm_Load(object sender, EventArgs e)
        {
            pos.dbConnect("localhost", "DB_POS");
            string[] data = pos.select("TableProduct", "ProductName", "ProductID");

            for (int i = 0; i < data.Length; i++)
            {
                ComboBoxProductID.Items.Add(data[i]);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string[] data = pos.searchID(
                   "TableStock",
                   "StockID",
                   Int16.Parse(TextBoxStockID.Text));
            // ៣. ពិនិត្យមើលលទ្ធផល (Check for null)
            if (data == null)
            {
                // បើ data == null មានន័យថា រកមិនឃើញទិន្នន័យក្នុង Database ទេ
                MessageBox.Show("Data is emty");

                // សម្អាត TextBox ផ្សេងៗឱ្យទទេ
                TextBoxQuantity.Clear();
            }
            else
            {
                TextBoxStockID.Text = data[0];

                foreach (var item in ComboBoxProductID.Items)
                {

                    if (item.ToString().StartsWith(data[1] + "-"))
                    {
                        ComboBoxProductID.SelectedItem = item;
                        break;
                    }

                }
                TextBoxQuantity.Text = data[2];
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] columns = { "StockID", "ProductID", "Quantity"};
            string product = ComboBoxProductID.Text.Split('-')[0];
            string[] datas = { TextBoxStockID.Text,product,TextBoxQuantity.Text};
            pos.dbUpdate("TableStock", columns, datas);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pos.dbDelete("TableStock", "StockID", Int16.Parse(TextBoxStockID.Text));

        }

        private void button5_Click(object sender, EventArgs e)
        {
            StartupForm st = new StartupForm();
            st.Show();
            this.Hide();

        }
    }
}
