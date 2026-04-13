using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCCinemaProgramme.Models
{
    public class Seat
    {
        public int Id { get; set; }

        [Display(Name = "Seat's number")]
        public int Number { get; set; }

        public bool Taken { get; set; } = false;

        public int HallId { get; set; }
        [ForeignKey("HallId")]
        public virtual Hall? Hall { get; set; }
    }
}
