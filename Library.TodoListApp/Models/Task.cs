// Miguel Quezada
// Assignment #2
// 10/02/2021
using System;
namespace Library.TodoListApp.Models
{
    public class TodoTask : TodoItem
    {
        public string TodoType
        {
            get => "Task";
        }
        public DateTime Deadline { get; set; }
        public bool IsCompleted { get; set; }
        public TodoTask() : base() { }
        public TodoTask(string name, string description, string priority, DateTime deadline, bool isCompleted = false) : base(name, description, priority)
        {
            Deadline = deadline;
            IsCompleted = isCompleted;
        }
        public override void Log()
        {
            Console.WriteLine("-------------");
            Console.WriteLine("Type : Id");
            Console.WriteLine("-------------");
            Console.WriteLine("Task : {0}", Id);
            Console.WriteLine("-------------");
            base.Log();
            Console.WriteLine("Deadline: {0}\nIsCompleted: {1}\n", Deadline.Date.ToShortDateString(), IsCompleted);
        }
    }
}
