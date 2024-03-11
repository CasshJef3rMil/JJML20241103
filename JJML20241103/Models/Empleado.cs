using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JJML20241103.Models
{
    public partial class Empleado
    {
        public Empleado()
        {
            ReferenciasPersonales = new HashSet<ReferenciasPersonale>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage ="El nombre del empleado es obligatorio")]
        public string Nombre { get; set; } = null!;
        [Required(ErrorMessage = "El apellido del empleado es obligatorio")]
        public string Apellido { get; set; } = null!;
        public int? Edad { get; set; }
        public string? Cargo { get; set; }
        public DateTime? FechaContratacion { get; set; }

        public virtual ICollection<ReferenciasPersonale> ReferenciasPersonales { get; set; }
    }
}
