using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VK.WindowsPhone.SDK;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using VK.WindowsPhone.SDK.API;
using VK.WindowsPhone.SDK.API.Model;
using Windows.UI.Xaml.Media.Imaging;
using System.Text;

namespace teamproject
{
   
    public sealed partial class MainPage : Page
    {
        private List<string> _scope = new List<string> { VKScope.AUDIO, VKScope.STATUS};
        public MainPage()
        {
            VKSDK.Initialize("5765082");
            VKSDK.Authorize(_scope, false, false);
            
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }
        public string GetTrueUrl(string InputString)
        {
            return InputString.Substring(0, InputString.IndexOf('?'));

        }
        private int Url_Validation(int index)
        {
            index = audioView.SelectedIndex;
            if ((audioView.Items[audioView.SelectedIndex] as VKAudio).url == null)
            {
                while ((audioView.Items[audioView.SelectedIndex] as VKAudio).url != null)
                {
                    index += 1;             
                }
                return index;
            }
            return index;
        }
        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        { 
            PlayTrack((sender as TextBlock).Tag.ToString());
        }

        private void textRequest_TextChanged(object sender, TextChangedEventArgs e)
        {
            VKRequest.Dispatch<VKList<VKAudio>>(new VKRequestParameters(
               "audio.search",
               "q",
               textRequest.Text),
               (result) =>
               {
                   audioView.ItemsSource = result.Data.items;
               }
           );
        }
        
        private void ImagePrevious_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PlayTrack((audioView.Items[Url_Validation(--audioView.SelectedIndex)] as VKAudio).url);

        }

        private void PlayTrack(string tempur1)
        {
            Player.Source = new Uri(GetTrueUrl(tempur1));
            Player.Play();
        }

        private void ImageNext_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PlayTrack((audioView.Items[Url_Validation(audioView.SelectedIndex)] as VKAudio).url);
        }


        private void ImagePlay_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(Player.CurrentState == MediaElementState.Playing)
            {
                Player.Pause();
                Image_Loaded(@"images/player_next.png");
            }
            else
            {
                Player.Play();
                Image_Loaded(@"images/player_pause.png");
            }
        }
        void Image_Loaded( string Icon)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(ImagePlay.BaseUri, Icon);
            ImagePlay.Source = bitmapImage;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void Player_MediaEnded(object sender, RoutedEventArgs e)
        {
            if ((audioView.Items[++audioView.SelectedIndex] as VKAudio).url != null)
            {
                PlayTrack((audioView.Items[++audioView.SelectedIndex] as VKAudio).url);
            }
            else
            {
                int next = ++audioView.SelectedIndex;
                PlayTrack((audioView.Items[++next] as VKAudio).url);
            }
        }
    }
}
