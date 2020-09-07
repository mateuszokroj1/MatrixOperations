using System;
using MatrixOperations.Helpers;
using Xunit;

namespace MatrixOperations.Tests.Helpers
{
    public class AngleConverterTest
    {
        [Fact]
        public void ConvertDegreesToRadians_ShouldReturnValidValue()
        {
            Assert.Equal(0, AngleConverter.ConvertDegreesToRadians(0));
            Assert.Equal(Math.PI/2, AngleConverter.ConvertDegreesToRadians(90.0f));
            Assert.Equal(Math.PI*2, AngleConverter.ConvertDegreesToRadians(-360.0));
        }
    }
}
