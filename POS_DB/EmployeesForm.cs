using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_DB
{
    public partial class EmployeesForm : Form
    {
        public EmployeesForm()
        {
            InitializeComponent();
        }
       CDB pos = new CDB();
        private void EmployeesForm_Load(object sender, EventArgs e)
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
            string[] columns = { "FullName","Position","Phone","HireDate","IsActiveID" };
            string active = ComboBoxActive.Text.Split('-')[0];
            string[] datas = { TextBoxEmployeeFullName.Text,TextBoxPosition.Text,TextBoxPhone.Text,
                TextBoxHireDate.Text ,active };
            pos.dbInsert("TableEmployees", columns, datas);

        }

        private void button5_Click(object sender, EventArgs e)
        {
            StartupForm st=new StartupForm();
            st.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            string[] data = pos.searchID(
                    "TableEmployees",
                    "EmployeeID",
                    Int16.Parse(TextBoxEmployeeID.Text));
            // ៣. ពិនិត្យមើលលទ្ធផល (Check for null)
            if (data == null)
            {
                // បើ data == null មានន័យថា រកមិនឃើញទិន្នន័យក្នុង Database ទេ
                MessageBox.Show("Data is emty");

                // សម្អាត TextBox ផ្សេងៗឱ្យទទេ
                TextBoxEmployeeFullName.Clear();
                TextBoxPosition.Clear();
                TextBoxPhone.Clear();
                TextBoxHireDate.Clear();
                
            }
            else
            {
                TextBoxEmployeeID.Text = data[0];
                TextBoxEmployeeFullName.Text = data[1];
                TextBoxPosition.Text=data[2];
                TextBoxPhone.Text = data[3];
                string InputDate = data[4];
                DateTime create = DateTime.Parse(InputDate);

                // ប្តូរវាទៅជាទម្រង់ dd,MM,yyyy រួចដាក់ចូល TextBox វិញ
                TextBoxHireDate.Text = create.ToString("yyyy-MM-dd");
                foreach (var item in ComboBoxActive.Items)
                {

                    if (item.ToString().StartsWith(data[5] + "-"))
                    {
                        ComboBoxActive.SelectedItem = item;
                        break;
                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] columns = { "EmployeeID", "FullName", "Position", "Phone", "HireDate", "IsActiveID" };
            string active= ComboBoxActive.Text.Split('-')[0];
            string[] datas = {TextBoxEmployeeID.Text, TextBoxEmployeeFullName.Text,TextBoxPosition.Text,TextBoxPhone.Text,
                TextBoxHireDate.Text ,active };
            pos.dbUpdate("TableEmployees", columns, datas);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            pos.dbDelete("TableEmployees", "EmployeeID", Int16.Parse(TextBoxEmployeeID.Text));
        
        }

        private void LabelCount_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void TextBoxEmployeeID_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void TextBoxEmployeeFullName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void TextBoxPosition_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void TextBoxHireDate_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
