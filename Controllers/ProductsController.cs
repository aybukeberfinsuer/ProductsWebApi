using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController:ControllerBase{

        private static List<Product>  _products;

        public ProductsController()
        {

            _products=
            [
                new Product{ ProductId=1, ProductName="IPhone 14", Price=60000,IsActive=true },
                new Product{ ProductId=2, ProductName="IPhone 15", Price=70000,IsActive=true },
                new Product{ ProductId=3, ProductName="IPhone 16", Price=80000,IsActive=true },
            ];

           
        }
        [HttpGet]
        public List<Product> GetProducts(){

        return _products ?? new List<Product>();
        }

        [HttpGet("{id}")]
        public Product GetProducts(int id){
            return _products?.FirstOrDefault(x => x.ProductId == id) ?? new Product();
        }


    }

}