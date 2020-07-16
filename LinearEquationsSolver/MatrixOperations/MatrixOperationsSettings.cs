using System;

namespace MatrixOperations
{
    public static class MatrixOperationsSettings
    {
        public static int MinimumCountForParallelMode { get; set; } = 5000;

        public static bool CanRunInParallelMode { get; set; } = Environment.ProcessorCount > 1;

        #region CheckIsParallelModeUseful

        internal static bool CheckIsParallelModeUseful(ushort itemCount)
        => itemCount >= MinimumCountForParallelMode && CanRunInParallelMode;

        internal static bool CheckIsParallelModeUseful(short itemCount)
        => itemCount >= MinimumCountForParallelMode && CanRunInParallelMode;

        internal static bool CheckIsParallelModeUseful(int itemCount)
        => itemCount >= MinimumCountForParallelMode && CanRunInParallelMode;

        internal static bool CheckIsParallelModeUseful(uint itemCount)
        => itemCount >= MinimumCountForParallelMode && CanRunInParallelMode;

        internal static bool CheckIsParallelModeUseful(long itemCount)
        => itemCount >= MinimumCountForParallelMode && CanRunInParallelMode;

        internal static bool CheckIsParallelModeUseful(ulong itemCount)
        => itemCount >= (ulong)MinimumCountForParallelMode && CanRunInParallelMode;

        #endregion
    }
}
