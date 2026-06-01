namespace FactorPolar.Domain.Entities
{
    public class BeneficiarioRubrica
    {
        public int Id { get; set; }
        public Usuario Usuario { get; set; }
        public RubricaEvaluacion RubricaEvaluacion { get; set; }
        public Beneficiario Beneficiario { get; set; }
        public int Puntuacion { get; set; }
        public decimal FactorResultado { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public DateTime? DismissDate { get; set; }
        public int? UsuarioDismissId { get; set; }

    }

    public class BeneficiarioRubricaEnVivo
    {
        public int Id { get; set; }
        public Usuario Usuario { get; set; }
        public RubricaEvaluacion RubricaEvaluacion { get; set; }
        public Beneficiario Beneficiario { get; set; }
        public int Puntuacion { get; set; }
        public decimal FactorResultado { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

    }

    public class BeneficiarioClasificadoVotoWeb
    {
        public int Id { get; set; }
        public Beneficiario Beneficiario { get; set; }
    }
}
