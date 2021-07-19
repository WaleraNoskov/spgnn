using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using spgnn.DAL.Repositories;
using spgnn.Models;
using spgnn.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Hosting;

namespace spgnn.Controllers
{
    [Route("news/[action]")]
    public class NewsListController : Controller
    {
        private IRepositoryBase<Article> _repository;
        private readonly IWebHostEnvironment _appEnvironment;
        private List<Article> Articles;
        public NewsListController(IRepositoryBase<Article> repository, IWebHostEnvironment appEnvironment)
        {
            this._repository = repository;
            this._appEnvironment = appEnvironment;
            this.Articles = _repository.Query(x => x.Id >= 0).ToList();
        }

        [HttpGet]
        [ActionName("show")]
        public IActionResult Show(int pageCount)
        {
            if (Articles.Count == 0)
            {
                return RedirectToAction("AddOrEdit", "Article", new {id = -1});
            }
            
            var realPageCount = (Articles.FirstOrDefault().Id/10)*10 + pageCount*10;
            var models = _repository.Query(x => x.Id >= realPageCount && x.Id <= realPageCount+10).ToList();
            return View("show", new NewsListViewModel() {Articles = models, PageCount = this.Articles.Count / 10, CurrentPage = pageCount});
        }

        [HttpGet]
        [ActionName("delete")]
        public IActionResult Delete(int id)
        {
            var article = _repository.Find(id);
            _repository.Remove(article);

            var dir = _appEnvironment.WebRootPath + $"/Files/{id}";
            if(Directory.Exists(dir))
                Directory.Delete(dir, true);
            
            return View();
        }
    }
}