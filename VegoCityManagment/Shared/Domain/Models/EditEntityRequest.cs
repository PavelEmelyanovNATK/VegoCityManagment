using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegoCityManagment.Shared.Domain.Models
{
    public class EditEntityRequest
    {
        public int EntityId { get; set; }
        public Dictionary<string, string> ChangedFields { get; set; }
    }
}
