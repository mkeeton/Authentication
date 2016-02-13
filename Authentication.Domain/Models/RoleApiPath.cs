using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Domain.Models
{
  public class RoleApiPath
  {
    public Guid Id { get; set; }

    public Guid RoleId { get; set; }

    public string ActionPath { get; set; }

    public string ActionMethod { get; set; }
  }
}
