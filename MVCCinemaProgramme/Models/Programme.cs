using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCCinemaProgramme.Models
{
    public class Programme
    {
        public int Id { get; set; }

        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public virtual Movie? Movie { get; set; }
        public int HallId { get; set; }
        [ForeignKey("HallId")]
        public virtual Hall? Hall { get; set; }

        [Display(Name = "Beggins at")]
        [DataType(DataType.DateTime)]
        public DateTime Beggin { get; set; }

        [Display(Name = "Ends at")]
        [DataType(DataType.DateTime)]
        public DateTime End { get; set; }



    }
}
