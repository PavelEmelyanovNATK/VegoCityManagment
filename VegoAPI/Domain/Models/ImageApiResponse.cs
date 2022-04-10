using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegoAPI.Domain.Models
{
    internal class ImageApiResponse
    {
        public ImageField image { get; set; }
    }

    internal class ImageField
    {
        public FileField file { get; set; }
    }

    internal class FileField
    {
        public ResourceField resource { get; set; }
    }

    internal class ResourceField
    {
        public ChainField chain { get; set; }
    }

    internal class ChainField
    {
        public string image { get; set; }
        public string medium { get; set; }
        public string thumb { get; set; }
    }
}
