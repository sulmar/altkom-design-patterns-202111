using System;
using System.Collections.Generic;
using System.Text;

namespace StatePattern
{
    // Abstract State
    public interface ITool
    {
        void MouseDown();
        void MouseUp();
        void KeyDown();     
    }

    // Concrete State
    public class SelectionTool : ITool
    {
        public void KeyDown()
        {
            throw new NotImplementedException();
        }

        public void MouseDown()
        {
            Console.WriteLine("Selection icon");
        }

        public void MouseUp()
        {
            Console.WriteLine("Draw dashed rectangle");
        }
    }

    // Concrete State
    public class BrushTool : ITool
    {
        public void KeyDown()
        {
            throw new NotImplementedException();
        }

        public void MouseDown()
        {
            Console.WriteLine("Brush icon");
        }

        public void MouseUp()
        {
            Console.WriteLine("Draw rectangle");
        }
    }
    
    // Concrete State
    public class EraserTool : ITool
    {
        public void KeyDown()
        {
            throw new NotImplementedException();
        }

        public void MouseDown()
        {
            Console.WriteLine("Eraser icon");
        }

        public void MouseUp()
        {
            Console.WriteLine("Erase selected object");
        }
    }

    // Context
    public class Canvas
    {
        // Current state
        public ITool CurrentTool { get; set; }

        public void MouseDown()
        {
            CurrentTool.MouseDown();
        }

        public void MouseUp()
        {
            CurrentTool.MouseUp();
        }
    }

    
}
