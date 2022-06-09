using FreeCourse.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreeCourse.Shared.ControllerBases
{
    public class CustomBaseController  : ControllerBase
    {
        //ControllerBase aslında aspnet framework ünde gelen bir class. Bu class a dışardan erişmenin yolu:  Proje>sağ tık=> Edit Project File>
        //<ItemGroup>
		//<FrameworkReference Include = "Microsoft.AspNetCore.App"/>
        //</ItemGroup>

        public IActionResult CreateActionResultInstance<T>(Response<T> response)
        {
            return new ObjectResult(response)
            {
                StatusCode=response.StatusCode
            };
        }
    }
}

