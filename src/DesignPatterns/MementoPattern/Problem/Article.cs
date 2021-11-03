using System.Collections.Generic;
using System.Text;

namespace MementoPattern.Problem
{
    public class Article
    {
        public string Content { get; set; }
        private IList<string> previousContents;

        public string Title { get; set; }
        private IList<string> previousTitles;
    }
}
