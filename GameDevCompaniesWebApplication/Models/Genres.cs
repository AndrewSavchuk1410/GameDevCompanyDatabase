using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameDevCompaniesWebApplication
{
    public partial class Genres
    {
        public Genres()
        {
            GamesGenres = new HashSet<GamesGenres>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "The field has to be filled")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The field has to be filled")]
        public string Info { get; set; }
        public virtual ICollection<GamesGenres> GamesGenres { get; set; }
    }
}
