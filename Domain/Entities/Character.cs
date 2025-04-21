using BazaPotworow.Domain.Enums;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaPotworow.Domain.Entities
{
    [Table("character")]
    public class Character : BaseModel
    {

        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("strength")]
        public int Strength { get; set; }

        [Column("dexterity")]
        public int Dexterity { get; set; }

        [Column("constitution")]
        public int Constitution { get; set; }

        [Column("intelligence")]
        public int Intelligence { get; set; }

        [Column("wisdom")]
        public int Wisdom { get; set; }

        [Column("charisma")]
        public int Charisma { get; set; }

        [Column("weapon_id")]
        public int? WeaponId { get; set; } // Nullable, ponieważ `weapon_id` może być NULL

        [Column("vulnerability")]

        public DmgType Vulnerability { get; private set; }
        [Column("resistance")]
        public DmgType Resistance { get; private set; }
    }
}
