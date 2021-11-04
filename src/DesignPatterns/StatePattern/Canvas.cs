using System;
using System.Collections.Generic;
using System.Text;

namespace StatePattern
{
    public class Canvas
    {
        public ToolType ToolType { get; set; }

        public void MouseDown()
        {
            if (ToolType == ToolType.Selection)
            {
                Console.WriteLine("Selection icon");
            }
            else if (ToolType == ToolType.Brush)
            {
                Console.WriteLine("Brush icon");
            }
            else if (ToolType == ToolType.Eraser)
            {
                Console.WriteLine("Eraser icon");
            }
        }

        public void MouseUp()
        {
            if (ToolType == ToolType.Selection)
            {
                Console.WriteLine("Draw dashed rectangle");
            }
            else if (ToolType == ToolType.Brush)
            {
                Console.WriteLine("Draw rectangle");
            }
            else if (ToolType == ToolType.Eraser)
            {
                Console.WriteLine("Eraser something");
            }
        }
    }

    public enum ToolType
    {
        Selection,
        Brush,
        Eraser
    }
}
