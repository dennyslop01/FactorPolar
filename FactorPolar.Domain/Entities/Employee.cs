using Microsoft.EntityFrameworkCore;

namespace FactorPolar.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; } //&
        public string Email { get; set; } //AK
        public Int64 EmployeeNumber { get; set; }//A
        public string FullName { get; set; } //B
        public string Gender { get; set; } //O
        public string Status { get; set; } //D
        public string CodigoTipo { get; set; } //E
        public string DescipcionTipo { get; set; } //F
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }

    public class Beneficiario
    {
        public int Id { get; set; } //&
        public string Denominacion { get; set; } //U   
        public string CedulaIdentidad { get; set; } //V   
        public string FullName { get; set; } //W
        public string Gender { get; set; } //Z
        public DateTime FechaNacimiento { get; set; } //AA
        public int Edad { get; set; } //AB
        public string TipoInstitucion { get; set; } //AC
        public string NombreInstitucion { get; set; } //AE
        public string RifInstitucion { get; set; } //AF
        public string NivelEducativo { get; set; } //AG
        public string GradoEducativo { get; set; } //AH
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public Employee Employee { get; set; }
    }
}
