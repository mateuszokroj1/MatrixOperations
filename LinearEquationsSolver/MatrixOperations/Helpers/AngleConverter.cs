using System;

namespace MatrixOperations.Helpers
{
    public static class AngleConverter
    {
        public static double ConvertDegreesToRadians(double angleToConvert)
        => (angleToConvert / 180) * Math.PI;

        public static float ConvertDegreesToRadians(float angleToConvert)
        => Convert.ToSingle((angleToConvert / 180) * Math.PI);
    }
}
