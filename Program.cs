

using System.Xml.Schema;
/**       
*--------------------------------------------------------------------
* 	   File name:Program
* 	Project name:CrowdisLab3
*--------------------------------------------------------------------
* Author’s name and email:	 kinsley crowdis crowdis@etsu.edu			
*          Course-Section: CSCI 2150-800
*           Creation Date:	09/14/2023
* -------------------------------------------------------------------
*/
namespace CrowdisLab3
{
    internal class Program
    {
        static string projectRootFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString();
        static string dbPath = projectRootFolder + "\\data\\data.db";


        static void Main(string[] args)
        {
            QueryBuilder stuff = new QueryBuilder(dbPath);
            Pokemon pokemon = new Pokemon();
            BannedGame bannedGame = new BannedGame();


            ///<summary>
            ///Write all objects in a collection to the database (create)
            ///</summary>
            ///

            var filePath = projectRootFolder + "\\data\\AllPokemon.csv";
            using (var sr = new StreamReader(filePath))
            {

                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    var data = line.Split(',');

                    Pokemon p = new Pokemon();

                    p.DexNumber = int.Parse(data[0]);
                    p.Name = data[1];
                    p.Form = data[2];
                    p.Type1 = data[3];
                    p.Type2 = data[4];
                    p.Total = int.Parse(data[5]);
                    p.HP = int.Parse(data[6]);
                    p.Attack = int.Parse(data[7]);
                    p.Defense = int.Parse(data[8]);
                    p.SpecialAttack = int.Parse(data[9]);
                    p.SpecialDefense = int.Parse(data[10]);
                    p.Speed = int.Parse(data[11]);
                    p.Generation = int.Parse(data[12]);





                    stuff.Create<Pokemon>(p);
                }


                List<Pokemon> dataList = stuff.ReadAll<Pokemon>();
                Console.WriteLine("Pokemon Table: ");
                foreach (var p in dataList)
                    Console.WriteLine(p);

                var filePathTwo = projectRootFolder + "\\data\\BannedGames.csv";
                using (var sr2 = new StreamReader(filePathTwo))
                {
                    sr2.ReadLine();

                    while (!sr2.EndOfStream)
                    {
                        var line = sr2.ReadLine();
                        var data = line.Split(',');

                        BannedGame bg = new BannedGame();
                        bg.Title = data[0];
                        bg.Series = data[1];
                        bg.Country = data[2];
                        bg.Details = data[3];


                        stuff.Create<BannedGame>(bg);
                    }

                    List<BannedGame> bannedGames = stuff.ReadAll<BannedGame>();
                    Console.WriteLine("Banned Games Table: ");
                    foreach (var item in bannedGames)
                    {
                        Console.WriteLine(item);
                    }

                    Console.WriteLine("Write all objects in a collection to the database ✓");


                    ///<summary>
                    ///Write a single object to the database (create)
                    ///</summary>
                    Console.Write("Would you like to add 'pokemon' or a 'banned game' to the database? ");
                    string userinput = (Console.ReadLine().Trim().ToLower()).Substring(0, 1);
                create:
                    switch (userinput)
                    {
                        case "p":
                            Pokemon userPokemon = new Pokemon();
                            Console.WriteLine("Creating New Pokemon...");
                            Console.WriteLine("--------------------------------------");
                            Console.WriteLine("Enter the following information...");

                            //id gets auto added

                            Console.Write("Dex Number: ");
                            userPokemon.DexNumber = int.Parse(Console.ReadLine());

                            Console.Write("Name of Pokemon: ");
                            userPokemon.Name = Console.ReadLine();

                            Console.Write("Form: ");
                            userPokemon.Form = Console.ReadLine();

                            Console.Write("Type 1: ");
                            userPokemon.Type1 = Console.ReadLine();

                            Console.Write("Type 2: ");
                            userPokemon.Type2 = Console.ReadLine();

                            Console.Write("Hit Points: ");
                            userPokemon.HP = int.Parse(Console.ReadLine());

                            Console.Write("Attack: ");
                            userPokemon.Attack = int.Parse(Console.ReadLine());

                            Console.Write("Defense: ");
                            userPokemon.Defense = int.Parse(Console.ReadLine());

                            Console.Write("Special Attack: ");
                            userPokemon.SpecialAttack = int.Parse(Console.ReadLine());

                            Console.Write("Special Defense: ");
                            userPokemon.SpecialDefense = int.Parse(Console.ReadLine());

                            Console.Write("Speed: ");
                            userPokemon.Speed = int.Parse(Console.ReadLine());

                            Console.Write("Generation: ");
                            userPokemon.Generation = int.Parse(Console.ReadLine());

                            Console.WriteLine("Calculating 'Total'...");

                            userPokemon.Total = userPokemon.HP + userPokemon.Attack + userPokemon.Defense + userPokemon.SpecialAttack + userPokemon.SpecialDefense + userPokemon.Speed;

                            Console.WriteLine($"'Total' value is: {userPokemon.Total}");


                            stuff.Create(userPokemon);
                            

                            List<Pokemon> dataListNew = stuff.ReadAll<Pokemon>();
                            Console.WriteLine("Click Enter to view new Pokemon Table: ");
                            Console.ReadLine();
                            foreach (var item in dataListNew)
                            {
                                Console.WriteLine(item);
                            }
                            break;


                        case "b":

                            BannedGame userBannedGame = new BannedGame();
                            Console.WriteLine("Creating New Banned Game...");
                            Console.WriteLine("--------------------------------------");
                            Console.WriteLine("Enter the following information...");

                            Console.Write("Title of Game: ");
                            userBannedGame.Title = Console.ReadLine();

                            Console.Write("Name of Series: ");
                            userBannedGame.Series = Console.ReadLine();

                            Console.Write("Country Game is Banned in: ");
                            userBannedGame.Country = Console.ReadLine();

                            Console.Write("Details about Ban: ");
                            userBannedGame.Details = Console.ReadLine();


                            stuff.Create(userBannedGame);

                            List<BannedGame> bannedGamesNew = stuff.ReadAll<BannedGame>();
                            Console.WriteLine("Click enter to view new Banned Games table: ");
                            Console.ReadLine();
                            foreach (var item in bannedGamesNew)
                            {
                                Console.WriteLine(item);
                            }

                            break;

                        default:
                            Console.Write("Please enter 'pokemon' or 'banned game': ");
                            userinput = (Console.ReadLine().Trim().ToLower()).Substring(0, 1);
                            goto create;
                    }
                    Console.WriteLine("Single object written to the database ✓");

                    ///<summary>
                    ///erase all records (deleteall)
                    ///</summary>
                    Console.WriteLine("Clearing Database...");
                    stuff.DeleteAll();
                    Console.WriteLine("Database Erased ✓");

                    Console.WriteLine("Thanks for using :)");



                }
            }
        }
    }
}