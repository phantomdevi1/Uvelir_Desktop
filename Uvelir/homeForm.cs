using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Uvelir
{
    public partial class homeForm : Form
    {
        private string connectionString = "Data Source=RX;Initial Catalog=Uvelir;Integrated Security=True";

        public homeForm()
        {
            InitializeComponent();
            DisplayProducts();
            flowLayoutPanelProducts.AutoScroll = true;
            flowLayoutPanelProducts.WrapContents = false;
            flowLayoutPanelProducts.FlowDirection = FlowDirection.LeftToRight;
        }
       

        private void DisplayProducts()
        {
            DataTable dataTable = GetProductsData();

            foreach (DataRow row in dataTable.Rows)
            {
                AddProductCard(row);
            }
        }

        private DataTable GetProductsData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ProductID, ProductName, Material, Price, StockQuantity, ProductImage FROM Products";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
        }

        private void AddProductCard(DataRow productData)
        {
            Panel productPanel = new Panel();
            productPanel.BorderStyle = BorderStyle.FixedSingle;
            productPanel.Size = new Size(200, 300);

            PictureBox productImage = new PictureBox();
            productImage.Size = new Size(200, 150);
            productImage.SizeMode = PictureBoxSizeMode.Zoom;
            byte[] imageData = (byte[])productData["ProductImage"];
            productImage.Image = byteArrayToImage(imageData);
            productImage.Location = new Point(0, 0);
            productPanel.Controls.Add(productImage);

            Label productNameLabel = new Label();
            productNameLabel.Text = productData["ProductName"].ToString();
            productNameLabel.Location = new Point(10, 155);
            productPanel.Controls.Add(productNameLabel);

            Label productMaterialLabel = new Label();
            productMaterialLabel.Text = productData["Material"].ToString();
            productMaterialLabel.Location = new Point(10, 180); 
            productPanel.Controls.Add(productMaterialLabel);

            Label productPriceLabel = new Label();
            productPriceLabel.Text = $"Цена: {productData["Price"].ToString()}$";
            productPriceLabel.Location = new Point(10, 205);
            productPanel.Controls.Add(productPriceLabel);

            Button addToCartButton = new Button();
            addToCartButton.Text = "В корзину";
            addToCartButton.Location = new Point(10, 250); // Левый верхний угол цены + 5
            addToCartButton.Click += (sender, e) => AddToCartClicked(productData);
            productPanel.Controls.Add(addToCartButton);

            flowLayoutPanelProducts.Controls.Add(productPanel);
        }

        private void AddToCartClicked(DataRow productData)
        {
            
        }

        private Image byteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream(byteArrayIn))
                {
                    Image returnImage = Image.FromStream(ms);
                    return returnImage;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при конвертации изображения: " + ex.Message);
                return null;
            }
        }


        private void PictureBox1_Click_1(object sender, EventArgs e)
        {
            lofinForm form1 = new lofinForm();
            form1.Show();
            this.Hide();
        }
    }
}
