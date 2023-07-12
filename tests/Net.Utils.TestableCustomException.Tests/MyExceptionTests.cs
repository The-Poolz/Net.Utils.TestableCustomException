using Xunit;
using FluentAssertions;

namespace Net.Utils.TestableCustomException.Tests;  

public class MyExceptionTests : CustomExceptionTester<MyException>
{
    [Fact]
    internal void MyException_SerializationTest()
    {
        const string expectedMessage = "My custom exception message.";
        RunSerializationTest(expectedMessage);

        var actualMessage = infoForGetObjectData.GetString("Message");

        expectedMessage.Should().Be(actualMessage);
    }
}
