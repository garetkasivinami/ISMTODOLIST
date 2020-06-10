using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ISMTodoList.Models
{
    public static class HelperStrings
    {
        public const string EmailRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
    }
    public class LoginModel
    {
        [Required(ErrorMessage = "Це поле необхідно заповнити!")]
        [RegularExpression(HelperStrings.EmailRegex, ErrorMessage = "Введена пошта не валідна!")]
        [Display(Name = "Введіть ваш емейл (логін)")]
        public string Name { get; set; }

        [Required (ErrorMessage = "Це поле необхідно заповнити!")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Пароль повинен містити мінімум 8 символів!")]
        [Display(Name = "Введіть пароль")]
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessage = "Це поле необхідно заповнити!")]
        [RegularExpression(HelperStrings.EmailRegex, ErrorMessage = "Введена пошта не валідна!")]
        [Display(Name="Введіть ваш емейл(логін)")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Це поле необхідно заповнити!")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Пароль повинен містити мінімум 8 символів!")]
        [Display(Name = "Введіть пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Це поле необхідно заповнити!")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Паролі не співпадають")]
        [Display(Name = "Повторіть пароль")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Це поле необхідно заповнити!")]
        [Display(Name = "Вкажіть ваш вік")]
        public int Age { get; set; }
    }
}