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

        [Fact]
        public void CheckIsParallelModeUseful_ShouldReturnValidValue()
        {
            MatrixOperationsSettings.CanRunInParallelMode = false;
            MatrixOperationsSettings.MinimumCountForParallelMode = 10;
            Assert.False(MatrixOperationsSettings.CheckIsParallelModeUseful(20));

            MatrixOperationsSettings.CanRunInParallelMode = true;
            MatrixOperationsSettings.MinimumCountForParallelMode = 200;
            Assert.True(MatrixOperationsSettings.CheckIsParallelModeUseful(200L));

            MatrixOperationsSettings.CanRunInParallelMode = true;
            MatrixOperationsSettings.MinimumCountForParallelMode = 2000;
            Assert.False(MatrixOperationsSettings.CheckIsParallelModeUseful(100.0f));

            MatrixOperationsSettings.CanRunInParallelMode = true;
            MatrixOperationsSettings.MinimumCountForParallelMode = 2000;
            Assert.True(MatrixOperationsSettings.CheckIsParallelModeUseful(5000M));
        }
    }
}
