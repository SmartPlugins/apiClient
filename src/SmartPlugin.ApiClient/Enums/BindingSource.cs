using System;
using System.Collections.Generic;
using System.Text;

namespace SmartPlugin.ApiClient.Enums
{
   public enum BindingSource
    {       //Route
        Path = 0,
        Query = 1,
        Body = 2,
        Form = 3,
        Header = 4,
        /*  Service=5*/
    }
}
