﻿using InventoryModels.Constants;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace InventoryModels.Models
{
    public class Item : FullAuditModel
    {
        [StringLength(InventoryModelsConstants.MAX_NAME_LENGTH)]
        [Required]
        public string Name { get; set; }

        [Range(InventoryModelsConstants.MINIMUM_QUANTITY, InventoryModelsConstants.MAXIMUM_QUANTITY)]

        public int Quantity { get; set; }

        [StringLength(InventoryModelsConstants.MAX_DESCRIPTION_LENGTH)]
        public string Description { get; set; } = string.Empty;

        [StringLength(InventoryModelsConstants.MAX_NOTES_LENGTH, MinimumLength = 10)]
        public string Notes { get; set; } = string.Empty;

        public bool IsOnSale { get; set; } = false;
        public DateTime? PurchasedDate { get; set; }
        public DateTime? SoldDate { get; set; }

        [Range(InventoryModelsConstants.MINIMUM_PRICE, InventoryModelsConstants.MAXIMUM_PRICE)]
        public decimal? PurchasePrice { get; set; }

        [Range(InventoryModelsConstants.MINIMUM_PRICE, InventoryModelsConstants.MAXIMUM_PRICE)]
        public decimal? CurrentOrFinalPrice { get; set; }

        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        public virtual List<Player> Players { get; set; } = new List<Player>();

        public virtual List<ItemGenre> ItemGenres { get; set; } = new List<ItemGenre>();
    }
}