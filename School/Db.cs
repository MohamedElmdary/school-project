using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace School
{
    public class Db
    {
        private const string ConnString = "Server=localhost;Port=3306;Database=School;Uid=root;password=123456;";

        private static MySqlConnection db = new MySqlConnection(ConnString);

        public static void createDbConnection()
        {
            try
            {
                db.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("something went wrong while connection to database");
            }
        }

        public static string ValuesBuilder(List<String> s)
        {
            string values = "";
            foreach (var str in s)
            {
                values += "\"" + str + "\",";
            }

            return values.Remove(values.Length - 1);
        }

        public static MySqlDataReader Read(string query)
        {
                var cmd = db.CreateCommand();
                cmd.CommandText = query;
                var reader = cmd.ExecuteReader();
                return reader;
        }

        public static bool Mutation(string query)
        {
            try
            {
                var cmd = db.CreateCommand();
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                // MessageBox.Show(e.Message);
                return false;
            }
        }
    }
}
