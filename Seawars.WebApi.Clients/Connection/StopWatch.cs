using System;
using System.Configuration;
using System.Linq;
using System.Timers;
using Microsoft.Extensions.Configuration;
using Seawars.DAL.GamesBase;

namespace Seawars.WebApi.Clients.Connection
{
    public static class StopWatch
    {
        private static Timer timer;
        private static readonly int AvaibleMinutes = 10;

        static StopWatch() => TotalGamesCounnt = 1;

        public static int TotalGamesCounnt { get; set; }
        public static bool IsActive { get; set; }

        public static void StartTimer()
        {
            IsActive = true;

            timer = new Timer(60000);

            timer.Elapsed += (s, e) => Tick();

            timer.Start();
        }

        public static void StopTimer()
        {
            IsActive = false;

            TotalGamesCounnt = 1;

            timer.Elapsed -= (s, e) => Tick();

            timer.Stop();
        }

        private static void Tick()
        {
            var collection = Collection.GetGame();

            for (int i = collection.Games.Keys.OrderBy(x => x).FirstOrDefault(); i <= TotalGamesCounnt; i++)
            {
                if (!collection.Games.ContainsKey(i)) continue;

                if (DateTime.Now - collection.Games[i][1].Timer > new TimeSpan(0, AvaibleMinutes, 0) ||
                    DateTime.Now - collection.Games[i][2].Timer > new TimeSpan(0, AvaibleMinutes, 0))
                    collection.Games.Remove(i);
            }

            if (collection.Games.Keys.Count is 0) StopTimer();

        }

    }
}
