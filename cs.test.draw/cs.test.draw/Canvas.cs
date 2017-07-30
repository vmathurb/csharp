using System;
using System.Collections.Generic;

namespace cs.test.draw
{
    public class Canvas : ICanvas
    {
        private int _height;
        private int _width;

        char[,] _canvasPoints = null;

        const char CHAR_LINE = 'x';
        const char CHAR_HORIZONTAL_BORDER = '-';
        const char CHAR_VERTICAL_BORDER = '|';

        private int _getCounter;
        private int _setCounter;
        private int _matchCounter;

        private void ResetCounters()
        {
            _getCounter = 0;
            _setCounter = 0;
            _matchCounter = 0;
        }

        private void PrintCounters()
        {
            Console.WriteLine("Operation took Get:{0} Set:{1} Match:{2}", _getCounter, _setCounter, _matchCounter);
        }

        private bool InCanvasHeightBounds(int y)
        {
            return y > 0 && y <= this._height;
        }

        private bool InCanvasWidthBounds(int x)
        {
            return x > 0 && x <= this._width;
        }

        private bool Match(char colorCode, char seedPointColorCode)
        {
            _matchCounter++;
            return colorCode == seedPointColorCode;
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
                SetColorCode(i, 0, CHAR_HORIZONTAL_BORDER);
                SetColorCode(i, height + 1, CHAR_HORIZONTAL_BORDER);
            }

            for (int i = 1; i <= height; i++)
            {
                SetColorCode(0, i, CHAR_VERTICAL_BORDER);
                SetColorCode(width + 1, i, CHAR_VERTICAL_BORDER);
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
                    SetColorCode(x1, y, CHAR_LINE);
                }
            }
            else
            {
                int lower = (x1 < x2) ? x1 : x2;
                int higher = (lower == x1) ? x2 : x1;

                for (int x = lower; x <= higher; x++)
                {
                    SetColorCode(x, y1, CHAR_LINE);
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
            ResetCounters();

            if (!InCanvasWidthBounds(x))
            {
                throw new ArgumentOutOfRangeException(String.Format("x has to be in the range [1-{0}]", this._width));
            }

            if (!InCanvasHeightBounds(y))
            {
                throw new ArgumentOutOfRangeException(String.Format("y has to be in the range [1-{0}]", this._height));
            }

            if (Match(CHAR_LINE, colorCode))
            {
                throw new ArgumentOutOfRangeException("colorCode",
                    String.Format("Fill char {0} cannot match line char {1}", colorCode, CHAR_LINE));
            }

            if (Match(CHAR_LINE, GetColorCode(x, y)))
            {
                // NOTE: Assumption that bucket fill with point on line does nothing
                return;
            }

            Fill4WayRecursive(x, y, colorCode);
            //FillScanLine(x, y, colorCode);
            PrintCounters();
        }

        private void Fill4WayRecursive(int x, int y, char colorCode)
        {
            char seedPointColorCode = GetColorCode(x, y);

            if (Match(colorCode, seedPointColorCode) || Match(CHAR_LINE, seedPointColorCode))
            {
                return;
            }
            else
            {
                SetColorCode(x, y, colorCode);
            }

            if (x + 1 <= _width)
                Fill4WayRecursive(x + 1, y, colorCode);

            if (x - 1 > 0)
                Fill4WayRecursive(x - 1, y, colorCode);

            if (y + 1 <= _height)
                Fill4WayRecursive(x, y + 1, colorCode);

            if (y - 1 > 0)
                Fill4WayRecursive(x, y - 1, colorCode);
        }

        void FillScanLine(int x, int y, char colorCode)
        {
            ICanvasRenderer renderer = new ConsoleCanvasRenderer();
            Queue<Point> pointQueue = new Queue<Point>();
            pointQueue.Enqueue(new Point(x, y));

            while (pointQueue.Count > 0)
            {
                //renderer.Render(this);
                //System.Threading.Thread.Sleep(200);

                Point p = pointQueue.Dequeue();

                for (int xleft = p.X; xleft > 0; xleft--)
                {
                    char pointColorCode = GetColorCode(xleft, p.Y);

                    if (!Match(pointColorCode, CHAR_LINE) && !Match(pointColorCode, colorCode))
                    {
                        SetColorCode(xleft, p.Y, colorCode);
                    }
                    else
                    {
                        break;
                    }

                    char topPointColorCode = GetColorCode(xleft, p.Y + 1);

                    if (p.Y + 1 < _height + 1 && !Match(topPointColorCode, CHAR_LINE) && !Match(topPointColorCode, colorCode))
                    {
                        pointQueue.Enqueue(new Point(xleft, p.Y + 1));
                    }

                    char bottomPointColorCode = GetColorCode(xleft, p.Y - 1);

                    if (p.Y - 1 > 0 && !Match(bottomPointColorCode, CHAR_LINE) && !Match(bottomPointColorCode, colorCode))
                    {
                        pointQueue.Enqueue(new Point(xleft, p.Y - 1));
                    }
                }

                for (int xRight = p.X + 1; xRight < _width + 1; xRight++)
                {
                    char pointColorCode = GetColorCode(xRight, p.Y);

                    if (!Match(pointColorCode, CHAR_LINE) && !Match(pointColorCode, colorCode))
                    {
                        SetColorCode(xRight, p.Y, colorCode);
                    }
                    else
                    {
                        break;
                    }

                    char topPointColorCode = GetColorCode(xRight, p.Y + 1);

                    if (p.Y + 1 < _height + 1 && !Match(topPointColorCode, CHAR_LINE) && !Match(topPointColorCode, colorCode))
                    {
                        pointQueue.Enqueue(new Point(xRight, p.Y + 1));
                    }

                    char bottomPointColorCode = GetColorCode(xRight, p.Y - 1);

                    if (p.Y - 1 > 0 && !Match(bottomPointColorCode, CHAR_LINE) && !Match(bottomPointColorCode, colorCode))
                    {
                        pointQueue.Enqueue(new Point(xRight, p.Y - 1));
                    }
                }
            }
        }

        public char GetColorCode(int x, int y)
        {
            _getCounter++;
            return _canvasPoints[y, x];
        }

        public void SetColorCode(int x, int y, char colorCode)
        {
            _setCounter++;
            _canvasPoints[y, x] = colorCode;
        }
    }
}