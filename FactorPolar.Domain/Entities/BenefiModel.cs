using System.ComponentModel.DataAnnotations;

namespace FactorPolar.Domain.Entities
{
    public class BenefiModel
    {
        [Required(ErrorMessage = "Debe completar el campo.")]
        public string TipoInstitucion { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Debe completar el campo.")]
        public string NombreInstitucion { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Debe completar el campo.")]
        public string RifInstitucion { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Debe completar el campo.")]
        public string NivelEducativo { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Debe completar el campo.")]
        public string GradoEducativo { get; set; } = string.Empty;

        [Range(0, 20, ErrorMessage = "El promedio debe ser A o estar entre 18 a 20.")]
        public int? Promedio { get; set; }
    }

    public class Benefi2MomentoModel
    {
        public string TipoInstitucion { get; set; } = string.Empty;
        public string NombreInstitucion { get; set; } = string.Empty;
        public string RifInstitucion { get; set; } = string.Empty;
        public string NivelEducativo { get; set; } = string.Empty;
        public string GradoEducativo { get; set; } = string.Empty;
        public int? Promedio { get; set; }

        [Required(ErrorMessage = "Debe completar el campo.")]

        [Range(0, 20, ErrorMessage = "El promedio debe ser A o estar entre 18 a 20.")]
        public int? Promedio2 { get; set; }
    }
}
