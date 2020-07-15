using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixOperations
{
    public static class MatrixOperationsSettings
    {
        public static int MinimumCountForParallelMode { get; set; } = 5000;

        public static bool CanRunInParallelMode { get; set; } = Environment.ProcessorCount > 1;

        internal static bool CheckIsParallelModeUseful(int itemCount)
        => itemCount >= MinimumCountForParallelMode && CanRunInParallelMode;
    }
}
