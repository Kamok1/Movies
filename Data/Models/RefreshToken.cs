using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public record RefreshToken
{
  [Key] public int UserId { get; set; } 
  public User User { get; set; } 
  [MaxLength(200)]
  public string Token { get; set; } = string.Empty;
  public DateTime Expires { get; set; } = DateTime.MinValue;

}