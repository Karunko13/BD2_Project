using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Xml;
using System.IO;

namespace ProjektBD
{   
    /// <summary>
    /// Klasa obsługująca zapytania do bazy danych
    /// </summary>
    public class DatabaseFunc
    {
        public static List<string> nameslist = new List<string>();
        public static List<string> correct = new List<string>();

        private static SqlConnection con_;
        public DatabaseFunc(SqlConnection con) { con_ = con; }

        // getter do listy nazw z bazy
        public static List<String> getNames() { return correct; }
        
        //funkcja dodająca nowy wiersz z dokumentem do bazy
        public static int insert(string str = " ")
        {
            String[] tokens = str.Split(' ');
            String content = null;

            try
            {
                content = File.ReadAllText(tokens[2]);

            }
            catch (Exception)
            {
                // Console.WriteLine("Błąd podczas odczytu pliku!\n");
                // return 0;
            }
            if (content == null)
            {
                content = tokens[2];
            }

            XmlDocument insert_xml = new XmlDocument();

            bool flag = false;
            try
            {
                insert_xml.LoadXml(content);
            }
            catch (Exception)
            {
                Console.WriteLine("Pusty dokument XML!");
                flag = true;
            }

            SqlCommand command = new SqlCommand(
                "INSERT INTO dbo.XMLS(Name,XMLColumn) VALUES (@doc_name, @doc_flesh)",
                con_);
            command.Parameters.Add("@doc_name", SqlDbType.VarChar).Value = tokens[1];
            command.Parameters.Add("@doc_flesh", SqlDbType.Xml).Value =
                insert_xml.OuterXml;

            if (tokens[1].Length == 0 || flag == true)
            {
                Console.WriteLine(
                    "Nie można dodac do bazy dokumentu pustego lub bez nazwy!");
                return 0;

            }
            else
            {
                try
                {
                    Object o = command.ExecuteScalar();
                    Console.WriteLine("Poprawnie dodano do bazy");
                    return 1;
                }
                catch (SqlException e)
                {
                    Console.WriteLine("Dodawanie do bazy nie powiodło się");
                    return 0;
                }
            }
        }

        /// funkcja usuwająca wiersz o podanej nazwie
        public static int delete(string str = " ")
        {
            String[] tokens = str.Split(' ');
            SqlCommand command = new SqlCommand(
                "DELETE FROM dbo.XMLS WHERE Name = '" + tokens[1] + "'", con_);
            try
            {
                Object o = command.ExecuteScalar();
                Console.WriteLine("Poprawnie usunięto wiersz");
                return 1;
            }
            catch (SqlException e)
            {
                Console.WriteLine("Błąd podczas usuwania");
                return 0;
            }
        }

        /// funkcja wypisująca wszystkie pozycje z tabeli
        public static int show(string str = " ")
        {
            String s = "";
            SqlCommand command = new SqlCommand("SELECT * FROM dbo.XMLS", con_);
            SqlDataReader reader = command.ExecuteReader();

            int i = 1;
            while (reader.Read())
            {
                s += String.Format("{0}.{1}\t{2}\n\n", i++, reader.GetString(0),
                                   reader.GetString(1));
            }
            reader.Close();
            if (s == "")
            {
                return 0;
            }
            else
            {
                s += "\n";
                Console.Write(s);
                return 1;
            }
        }

        // funkcja szukająca konkretnego węzła
        public static int findNodes(string str = " ")
        {
            String[] tokens = str.Split(' ');
            String s = "";

            List<string> names = new List<string>();
            SqlCommand cmn = new SqlCommand("SELECT Name FROM dbo.XMLS", con_);
            SqlDataReader r1 = cmn.ExecuteReader();

            while (r1.Read())
            {
                names.Add(r1[0]
                              .ToString());
            }
            r1.Close();

            if (!names.Contains(tokens[1]))
            {
                return 0;
            }

            SqlCommand command = new SqlCommand(
                "SELECT XMLColumn FROM dbo.XMLS WHERE name = @name", con_);
            command.Parameters.Add("@name", SqlDbType.VarChar).Value = tokens[1];
            XmlReader reader = command.ExecuteXmlReader();

            if (reader != null)
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(reader);

                try
                {
                    XmlNodeList xnList = xml.SelectNodes(tokens[2]);

                    foreach (XmlNode xn in xnList)
                    {
                        if (xn != null)
                        {
                            findChildNodes(xn);
                        }
                    }
                    reader.Close();
                    return 1;
                }
                catch (Exception)
                {
                    return 0;
                }

            }
            else
            {
                reader.Close();
                return 0;
            }
        }

