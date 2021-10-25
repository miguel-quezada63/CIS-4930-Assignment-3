using System;
using System.Collections.Generic;
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
using TodoListApp.Dialogs;
using TodoListApp.ViewModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TodoListApp
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();
        }
        private async void AddNew_Click(object sender, RoutedEventArgs e)
        {
            var diag = new TodoItemDialog((DataContext as MainViewModel)?.TodoItems);
            await diag.ShowAsync();

        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel)?.Remove();
        }
        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel)?.SortList();
        }
        private async void Edit_Click(object sender, RoutedEventArgs e)
        {
            await (DataContext as MainViewModel)?.EditTicket();
        }
        private void Search(object sender, TextChangedEventArgs e)
        {
            (DataContext as MainViewModel)?.RefreshList();
        }
    }
}
