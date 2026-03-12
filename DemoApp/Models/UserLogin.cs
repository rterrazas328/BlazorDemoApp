using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoApp.Models
{

    //Model for holding username, password, and any future authentication inputs such as remember me tokens, honeypots, etc.
    //This will be the viewModel that maps onto UserAccount
    public class UserLogin
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(64)")]
        public String username {get; set;}

        [Required]
        [Column(TypeName = "nvarchar(128)")]
        public String password { get; set; }

        [Column(TypeName = "nchar(32)")]
        public String? role { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? createdAt { get; set; }

    }

}
