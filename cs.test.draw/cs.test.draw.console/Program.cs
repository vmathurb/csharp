using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.test.draw.console
{
    class Program
    {
        static void Main(string[] args)
        {
            ICanvas canvas = new Canvas(20, 8);

            canvas.DrawLine(3, 2, 6, 2);
            canvas.DrawRectangle(1, 7, 15, 3);
            canvas.BucketFillAreaConnectedTo(5, 4, 'p');
            
            //canvas.DrawLine(3, 3, 6, 3);
            //canvas.DrawLine(6, 3, 6, 6);
            
            ICanvasRenderer consoleRenderer = new ConsoleCanvasRenderer();
            consoleRenderer.Render(canvas);
        }
    }
}
