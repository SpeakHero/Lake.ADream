using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lake.ADream.ViewModels.AccountViewModels
{
    public class LoginWith2faViewModel
    {
        [Required]
        [StringLength(7, ErrorMessage = "{0}必须至少{2}，最多{1}字符长。", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "认证码")]
        public string TwoFactorCode { get; set; }

        [Display(Name = "记住我？")]
        public bool RememberMachine { get; set; }
        [Display(Name = "记住我？")]
        public bool RememberMe { get; set; }
    }
}
