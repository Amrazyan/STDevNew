using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace App1.Scripts
{
    class MyContainer
    {
        public HttpWebRequest conRequest;
        public Action onSuccessAction;
        public Action onFailedAction;
    }
}
