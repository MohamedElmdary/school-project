using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace School
{
    public partial class editstudent : Form
    {
        private DataGridViewRow row;
        private string sid;
        private string teacher_id;
        private string arabic;
        private string english;
        private string smath;
        private string sprog;
        public editstudent(
            DataGridViewRow row,
            string id,
            string teacher_id,
            string arabic,
            string english,
            string math,
            string sprog)
        {
            this.row = row;
            this.sid = id;
            this.teacher_id = teacher_id;
            this.arabic = arabic;
            this.english = english;
            this.smath = math;
            this.sprog = sprog;

            CenterToScreen();
            InitializeComponent();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            schooldata.isOpen = false;
            Close();
        }

        private void editstudent_Load(object sender, EventArgs e)
        {
            id.Text = this.sid;
            t_id.Text = this.teacher_id;
            ar.Text = this.arabic;
            en.Text = this.english;
            math.Text = this.smath;
            prog.Text = this.sprog;
        }

        private void save_Click(object sender, EventArgs e)
        {
            int a = 0, eng = 0, m = 0, p = 0;
            string errors = "";
            try
            {
                a = Convert.ToInt32(ar.Text);
                eng = Convert.ToInt32(en.Text);
                m = Convert.ToInt32(math.Text);
                p = Convert.ToInt32(prog.Text);
            }
            catch (Exception)
            {
                errors += "Invalid value as degree\n";
            }

            if (errors.Length == 0)
            {
                if (a < 0 || a > 100)
                {
                    errors += "Arabic degree max value is 100 and lowest one is 0\n";
                }
                if (eng < 0 || eng > 100)
                {
                    errors += "English degree max value is 100 and lowest one is 0\n";
                }
                if (m < 0 || m > 100)
                {
                    errors += "Math degree max value is 100 and lowest one is 0\n";
                }
                if (p < 0 || p > 100)
                {
                    errors += "Programming degree max value is 100 and lowest one is 0\n";
                }
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors);
            }
            else
            {
                var res = Db.Mutation("UPDATE students SET " + 
                                      "arabic = " + Db.ValuesBuilder(new List<string>() { a.ToString() }) +
                                      ",english = " + Db.ValuesBuilder(new List<string>() { eng.ToString() }) +
                                      ",math = " + Db.ValuesBuilder(new List<string>() { m.ToString() }) +
                                      ",programming = " + Db.ValuesBuilder(new List<string>() { p.ToString() }) + 
                                      " WHERE id = " + Db.ValuesBuilder(new List<string>() { sid }) + ";");

                if (res)
                {
                    row.Cells["arabic"].Value = a;
                    row.Cells["english"].Value = eng;
                    row.Cells["math"].Value = m;
                    row.Cells["programming"].Value = p;
                    var total = a + eng + m + p;
                    row.Cells["total"].Value = total;
                    row.Cells["passed"].Value = total < 200 ? "NO" : "YES";
                    schooldata.isOpen = false;
                    Close();
                }
                else
                {
                    MessageBox.Show("something went wrong while updating.");
                    schooldata.isOpen = false;
                    Close();
                }
            }
        }
    }
}
