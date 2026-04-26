using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorPolar.Domain.Entities
{
    public class BeneficiarioClasificado
    {
        public int Id { get; set; }
        public Beneficiario Beneficiario { get; set; }
        public int Puntuacion { get; set; }
        public decimal FactorResultado { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int Estado { get; set; }
        public string Categoria { get; set; }
    }
}
