using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeApp.Base.Interfaces.Models
{
    public interface IInstruction
    {
        public string Guid { get; set; }
        public int OrderNumber { get; set; }
        public string Text { get; set; }
        public string Notes { get; set; }
    }
}
