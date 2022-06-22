using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ProjektBDTests
{
    /// <summary>
    /// Test sprawdzający czy połączenie zostało poprawnie ustanowione
    /// </summary>
    [TestClass]
    public class PolaczenieTest
    {
        // test connection status
        [TestMethod]
        public void Polaczenie()
        {
            ProjektBD.Connection cnn = new ProjektBD.Connection();
            SqlConnection con = cnn.connect();

            Assert.AreEqual(true, cnn.getFlag());
            Assert.AreNotEqual(con, null);

            con.Close();
        }
    }
}