using AcmeWeb.Dal;
using Microsoft.AspNetCore.Mvc;
using PoweredSoft.DynamicQuery.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeWeb
{
    [Route("api/[controller]"), ApiController]
    public class DynamicController<T, TKey> : Controller
        where T : class
    {
        private static Random random = new Random();

        [HttpGet]
        public async Task<IQueryExecutionResult<T>> Get(
            [FromServices]AcmeContext context, 
            [FromServices]IQueryHandlerAsync handler, 
            [FromServices]IQueryCriteria criteria,
            int? page = null,
            int? pageSize = null)
        {
            criteria.Page = page;
            criteria.PageSize = pageSize;
            IQueryable<T> query = context.Set<T>();
            var result = await handler.ExecuteAsync(query, criteria);
            return result;
        }

        [Route("read"), HttpPost]
        public async Task<IQueryExecutionResult<T>> Read(
            [FromServices]AcmeContext context,
            [FromServices]IQueryHandlerAsync handler,
            [FromBody]IQueryCriteria criteria)
        {
            IQueryable<T> query = context.Set<T>();
            var result = await handler.ExecuteAsync(query, criteria);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<T>> Create([FromServices]AcmeContext context, [FromBody]T model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            context.Set<T>().Add(model);
            await context.SaveChangesAsync();
            return model;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<T>> Update([FromServices]AcmeContext context, [FromRoute]TKey id, [FromBody]T model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            context.Set<T>().Update(model);
            await context.SaveChangesAsync();
            return model;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromServices]AcmeContext context, [FromRoute]TKey id)
        {
            int next;
            lock (random)
            {
                next = random.Next(0, 2);
            }

            if (next == 0)
                return BadRequest(new Exception("BLEEE"));
            else if (next == 1)
                return BadRequest("Reeee 2");

            var model = context.Set<T>().Find(id);
            context.Set<T>().Remove(model);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
