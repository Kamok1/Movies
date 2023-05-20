using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;
[Index(nameof(UserId))]
public record RefreshToken
{
  [Key]
  public int Id { get; set; }
  [MaxLength(15)]
  public string Ip { get; set; }
  public int UserId { get; set; } 
  public User User { get; set; } 
  [MaxLength(200)]
  public string Token { get; set; } = string.Empty;
  public DateTime Expires { get; set; } = DateTime.MinValue;

}