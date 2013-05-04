namespace MySlApi.Core.JsonDto
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    internal class TravelCardListResponse
    {
        [JsonProperty(PropertyName = "travel_card_list")]
        public List<AccessCardListResponseTravelCardItem> Cards { get; set; }

        internal class AccessCardListResponseTravelCardItem
        {
            [JsonProperty(PropertyName = "travel_card")]
            public AccessCardListResponseTravelCard Card { get; set; }
        }

        internal class AccessCardListResponseTravelCard
        {
            [JsonProperty(PropertyName = "serial_number")]
            public string SerialNumber { get; set; }

            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "href")]
            public string Href { get; set; }
        }
    }
}
