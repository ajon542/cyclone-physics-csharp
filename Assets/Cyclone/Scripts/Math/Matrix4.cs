using System;

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
            return -Data[8] * Data[5] * Data[2] +
                    Data[4] * Data[9] * Data[2] +
                    Data[8] * Data[1] * Data[6] -
                    Data[0] * Data[9] * Data[6] -
                    Data[4] * Data[1] * Data[10] +
                    Data[0] * Data[5] * Data[10];
        }

        public void SetInverse(Matrix4 m)
        {
            // Make sure the determinant is non-zero.
            double det = GetDeterminant();
            if (det == 0)
            {
                return;
            }
            det = (1.0) / det;

            Data[0] = (-m.Data[9] * m.Data[6] + m.Data[5] * m.Data[10]) * det;
            Data[4] = (m.Data[8] * m.Data[6] - m.Data[4] * m.Data[10]) * det;
            Data[8] = (-m.Data[8] * m.Data[5] + m.Data[4] * m.Data[9]) * det;

            Data[1] = (m.Data[9] * m.Data[2] - m.Data[1] * m.Data[10]) * det;
            Data[5] = (-m.Data[8] * m.Data[2] + m.Data[0] * m.Data[10]) * det;
            Data[9] = (m.Data[8] * m.Data[1] - m.Data[0] * m.Data[9]) * det;

            Data[2] = (-m.Data[5] * m.Data[2] + m.Data[1] * m.Data[6]) * det;
            Data[6] = (+m.Data[4] * m.Data[2] - m.Data[0] * m.Data[6]) * det;
            Data[10] = (-m.Data[4] * m.Data[1] + m.Data[0] * m.Data[5]) * det;

            Data[3] = (m.Data[9] * m.Data[6] * m.Data[3]
                       - m.Data[5] * m.Data[10] * m.Data[3]
                       - m.Data[9] * m.Data[2] * m.Data[7]
                       + m.Data[1] * m.Data[10] * m.Data[7]
                       + m.Data[5] * m.Data[2] * m.Data[11]
                       - m.Data[1] * m.Data[6] * m.Data[11]) * det;
            Data[7] = (-m.Data[8] * m.Data[6] * m.Data[3]
                       + m.Data[4] * m.Data[10] * m.Data[3]
                       + m.Data[8] * m.Data[2] * m.Data[7]
                       - m.Data[0] * m.Data[10] * m.Data[7]
                       - m.Data[4] * m.Data[2] * m.Data[11]
                       + m.Data[0] * m.Data[6] * m.Data[11]) * det;
            Data[11] = (m.Data[8] * m.Data[5] * m.Data[3]
                       - m.Data[4] * m.Data[9] * m.Data[3]
                       - m.Data[8] * m.Data[1] * m.Data[7]
                       + m.Data[0] * m.Data[9] * m.Data[7]
                       + m.Data[4] * m.Data[1] * m.Data[11]
                       - m.Data[0] * m.Data[5] * m.Data[11]) * det;
        }

        public Matrix4 Inverse()
        {
            Matrix4 result = new Matrix4();
            result.SetInverse(this);
            return result;
        }

        public void Invert()
        {
            SetInverse(this);
        }

        public Vector3 TransformDirection(Vector3 vector)
        {
            return new Vector3
               (
                vector.x * Data[0] +
                vector.y * Data[1] +
                vector.z * Data[2],

                vector.x * Data[4] +
                vector.y * Data[5] +
                vector.z * Data[6],

                vector.x * Data[8] +
                vector.y * Data[9] +
                vector.z * Data[10]
                );
        }

        public Vector3 TransformInverseDirection(Vector3 vector)
        {
            return new Vector3
                (
                vector.x * Data[0] +
                vector.y * Data[4] +
                vector.z * Data[8],

                vector.x * Data[1] +
                vector.y * Data[5] +
                vector.z * Data[9],

                vector.x * Data[2] +
                vector.y * Data[6] +
                vector.z * Data[10]
                );
        }

        public Vector3 TransformInverse(Vector3 vector)
        {
            Vector3 tmp = vector;
            tmp.x -= Data[3];
            tmp.y -= Data[7];
            tmp.z -= Data[11];
            return new Vector3
                (
                tmp.x * Data[0] +
                tmp.y * Data[4] +
                tmp.z * Data[8],

                tmp.x * Data[1] +
                tmp.y * Data[5] +
                tmp.z * Data[9],

                tmp.x * Data[2] +
                tmp.y * Data[6] +
                tmp.z * Data[10]
                );
        }

        public Vector3 GetAxisVector(int i)
        {
            return new Vector3(Data[i], Data[i + 4], Data[i + 8]);
        }

        public void SetOrientationAndPos(Quaternion q, Vector3 pos)
        {
            //Data[0] = 1 - (2 * q.j * q.j + 2 * q.k * q.k);
            //Data[1] = 2 * q.i * q.j + 2 * q.k * q.r;
            //Data[2] = 2 * q.i * q.k - 2 * q.j * q.r;
            //Data[3] = pos.x;

            //Data[4] = 2 * q.i * q.j - 2 * q.k * q.r;
            //Data[5] = 1 - (2 * q.i * q.i + 2 * q.k * q.k);
            //Data[6] = 2 * q.j * q.k + 2 * q.i * q.r;
            //Data[7] = pos.y;

            //Data[8] = 2 * q.i * q.k + 2 * q.j * q.r;
            //Data[9] = 2 * q.j * q.k - 2 * q.i * q.r;
            //Data[10] = 1 - (2 * q.i * q.i + 2 * q.j * q.j);
            //Data[11] = pos.z;
            throw new NotImplementedException();
        }
    }
}
