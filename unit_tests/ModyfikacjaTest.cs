using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ProjektBDTests
{
    /// <summary>
    /// Test funkcji modyfikującej dane elementy
    /// </summary>
    [TestClass]
    public class ModyfikacjaTest
    {
        [TestMethod]
        public void Modyfikuj()
        {   
            //init
            ProjektBD.Connection cnn = new ProjektBD.Connection();
            SqlConnection con = cnn.connect();
            Assert.AreEqual(true, cnn.getFlag());
            Assert.AreNotEqual(con, null);

            ProjektBD.DatabaseFunc db = new ProjektBD.DatabaseFunc(con);
            ProjektBD.DatabaseFunc.delete("delete mod_test");
            //test
            int status6 = ProjektBD.DatabaseFunc.insert(
                "insert mod_test C:\\Users\\yeet\\source\\repos\\ProjektBD\\ProjektBD\\names.xml");
            Assert.AreEqual(1, status6);

            int status = ProjektBD.DatabaseFunc.modify(
                "modify mod_test (/Names/Name/FirstName[1]/text())[1] \"paluch\" ");
            Assert.AreEqual(1, status);

            int status2 = ProjektBD.DatabaseFunc.modify(
                "modify mod_test (/Names/Name/Blad[1]/text())[1] \"paluch\" ");
            Assert.AreEqual(1, status2);

            int status3 = ProjektBD.DatabaseFunc.modify(
                "modify mod_test (/Names/Name/Blad[1]/text())[1] ");
            Assert.AreEqual(0, status3);

            int status4 = ProjektBD.DatabaseFunc.modify("modify mod_test test ");
            Assert.AreEqual(0, status4);

            int status5 = ProjektBD.DatabaseFunc.modify("modify mod_test (/Nam] test ");
            Assert.AreEqual(0, status5);
            
            //delete
            ProjektBD.DatabaseFunc.delete("delete mod_test");
        }
    }
}