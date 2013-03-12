namespace MySlApi.Core.Entities
{
    using System;

    public class Ticket
    {
        public string Name { get; set; }

        public double Price { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime Expires { get; set; }
    }
}
