using System.ComponentModel.DataAnnotations;
using spgnn.Models;

namespace spgnn.ViewModels
{
    public class EditUserViewModel 
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        
        public string Email { get; set; }
        
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}