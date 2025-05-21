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
                MySqlDataReader dataNetworks = (MySqlDataReader)new DBConnection().Query("SELECT * FROM network", connection);
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
                    new DBConnection().Query("UPDATE network " +
                        "SET " +
                        $"equipment_id  = {this.EquipmentId}, " +
                        $"ip_address = '{this.IpAddress}', " +
                        $"subnet_mask = '{this.SubnetMask}', " +
                        $"gateway = '{this.Gateway}', " +
                        $"dns_servers = '{this.DnsServers}' " +
                        $"WHERE id = {this.Id}", connection);
                }
            }
            else
            {
                using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
                {
                    new DBConnection().Query("INSERT INTO network " +
                        "(equipment_id , ip_address, subnet_mask, gateway, dns_servers) " +
                        "VALUES (" +
                        $"{this.EquipmentId}, " +
                        $"'{this.IpAddress}', " +
                        $"'{this.SubnetMask}', " +
                        $"'{this.Gateway}', " +
                        $"'{this.DnsServers}')", connection);
                }
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM network WHERE id = {this.Id}", connection);
            }
        }
    }
}