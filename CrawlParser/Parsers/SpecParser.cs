using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CrawlParser
{
    /// <summary>
    /// Parse out the specifications information associated to a product.
    /// </summary>
    public class SpecParser
    {
        /// <summary>
        /// Parse the given string which contains html content tags with delimited values.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public List<SpecificationEntity> Parse(string input, char delimiter = ':')
        {
            /********************************************************
            * CONSTANTS
            * ------------------------------------------------------
            * (none)
            *******************************************************/
            List<SpecificationEntity> specs;
            Regex regex;
            MatchCollection matches;


            specs = new List<SpecificationEntity>();
            regex = new Regex(@"(?<=\bcontent="")[^""]*");
            matches = regex.Matches(input);

            foreach (Match item in matches)
            {
                /********************************************************
                 * CONSTANTS
                 * ------------------------------------------------------
                 * (none)
                 *******************************************************/
                string value;
                string[] values; // The key and value contained within the content tag.

                value = item.Value;

                if (value.Contains(delimiter))
                {
                    // Splie the value of the content tag on its delimiter to get the key and value.
                    values = item.Value.Split(delimiter);

                    // Add the values to the return collection.
                    specs.Add(new SpecificationEntity(values[0], values[1]));
                }
                else
                {
                    throw new Exception("Specification match did not contain a delimiter.");
                }
            }

            // Sort the specs by their type.
            specs = specs.OrderBy(x => (int)x.Spec_Type).ThenBy(x => x.Label).ToList();

            // Return all specs.
            return specs;
        }
    }
}
