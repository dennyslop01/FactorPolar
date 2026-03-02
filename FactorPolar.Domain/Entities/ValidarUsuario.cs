using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorPolar.Domain.Entities
{
    public class ValidarUsuario
    {
        [Required(ErrorMessage = "Debe completar el campo.")]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe completar el campo.")]
        public string NombreCorto { get; set; } = string.Empty;
    }
}
