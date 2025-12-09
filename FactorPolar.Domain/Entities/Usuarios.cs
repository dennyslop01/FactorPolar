using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactorPolar.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; } //&
        public string? Email { get; set; } //AK
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
