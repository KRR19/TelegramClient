using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegrammClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<TelegramUser> Users;
        TelegramBotClient bot;
        public MainWindow()
        {
            InitializeComponent();

            Users = new ObservableCollection<TelegramUser>();
            userList.ItemsSource = Users;

            string token = System.IO.File.ReadAllText("token");
            bot = new TelegramBotClient(token);

            bot.OnMessage += delegate (object sender, Telegram.Bot.Args.MessageEventArgs e)
            {
                string msg = $"{DateTime.Now}: {e.Message.Chat.FirstName}, {e.Message.Chat.Id}, {e.Message.Text}";
                System.IO.File.AppendAllText("data.log", $"{msg}\n");
                Debug.WriteLine(msg);

                this.Dispatcher.Invoke(() =>
                {
                    var persone = new TelegramUser(e.Message.Chat.FirstName, e.Message.Chat.Id);
                    if (!Users.Contains(persone)) Users.Add(persone);
                    Users[Users.IndexOf(persone)].AddMessage($"{persone.Nick}: {e.Message.Text}");
                });
            };
                bot.StartReceiving();

                btnSendMsg.Click += delegate { SendMsg(); };
                txtBxSendMsg.KeyDown += (s, e) => { if (e.Key == Key.Return) { SendMsg(); } };
            
        }

        public void SendMsg()
        {
            var currentUser = Users[Users.IndexOf(userList.SelectedItem as TelegramUser)];
            string responseMsg = $"Support: {txtBxSendMsg.Text}";
            currentUser.Message.Add(responseMsg);

            bot.SendTextMessageAsync(currentUser.Id, txtBxSendMsg.Text);
            string logText = $"{ DateTime.Now}: >> {currentUser.Id} {currentUser.Nick} {responseMsg}\n ";
            System.IO.File.AppendAllText("data.log", logText);

            txtBxSendMsg.Text = string.Empty;
        }

    }
}
