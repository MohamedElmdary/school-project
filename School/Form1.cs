using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace School
{
    public partial class Form1 : Form
    {
        public static bool popup = false;
        public Form1()
        {
            CenterToScreen();
            InitializeComponent();
            Db.createDbConnection();
            DesignHelpers.Placeholder(phone, "Phone");
            DesignHelpers.Placeholder(password, "**********");
            password.PasswordChar = '*';
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            if (!popup)
            {
                popup = true;
                new Register().Show();
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (popup) return;
            string phone = this.phone.Text;
            string password = this.password.Text;
            
            if (phone.Length != 11)
            {
                MessageBox.Show("Invalid phone");
            }
            else
            {
                try
                {
                    var reader = Db.Read("SELECT id, username, phone, password FROM teachers WHERE phone=" +
                                         Db.ValuesBuilder(new List<string>() { phone }));
                    reader.Read();
                    var user = new
                    {
                        username = reader["username"],
                        phone = reader["phone"],
                        password = reader["password"],
                        id = reader["id"]

                    };
                    reader.Close();

                    var valid = SecurePasswordHasher.Verify(password, user.password.ToString());

                    Console.WriteLine(valid.ToString());
                    if (valid)
                    {
                        string username = user.username.ToString();
                        schooldata sd = new schooldata(username, user.id.ToString(), phone);
                        sd.Show();
                        Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid password please try again");
                        this.password.Focus();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Teacher not exists");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!popup)
            {
                popup = true;
                new resetpassword().Show();
            }
        }
    }
}
