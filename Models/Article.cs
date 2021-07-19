using System;
using System.Net.Http.Headers;

namespace spgnn.Models
{
    public class Article : EntityBase
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string NoneImagesText { get; set; }
        public DateTime Date { get; set; }
        public DateTime AfterEditingDate { get; set; }

        public Article()
        {
            this.Date = DateTime.Now;
        }
    }
}