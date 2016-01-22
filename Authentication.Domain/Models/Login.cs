using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Domain.Models
{
  public class Login
  {
    public virtual string LoginProvider { get; set; }

    public virtual string ProviderKey { get; set; }

    public virtual Guid UserId { get; set; }
  }
}
