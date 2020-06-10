using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ISMTodoList.Models
{
    public class UserTask
    {
        [Required]
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Це поле необхідно заповнити!")]
        [Display(Name = "Назва справи")]
        public string Name { get; set; }
        [Display(Name = "Опис справи")]
        public string Description { get; set; }
        [Display(Name = "Дата")]
        [DataType(DataType.DateTime)]
        public DateTime? Date { get; set; }
    }
}