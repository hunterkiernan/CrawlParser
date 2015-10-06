using System.Collections.Generic;
using System.ComponentModel;

namespace CrawlParser
{
    /// <summary>
    /// Application logic for product translation from crawl data to normalized data.
    /// </summary>
    public class ProductsLogic
    {
        public enum Categories
        {
            [Description("Plumbing Sinks")]
            SINKS,
            [Description("Plumbing Faucets")]
            FAUCETS
        }

        /// <summary>
        /// Get and return all products in the given .csv file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public ProductsEntity GetProducts(string fileName)
        {
            // Get and return all products found int he .csv file.
            return GetAllProducts(fileName);
        }

        /// <summary>
        /// Create a new instance whose data is filtered to the given category.
        /// </summary>
        /// <param name="fileName">Full path to .csv file.</param>
        /// <param name="category">Category to filter by.</param>
        /// <returns></returns>
        public ProductsEntity GetProducts(string fileName, Categories category = Categories.FAUCETS)
        {
            /*******************************************************
            * CONSTANTS
            * ------------------------------------------------------
            * (none)
            ********************************************************/
            ProductsEntity products; // All products.
            
            // Get all products.
            products = GetAllProducts(fileName);

            // Filter products by category.
            products.Items = products.Items.FindAll(x => x.Category.StartsWith(EnumUtils<Categories>.GetDescription(category)));

            // Return the correct products.
            return products;
        }

        /// <summary>
        /// Write all of the given products to the given .xls file.
        /// </summary>
        /// <param name="fileName">Full path to the .xls file to write to.</param>
        /// <param name="products">The products to be written.</param>
        public void SaveProducts(string fileName, ref ProductsEntity products)
        {
            /*******************************************************
            * CONSTANTS
            * ------------------------------------------------------
            * (none)
            ********************************************************/
            ExcelDAO excelDAO;

            // Write the products to the given file.
            excelDAO = new ExcelDAO(fileName);
            excelDAO.Write(ref products);
        }

        /// <summary>
        /// Get all products contained within the given file.
        /// </summary>
        /// <param name="fileName">Full path to .csv file.</param>
        /// <returns></returns>
        private ProductsEntity GetAllProducts(string fileName)
        {
            /*******************************************************
            * CONSTANTS
            * ------------------------------------------------------
            * (none)
            ********************************************************/
            List<ProductEntity> products;     // Products to be returned.
            int maxCrumbCount;                // Maximum number of crumb segments returned.
            List<string> specificationLabels; // Unique list of all specification labels.
            CsvDAO dao;                       // CSV Data Access Object.

            //  Used to read .csv file.
            dao = new CsvDAO(fileName);

            // Get all products.
            products = dao.Read(out specificationLabels, out maxCrumbCount);

            // Return the correct products.
            return new ProductsEntity()
            {
                Items = products,
                SpecificationLabels = specificationLabels,
                MaxCrumbCount = maxCrumbCount
            };
        }

        
    }
}
