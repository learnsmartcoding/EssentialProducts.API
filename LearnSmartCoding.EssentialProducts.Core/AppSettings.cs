using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearnSmartCoding.EssentialProducts.Core
{
    [Table("AppSettings", Schema = "dbo")]
    public partial class AppSettings
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column("AppSettingKey")]
        [StringLength(50)]
        public string AppSettingKey { get; set; }
        [Required]
        [Column("AppSettingValue")]
        [StringLength(200)]
        public string AppSettingValue { get; set; }

        [Column("ValidFromDt", TypeName = "datetime")]
        public DateTime ValidFromDt { get; set; }
        [Column("ValidToDt", TypeName = "datetime")]
        public DateTime? ValidToDt { get; set; }       
    }
}
