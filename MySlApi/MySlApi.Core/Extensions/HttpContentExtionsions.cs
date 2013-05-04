namespace MySlApi.Core.Extensions
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public static class HttpContentExtionsions
    {
        public static async Task<T> ReadJsonAsTypeAsync<T>(this HttpContent content)
        {
            var stringContent = await content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<T>(stringContent);

            return json;
        }

        public static async Task<T> ReadJsonAsAnonymousTypeAsync<T>(this HttpContent content, T anonymousTypeObject)
        {
            var stringContent = await content.ReadAsStringAsync();
            var jsonObject = JsonConvert.DeserializeAnonymousType(stringContent, anonymousTypeObject);

            return jsonObject;
        }
    }
}
