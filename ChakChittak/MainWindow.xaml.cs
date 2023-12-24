using Microsoft.AspNetCore.SignalR.Client;
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
        private HubConnection? _hubConnection;
        public readonly string server = "http://localhost:5004/mychat";
        private string UserName = string.Empty;
        private bool Disconnected = false;
        public MainWindow() 
        { 
            InitializeComponent();
            this._hubConnection = new HubConnectionBuilder()
                .WithUrl(server)
                .Build();

            UserName = user.Text.ToString();

            _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Dispatcher.Invoke(() =>
                {
                    mychat.Items.Add($"{user}: {message}\n");
                });
            });
        }

        private void log_in(object sender, RoutedEventArgs e)
        {
            UserName = user.Text.ToString();
            StartConnection();

        }
    }
}
