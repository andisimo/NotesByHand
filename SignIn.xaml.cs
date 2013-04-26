using Microsoft.Xrm.Sdk.Client;
using NotesByHand.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace NotesByHand
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SignIn : Page
    {
        public SignIn()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // TODO -remove this
            usernameBox.Text = "anschult@psecdemo.onmicrosoft.com";
            passwordBox.Password = "CrmD3mo!";

            usernameBox.IsEnabled = false;
            passwordBox.IsEnabled = false;
            var button = sender as Button;
            button.IsEnabled = false; 

            AuthenticationCredentials creds = new AuthenticationCredentials();
            creds.ClientCredentials.UserName.UserName = usernameBox.Text;
            creds.ClientCredentials.UserName.Password = passwordBox.Password;

            CrmConnector.Connected += GetContacts;
            CrmConnector.Connect(creds);
        }

        private void GetContacts()
        {
            App.ContactsNotesVM.LoadContactsAsync();
            // TODO - change this from an event handler to observable properties
            App.ContactsNotesVM.Loaded += DataLoaded;
        }

        private async void DataLoaded()
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                this.Frame.Navigate(typeof(BasicPage1));
            });
        }
    }
}
