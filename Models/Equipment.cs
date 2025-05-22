using System.ComponentModel;

namespace inventory.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public string InventoryNumber { get; set; }
        public int? RoomId { get; set; }
        public Room Room { get; set; }
        public int? ResponsibleId { get; set; }
        public User Responsible { get; set; }
        public int? TempResponsibleId { get; set; }
        public User TempResponsible { get; set; }
        public decimal? Cost { get; set; }
        public string Comment { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public int? ModelId { get; set; }
        public EquipmentModel Model { get; set; }
        public int EquipmentTypeId { get; set; }
        public EquipmentType EquipmentType { get; set; }
        public int? DirectionId { get; set; }
        public Direction Direction { get; set; }
        public int? InventoryId { get; set; }
        public Inventory Inventory { get; set; }
    }
}