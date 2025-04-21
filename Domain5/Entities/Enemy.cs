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
    [Table("enemy")]
    public class Enemy : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; } // UNIQUEIDENTIFIER w SQL Server mapuje się na Guid w C#

        [Column("name")]
        public string Name { get; set; }

        [Column("ac")]
        public int AC { get; set; } // Armor Class (AC)

        [Column("vulnerability")]
        public DmgType Vulnerability { get; set; }

        [Column("resistance")]
        public DmgType Resistance { get; set; }
    }
}
