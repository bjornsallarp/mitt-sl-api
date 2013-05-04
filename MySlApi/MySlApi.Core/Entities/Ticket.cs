namespace MySlApi.Core.Entities
{
    using System;

    public class Ticket
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime? ValidFrom { get; set; }

        public DateTime? Expires { get; set; }

        public bool Blocked { get; set; }

        public bool Active { get; set; }
    }
}
