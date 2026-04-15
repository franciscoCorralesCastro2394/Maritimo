using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Maritimo.Models.Models
{
    public class Puerto
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }
    }
}
