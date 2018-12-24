using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace School
{
    public partial class resetpassword : Form
    {
        public resetpassword()
        {
            CenterToScreen();
            InitializeComponent();
            DesignHelpers.Placeholder(phone, "Phone");
            DesignHelpers.Placeholder(password, "**********");
            DesignHelpers.Placeholder(secret, "Secret");
            password.PasswordChar = '*';
            FormClosed += MyClosedHandler;
        }
        protected void MyClosedHandler(object sender, EventArgs e)
        {
            Form1.popup = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string phone = this.phone.Text;
            string password = this.password.Text;
            string errors = "";
            if (phone.Trim().Length != 11)
            {
                errors += "Invalid phone number\n";
            }

            if (password.Trim().Length < 6)
            {
                errors += "Invalid password\n";
            }

            if (errors.Trim().Length > 0)
            {
                MessageBox.Show(errors);
            }
            else
            {
                var reader = Db.Read("SELECT secret FROM teachers WHERE phone=" + 
                                     Db.ValuesBuilder(new List<string>() { phone }));
                bool r = false;
                if (reader != null)
                {
                    reader.Read();
                    r = SecurePasswordHasher.Verify(secret.Text, reader["secret"].ToString());
                }
                reader.Close();

                if (r)
                {
                    bool res = Db.Mutation("UPDATE teachers set password = " +
                                           Db.ValuesBuilder(new List<string>() { SecurePasswordHasher.Hash(password) }) +
                                           "WHERE phone = " + Db.ValuesBuilder(new List<string>() { phone }));
                    if (res)
                    {
                        MessageBox.Show("successfully changed password");
                        Form1.popup = false;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("something went wrong while changing password");
                    }
                }
            }
        }
    }
}
