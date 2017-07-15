using System;
using System.Collections.Generic;

namespace cs.test.draw
{
    public class Canvas : ICanvas
    {
        private int _height;
        private int _width;

        char[,] _canvasPoints = null;

        const char LINE_CHAR = 'x';
        const char HORIZONTAL_BORDER_CHAR = '-';
        const char VERTICAL_BORDER_CHAR = '|';

        private bool InCanvasHeightBounds(int y)
        {
            return y > 0 && y <= this._height;
        }

        private bool InCanvasWidthBounds(int x)
        {
            return x > 0 && x <= this._width;
        }

        public Canvas(int width, int height)
        {
            if (height <= 0)
                throw new ArgumentOutOfRangeException("Height of canvas must be greater than 0");
            else
                this._height = height;

            if (width <= 0)
                throw new ArgumentOutOfRangeException("Width of canvas must be greater than 0");
            else
                this._width = width;

            // NOTE: The +2 is for the borders
            _canvasPoints = new char[height + 2, width + 2];

            for (int i = 0; i < width + 2; i++)
            {
                SetColorCode(i, 0, HORIZONTAL_BORDER_CHAR);
                SetColorCode(i, height + 1, HORIZONTAL_BORDER_CHAR);
            }

            for (int i = 1; i <= height; i++)
            {
                SetColorCode(0, i, VERTICAL_BORDER_CHAR);
                SetColorCode(width + 1, i, VERTICAL_BORDER_CHAR);
            }
        }

        public int Height
        {
            get { return this._height; }
        }

        public int Width
        {
            get { return this._width; }
        }

        public void DrawLine(int x1, int y1, int x2, int y2)
        {
            // TODO: Split validation out

            if (!InCanvasWidthBounds(x1))
            {
                throw new ArgumentOutOfRangeException(String.Format("x1 has to be in the range [1-{0}]", this._width));
            }

            if (!InCanvasWidthBounds(x2))
            {
                throw new ArgumentOutOfRangeException(String.Format("x2 has to be in the range [1-{0}]", this._width));
            }

            if (!InCanvasHeightBounds(y1))
            {
                throw new ArgumentOutOfRangeException(String.Format("y1 has to be in the range [1-{0}]", this._height));
            }

            if (!InCanvasHeightBounds(y2))
            {
                throw new ArgumentOutOfRangeException(String.Format("y2 has to be in the range [1-{0}]", this._height));
            }

            if (x1 != x2 && y1 != y2)
            {
                throw new NotSupportedException("Line has to be vertical or horizontal");
            }

            if (x1 == x2)
            {
                int lower = (y1 < y2) ? y1 : y2;
                int higher = (lower == y1) ? y2 : y1;

                for (int y = lower; y <= higher; y++)
                {
                    SetColorCode(x1, y, LINE_CHAR);
                }
            }
            else
            {
                int lower = (x1 < x2) ? x1 : x2;
                int higher = (lower == x1) ? x2 : x1;

                for (int x = lower; x <= higher; x++)
                {
                    SetColorCode(x, y1, LINE_CHAR);
                }
            }
        }

        public void DrawRectangle(int upperLeftx1, int upperLefty1, int lowerRightx2, int lowerRighty2)
        {
            DrawLine(upperLeftx1, upperLefty1, upperLeftx1, lowerRighty2);
            DrawLine(upperLeftx1, lowerRighty2, lowerRightx2, lowerRighty2);
            DrawLine(lowerRightx2, lowerRighty2, lowerRightx2, upperLefty1);
            DrawLine(lowerRightx2, upperLefty1, upperLeftx1, upperLefty1);
        }

        public void BucketFillAreaConnectedTo(int x, int y, char colorCode)
        {
            if (!InCanvasWidthBounds(x))
            {
                throw new ArgumentOutOfRangeException(String.Format("x has to be in the range [1-{0}]", this._width));
            }

            if (!InCanvasHeightBounds(y))
            {
                throw new ArgumentOutOfRangeException(String.Format("y has to be in the range [1-{0}]", this._height));
            }

            if (LINE_CHAR == colorCode)
            {
                throw new ArgumentOutOfRangeException("colorCode",
                    String.Format("Fill char {0} cannot match line char {1}", colorCode, LINE_CHAR));
            }

            if (LINE_CHAR == GetColorCode(x,y))
            {
                // NOTE: Assumption that bucket fill with point on line does nothing
                return;
            }

            ScanLineFill(x, y, colorCode);
        }

        private void Fill(int x, int y, char colorCode)
        {
            if (colorCode == GetColorCode(x, y) || LINE_CHAR == GetColorCode(x, y))
            {
                return;
            }
            else
            {
                SetColorCode(x, y, colorCode);
            }

            if (x + 1 <= _width)
                Fill(x + 1, y, colorCode);

            if (x - 1 > 0)
                Fill(x - 1, y, colorCode);

            if (y + 1 <= _height)
                Fill(x, y + 1, colorCode);

            if (y - 1 > 0)
                Fill(x, y - 1, colorCode);
        }

        void ScanLineFill(int x, int y, char colorCode)
        {
            Queue<Point> pointQueue = new Queue<Point>();
            pointQueue.Enqueue(new Point(x, y));

            while (pointQueue.Count > 0)
            {
                Point p = pointQueue.Dequeue();

                for (int xleft = p.X; xleft > 0; xleft--)
                {

                    if (GetColorCode(xleft, p.Y) != LINE_CHAR)
                    {
                        SetColorCode(xleft, p.Y, colorCode);
                    }
                    else
                    {
                        break;
                    }

                    if (p.Y + 1 < _height + 1 && GetColorCode(xleft, p.Y + 1) != LINE_CHAR && GetColorCode(xleft, p.Y + 1) != colorCode)
                    {
                        pointQueue.Enqueue(new Point(xleft, p.Y + 1));
                    }

                    if (p.Y - 1 > 0 && GetColorCode(xleft, p.Y - 1) != LINE_CHAR && GetColorCode(xleft, p.Y - 1) != colorCode)
                    {
                        pointQueue.Enqueue(new Point(xleft, p.Y - 1));
                    }
                }

                for (int xright = p.X; xright < _width + 1; xright++)
                {
                    if (GetColorCode(xright, p.Y) != LINE_CHAR)
                    {
                        SetColorCode(xright, p.Y, colorCode);
                    }
                    else
                    {
                        break;
                    }

                    if (p.Y + 1 < _height + 1 && GetColorCode(xright, p.Y + 1) != LINE_CHAR && GetColorCode(xright, p.Y + 1) != colorCode)
                    {
                        pointQueue.Enqueue(new Point(xright, p.Y + 1));
                    }

                    if (p.Y - 1 > 0 && GetColorCode(xright, p.Y - 1) != LINE_CHAR && GetColorCode(xright, p.Y - 1) != colorCode)
                    {
                        pointQueue.Enqueue(new Point(xright, p.Y - 1));
                    }
                }
            }
        }

        public char GetColorCode(int x, int y)
        {
            return _canvasPoints[y, x];
        }

        public void SetColorCode(int x, int y, char colorCode)
        {
            _canvasPoints[y, x] = colorCode;
        }
    }
}