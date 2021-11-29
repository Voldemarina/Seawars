using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Seawars.WPF.Infrastructure
{
    public class HttpRequest
    {
        public static string Get(string Path, string parametrs)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, Path + parametrs);

            request.Headers.Add("Id", GameState.GetState().CryptedGameId);

            var response = new HttpClient().SendAsync(request);

            return response.Result.Content.ReadAsStringAsync().Result.ToString();
        }
    }
}
