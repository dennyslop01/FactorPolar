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

        [Range(0, 20, ErrorMessage = "El promedio debe estar entre 0 a 20.")]
        public int? Promedio { get; set; }
    }
}
