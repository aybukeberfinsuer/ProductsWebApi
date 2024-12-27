using Microsoft.AspNetCore.Mvc;

namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController:ControllerBase{

        private static readonly string[] Products= {
            "IPhone 14", "IPhone 15", "IPhone 16"
    };
        [HttpGet]
        public string[] GetProducts(){

        return Products;
        }

        [HttpGet("{id}")]
        public string GetProducts(int id){
         return Products[1];
        }


    }

}