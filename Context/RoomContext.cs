using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class RoomContext : Room, IRoom
    {
        public List<Room> AllRooms()
        {
            List<Room> allRooms = new List<Room>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataRooms = (MySqlDataReader)new DBConnection().Query("SELECT * FROM Rooms", connection);
                while (dataRooms.Read())
                {
                    Room newRoom = new Room();
                    newRoom.Id = dataRooms.GetInt32(0);
                    newRoom.Name = dataRooms.GetString(1);
                    newRoom.ShortName = dataRooms.GetString(2);
                    newRoom.ResponsibleId = dataRooms.IsDBNull(3) ? (int?)null : dataRooms.GetInt32(3);
                    newRoom.TempResponsibleId = dataRooms.IsDBNull(4) ? (int?)null : dataRooms.GetInt32(4);
                    allRooms.Add(newRoom);
                }
            }
            return allRooms;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("UPDATE Rooms " +
                        "SET " +
                        $"Name = '{this.Name}', " +
                        $"ShortName = '{this.ShortName}', " +
                        $"ResponsibleId = {(this.ResponsibleId.HasValue ? this.ResponsibleId.ToString() : "NULL")}, " +
                        $"TempResponsibleId = {(this.TempResponsibleId.HasValue ? this.TempResponsibleId.ToString() : "NULL")} " +
                        $"WHERE Id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO Rooms " +
                        "(Name, ShortName, ResponsibleId, TempResponsibleId) " +
                        "VALUES (" +
                        $"'{this.Name}', " +
                        $"'{this.ShortName}', " +
                        $"{(this.ResponsibleId.HasValue ? this.ResponsibleId.ToString() : "NULL")}, " +
                        $"{(this.TempResponsibleId.HasValue ? this.TempResponsibleId.ToString() : "NULL")})", connection);
                }
            }
        }

        public void Delete()
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM Rooms WHERE Id = {this.Id}", connection);
            }
        }
    }
}