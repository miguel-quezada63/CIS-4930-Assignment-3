using Library.TodoListApp.Models;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Library.TodoListApp.util;
using TodoListApp.ViewModels;

namespace TodoListApp.Dialogs
{
    public sealed partial class TodoItemDialog : ContentDialog
    {
        private readonly ObservableCollection<TodoItem> _todoItems;
        public TodoItemDialog(ObservableCollection<TodoItem> todoItems)
        {
            this.InitializeComponent();
            DataContext = new DialogViewModel();
            this._todoItems = todoItems;
        }
        public TodoItemDialog(ObservableCollection<TodoItem> todoItems, TodoItem item)
        {
            InitializeComponent();
            DataContext = new DialogViewModel(item);
            this._todoItems= todoItems;
        }
        private void Dialog_OkayBtnClick(object sender, RoutedEventArgs args)
        {
            var dc = DataContext as DialogViewModel;
            var itemToEdit = dc?.NewItem;
            var i = _todoItems.IndexOf(itemToEdit);
            if (i >= 0)
            {
                _todoItems.Remove(itemToEdit);
                _todoItems.Insert(i, itemToEdit);
            }else
                _todoItems.Add(dc?.NewItem);
            JsonData.Save(_todoItems);
            this.Hide();
        }
        private void Dialog_CancelBtnClick(object sender, RoutedEventArgs args) => this.Hide();
    }
}
