using System.ComponentModel.DataAnnotations;

namespace RestaurantOrderingSystem.Models
{
    public class OrderDb
    {


        [Key]
        public string? OrderId { get; set; }


        public string? OrderNo { get; set; }

        public string? Customer { get; set; }


        public string? Items { get; set; }


        public int Price { get; set; }

        public string? Payment { get; set; }

    }
}
