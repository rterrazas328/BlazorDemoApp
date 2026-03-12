using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DemoApp.Services;


namespace DemoApp.Models.Authentication
{
    public class Registration : IValidatableObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(64)]
        public String username { get; set; }

        [Required]
        [MaxLength(128)]
        public String passwordHash { get; set; }

        [Required]
        [MaxLength(128)]
        public String confirmPassword { get; set; }

        [Required]
        [MaxLength(64)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public String email { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (passwordHash != confirmPassword)
            {
                yield return new ValidationResult(
                    "Passwords do not match.",
                    new[] { nameof(confirmPassword) }
                );
            }
            
        }


    }
}
