using Syncfusion.UI.Xaml.Chat;
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
using OpenTelemetry.ViewModel;

namespace OpenTelemetry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Chat_PromptRequest(
    object sender,
    PromptRequestEventArgs e)
        {
            e.Handled = true;

            var vm = Chat.DataContext as ConversationHistoryViewModel;

            ITextMessage message = e.InputMessage as ITextMessage;

            vm.Chats.Add(new TextMessage
            {
                Text = message.Text,
                DateTime = DateTime.Now,
                Author = Chat.CurrentUser
            });

            await vm.SendUserMessageAsync(message.Text);
        }
    }
}
