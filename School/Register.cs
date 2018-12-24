using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace School
{
    public partial class Register : Form
    {
        
        public Register()
        {
            CenterToScreen();
            InitializeComponent();
            DesignHelpers.Placeholder(username, "Username");
            DesignHelpers.Placeholder(phone, "Phone");
            password.PasswordChar = '*';
            DesignHelpers.Placeholder(password, "**********");
            DesignHelpers.Placeholder(secret, "Secret");
            FormClosed += MyClosedHandler;
        }

        protected void MyClosedHandler(object sender, EventArgs e)
        {
            Form1.popup = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = this.username.Text;
            string phone = this.phone.Text;
            string password = this.password.Text;
            string secret = this.secret.Text; 
            string errors = "";
            if (username.Trim().Length < 2)
            {
                errors += "Username min length is 2\n";
            }

            if (password.Trim().Length < 6)
            {
                errors += "Password too short\n";
            }

            if (phone.Trim().Length != 11)
            {
                errors += "Invalid phone number\n";
            }

            if (secret.Trim().Length < 1)
            {
                errors += "Secret min length is 1\n";
            }

            if (errors.Trim().Length > 0)
            {
                MessageBox.Show(errors);
            }
            else
            {
                var res = Db.Mutation("INSERT INTO teachers(username, phone, password, secret) VALUES(" + 
                                      Db.ValuesBuilder(new List<string>()
                                      {
                                          username,
                                          phone,
                                          SecurePasswordHasher.Hash(password),
                                          SecurePasswordHasher.Hash(secret)
                                      })
                                      + ")");

                if (res)
                {
                    MessageBox.Show("Successfully create new user");
                    Form1.popup = false;
                    Close();
                    return;
                }

                MessageBox.Show("Something went wrong please try again later");
            }
        }
    }
}
