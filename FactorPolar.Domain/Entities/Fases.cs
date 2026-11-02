using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorPolar.Domain.Entities
{
    public class Fases
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public int Estatus { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
