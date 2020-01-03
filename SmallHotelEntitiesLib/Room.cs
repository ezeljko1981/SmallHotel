using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmallHotelEntitiesLib
{
    public class Room
    {
        public int RoomID { get; set; }

        public string RoomName { get; set; }

        [Column(TypeName = "smallint")]
        public int RoomCapacity { get; set; }

        public bool RoomWiFi { get; set; }

        public bool RoomMinibar { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
