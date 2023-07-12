using System.Reflection;
using System.Runtime.Serialization;

namespace Net.Utils.TestableCustomException;

/// <summary>
/// Helps with testing serialization of exceptions.
/// </summary>
public abstract class CustomExceptionTester<TException>
    where TException : Exception
{
    protected SerializationInfo infoForGetObjectData = null!;

    /// <summary>
    /// Tests serialization of the exception.
    /// </summary>
    protected void RunSerializationTest(string expectedMessage)
    {
        var info = new SerializationInfo(typeof(TException), new FormatterConverter());
        info.AddValue("Message", expectedMessage);
        info.AddValue("InnerException", null, typeof(Exception));
        info.AddValue("HelpURL", null, typeof(string));
        info.AddValue("StackTraceString", null, typeof(string));
        info.AddValue("RemoteStackTraceString", null, typeof(string));
        info.AddValue("HResult", int.MinValue, typeof(int));
        info.AddValue("Source", null, typeof(string));
        info.AddValue("ClassName", "Exception");
        info.AddValue("RemoteStackIndex", 0, typeof(int));
        info.AddValue("ExceptionMethod", null, typeof(string));

        var context = new StreamingContext();

        var exception = CreateTestableException(info, context);

        infoForGetObjectData = new SerializationInfo(typeof(TException), new FormatterConverter());
        exception.GetObjectData(infoForGetObjectData, context);
    }

    private static TException CreateTestableException(SerializationInfo info, StreamingContext context)
    {
        var ctor = typeof(TException).GetConstructor(
            bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance,
            binder: null, 
            types: new[] { typeof(SerializationInfo), typeof(StreamingContext) },
            modifiers: null
        ) ?? throw new InvalidOperationException($"Exception of type {typeof(TException)} does not have a serialization constructor.");

        try
        {
            return (TException)ctor.Invoke(new object[] { info, context });
        }
        catch (TargetInvocationException tie)
        {
            throw new InvalidOperationException($"Failed to invoke the serialization constructor of exception type {typeof(TException)}", tie);
        }
    }
}
