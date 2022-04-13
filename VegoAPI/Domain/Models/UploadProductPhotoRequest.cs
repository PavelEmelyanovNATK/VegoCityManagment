using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegoAPI.Domain.Models
{
    public class UploadProductPhotoRequest
    {
        public Guid ProductId { get; set; }
        /// <summary>
        /// URL or Base64 image string.
        /// </summary>
        public string Source { get; set; }
    }
}
