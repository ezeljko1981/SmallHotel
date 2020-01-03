using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using SmallHotelContextLib;
using SmallHotelEntitiesLib;
using System.Text;

namespace SmallHotelService.Controllers
{
    [Route("api/[controller]")]
    public class RoomsController
    {
        // GET api/rooms
        [HttpGet]
        public string Get()
        {
            StringBuilder sb = new StringBuilder();
            List<Room> listOfRooms = new List<Room>();
            using (var db = new SmallHotelDB())
            {
                IQueryable<Room> rooms = db.Rooms;
                string item = string.Empty;
                foreach (Room r in rooms)
                {
                    listOfRooms.Add(r);
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(listOfRooms);
        }

        // GET api/rooms/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            using (var db = new SmallHotelDB())
            {
                IQueryable<Room> rooms = db.Rooms.Where(r => r.RoomID == id);
                foreach (Room r in rooms)
                {
                    return Newtonsoft.Json.JsonConvert.SerializeObject(r);
                }
                return "No room!";
            }
        }

    }
}
