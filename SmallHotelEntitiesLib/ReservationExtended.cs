using System;
using System.Collections.Generic;
using System.Text;

namespace SmallHotelEntitiesLib
{
    public class ReservationExtended : Reservation
    {
        public string RoomName { get; set; }

        public string UserName { get; set; }
    }
}
