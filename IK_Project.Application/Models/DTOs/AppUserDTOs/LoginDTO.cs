using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IK_Project.Application.Models.DTOs.AppUserDTOs
{
    
		public class LoginDTO
		{
			//[Required(ErrorMessage = "Kullanıcı Adı girilmesi zorunludur.")]
			//[Display(Name = "Kullanıcı Adı")]
			//public string UserName { get; set; }


			[Required(ErrorMessage = "E-Mail girilmesi zorunludur.")]
			[DataType(DataType.EmailAddress)]
			[Display(Name = "E-Mail")]
			public string Email { get; set; }

			[Required(ErrorMessage = "Parola girilmesi zorunludur!")]
			[DataType(DataType.Password)]
			[Display(Name = "Parola")]
			public string Password { get; set; }
		
		}
}
