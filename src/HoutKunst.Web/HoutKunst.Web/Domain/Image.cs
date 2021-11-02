using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public partial class Image
    {
        public int Id { get; set; }
        public string  Path { get; set; }
        //public ICollection<ArtworkImages> ArtworkImages { get; set; }

        public Image()
        {
            Id = 1;
            // ArtworkImages = new HashSet<ArtworkImages>();
            Path = "D:\\school\\3e jaar\\projecten 3\\devops-project-web-h1\\src\\HoutKunst.Web\\HoutKunst.Web\\Client\\wwwroot\\Images\\image_1_244357.jpg";
        }
    }
}
