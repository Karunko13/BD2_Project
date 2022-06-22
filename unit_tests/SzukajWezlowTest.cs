using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ProjektBDTests
{
    [TestClass]
    public class SzukanieDokumentowTest
    {
        /// <summary>
        /// Test sprawdzający czy wyszukanie węzłów wykonało się poprawnie
        /// </summary>
        [TestMethod]
        public void Szukanie()
        {  
            // init
            ProjektBD.Connection cnn = new ProjektBD.Connection();
            SqlConnection con = cnn.connect();
            Assert.AreEqual(true, cnn.getFlag());
            Assert.AreNotEqual(con, null);

            
            ProjektBD.DatabaseFunc db = new ProjektBD.DatabaseFunc(con);
            ProjektBD.DatabaseFunc.delete("delete unitszuk1");
            ProjektBD.DatabaseFunc.delete("delete unitszuk2");
            ProjektBD.DatabaseFunc.insert(
                "insert unitszuk1 C:\\Users\\yeet\\source\\repos\\ProjektBD\\ProjektBD\\names.xml");
            ProjektBD.DatabaseFunc.insert(
                "insert unitszuk2 C:\\Users\\yeet\\source\\repos\\ProjektBD\\ProjektBD\\names.xml");

            // szukanie
            int status2 = ProjektBD.DatabaseFunc.findDoc("search /Imiona/Test");

            Assert.AreEqual(1, status2);

            List<String> testlist = ProjektBD.DatabaseFunc.getNames();
            Assert.AreEqual(true, testlist.Contains("unitszuk1"));
            Assert.AreEqual(true, testlist.Contains("unitszuk2"));
            Assert.AreEqual(false, testlist.Contains("inne"));

            // clean
            ProjektBD.DatabaseFunc.delete("delete unitszuk1");
            ProjektBD.DatabaseFunc.delete("delete unitszuk2");
        }
    }
}