using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Linq;

namespace CrawlParser
{
    /// <summary>
    /// Read product records from a .csv file.
    /// </summary>
    public class CsvDAO
    {
        /// <summary>
        /// The full path to the .csv file to read.
        /// </summary>
        private string _file;

        /// <summary>
        /// The fields in the csv file.
        /// </summary>
        private enum Fields
        {
            INPUT,
            RESULT_NUMBER,
            WIDGET,
            SOURCE,
            RESULT_ROW_NUMBER,
            SOURCE_PAGE_URL,
            CRUMB,
            SKU,
            SHORT_DESC,
            IMAGE,
            IMAGE_SOURCE,
            IMAGE_TITLE,
            IMAGE_ALT,
            PRICE,
            PRICE_SOURCE,
            PRICE_CURRENCY,
            PRICE_TITLE,
            PRICE_TEXT,
            MANU_IMAGE,
            MANU_IMAGE_SOURCE,
            MANU_IMAGE_TITLE,
            MANU_IMAGE_ALT,
            SPECS_RAW,
            DESC,
            PDF_LINK,
            CRUMB_RAW   
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="file"></param>
        public CsvDAO(string file)
        {
            _file = file;
        }

        /// <summary>
        /// Read all data in the given file.
        /// </summary>
        /// <param name="specificationLabels">Unique list of product specification labels.</param>
        /// <param name="maxCrumbCount">Maximum crumb segment count in the crawl results.</param>
        /// <returns></returns>
        public List<ProductEntity> Read(out List<string> specificationLabels, out int maxCrumbCount)
        {
            /*******************************************************
            * CONSTANTS
            * ------------------------------------------------------
            * (none)
            ********************************************************/
            List<ProductEntity> products; // The normalized products (returned).
            SpecParser specParser;        // Parse a product's specifications.
            DataTable data;               // Raw data result from file read.
            StringBuilder specs_raw;      // The raw product specifications.
            HashSet<string> uniqueSpecs;  // Every, unique, product specification.
            CrumbParser crumbParser;      // Used to parse the crumb (Product categories, sub categories).

            // Instantiate / initialize.
            specParser = new SpecParser();
            crumbParser = new CrumbParser();
            products = new List<ProductEntity>();
            specs_raw = new StringBuilder();
            uniqueSpecs = new HashSet<string>();
            maxCrumbCount = 0;

            try
            {
                // Read all data from the file.
                data = ReadFile();
            }
            catch (Exception ex)
            {
                // Re-throw exception.
                throw new Exception("Data read error. " + ex.Message);
            }

            // Iterate over each product record.
            foreach (DataRow row in data.Rows)
            {
                // Populate product from data.
                ProductEntity product = new ProductEntity();
                product.Description = ReplaceFirst(row[(int)Fields.DESC].ToString(), "Description ");
                product.SKU = row[(int)Fields.SKU].ToString();
                product.Price = ParseCurrency(row[(int)Fields.PRICE].ToString());
                product.ShortDescription = row[(int)Fields.SHORT_DESC].ToString();
                product.ImageUrl = row[(int)Fields.IMAGE].ToString();
                product.ManufacturerImageUrl = row[(int)Fields.MANU_IMAGE].ToString();
                product.Category = ReplaceFirst(row[(int)Fields.CRUMB].ToString(), "Home ");

                try
                {
                    /*******************************************************
                    * CONSTANTS
                    * ------------------------------------------------------
                    * (none)
                    ********************************************************/
                    int count; // Total number of crumb segments. 

                    // Parse out the crumb segments.
                    product.Crumbs = crumbParser.Parse(row[(int)Fields.CRUMB_RAW].ToString());

                    // Get the total number of segments. 
                    count = product.Crumbs.Count;

                    // If this product's crumb segments is larger then the current max then overwrite max.
                    if (count > maxCrumbCount) maxCrumbCount = count;
                }
                catch (Exception exp)
                {                    
                    // Re-throw error with additional details for debugging.
                    throw new Exception("Error parsing crumb. " + exp.Message);
                }

                try
                {
                    // Attempt to parse the specification data.
                    product.Specifications = specParser.Parse(row[(int)Fields.SPECS_RAW].ToString());
                }
                catch (Exception exp)
                {
                    // Re-throw error with additional details for debugging.
                    throw new Exception("Error parsing specifications. " + exp.Message);
                }

                // Iterate over all product specifications adding to unique list.
                foreach (var spec in product.Specifications) uniqueSpecs.Add(spec.Label);    

                // Add product to return collection.
                products.Add(product);
            }

            // Convert the specifications to a list for future, index based, lookups - sort alphabetically for output.
            specificationLabels = uniqueSpecs.ToList();
            specificationLabels.Sort();

            // Return all products.
            return products;
        }

