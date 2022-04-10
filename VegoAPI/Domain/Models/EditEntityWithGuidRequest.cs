using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegoAPI.Domain.Models
{
    public class EditEntityWithGuidRequest
    {
        public Guid EntityId { get; set; }
        public Dictionary<string, string> ChangedFields { get; set; }
    }
}
