namespace FactorPolar.Domain.Entities
{
    public class GrupoEdad
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int EdadMinima { get; set; }
        public int EdadMaxima { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}

