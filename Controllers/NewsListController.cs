using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using spgnn.DAL.Repositories;
using spgnn.Models;
using spgnn.ViewModels;
using System.Linq;

namespace spgnn.Controllers
{
    [Route("news/[action]")]
    public class NewsListController : Controller
    {
        private IRepositoryBase<Article> repository;
        private int pageCount;
        public NewsListController(IRepositoryBase<Article> repository)
        {
            this.repository = repository;
            pageCount = 0;
        }

        [HttpGet]
        [ActionName("show")]
        public IActionResult Show()
        {
            var models = repository.Query(x => x.Id >= pageCount*10 && pageCount <= pageCount*10).ToList();

            return View(new NewsListViewModel() {Articles = models});
        }
    }
}