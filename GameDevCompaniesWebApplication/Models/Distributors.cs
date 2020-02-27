using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameDevCompaniesWebApplication
{
    public partial class Distributors
    {
        public Distributors()
        {
            GamesDistributors = new HashSet<GamesDistributors>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "The field must be filled")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The field must be filled")]

        public string Info { get; set; }

        public virtual ICollection<GamesDistributors> GamesDistributors { get; set; }
    }
}
