// day 1 2 hours
// day 2 2:15 + 1:30 hours
// day 3 0:30 hours
// day 4 1:15 hours
// day 5 1:15 hours
// day 6 

// Type exeption error ???

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace cristal_caverns
{


    public class cavern //object template for each room
    {
        public int[] pos { get; set; } // position in array
        public string discribe { get; set; } // strings containing discription of the room
        public string[] exits { get; set; } //strings containing discriptions of the exits
        public int[,] paths { get; set; } //array containing the path each exit takes
        public item[] inventory { get; set; } = new item[100]; // array containing Items on the ground

        public cavern() // empty constructor
        {
            // temp variables set
            int[] pos = { 0, 0, 0 };
            discribe = "null";
            string[] exits = { "N", "S", "E", "W", "U", "D" };
            int[,] paths = { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        }

        public cavern(int[] Pos, string Discribe, string[] Exits, int[,] Paths) // Room constructor
        {
            // sets variables in the new object
            pos = Pos;
            discribe = Discribe;
            exits = Exits;
            paths = Paths;
        }

        // Return the point's value as a string.
        public override String ToString()
        {
            return string.Format("({0}, {1}, {2}) ({3}) ({4})", pos[0], pos[1], pos[2], discribe, exits[0]);
            //string[] Text = {discribe, exits};
            //return string.Format(Text);
        }



        // Return a copy of this point object by making a simple field copy.
        public cavern Copy()
        {
            return (cavern)this.MemberwiseClone();
        }
    }

    public class item //object template for Items
    {
        public string name { get; set; } = @"_"; // Item Name
        public bool movable { get; set; } = false; // Item Movability
        public string discription { get; set; } = null; // Item Discription

        public item() // empty constructor
        {
            name = null;
            discription = null;
            movable = false;
        }

        public item(string Name, string Discription, bool Movable) // sets Variables
        {
            name = Name;
            discription = Discription;
            movable = Movable;
        }

        public bool equals() // overides the string value of this object
        {
            return (name == @"_");
            
        }

        public item Copy()
        {
            return (item)this.MemberwiseClone();
        }
    }

    public class player
    {
        public string name { get; set; } = "";
        public item[] inventory { get; set; } = new item[100000000];

        public player(string Name)
        {
            name = Name;
        }

        public override String ToString()
        {
            return string.Format("{0}", name);
        }
    }

    class Program
    {
        // Initalization of functions
        static void Load() // loads map from file
        {
            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "../../", "Save_template.txt")); // stores current directory in path
            Console.WriteLine(path);
            foreach (string line in File.ReadLines(path))
            {
                if (line.Contains("//")) // finds comments and reads them in debug
                {
                    // Console.WriteLine(line); // debug comment reader
                }
                else if (line.Contains("Cavern:")) // finds all cavern defs from file
                {
                    int pos = 0;   // Pointers
                    int exits = 0; //
                    int route = 0; //

                    int[] Pos = new int[3];         // Arrays 
                    string[] Exits = new string[6]; //
                    int[,] Route = new int[5, 2];   // ...

                    string Dis = ""; // String

                    string T = line.Substring(8); // looses the "Cavern:" part of the string
                    int i = 0;
                    for (int I = 1; I < T.Length; I++) // iterates through the line
                    {
                        if (T[I] == "<"[0]) // finds pos values apended with a < 
                        {


                            string test = (T.Substring(i, (I - i)));
                            //Console.WriteLine(test);            //Debug
                            //Console.WriteLine("{0},{1}", I, i); //Debug
                            Pos[pos] = int.Parse(test); // pulls numerical value followed by a <

                            i = I + 1; //Advances start pointer
                            pos++;
                        }
                        else if (T[I] == ";"[0]) // finds Room discription values apended with a ;
                        {
                            string test = (T.Substring(i, (I - i)));
                            if (test[0] == " "[0])
                            {
                                test = test.Substring(1, test.Length - 1);
                            }
                            //Console.WriteLine(test);            //Debug
                            //Console.WriteLine("{0},{1}", I, i); //Debug
                            Dis = test; // pulls string value followed by a ;

                            i = I + 1; //Advances start pointer
                        }
                        else if (T[I] == ":"[0]) // finds Exit discription values apended with a :
                        {
                            string test = (T.Substring(i, (I - i)));
                            if (test[0] == " "[0])
                            {
                                test = test.Substring(1, test.Length - 1);
                            }
                            //Console.WriteLine(test);            //Debug
                            //Console.WriteLine("{0},{1}", I, i); //Debug
                            Exits[exits] = test; // pulls string value followed by a :

                            i = I + 1; //Advances start pointer
                            exits++;
                        }
                        else if (T[I] == ">"[0]) // finds route sets apended with a >
                        {
                            int Start = 0;
                            int pointer = 0;
                            int[] Delta = new int[2];
                            string test = (T.Substring(i, (I - i)));
                            //Console.WriteLine(test);            //Debug
                            //Console.WriteLine("{0},{1}", I, i); //Debug
                            for (int End = 1; End < test.Length; End++)
                            {
                                if (T[End] == "|"[0]) // finds route values apended with a |
                                {
                                    string testSub = (T.Substring(Start, (End - Start)));
                                    //Console.WriteLine(testSub);               //Debug
                                    //Console.WriteLine("{0},{1}", End, Start); //Debug
                                    Route[route, pointer] = int.Parse(testSub); // pulls string value followed by a |

                                    Start = End + 1; //Advances start pointer
                                    pointer++;
                                }
                            }
                            i = I + 1; //Advances start pointer
                            route++;
                        }

                    }

                    Map1[Pos[0], Pos[1], Pos[2]] = new cavern(Pos, Dis, Exits, Route); //make new room and store into an array

                }
                else if (line.Contains("Item:"))
                {
                    int pos = 0;      // pointer
                    int[] Pos = new int[3];
                    string Dis = "";  // string
                    string Name = ""; // 
                    int Amount = 1;   // int
                    bool Move = false;

                    string T = line.Substring(5); // looses the "Item:" part of the string
                    int i = 0; // start pointer
                    for (int I = 1; I < T.Length; I++) // iterates through the line
                    {
                        if (T[I] == "<"[0]) // finds pos values apended with a < 
                        {
                            string test = (T.Substring(i, (I - i)));
                            //Console.WriteLine(test);            //Debug
                            //Console.WriteLine("{0},{1}", I, i); //Debug
                            Pos[pos] = int.Parse(test); // pulls numerical value followed by a <

                            i = I + 1; //Advances start pointer
                            pos++;
                        }
                        else if (T[I] == ";"[0]) // finds Item name values apended with a ;
                        {
                            string test = (T.Substring(i, (I - i)));
                            if (test[0] == " "[0])
                            {
                                test = test.Substring(1, test.Length - 1);
                            }
                            //Console.WriteLine(test);            //Debug
                            //Console.WriteLine("{0},{1}", I, i); //Debug
                            Name = test; // pulls string value followed by a ;

                            i = I + 1; //Advances start pointer
                        }
                        else if (T[I] == ":"[0]) // finds Item discription values apended with a :
                        {
                            string test = (T.Substring(i, (I - i)));
                            if (test[0] == " "[0])
                            {
                                test = test.Substring(1, test.Length - 1);
                            }
                            //Console.WriteLine(test);            //Debug
                            //Console.WriteLine("{0},{1}", I, i); //Debug
                            Dis = test; // pulls string value followed by a :

                            i = I + 1; //Advances start pointer
                        }
                        else if (T[I] == "~"[0]) // finds Item amount values apended with a ~
                        {
                            string test = (T.Substring(i, (I - i)));
                            //Console.WriteLine(test);            //Debug
                            //Console.WriteLine("{0},{1}", I, i); //Debug
                            Amount = int.Parse(test); // pulls string value followed by a ~

                            i = I + 1; //Advances start pointer
                        }
                        if (T[I] == "|"[0]) // finds route values apended with a |
                        {
                            string test = (T.Substring(i, (I - i)));
                            //Console.WriteLine(test);            //Debug
                            //Console.WriteLine("{0},{1}", I, i); //Debug
                            Move = bool.Parse(test); // pulls string value followed by a |

                            i = I + 1; //Advances start pointer
                        }
                    }

                    int max = Map1[Pos[0], Pos[1], Pos[2]].inventory.GetLength(0); // finds the max inventory size
                    item[] inventory = Map1[Pos[0], Pos[1], Pos[2]].inventory; // sets inventory as a local variable for simplicity

                    for (var I = 0; I < Amount; I++) // adds one itm to the frst null entry in the inventory
                    {
                        for (int index = 0; index < max ; index++) // finds first null entry
                        {
                            if(inventory[index] == null) // null entry
                            {
                                inventory[index] = new item(Name, Dis, Move);
                                break;
                            }
                            else if (inventory[index].equals()) // empty entry
                            {
                                inventory[index] = new item(Name, Dis, Move);
                                break;
                            }

                        }
                    }
                    Map1[Pos[0], Pos[1], Pos[2]].inventory = inventory; // dump the resulting local inventory into the cavern inventory
                }
                // outside if-else for file read
            }
        }
        // More functions
        static void debugCheck()
        {
            for (int x = 0; x < Map1.GetLength(0); x++) //x iterator
            {
                for (int y = 0; y < Map1.GetLength(1); y++) // y iterator
                {
                    for (int z = 0; z < Map1.GetLength(2); z++) // z iterator
                    {
                        if (Map1[x, y, z] != null) // safety
                        {
                            cavern current = Map1[x, y, z]; // set local var current to cavern
                            Console.WriteLine( "{0}, {1}, {2}", current.pos[0], current.pos[1], current.pos[2]); // debug pos
                            Console.WriteLine(current.discribe); // debug discription
                            for (byte w = 0; w < 6; w++) // exit iterator
                            {
                                Console.WriteLine(current.exits[w]); // debug exits
                            }
                        }
                    }
                }
            }
        }
        // Yet more
        static void loadRoom(int x, int y, int z) // current room loader
        {
            cavern current = Map1[x, y, z]; // loads current room into the local variable current


        }

        static cavern[,,] Map1 = new cavern[100, 100, 100]; //define the map

        static void Main(string[] args)
        {
            Load(); // load function
            debugCheck(); // Debug!
            Console.ReadKey(); // pause...
        }
    }
}