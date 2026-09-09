using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller
{

    [ApiController]
    [Route("api/[Controller]")]
    public class ProductsController(IProductRepository productsRepository) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
        {
            return Ok(await productsRepository.GetProductsAsync(brand, type, sort));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await productsRepository.GetProductByIdAsync(id);

            if (product == null) return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            productsRepository.AddProduct(product);

            if (await productsRepository.SaveChangesAsync())
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

            productsRepository.UpdateProduct(product);

            if (await productsRepository.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem updating product!");
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await productsRepository.GetProductByIdAsync(id);
            
            if (product == null)
            {
                return NotFound();
            }

            productsRepository.DeleteProduct(product);
            if (await productsRepository.SaveChangesAsync())
            {
                return NoContent();
            }

            return BadRequest("Problem deleting product!");
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<String>>> GetBrands()
        {
            return Ok(await productsRepository.GetBrandsAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<String>>> GetTypes()
        {
            return Ok(await productsRepository.GetTypesAsync());
        }

        private bool ProductExists(int id)
        {
            return productsRepository.ProductExists(id);
        }

    }

}
