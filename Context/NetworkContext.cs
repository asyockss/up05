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

public void Save(bool Update = false)
        {
            // Валидация сетевых настроек
            if (!IsValidIpAddress(this.IpAddress) ||
            (this.SubnetMask != null && !IsValidIpAddress(this.SubnetMask)) ||
            (this.Gateway != null && !IsValidIpAddress(this.Gateway)) ||
            (this.DnsServers != null && !IsValidDnsServers(this.DnsServers)))
            {
                throw new ArgumentException("Некорректный формат сетевых настроек");
            }

            // Проверка уникальности IP-адреса
            if (!Update && IsIpAddressInUse(this.IpAddress, this.EquipmentId))
            {
                throw new ArgumentException("IP-адрес уже используется другим оборудованием");
            }

            using (MySqlConnection connection = (MySqlConnection)new DBConnection().OpenConnection("MySql"))
            {
                string query = Update
                ? "UPDATE network SET equipment_id = @EquipmentId, ip_address = @IpAddress, subnet_mask = @SubnetMask, gateway = @Gateway, dns_servers = @DnsServers WHERE id = @Id"
                : "INSERT INTO network (equipment_id, ip_address, subnet_mask, gateway, dns_servers) VALUES (@EquipmentId, @IpAddress, @SubnetMask, @Gateway, @DnsServers)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", this.Id);
                command.Parameters.AddWithValue("@EquipmentId", this.EquipmentId);
                command.Parameters.AddWithValue("@IpAddress", this.IpAddress);
                command.Parameters.AddWithValue("@SubnetMask", this.SubnetMask ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Gateway", this.Gateway ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@DnsServers", this.DnsServers ?? (object)DBNull.Value);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Logging.LogError(ex, "Ошибка при сохранении сетевых настроек");
                    throw;
                }
            }
        }

        private bool IsValidIpAddress(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return false;
            string pattern = @"^(\d{1,3}).(\d{1,3}).(\d{1,3}).(\d{1,3})$";
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