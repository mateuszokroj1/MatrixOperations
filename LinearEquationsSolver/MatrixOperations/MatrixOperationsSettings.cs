using System;

namespace MatrixOperations
{
    public static class MatrixOperationsSettings
    {
        public static int MinimumCountForParallelMode { get; set; } = 5000;

        public static bool CanRunInParallelMode { get; set; } = Environment.ProcessorCount > 1;

        #region CheckIsParallelModeUseful

        internal static bool CheckIsParallelModeUseful<T>(T itemCount) where T : struct, IComparable
        => itemCount.CompareTo(MinimumCountForParallelMode) >= 0 && CanRunInParallelMode;

        #endregion
    }
}
