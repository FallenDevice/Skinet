using API.ReqestHelpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller
{

    [ApiController]
    [Route("api/[Controller]")]
    public class BaseApiController : ControllerBase
    {
        
        protected async Task<ActionResult> CreatePagedResult<T>(IGenericRepository<T> repository, 
        ISpecification<T> specification, int pageIndex, int pageSize) where T : BaseEntity
        {
            var items = await repository.ListAsyncWithSpec(specification);
            var count = await repository.CountAsync(specification);

            var pagination = new Pagination<T>(pageIndex, pageSize, count, items);

            return Ok(pagination);
        }

    }

}