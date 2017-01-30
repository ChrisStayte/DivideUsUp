using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DivideUsUp
{
    class Program
    {
        static void Main(string[] args)
        {
            string CurrentDirectory = (AppDomain.CurrentDomain.BaseDirectory);
            Console.Write("Type of File ie. (tif or las): ");
            string FileType = Console.ReadLine();

            string[] files = System.IO.Directory.GetFiles(CurrentDirectory, "*." + FileType);

            // Check for files and if none found report back to use and exit program
            if (files.Length <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\nNo Files with '" + FileType + "' extension\n\nPress AnyKey To Exit...");
                Console.ReadKey();
                Environment.Exit(0);
            }


            // Get Amount of Divisions From User
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\n" + files.Length + " Files were found\n\nHow Many Folders: ");
            Console.ResetColor();
            int FolderCount = 0;
            Int32.TryParse(Console.ReadLine(), out FolderCount);

            if (FolderCount > files.Length)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\nCannot divide files into more folders than there are files\n\nPress AnyKey To Exit...");
                Console.ReadKey();
                Environment.Exit(0);
            }

            if (FolderCount == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\nFolder Count Error\n\nPress AnyKey To Exit...");
                Console.ReadKey();
                Environment.Exit(0);
            }

            List<String> MovePaths = new List<String>();
            int filesPerFolder = files.Length / FolderCount;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n" + filesPerFolder + " Files Per Folder");
            Console.ResetColor();


            var groups = from index in Enumerable.Range(0, files.Length) group files[index] by index / filesPerFolder;
            int groupIndex = 1;

            foreach (var group in groups)
            {
                string path = CurrentDirectory + "\\" + groupIndex;
                if (group.ToArray().Length < filesPerFolder)
                {
                    path = CurrentDirectory + "\\" + (groupIndex - 1);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nCouldn't Be Divided Equally...\nStragglers went to Last Folder");
                    Console.ResetColor();
                } else
                {
                    System.IO.Directory.CreateDirectory(path);
                }
                
                

                foreach (var file in group)
                {    
                    try
                    {
                        System.IO.File.Move(file, path + "\\" + System.IO.Path.GetFileName(file));
                    } catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\nMove Error\n" + ex + "\n\nPress AnyKey To Exit...");
                        Console.ReadKey();
                        Environment.Exit(0);
                    }
                    
                }
                groupIndex++;
            }
            Console.WriteLine("Press AnyKey to Exit...");
            Console.ReadKey();
        }
    }
}
