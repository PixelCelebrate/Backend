using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PixelCelebrateBackend.Database;
using PixelCelebrateBackend.Models;
using System;
using System.Data;
using System.Globalization;
using System.Xml.Linq;

namespace PixelCelebrateBackend.Controllers
{
    //Spatiu namespace;
    //Trebuie clase;

    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        //Spatiu clasa;
        //Metode / Campuri;

        //Baza de date:
        PixelCelebrateContext pixelCelebrateContext = new PixelCelebrateContext();
        //DbSet<User> userData = pixelCelebrateContext.Users;

        //Get pentru un user in functie de username si parola:
        [HttpPost]
        [Route("loginUserData")]
        public JsonResult LoginUser([FromBody] UserForLogin userLoginTest)
        {
            var userData = pixelCelebrateContext.Users;

            //Varianta sql:
            //var userId = userData.FromSqlInterpolated($"SELECT * FROM [User] WHERE UserName = {id} AND FirstName = {name}")
            //.FirstOrDefault();

            //Varianta lambda:
            var userTest = userData.SingleOrDefault(u => 
            String.Equals(u.UserName, userLoginTest.UserName) &&
            String.Equals(u.Password, userLoginTest.Password));

            //Este sau nu este acest user:
            if (userTest != null)
            {
                //return new JsonResult("User is logged in!");
                return new JsonResult(userTest);
            }
            else
            {
                //Sau null:
                return new JsonResult("There is not such user!");
            }
        }
    }
}
