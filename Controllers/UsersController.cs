using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProductsAPI.DTO;
using ProductsAPI.Models;

namespace ProductsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController:ControllerBase{
        private UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        private IConfiguration _configuration;

        public UsersController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,IConfiguration configuration){
            _userManager=userManager;
            _signInManager=signInManager;
            _configuration=configuration;
        } 

        [HttpPost("register")]

        public async Task<IActionResult> CreateUser (UserDto model){
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var user= new AppUser{
                FullName= model.FullName,
                UserName=model.UserName,
                Email=model.Email,
                DateAdded= DateTime.Now
            };

            var result= await _userManager.CreateAsync(user,model.Password);
            if(result.Succeeded){
                return StatusCode(201);
            }
            return BadRequest(result.Errors);
            
           

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model){

            var user= await _userManager.FindByEmailAsync(model.Email);
            if(user==null){
                return BadRequest( new{message="incorrect username"});

            }

            var result = await _signInManager.CheckPasswordSignInAsync(user,model.Password,false);

            if(result.Succeeded){
                return Ok(new{ token=GenrateJWT(user)});
            }

            return Unauthorized();

    }

    private object GenrateJWT(AppUser user){

    var tokenHandler = new  JwtSecurityTokenHandler();
    var key= Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Secret").Value ?? "");
   var tokenDescriptor= new SecurityTokenDescriptor{
    Subject= new ClaimsIdentity( new Claim[] {
        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
        new Claim(ClaimTypes.Name,user.UserName ?? ""),
    }),
    Expires=DateTime.UtcNow.AddDays(1),
    SigningCredentials= new SigningCredentials( new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)

   };

   var token=tokenHandler.CreateToken(tokenDescriptor);
   return tokenHandler.WriteToken(token);
   
   
   
    }

    }
    

    
}