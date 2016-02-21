using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Authentication.Domain.Models
{
  public class RefreshToken
  {
    [Key]
    [Required]
    public string Id { get; set;}

    [Required]
    public Guid UserId { get; set; }

    public DateTime IssuedUtc { get; set; }

    public DateTime ExpiresUtc { get; set; }

    [Required]
    public string ProtectedTicket { get; set; }
  }
}
