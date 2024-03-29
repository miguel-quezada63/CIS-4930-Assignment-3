﻿using System;
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
        private TodoItem _selectedItem;
        private ObservableCollection<TodoItem> _filteredItems;
        public bool EditIsEnabled => SelectedItem != null;
        public bool SortByHighest { get; set; }
        public string SortByType => SortByHighest ? "Sort by lowest priority" : "Sort by highest priority";
        public string SortByTypeColor => SortByHighest ? "#007bff" : "#dc3545";
        public string SearchQuery { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public TodoItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged(nameof(EditIsEnabled));
            }
        }
        public ObservableCollection<TodoItem> TodoItems { get; set; }
        public ObservableCollection<TodoItem> FilteredItems
        {
            get
            {
                if (string.IsNullOrWhiteSpace(SearchQuery)) return TodoItems;
                _filteredItems =
                    new ObservableCollection<TodoItem>(TodoItems.Where(item => item.Contains(SearchQuery)).ToList());
                return _filteredItems;
            }
        }
        private async void Init()
        {
            var items = await JsonData.Load();
            items.ForEach(delegate (TodoItem item) {
                if (item is TodoTask todoTask)
                    TodoItems.Add(todoTask);
                else if(item is TodoAppointment todoAppointment)
                    TodoItems.Add(todoAppointment);
                else 
                    TodoItems.Add(item);
            }); 
            SortByHighest = FilteredItems.Count > 0 && FilteredItems[0]?.Priority == TodoItem.PriorityType.HIGH;
            NotifyPropertyChanged(nameof(SortByType));
            NotifyPropertyChanged(nameof(SortByTypeColor));
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
            SortByHighest = !SortByHighest;
            var orderedByPriority = FilteredItems.OrderBy(item => item, new PriorityComparer());
            var sorted = new ObservableCollection<TodoItem>(SortByHighest ? orderedByPriority : orderedByPriority.Reverse());
            FilteredItems.Clear();
            foreach (var item in sorted) FilteredItems.Add(item);
            NotifyPropertyChanged(nameof(FilteredItems));
            NotifyPropertyChanged(nameof(SortByType));
            NotifyPropertyChanged(nameof(SortByTypeColor));
        }
        public void RefreshList()
        {
            NotifyPropertyChanged(nameof(FilteredItems));
        }
        public async Task EditTicket()
        {
            await (new TodoItemDialog(TodoItems, SelectedItem)).ShowAsync();
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
            var priorityRank = new Dictionary<string, int> {{TodoItem.PriorityType.LOW, 0}, {TodoItem.PriorityType.MEDIUM, 1}, {TodoItem.PriorityType.HIGH, 2}};
            if (a.Priority == null || b.Priority == null || a.Priority == b.Priority) return 0;
            if (priorityRank[a.Priority] < priorityRank[b.Priority]) return 1;
            if (priorityRank[a.Priority] > priorityRank[b.Priority]) return -1;
            return 0;
        }
    }
}
