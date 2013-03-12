namespace MySlApi.Core.ScreenScrapers
{
    using System.Collections.Generic;
    using System.Linq;

    using HtmlAgilityPack;

    public class LoginPageScraper
    {
        public LoginPageScraper(string html)
        {
            this.Html = html;
        }

        public string Html { get; set; }

        public List<string[]> ScrapePostParameters()
        {
            var parameters = new List<string[]>();

            var doc = new HtmlDocument();
            doc.LoadHtml(this.Html);

            var inputNodes = doc.DocumentNode.SelectNodes("//input");

            foreach (HtmlNode hiddenField in inputNodes.Where(node => node.GetAttributeValue("type", string.Empty) == "hidden"))
            {
                var name = hiddenField.GetAttributeValue("name", string.Empty);

                if (!string.IsNullOrEmpty(name))
                {
                    parameters.Add(new[] { name, hiddenField.GetAttributeValue("value", string.Empty) });
                }
            }

            return parameters;
        }
    }
}
