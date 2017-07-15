using System;

namespace cs.test.draw
{
    public class ConsoleCanvasRenderer : ICanvasRenderer
    {
        void ICanvasRenderer.Render(ICanvas canvas)
        {
            const string PAD_STRING = "  ";

            for (int y = canvas.Height + 1; y >= 0; y--)
            {
                Console.Write(PAD_STRING);

                for (int x = 0; x <= canvas.Width + 1; x++)
                {
                    Console.Write(canvas.GetColorCode(x, y));
                }

                Console.WriteLine();
            }
        }
    }
}