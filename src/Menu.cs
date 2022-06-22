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
    /// klasa obsługująca wyświetlenie dostepynch opcji oraz wywołanie odpowiednich funkcji
    /// </summary>
    public class Menu
    {
        private static SqlConnection con_;
        private static DatabaseFunc db_fun_;
        private Dictionary<string, Func<string, int>> command_dict =
            new Dictionary<string, Func<string, int>>{
          {"help", help},
          {"insert", DatabaseFunc.insert},
          {"delete", DatabaseFunc.delete},
          {"show", DatabaseFunc.show},
          {"print_node", DatabaseFunc.findNodes},
          {"modify", DatabaseFunc.modify},
          {"search", DatabaseFunc.findDoc},
          {"print_doc", DatabaseFunc.print_doc}

            };
        public Menu(SqlConnection con)
        {
            con_ = con;
            db_fun_ = new DatabaseFunc(con);
        }

        public void start()
        {
            help();
            Console.WriteLine("Wpisz komendę:");

            String input = Console.ReadLine();
            while (input.ToLower() != "exit")
            {
                HandleCommand(input);
                Console.WriteLine("Wpisz nową komendę:");
                input = Console.ReadLine();
            }
        }

        // funkcja wywolujaca dostępne opcje
        public static int help(string s = " ")
        {
            String str = "\n";
            str += "Dostępne komendy: \n";
            str += "help - wypisanie dostępnych komend\n";
            str += "show - wypisanie wszystkich przechowywanych  dokumentów\n";
            str += "insert - dodanie nowego pliku xml\n";
            str += "delete - usuniecie pliku xml\n";
            str += "print_node - wypisanie węzła w danym pliku\n";
            str += "modify - zmiana wartości wskazanego atrybutu\n";
            str +=
                "search - zwraca wszystkie dokumenty w których wystepuje wskazany węzeł\n";
            str += "print_doc - wypisanie całego dokumentu XML\n";
            str += "exit - wyjście z programu\n";

            Console.WriteLine(str);
            return 1;
        }
        // funkcja szukająca odpowiednią funkcje w zależności od inputu
        public void HandleCommand(String str)
        {
            String[] tokens = str.Split(' ');
            foreach (KeyValuePair<string, Func<string, int>> entry in command_dict)
            {
                if (entry.Key == tokens[0])
                {
                    HandleFunction(entry.Value, str);
                }
            }
        }
        //funkcja wołająca odpowiednią funkcję
        public void HandleFunction(Func<string, int> fnc, String str)
        {
            if (fnc(str) != 1)
            {
                Console.WriteLine("Komenda nie powiodła się!");
            }
        }
    }
}