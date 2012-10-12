using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer
{
    public class AbsoluteUriAttribute : ValidationAttribute
    {
        public AbsoluteUriAttribute()
        {
            ErrorMessage = "{0} must be an absolute Uri.";
        }
        
        public override bool IsValid(object value)
        {
            Uri uri = value as Uri;
            if (uri == null) return true;
            return uri.IsAbsoluteUri;
        }
    }
}