        //rekurencyjne wywołanie funkcji findNodes
        public static String findChildNodes(XmlNode xn, String tabulator = "")
        {
            String element = "";
            element += "\n" + tabulator;
            element += "<";
            try
            {
                if (xn.NodeType == XmlNodeType.Element)
                {
                    element += String.Format("{0}", xn.Name);

                    if (xn.NodeType != XmlNodeType.Text)
                    {
                        if (xn.Attributes.Count != 0)
                        {
                            for (int i = 0; i < xn.Attributes.Count; i++)
                            {
                                element += String.Format(" {0} = {1}", xn.Attributes[i].Name,
                                                         xn.Attributes[i].Value);
                            }
                        }
                    }
                }
                if (xn.NodeType == XmlNodeType.Text)
                {
                    element += xn.Value;
                }
                if (xn.NodeType == XmlNodeType.DocumentType)
                {
                    element += String.Format("DOCTYPE {0}", xn.Name);
                }

                element += ">";
                Console.WriteLine(element);
                XmlNodeList xnList = xn.ChildNodes;
                tabulator += "\t";
                foreach (XmlNode x in xnList)
                {
                    element += findChildNodes(x, tabulator);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Błąd w pliku xml");
            }

            return element;
        }

        //funkcja modyfikująca wybrany atrybut
        public static int modify(string str = "")
        {

            
            String[] tokens = str.Split(' ');
            String s;
            if(tokens.Length<3)
            {
                return 0;
            }
            int temp_stat = findNodes("find "+tokens[1]+" "+tokens[2]);
            if(temp_stat == 0)
            {
                return 0;
            }
            SqlCommand command = new SqlCommand(
                "UPDATE dbo.XMLS " + "SET XMLColumn.modify('replace value of " +
                    tokens[2] + " with " + tokens[3] + "') " + "WHERE Name = '" +
                    tokens[1] + "'",
                con_);
            try
            {
                Object o = command.ExecuteScalar();
                Console.WriteLine("Poprawnie zmodyfikowano wartość");
                return 1;
            }
            catch (SqlException e)
            {
                Console.WriteLine("Błąd podczas modyfikacji");
                return 0;
            }
        }

        //funkcja wypisująca cały dokument
        public static int print_doc(string str = "")
        {

            String[] tokens = str.Split(' ');
            String s;

            SqlCommand command = new SqlCommand(
               "SELECT XMLColumn FROM dbo.XMLS WHERE name = @name", con_);
            command.Parameters.Add("@name", SqlDbType.VarChar).Value = tokens[1];
            XmlReader reader = command.ExecuteXmlReader();

            XmlDocument xml = new XmlDocument();
            xml.Load(reader);
            xml.Save(Console.Out);

            return 1;

        }
        // znalezienie wszystkich dokumentów w których pojawia się dany wezel, zwraca
        // liste dokumentow
        public static int findDoc(string str = "")
        {
            String[] tokens = str.Split(' ');
            String s;

            SqlCommand command = new SqlCommand("SELECT Name FROM dbo.XMLS", con_);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                nameslist.Add(reader[0]
                                  .ToString());
            }
            reader.Close();

            foreach (String name in nameslist)
            {
                SqlCommand cmn = new SqlCommand(
                    "SELECT CAST(XMLColumn.exist('" + tokens[1] +
                        "') AS int) FROM dbo.XMLS WHERE Name = '" + name + "'",
                    con_);
                Int32 exist_flag = 0;
                try
                {
                    exist_flag = (Int32)cmn.ExecuteScalar();

                }
                catch (Exception)
                {
                }

                if (exist_flag == 1)
                {
                    correct.Add(name);
                }
            }

            if (correct.Count == 0)
            {
                Console.WriteLine("Nie znaleziono takiego węzła w żadnym dokumencie!\n");

                return 0;
            }
            else
            {
                Console.WriteLine("Węzły znalezione w dokumentach o nazwach: ");
                foreach (String name in correct)
                {
                    Console.Write(name + "\t");
                }
                Console.WriteLine("\n\n");
                return 1;
            }
        }
    }
}