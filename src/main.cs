using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ProjektBD
{   
    /// <summary>
    /// Główna klasa programu
    /// </summary>
    class main
    {   

        static void Main(string[] args)
        {
            Connection cnn = new Connection();
            SqlConnection con = cnn.connect();
            Menu menu = new Menu(con);
            menu.start();
        }
    }
}