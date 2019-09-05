using App1.Scripts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace App1.Scripts
{
    public class MakeRequest
    {
        public static void UrlIsValid(string url, Action onSuccess, Action onFailed)
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;
                request.ContinueTimeout = 5000;
                request.Method = "HEAD";
                request.BeginGetResponse(responseCompleted, new MyContainer { conRequest = request, onSuccessAction = onSuccess,onFailedAction = onFailed });

            }
            catch (Exception)
            {
                onFailed?.Invoke();
            }

        }

        private static void responseCompleted(IAsyncResult asyncResult)
        {
            MyContainer container = (MyContainer)asyncResult.AsyncState;

            HttpWebRequest request = container.conRequest;            

                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(asyncResult))
                    {
                        int statusCode = (int)response.StatusCode;

                        if (statusCode >= 100 && statusCode < 400) //Good requests
                        {
                            container.onSuccessAction?.Invoke();
                        }
                        else
                        {
                            container.onFailedAction?.Invoke();
                        }
                    }
                }
                catch (Exception)
                {
                    container.onFailedAction?.Invoke(); 
                }

           
        }

        public static bool IsUrlRegexValid(string url)
        {

            string pattern = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
            Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            return reg.IsMatch(url);
        }
    }
}
