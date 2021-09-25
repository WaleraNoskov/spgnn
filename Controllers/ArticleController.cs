using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using spgnn.DAL.Repositories;
using spgnn.Models;
using spgnn.ViewModels;
using Microsoft.AspNetCore.Hosting;
using ImageMagick;
using Microsoft.AspNetCore.Http;

namespace spgnn.Controllers
{
    [Route("article/[action]")]
    public class ArticleController : Controller
    {
        private readonly IRepositoryBase<Article> _articleRepository;
        private readonly IWebHostEnvironment _appEnvironment;

        public ArticleController(IRepositoryBase<Article> articleRepository, IWebHostEnvironment appEnvironment)
        {
            this._articleRepository = articleRepository;
            this._appEnvironment = appEnvironment;
        }

        [HttpGet]
        [ActionName("addoredit")]
        public IActionResult AddOrEdit(int id)
        {
            if (id >= 0)
            {
                var model = _articleRepository.Find(id);
                var viewModel = new AddOrEditViewModel() {Dto = model, NeedEdit = true};
                return View(viewModel);
            }
            else
            {
                return View(new AddOrEditViewModel() {Dto = new Article(), NeedEdit = false});
            }
        }

        [HttpPost]
        [ActionName("addoredit")]
        public IActionResult AddOrEdit(AddOrEditViewModel viewModel, bool needEdit)
        {
            var model = viewModel.Dto;

            if (needEdit)
            {
                model.AfterEditingDate = DateTime.Now;
                _articleRepository.Update(model);
                return View(viewModel);
            }
            else
            {
                model.Date = DateTime.Now;
                _articleRepository.Insert(model);
                return View(viewModel);
            }
        }

        [HttpPost]
        public IActionResult TinyUpload(int id)
        {
            var uploadedFile = Request.Form.Files[0];
            var path = "";
            if (uploadedFile != null)
            {
                var directory = _appEnvironment.WebRootPath + $"/Files/{id}";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                var filesCount = Directory.GetDirectories(directory).Length + Directory.GetFiles(directory).Length;
                var fileExtension = System.IO.Path.GetExtension(uploadedFile.FileName);
                path = $"/Files/{id}/image{filesCount}{fileExtension}";
                

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    using (var uploadedFileStream = uploadedFile.OpenReadStream())
                    {
                        using (var image = new MagickImage(uploadedFileStream))
                        {
                            image.Strip();
                            image.Quality = 90;
                            if (image.Width > 1000)
                                image.Resize(1024, 0);
                            if (image.Height > 1000)
                                image.Resize(0, 1024);

                            if (image.Format != MagickFormat.Jpeg)
                                image.Format = MagickFormat.Jpeg;

                            image.Write(fileStream);
                        }
                    }
                }
            }

            var location = Url.Content(path);
            return Json(new {location});
        }

        [HttpPost]
        public IActionResult TinyDeleteImage(string path)
        {
            var regex = new Regex(@"/Files/\d{1,10000000}/\S{1,10000000}.jpg",
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            var matches = regex.Matches(path);
            var deletablePath = _appEnvironment.WebRootPath + matches[0].Value;
            System.IO.File.Delete(deletablePath);
            return null;
        }

        [HttpGet]
        [ActionName("show")]
        public IActionResult Show(int id)
        {
            var model = _articleRepository.Find(id);
            return View(model);
        }
    }
}