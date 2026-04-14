using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCCinemaProgramme.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Genre { get; set; }
        public string? Rating { get; set; }

        [Display(Name = "Ticket Price")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TicketPrice { get; set; }

    }
}
