using InventoryModels.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryModels.Models
{
	public class Genre : FullAuditModel
	{
		[Required]
		[StringLength(InventoryModelsConstants.MAX_NAME_LENGTH)]
		public string Name { get; set; }

		public virtual List<ItemGenre> GenreItems { get; set; } = new List<ItemGenre>();
	}

}
