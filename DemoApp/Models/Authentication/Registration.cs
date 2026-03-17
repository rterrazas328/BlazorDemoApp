using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
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
        [MinLengthAttribute(6)]
        public String username { get; set; }

        [Required]
        [MaxLength(128)]
        [MinLengthAttribute(8)]
        public String passwordHash { get; set; }

        [Required]
        [MaxLength(128)]
        [MinLengthAttribute(8)]
        public String confirmPassword { get; set; }

        [Required]
        [MaxLength(64)]
        [MinLengthAttribute(6)]
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

            if (Regex.IsMatch(passwordHash, "[A-Z]"))
            {
                if (Regex.IsMatch(passwordHash, "[a-z]"))
                {
                    if(Regex.IsMatch(passwordHash, "[0-9]"))
                    {
                        if (Regex.IsMatch(passwordHash, "[^a-zA-Z0-9]"))
                        {
                            
                        }
                        else
                        {
                            yield return new ValidationResult(
                            "Password must have at least one special character",
                            new[] { nameof(passwordHash) }
                            );
                        }
                    }
                    else
                    {
                        yield return new ValidationResult(
                        "Password must contain at least one number",
                        new[] { nameof(passwordHash) }
                        );
                    }
                }
                else
                {
                    yield return new ValidationResult(
                    "Password must have at least one lower case character",
                    new[] { nameof(passwordHash) }
                    );
                }
            }
            else
            {
                yield return new ValidationResult(
                    "Password must have at least one upper case character",
                    new[] { nameof(passwordHash) }
                );
            }
            
        }


    }
}
