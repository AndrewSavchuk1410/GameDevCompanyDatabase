using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameDevCompaniesWebApplication
{
    public partial class Platforms
    {
        public Platforms()
        {
            GamesPlatforms = new HashSet<GamesPlatforms>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "The field has to be filled")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The field has to be filled")]

        public string Info { get; set; }

        public virtual ICollection<GamesPlatforms> GamesPlatforms { get; set; }
    }
}
