using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Uchu.Core.Client
{
	[Table("MinifigDecals_Torsos")]
	public class MinifigDecalsTorsos
	{
		[Key] [Column("efId")]
		public int EfId { get; set; }

		[Column("ID")]
		public int? ID { get; set; }

		[Column("High_path")]
		public string Highpath { get; set; }

		[Column("CharacterCreateValid")]
		public bool? CharacterCreateValid { get; set; }

		[Column("male")]
		public bool? Male { get; set; }

		[Column("female")]
		public bool? Female { get; set; }
	}
}