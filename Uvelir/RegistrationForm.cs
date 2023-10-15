using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Uvelir
{
    public partial class RegistrationForm : Form
    {
        private string connectionString = "Data Source=RX;Initial Catalog=Uvelir;Integrated Security=True";
        private SqlConnection connection;

        public RegistrationForm()
        {
            InitializeComponent();

            // Создаем подключение к базе данных при загрузке формы
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
                    // Можете добавить дополнительные действия при успешном подключении
                }
                else
                {
                    MessageBox.Show("Соединение с базой данных не удалось установить.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к базе данных: " + ex.Message);
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            string login = loginTextBox.Text;
            string password = passwordTextBox.Text;
            string email = email_Textbox.Text;
            string phone = phone_Textbox.Text;
            string name = name_Textbox.Text;

            // Проверка на пустые поля
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }
            if( !(email.Contains("@") && email.Contains(".")))
            {
                MessageBox.Show("Введите корректный email!");
                return;
            }
            if (!(Regex.IsMatch(phone, @"^\d{11}$")))
            {
                MessageBox.Show("Номер телефона введен не корректно.");
                return;
            }
            if (!(name.Count(char.IsWhiteSpace) == 1))
            {
                MessageBox.Show("Введите корректное имя и фамилию.");
                return;
            }

            if (connection == null || connection.State != ConnectionState.Open)
            {
                MessageBox.Show("Нет подключения к базе данных.");
                return;
            }

            string query = "INSERT INTO users (user_login, email, phone_number,  username, user_password) " +
                           "VALUES (@Login, @Email, @Phone, @Name, @Password)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Login", login);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@Name", name);

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Регистрация выполнена успешно!");
                    }
                    else
                    {
                        MessageBox.Show("Не удалось выполнить регистрацию.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при регистрации: " + ex.Message);
                }
            }
        }

        private void Input_btn_Click(object sender, EventArgs e)
        {
            // Теперь этот метод вызывает код для регистрации пользователя
            RegisterButton_Click(sender, e);
        }

        private void Label5_Click(object sender, EventArgs e)
        {
            lofinForm form1 = new lofinForm();
            form1.Show();
            this.Close();
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            passwordTextBox.PasswordChar = checkBox1.Checked ? '\0' : '*';
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            // Это событие не используется в данном контексте. Вы можете удалить этот метод, если он вам не нужен.
        }
    }
}
