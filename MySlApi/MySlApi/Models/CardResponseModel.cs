namespace MySlApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    using MySlApi.Core.Entities;

    [DataContract]
    public class CardResponseModel
    {
        public CardResponseModel(AccessCard card)
        {
            this.Balance = card.Balance;
            this.Name = card.Name;
            this.CardNumber = card.CardNumber;
            this.Tickets = card.Tickets.Select(ticket => new TicketModel(ticket)).ToList();
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public double Balance { get; set; }

        [DataMember]
        public int CardNumber { get; set; }

        [DataMember]
        public List<TicketModel> Tickets { get; set; }
    }

    [DataContract]
    public class TicketModel
    {
        public TicketModel(Ticket ticket)
        {
            this.Name = ticket.Name;
            this.Price = ticket.Price;
            this.ValidFrom = ticket.ValidFrom;
            this.Expires = ticket.Expires;
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public double Price { get; set; }

        [DataMember]
        public DateTime ValidFrom { get; set; }

        [DataMember]
        public DateTime Expires { get; set; }
    }
}