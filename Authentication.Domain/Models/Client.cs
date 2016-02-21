using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Authentication.Domain.Models
{
  public class Client
  {
    [Key]
    public Guid Id { get; set;}

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    public string URL {  get; set;}

    public string ClientId { get; set; }

    [Required]
    public string Secret { get; set; }

    public bool Active { get; set; }

    public int RefreshTokenLifeTime { get; set; }

    public List<ClientAllowedOrigin> AllowedOrigins { get; set; }
  }
}
