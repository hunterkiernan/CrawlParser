using System;

namespace CrawlParser
{
    /// <summary>
    /// An application used to parse the results of a ferguson public site crawl performed by
    /// import.io
    /// 
    /// 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            /*******************************************************
            * CONSTANTS
            * ------------------------------------------------------
            * (none)
            ********************************************************/
            string openPath;             // Path to the csv file to open.
            string savePath;             // Path to save the xls file to.
            ProductsLogic logic;         // Logic for parsing the csv file to xls.
            ProductsEntity products; // The collection of products to be read/writtin.

            // Get the full path to the csv file
            Console.WriteLine("Read path to .csv crawl file:");
            openPath = Console.ReadLine();

            // If path not provided then exit.
            if (openPath.Length == 0) return;

            // Read the data.
            Console.WriteLine("Working...");
            logic = new ProductsLogic();
            products = logic.GetProducts(openPath);
            Console.WriteLine("Read Done (" + products.Items.Count + ")!");

            // Get the xls save path.
            Console.WriteLine("Save path to .xls file:");
            savePath = Console.ReadLine();
            
            // Save the xls file.
            Console.WriteLine("Saving...");
            logic.SaveProducts(savePath, ref products);
            Console.WriteLine("Save Complete! " + savePath);

            // Keep the console open.
            Console.Read();
        }
    }
}
