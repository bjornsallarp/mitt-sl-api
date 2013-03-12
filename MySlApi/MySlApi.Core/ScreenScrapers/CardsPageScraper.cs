namespace MySlApi.Core.ScreenScrapers
{
    using System;
    using System.Collections.Generic;

    using HtmlAgilityPack;

    using MySlApi.Core.Entities;

    public class CardsPageScraper
    {
        public CardsPageScraper(string html)
        {
            this.Html = html;
        }

        public string Html { get; set; }

        public List<AccessCard> ScrapeAllCards()
        {
            var cards = new List<AccessCard>();

            var doc = new HtmlDocument();
            doc.LoadHtml(this.Html);

            var tableRows = doc.DocumentNode.SelectNodes("//table[@id = 'UserCardsActive']/tr");

            if (tableRows == null || tableRows.Count == 0)
            {
                return cards;
            }

            for (int i = 0; i < tableRows.Count; i += 2)
            {
                var nameRow = tableRows[i];
                var informationRow = tableRows[i + 1];

                var card = ParseCard(nameRow, informationRow);
                if (card != null)
                {
                    cards.Add(card);
                }
            }

            return cards;
        }

        private static AccessCard ParseCard(HtmlNode nameRow, HtmlNode informationRow)
        {
            var nameLink = nameRow.SelectSingleNode(nameRow.XPath + "//a");
            var ticketNodes = informationRow.SelectNodes(informationRow.XPath + "//li");

            if (nameLink != null && ticketNodes != null)
            {
                var nameText = nameLink.InnerText.Trim();
                var nameContent = nameText.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                int cardNumber;

                if (nameContent.Length >= 2 && int.TryParse(nameContent[0].Trim(), out cardNumber))
                {
                    var cardName = nameContent[1].Trim();
                    var parsedCard = new AccessCard { Name = cardName, CardNumber = cardNumber };

                    foreach (var ticketNode in ticketNodes)
                    {
                        var divs = ticketNode.SelectNodes(ticketNode.XPath + "//div");

                        if (divs.Count < 3)
                        {
                            continue;
                        }

                        var title = divs[0].InnerText;
                        var validDatesString = divs[1].InnerText;
                        var amountString = divs[2].InnerText;

                        if (title.Contains("Belopp"))
                        {
                            var balanceString = amountString.Replace("kr", string.Empty).Replace("Belopp:", string.Empty).Trim();

                            double cardBalance;
                            if (double.TryParse(balanceString, out cardBalance))
                            {
                                parsedCard.Balance = cardBalance;
                            }
                        }
                        else if (title.Contains("Biljett"))
                        {
                            var ticket = ParseTicket(title, validDatesString, amountString);
                            parsedCard.Tickets.Add(ticket);
                        }
                    }

                    return parsedCard;
                }
            }

            return null;
        }

        private static Ticket ParseTicket(string title, string validDatesString, string amountString)
        {
            var ticketName = title.Replace("Biljett:", string.Empty).Trim().Replace("  ", " ");
            var ticket = new Ticket { Name = ticketName };

            validDatesString = validDatesString.Replace("Giltighet:", string.Empty).Trim();
            var dates = validDatesString.Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries);

            if (dates.Length == 2)
            {
                ticket.ValidFrom = DateTime.Parse(dates[0].Trim());
                ticket.Expires = DateTime.Parse(dates[1].Trim());
            }

            var priceString = amountString.Replace("kr", string.Empty).Replace("Pris:", string.Empty).Trim();

            double price;
            if (double.TryParse(priceString, out price))
            {
                ticket.Price = price;
            }

            return ticket;
        }
    }
}
