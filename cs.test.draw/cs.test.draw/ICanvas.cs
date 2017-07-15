using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.test.draw
{
    /// <summary>
    /// Canvas on which lines and rectangles can be drawn
    /// </summary>
    public interface ICanvas
    {
        /// <summary>
        /// Height of canvas 
        /// </summary>
        int Height { get; }

        /// <summary>
        /// Width of canvas
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Draw a line from (x1,y1) to (x2,y2)
        /// </summary>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        void DrawLine(int x1, int y1, int x2, int y2);

        /// <summary>
        /// Draw a rectangle where upper left corner is (x1,y1) and lower right corner is (x2,y2)
        /// </summary>
        void DrawRectangle(int upperLeftx1, int upperLefty1, int lowerRightx2, int lowerRighty2);

        /// <summary>
        /// Fill the entire area that (x,y) belongs to with color identified by colorCode. "Bucket Fill"
        /// </summary>
        void BucketFillAreaConnectedTo(int x, int y, char colorCode);

        /// <summary>
        /// Gets color code at given point
        /// </summary>
        char GetColorCode(int x, int y);

        /// <summary>
        /// Set color code at given point
        /// </summary>
        void SetColorCode(int x, int y, char colorCode);
    }
}
