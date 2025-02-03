namespace CoinFlow.Models.UserEntity.Dto;

using System.ComponentModel.DataAnnotations;

public class UserRequest
{
    [Required(ErrorMessage = "Username is required.")]
    public string UserName { get; init; } = string.Empty;

    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; init; } = string.Empty;

    [Display(Name = "Email")]
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;
}
