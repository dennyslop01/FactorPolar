using Microsoft.EntityFrameworkCore;

namespace FactorPolar.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; } //&
        public string? Email { get; set; } //AK
        public Int64 EmployeeNumber { get; set; }//A
        public string? FullName { get; set; } //B
        public string? Gender { get; set; } //O
        public string? Status { get; set; } //D
        public string? CodigoTipo { get; set; } //E
        public string? DescipcionTipo { get; set; } //F
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class Beneficiario
    {
        public int Id { get; set; } //&
        public string? Denominacion { get; set; } //U   
        public string? CedulaIdentidad { get; set; } //V   
        public string? FullName { get; set; } //W
        public string? Gender { get; set; } //Z
        public DateTime FechaNacimiento { get; set; } //AA
        public int Edad { get; set; } //AB
        public string? TipoInstitucion { get; set; } //AC
        public string? NombreInstitucion { get; set; } //AE
        public string? RifInstitucion { get; set; } //AF
        public string? NivelEducativo { get; set; } //AG
        public string? GradoEducativo { get; set; } //AH
        public string? Documento1 { get; set; } //AH
        public string? Documento2 { get; set; } //AH
        public string? Documento3 { get; set; } //AH
        public string? RutaNotas { get; set; } //AH
        public string? RutaVideo { get; set; } //AH
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public Employee Employee { get; set; }
        public int? Promedio { get; set; }
        public string? TipoInstitucionAux { get; set; }
        public string? NombreInstitucionAux { get; set; }
        public string? RifInstitucionAux { get; set; }
        public string? NivelEducativoAux { get; set; }
        public string? GradoEducativoAux { get; set; }
        public string? NombreNotas { get; set; }
        public string? NombreVideo { get; set; }
        public int? EstadoPostulacion { get; set; }

    }
}
