using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameDevCompaniesWebApplication
{
    public partial class GameDevCompanies
    {
        public GameDevCompanies()
        {
            GamesPublishers = new HashSet<GamesPublishers>();
            Subsidiaries = new HashSet<Subsidiaries>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "The field must be filled")]
        [Display(Name = "Company")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The field must be filled")]
        public string Location { get; set; }
        [Required(ErrorMessage = "The field must be filled")]
        [Display(Name = "CEO")]
        public string DirectorFullName { get; set; }

        public virtual ICollection<GamesPublishers> GamesPublishers { get; set; }
        public virtual ICollection<Subsidiaries> Subsidiaries { get; set; }
    }
}
