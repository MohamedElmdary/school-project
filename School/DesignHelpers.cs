using System;
using System.Drawing;
using System.Windows.Forms;

namespace School
{
    public class DesignHelpers
    {
        public static void Placeholder(TextBox myTxtbx, string placeholder)
        {
            myTxtbx.GotFocus += RemoveText;
            myTxtbx.LostFocus += AddText;

            void RemoveText(object sender, EventArgs e)
            {
                if (myTxtbx.Text == placeholder || myTxtbx.PasswordChar == '*')
                {
                    myTxtbx.Text = "";
                    myTxtbx.ForeColor = Color.Black;
                }
            }

            void AddText(object sender, EventArgs e)
            {
                if (string.IsNullOrWhiteSpace(myTxtbx.Text))
                {
                    myTxtbx.Text = placeholder;
                    myTxtbx.ForeColor = Color.Gray;
                }

            }
        }
    }
}
