using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegoAPI.Domain.Models
{
    public class LoadProductPhotoRequest
    {
        public Guid ProductId { get; set; }
        public byte[] PhotoBytes { get; set; }
    }
}
