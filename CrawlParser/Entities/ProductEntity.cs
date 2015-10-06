using System;
using System.Collections.Generic;

namespace CrawlParser
{
    /// <summary>
    /// Represents a normalized product.
    /// </summary>
    public class ProductEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public string SKU { get; set; }

        /// <summary>
        /// Short description.
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// Detail description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Product specifications (Ex. Ada Compliant).
        /// </summary>
        public List<SpecificationEntity> Specifications { get; set; }

        /// <summary>
        /// Price (currency).
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Url to product image (one).
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// The product's category as derived from product crumb.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Manufacturer's logo image url.
        /// </summary>
        public string ManufacturerImageUrl { get; set; }

        /// <summary>
        /// The entire crumb (aka product category and subcategories).
        /// </summary>
        public List<string> Crumbs { get; set; }

        /// <summary>
        /// Write the product for testing purposes.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0}, {1}", SKU, Price.ToString());
        }

    }
}
