using Domain;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace HoutKunst.Web.Server.Controllers
{
    public class ArtworkDetailController : Controller
    {
        private Artwork CurrentArtwork;
        private IArtworkRepository ArtworkRepo;

        public ArtworkDetailController(IArtworkRepository artworkRepo)
        {
            this.ArtworkRepo = artworkRepo;
        }

        public IActionResult Index()
        {
            return Ok("Ok start op");
        }

        #region ImageHandlers
        //Download function not required (Displaying the image is enough)
        public ActionResult Download(string url)
        {
            string path = FTPConnection.Download(url);
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, MediaTypeNames.Application.Octet, Path.GetFileName(path));
        }

        //Uploading an image
        [HttpPost]
        public ActionResult UploadFile()
        {
            Artwork artwork = (Artwork)JsonConvert.DeserializeObject<Artwork>((string)TempData.Peek("artwork"));
            artwork = ArtworkRepo.GetById(artwork.Id);
            var file = Request.Form.Files[0];
            try
            {


                if (file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);
                        string name = file.FileName;
                        Image image = new Image();
                        Byte[] byteArray = ms.ToArray();
                        using (var md5 = System.Security.Cryptography.MD5.Create())
                        {
                            md5.TransformFinalBlock(byteArray, 0, byteArray.Length);
                            var hash = md5.Hash;
                            image.Path = Convert.ToBase64String(hash) + name;

                        }
                        artwork.AddImage(image);
                        ArtworkRepo.SaveChanges();

                        FTPConnection.UploadBijlages(byteArray, image.Path);
                    }
                    return RedirectToAction(nameof(Index), artwork);

                }
                ViewBag.Message = "File Uploaded Successfully!!";
                return RedirectToAction(nameof(Index), artwork);
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return RedirectToAction(nameof(Index), artwork);
            }
        }
        #endregion
    }
}
