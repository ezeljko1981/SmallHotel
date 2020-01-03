using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using SmallHotelContextLib;
using SmallHotelEntitiesLib;
using System.Text;

namespace SmallHotelService.Controllers
{
    [Route("api/[controller]")]
    public class UsersController
    {
        // GET api/users
        [HttpGet]
        public string Get()
        {
            List<User> listOfUsers = new List<User>();
            using (var db = new SmallHotelDB())
            {
                IQueryable<User> users = db.Users;
                string item = string.Empty;
                foreach (User u in users)
                {
                    listOfUsers.Add(u);
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(listOfUsers);
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            using (var db = new SmallHotelDB())
            {
                IQueryable<User> users = db.Users.Where(u => u.UserID == id);
                foreach (User u in users)
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(u);                 
                }
                return "No user!";
            }
        }
    }
}
