using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Authentication.Domain.Models
{
  public class ClientAllowedOrigin
  {
    [Key]
    public Guid Id { get; set;}

    [Required]
    [MaxLength(100)]
    public string AllowedURL {get; set;}
  }
}
