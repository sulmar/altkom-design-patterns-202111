using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VisitorPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Visitor Pattern!");

            Form form = Get();

            string html = form.GetHtml();

            System.IO.File.WriteAllText("index.html", html);
        }

        public static Form Get()
        {
            Form form = new Form
            {
                Name = "/forms/customers",
                Title = "Design Patterns",

                Body = new Collection<Control>
                {

                    new Control { Type = ControlType.Label, Caption = "Person", Name = "lblName" },
                    new Control { Type = ControlType.TextBox, Caption = "FirstName", Name = "txtFirstName", Value = "John"},
                    new Control { Type = ControlType.Checkbox, Caption = "IsAdult", Name = "chkIsAdult", Value = "true" },
                    new Control {  Type = ControlType.Button, Caption = "Submit", Name = "btnSubmit", ImageSource = "save.png" },
                }

            };

            return form;
        }
    }

    #region Models

    public class Form
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public ICollection<Control> Body { get; set; }

        public string GetHtml()
        {
            string html = "<html>";

            html += $"<title>{Title}</title>";

            html += "<body>";

            foreach (var control in Body)
            {
                switch (control.Type)
                {
                    case ControlType.Label:
                        html += $"<span>{control.Caption}</span>"; break;

                    case ControlType.TextBox:
                        html += $"<span>{control.Caption}</span><input type='text' value='{control.Value}'></input>"; break;

                    case ControlType.Checkbox:
                        html += $"<span>{control.Caption}</span><input type='checkbox' value='{control.Value}'></input>"; break;

                    case ControlType.Button:
                        html += $"<button><img src='{control.ImageSource}'/>{control.Caption}</button>"; break;
                }

            }

            html += "</body>";
            html += "</html>";

            return html;
        }
    }

    public class Control
    {
        public string Name { get; set; }
        public string Caption { get; set; }
        public ControlType Type { get; set; }
        public string Value { get; set; }
        public string ImageSource { get; set; }
    }

    public enum ControlType
    {
        Label,
        TextBox,
        Checkbox,
        Button
    }


    #endregion

}
