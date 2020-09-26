using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MangoServer.Model
{
    public class Utilisateur
    {
        public int Id { get; set; }
        [Required]
        public string Identifiant { get; set; }
        [Required]
        public string Mdp { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
