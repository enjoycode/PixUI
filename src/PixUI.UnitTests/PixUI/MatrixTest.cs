using System;
using NUnit.Framework;

namespace PixUI.UnitTests
{
    public class MatrixTest
    {
        [Test]
        public void Test1()
        {
            var parent = Matrix4.CreateTranslation(20f, 30f, 0);
            Console.WriteLine(parent);

            var transform = Matrix4.CreateIdentity();
            transform.Translate(10, 10);
            transform.RotateZ(20 * Matrix3.DegreesToRadians);

            parent.PreConcat(transform);
            Console.WriteLine(parent);

            var mapPt = MatrixUtils.TransformPoint(parent, 0, 0);
            Console.WriteLine(mapPt);
        }

        [Test]
        public void Test2()
        {
            var transform = Matrix4.CreateIdentity();
            transform.Translate(10, 20);

            var mapPt = MatrixUtils.TransformPoint(transform, 0, 0);
            Console.WriteLine(mapPt);
        }

        [Test]
        public void Test3()
        {
            var transform = Matrix4.CreateIdentity();
            transform.RotateZ((float)(0.75 * Math.PI * 2.0f));
            Console.WriteLine(transform);
        }

        [Test]
        public void TestMatrix3()
        {
            var matrix = Matrix3.CreateTranslation(10, 10);
            //var matrix = Matrix3.CreateRotationDegrees(45);
            var pt = matrix.MapPoint(1, 1);
            Assert.True(pt.X == 11 && pt.Y == 11);
        }
    }
}