namespace MySlApi.Core.Entities
{
    using System;
    using System.Collections.Generic;

    public class AccessCard
    {
        public AccessCard()
        {
            this.Tickets = new List<Ticket>();
        }

        public string Name { get; set; }

        public string CardNumber { get; set; }

        public decimal PurseBalance { get; set; }

        public bool PurseBlocked { get; set; }

        public bool Blocked { get; set; }

        public DateTime? BlockedAt { get; set; }

        public DateTime? ExpireDate { get; set; }

        public string CardStatus { get; set; }

        public List<Ticket> Tickets { get; set; }
    }
}
