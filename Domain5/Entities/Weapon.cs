using KalkulatorObrazen.Domain.Enums;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KalkulatorObrazen.Domain.Entities
{
    [Table("weapon")]
    public class Weapon : BaseModel
    {


        [Column("id")]
        public int Id { get; private set; }
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("damage_type")]
        public DmgType DamageType { get; set; }

        [Column("damage_dice")]
        public DiceType DamageDice { get; set; }

        [Column("damage_dice_count")]
        public int DamageDiceCount { get; set; }
    }
}
