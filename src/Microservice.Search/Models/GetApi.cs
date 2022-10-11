using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microservice.Search.Models
{
    public class GetApi
    {
        private HttpClient _client;
        public string _url = "https://localhost:44331/";
        // Get Api from BookStore
        public GetApi()
        {
            _client = new HttpClient();
        }

        public async Task<IEnumerable<Book>> GetAllBook()
        {
            var path = Path.Combine(_url, "api", "GetAllbook");
            using var result = _client.GetStringAsync(path);
            var bookList = JsonConvert.DeserializeObject<IEnumerable<Book>>(await result);
            return bookList;
        }

        public async Task<IEnumerable<Book>> GetBookByTitle(string title)
        {
            //Book book = null;
            var path = _url + Path.Combine("api", "getBook", title);
            using var result = _client.GetStringAsync(path);
            var apiResponse = JsonConvert.DeserializeObject <IEnumerable<Book>>(await result);
            return apiResponse;
        }
    }
}