using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SBoxModList
{
    public class WebTimeout : WebClient
    {
        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            w.Timeout = 5000;
            return w;
        }
    }
}
