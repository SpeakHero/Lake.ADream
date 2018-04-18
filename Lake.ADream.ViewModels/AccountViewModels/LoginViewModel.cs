using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace Lake.ADream.ViewModels.AccountViewModels
{
    public class LoginViewModel
    {
     
        public  string ReturnUrl { get; set; }
        [Required(ErrorMessage = "你必须填写{0}")]
        [Display(Name = "密码", AutoGenerateField = true, Description = "密码", Prompt = "密码")]
        [DataType(DataType.Password)]
        public string Password
        {
            get; set;
        }

        [Display(Name = "记住我", AutoGenerateField = true, Description = "记住我", Prompt = "记住我")]
        public bool RememberMe { get; set; }
        

        [Required(ErrorMessage = "你必须填写{0}")]
        [Display(Name = "验证码", AutoGenerateField = true, Description = "验证码", Prompt = "请输入验证码")]
        //[Remote("VerifyCodes", "Account", HttpMethod = "post", AdditionalFields = "Provider,__RequestVerificationToken,Email,Phone", ErrorMessage = "{0}错误")]
        public string Code { get; set; }
        [Required(ErrorMessage = "你必须填写{0}")]
        [Display(Name = "用户名、手机或者邮箱", AutoGenerateField = true, Description = "用户名", Prompt = "请输入用户名、手机或者邮箱")]
        public string Account
        {
            get; set;
        }
        private string _name;

    }
}
