using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController:ControllerBase{

        private readonly ProductContext  _context;

        public ProductsController(ProductContext context)
        {
            _context=context;
                      
        }
        [HttpGet]
        public async Task<IActionResult> GetProducts(){

            var products=  await _context.Products.ToListAsync();
            return Ok(products);
       
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProducts(int? id){
           if(id==null){
            return NotFound();
           }

            var p = await _context.Products.FirstOrDefaultAsync(i => i.ProductId == id);

           if( p==null){
            return NotFound();
           }

           return Ok(p);

        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product entity){

                _context.Products.Add(entity);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetProducts), new {id = entity.ProductId},entity);


        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id,Product entity){

            if(id== entity.ProductId){
                 return BadRequest();
            }

            var oldObject= await _context.Products.FirstOrDefaultAsync(i=>i.ProductId==id);
            if(oldObject==null){
                return NotFound();
            }


            oldObject.ProductName= entity.ProductName;
            oldObject.Price=entity.Price;
            oldObject.IsActive=entity.IsActive;

            
            try{
                await _context.SaveChangesAsync();
            }
            catch(Exception E){
                return NotFound();
            }

            return NoContent();

        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id){
            if(id==null){
                return NotFound();
            }

            var product= await _context.Products.FirstOrDefaultAsync(i=>i.ProductId==id);
            if(product==null){
                    return NotFound();
            }

            _context.Products.Remove(product);

            try{
                await _context.SaveChangesAsync();
            }
            catch(Exception e){

                return NotFound();
            }
            return NoContent();

        }
    }

}