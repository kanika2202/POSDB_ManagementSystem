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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }
        CDB pos = new CDB();
        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            pos.dbConnect("localhost", "DB_POS");
            string[] col = { "AdminName", "Password" };
            string[] data = { TextBoxAdminName.Text, TextBoxPassword.Text };

            pos.LoginSelect("TableAdmin", col, data, this);
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            RegisterForm re = new RegisterForm();
            re.Show();
            this.Hide();
        }
    }
}
