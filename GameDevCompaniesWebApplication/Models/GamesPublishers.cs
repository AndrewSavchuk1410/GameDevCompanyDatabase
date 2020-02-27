using System;
using System.Collections.Generic;

namespace GameDevCompaniesWebApplication
{
    public partial class GamesPublishers
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int GameId { get; set; }

        public virtual GameDevCompanies Company { get; set; }
        public virtual ComputerGames Game { get; set; }
    }
}
