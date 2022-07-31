using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryModels.Constants;
namespace InventoryModels.Models
{
    public class Player : FullAuditModel
    {
        [Required]
        [StringLength(InventoryModelsConstants.MAX_PLAYERNAME_LENGTH)]
        public string Name { get; set; }

        [StringLength(InventoryModelsConstants.MAX_PLAYERDESCRIPTION_LENGTH)]
        public string Description { get; set; }

        public virtual List<Item> Items { get; set; } = new List<Item>();
    }
}
