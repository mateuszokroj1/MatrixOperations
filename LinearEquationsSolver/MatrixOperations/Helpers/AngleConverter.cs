using System;

namespace MatrixOperations.Helpers
{
    public static class AngleConverter
    {

        public static double ConvertDegreesToRadians(double angleToConvert)
        => angleToConvert / 180 * Math.PI;

    }
}
