using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegoAPI.Domain.Models
{
    public class SetProductMainPhotoRequest
    {
        public Guid ProductId { get; set; }
        public Guid PhotoId { get; set; }
    }
}
