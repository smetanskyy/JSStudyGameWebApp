using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JSStudyGameWebApp.Entities
{
    [Table("tblPlayersAdditionalInfo")]
    public class PlayerAdditionalInfo
    {
        [Key, ForeignKey("Player")]
        public int IdPlayerAdditionalInfo { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Surname { get; set; }
        [StringLength(250)]
        public string Photo { get; set; }
        public DateTime BirthDate { get; set; }
        public bool Gender { get; set; }

        public virtual Player Player { get; set; }
    }
}
