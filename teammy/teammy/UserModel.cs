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
        public string Username { get; set; }
        private string Password { get; set; }
        public string Privilege { get; set; }

        public UserModel()
        {

        }
        public UserModel(string username, string password, string privilege)
        {
            Username = username;
            Password = password;
            Privilege = privilege;
        }

        public bool VerifyPassword(string passEntered)
        {
            return Password.Equals(passEntered);
        }
    }
}
