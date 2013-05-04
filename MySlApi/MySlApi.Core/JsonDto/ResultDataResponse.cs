namespace MySlApi.Core.JsonDto
{
    using Newtonsoft.Json;

    internal class ResultDataResponse<T>
    {
        [JsonProperty(PropertyName = "result_data")]
        public T Result { get; set; }
    }
}
