using MatrixOperations.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixOperations
{
    public static class TransformationMatrices
    {
        #region Translation

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

        public static Matrix<double> GenerateRotate2D(double angle, AngleMode angleMode = AngleMode.Radians, double centerX = default, double centerY = default)
        {
            Matrix<double> translation = GenerateTranslate2D(centerX,centerY);

            angle = angleMode == AngleMode.Radians ? angle : AngleConverter.ConvertDegreesToRadians(angle);

            Matrix<double> rotateTransformation = new Matrix<double>(2, 2);
            rotateTransformation[0, 0] = Math.Cos(angle);
            rotateTransformation[0, 1] = -Math.Sin(angle);
            rotateTransformation[1, 0] = Math.Sin(angle);
            rotateTransformation[1, 1] = rotateTransformation[0, 0];

            if (centerX == 0 && centerY == 0)
                return rotateTransformation;
            else
                return Matrix<double>.Multiply(translation, rotateTransformation, GenerateTranslate2D(-centerX, -centerY));
        }

        public static Matrix<double> GenerateRotate3D(double angleX, double angleY, double angleZ = 0, AngleMode angleMode = AngleMode.Radians, double centerX = 0, double centerY = 0, double centerZ = 0)
        {
            Matrix<double> translation = GenerateTranslate3D(centerX, centerY, centerZ);

            angleX = angleMode == AngleMode.Radians ? angleX : AngleConverter.ConvertDegreesToRadians(angleX);
            angleY = angleMode == AngleMode.Radians ? angleY : AngleConverter.ConvertDegreesToRadians(angleY);
            angleZ = angleMode == AngleMode.Radians ? angleZ : AngleConverter.ConvertDegreesToRadians(angleZ);

            Matrix<double> rotationX = new Matrix<double>(3, 3);
            rotationX[0, 0] = 1;
            rotationX[0, 1] = 0;
            rotationX[0, 2] = 0;

            rotationX[1, 0] = 0;
            rotationX[1, 1] = Math.Cos(angleX);
            rotationX[1, 2] = -Math.Sin(angleX);

            rotationX[];

            Matrix<double> rotationY = new Matrix<double>(3,3);


            Matrix<double> rotationZ = new Matrix<double>(3,3);
        }

        #endregion
    }
}
