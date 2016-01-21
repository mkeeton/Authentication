using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Authentication.Domain.Models
{
  public class Role : IRole<Guid>
  {
    public Guid Id { get; set; }

    public string Name { get; set; }
  }
}
