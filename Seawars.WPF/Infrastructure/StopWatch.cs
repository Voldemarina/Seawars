using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Newtonsoft.Json;
using Seawars.Infrastructure.Data;

namespace Seawars.WPF.Infrastructure
{
    public static class StopWatch
    {
        public static event Action UpdateGameState;
        private static Timer timer;
        private static string Path = ConnectionStrings.ApiPath;
        public static void StartTimer()
        {
            timer = new Timer(500);

            timer.Elapsed += (s, e) => Tick();

            timer.Start();
        }

        public static void StopTimer()
        {
            timer?.Stop();
        }

        private async static void Tick()
        {
            var response = GetGameState();

            var game = JsonConvert.DeserializeObject<GameState>(response);

            GameState.GetState(GameState.GetState().CurrentUserIsHost, game);

            await Task.Run(() => UpdateGameState?.Invoke());
        }

        private static string GetGameState()
        {
            var content = new HttpRequestMessage(HttpMethod.Get, Path + "gamestate");

            content.Headers.Add("Id", GameState.GetState().CryptedGameId);

            var request = new HttpClient().SendAsync(content);

            return request.Result.Content.ReadAsStringAsync().Result.ToString();

        }
    }
}
