using System.Collections.Generic;

namespace CrawlParser
{
    /// <summary>
    /// The normalized results of a .csv translation.
    /// </summary>
    public class ProductsEntity
    {
        /// <summary>
        /// Normalized products.
        /// </summary>
        public List<ProductEntity> Items { get; set; }

        /// <summary>
        /// A unique list of all product specification labels.
        /// </summary>
        public List<string> SpecificationLabels { get; set; }

        /// <summary>
        /// Total number of 
        /// </summary>
        public int MaxCrumbCount { get; set; }
    }
}
