using MVVM.Base.ViewModel;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVM.Base
{
    public class AsyncCommand : Command, INotifyPropertyChanged
    {
        public static Dictionary<int, AsyncCommand> ActiveCommands = new Dictionary<int, AsyncCommand>();

        private Message _lastMessage;
        private bool _IsRunning;

        public AsyncCommand(Func<Task> function, bool singleInstance = true)
        {
            Function = function;
            SingleInstance = singleInstance;

            Tools.DispatcherExecute(() => { Messages = new SyncObservableCollection<Message>(); });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Func<Task> Function { get; }

        public bool KeepMessages { get; set; }

        public bool IsRunning
        {
            get => _IsRunning; set
            {
                _IsRunning = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
            }
        }


        public Message LastMessage
        {
            get => _lastMessage; set
            {
                _lastMessage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LastMessage)));
            }
        }

        public SyncObservableCollection<Message> Messages { get; private set; }

        public bool SingleInstance { get; }

        public int? TaskID { get; private set; }

        public bool ThrowsError { get; set; }

        public static async void PostMessage(string message)
        {
            if (Task.CurrentId == null)
                return;

            if (ActiveCommands.TryGetValue(Task.CurrentId.Value, out var index))
            {
                index.Messages.Add(index.LastMessage = new Message(message));
            }
        }
        public override bool CanExecute(object parameter)
        {
            return !IsRunning;
        }
        public override void Execute(object parameter)
        {
            var task = Task.Run(() => { ActiveCommands.Add((TaskID = Task.CurrentId).Value, this); Function(); }, CancellationToken.None);
            if (SingleInstance)
            {
                IsRunning = true;
                task.GetAwaiter().OnCompleted(() =>
                {
                    TaskID = null;
                    IsRunning = false;
                    CommandManager.InvalidateRequerySuggested();

                    ActiveCommands.Remove(TaskID.Value);

                    if (!KeepMessages)
                    {
                        Messages.Clear();
                        LastMessage = null;
                    }
                    if (ThrowsError && task.IsFaulted)
                        throw task.Exception;
                });
            }
        }
    }
}