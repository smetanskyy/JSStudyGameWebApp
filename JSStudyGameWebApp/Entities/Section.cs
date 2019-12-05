using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JSStudyGameWebApp.Entities
{
    [Table("tblSections")]
    public class Section
    {
        [Key]
        public int IdSection { get; set; }
        [Required, StringLength(100)]
        public string NameOFSection { get; set; }

        public ICollection<Test> Tests { get; set; }
    }
}
