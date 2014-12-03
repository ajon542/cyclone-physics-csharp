using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyclone.Math
{
    public class Matrix4
    {
        public double[] Data { get; set; }

        public Matrix4()
        {
            Data = new double[12];
            Data[0] = Data[5] = Data[10] = 1;
        }

        public void SetDiagonal(double a, double b, double c)
        {
            Data[0] = a;
            Data[5] = b;
            Data[10] = c;
        }

        public static Matrix4 operator *(Matrix4 lhs, Matrix4 rhs)
        {
            Matrix4 result = new Matrix4();
            result.Data[0] = (rhs.Data[0] * lhs.Data[0]) + (rhs.Data[4] * lhs.Data[1]) + (rhs.Data[8] * lhs.Data[2]);
            result.Data[4] = (rhs.Data[0] * lhs.Data[4]) + (rhs.Data[4] * lhs.Data[5]) + (rhs.Data[8] * lhs.Data[6]);
            result.Data[8] = (rhs.Data[0] * lhs.Data[8]) + (rhs.Data[4] * lhs.Data[9]) + (rhs.Data[8] * lhs.Data[10]);

            result.Data[1] = (rhs.Data[1] * lhs.Data[0]) + (rhs.Data[5] * lhs.Data[1]) + (rhs.Data[9] * lhs.Data[2]);
            result.Data[5] = (rhs.Data[1] * lhs.Data[4]) + (rhs.Data[5] * lhs.Data[5]) + (rhs.Data[9] * lhs.Data[6]);
            result.Data[9] = (rhs.Data[1] * lhs.Data[8]) + (rhs.Data[5] * lhs.Data[9]) + (rhs.Data[9] * lhs.Data[10]);

            result.Data[2] = (rhs.Data[2] * lhs.Data[0]) + (rhs.Data[6] * lhs.Data[1]) + (rhs.Data[10] * lhs.Data[2]);
            result.Data[6] = (rhs.Data[2] * lhs.Data[4]) + (rhs.Data[6] * lhs.Data[5]) + (rhs.Data[10] * lhs.Data[6]);
            result.Data[10] = (rhs.Data[2] * lhs.Data[8]) + (rhs.Data[6] * lhs.Data[9]) + (rhs.Data[10] * lhs.Data[10]);

            result.Data[3] = (rhs.Data[3] * lhs.Data[0]) + (rhs.Data[7] * lhs.Data[1]) + (rhs.Data[11] * lhs.Data[2]) + lhs.Data[3];
            result.Data[7] = (rhs.Data[3] * lhs.Data[4]) + (rhs.Data[7] * lhs.Data[5]) + (rhs.Data[11] * lhs.Data[6]) + lhs.Data[7];
            result.Data[11] = (rhs.Data[3] * lhs.Data[8]) + (rhs.Data[7] * lhs.Data[9]) + (rhs.Data[11] * lhs.Data[10]) + lhs.Data[11];

            return result;
        }

        public static Vector3 operator *(Matrix4 lhs, Vector3 vector)
        {
            return new Vector3
                (
                vector.x * lhs.Data[0] +
                vector.y * lhs.Data[1] +
                vector.z * lhs.Data[2] + lhs.Data[3],

                vector.x * lhs.Data[4] +
                vector.y * lhs.Data[5] +
                vector.z * lhs.Data[6] + lhs.Data[7],

                vector.x * lhs.Data[8] +
                vector.y * lhs.Data[9] +
                vector.z * lhs.Data[10] + lhs.Data[11]
                );
        }

        public Vector3 Transform(Vector3 vector)
        {
            return this * vector;
        }

        public double GetDeterminant()
        {
            return 0;
        }

        public void SetInverse(Matrix4 matrix)
        {

        }


    }
}
