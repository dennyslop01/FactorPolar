namespace FactorPolar.Domain.Entities
{
    public class GrupoEvaluacion
    {
        public int Id { get; set; }
        public string? NombreGrupo { get; set; }
        public int? TipoEvaluacion { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
