using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace GameDevCompaniesWebApplication
{
    public partial class Subsidiaries
    {
        public Subsidiaries()
        {
            GamesDevelopers = new HashSet<GamesDevelopers>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "The field must be filled")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The field must be filled")]
        public string Location { get; set; }
        public int CompanyId { get; set; }
        [Required(ErrorMessage = "The field must be filled")]
        [Display(Name = "Studio manager")]
        public string ManagerFullName { get; set; }

        public virtual GameDevCompanies Company { get; set; }
        public virtual ICollection<GamesDevelopers> GamesDevelopers { get; set; }
    }
}
