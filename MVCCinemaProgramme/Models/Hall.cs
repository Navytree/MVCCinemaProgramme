using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace MVCCinemaProgramme.Models
{
    public class Hall
    {
        public int Id { get; set; }

        [Display(Name = "Hall's number")]
        public string Name { get; set; }
        public virtual ICollection<Seat>? Seats { get; set; }


    }
}
