using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoApp.Models
{
    [Keyless]
    public class ProductReorder
    {

        public int ProductKey { get; set;  }
        public int UnitsBalance { get; set; }
        public int SafetyStockLevel { get; set; }
        public int ReorderPoint { get; set; }
        public int MinUnitstoOrder {  get; set; }
        public String? EnglishProductName { get; set; }
        public String? ProductAlternate { get; set; }

    }
}
