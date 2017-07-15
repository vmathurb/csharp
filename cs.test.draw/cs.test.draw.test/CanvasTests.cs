using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace cs.test.draw.tests
{
    [TestClass]
    public class CanvasTests
    {
        #region Canvas Creation

        [TestMethod]
        public void CreateCanvasTest()
        {
            ICanvas canvas = new Canvas(10, 20);
            Assert.IsNotNull(canvas);
        }

        [TestMethod]
        public void CanvasIsCreatedWithCorrectHeight()
        {
            ICanvas canvas = new Canvas(10, 20);
            Assert.IsTrue(canvas.Width == 10);
        }

        [TestMethod]
        public void CanvasIsCreatedWithCorrectWidth()
        {
            ICanvas canvas = new Canvas(10, 20);
            Assert.IsTrue(canvas.Height == 20);
        }

        [TestMethod]
        public void CanvasCannotBeCreatedWithNegativeHeight()
        {
            ICanvas canvas = null;
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => canvas = new Canvas(-10, 20));
        }

        [TestMethod]
        public void CanvasCannotBeCreatedWithNegativeWidth()
        {
            ICanvas canvas = null;
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => canvas = new Canvas(10, -20));
        }

        [TestMethod]
        public void CanvasCannotBeCreatedWithZeroHeight()
        {
            ICanvas canvas = null;
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => canvas = new Canvas(0, 20));
        }

        [TestMethod]
        public void CanvasCannotBeCreatedWithZeroWidth()
        {
            ICanvas canvas = null;
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => canvas = new Canvas(10, 0));
        }

        #endregion

        #region Draw Line

        [TestMethod]
        public void LineCannotExtendBeyondCanvas()
        {
            ICanvas canvas = new Canvas(10, 10);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => canvas.DrawLine(-1, 1, -1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => canvas.DrawLine(7, -1, 7, -1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => canvas.DrawLine(1, 11, 1, 11));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => canvas.DrawLine(11, 1, 11, 1));
        }

        [TestMethod]
        public void LineHasToBeHorizontalOrVertical()
        {
            ICanvas canvas = new Canvas(10, 10);
            Assert.ThrowsException<NotSupportedException>(() => canvas.DrawLine(3, 3, 6, 6));
        }

        [TestMethod]
        public void APointIsALine()
        {
            ICanvas canvas = new Canvas(10, 10);
            canvas.DrawLine(3, 3, 3, 3);
            // What is the assert in this test case?
        }

        [TestMethod]
        public void ValidVerticalLine()
        {
            ICanvas canvas = new Canvas(10, 10);
            canvas.DrawLine(3, 3, 3, 6);
            // What is the assert in this test case?
        }

        [TestMethod]
        public void ValidHorizontalLine()
        {
            ICanvas canvas = new Canvas(10, 10);
            canvas.DrawLine(3, 3, 6, 3);
            // What is the assert in this test case?
        }

        [TestMethod]
        public void LineTouchingCanvasBoundaries()
        {
            ICanvas canvas = new Canvas(10, 10);
            canvas.DrawLine(1, 10, 10, 10);
            // What is the assert in this test case?
        }

        #endregion

        #region Draw Rectangle

        [TestMethod]
        public void RectangleEndPointsCannotExtendBeyondCanvas()
        {
            ICanvas canvas = new Canvas(10, 10);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => canvas.DrawRectangle(-1, -1, 1, 1));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => canvas.DrawRectangle(1, 1, 12, 12));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => canvas.DrawRectangle(12, 12, 20, 5));
        }

        [TestMethod]
        public void APointIsARectangle()
        {
            ICanvas canvas = new Canvas(10, 10);
            canvas.DrawRectangle(3, 3, 3, 3);
        }

        [TestMethod]
        public void ValidRectangle()
        {
            ICanvas canvas = new Canvas(10, 10);
            canvas.DrawRectangle(3, 3, 6, 6);
        }

        #endregion

        #region Bucket Fill

        [TestMethod]
        public void CannotBucketFillWithPointOutsideCanvas()
        {
            ICanvas canvas = new Canvas(10, 10);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => canvas.BucketFillAreaConnectedTo(11, 11, 'c'));
        }

        [TestMethod]
        public void CannotBucketFillWithLineColorCode()
        {
            ICanvas canvas = new Canvas(10, 10);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => canvas.BucketFillAreaConnectedTo(3, 3, 'x'));
        }

        [TestMethod]
        public void BucketFillWithPointOnLineDoesNothing()
        {
            ICanvas canvas = new Canvas(10, 10);

            canvas.DrawLine(3, 2, 6, 2);

            Assert.IsTrue('x' == canvas.GetColorCode(5, 2));

            char lower = canvas.GetColorCode(5, 1);
            char lowerLeft = canvas.GetColorCode(4, 1);
            char lowerRight = canvas.GetColorCode(6, 1);
            char upper = canvas.GetColorCode(5, 3);
            char upperLeft = canvas.GetColorCode(4, 3);
            char upperRight = canvas.GetColorCode(6, 3);

            canvas.BucketFillAreaConnectedTo(5, 2, 'c');

            Assert.IsTrue('x' == canvas.GetColorCode(5, 2));
            Assert.IsTrue(lower == canvas.GetColorCode(5, 1));
            Assert.IsTrue(lowerLeft == canvas.GetColorCode(4, 1));
            Assert.IsTrue(lowerRight == canvas.GetColorCode(6, 1));
            Assert.IsTrue(upper == canvas.GetColorCode(5, 3));
            Assert.IsTrue(upperLeft == canvas.GetColorCode(4, 3));
            Assert.IsTrue(upperRight == canvas.GetColorCode(6, 3));
        }

        #endregion
    }
}
