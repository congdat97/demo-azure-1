using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Model
{
    public class LoginRequest
    {
        [DefaultValue("admin")]
        public string? Username { get; set; }

        [DefaultValue("123")]
        public string? Password { get; set; }
    }
}
