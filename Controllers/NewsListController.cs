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
        private IRepositoryBase<Article> _repository;
        private int _pageCount;
        public NewsListController(IRepositoryBase<Article> repository)
        {
            this._repository = repository;
            _pageCount = 0;
        }

        [HttpGet]
        [ActionName("show")]
        public IActionResult Show()
        {
            var models = _repository.Query(x => x.Id >= _pageCount*10 && _pageCount <= _pageCount*10).ToList();

            return View("show", new NewsListViewModel() {Articles = models});
        }

        [HttpGet]
        [ActionName("delete")]
        public IActionResult Delete(int id)
        {
            var article = _repository.Find(id);
            _repository.Remove(article);
            return View();
        }
    }
}