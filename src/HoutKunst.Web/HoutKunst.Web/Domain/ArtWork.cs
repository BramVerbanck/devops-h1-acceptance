using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Artwork
    {

        public int Id { get; }
        public string Info  { get; set; }
        public bool IsOutdoors { get; set; }
        public virtual ICollection<ArtworkImages> ArtworkImages { get; set; }

        public Artwork()
        {

        }

        public Artwork(string Info)
        {
           ArtworkImages = new HashSet<ArtworkImages>();
            Image img = new Image();
            this.AddImage(img);
            this.Info = Info;
        }

        public void  AddImage(Image i) {
            ArtworkImages.Add(new ArtworkImages() { Image = i, Artwork = this, ImageId = i.Id, ArtworkId = this.Id });
        }

       
    }
}
