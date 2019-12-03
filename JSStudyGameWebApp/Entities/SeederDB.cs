using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JSStudyGameWebApp.Entities
{
    public class SeederDB
    {
        private static void SeedCountries(AppDbContext context)
        {
            Player admin = new Player
            {
                Login = "StepanSm",
                Email = "ivanna.pugaiko@gmail.com",
                Password = "Stepan123456",
                IsAdmin = true
            };
            var player = context.Players.SingleOrDefault(p => p.Login == admin.Login);
            if (player == null)
            {
                context.Add(admin);
                context.SaveChanges();
            }
            else
                admin.Id = player.Id;

            var playerAddInfo = context.PlayersAdditionalInfo.SingleOrDefault(p => p.IdPlayerAdditionalInfo == admin.Id);

            PlayerAdditionalInfo adminAddInfo = new PlayerAdditionalInfo
            {
                IdPlayerAdditionalInfo = admin.Id,
                Name = "Stepan",
                Surname = "Smetanskyy",
                Photo = "stepanPhoto.jpg",
                BirthDate = new DateTime(1988, 4, 24),
                Gender = true
            };

            if (playerAddInfo == null)
            {
                context.Add(adminAddInfo);
                context.SaveChanges();
            }
        }
        public static void SeedData(IServiceProvider services, IHostingEnvironment env, IConfiguration config)
        {
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                SeederDB.SeedCountries(context);
            }
        }
    }
}
