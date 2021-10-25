using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Library.TodoListApp.Models;
using Library.TodoListApp.util;
using TodoListApp.Dialogs;

namespace TodoListApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public bool EditIsEnabled
        {
            get => SelectedItem != null;
        }
        public string SearchQuery { get; set; }
        private TodoItem selectedItem;
        public TodoItem SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                NotifyPropertyChanged("EditIsEnabled");
            }
        }
        public ObservableCollection<TodoItem> TodoItems { get; set; }
        private ObservableCollection<TodoItem> filteredItems;
        public ObservableCollection<TodoItem> FilteredItems
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(SearchQuery))
                {
                    filteredItems =
                        new ObservableCollection<TodoItem>(TodoItems.Where(item => item.Contains(SearchQuery)).ToList());
                    return filteredItems;
                }
                return TodoItems;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private async void Init()
        {
            var items = await JsonData.Load();
            items.ForEach(delegate (TodoItem item) {
                if (item is TodoTask)
                    TodoItems.Add(item as TodoTask);
                else
                    TodoItems.Add(item as TodoAppointment);
            });
        }
        public MainViewModel()
        {
            TodoItems = new ObservableCollection<TodoItem>();
            Init();
        }

        public void Remove()
        {
            if (SelectedItem == null) return;
            TodoItems.Remove(SelectedItem);
            JsonData.Save(TodoItems);
        }

        public void SortList()
        {
            var sorted = new ObservableCollection<TodoItem>(FilteredItems.OrderBy(item => item, new PriorityComparer()));
            foreach (var item in FilteredItems.ToList()) FilteredItems.Remove(item);
            foreach(var item in sorted) FilteredItems.Add(item);
            NotifyPropertyChanged("FilteredItems");
            NotifyPropertyChanged("TodoItems");
        }
        public void RefreshList()
        {
            NotifyPropertyChanged("FilteredItems");
        }
        public async Task EditTicket()
        {
            var diag = new TodoItemDialog(TodoItems, SelectedItem);
            NotifyPropertyChanged("SelectedItem");
            await diag.ShowAsync();
            JsonData.Save(TodoItems);
        }
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    class PriorityComparer : IComparer<TodoItem>
    {
        public int Compare(TodoItem a, TodoItem b)
        {
            var priorityRank = new Dictionary<string, int>
            {
                {"Low", 0},
                {"Medium", 1},
                {"High", 2}
            };
            if ((a.Priority == null || b.Priority == null) || (a.Priority == b.Priority)) return 0;
            if (priorityRank[a.Priority] < priorityRank[b.Priority]) return 1;
            if (priorityRank[a.Priority] > priorityRank[b.Priority]) return -1;
            return 0;
        }
    }
}
