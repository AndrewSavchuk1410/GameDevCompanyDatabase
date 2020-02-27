using System;
using System.Collections.Generic;

namespace GameDevCompaniesWebApplication
{
    public partial class GamesDistributors
    {
        public int Id { get; set; }
        public int DistributorId { get; set; }
        public int GameId { get; set; }

        public virtual Distributors Distributor { get; set; }
        public virtual ComputerGames Game { get; set; }
    }
}
