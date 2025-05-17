using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inventory.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Логин обязателен")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Роль обязательна")]
        public string Role { get; set; } // admin, teacher, employee

        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Phone(ErrorMessage = "Неверный формат телефона")]
        public string Phone { get; set; }

        public string Address { get; set; }

        public virtual ICollection<Equipment> ResponsibleForEquipment { get; set; }
        public virtual ICollection<Equipment> TempResponsibleForEquipment { get; set; }
        public virtual ICollection<Room> ResponsibleForRooms { get; set; }
        public virtual ICollection<Room> TempResponsibleForRooms { get; set; }
        public virtual ICollection<Consumable> ResponsibleForConsumables { get; set; }
        public virtual ICollection<Consumable> TempResponsibleForConsumables { get; set; }
        public virtual ICollection<Inventory> Inventories { get; set; }
        public virtual ICollection<InventoryCheck> InventoryChecks { get; set; }
        public virtual ICollection<EquipmentResponsibleHistory> EquipmentResponsibleHistory { get; set; }
        public virtual ICollection<ConsumableResponsibleHistory> ConsumableResponsibleHistory { get; set; }

        public User()
        {
            ResponsibleForEquipment = new HashSet<Equipment>();
            TempResponsibleForEquipment = new HashSet<Equipment>();
            ResponsibleForRooms = new HashSet<Room>();
            TempResponsibleForRooms = new HashSet<Room>();
            ResponsibleForConsumables = new HashSet<Consumable>();
            TempResponsibleForConsumables = new HashSet<Consumable>();
            Inventories = new HashSet<Inventory>();
            InventoryChecks = new HashSet<InventoryCheck>();
            EquipmentResponsibleHistory = new HashSet<EquipmentResponsibleHistory>();
            ConsumableResponsibleHistory = new HashSet<ConsumableResponsibleHistory>();
        }

        public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
    }
}
}
