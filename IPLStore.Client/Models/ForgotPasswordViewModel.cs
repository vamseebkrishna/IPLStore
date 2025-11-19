using System.ComponentModel.DataAnnotations;

namespace IPLStore.Client.Models;

public class ForgotPasswordViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
