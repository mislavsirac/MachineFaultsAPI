using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MachineFaultsAPI.Models
{
    public class Stroj
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public double Prosjecno_Trajanje_Kvarova { get; set; }
    }

}