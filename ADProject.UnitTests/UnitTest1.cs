using FluentAssertions;
using Xunit;

namespace ADProject.UnitTests;
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var result = 2 + 2;
            result.Should().Be(4);
        }
    }

