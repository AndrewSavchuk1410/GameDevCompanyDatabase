using System;
using System.Collections.Generic;

namespace GameDevCompaniesWebApplication
{
    public partial class GamesDevelopers
    {
        public int Id { get; set; }
        public int SubsidiariesId { get; set; }
        public int GameId { get; set; }

        public virtual ComputerGames Game { get; set; }
        public virtual Subsidiaries Subsidiaries { get; set; }
    }
}
