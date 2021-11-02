using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoutKunst.Web.Shared
{
   public class ArtWorkDto
    {

        public int Id { get; set; }

        public string info { get; set; }

        public string  Image { get; set; }


        public string Maker { get; set; }
        public int MaakJaar { get; set; }

        public int Vraagprijs { get; set; }
        public bool IsOutdoors { get; set; }
        public ArtType ArtType { get; set; }

        public ArtWorkDto()
        {

        }

        public ArtWorkDto(Artwork art)
        {
            this.Id = art.Id;
            this.info = art.Info;

        }

        
    }
}
