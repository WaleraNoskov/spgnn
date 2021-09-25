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
        
        public NewsListController(IRepositoryBase<Article> repository, IWebHostEnvironment appEnvironment)
        {
            this._repository = repository;
            this._appEnvironment = appEnvironment;
        }

        [HttpGet]
        [ActionName("show")]
        public IActionResult Show(int selectedPage)
        {
            var Articles = new List<Article>();
            
            int count = 0;
            while (Articles.Count != 5)
            {
                var article = _repository.Find(selectedPage  + count);
                if (article != null)
                {
                    Articles.Add(article);
                }

                count++;
                if (count > 10)
                {
                    break;
                }
            }

            return View(new NewsListViewModel {ArticlesResult = Articles, CurrentPage = selectedPage, GlobalCount = 
                _repository.Query(x => x.Id > 0).ToList().Count}); 
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