        /// <summary>
        /// Parse a currency value stored as a string.
        /// </summary>
        /// <param name="money">The string containing the money.</param>
        /// <param name="symbol">The currency symbol displayed before the money.</param>
        /// <returns>Returns 0.0 on failure.</returns>
        private double ParseCurrency(string money, string symbol = "$")
        {
            /*******************************************************
            * CONSTANTS
            * ------------------------------------------------------
            * (none)
            ********************************************************/
            double price; // The parsed price (return value).

            // Replace the currency symbol.
            money = ReplaceFirst(money, symbol);

            // Attempt to parse the remaining money value.
            if (!Double.TryParse(money, out price))
            {
                // If the parse fails then return a valid price.
                price = 0.0;
            }

            // Return the price.
            return price;
        }

        /// <summary>
        /// Replace the first letter in the input.
        /// </summary>
        /// <param name="input">The string to be updated.</param>
        /// <param name="replace">What to be removed from the beginning of the string.</param>
        /// <param name="newString">Optional, replace with, string.</param>
        /// <returns></returns>
        private string ReplaceFirst(string input, string replace, string newString = "")
        {
            /*******************************************************
            * CONSTANTS
            * ------------------------------------------------------
            * (none)
            ********************************************************/
            int pos; // The position of the to-be-replaced string.

            // Get the position of the replace text.
            pos = input.IndexOf(replace);

            // Was the replace value not found?
            if (pos < 0)
            {
                return input;
            }

            // Recreate the string with the to-be-replaced text omitted and the replace text added in place (if any).
            return input.Substring(0, pos) + newString + input.Substring(pos + replace.Length);
        }

        /// <summary>
        /// Read the 
        /// </summary>
        /// <param name="firstRowIsHeaders"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        private DataTable ReadFile(bool firstRowIsHeaders = true, char delimiter = ',')
        {
            /*******************************************************
            * CONSTANTS
            * ------------------------------------------------------
            * (none)
            ********************************************************/
            int index = 0;  // LCV.
            DataTable data; // The data read from the file.

            
            data = new DataTable();

            // Open the file.
            using (StreamReader reader = new StreamReader(File.OpenRead(_file)))
            {
                // Iterate through the file.
                while (!reader.EndOfStream) 
                {
                    /*******************************************************
                    * CONSTANTS
                    * ------------------------------------------------------
                    * (none)
                    ********************************************************/
                    string line;     // The currently being read line.
                    string[] values; // The parsed, delimited, line values.

                    // Read the line.
                    line = reader.ReadLine();

                    // Parse the line on the delimiter.
                    values = line.Split(delimiter);

                    // Is this the first line?
                    if (index != 0)
                    {
                        // Add data.
                        data.Rows.Add(values);
                    }
                    else if (firstRowIsHeaders)
                    {
                        /*******************************************************
                        * CONSTANTS
                        * ------------------------------------------------------
                        * (none)
                        ********************************************************/
                        int columnCount; // Total number of columns in the .csv file.

                        // Get total number of data table columns.
                        columnCount = values.Length;

                        // Configure columns needed by the data table. Rows can be added
                        //   instead on all subsequent loops.
                        for (int i = 0; i < columnCount; i++)
                        {
                            data.Columns.Add(values[i]);
                        }
                    }

                    // Increment index.
                    index++;
                }
            }

            return data;
        }
    }
}
