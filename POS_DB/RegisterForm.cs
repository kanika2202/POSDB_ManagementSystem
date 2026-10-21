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
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }
        CDB pos = new CDB();
        private void RegisterForm_Load(object sender, EventArgs e)
        {
            pos.dbConnect("localhost", "DB_POS");
        }

        private void ButtonRegister_Click(object sender, EventArgs e)
        {
            try
            {
                // ១. ឆែកមើលប្រឡោះទទេ
                if (string.IsNullOrEmpty(TextBoxUserName.Text) || string.IsNullOrEmpty(TextBoxPassword.Text))
                {
                    MessageBox.Show("សូមបញ្ចូល UserName និង Password!");
                    return;
                }

                pos.dbConnect("localhost", "DB_POS");

                // ២. ហៅ IsUserExists មកឆែកឈ្មោះជាន់គ្នា
                if (pos.IsUserExists(TextBoxUserName.Text))
                {
                    MessageBox.Show("ឈ្មោះ " + TextBoxUserName.Text + " ត្រូវបានគេប្រើរួចហើយ!");
                }
                else
                {
                    string[] columns = { "AdminName", "Password" };
                    string[] datas = { TextBoxUserName.Text, TextBoxPassword.Text };

                    pos.dbInsert("TableAdmin", columns, datas);
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();
            login.Show();
            this.Hide();
        }
    }
}
