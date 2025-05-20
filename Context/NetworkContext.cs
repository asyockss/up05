using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;

namespace inventory.Context.MySql
{
    public class NetworkContext : Network, INetwork
    {
        public List<Network> AllNetworks()
        {
            List<Network> allNetworks = new List<Network>();
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlDataReader dataNetworks = (MySqlDataReader)new DBConnection().Query("SELECT * FROM Networks", connection);
                while (dataNetworks.Read())
                {
                    Network newNetwork = new Network();
                    newNetwork.Id = dataNetworks.GetInt32(0);
                    newNetwork.EquipmentId = dataNetworks.GetInt32(1);
                    newNetwork.IpAddress = dataNetworks.GetString(2);
                    newNetwork.SubnetMask = dataNetworks.GetString(3);
                    newNetwork.Gateway = dataNetworks.GetString(4);
                    newNetwork.DnsServers = dataNetworks.GetString(5);
                    allNetworks.Add(newNetwork);
                }
            }
            return allNetworks;
        }

        public void Save(bool Update = false)
        {
            if (Update)
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("UPDATE Networks " +
                        "SET " +
                        $"EquipmentId = {this.EquipmentId}, " +
                        $"IpAddress = '{this.IpAddress}', " +
                        $"SubnetMask = '{this.SubnetMask}', " +
                        $"Gateway = '{this.Gateway}', " +
                        $"DnsServers = '{this.DnsServers}' " +
                        $"WHERE Id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO Networks " +
                        "(EquipmentId, IpAddress, SubnetMask, Gateway, DnsServers) " +
                        "VALUES (" +
                        $"{this.EquipmentId}, " +
                        $"'{this.IpAddress}', " +
                        $"'{this.SubnetMask}', " +
                        $"'{this.Gateway}', " +
                        $"'{this.DnsServers}')", connection);
                }
            }
        }

        public void Delete()
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM Networks WHERE Id = {this.Id}", connection);
            }
        }
    }
}