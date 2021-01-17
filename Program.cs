using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp_ContactList
{
    class Program
    {
        public static List<Person> People = new List<Person>();
        public class Person
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string[] Addresses { get; set; }
            public string Type { get; set; }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("******Welcome to contact list!******\n");
            LoadData();
            string command = "";
            while (command != "exit")
            {
                printMenu();
                Console.WriteLine("\nPlease enter a command: ");
                command = Console.ReadLine().ToLower();
                switch (command)
                {
                    case "add":
                        AddPerson();
                        break;
                    case "delete":
                        RemovePerson();
                        break;
                    case "list":
                        ListPeople();
                        break;
                    case "search":
                        SearchPerson();
                        break;
                    case "edit":
                        break;
                    case "clear":
                        Console.Clear();
                        break;
                }
            }
            SaveData();
        }
        private static void printMenu()
        {
            Console.WriteLine("======================================\n"
                + ".add\n"
                + ".delete\n"
                + ".list\n"
                + ".edit(not implemented)\n"
                + ".search\n"
                + ".clear\n"
                + ".exit\n"
                + "======================================");
        }

        private static void LoadData()
        {
            try
            {
                string[] inputFile = File.ReadAllLines("Text.txt");
                //Console.WriteLine("Data loaded...TEST" + inputFile.Count());
                int counter = inputFile.Count() / 8;
                for (int i = 0; i < counter; i++)
                {
                    Person p = new Person();
                    p.FirstName = inputFile[i * 8];
                    p.LastName = inputFile[1 + i * 8];
                    p.PhoneNumber = inputFile[2 + i * 8];
                    p.Email = inputFile[3 + i * 8];
                    string[] addresses = new string[2];
                    addresses[0] = inputFile[4 + i * 8];
                    addresses[1] = inputFile[5 + i * 8];
                    p.Addresses = addresses;
                    p.Type = inputFile[i * 8 + 6];
                    People.Add(p);
                    //Console.WriteLine("Data loaded...TEST" + i + People[i].FirstName);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong, no existing file.");
            }
            finally
            {
                Console.WriteLine("Data loaded sucess.");
            }
        }

        private static void AddPerson()
        {
            Person person = new Person();

            Console.Write("Enter First Name: ");
            person.FirstName = Console.ReadLine();

            Console.Write("Enter Last Name: ");
            person.LastName = Console.ReadLine();

            Console.Write("Enter Email: ");
            person.Email = Console.ReadLine();

            Console.Write("Enter Phone Number: ");
            person.PhoneNumber = Console.ReadLine();

            Console.Write("Enter Address 1: ");
            string[] addresses = new string[2];
            addresses[0] = Console.ReadLine();
            Console.Write("Enter Address 2 (Optional): ");
            addresses[1] = Console.ReadLine();
            person.Addresses = addresses;

            Console.Write("Enter Type/Remark: ");
            person.Type = Console.ReadLine();

            Console.Write("A new contact added!\n");
            People.Add(person);
        }

        private static void PrintPerson(Person person)
        {
            Console.WriteLine("First Name: " + person.FirstName);
            Console.WriteLine("Last Name: " + person.LastName);
            Console.WriteLine("Email: " + person.Email);
            Console.WriteLine("Phone Number: " + person.PhoneNumber);
            Console.WriteLine("Address 1: " + person.Addresses[0]);
            Console.WriteLine("Address 2: " + person.Addresses[1]);
            Console.WriteLine("Type: " + person.Type);
            Console.WriteLine("-------------------------------------------");
        }

        private static List<Person> SearchPerson()
        {
            Console.WriteLine("Enter the first name/start letter of the contact to search: ");
            string firstName = Console.ReadLine();
            //if no person with input first name return null "FirstOrDefault"
            //Person person = People.FirstOrDefault(x => x.FirstName.ToLower() == firstName.ToLower());

            List<Person> searchResult = new List<Person>();
            //searchResult.AddRange(from x in People
            //where x.FirstName.ToLower() == firstName.ToLower()
            //select x);

            searchResult.AddRange(from x in People
                                  where x.FirstName.ToLower().StartsWith(firstName.ToLower())
                                  select x);

            //test codes
            if (searchResult.Count() == 0)
            {
                Console.WriteLine("Name could not be found. Press any key to continue");
                Console.ReadKey();
            }
            else
            {
                for (int i = 0; i < searchResult.Count(); i++)
                {
                    int nr = i + 1;
                    Console.WriteLine("Search result #: " + nr);
                    PrintPerson(searchResult[i]);
                }
                Console.WriteLine(searchResult.Count() + " results found.");
            }

            return searchResult;
        }

        private static void ListPeople()
        {
            if (People.Count == 0)
            {
                Console.WriteLine("\nContact list is empty. Press any key to continue.");
                Console.ReadKey();
                return;
            }
            
            int count = 1;
            foreach (var person in People)
            {
                Console.WriteLine("#" + count + ":");
                PrintPerson(person);
                count++;
            }
            Console.WriteLine(People.Count + " people in contact list:\n");
        }
        private static void RemovePerson()
        {
            List<Person> ListP = SearchPerson();
            while (ListP.Count() != 0)
            {
                Console.WriteLine("\nSelect the number to remove or type A to remove all...");
                ConsoleKeyInfo UserInput = Console.ReadKey();
                Console.WriteLine("\nUser input: " + UserInput.KeyChar.ToString());
                if (char.IsDigit(UserInput.KeyChar))
                {
                    int index = int.Parse(UserInput.KeyChar.ToString())-1;
                    if (index >= 0 && index < ListP.Count())
                    {
                        Person p = ListP[index];
                        People.Remove(p);
                        Console.WriteLine("Person removed");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Invalid number...(number out of index). Press any key to continue.");
                        Console.ReadKey();
                        return;
                    }
                }
                else if (UserInput.Key == ConsoleKey.A)
                {
                    foreach(Person item in ListP)
                    {
                        People.Remove(item);
                    }
                    Console.WriteLine("Person removed");
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid input...(choose number or type A). Press any key to continue.");
                    Console.ReadKey();
                    return;
                }
            }
        }

        private static void edit()
        {
            List<Person> results = SearchPerson();
            int count = results.Count();
            //case no name found
            if (count == 0)
            {
                return;
            }
            else
            {
                Console.WriteLine("Select the number to edit");
                ConsoleKeyInfo UserInput = Console.ReadKey();
                int choice = int.Parse(UserInput.KeyChar.ToString());
                if (char.IsDigit(UserInput.KeyChar) && (choice < 1 || choice > count - 1))
                {
                    throw new NotImplementedException();
                }
            }
        }
        private static void SaveData()
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter("Text.txt"))
                foreach (Person item in People)
                {
                    string[] lines = {item.FirstName, item.LastName, item.PhoneNumber, item.Email,
                    item.Addresses[0], item.Addresses[1], item.Type, "***"};
                    foreach (string li in lines)
                    {
                        file.WriteLine(li);
                    }
                }
            Console.WriteLine("...data saveded..." + "Good Bye");
        }
        
    }
}