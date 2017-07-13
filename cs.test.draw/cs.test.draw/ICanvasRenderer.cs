using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs.test.draw
{
    /// <summary>
    /// Canvas renderer
    /// </summary>
    public interface ICanvasRenderer
    {
        /// <summary>
        /// Renders the canvas
        /// </summary>
        /// <param name="canvas"></param>
        void Render(ICanvas canvas);
    }
}
