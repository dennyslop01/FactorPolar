namespace FactorPolar.Domain.Entities
{
    public class RubricaEvaluacion
    {
        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public decimal Factor { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public GrupoEvaluacion GrupoEvaluacion { get; set; }
    }
}
