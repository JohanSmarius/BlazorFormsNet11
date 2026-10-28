using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Validation;

namespace CustomerApp.Client.Models;

[ValidatableType]
public class RegistrationModel
{
    [Required]
    [StringLength(100)]
    [Display(Name = nameof(RegistrationDisplayNames.FirstName), ResourceType = typeof(RegistrationDisplayNames))]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    [Display(Name = nameof(RegistrationDisplayNames.LastName), ResourceType = typeof(RegistrationDisplayNames))]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(256)]
    [Display(Name = nameof(RegistrationDisplayNames.Email), ResourceType = typeof(RegistrationDisplayNames))]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    [AvailableInviteCode]
    [Display(Name = nameof(RegistrationDisplayNames.InviteCode), ResourceType = typeof(RegistrationDisplayNames))]
    public string InviteCode { get; set; } = string.Empty;
}
