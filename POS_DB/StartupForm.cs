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
    public partial class StartupForm : Form
    {
        public StartupForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 fm1 = new Form1();
            fm1.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoginForm Fl = new LoginForm();
            Fl.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            EmployeesForm Fl = new EmployeesForm();
            Fl.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ProductForm pr=new ProductForm();
            pr.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OrderDetail od = new OrderDetail();
            od.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FormOrder or=new FormOrder();
            or.Show();
            this.Hide();
        }

        private void StartupForm_Load(object sender, EventArgs e)
        {
        }
        public StartupForm(string AdminName)
        {
            InitializeComponent();

            // បញ្ជាក់៖ LabelAdminName ត្រូវតែមានឈ្មោះក្នុង Design Form របស់អ្នក
            LabelAdminName.Text = "Hello : " + AdminName;
        }

        private void ButtonStock_Click(object sender, EventArgs e)
        {
            StockForm od = new StockForm();
            od.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            InformationOurTeam od = new InformationOurTeam();
            od.Show();
            this.Hide();
        }
    }
}
