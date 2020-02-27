using System;
using System.Collections.Generic;

namespace GameDevCompaniesWebApplication
{
    public partial class GamesGenres
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public int GenreId { get; set; }

        public virtual ComputerGames Game { get; set; }
        public virtual Genres Genre { get; set; }
    }
}
