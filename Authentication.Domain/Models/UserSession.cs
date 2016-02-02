using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Domain.Models
{
  public class UserSession
  {
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public DateTime StartDate { get; set; }
  }
}
