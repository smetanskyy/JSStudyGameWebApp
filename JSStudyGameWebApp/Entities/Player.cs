using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace JSStudyGameWebApp.Entities
{
    [Table("tblPlayers")]
    public class Player
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(200)]
        public string Email { get; set; }
        [Required, StringLength(200)]
        public string Login { get; set; }
        [Required, StringLength(200)]
        public string Password { get; set; }
        [Required]
        public bool IsAdmin { get; set; }

        public virtual PlayerAdditionalInfo AdditionalInfo { get; set; }
        public virtual PlayerScore Score { get; set; }
    }
}
