using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Library.TodoListApp.Models;


namespace TodoListApp.ViewModels
{
    public class DialogViewModel : INotifyPropertyChanged
    {
        private DateTimeOffset boundStart;
        public DateTimeOffset BoundStart
        {
            get
            {
                if (NewItem is TodoAppointment newItem)
                    return newItem.Start;
                return boundStart;
            }
            set
            {
                boundStart = value;
                if (NewItem is TodoAppointment newItem)
                    newItem.Start = boundStart.Date;
            }
        }
        private DateTimeOffset boundStop;
        public DateTimeOffset BoundStop
        {
            get
            {
                if (NewItem is TodoAppointment newItem)
                    return newItem.Stop;
                return boundStop;
            }
            set
            {
                boundStop = value;
                if (NewItem is TodoAppointment newItem)
                    newItem.Stop = boundStop.Date;
            }
        }
        private DateTimeOffset boundDeadline;
        public DateTimeOffset BoundDeadline
        {
            get
            {
                if (NewItem is TodoTask newItem)
                    return newItem.Deadline;
                return boundDeadline;
            }
            set
            {
                boundDeadline = value;
                if(NewItem is TodoTask newItem)
                    newItem.Deadline = boundDeadline.Date;
            }
        }
        private string attendeesString;
        public string AttendeesString
        {
            get
            {
                if (NewItem is TodoAppointment newItem)
                    return String.Join(",", newItem.Attendees);
                return attendeesString;
            }
            set
            {
                if (NewItem is TodoAppointment newItem)
                    newItem.Attendees = value.Split(",").ToList();
                attendeesString = value;
            }
        }

        public Visibility ShowTask
        {
            get => NewItem is TodoTask ? Visibility.Visible : Visibility.Collapsed;
        }
        public Visibility ShowAppointment
        {
            get => NewItem is TodoAppointment ? Visibility.Visible : Visibility.Collapsed;
        }

        private bool showCheckBox;
        public Visibility ShowCheckBox
        {
            get => showCheckBox ? Visibility.Visible : Visibility.Collapsed;
        }
        public TodoItem NewItem { get; set; }
        private string itemType = "Task";
        private readonly bool isAppointment = false;
        public bool IsAppointment
        {
            get => isAppointment;
            set
            {
                if (value)
                    NewItem = new TodoAppointment();
                else
                    NewItem = new TodoTask();
                NotifyPropertyChanged();
                NotifyPropertyChanged("NewItem");
                NotifyPropertyChanged("ShowAppointment");
                NotifyPropertyChanged("ShowTask");
            }
        }
        public string ItemType
        {
            get => itemType;
            set
            {
                if (value != null && !value.Equals(itemType, StringComparison.InvariantCultureIgnoreCase))
                {
                    itemType = value;
                    if (value.Equals("Task", StringComparison.InvariantCultureIgnoreCase))
                        NewItem = new TodoTask();
                    else if (value.Equals("Appointment", StringComparison.InvariantCultureIgnoreCase))
                        NewItem = new TodoAppointment();
                    else
                        NewItem = new TodoItem();
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("NewItem");
                }
            }
        }
        public DialogViewModel()
        {
            NewItem = new TodoTask();
            showCheckBox = true;
            BoundDeadline = DateTime.Now;
        }

        public DialogViewModel(TodoItem item)
        {
            NewItem = item;
            showCheckBox = false;
            if (NewItem is TodoTask) isAppointment = false;
            else isAppointment= true;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
