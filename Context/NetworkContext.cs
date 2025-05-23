using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using inventory.Interfase;
using inventory.Models;
using inventory.Context.Common;
using System.Text.RegularExpressions;

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

        public void Save(Network network, bool isUpdate)
        {
            using (var connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                var cmd = new MySqlCommand("SELECT COUNT(*) FROM network WHERE IpAddress = @IpAddress", connection);
                cmd.Parameters.AddWithValue("@IpAddress", network.IpAddress);
                if (isUpdate)
                {
                    cmd.CommandText += " AND Id != @Id";
                    cmd.Parameters.AddWithValue("@Id", network.Id);
                }
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    throw new Exception("IP-адрес уже используется.");
                }
                if (isUpdate)
                {
                    var updateCmd = new MySqlCommand("UPDATE network SET IpAddress = @IpAddress, SubnetMask = @SubnetMask, Gateway = @Gateway, DnsServers = @DnsServers, EquipmentId = @EquipmentId WHERE Id = @Id", connection);
                    updateCmd.Parameters.AddWithValue("@IpAddress", network.IpAddress);
                    updateCmd.Parameters.AddWithValue("@SubnetMask", network.SubnetMask);
                    updateCmd.Parameters.AddWithValue("@Gateway", network.Gateway);
                    updateCmd.Parameters.AddWithValue("@DnsServers", network.DnsServers);
                    updateCmd.Parameters.AddWithValue("@EquipmentId", network.EquipmentId);
                    updateCmd.Parameters.AddWithValue("@Id", network.Id);
                    updateCmd.ExecuteNonQuery();
                }
                else
                {
                    var insertCmd = new MySqlCommand("INSERT INTO network (IpAddress, SubnetMask, Gateway, DnsServers, EquipmentId) VALUES (@IpAddress, @SubnetMask, @Gateway, @DnsServers, @EquipmentId)", connection);
                    insertCmd.Parameters.AddWithValue("@IpAddress", network.IpAddress);
                    insertCmd.Parameters.AddWithValue("@SubnetMask", network.SubnetMask);
                    insertCmd.Parameters.AddWithValue("@Gateway", network.Gateway);
                    insertCmd.Parameters.AddWithValue("@DnsServers", network.DnsServers);
                    insertCmd.Parameters.AddWithValue("@EquipmentId", network.EquipmentId);
                    insertCmd.ExecuteNonQuery();
                }
            }
        }

        public void Save(bool update)
        {
            throw new NotImplementedException("This method is not supported. Use Save(Network network, bool isUpdate) instead.");
        }

        private bool IsValidIpAddress(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return false;
            string pattern = @"^(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})$";
            if (!Regex.IsMatch(ip, pattern)) return false;
            var parts = ip.Split('.');
            foreach (var part in parts)
            {
                if (!int.TryParse(part, out int num) || num < 0 || num > 255) return false;
            }
            return true;
        }

        private bool IsValidDnsServers(string dnsServers)
        {
            if (string.IsNullOrEmpty(dnsServers)) return true;
            var dnsList = dnsServers.Split(',');
            foreach (var dns in dnsList)
            {
                if (!IsValidIpAddress(dns.Trim())) return false;
            }
            return true;
        }

        private bool IsIpAddressInUse(string ipAddress, int equipmentId)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                MySqlCommand command = new MySqlCommand("SELECT COUNT(*) FROM network WHERE ip_address = @IpAddress AND equipment_id != @EquipmentId", connection);
                command.Parameters.AddWithValue("@IpAddress", ipAddress);
                command.Parameters.AddWithValue("@EquipmentId", equipmentId);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public void Delete(int id)
        {
            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                new DBConnection().Query($"DELETE FROM network WHERE id = {id}", connection);
            }
        }
    }
}