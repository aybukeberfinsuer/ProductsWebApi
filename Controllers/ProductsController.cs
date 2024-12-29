using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.DTO;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext _context;

        public ProductsController(ProductContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products
                .Where(i => i.IsActive)
                .ToListAsync();

            var productDtos = products.Select(p => ProductToDto(p)).ToList();
            return Ok(productDtos);
        }

      [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int? id)
        {
            if (id == null)
            {
                return NotFound(new { message = "Product ID is null" });
            }

            // Log the received ID
            Console.WriteLine($"Received Product ID: {id}");

            var product = await _context.Products
                .Where(i => i.ProductId == id)
                .Select(p => ProductToDto(p))
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product entity)
        {
            _context.Products.Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = entity.ProductId }, entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product entity)
        {
            if (id != entity.ProductId)
            {
                return BadRequest();
            }

            var oldObject = await _context.Products.FirstOrDefaultAsync(i => i.ProductId == id);
            if (oldObject == null)
            {
                return NotFound();
            }

            oldObject.ProductName = entity.ProductName;
            oldObject.Price = entity.Price;
            oldObject.IsActive = entity.IsActive;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(i => i.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        private static ProductDto ProductToDto(Product p)
        {
            var entity= new ProductDto();
            if(p!=null){
                entity.ProductId= p.ProductId;
                entity.ProductName=p.ProductName;
                entity.Price=p.Price;
            };

            return entity;
            
        }
    }
}
