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

            canvas.DrawLine(6, 5, 9, 5);
            canvas.DrawRectangle(2, 8, 15, 3);
            canvas.BucketFillAreaConnectedTo(8, 6, 'p');
            
            //canvas.DrawLine(3, 3, 6, 3);
            //canvas.DrawLine(6, 3, 6, 6);
            
            ICanvasRenderer consoleRenderer = new ConsoleCanvasRenderer();
            consoleRenderer.Render(canvas);
        }
    }
}
