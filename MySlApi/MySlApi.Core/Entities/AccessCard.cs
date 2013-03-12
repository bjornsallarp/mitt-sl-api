namespace MySlApi.Core.Entities
{
    using System.Collections.Generic;

    public class AccessCard
    {
        public AccessCard()
        {
            this.Tickets = new List<Ticket>();
        }

        public string Name { get; set; }

        public int CardNumber { get; set; }

        public double Balance { get; set; }

        public List<Ticket> Tickets { get; set; }
    }
}
