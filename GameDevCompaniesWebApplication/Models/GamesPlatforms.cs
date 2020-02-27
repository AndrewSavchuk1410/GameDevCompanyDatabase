using System;
using System.Collections.Generic;

namespace GameDevCompaniesWebApplication
{
    public partial class GamesPlatforms
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int PlatformId { get; set; }

        public virtual ComputerGames Game { get; set; }
        public virtual Platforms Platform { get; set; }
    }
}
