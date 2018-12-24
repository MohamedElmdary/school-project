using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace School
{
    public partial class schooldata : Form
    {
        public static bool isOpen = false;
        private string username;
        private string teacher_id;
        private string phone;
        public schooldata(string username, string teacher_id, string phone)
        {
            this.username = username;
            this.teacher_id = teacher_id;
            this.phone = phone;
            InitializeComponent();
            CenterToScreen();
            FormClosed += MyClosedHandler;
        }
        protected void MyClosedHandler(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void schooldata_Load(object sender, EventArgs e)
        {
            MySqlDataReader reader = Db.Read(
                "SELECT *, (arabic + english + math + programming) AS total FROM students WHERE teacher_id=" +
                                             Db.ValuesBuilder(new List<string>() { teacher_id }) +
                                             " ORDER BY total");

            while (reader.Read())
            {
                students.Rows.Add(
                    reader["id"],
                    reader["name"],
                    reader["arabic"],
                    reader["english"],
                    reader["math"],
                    reader["programming"],
                    reader["total"],
                    Convert.ToInt32(reader["total"]) > 200 ? "YES" : "NO"
                );
            }
            reader.Close();

        }

        private void edit_Click(object sender, EventArgs e)
        {
            if (!isOpen && students.SelectedRows.Count != 0)
            {
                isOpen = true;
                DataGridViewRow selectedRow = students.SelectedRows[0];
                new editstudent(
                    selectedRow,
                    selectedRow.Cells["id"].Value.ToString(),
                    this.teacher_id.ToString(),
                    selectedRow.Cells["arabic"].Value.ToString(),
                    selectedRow.Cells["english"].Value.ToString(),
                    selectedRow.Cells["math"].Value.ToString(),
                    selectedRow.Cells["programming"].Value.ToString()).Show();
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            if (students.SelectedRows.Count != 0 && !isOpen)
            {
                var selectedRows = students.SelectedRows;
                string msg = "You are removing the following students:\n";
                foreach (DataGridViewRow row in selectedRows)
                {
                    if (row != null)
                        msg += row.Cells["name"].Value.ToString() + " with id = " + row.Cells["id"].Value.ToString() + ".\n";
                }

                if (MessageBox.Show(msg, "confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in selectedRows)
                    {
                        if (row != null)
                        {
                            var res = Db.Mutation("DELETE FROM students WHERE id = " +
                                                  Db.ValuesBuilder(new List<string>() { row.Cells["id"].Value.ToString() })
                                                  +";");
                            // MessageBox.Show(res.ToString());
                            if (res)
                                students.Rows.Remove(row);
                        }
                    }
                    MessageBox.Show("removed all students");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!isOpen)
            {
                isOpen = true;
                new New_Student(this.teacher_id, students).Show();
            }
        }
    }

}
