using System;

namespace SciChart.Sandbox
{
    public class ExampleDataObject
    {
        protected bool Equals(ExampleDataObject other)
        {
            return Type == other.Type;
        }

        public override int GetHashCode()
        {
            return (Type != null ? Type.GetHashCode() : 0);
        }

        public Type Type { get; set; }
        public string Title { get; set; }

        public ExampleDataObject(Type type, string title)
        {
            Type = type;
            Title = title;
        }

        public override string ToString()
        {
            return Title;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
    }
}