using BazaBroni.Domain.Enums;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaBroni.Domain.Entities
{
    [Table("character")]
    public class Character : BaseModel
    {
        public Character(int id, string name, int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma, DmgType vulnerability, DmgType resistance)
        {
            Id = id;
            Name = name;
            this.Strength = strength;
            this.Dexterity = dexterity;
            this.Constitution = constitution;
            this.Intelligence = intelligence;
            this.Wisdom = wisdom;
            this.Charisma = charisma;
            Vulnerability = vulnerability;
            Resistance = resistance;
        }

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
