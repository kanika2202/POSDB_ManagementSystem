using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_DB
{
    public partial class OrderDetail : Form
    {
        public OrderDetail()
        {
            InitializeComponent();
        }
        CDB pos = new CDB();
        private void OrderDetail_Load(object sender, EventArgs e)
        {
            pos.dbConnect("localhost", "DB_POS");
            string[] data = pos.select("TableProduct", "ProductName", "ProductID");

            for (int i = 0; i < data.Length; i++)
            {
                ComboBoxProductName.Items.Add(data[i]);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                pos.dbConnect("localhost", "DB_POS");

                // ---- CHECK EMPTY ----
                if (string.IsNullOrEmpty(TextBoxOrderID.Text) ||
                    ComboBoxProductName.SelectedItem == null ||
                    string.IsNullOrEmpty(TextBoxQuantity.Text))
                {
                    MessageBox.Show("Please fill in all fields!");
                    return;
                }

                string productID = ComboBoxProductName.Text.Split('-')[0].Trim();
                int orderQty = int.Parse(TextBoxQuantity.Text);

                // ឆែកស្តុកមុននឹងលក់
                int stockQty = pos.CheckStock(productID);
                if (orderQty > stockQty)
                {
                    MessageBox.Show("Stock Error: Product is not enough! (Left: " + stockQty + ")");
                    return;
                }

                // រៀបចំទិន្នន័យសម្រាប់ Insert
                string[] columns = { "OrderID", "ProductID", "Quantity", "UnitPrice", "SubTotal" };
                decimal unitPrice = decimal.Parse(TextBoxUnitPrice.Text);
                decimal subTotal = orderQty * unitPrice;

                string[] datas = {
            TextBoxOrderID.Text,
            productID,
            orderQty.ToString(),
            unitPrice.ToString(),
            subTotal.ToString()
        };

                // ១. Insert ចូល TableOrderDetail
                pos.dbInsert("TableOrderDetail", columns, datas);

                // ២. >>> ហៅ Method ដកស្តុកនៅទីនេះ <<<
                pos.DeductStock(productID, orderQty);

                // ៣. Update Total ក្នុង TableOrder
                pos.UpdateOrderTotal(int.Parse(TextBoxOrderID.Text));

                MessageBox.Show("Add Success and Stock Updated!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string[] data = pos.searchID(
                    "TableOrderDetail",
                    "OrderDetailID",
                    Int16.Parse(TextBoxOrderDetail.Text));
            // ៣. ពិនិត្យមើលលទ្ធផល (Check for null)
            if (data == null)
            {
                // បើ data == null មានន័យថា រកមិនឃើញទិន្នន័យក្នុង Database ទេ
                MessageBox.Show("Data is emty");

                // សម្អាត TextBox ផ្សេងៗឱ្យទទេ
                TextBoxOrderID.Clear();
                TextBoxQuantity.Clear();
                TextBoxUnitPrice.Clear();
                TextBoxSubTotal.Clear();

            }
            else
            {
                TextBoxOrderDetail.Text = data[0];
                TextBoxOrderID.Text = data[1];

              
                foreach (var item in ComboBoxProductName.Items)
                {

                    if (item.ToString().StartsWith(data[2] + "-"))
                    {
                        ComboBoxProductName.SelectedItem = item;
                        break;
                    }

                }
                TextBoxQuantity.Text = data[3];
                TextBoxUnitPrice.Text = data[4];
                TextBoxSubTotal.Text=data[5];
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // ១. ទាញទិន្នន័យ "ចាស់" ចេញពី Database មុនពេល Update
                string[] oldData = pos.searchID("TableOrderDetail", "OrderDetailID", int.Parse(TextBoxOrderDetail.Text));

                if (oldData != null)
                {
                    string oldProductID = oldData[2];
                    int oldQty = int.Parse(oldData[3]);

                    // ២. ទាញទិន្នន័យ "ថ្មី" ពីលើ Form
                    string newProductID = ComboBoxProductName.Text.Split('-')[0].Trim();
                    int newQty = int.Parse(TextBoxQuantity.Text);

                    // ៣. ឆែកស្តុកថ្មី (តើគ្រាន់សម្រាប់ចំនួនថ្មីទេ?)
                    // យើងត្រូវគិតថា ស្តុកបច្ចុប្បន្ន + ចំនួនចាស់ = ស្តុកដែលអាចលក់បានពិតប្រាកដ
                    int currentStock = pos.CheckStock(newProductID);
                    if (newQty > (currentStock + (oldProductID == newProductID ? oldQty : 0)))
                    {
                        MessageBox.Show("stock is not enough for sale!");
                        return;
                    }

                    // ៤. ចាប់ផ្តើមដំណើរការ Update ស្តុក
                    // ក. បូកចំនួនចាស់ចូលស្តុកវិញ
                    pos.AddStockBack(oldProductID, oldQty);

                    // ខ. Update ទិន្នន័យក្នុង TableOrderDetail
                    string[] columns = { "OrderDetailID", "OrderID", "ProductID", "Quantity", "UnitPrice", "SubTotal" };
                    decimal unitPrice = decimal.Parse(TextBoxUnitPrice.Text);
                    decimal subTotal = newQty * unitPrice;
                    string[] datas = { TextBoxOrderDetail.Text, TextBoxOrderID.Text, newProductID, newQty.ToString(), unitPrice.ToString(), subTotal.ToString() };
                    pos.dbUpdate("TableOrderDetail", columns, datas);

                    // គ. ដកចំនួនថ្មីចេញពីស្តុក
                    pos.DeductStock(newProductID, newQty);

                    // ៥. Update Total រួម
                    pos.UpdateOrderTotal(int.Parse(TextBoxOrderID.Text));

                    MessageBox.Show("Update successfull!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // ១. ទាញយកទិន្នន័យចាស់ដែលចង់លុបសិន ដើម្បីដឹងថា Product អ្វី និងចំនួនប៉ុន្មាន
            string[] oldData = pos.searchID("TableOrderDetail", "OrderDetailID", int.Parse(TextBoxOrderDetail.Text));

            if (oldData != null)
            {
                string productID = oldData[2]; // ProductID
                int oldQty = int.Parse(oldData[3]); // Quantity ចាស់ដែលបានលក់

                // ២. លុបទិន្នន័យចេញពី TableOrderDetail
                pos.dbDelete("TableOrderDetail", "OrderDetailID", int.Parse(TextBoxOrderDetail.Text));

                // ៣. បូកចំនួនដែលលុបនោះ ចូលស្តុកវិញ
                pos.AddStockBack(productID, oldQty);

                // ៤. Update Total ឡើងវិញ
                pos.UpdateOrderTotal(int.Parse(TextBoxOrderID.Text));

                MessageBox.Show("delete success and add stock jol venh hx");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            StartupForm fr = new StartupForm();
            fr.Show();
            this.Hide();
        }

        private void TextBoxUnitPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void ComboBoxProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            if (ComboBoxProductName.SelectedItem != null)
            {
                // ១. ផ្ដាច់យក ID ចេញពី ComboBox (ឧទាហរណ៍ "1-Coca" យកតែ "1")
                string productID = ComboBoxProductName.Text.Split('-')[0].Trim();

                // ២. ហៅ Method ពី CDB មកប្រើ
                string price = pos.getProductPrice(productID);

                // ៣. បង្ហាញតម្លៃក្នុង TextBoxUnitPrice
                TextBoxUnitPrice.Text = price;

                // ៤. (បន្ថែម) បើចង់ឱ្យវាគណនា SubTotal ភ្លាមៗក៏បាន
                if (!string.IsNullOrEmpty(TextBoxQuantity.Text))
                {
                    decimal qty = decimal.Parse(TextBoxQuantity.Text);
                    decimal uPrice = decimal.Parse(price);
                    TextBoxSubTotal.Text = (qty * uPrice).ToString();
                }
            }
        }
    }
}
