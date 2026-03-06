using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorPolar.Domain.Entities
{
    public class BeneficiarioRubrica
    {
        public int Id { get; set; }
        public Usuario Usuario { get; set; }
        public RubricaEvaluacion Rubrica { get; set; }
        public Beneficiario Beneficiario { get; set; }
        public int Puntuacion { get; set; }
        public decimal FactorResultado { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
