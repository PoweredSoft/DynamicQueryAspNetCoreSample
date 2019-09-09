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
        [HttpGet]
        public IQueryExecutionResult<T> Get(
            [FromServices]AcmeContext context, 
            [FromServices]IQueryHandler handler, 
            [FromServices]IQueryCriteria criteria,
            int? page = null,
            int? pageSize = null)
        {
            criteria.Page = page;
            criteria.PageSize = pageSize;
            IQueryable<T> query = context.Set<T>();
            var result = handler.Execute(query, criteria);
            return result;
        }

        [Route("read"), HttpPost]
        public IQueryExecutionResult<T> Read(
            [FromServices]AcmeContext context,
            [FromServices]IQueryHandler handler,
            [FromBody]IQueryCriteria criteria)
        {
            IQueryable<T> query = context.Set<T>();
            var result = handler.Execute(query, criteria);
            return result;
        }

        [HttpPost]
        public T Create([FromServices]AcmeContext context, [FromBody]T model)
        {
            context.Set<T>().Add(model);
            context.SaveChanges();
            return model;
        }

        [HttpPut("{id}")]
        public T Update([FromServices]AcmeContext context, [FromRoute]TKey id, [FromBody]T model)
        {
            context.Set<T>().Update(model);
            context.SaveChanges();
            return model;
        }

        [HttpDelete("{id}")]
        public void Delete([FromServices]AcmeContext context, [FromRoute]TKey id)
        {
            var model = context.Set<T>().Find(id);
            context.Set<T>().Remove(model);
            context.SaveChanges();
        }
    }
}
