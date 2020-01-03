using SmallHotelContextLib;
using System;
using static System.Console;
using System.Linq;
using SmallHotelEntitiesLib;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            getAllRooms();
            getAllUsers();
            getAllReservations();
            getAllReservationsJoin();
            getFreeRoomsForAPeriod(2019, 8, 18, 2019, 9, 5);
            Console.ReadKey();
        }

        private static void getFreeRoomsForAPeriod(int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay)
        {
            WriteLine("List of free rooms: ");
            DateTime startDateTime = new DateTime(startYear, startMonth, startDay, 14, 0, 0);
            DateTime endDateTime = new DateTime(endYear, endMonth, endDay, 10, 0, 0);
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

                String resultJson = Newtonsoft.Json.JsonConvert.SerializeObject(completeFreeRooms);

                //test reports
                foreach (var item in occupiedRooms)
                {
                    WriteLine($"Room \"{item.RoomID}\" is occupied ");
                }

                foreach (var item in allRooms)
                {
                    WriteLine($"Room \"{item.RoomID}\" exists ");
                }

                foreach (var item in freeRooms)
                {
                    WriteLine($"Room \"{item.RoomID}\" is free ");
                }

                foreach (var item in completeFreeRooms)
                {
                    WriteLine($"Free room ID\"{item.RoomID}\" has name {item.RoomName}");
                }

                WriteLine(resultJson);

            }
        }

        static void getAllRooms()
        {
            WriteLine("List of SmallHotelDB rooms: ");
            using (var db = new SmallHotelDB())
            {
                IQueryable<Room> rooms = db.Rooms;
                foreach (Room r in rooms)
                {
                    WriteLine($"{r.RoomID} has name {r.RoomName} for {r.RoomCapacity} people with wifi {r.RoomWiFi} and minibar {r.RoomMinibar}");
                }
            }
        }

        static void getAllUsers()
        {
            WriteLine("List of SmallHotelDB users: ");
            using (var db = new SmallHotelDB())
            {
                IQueryable<User> users = db.Users
                    .Include(u => u.Reservations);
                foreach (User u in users)
                {
                    WriteLine($" {u.UserID} : {u.UserName} has {u.Reservations.Count()} reservations.");
                }
            }
        }

        static void getAllReservations()
        {
            WriteLine("List of SmallHotelDB reservations: ");
            using (var db = new SmallHotelDB())
            {
                IQueryable<Reservation> reservations = db.Reservations;
                foreach (Reservation r in reservations)
                {
                    WriteLine($"{r.ReservationID} in room {r.RoomID} was reserved from {r.ReservationStartDate} to {r.ReservationEndDate} by {r.UserID}");
                }
            }
        }

        static void getAllReservationsJoin()
        {
            WriteLine("List of SmallHotelDB reservations: ");
            using (var db = new SmallHotelDB())
            {
                var users = db.Users.Select(u => new { u.UserID, u.UserName }).ToArray();
                var rooms = db.Rooms.Select(r => new { r.RoomID, r.RoomName }).ToArray();
                var reservations = db.Reservations.Select(r => new { r.RoomID, r.UserID, r.ReservationStartDate, r.ReservationEndDate });

                var queryUserReservation = users.Join(reservations,
                    user => user.UserID,
                    reservation => reservation.UserID,
                    (u,r) => new { u.UserName, r.RoomID, r.ReservationStartDate, r.ReservationEndDate });

                var queryUserReservationRoom = rooms.Join(queryUserReservation,
                    room => room.RoomID,
                    qur=> qur.RoomID,
                    (r, ur) => new { r.RoomName, ur.UserName, ur.ReservationStartDate, ur.ReservationEndDate}
                    );

                foreach (var item in queryUserReservationRoom)
                {
                    WriteLine($"User \"{item.UserName}\" is in room \"{item.RoomName}\" from {item.ReservationStartDate} to {item.ReservationEndDate}");
                }

            }
        }

        static void serializeUser(User u)
        {
            string output = String.Empty;
            JsonSerializer serializer = new JsonSerializer();
        }

    }
}
