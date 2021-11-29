using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Seawars.DAL.Context;
using Seawars.Domain.Entities;
using Seawars.Domain.Enums;

namespace Seawars.Console
{

    class Connection
    {
        private static IHost _Hosting;

        public static IHost Hosting => _Hosting ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        public static IServiceProvider Services = Hosting.Services;

        private static IHostBuilder CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureServices(ConfigureServices);

        private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {

            services.AddDbContext<MsSqlContext>(x =>
                x.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Seawars;Trusted_Connection=True", o =>
                    o.MigrationsAssembly("Seawars.DAL.SqlServer")));
        }
    }

    class Program
    {
        static void Main(string[] args)//Выяснить проблему с айдишкой(при удалении записи айди продолжает инкриментиться с той итерации что и была до удаления)
        {                              //Как сделать систему оплаты?) при премиум аккаунте можна смотреть статистику других 
           
            var context = Connection.Hosting.Services.GetRequiredService<MsSqlContext>();
            //var a = context.Steps.Where(x => x.Id >= 31).Select(x => x).ToList();
            //context.Steps.RemoveRange(a);
            //context.SaveChanges();
            // var user = new User()
            // {
            //     Name = "Anton",
            //     UserName = "Anton",
            //     Password = "0601",
            //     CountOfWonGames = 1,
            //     GamesWithComputer = 1,
            //     TotalGamesCount = 2,

            // };

            // context.Users.Add(user);
            //// var user = context.Users.FirstOrDefault(x => x.Name == "Anton");
            // var game = new Game();

            // game.User = user;
            // game.Steps = new List<Step>()
            // {

            //     new Step()
            //     {
            //         X = 5,
            //         Y = 6,
            //         Game =game,
            //         Move = (Move)2
            //     },
            //     new Step()
            //     {
            //         X = 7,
            //         Y = 8,
            //         Game =game,
            //         Move = (Move)1
            //     },
            //     new Step()
            //     {
            //         X = 9,
            //         Y = 5,
            //           Game =game,
            //           Move = (Move)2
            //     }, 
            //     new Step()
            //     {
            //         X = 9,
            //         Y = 4,
            //           Game =game,
            //           Move = (Move)2
            //     }

            // };

            // var game1 = new Game();

            // game1.User = user;
            // game1.Steps = new List<Step>()
            // {

            //     new Step()
            //     {
            //         X = 4,
            //         Y = 4,
            //         Game =game,
            //         Move = (Move)1
            //     },
            //     new Step()
            //     {
            //         X = 1,
            //         Y = 1,
            //         Game =game,
            //         Move = (Move)2
            //     },
            //     new Step()
            //     {
            //         X = 2,
            //         Y = 2,
            //         Game =game,
            //         Move = (Move)1
            //     },
            //     new Step()
            //     {
            //         X = 3,
            //         Y = 3,
            //         Game =game,
            //         Move = (Move)2
            //     }

            // };
            // context.Games.Add(game);
            // context.Games.Add(game1);


            // context.SaveChanges();





            //var Game = context.Games.FirstOrDefault(x => x.Id == 4);
            //context.Steps.Add(new Step() { X = 3, Y = 9, Game = Game });
            //context.SaveChanges();


            //var user = context.Users.FirstOrDefault(x => x.UserName == "Anton");
            //var game = context.Games.Include(x => x.Steps).FirstOrDefault(x => x.User.Id == user.Id);
            //var steps = context.Steps.Where(x => x.Id >= 1).Select(x => x);
            //context.Steps.RemoveRange(steps);
            //context.Games.Remove(game);
            //context.Users.Remove(user);
            //context.SaveChanges();



            //var user = context.Users.FirstOrDefault(x => x.UserName == "Vova");
            //var game = context.Games.Include(x => x.Steps).FirstOrDefault(x => x.Id == 10);

            //context.Users.Remove(user);
            //context.Games.Remove(game);
            //context.SaveChanges();
        }
    }
}
