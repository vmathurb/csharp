using System;

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
                _canvasPoints[0, i] = HORIZONTAL_BORDER_CHAR;
                _canvasPoints[height + 1, i] = HORIZONTAL_BORDER_CHAR;
            }

            for (int i = 1; i <= height; i++)
            {
                _canvasPoints[i, 0] = VERTICAL_BORDER_CHAR;
                _canvasPoints[i, width + 1] = VERTICAL_BORDER_CHAR;
            }
        }

        int ICanvas.Height
        {
            get { return this._height; }
        }

        int ICanvas.Width
        {
            get { return this._width; }
        }

        void ICanvas.DrawLine(int x1, int y1, int x2, int y2)
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
                    _canvasPoints[y, x1] = LINE_CHAR;
                }
            }
            else
            {
                int lower = (x1 < x2) ? x1 : x2;
                int higher = (lower == x1) ? x2 : x1;

                for (int x = lower; x <= higher; x++)
                {
                    _canvasPoints[y1,x] = LINE_CHAR;
                }
            }
        }

        void ICanvas.DrawRectangle(int upperLeftx1, int upperLefty1, int lowerRightx2, int lowerRighty2)
        {
            (this as ICanvas).DrawLine(upperLeftx1, upperLefty1, upperLeftx1, lowerRighty2);
            (this as ICanvas).DrawLine(upperLeftx1, lowerRighty2, lowerRightx2, lowerRighty2);
            (this as ICanvas).DrawLine(lowerRightx2, lowerRighty2, lowerRightx2, upperLefty1);
            (this as ICanvas).DrawLine(lowerRightx2, upperLefty1, upperLeftx1, upperLefty1);
        }

        void ICanvas.BucketFillAreaConnectedTo(int x, int y, char colorCode)
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

            if (LINE_CHAR == _canvasPoints[y, x])
            {
                // NOTE: Assumption that bucket fill with point on line does nothing
                return;
            }

            Fill(x, y, colorCode);
        }

        private void Fill(int x, int y, char colorCode)
        {
            if (colorCode == _canvasPoints[y, x] || LINE_CHAR == _canvasPoints[y, x])
            {
                return;
            }
            else
            {
                _canvasPoints[y, x] = colorCode;
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

        void ScanFill(int x, int y, char colorCode)
        {
            for (int xleft = x; xleft >= 0; xleft--)
            {
                for (int ytop = y; ytop < _height + 2; ytop++)
                {
                    if (_canvasPoints[xleft, ytop] != LINE_CHAR)
                    {
                        _canvasPoints[xleft, ytop] = colorCode;
                    }
                    else
                    {
                        break;
                    }
                }

                for (int ybottom = y - 1; ybottom >= 0; ybottom--)
                {
                    if (_canvasPoints[xleft, ybottom] != LINE_CHAR)
                    {
                        _canvasPoints[xleft, ybottom] = colorCode;
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        char ICanvas.QueryPoint(int x, int y)
        {
            return _canvasPoints[y, x]; 
        }
    }
}