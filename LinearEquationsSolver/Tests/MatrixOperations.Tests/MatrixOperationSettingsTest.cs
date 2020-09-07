using System;

using Xunit;

namespace MatrixOperations.Tests
{
    public class MatrixOperationSettingsTest
    {
        [Fact]
        public void MinimumCountForParallelMode_ShouldSetAndGet()
        {
            MatrixOperationsSettings.MinimumCountForParallelMode = 3;
            Assert.Equal(3, MatrixOperationsSettings.MinimumCountForParallelMode);
        }

        [Fact]
        public void CanRunInParallelMode_ShouldHaveDefaultValueValid()
        {
            Assert.Equal(Environment.ProcessorCount > 2, MatrixOperationsSettings.CanRunInParallelMode);
        }

        [Fact]
        public void CanRunInParallelMode_ShouldSetAndGetValue()
        {
            MatrixOperationsSettings.CanRunInParallelMode = false;
            Assert.False(MatrixOperationsSettings.CanRunInParallelMode);

            MatrixOperationsSettings.CanRunInParallelMode = true;
            Assert.True(MatrixOperationsSettings.CanRunInParallelMode);
        }
    }
}
