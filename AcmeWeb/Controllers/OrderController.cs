using AcmeWeb.Dal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoweredSoft.DynamicQuery.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeWeb.Controllers
{
    public class OrderController : Controller
    {
        [Route("api/orders"), HttpGet, HttpPost]
        public IActionResult Get([FromServices]AcmeContext context, 
            [FromServices]IQueryHandler handler,
            [FromBody]IQueryCriteria criteria)
        {
            return Ok(handler.Execute(context.Orders, criteria));
        }
    }
}
