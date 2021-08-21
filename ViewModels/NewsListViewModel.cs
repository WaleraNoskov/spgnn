using System.Collections.Generic;
using spgnn.Models;

namespace spgnn.ViewModels
{
    public class NewsListViewModel
    {
        public List<Article> ArticlesResult { get; set; }
        public int CurrentPage { get; set; }
        public int GlobalCount { get; set; }
    }
}