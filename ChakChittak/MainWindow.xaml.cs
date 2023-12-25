using ChakChittak.UserControls;
using Chat.Client;
using ChatCore;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChakChittak
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private HubConnection? _hubConnection;
        //public readonly string server = "http://localhost:5004/mychat";
        //private string UserName = string.Empty;
        //private bool Disconnected = false;
        public List<Users> OnlineUsers { get; set; } = new();
        public List<Message> Messages { get; set; } = new();
        public bool StartedChat { get; set; }
        public string? CurrentMessage { get; set; }
        public string? CurrentUsername { get; set; }
        public Users? CurrentUser { get; set; }
        public string? Error { get; set; }

        private readonly ChatClient _client;

        public MainWindow() 
        { 
            InitializeComponent();
            _client = new ChatClient("http://localhost:5004/mychat");
            tbUserName.Focus();

            _client.OnMessageReceived += (s, message) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Messages.Add(message);
                    MessageUserControl messageUserControl = new MessageUserControl();
                    messageUserControl.SetData(message);
                    if(message.Sender.Name == CurrentUsername)
                    {
                        messageUserControl.HorizontalAlignment = HorizontalAlignment.Right;
                    }
                    spMessages.Children.Add(messageUserControl);
                });

                //Messages.Add(message);
                //MessageUserControl messageUserControl = new MessageUserControl();
                //messageUserControl.SetData(message);
                //spMessages.Children.Add(messageUserControl);
            };

        }
        public async Task StartChatAsync()
        {
            if (string.IsNullOrWhiteSpace(CurrentUsername))
            {
                Error = "Please enter your name";
                return;
            }

            StartedChat = true;
            CurrentUser = new Users(CurrentUsername);

            await _client.StartAsync(CurrentUser);
            //OnlineUsers = (await _client.GetOnlineUsersAsync()).ToList();
        }
        public void RefreshAsync()
        {

        }
        public async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(CurrentMessage))
            {
                //Error = "Please enter a message";
                MessageBox.Show("Please enter the message ");
                return;
            }

            await _client.SendMessageAsync(CurrentMessage);
            CurrentMessage = string.Empty;
        }

        private async void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            CurrentUsername=tbUserName.Text;
            if (string.IsNullOrEmpty(CurrentUsername)==false)
            {
                await StartChatAsync();
                tbUserName.Visibility = Visibility.Collapsed;
                btnLogin.Visibility = Visibility.Collapsed;
                brMessage.Visibility  = Visibility.Visible;
                tbMessage.Focus();
            }
        }

        private void tbMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private async void pkdEnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CurrentMessage = tbMessage.Text;
                await SendMessageAsync();
                tbMessage.Text = "";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(this.Messages.Count > 0)
            {
                foreach (var message in this.Messages)
                {
                    MessageUserControl messageUserControl = new MessageUserControl();
                    messageUserControl.SetData(message);
                    spMessages.Children.Add(messageUserControl);

                }
            }
           
        }
    }
}
