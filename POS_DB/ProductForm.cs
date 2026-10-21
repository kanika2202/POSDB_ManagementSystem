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
    public partial class ProductForm : Form
    {
        public ProductForm()
        {
            InitializeComponent();
        }
       CDB pos = new CDB();
        private void ProductForm_Load(object sender, EventArgs e)
        {
            pos.dbConnect("localhost", "DB_POS");
            string[] data = pos.select("TableActive", "IsActive", "IsActiveID");

            for (int i = 0; i < data.Length; i++)
            {
                ComboBoxActive.Items.Add(data[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pos.dbConnect("localhost", "DB_POS");
            string[] columns = { "ProductName", "UnitPrice", "CreateAt", "IsActiveID" };
            string active = ComboBoxActive.Text.Split('-')[0];
            string[] datas = { TextBoxProductName.Text,TextBoxUnitPrice.Text,
                TextBoxCreateAt.Text ,active };
            pos.dbInsert("TableProduct", columns, datas);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string[] data = pos.searchID(
                    "TableProduct",
                    "ProductID",
                    Int16.Parse(TextBoxProductID.Text));
            // ៣. ពិនិត្យមើលលទ្ធផល (Check for null)
            if (data == null)
            {
                // បើ data == null មានន័យថា រកមិនឃើញទិន្នន័យក្នុង Database ទេ
                MessageBox.Show("Data is emty");

                // សម្អាត TextBox ផ្សេងៗឱ្យទទេ
                TextBoxProductName.Clear();
                TextBoxUnitPrice.Clear();
                TextBoxCreateAt.Clear();

            }
            else
            {
                TextBoxProductID.Text = data[0];
                TextBoxProductName.Text = data[1];
                TextBoxUnitPrice.Text = data[2];
                string InputDate = data[3];
                DateTime create = DateTime.Parse(InputDate);

                // ប្តូរវាទៅជាទម្រង់ dd,MM,yyyy រួចដាក់ចូល TextBox វិញ
                TextBoxCreateAt.Text = create.ToString("yyyy-MM-dd");
                foreach (var item in ComboBoxActive.Items)
                {

                    if (item.ToString().StartsWith(data[4] + "-"))
                    {
                        ComboBoxActive.SelectedItem = item;
                        break;
                    }

                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            StartupForm st=new StartupForm();
            st.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] columns = { "ProductID", "ProductName","UnitPrice","CreateAt","IsActiveID" };
            string active = ComboBoxActive.Text.Split('-')[0];
            string[] datas = {TextBoxProductID.Text,TextBoxProductName.Text,TextBoxUnitPrice.Text,
                TextBoxCreateAt.Text ,active };
            pos.dbUpdate("TableProduct", columns, datas);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pos.dbDelete("TableProduct", "ProductID", Int16.Parse(TextBoxProductID.Text));

        }
    }
}
