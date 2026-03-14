using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorPolar.Domain.Entities
{
    public class DatosCSV()
    {
        [Range(0, 1000, ErrorMessage = "El tipo separador es obligatorio.")]
        public int TipoSeparador { get; set; }
    }

    public class RubricaBasica
    {
        public int IdRubrica { get; set; }
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public string Codigo { get; set; }
    }

    public class RubricaCompuesta
    {
        public int IdRubrica { get; set; }
        public int IdGrupo { get; set; }
        public string DescGrupo { get; set; }
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Range(1, 5, ErrorMessage = "El valor debe estar entre 1 y 5")]
        public int Codigo { get; set; }
    }
}
