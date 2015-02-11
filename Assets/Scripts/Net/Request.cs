using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace net
{
    public class Request
    {
        public string Url { get; set; }

        public WWW WWW { get; private set; }

        public class RequestResult
        {
            public bool Succesfull;
            public string Message;
        }

        public RequestResult Result  { get; set; }

        public IEnumerator Start()
        {
            var www = new WWW (Url);
            
            yield return www;

            Result = new RequestResult() { Succesfull = string.IsNullOrEmpty(www.error), Message = www.error };

            WWW = www;
        }
    }

}
