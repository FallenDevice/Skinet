using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controller
{

    [ApiController]
    [Route("api/[Controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly StoreContext context;

        public ProductsController(StoreContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await this.context.Products.ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await this.context.Products.FindAsync(id);

            if (product == null) return NotFound();

            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            this.context.Products.Add(product);

            await this.context.SaveChangesAsync();

            return product;
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateProduct(int id, Product product)
        {
            if (product.Id != id || !ProductExists(id))
            {
                return BadRequest("Cannot update this product!");
            }

            this.context.Entry(product).State = EntityState.Modified;

            await this.context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await context.Products.FindAsync(id);
            
            if (product == null)
            {
                return NotFound();
            }

            this.context.Products.Remove(product);

            await this.context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return this.context.Products.Any(x => x.Id == id);
        }

    }

}
