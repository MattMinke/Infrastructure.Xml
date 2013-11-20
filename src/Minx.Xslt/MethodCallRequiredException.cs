using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minx.Xslt
{
    [Serializable]
    public class MethodCallRequiredException : Exception
    {
        public string Method { get; private set; }

        public MethodCallRequiredException(string method) : base(string.Format("Method '{0}' must be called", method)) { this.Method = method; }
        public MethodCallRequiredException(string method, string message) : base(message) { this.Method = method; }
        public MethodCallRequiredException(string method, string message, Exception inner) : base(message, inner) { this.Method = method; }
        protected MethodCallRequiredException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            this.Method = info.GetString("Method");
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            info.AddValue("Method", this.Method);

            base.GetObjectData(info, context);
        }
    }
}
