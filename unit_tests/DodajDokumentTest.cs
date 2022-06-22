using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ProjektBDTests
{
    [TestClass]
    public class DodajDokumentTest
    {   
        /// <summary>
        /// Test kontrolujący czy funkcja insert działa poprawnie
        /// </summary>
        [TestMethod]
        public void DodanieDoBazy()
        {
            //init
            ProjektBD.Connection cnn = new ProjektBD.Connection();
            SqlConnection con = cnn.connect();
            Assert.AreEqual(true, cnn.getFlag());
            Assert.AreNotEqual(con, null);

            // dodanie
            ProjektBD.DatabaseFunc db = new ProjektBD.DatabaseFunc(con);
            ProjektBD.DatabaseFunc.delete("delete unit1");
            int status = ProjektBD.DatabaseFunc.insert(
                "insert unit1 C:\\Users\\yeet\\source\\repos\\ProjektBD\\ProjektBD\\names.xml");
            Assert.AreEqual(1, status);

            SqlCommand cmn =
                new SqlCommand("SELECT Name FROM dbo.XMLS WHERE Name = 'unit1'", con);
            String stat = "";
            try
            {
                stat = (String)cmn.ExecuteScalar();

            }
            catch (Exception)
            {
            }
            Assert.AreEqual("unit1", stat);

            // usuniecie
            int status2 = ProjektBD.DatabaseFunc.delete("delete unit1");

            // dodanie dokumentu pustego
            int status3 = ProjektBD.DatabaseFunc.insert("insert brak brakbrak");
            Assert.AreEqual(0, status3);

            // dodanie dokumenu bez nazwy
            int status4 = ProjektBD.DatabaseFunc.insert("insert  brakbrak");
            Assert.AreEqual(0, status4);

            XmlDocument testdoc = new XmlDocument();
            String xmlDoc = "<wezel>13</wezel>";
            testdoc.LoadXml(xmlDoc);

            ProjektBD.DatabaseFunc.delete("delete test_doc");
            int status5 =
                ProjektBD.DatabaseFunc.insert("insert test_doc <wezel>13</wezel>");
            Assert.AreEqual(1, status5);

            //clean
            ProjektBD.DatabaseFunc.delete("delete test_doc");

            con.Close();
        }
    }
}