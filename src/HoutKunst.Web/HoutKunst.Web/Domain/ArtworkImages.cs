using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public partial class ArtworkImages
    {
        public int ArtworkId { get; set; }
        public int ImageId { get; set; }

        public Image Image { get; set; }
        public Artwork Artwork { get; set; }

        public bool Approved { get; set; }
    }
}
