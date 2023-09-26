using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G1FOODLibrary.DTO
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email không được bỏ trống!")]
        [MaxLength(100, ErrorMessage = "Email phải dưới 100 ký tự!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống!")]
        [MaxLength(100, ErrorMessage = "Mật khẩu phải dưới 100 ký tự!")]
        public string Password { get; set; }
    }
}
