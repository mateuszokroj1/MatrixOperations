using System;

namespace MatrixOperations.Helpers
{
    public static class AngleConverter
    {

        public static T ConvertDegreesToRadians<T>(T angleToConvert) where T : struct
        => Convert.ChangeType(((dynamic)angleToConvert / 180) * Math.PI, typeof(T));

    }
}
