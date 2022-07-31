﻿using InventoryModels.Constants;
using InventoryModels.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryModels.Models
{
    public abstract class FullAuditModel : IActivatableModel, IAuditedModel, IIdentityModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(InventoryModelsConstants.MAX_USERID_LENGTH)]
        public string CreatedByUserId { get; set; } = "SYSTEM";
        public DateTime CreatedDate { get; set; }

        [StringLength(InventoryModelsConstants.MAX_USERID_LENGTH)]
        public string? LastModifiedUserId { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool IsActive { get; set; } = false;
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }
    }
}
