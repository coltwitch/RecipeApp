using RecipeApp.Base.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApp.API.Models
{
    public class InstructionRequest : IInstruction
    {
        public string Guid { get; set; }
        public int OrderNumber { get; set; }
        public string Text { get; set; }
        public string Notes { get; set; }
    }
}
