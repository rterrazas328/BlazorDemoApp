using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoApp.Models
{
    public class UserAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(64)")]
        public String username { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(128)")]
        public String passwordHash { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(64)")]
        public String email { get; set; }

        [Required]
        [Column(TypeName = "nchar(32)")]
        public String role { get; set; }

        [Column(TypeName = "nvarchar(64)")]
        public String? firstname { get; set; }

        [Column(TypeName = "nvarchar(64)")]
        public String? lastname { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime createdAt { get; set; }
    }
}
