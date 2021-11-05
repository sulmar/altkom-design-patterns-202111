using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace VisitorPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Visitor Pattern!");

            Form form = Get();

            IVisitor visitor = new MarkdownVisitor();

            form.Accept(visitor);

            string html = visitor.Output;

            System.IO.File.WriteAllText("index.html", html);
        }

        public static Form Get()
        {
            Form form = new Form
            {
                Name = "/forms/customers",
                Title = "Design Patterns",

                Body = new Collection<ControlBase>
                {

                    new LabelControl { Caption = "Person", Name = "lblName" },
                    new TextBoxControl { Caption = "FirstName", Name = "txtFirstName", Value = "John"},
                    new CheckBoxControl { Caption = "IsAdult", Name = "chkIsAdult", Value = true },
                    new ButtonControl {  Caption = "Submit", Name = "btnSubmit", ImageSource = "save.png" },
                }

            };

            return form;
        }
    }

    #region Models

    public class Form : ControlBase
    {
        public string Title { get; set; }
        public ICollection<ControlBase> Body { get; set; }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

  


    #endregion

    // Abstract Visitor
    public interface IVisitor
    {
        void Visit(Form form);
        void Visit(LabelControl control);
        void Visit(TextBoxControl control);
        void Visit(CheckBoxControl control);
        void Visit(ButtonControl control);

        string Output { get; }
    }

    // Concrete Visitor

    public class HtmlVisitor : IVisitor
    {
        private readonly StringBuilder builder = new StringBuilder();

        public void Visit(Form form)
        {
            string header = "<html>";

            header += $"<title>{form.Title}</title>";

            header += "<body>";

            builder.AppendLine(header);

            foreach (var control in form.Body)
            {
                control.Accept(this);                
            }

            string footer = "</body>";
            footer += "</html>";

            builder.AppendLine(footer);
        }

        public void Visit(LabelControl control)
        {
            builder.AppendLine($"<span>{control.Caption}</span>");
        }

        public void Visit(TextBoxControl control)
        {
            builder.AppendLine($"<span>{control.Caption}</span><input type='text' value='{control.Value}'></input>");
        }

        public void Visit(CheckBoxControl control)
        {
            builder.AppendLine($"<span>{control.Caption}</span><input type='checkbox' value='{control.Value}'></input>");
        }

        public void Visit(ButtonControl control)
        {
            builder.AppendLine($"<button><img src='{control.ImageSource}'/>{control.Caption}</button>");
        }

        public string Output => builder.ToString();
    }

    // Concrete Visitor

    public class MarkdownVisitor : IVisitor
    {
        private readonly StringBuilder builder = new StringBuilder();

        public void Visit(Form form)
        {
            builder.AppendLine( $"# {form.Title}");


            foreach (var control in form.Body)
            {
                control.Accept(this);
            }

        }

        public void Visit(LabelControl control)
        {
            builder.AppendLine($"## {control.Caption}");
        }

        public void Visit(TextBoxControl control)
        {
            builder.AppendLine($"**{control.Caption}** _{control.Value}_");
        }

        public void Visit(CheckBoxControl control)
        {
            builder.AppendLine($"**{control.Caption}** _{control.Value}_");
        }

        public void Visit(ButtonControl control)
        {
            builder.AppendLine($"**{control.Caption}** [{control.ImageSource}]");
        }

        public string Output => builder.ToString();

    }

    // Abstract Element
    public abstract class ControlBase
    {
        public string Name { get; set; }
        public string Caption { get; set; }

        public abstract void Accept(IVisitor visitor);
    }

    // Concrete Element
    public class LabelControl : ControlBase
    {
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    // Concrete Element
    public class TextBoxControl : ControlBase
    {
        public string Value { get; set; }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    // Concrete Element
    public class CheckBoxControl : ControlBase
    {
        public bool Value { get; set; }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    // Concrete Element
    public class ButtonControl : ControlBase
    {
        public string ImageSource { get; set; }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }


}
