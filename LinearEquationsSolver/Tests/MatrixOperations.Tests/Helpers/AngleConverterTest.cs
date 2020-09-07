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
            Assert.True(Math.Abs(Math.PI/2 - AngleConverter.ConvertDegreesToRadians(90.0f)) <= 0.0000001);
            Assert.True(Math.Abs(-Math.PI*2 - AngleConverter.ConvertDegreesToRadians(-360.0)) <= 0.0000001);
        }
    }
}
