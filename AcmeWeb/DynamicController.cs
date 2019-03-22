using AcmeWeb.Dal;
using Microsoft.AspNetCore.Mvc;
using PoweredSoft.DynamicQuery.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcmeWeb
{
    public class DynamicController<T> : Controller
        where T : class
    {
        [Route("api/[controller]"), HttpGet]
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

        [Route("api/[controller]/read"), HttpPost]
        public IQueryExecutionResult<T> Read(
            [FromServices]AcmeContext context,
            [FromServices]IQueryHandler handler,
            [FromBody]IQueryCriteria criteria)
        {
            IQueryable<T> query = context.Set<T>();
            var result = handler.Execute(query, criteria);
            return result;
        }
    }
}
