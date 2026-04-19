using System.ComponentModel.DataAnnotations;

namespace MVCCinemaProgramme.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public int SeatId { get; set; }
        public Seat? Seat { get; set; }

        public int ProgrammeId { get; set; }
        public Programme? Programme { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string CustomerEmail { get; set; }

        [Display(Name = "Purchase date")]
        public DateTime PurchaseDate { get; set; } = DateTime.Now;
    }
}
