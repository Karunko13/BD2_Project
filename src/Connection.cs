using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ProjektBD
{   
    /// <summary>
    /// Klasa odpowiedzialna za połączenie z baza danych
    /// </summary>
    public class Connection
    {
        bool flag;
        public SqlConnection conn;
        public Connection() { }
        public bool getFlag() { return flag; }
        public SqlConnection connect()
        {
            conn = new SqlConnection(
                "SERVER=DESKTOP-O9R1827; INITIAL CATALOG=projekt; INTEGRATED SECURITY=SSPI;");
            try
            {
                conn.Open();
                Console.WriteLine("Połączono z baza danych.");
                flag = true;
            }
            catch
            {
                Console.WriteLine("Nie udalo polaczyc sie z baza danych.");
                flag = false;
                return null;
            }

            return conn;
        }
    }
}