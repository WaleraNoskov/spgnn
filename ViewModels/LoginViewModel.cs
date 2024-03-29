﻿using System.ComponentModel.DataAnnotations;

namespace spgnn.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Не указан логин")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public  string Password { get; set; }
    }
}