using System;
using System.Collections.Generic;
using System.Text;

namespace Lake.ADream.ViewModels.AccountViewModels
{
    public  class LoginoutViewModel
    {
        public string ReturnUrl { get; set; }
        public bool Lockout { get; set; }
        public DateTime LockoutTime { get; set; }
        public bool Need2fa { get; set; }
    }
}
