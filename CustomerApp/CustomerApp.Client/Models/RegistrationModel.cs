using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Validation;

namespace CustomerApp.Client.Models;

[ValidatableType]
public class RegistrationModel
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100, ErrorMessage = "First name cannot be longer than 100 characters.")]
    [Display(Name = "First name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100, ErrorMessage = "Last name cannot be longer than 100 characters.")]
    [Display(Name = "Last name")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    [StringLength(256, ErrorMessage = "Email address cannot be longer than 256 characters.")]
    [Display(Name = "Email address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Invite code is required.")]
    [StringLength(50, ErrorMessage = "Invite code cannot be longer than 50 characters.")]
    [Display(Name = "Invite code")]
    public string InviteCode { get; set; } = string.Empty;
}
