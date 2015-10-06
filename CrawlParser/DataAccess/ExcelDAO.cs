using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Excel = Microsoft.Office.Interop.Excel;

namespace CrawlParser
{
    /// <summary>
    /// Used to write normalized product data to Excel.
    /// </summary>
    public class ExcelDAO
    {
        /// <summary>
        /// The full path to the Excel file.
        /// </summary>
        string _fileName;

        /// <summary>
        /// The columns to be added to the Excel file.
        /// </summary>
        enum Columns
        {
            [Description("Category")]
            CATEGORY = 1,
            [Description("SKU")]
            SKU = 2,
            [Description("Short Description")]
            SHORT_DESC = 3,
            [Description("Price")]
            PRICE = 4,
            [Description("Description")]
            DESC = 5,
            [Description("Product Image Url")]
            IMAGE_URL = 6,
            [Description("Manufacturer Image Url")]
            MANU_IMAGE_URL = 7
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName">Full path to Excel file.</param>
        public ExcelDAO(string fileName)
        {
            _fileName = fileName;
        }

        /// <summary>
        /// Write the product data to Excel file.
        /// </summary>
        /// <param name="products">Products to be written.</param>
        public void Write(ref ProductsEntity products)
        {
            /*******************************************************
            * CONSTANTS
            * ------------------------------------------------------
            * (none)
            ********************************************************/
            Excel.Application excelApp;          // Excel application reference.
            Excel.Workbook wb;                   // Excel workbook reference.
            Excel.Worksheet sh;                  // Excel worksheet reference. 
            int row;                             // Row index.
            int specCount;                       // Product specification count.
            int crumbCount;                      // Crumb segment count.
            Dictionary<string, int> specIndexes; // Specification indexes.
            int columnCount;                     // Total number of columns to be written (minus unknown specification count).
            int columnIndex;                     // Excel file column write index.

            try
            {
                // Fire up Excel!
                excelApp = new Excel.Application();
            }
            catch (Exception ex)
            {
                // Re-throw exception with details.
                throw new Exception("Error creating excel: " + ex.Message);
            }


            // Instantiate / initialize.
            wb = excelApp.Workbooks.Add();
            sh = wb.Sheets.Add();
            row = 1;
            sh.Name = "Data";
            specCount = products.SpecificationLabels.Count();
            specIndexes = new Dictionary<string, int>(specCount);
            crumbCount = products.MaxCrumbCount;
            columnCount = Enum.GetNames(typeof(Columns)).Length;
            columnIndex = columnCount + crumbCount;
                       

            try
            {
                // Iterate over each crumb segment.
                for (int i = 0; i < crumbCount; i++)
                {
                    /*******************************************************
                    * CONSTANTS
                    * ------------------------------------------------------
                    * (none)
                    ********************************************************/
                    string value; // Pretty column labels for the product categories (aka crumb).

                    switch (i)
                    {
                        case 0:
                            value = "Product Category";
                            break;
                        case 1:
                            value = "Product Type";
                            break;
                        default:
                            value = "Product Sub Type";
                            break;
                    }

                    // Write the column header.
                    sh.Cells[row, i + 1].Value2 = value;                        
                }

                // Add the default columns.
                sh.Cells[row, (int)Columns.CATEGORY + crumbCount].Value2 = EnumUtils<Columns>.GetDescription(Columns.CATEGORY);
                sh.Cells[row, (int)Columns.SKU + crumbCount].Value2 = EnumUtils<Columns>.GetDescription(Columns.SKU);
                sh.Cells[row, (int)Columns.SHORT_DESC + crumbCount].Value2 = EnumUtils<Columns>.GetDescription(Columns.SHORT_DESC);
                sh.Cells[row, (int)Columns.PRICE + crumbCount].Value2 = EnumUtils<Columns>.GetDescription(Columns.PRICE);
                sh.Cells[row, (int)Columns.DESC + crumbCount].Value2 = EnumUtils<Columns>.GetDescription(Columns.DESC);
                sh.Cells[row, (int)Columns.IMAGE_URL + crumbCount].Value2 = EnumUtils<Columns>.GetDescription(Columns.IMAGE_URL);
                sh.Cells[row, (int)Columns.MANU_IMAGE_URL + crumbCount].Value2 = EnumUtils<Columns>.GetDescription(Columns.MANU_IMAGE_URL);

                // Iterate over all unique product specification labels and add as column header.
                foreach (var item in products.SpecificationLabels)
                {
                    sh.Cells[row, ++columnIndex].Value2 = item;
                    specIndexes.Add(item, columnIndex);
                }

                // Move to first data row.
                row++;

                // Iterate over all products and write to file.
                foreach (var product in products.Items)
                {
                    // Write all crumb segments. 
                    for (int i = 0; i < product.Crumbs.Count; i++)
                    {
                        sh.Cells[row, i + 1].Value2 = product.Crumbs[i];
                    }

                    // Write standard data.
                    sh.Cells[row, (int)Columns.CATEGORY + crumbCount].Value2 = product.Category;
                    sh.Cells[row, (int)Columns.SKU + crumbCount].Value2 = product.SKU;
                    sh.Cells[row, (int)Columns.SHORT_DESC + crumbCount].Value2 = product.ShortDescription;
                    sh.Cells[row, (int)Columns.PRICE + crumbCount].Value2 = product.Price;
                    sh.Cells[row, (int)Columns.DESC + crumbCount].Value2 = product.Description;
                    sh.Cells[row, (int)Columns.IMAGE_URL + crumbCount].Value2 = product.ImageUrl;
                    sh.Cells[row, (int)Columns.MANU_IMAGE_URL + crumbCount].Value2 = product.ManufacturerImageUrl;

                    // Iterate over all of the product's specifications and write to the correct column.
                    foreach (var item in product.Specifications)
                    {
                        /*******************************************************
                        * CONSTANTS
                        * ------------------------------------------------------
                        * (none)
                        ********************************************************/
                        int index; // Index of the cell.
                        
                        // Get the index of the column that the specification should be written to.
                        index = specIndexes[item.Label];

                        // Write to the correct cell.
                        sh.Cells[row, index].Value2 = item.Value; 
                    }

                    // Move to next data row.
                    row++;
                }
            }
            catch (Exception ex)
            {
                // Re-throw with details.
                throw new Exception("Excel write error: " + ex.Message);
            }
            finally
            {
                // Save the file and clean up.s
                wb.SaveAs(_fileName);
                wb.Close();
                excelApp.Quit();
            }
        }
    }
}
