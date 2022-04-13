using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegoAPI.Domain.Models
{
    public class PhotoResponse
    {
        public Guid PhotoId { get; set; }
        public string LowResPath { get; set; }
        public string HighResPath { get; set; }
    }
}
