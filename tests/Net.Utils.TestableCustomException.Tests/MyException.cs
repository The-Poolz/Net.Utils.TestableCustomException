using System.Runtime.Serialization;

namespace Net.Utils.TestableCustomException.Tests;

[Serializable]
public class MyException : Exception
{
    public MyException()
        : base("My exception message.")
    { }

    protected MyException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    { }
}
