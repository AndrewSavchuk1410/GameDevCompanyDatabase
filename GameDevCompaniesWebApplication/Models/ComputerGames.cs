using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameDevCompaniesWebApplication
{
    public partial class ComputerGames
    {
        public ComputerGames()
        {
            GamesDevelopers = new HashSet<GamesDevelopers>();
            GamesDistributors = new HashSet<GamesDistributors>();
            GamesGenres = new HashSet<GamesGenres>();
            GamesPlatforms = new HashSet<GamesPlatforms>();
            GamesPublishers = new HashSet<GamesPublishers>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "The field has to be filled")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The field has to be filled")]
        //[Display(Name = "Budget of Computer Game")]
        public decimal? Budget { get; set; }
        [Required(ErrorMessage = "The field has to be filled")]
        public string Engine { get; set; }

        public virtual ICollection<GamesDevelopers> GamesDevelopers { get; set; }
        public virtual ICollection<GamesDistributors> GamesDistributors { get; set; }
        public virtual ICollection<GamesGenres> GamesGenres { get; set; }
        public virtual ICollection<GamesPlatforms> GamesPlatforms { get; set; }
        public virtual ICollection<GamesPublishers> GamesPublishers { get; set; }
    }
}
