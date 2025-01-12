using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;

namespace CommandsService.Data
{
    public static class PrepDb
    {
        public static void prepPopulation(IApplicationBuilder app)
        {
            using (var serverScope = app.ApplicationServices.CreateScope())
            {
                var grpcClient = serverScope.ServiceProvider.GetService<IPlatformDataClient>();
                var platform = grpcClient.ReturnAllPlatforms();

                SeedData(serverScope.ServiceProvider.GetService<ICommandRepo>(), platform);
            }
        }

        private static void SeedData (ICommandRepo repo, IEnumerable<Platform> platforms)
        {
            Console.WriteLine($"--> Seeding new platform...");
            foreach (var plat in platforms)
            {
                if (!repo.ExternalPlatformsExits(plat.ExternalID))
                {
                    repo.CreatePlatform(plat);
                }
                repo.SaveChange();
            }
        }
    }
}
