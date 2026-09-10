using API.ReqestHelpers;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller
{

    public class ProductsController(IGenericRepository<Product> repository) : BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(
            [FromQuery] ProductSpecParams specParams)
        {
            var spec = new ProductSpecification(specParams);

            return Ok(await CreatePagedResult(repository, spec, specParams.PageIndex, specParams.PageSize));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await repository.GetByIdAsync(id);

            if (product == null) return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            repository.Add(product);

            if (await repository.SaveAllAsync())
            {
                return Created("GetProduct", new {id = product.Id});
            }

            return BadRequest("Problem creating product!");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id))
            {
                return BadRequest("Cannot update this product!");
            }

            repository.Update(product);

            if (await repository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem updating product!");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await repository.GetByIdAsync(id);
            
            if (product == null)
            {
                return NotFound();
            }

            repository.Remove(product);
            if (await repository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem deleting product!");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<String>>> GetBrands()
        {   
            var spec = new BrandListSpecification();

            return Ok(await repository.ListAsyncWithSpec(spec));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<String>>> GetTypes()
        {
            var spec = new TypeListSpecification();

            return Ok(await repository.ListAsyncWithSpec(spec));
        }

        private bool ProductExists(int id)
        {
            return repository.Exists(id);
        }

    }

}
