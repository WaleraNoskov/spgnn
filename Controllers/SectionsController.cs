using System;
using System.IO;
using System.Text.RegularExpressions;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using spgnn.DAL.Repositories;
using spgnn.Models;
using spgnn.ViewModels;

namespace spgnn.Controllers
{
    public class SectionsController : Controller
    {
        private readonly ISectionsRepository<Article> _repository;
        private readonly IWebHostEnvironment _appEnvironment;

        public SectionsController(ISectionsRepository<Article> articleRepository, IWebHostEnvironment appEnvironment)
        {
            this._repository = articleRepository;
            this._appEnvironment = appEnvironment;
        }

        [HttpGet]
        [ActionName("addoredit")]
        public IActionResult AddOrEdit(int id)
        {
            if (id >= 0)
            {
                var model = _repository.Find(id);
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
                _repository.Update(model);
                return View(viewModel);
            }
            else
            {
                model.Date = DateTime.Now;
                _repository.Insert(model);
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
                path = $"/Files/sections/{id}/image{filesCount}{fileExtension}";
                

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
            var regex = new Regex(@"/Files/sections/\d{1,10000000}/\S{1,10000000}.jpg",
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
            var model = _repository.Find(id);
            return View(model);
        }
        
        [HttpGet]
        [ActionName("delete")]
        public IActionResult Delete(int id)
        {
            var article = _repository.Find(id);
            _repository.Remove(article);

            var dir = _appEnvironment.WebRootPath + $"/Files/sections/{id}";
            if(Directory.Exists(dir))
                Directory.Delete(dir, true);
            
            return RedirectToAction("Show", "NewsList", new {selectedPage = 40});
        }
    }
}