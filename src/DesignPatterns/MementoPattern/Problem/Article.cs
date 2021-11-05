using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MementoPattern.Problem
{
    // Originator
    public class Article
    {
        public string Content { get; set; }
        public string Title { get; set; }
        public bool IsSelected { get; set; }

        public ArticleMemento CreateMemento()
        {
            return new ArticleMemento(this.Content, this.Title);
        }

        // Restore from memento
        public void SetMemento(ArticleMemento articleMemento)
        {
            this.Content = articleMemento.Content;
            this.Title = articleMemento.Title;
        }
    }


    // Memento
    public class ArticleMemento
    {
        public ArticleMemento(string content, string title)
        {
            SnapshotDate = DateTime.Now;
            Id = Guid.NewGuid();

            Content = content;
            Title = title;            
        }

        public Guid Id { get; set; }
        public DateTime SnapshotDate { get; }

        public string Content { get; }
        public string Title { get; }

        public override string ToString()
        {
            return $"[{SnapshotDate}] {Id} - {Content} : { Title}";
        }


    }

    // Abstract Caretaker
    public interface IArticleCaretaker
    {
        void SetState(ArticleMemento memento);
        ArticleMemento GetState();
        IEnumerable<ArticleMemento> History { get; }
    }

    // Caretaker
    public class ArticleCaretaker : IArticleCaretaker
    {
        private ArticleMemento memento;

        public IEnumerable<ArticleMemento> History => throw new NotSupportedException();

        public void SetState(ArticleMemento memento)
        {
            this.memento = memento;
        }

        public ArticleMemento GetState()
        {
            return memento;
        }
    }

    public class HistoryArticleCaretaker : IArticleCaretaker
    {
        private Stack<ArticleMemento> states = new Stack<ArticleMemento>();

        public void SetState(ArticleMemento memento)
        {
            states.Push(memento);
        }

        public ArticleMemento GetState()
        {
            if (CanGetState)
                return states.Pop();
            else
                return new ArticleMemento(string.Empty, string.Empty);
        }

        public bool CanGetState => states.Any();

        public IEnumerable<ArticleMemento> History => states.Reverse();        
    }

    public class RedisArticleCaretaker : IArticleCaretaker
    {
        public IEnumerable<ArticleMemento> History => throw new NotImplementedException();

        public ArticleMemento GetState()
        {
            throw new System.NotImplementedException();
        }

        public void SetState(ArticleMemento memento)
        {
            throw new System.NotImplementedException();
        }
    }
}
