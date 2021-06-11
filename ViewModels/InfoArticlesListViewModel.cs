using System.Collections.Generic;
using spgnn.Models;

namespace spgnn.ViewModels
{
    public class InfoArticlesListViewModel
    {
        public Article CurrentArticle { get; set; }
        public List<Article> Articles { get; set; }
    }
}