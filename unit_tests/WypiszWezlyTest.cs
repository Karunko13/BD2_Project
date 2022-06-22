using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ProjektBDTests
{
    /// <summary>
    /// Test sprawdzajacy czy wezly wypisuja sie poprawnie
    /// </summary>
    [TestClass]
    public class WypiszWezlyTest
    {
        [TestMethod]
        public void WypisanieWezlow()
        {
            //init
            ProjektBD.Connection cnn = new ProjektBD.Connection();
            SqlConnection con = cnn.connect();
            Assert.AreEqual(true, cnn.getFlag());
            Assert.AreNotEqual(con, null);

            ProjektBD.DatabaseFunc db = new ProjektBD.DatabaseFunc(con);
            //test
            int status = ProjektBD.DatabaseFunc.findNodes("print_node n4");
            Assert.AreEqual(0, status);

            int status2 = ProjektBD.DatabaseFunc.findNodes("print_node n4 /bookstore");
            Assert.AreEqual(1, status2);

            int status3 =
                ProjektBD.DatabaseFunc.findNodes("print_node losowydok /bookstore");
            Assert.AreEqual(0, status3);

            int status4 = ProjektBD.DatabaseFunc.findNodes("print_node n4 /bbladblad");
            // nie znaleziono w bazie takiego węzła, jednak polecenie wykonane poprawnie
            Assert.AreEqual(1, status4);
        }
    }
}