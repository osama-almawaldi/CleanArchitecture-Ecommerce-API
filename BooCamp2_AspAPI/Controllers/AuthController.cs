using Bootcamp2_AspMVC.Data;
using Bootcamp2_AspMVC.Dtos;
using Bootcamp2_AspMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BootCamp2_AspAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    { 
        private readonly ApplicationDbContext _context;
        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("login")]
        public IActionResult Login([FromQuery] LoginRequest request)
        {

            var user = _context.Users.FirstOrDefault(e => e.Email == request.email && e.Password == request.Password);
            if (user != null)
            {
                return Ok(new { firstName = user.FirstName });
            }


            //// Dummy authentication logic
            //if (request.email == "user@gmail.com" && request.Password == "123456")
            //{
            //    return Ok(new { firstName = "Mohamed Alswaify" });
            //}
            return Unauthorized();
        }



        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDto request)
        {
            if(request == null)
            {
                return BadRequest(new { message = "Invalid registration details" });
            }
            if(ModelState.IsValid == false) return BadRequest(ModelState);

            var existingUser = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (existingUser != null)
            {
                return Conflict(new { message = "Email is already registered" });
            }

            var User = new User
            {
                BirthDate = request.BirthDate,
                Country = request.Country,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = request.Gender,
                Password = request.Password,
                Phone = request.Phone
            };

            try
            {
                _context.Users.Add(User);
                _context.SaveChanges();
                return Ok(new { message = "User registered successfully" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while registering the user" });


            }
        }




    }
}
