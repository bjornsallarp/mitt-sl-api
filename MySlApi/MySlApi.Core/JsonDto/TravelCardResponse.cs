namespace MySlApi.Core.JsonDto
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    internal class TravelCardResponse
    {
        [JsonProperty(PropertyName = "travel_card")]
        internal TravelCardResponseBaseInfo TravelCard { get; set; }

        internal class TravelCardResponseBaseInfo
        {
            [JsonProperty(PropertyName = "serial_number")]
            public string SerialNumber { get; set; }

            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "blocked")]
            public bool Blocked { get; set; }

            [JsonProperty(PropertyName = "blocked_date")]
            public DateTime? BlockedDate { get; set; }

            [JsonProperty(PropertyName = "detail")]
            public TravelCardResponseDetails Details { get; set; }

            [JsonProperty(PropertyName = "products")]
            public List<TravelCardResponseProduct> Products { get; set; }
        }

        internal class TravelCardResponseProduct
        {
            [JsonProperty(PropertyName = "blocked")]
            public bool Blocked { get; set; }

            [JsonProperty(PropertyName = "start_date")]
            public DateTime? StartDate { get; set; }

            [JsonProperty(PropertyName = "end_date")]
            public DateTime? EndDate { get; set; }

            [JsonProperty(PropertyName = "active")]
            public bool Active { get; set; }

            [JsonProperty(PropertyName = "product_price")]
            public decimal ProductPrice { get; set; }

            [JsonProperty(PropertyName = "product_type")]
            public string ProductType { get; set; }

            [JsonProperty(PropertyName = "product_id")]
            public int ProductId { get; set; }
        }

        internal class TravelCardResponseDetails
        {
            [JsonProperty(PropertyName = "card_status")]
            public string Status { get; set; }

            [JsonProperty(PropertyName = "expire_date")]
            public DateTime? ExpireDate { get; set; }

            [JsonProperty(PropertyName = "purse_value")]
            public decimal PurseValue { get; set; }

            [JsonProperty(PropertyName = "purse_blocked")]
            public bool PurseBlocked { get; set; }
        }
    }
}
