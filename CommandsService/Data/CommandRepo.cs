using CommandsService.Models;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepo
    {
        private readonly AppDbContext _context;
        public CommandRepo(AppDbContext context) 
        {
            _context = context;
        }
        public void CreateCommad(int platformId, Command commad)
        {
            if (commad == null)
            {
                throw new ArgumentNullException(nameof(commad));
            }
            commad.PlatformId = platformId;
            _context.commands.Add(commad);
        }

        public void CreatePlatform(Platform platform)
        {
            if (platform == null) throw new ArgumentNullException(nameof(platform));
            _context.platforms.Add(platform);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.platforms.ToList();
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _context.commands
                .Where(x => x.PlatformId == platformId && x.Id == commandId).FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsForPlatform(int platformId)
        {
            return _context.commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.Platform.Name);
        }

        public bool PlatformsExits(int platformId)
        {
            return _context.platforms.Any(p => p.Id == platformId);
        }

        public bool SaveChange()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
