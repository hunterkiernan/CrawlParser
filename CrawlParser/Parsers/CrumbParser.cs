using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;

namespace CrawlParser
{
    /// <summary>
    /// Parse out the crumb information associated to a product.
    /// </summary>
    public class CrumbParser
    {
        /// <summary>
        /// Parse the given string which contains html content tags with delimited values.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public List<string> Parse(string input, char delimiter = ':')
        {
            List<string> crumbs = new List<string>();
            var doc = new HtmlDocument();
            doc.LoadHtml(input);
            
            var linksOnPage = (from link in doc.DocumentNode.Descendants()
                               where link.Name == "a"
                               select link.InnerText).ToList();

            // Remove the "Home" value.
            linksOnPage.RemoveAt(0);

            // Return.
            return crumbs = linksOnPage;
        }
    }
}
