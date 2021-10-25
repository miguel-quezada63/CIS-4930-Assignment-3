using Library.TodoListApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Library.TodoListApp.util;
using TodoListApp.ViewModels;
using TodoTask = Library.TodoListApp.Models.TodoTask;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace TodoListApp.Dialogs
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TodoItemDialog : ContentDialog
    {
        private readonly ObservableCollection<TodoItem> todoItems;
        public TodoItemDialog(ObservableCollection<TodoItem> todoItems)
        {
            this.InitializeComponent();
            DataContext = new DialogViewModel();
            this.todoItems = todoItems;
        }
        public TodoItemDialog(ObservableCollection<TodoItem> todoItems, TodoItem item)
        {
            InitializeComponent();
            DataContext = new DialogViewModel(item);
            this.todoItems= todoItems;
        }
        private void Dialog_OkayBtnClick(object sender, RoutedEventArgs args)
        {
            var dc = DataContext as DialogViewModel;
            var itemToEdit = dc?.NewItem;
            var i = todoItems.IndexOf(itemToEdit);
            if (i >= 0)
            {
                todoItems.Remove(itemToEdit);
                todoItems.Insert(i, itemToEdit);
            }else
                todoItems.Add(dc?.NewItem);
            JsonData.Save(todoItems);
            this.Hide();
        }
        private void Dialog_UpdateDeadline(object sender, DatePickerValueChangedEventArgs e)
        {
            if (sender is DatePicker dp && DataContext is DialogViewModel dc && dc.NewItem is TodoTask newItem)
                newItem.Deadline = e.NewDate.DateTime;
        }
        private void Dialog_UpdateStart(object sender, DatePickerValueChangedEventArgs e)
        {
            if (sender is DatePicker dp && DataContext is DialogViewModel dc && dc.NewItem is TodoAppointment newItem)
                newItem.Start = e.NewDate.DateTime;
        }
        private void Dialog_UpdateStop(object sender, DatePickerValueChangedEventArgs e)
        {
            if (sender is DatePicker dp && DataContext is DialogViewModel dc && dc.NewItem is TodoAppointment newItem)
                newItem.Stop = e.NewDate.DateTime;
        }
        private void Dialog_OnAttendeesChange(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox tb && DataContext is DialogViewModel dc && dc.NewItem is TodoAppointment newItem)
                newItem.Attendees = tb.Text.Split(",").ToList();
        }
        private void Dialog_CancelBtnClick(object sender, RoutedEventArgs args)
        {
            this.Hide();
        }
    }
}
