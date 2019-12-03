using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JSStudyGameWebApp.ViewModels
{
    public class PlayerFullVM
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Photo { get; set; }
        public DateTime BirthDate { get; set; }
        public bool Gender { get; set; }
    }
}
