using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PixelCelebrateBackend.Database;
using PixelCelebrateBackend.Models;
using System;
using System.IO;
using System.Data;
using System.Globalization;
using System.Xml.Linq;

namespace PixelCelebrateBackend.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        PixelCelebrateContext pixelCelebrateContext = new PixelCelebrateContext();

        //Get Users: All the data available:
        [HttpGet]
        [Route("getUserData")]
        public JsonResult GetUserData()
        {
            var userData = pixelCelebrateContext.Users;

            //No array list:
            //Merge cu lista pentru primire:
            List<UserAdminTable> userAdminData = new List<UserAdminTable>();

            foreach (var allUserData in userData)
            {
                userAdminData.Add(new UserAdminTable(
                    allUserData.FirstName,
                    allUserData.LastName,
                    allUserData.UserName,
                    allUserData.Password,
                    allUserData.Birthday,
                    allUserData.Role,
                    allUserData.Email,
                    allUserData.DateOfJoining));
            }

            return new JsonResult(userAdminData);
        }


        //Get Admin:


        //Get Angajat:


        //Add:
        [HttpPost]
        [Route("addUser")]
        public JsonResult AddUser([FromBody] string newUser)
        {
            //Users that are already there:
            var userData = pixelCelebrateContext.Users;

            //Get new id:
            //var newIdNumber = userData.FromSqlInterpolated($"SELECT * FROM [User]").Count() + 1;
            var random = new Random();
            var newIdNumber = random.Next(); //Altfel nu merge bine;

            //Verific unicitate:
            var flag = 0;
            while(flag == 0)
            {
                //Kepp trying:
                foreach (var allUserData in userData)
                {
                    if (allUserData.UserId == newIdNumber)
                    {
                        flag = 1;
                        break;
                    }
                }

                //Again:
                if(flag == 1)
                {
                    newIdNumber = random.Next();
                    flag = 0;
                }
                //Exit:
                else
                {
                    flag = 1;
                }
            }

            Console.WriteLine($"\nUser Data: {newIdNumber}");

            //Get user:
            string[] newUserData = newUser.Split(' ', ' ');

            //Type add user:
            Console.WriteLine("");
            foreach (var allUserData in newUserData)
            {
                Console.WriteLine($"User Data: {allUserData}");
            }

            //Get new user:
            var newUserObject = new UserEntity();
            newUserObject.UserId = newIdNumber;
            newUserObject.FirstName = newUserData[0];
            newUserObject.LastName = newUserData[1];
            newUserObject.UserName = newUserData[2];
            newUserObject.Password = newUserData[3];
            //DateTime birthday = DateTime.ParseExact(newUserData[4], "yyyy-MM-ddTHH:mm:ss:fffZ",
            //                           System.Globalization.CultureInfo.InvariantCulture);
            //Console.WriteLine($"Check: {newUserData[4]} .");
            //DateTime birthday = DateTime.ParseExact(newUserData[4], "d", null);
            //CultureInfo culture1 = new CultureInfo("en-US");
            //DateTime birthday = Convert.ToDateTime(newUserData[4], culture1);

            DateTime birthday;
            //Luna si Zi care incep cu 0: Fara ani sub 1000:
            //if (newUserData[4][0] == '0' && ...);

            /*
            if (newUserData[4][5] == '0' && newUserData[4][8] == '0')
            {
                birthday = new DateOnly(int.Parse(newUserData[4].Substring(0, 3)),
                                        newUserData[4][6] - '0',
                                        newUserData[4][9] - '0');
            }
            else if (newUserData[4][5] == '0' && newUserData[4][8] != '0')
            {
                birthday = new DateOnly(int.Parse(newUserData[4].Substring(0, 3)),
                                        newUserData[4][6] - '0',
                                        int.Parse(newUserData[4].Substring(8, 9)));
            }
            else if (newUserData[4][5] != '0' && newUserData[4][8] == '0')
            {
                birthday = new DateOnly(int.Parse(newUserData[4].Substring(0, 3)),
                                        int.Parse(newUserData[4].Substring(5, 6)),
                                        newUserData[4][9] - '0');
            }
            else if (newUserData[4][5] != '0' && newUserData[4][8] != '0')
            {
                birthday = new DateOnly(int.Parse(newUserData[4].Substring(0, 3)),
                                        int.Parse(newUserData[4].Substring(5, 6)),
                                        int.Parse(newUserData[4].Substring(8, 9)));
            }
            */

            //De la pozitie + length:
            Console.WriteLine(newUserData[4].Substring(0, 4));
            Console.WriteLine(newUserData[4].Substring(5, 2));
            Console.WriteLine(newUserData[4].Substring(8, 2));
            birthday = new DateTime(Convert.ToInt32(newUserData[4].Substring(0, 4)),
                                    Convert.ToInt32(newUserData[4].Substring(5, 2)),
                                    Convert.ToInt32(newUserData[4].Substring(8, 2)),
                                    0, 0, 0);
            //int.Parse(newUserData[4].Substring(11, 12),
            //int.Parse(newUserData[4].Substring(14, 15),
            //int.Parse(newUserData[4].Substring(16, 17));
            newUserObject.Birthday = birthday;
            newUserObject.Role = newUserData[5];
            newUserObject.Email = newUserData[6];
            //DateTime dateOfJoining = DateTime.ParseExact(newUserData[7], "yyyy-MM-ddTHH:mm:ss:fffZ",
            //                           System.Globalization.CultureInfo.InvariantCulture);
            //Console.WriteLine($"Check: {newUserData[7]} .");
            //DateTime dateOfJoining = DateTime.ParseExact(newUserData[7], "d", null);

            DateTime dateOfJoining = new DateTime(Convert.ToInt32(newUserData[7].Substring(0, 4)),
                                                  Convert.ToInt32(newUserData[7].Substring(5, 2)),
                                                  Convert.ToInt32(newUserData[7].Substring(8, 2)),
                                                  0, 0, 0);
            newUserObject.DateOfJoining = dateOfJoining;

            //Print user data: Schimba vizual cum este aranjat:
            Console.WriteLine($"Birthday: {newUserObject.Birthday} .");
            Console.WriteLine($"Date of joining: {newUserObject.DateOfJoining} .");


            //Verificare daca exista deja username sau email:
            foreach (var allUserData in userData)
            {
                if (String.Equals(newUserObject.UserName, allUserData.UserName) == true
                    || String.Equals(newUserObject.Email, allUserData.Email) == true)
                {
                    return new JsonResult("The username (or email) is already taken.");
                }
            }


            //Get Table User:
            var tableUser = new User(newUserObject.UserId, newUserObject.FirstName, newUserObject.LastName,
                                     newUserObject.UserName, newUserObject.Password, newUserObject.Birthday,
                                     newUserObject.Role, newUserObject.Email, newUserObject.DateOfJoining);

            //Insert user:
            //var userId = userData.FromSqlInterpolated($"INSERT into [User] values({newIdNumber}, {newUserData[0]}, {newUserData[1]}, {newUserData[2]}, {newUserData[3]}, {newUserData[4]}, {newUserData[5]}, {newUserData[6]}, {newUserData[7]})");
            /*
            if (userId != null)
            {
                Console.WriteLine($"\nUser id exists!");
                //Console.WriteLine(userId);
            }
            else
            {
                Console.WriteLine($"\nNo User id!");
            }
            */

            //Insert user: Table user:
            userData.Add(tableUser);
            //userData.FromSqlInterpolated($"INSERT into [User] values({newUserObject})");
            pixelCelebrateContext.SaveChanges();

            //Return message:
            return new JsonResult("Added new user.");
        }


        //Update:
        [HttpPost]
        [Route("updateUser")]
        public JsonResult UpdateUser([FromBody] string updateUser)
        {
            //Users that are already there:
            var userData = pixelCelebrateContext.Users;

            //Get user:
            string[] updateUserData = updateUser.Split(' ', ' ');

            //Type update user:
            Console.WriteLine("");
            foreach (var allUserData in updateUserData)
            {
                Console.WriteLine($"User Data: {allUserData}");
            }


            //Get update user:
            var updateUserObject = new UserEntity();
            updateUserObject.FirstName = updateUserData[0];
            updateUserObject.LastName = updateUserData[1];
            updateUserObject.UserName = updateUserData[2];
            updateUserObject.Password = updateUserData[3];

            //Birthday diferit:
            DateTime birthday;
            //No birthdate:
            if (String.Equals(updateUserData[4], "nothing") == false)
            {
                //De la pozitie + length:
                birthday = new DateTime(Convert.ToInt32(updateUserData[4].Substring(0, 4)),
                                        Convert.ToInt32(updateUserData[4].Substring(5, 2)),
                                        Convert.ToInt32(updateUserData[4].Substring(8, 2)),
                                        0, 0, 0);
                updateUserObject.Birthday = birthday;
            }
            else
            {
                //Implicit:
                updateUserObject.Birthday = new DateTime(1, 1, 1, 0, 0, 0);
            }

            updateUserObject.Role = updateUserData[5];
            updateUserObject.Email = updateUserData[6];

            //Date of Joining diferit:
            DateTime dateOfJoining; ;
            //No joindate:
            if (String.Equals(updateUserData[7], "nothing") == false)
            {
                //De la pozitie + length:
                dateOfJoining = new DateTime(Convert.ToInt32(updateUserData[7].Substring(0, 4)),
                                             Convert.ToInt32(updateUserData[7].Substring(5, 2)),
                                             Convert.ToInt32(updateUserData[7].Substring(8, 2)),
                                             0, 0, 0);
                updateUserObject.DateOfJoining = dateOfJoining;
            }
            else
            {
                updateUserObject.DateOfJoining = new DateTime(1, 1, 1, 0, 0, 0);
            }


            //Verificare daca exista deja userul:
            //foreach (var allUserData in userData)
            //{
            //    if (String.Equals(newUserObject.UserName, allUserData.UserName) == true)
            //    {
            //        return new JsonResult("The username was found.");
            //    }
            //}

            //Verificare daca exista userul dupa username + adaugare campuri daca sunt bune:
            var finalUpdateForUser = userData.SingleOrDefault(u => u.UserName == updateUserObject.UserName);
            if (finalUpdateForUser != null)
            {
                //Verificare daca trebuie sau nu adaugat la fiecare:
                if (String.Equals(updateUserObject.FirstName, "nothing") == false)
                {
                    finalUpdateForUser.FirstName = updateUserObject.FirstName;
                }
                if (String.Equals(updateUserObject.LastName, "nothing") == false)
                {
                    finalUpdateForUser.LastName = updateUserObject.LastName;
                }
                //if (String.Equals(updateUserObject.UserName, "nothing") == false)
                //{
                //    finalUpdateForUser.UserName = updateUserObject.UserName;
                //}
                if (String.Equals(updateUserObject.Password, "nothing") == false)
                {
                    finalUpdateForUser.Password = updateUserObject.Password;
                }
                if (updateUserObject.Birthday.CompareTo(new DateTime(1, 1, 1, 0, 0, 0)) != 0)
                {
                    //= simplu;
                    finalUpdateForUser.Birthday = updateUserObject.Birthday;
                }
                //Probabil se poate:
                if (String.Equals(updateUserObject.Role, "nothing") == false)
                {
                    finalUpdateForUser.Role = updateUserObject.Role;
                }
                if (String.Equals(updateUserObject.Email, "nothing") == false)
                {
                    finalUpdateForUser.Email = updateUserObject.Email;
                }
                if (updateUserObject.DateOfJoining.CompareTo(new DateTime(1, 1, 1, 0, 0, 0)) != 0)
                {
                    //= simplu;
                    finalUpdateForUser.DateOfJoining = updateUserObject.DateOfJoining;
                }

                //Save the db:
                pixelCelebrateContext.SaveChanges();

                return new JsonResult("Updated the user.");
            }
            else
            {
                return new JsonResult("Cannot find user by the username. Try a different username.");
            }
        }


        //Delete by username:
        [HttpPost]
        [Route("deleteUser")]
        public JsonResult DeleteUser([FromBody] string deleteUser)
        {
            //Users that are already there:
            var userData = pixelCelebrateContext.Users;

            //Verificare daca exista userul dupa username + stergere daca gaseste:
            var deleteUserFromTable = userData.SingleOrDefault(u => String.Equals(u.UserName, deleteUser));

            //Daca exista:
            if (deleteUserFromTable != null)
            {
                //Remove him and save:
                pixelCelebrateContext.Remove(userData.Single(u => String.Equals(u.UserName, deleteUser)));

                //Save the db:
                pixelCelebrateContext.SaveChanges();

                return new JsonResult("Deleted the user.");
            }
            //Nu gaseste:
            else
            {
                return new JsonResult("Cannot find user by the username. Try a different username.");
            }
        }


        //Select number of days:
        //Fisierul a fost creat:
        [HttpPost]
        [Route("selectNumberOfDays")]
        public JsonResult SelectNumberOfDays([FromBody] string numberOfDays)
        {
            //Scriere in fisier: String aici, Int dupa;

            StreamWriter writer = null;

            try
            {
                writer = new StreamWriter("../NumberOfDays.txt");

                //Write the number:
                writer.WriteLine(numberOfDays);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                //Close file:
                writer.Close();
            }

            Console.WriteLine($"New number of days: {numberOfDays} .");

            return new JsonResult("New number of days was set.");
        }


        //Get din fisier:
        [HttpGet]
        [Route("getNumberOfDays")]
        public JsonResult GetNumberOfDays()
        {
            //Citire din fisier:
            string dataFromFile = "";
            int dataFromFileNumber = 0;
            StreamReader reader = null;

            try
            {
                reader = new StreamReader("../NumberOfDays.txt");

                //Console.WriteLine("Data from file is: ");
                dataFromFile = reader.ReadLine();
                //Console.WriteLine(dataFromFile);

                //while (dataFromFile != null)
                //{
                //    Console.WriteLine(dataFromFile);
                //    dataFromFile = reader.ReadLine();
                //}
            }
            catch (Exception e)
            {
                //dataFromFile = "Error";
                Console.WriteLine(e.Message);
            }
            finally
            {
                //Close file:
                reader.Close();

                //Extract number:
                try
                {
                    dataFromFileNumber = Convert.ToInt32(dataFromFile);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return new JsonResult(dataFromFileNumber);
        }


        //Functie de trimis emails o data pe zi:
        //public static void SendEmails()
        ////public async void SendEmails()
        //{
        //    Console.WriteLine("Time now: " + DateTime.Now);
        //}

        //internal static void Run()
        //{
        //    //Every 30 seconds:
        //    int seconds = 30 * 1000;
        //    var timer = new Timer(SendEmails, null, 0, seconds);
        //    //Console.ReadKey();
        //}
    }
}

