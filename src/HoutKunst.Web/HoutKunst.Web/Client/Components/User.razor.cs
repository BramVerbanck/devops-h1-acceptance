using HoutKunst.Web.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HoutKunst.Web.Client.Components
{
    public partial class User
    {
        [Parameter] public UserDto user { get; set; }


        
    }
}
