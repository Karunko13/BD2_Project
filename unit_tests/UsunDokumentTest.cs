using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ProjektBDTests
{
    /// <summary>
    /// Test sprawdzający czy usuwanie dokumentu powiodło się pomyślnie
    /// </summary>
    [TestClass]
    public class UsunDokumentTest
    {
        [TestMethod]
        public void UsuniecieZBazy()
        {
            //init
            ProjektBD.Connection cnn = new ProjektBD.Connection();
            SqlConnection con = cnn.connect();
            Assert.AreEqual(true, cnn.getFlag());
            Assert.AreNotEqual(con, null);

            // dodanie
            ProjektBD.DatabaseFunc db = new ProjektBD.DatabaseFunc(con);
            ProjektBD.DatabaseFunc.delete("delete test666");
            int status = ProjektBD.DatabaseFunc.insert(
                "insert test666 C:\\Users\\yeet\\source\\repos\\ProjektBD\\ProjektBD\\names.xml");

            // usuniecie
            int status2 = ProjektBD.DatabaseFunc.delete("delete test666");

            Assert.AreEqual(1, status2);

            SqlCommand cmn =
                new SqlCommand("SELECT Name FROM dbo.XMLS WHERE Name = 'test666'", con);
            Int32 stat = 0;
            try
            {
                stat = (Int32)cmn.ExecuteScalar();

            }
            catch (Exception)
            {
            }
            Assert.AreNotEqual(1, stat);

            con.Close();
        }
    }
}