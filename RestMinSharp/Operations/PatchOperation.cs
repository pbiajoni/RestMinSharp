using System;

namespace RestMinSharp.Operations
{
    public class PatchOperation
    {
        public PatchOperation()
        {
        }

        public PatchOperation(string op, string path, object value)
        {
            this.op = op;
            this.path = path;
            this.value = value;
        }

        public string op { get; set; }
        public string path { get; set; }
        public object value { get; set; }
    }
}
