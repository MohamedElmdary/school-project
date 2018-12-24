using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace School
{
    public partial class New_Student : Form
    {
        private string teacher_id;
        private DataGridView gv;
        public New_Student(
            string teacher_id,
            DataGridView gv
            )
        {
            this.gv = gv;
            this.teacher_id = teacher_id;
            CenterToScreen();
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            schooldata.isOpen = false;
            Close();
        }

        private void New_Student_Load(object sender, EventArgs e)
        {
            id.Text = teacher_id;
        }

        private void add_Click(object sender, EventArgs e)
        {
            int a = 0, eng = 0, m = 0, p = 0;
            string n = name.Text;
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

            if (n.Trim().Length < 2)
            {
                errors += "student name min length is 2\n";
            }
            {

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
                var res = Db.Mutation("INSERT INTO students(name, arabic, english, math, programming, teacher_id) VALUES(" +
                                      Db.ValuesBuilder(new List<string>()
                                      {
                                          n, a.ToString(), eng.ToString(), m.ToString(), p.ToString(), teacher_id
                                      }) + ");");
                if (res)
                {
                    var reader = Db.Read("SELECT *, arabic + english + math + programming AS total FROM students ORDER BY id DESC LIMIT 1;");
                    reader.Read();
                    gv.Rows.Add(
                        reader["id"],
                        reader["name"],
                        reader["arabic"],
                        reader["english"],
                        reader["math"],
                        reader["programming"],
                        reader["total"],
                        Convert.ToInt32(reader["total"]) > 200 ? "YES" : "NO"
                    );
                    reader.Close();
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
