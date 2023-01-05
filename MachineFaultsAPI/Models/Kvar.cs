using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MachineFaultsAPI.Models
{
    public class Kvar
    {
        public int Id { get; set; }
        public int Id_Stroja { get; set; }
        public string Naziv_Stroja { get; set; }
        public string Naziv { get; set; }
        public string Prioritet { get; set; }
        public DateTime Vrijeme_Pocetka { get; set; }
        public DateTime? Vrijeme_Zavrsetka { get; set; }
        public string Opis { get; set; }
        public string Status { get; set; }
    }

}