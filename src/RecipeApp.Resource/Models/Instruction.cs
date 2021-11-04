using RecipeApp.Base.Interfaces.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApp.Resource.Models
{
    public class Instruction : IInstruction
    {
        public string Guid { get; set; }
        public string RecipeGuid { get; set; }
        public int OrderNumber { get; set; }
        public string Text { get; set; }
        public string Notes { get; set; }

        public static Instruction FromInterface(IInstruction instruction)
        {
            if (string.IsNullOrEmpty(instruction.Guid))
            {
                instruction.Guid = System.Guid.NewGuid().ToString();
            }
            var output = new Instruction()
            {
                Guid = instruction.Guid,
                OrderNumber = instruction.OrderNumber,
                Text = instruction.Text,
                Notes = instruction.Notes
            };
            return output;
        }
    }
}
