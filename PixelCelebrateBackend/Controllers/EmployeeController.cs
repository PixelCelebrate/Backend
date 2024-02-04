using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PixelCelebrateBackend.Database;
using PixelCelebrateBackend.Models;
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Xml.Linq;

namespace PixelCelebrateBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        PixelCelebrateContext pixelCelebrateContext = new PixelCelebrateContext();


        //Get User Own Data:
        [HttpPost]
        [Route("userDataEmployee")]
        public JsonResult GetUserDataByUsername([FromBody] string userNameData)
        {
            var userData = pixelCelebrateContext.Users;

            //Get Angajat by username:
            var userNameDataFinal = userData.SingleOrDefault(u => String.Equals(u.UserName, userNameData));

            return new JsonResult(userNameDataFinal);
        }


        //Get Users:
        [HttpGet]
        [Route("getUserInfo")]
        public JsonResult GetUserInfo()
        {
            var userData = pixelCelebrateContext.Users;

            //No array list:
            List<UserEmployeeTable> userEmployeeData = new List<UserEmployeeTable>();

            //Get Angajat:
            foreach (var allUserData in userData)
            {
                //Doar employees:
                if(String.Equals(allUserData.Role, "Employee"))
                {
                    userEmployeeData.Add(new UserEmployeeTable(
                         allUserData.UserName,
                         allUserData.Email));
                }
            }

            //foreach (var allUserData in userData)
            //{
            //    userEmployeeData.Add(new UserAdminTable(
            //        allUserData.FirstName,
            //        allUserData.LastName,
            //        allUserData.UserName,
            //        allUserData.Password,
            //        allUserData.Birthday,
            //        allUserData.Role,
            //        allUserData.Email,
            //        allUserData.DateOfJoining));
            //}

            return new JsonResult(userEmployeeData);
        }
    }
}
