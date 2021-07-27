using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ImageMagick;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using spgnn.DAL.Repositories;
using spgnn.Models;
using spgnn.ViewModels;

namespace spgnn.Controllers
{
    [Route("info/[action]")]
    public class InfoArticlesController : Controller
    {
        private IInfoArticleRepository<Article> _repository { get; set; }
        private readonly IWebHostEnvironment _appEnvironment;

        public InfoArticlesController(IInfoArticleRepository<Article> repository, IWebHostEnvironment appEnvironment)
        {
            _repository = repository;
            _appEnvironment = appEnvironment;
        }
        
        [HttpGet]
        [ActionName("Show")]
        public IActionResult Show(int id)
        {
            var models = _repository.Query(x => x.Id >= 0).ToList();
            if (models.Count > 0) {
                if (id>=0)
                    return View("show", new InfoArticlesListViewModel() {Articles = models, CurrentArticle = models.Find(x => x.Id == id)});
                return View("show", new InfoArticlesListViewModel() {Articles = models, CurrentArticle = models.First()});
            }
            else
            {
                return RedirectToAction("addoredit", new {id=-1});
            }
        }
        
        [HttpGet]
        [ActionName("addoredit")]
        public IActionResult AddOrEdit(int id)
        {
            if (id >= 0)
            {
                var model = _repository.Find(id);
                var viewModel = new InfoAddOrEditViewModel() {Dto = model, NeedEdit = true};
                return View(viewModel);
            }
            else
            {
                return View(new InfoAddOrEditViewModel() {Dto = new Article(), NeedEdit = false});
            }
        }

        [HttpPost]
        [ActionName("addoredit")]
        public IActionResult AddOrEdit(InfoAddOrEditViewModel viewModel, bool needEdit)
        {
            var model = viewModel.Dto;
            if (needEdit)
            {
                model.Date = DateTime.Now;
                _repository.Update(model);
                return View(viewModel);
            }
            else
            {
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
                var directory = _appEnvironment.WebRootPath + $"/Files/InfoArticles/{id}";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                var filesCount = Directory.GetDirectories(directory).Length + Directory.GetFiles(directory).Length;
                var fileExtension = System.IO.Path.GetExtension(uploadedFile.FileName);
                path = $"/Files/InfoArticles/{id}/image{filesCount}{fileExtension}";

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
            var regex = new Regex(@"/Files/InfoArticles/\d{1,10000000}/\S{1,10000000}.jpg",
                RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            var matches = regex.Matches(path);
            var deletablePath = _appEnvironment.WebRootPath + matches[0].Value;
            System.IO.File.Delete(deletablePath);
            return null;
        }
        
        [HttpGet]
        [ActionName("delete")]
        public IActionResult Delete(int id)
        {
            var article = _repository.Find(id);
            _repository.Remove(article);

            var dir = _appEnvironment.WebRootPath + $"/Files/InfoArticles/{id}";
            if(Directory.Exists(dir))
                Directory.Delete(dir, true);
            
            return RedirectToAction("Show", new {id = -1});
        }
    }
}















































































































/*Чел, прости, если ты это читаешь. У меня просто очень кривые рако-руки на данный момент, и что-то более умное, чем 
просто скопировать почти полностью страницу и почти все сущности для редактирования почти такой-же записи новостей я 
просто не придумал. И вообще, если это кому-то пригодится, меня просто этот сайт уже бесит. Возможно внутри я уже давно 
умер, я потерял стимул к жизни, особенно к этому сайту. Я просто чувствую себя бездушным программистом сайта гимназии, 
который уже не верит ни в чудо, не в счастье. И я очень надеюсь, что если тебе придется разбираться в этом дерьме, ты не
докатишься до такого
Как хорошо, что панку не нужен план*/