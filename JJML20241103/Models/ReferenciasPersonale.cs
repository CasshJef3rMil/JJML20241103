using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JJML20241103.Models
{
    public partial class ReferenciasPersonale
    {
        public int Id { get; set; }
        public int EmpleadoId { get; set; }
        [Required(ErrorMessage = "El nombre del Referente es obligatorio")]
        public string Nombre { get; set; } = null!;
        [Required(ErrorMessage = "El campo Apellido es obligatorio.")]
       
        public string Apellido { get; set; } = null!;
        public string? Relacion { get; set; }
        public string? Telefono { get; set; }

        public virtual Empleado Empleado { get; set; } = null!;
    }
}
