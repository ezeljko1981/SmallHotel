using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using SmallHotelContextLib;
using SmallHotelEntitiesLib;
using System.Text;
using System;
using Microsoft.EntityFrameworkCore;

namespace SmallHotelService.Controllers
{
    [Route("api/[controller]")]
    public class ReservationsController
    {
        // GET api/reservations
        [HttpGet]
        public string Get()
        {
            List<ReservationExtended> listOfReservationExtended = new List<ReservationExtended>();
            using (var db = new SmallHotelDB())
            {
                var users = db.Users.Select(u => new { u.UserID, u.UserName }).ToArray();
                var rooms = db.Rooms.Select(r => new { r.RoomID, r.RoomName }).ToArray();
                var reservations = db.Reservations.Select(r => new { r.ReservationID, r.RoomID, r.UserID, r.ReservationStartDate, r.ReservationEndDate });

                var queryUserReservation = users.Join(reservations,
                    user => user.UserID,
                    reservation => reservation.UserID,
                    (u, r) => new { r.ReservationID, u.UserID, u.UserName, r.RoomID, r.ReservationStartDate, r.ReservationEndDate });

                var queryUserReservationRoom = rooms.Join(queryUserReservation,
                    room => room.RoomID,
                    qur => qur.RoomID,
                    (r, ur) => new { ur.ReservationID, r.RoomID, ur.UserID, r.RoomName, ur.UserName, ur.ReservationStartDate, ur.ReservationEndDate }
                    );

                foreach (var item in queryUserReservationRoom)
                {
                    ReservationExtended re = new ReservationExtended
                    {
                        ReservationID = item.ReservationID,
                        RoomID = item.RoomID,
                        UserID = item.UserID,
                        ReservationStartDate = item.ReservationStartDate,
                        ReservationEndDate = item.ReservationEndDate,
                        RoomName = item.RoomName,
                        UserName = item.UserName
                    };
                    listOfReservationExtended.Add(re);
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(listOfReservationExtended);
            }
        }

        // GET api/reservations/2019/8/18/2019/9/5
        [HttpGet("{startYear}/{startMonth}/{startDay}/{endYear}/{endMonth}/{endDay}")]
        public string Get(int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay)
        {
            DateTime startDateTime = new DateTime(startYear, startMonth, startDay, 14, 0, 0);
            DateTime endDateTime = new DateTime(endYear, endMonth, endDay, 10, 0, 0);
            String result = String.Empty;
            using (var db = new SmallHotelDB())
            {
                var reservations = db.Reservations.Select(r => new { r.RoomID, r.UserID, r.ReservationStartDate, r.ReservationEndDate });
                var occupiedRooms = db.Reservations
                    .Where(r => r.ReservationStartDate < endDateTime && startDateTime < r.ReservationEndDate)
                    .Select(r => new { r.RoomID }).Distinct().ToList();
                var allRooms = db.Rooms.Select(r => new { r.RoomID }).Distinct().ToList();
                var freeRooms = allRooms.Except(occupiedRooms).Distinct().ToList();
                var rooms = db.Rooms.Select(r => new { r.RoomID, r.RoomName }).ToArray();
                var completeFreeRooms = db.Rooms.Join(freeRooms,
                    completeFreeRoom => completeFreeRoom.RoomID,
                    room => room.RoomID,
                    (c, r) => new { c.RoomID, c.RoomName }
                );
                result = Newtonsoft.Json.JsonConvert.SerializeObject(completeFreeRooms);
            }
            return result;
        }

        //Save new reservation
        [HttpGet("{userName}/{roomID}/{startYear}/{startMonth}/{startDay}/{endYear}/{endMonth}/{endDay}")]
        public string Get(string userName, int roomID, int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay)
        {
            using (var db = new SmallHotelDB())
            {
                //id if found, otherwise -1 
                int userId = -1;
                IQueryable<User> users = db.Users.Where(u => EF.Functions.Like(u.UserName, userName));
                if (users.Count() == 0)
                {
                    db.Users.Add(new User { UserName = userName });
                    db.SaveChanges();
                    users = db.Users.Where(u => EF.Functions.Like(u.UserName, userName));
                }
                User user = users.ToArray<User>()[0];
                userId = user.UserID;

                DateTime startDateTime = new DateTime(startYear, startMonth, startDay, 14, 0, 0);
                DateTime endDateTime = new DateTime(endYear, endMonth, endDay, 10, 0, 0);
                Reservation reservation = new Reservation { UserID = userId, RoomID = roomID, ReservationStartDate = startDateTime, ReservationEndDate = endDateTime };
                db.Reservations.Add(reservation);
                int affected = db.SaveChanges();
                String response = "Total " + affected.ToString() + " reservation made";
                return Newtonsoft.Json.JsonConvert.SerializeObject(response);
            }
        }
    }
}
