using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JSStudyGameWebApp.ViewModels
{
    public class PlayerAdditionalInfoVM
    {
        public int IdPlayerAdditionalInfo { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Photo { get; set; }
        public DateTime BirthDate { get; set; }
        public bool Gender { get; set; }
    }
}
