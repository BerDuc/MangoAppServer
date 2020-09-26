using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MangoServer.Model
{
    public class Item
    {
        public int Id { get; set; }
        [Required]
        public string Titre { get; set; }
        public string Description { get; set; }
    }
}
