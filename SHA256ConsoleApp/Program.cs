using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Security.Cryptography;


namespace SHA256ConsoleApp
{
    class Program
    {
        [STAThreadAttribute]
        static void Main(string[] args)
        {

            

            try
            {
                //
                byte[] bytes = Encoding.Unicode.GetBytes("adim");
                //
                SHA256Managed hashstring = new SHA256Managed();
                // 
                byte[] hash = hashstring.ComputeHash(bytes);
                //
                string hashString = string.Empty;
                //pour chaque hash afficher le resultat
                foreach (byte x in hash)
                {
                    hashString += String.Format("{0:x2}", x);   
                }
                //afficher le resultat
                Console.WriteLine(hashString);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("Error: The directory specified could not be found.");
            }
            catch (IOException)
            {
                Console.WriteLine("Error: A file in the directory could not be accessed.");
            }
            Console.ReadLine();
        }
    }
}
