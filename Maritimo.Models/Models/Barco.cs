using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Maritimo.Models.Models
{
    public class Barco
    {

        public int Id { get; set; }
        public bool Activo { get; set; }

        [Required(ErrorMessage = "La Nombre es obligatoria")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La Matricula es obligatoria")]
        public string Matricula { get; set; }

        [Required(ErrorMessage = "La Tonelaje es obligatoria")]
        public int Tonelaje { get; set; }

        public int PuertoId { get; set; }

        public Puerto Puerto { get; set; }

        [Required(ErrorMessage = "Modelo del motor es obligatoria")]
        public string ModeloMotor { get; set; }

        [Required(ErrorMessage = "Potencia del motor es obligatoria")]
        public string PotenciaMotor { get; set; }

        [Required(ErrorMessage = "Horas Uso del motor es obligatoria")]
        public int HoraUsoMotor { get; set; }

    }
}
