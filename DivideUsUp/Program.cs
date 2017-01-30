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

            // The Amount of Folders Should not exceed the amount of folders
            if (FolderCount > files.Length)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\nCannot divide files into more folders than there are files\n\nPress AnyKey To Exit...");
                Console.ReadKey();
                Environment.Exit(0);
            }

            // You can't divide the files up into no folders
            if (FolderCount == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\nFolder Count Error\n\nPress AnyKey To Exit...");
                Console.ReadKey();
                Environment.Exit(0);
            }

            
            int filesPerFolder = (int)Math.Floor(files.Length / (double)FolderCount);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n" + filesPerFolder + " Files Per Folder");
            Console.ResetColor();

            // Seperate the Files into groups 
            var groups = from index in Enumerable.Range(0, files.Length) group files[index] by index / filesPerFolder;

            // Keep Track Of Current Moving Direcotory Index
            int folderIndex = 1;
            
            // Don't allow double warning
            bool flag = false;

            foreach (var group in groups)
            {
                string path = CurrentDirectory + "\\" + folderIndex;
                
                // If the files can't fill the next directory fully or if the folderIndex exceeds the user amount
                if (group.ToArray().Length < filesPerFolder || folderIndex == (FolderCount + 1))
                {
                    path = CurrentDirectory + "\\" + (folderIndex - 1);
                    if (!flag)
                    {
                        
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\nCouldn't Be Divided Equally...\nStragglers went to Last Folder");
                        Console.ResetColor();
                        flag = true;
                    }
                    
                } else
                {
                    System.IO.Directory.CreateDirectory(path);
                    folderIndex++;
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
            }
            Console.WriteLine("Press AnyKey to Exit...");
            Console.ReadKey();
        }
    }
}
