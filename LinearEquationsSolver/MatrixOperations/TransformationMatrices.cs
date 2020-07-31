using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixOperations
{
    public static class TransformationMatrices
    {
        #region Translation

        public static Matrix<Tsource> GenerateTranslate2D<Tsource>(Tsource moveX, Tsource moveY)
            where Tsource : struct, IEquatable<Tsource>
        {
            Matrix<Tsource> matrix = Matrix<Tsource>.GenerateIdentity(3);
            matrix[0, 2] = moveX;
            matrix[1, 2] = moveY;
            return matrix;
        }

        public static Matrix<Tsource> GenerateTranslate3D<Tsource>(Tsource moveX, Tsource moveY, Tsource moveZ)
            where Tsource : struct, IEquatable<Tsource>
        {
            Matrix<Tsource> matrix = Matrix<Tsource>.GenerateIdentity(4);
            matrix[0, 3] = moveX;
            matrix[1, 3] = moveY;
            matrix[2, 3] = moveZ;
            return matrix;
        }

        #endregion

        #region Scaling

        public static Matrix<Tsource> GenerateScale2D<Tsource>(Tsource scaleFactor)
            where Tsource : struct, IEquatable<Tsource>
        => GenerateScale2D(scaleFactor, scaleFactor);

        public static Matrix<Tsource> GenerateScale2D<Tsource>(Tsource scaleFactorX, Tsource scaleFactorY)
            where Tsource : struct, IEquatable<Tsource>
        {
            Matrix<Tsource> matrix = new Matrix<Tsource>(3,3);
            matrix[0, 0] = scaleFactorX;
            matrix[1, 1] = scaleFactorY;
            matrix[2, 2] = (dynamic)1;
            return matrix;
        }

        public static Matrix<Tsource> GenerateScale3D<Tsource>(Tsource scaleFactor)
            where Tsource : struct, IEquatable<Tsource>
        => GenerateScale3D(scaleFactor, scaleFactor, scaleFactor);

        public static Matrix<Tsource> GenerateScale3D<Tsource>(Tsource scaleFactorX, Tsource scaleFactorY, Tsource scaleFactorZ)
            where Tsource : struct, IEquatable<Tsource>
        {
            Matrix<Tsource> matrix = new Matrix<Tsource>(4, 4);
            matrix[0, 0] = scaleFactorX;
            matrix[1, 1] = scaleFactorY;
            matrix[2, 2] = scaleFactorZ;
            matrix[3,3] = (dynamic)1;
            return matrix;
        }

        #endregion

        #region Rotating

        public static Matrix<double> GenerateRotate2D(double angle, AngleMode angleMode = AngleMode.Radians, double centerX = 0, double centerY = 0)
        => GenerateRotate2D(angle, angle);

        public static Matrix<double> GenerateRotate2D(double angleX, double angleY, AngleMode angleMode = AngleMode.Radians, double centerX = 0, double centerY = 0)
        {
            Matrix<double> matrix = null;

            if(centerX == 0 && centerY == 0)
                matrix = Matrix<>
            Matrix<double> matrix = GenerateTranslate2D<double>();
        }

        public static Matrix<> GenerateRotate3D(double angleX, double angleY, double angleZ)
        {

        }

        #endregion
    }
}
