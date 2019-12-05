using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JSStudyGameWebApp.Entities
{
    [Table("tblTests")]
    public class Test
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(500)]
        public string Question { get; set; }
        [Required, StringLength(100)]
        public string AnswerA { get; set; }
        [Required, StringLength(100)]
        public string AnswerB { get; set; }
        [Required, StringLength(100)]
        public string AnswerC { get; set; }
        [Required, StringLength(100)]
        public string AnswerD { get; set; }
        [Required, StringLength(100)]
        public string CorrectAnswer { get; set; }
        [StringLength(150)]
        public string Reference { get; set; }

        [ForeignKey("IdSection")]
        public int IdSection { get; set; }
        public Section Section { get; set; }
    }
}
