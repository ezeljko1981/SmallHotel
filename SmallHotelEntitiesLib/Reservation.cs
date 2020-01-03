using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmallHotelEntitiesLib
{
    public class Reservation
    {
        public int ReservationID { get; set; }

        public int RoomID { get; set; }

        public int UserID { get; set; }

        [Column(TypeName ="date")]
        public DateTime ReservationStartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ReservationEndDate { get; set; }
    }
}
