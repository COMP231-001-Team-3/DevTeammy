using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teammy
{
    public class UserModel
    {
        private int ID { get; set; }
        public string Username { get; set; }
        private string Password { get; set; }

        public UserModel(MySqlDataReader reader)
        {
            ID = int.Parse(reader[0].ToString());
            Username = reader[1].ToString();
            Password = reader[2].ToString();
        }

        public bool VerifyPassword(string passEntered)
        {
            return Password.Equals(passEntered);
        }
    }
}
