using System.Net.Mime;
using System.Net;
using System.Linq;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using spgnn.DAL.Repositories;
using spgnn.Models;
using spgnn.ViewModels;
using Microsoft.AspNetCore.Hosting;
using ImageMagick;

namespace spgnn.Controllers
{
    [Route("article/[action]")]
    public class ArticleController : Controller
    {
        private IRepositoryBase<Article> articleRepository;
        private IRepositoryBase<FileModel> fileRepository;
        private IWebHostEnvironment appEnvironment;

        public ArticleController(IRepositoryBase<Article> articleRepository, IRepositoryBase<FileModel> fileRepository, IWebHostEnvironment appEnvironment)
        {
            this.articleRepository = articleRepository;
            this.fileRepository = fileRepository;
            this.appEnvironment = appEnvironment;
        }

        [HttpGet]
        [ActionName("addoredit")]
        public IActionResult AddOrEdit(int id)
        {
            if (id >= 0)
            {
                var model = articleRepository.Find(id);
                var viewModel = new AddOrEditViewModel() { DTO = model, NeedEdit = true };
                return View(viewModel);
            }
            else
            {
                return View(new AddOrEditViewModel() { DTO = new Article(), NeedEdit = false });
            }
        }

        [HttpPost]
        [ActionName("addoredit")]
        public IActionResult AddOrEdit(AddOrEditViewModel viewModel, bool needEdit)
        {
            var model = viewModel.DTO;
            if (needEdit == true)
            {
                articleRepository.Update(model);
                return View(viewModel);
            }
            else
            {
                articleRepository.Insert(model);
                return View(viewModel);
            }
        }

        [HttpPost]
        public IActionResult TinyUpload(int id)
        {
            IFormFile uploadedFile = Request.Form.Files[0];
            string path = "";
            if (uploadedFile != null)
            {
                path = $"/Files/{id}/" + uploadedFile.FileName;
                string directory = appEnvironment.WebRootPath + $"/Files/{id}";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                using (var fileStream = new FileStream(appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    using(var uploadedFileStream = uploadedFile.OpenReadStream())
                    {
                        using(var image = new MagickImage(uploadedFileStream))
                        {
                            image.Strip();
                            image.Quality = 90;
                            if(image.Width > 1000)
                                image.Resize(1024, 0);
                            if(image.Height > 1000)
                                image.Resize(0, 1024);

                            if(image.Format != MagickFormat.Jpeg)
                                image.Format = MagickFormat.Jpeg;

                            image.Write(fileStream);
                        }
                    }
                }
            }

            string location = Url.Content(path);
            return Json(new { location });
        }

        [HttpGet]
        [ActionName("show")]
        public IActionResult Show(int id)
        {
            var model = articleRepository.Find(id);
            return View(model);
        }
    }
}