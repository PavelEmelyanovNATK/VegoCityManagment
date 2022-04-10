using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegoAPI.Domain.Models
{
    public class EditEntityWithIntIdRequest
    {
        public int EntityId { get; set; }
        public Dictionary<string, string> ChangedFields { get; set; }
    }
}
