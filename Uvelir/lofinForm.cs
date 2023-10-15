using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Uvelir
{
    public partial class lofinForm : Form
    {
        private string connectionString = "Data Source=RX;Initial Catalog=Uvelir;Integrated Security=True";
        private SqlConnection connection;

        public lofinForm()
        {
            InitializeComponent();

            this.passwordTextBox.AutoSize = false;
            this.passwordTextBox.Size = new Size(this.passwordTextBox.Size.Width, 57);

            // Попробуем установить соединение при создании формы
            InitializeDatabaseConnection();
        }

        private void InitializeDatabaseConnection()
        {
            connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                if (connection.State == ConnectionState.Open)
                {
                    
                }
                else
                {
                    MessageBox.Show("Соединение с базой данных не удалось установить.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к базе данных: " + ex. Message);
            }
        }

        private void lofinForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close(); // Закрыть соединение при закрытии формы
            }
        }

        private void Input_btn_Click(object sender, EventArgs e)
        {
            string login = loginTextBox.Text;
            string password = passwordTextBox.Text;

            if (connection == null || connection.State != ConnectionState.Open)
            {
                MessageBox.Show("Нет подключения к базе данных.");
                return;
            }

            string query = "SELECT * FROM users WHERE user_login = @Login AND user_password = @Password";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    homeForm form3 = new homeForm();
                    form3.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль.");
                }
                reader.Close();
            }
        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }

        private void Label5_Click(object sender, EventArgs e)
        {
            RegistrationForm form2 = new RegistrationForm();
            form2.Show();
            this.Hide();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            passwordTextBox.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }

        private void LofinForm_Load(object sender, EventArgs e)
        {

        }
    }
}
