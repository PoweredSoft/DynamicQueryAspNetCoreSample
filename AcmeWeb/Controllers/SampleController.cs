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
    public class SampleController : Controller
    {
        [Route("api/orders"), HttpGet, HttpPost]
        public IActionResult GetOrders([FromServices]AcmeContext context, 
            [FromServices]IQueryHandler handler,
            [FromBody]IQueryCriteria criteria)
        {
            return Ok(handler.Execute(context.Orders, criteria));
        }

        [Route("api/order-items"), HttpGet, HttpPost]
        public IActionResult GetOrderItems([FromServices]AcmeContext context,
            [FromServices]IQueryHandler handler,
            [FromBody]IQueryCriteria criteria)
        {
            return Ok(handler.Execute(context.OrderItems, criteria));
        }

        [Route("api/items"), HttpGet, HttpPost]
        public IActionResult GetItems([FromServices]AcmeContext context,
            [FromServices]IQueryHandler handler,
            [FromBody]IQueryCriteria criteria)
        {
            return Ok(handler.Execute(context.Items, criteria));
        }

        [Route("api/customers"), HttpGet, HttpPost]
        public IActionResult Get([FromServices]AcmeContext context,
            [FromServices]IQueryHandler handler,
            [FromBody]IQueryCriteria criteria)
        {
            return Ok(handler.Execute(context.Customers, criteria));
        }

        [Route("api/tickets"), HttpGet, HttpPost]
        public IActionResult GetTickets([FromServices]AcmeContext context,
            [FromServices]IQueryHandler handler,
            [FromBody]IQueryCriteria criteria)
        {
            return Ok(handler.Execute(context.Tickets, criteria));
        }

        [Route("api/tasks"), HttpGet, HttpPost]
        public IActionResult GetTasks([FromServices]AcmeContext context,
            [FromServices]IQueryHandler handler,
            [FromBody]IQueryCriteria criteria)
        {
            return Ok(handler.Execute(context.Tasks, criteria));
        }

    }
}
