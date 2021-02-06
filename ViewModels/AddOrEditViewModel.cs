using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using spgnn.Models;

namespace spgnn.ViewModels
{
    public class AddOrEditViewModel
    {
        public Article DTO { get; set; }
        public bool NeedEdit { get; set; }
        public IFormFileCollection UploadedFiles { get; set; }
    }
}