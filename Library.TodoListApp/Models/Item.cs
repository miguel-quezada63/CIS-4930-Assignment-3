// Miguel Quezada
// Assignment #2
// 10/02/2021
using System;
namespace Library.TodoListApp.Models
{
    public class TodoItem
    {
        public class PriorityType
        {
            public const string LOW = "Low";
            public const string MEDIUM = "Medium";
            public const string HIGH = "High";
        }
        public virtual string ItemSpecificProps { get; set; }
        public virtual string TodoType => "Item";
        public TodoItem() => Id = Guid.NewGuid().ToString();
        public TodoItem(string name, string description, string priority = PriorityType.LOW)
        {
            Id = Guid.NewGuid().ToString();
            Name = name;
            Description = description;
            Priority = priority;
        }
        public string Id { get; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string PriorityColor
        {
            get
            {
                switch (Priority)
                {
                    case PriorityType.HIGH:
                        return "#dc3545";
                    case PriorityType.MEDIUM:
                        return "#ffc107";
                    default:
                        return "#007bff";
                }
            }
        }

        public virtual bool Contains(string query) => Name.ToLower().Contains(query.ToLower()) || Description.ToLower().Contains(query.ToLower());
        public virtual void Log() => Console.Write("Name: {0}\nDescription: {1}\n", Name, Description);
    }
}
