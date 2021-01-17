using System;
using System.Collections.Generic;
using System.Linq;
using static ConsoleApp_ContactList.Program;

namespace ConsoleApp_ContactList
{
    internal class ProgramBase
    {
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

        private static void PrintPerson(Person person)
        {
            throw new NotImplementedException();
        }
    }
}