using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hy.FantasyEFCore.Domain
{
    [Table("game_account")]
    public class GameAccount
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("UserId")]
        public int UserId { get; set; }

        [Column("Gb")]
        public decimal Gb { get; set; }

        [Column("Rewards")]
        public decimal Rewards { get; set; }

        /// <summary>
        /// 总充值
        /// </summary>
        [Column("SumInvertGb")]
        public decimal SumInvertGb { get; set; }

        /// <summary>
        /// 货币类型
        /// </summary>
        [Column("Currency")]
        public string Currency { get; set; }

        [Column("UpdateTime")]
        public DateTime UpdateTime { get; set; }
    }
}