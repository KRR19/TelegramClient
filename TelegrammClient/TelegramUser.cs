using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace TelegrammClient
{
    public class TelegramUser : INotifyPropertyChanged, IEquatable<TelegramUser>
    {
        private string nick;
        private long id;
        public string Nick { 
            get
            {
                return this.nick;
            }
            set 
            {
                this.nick = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.nick)));
            }
        }
        public long Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.id)));
            }
        }
        public ObservableCollection<string> Message { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public TelegramUser(string nick, long id)
        {
            this.Id = id;
            this.Nick = nick;
            this.Message = new ObservableCollection<string>();
        }

        public bool Equals(TelegramUser other) => other.Id == this.Id;
        public void AddMessage(string Text) => Message.Add(Text);
    }
}
