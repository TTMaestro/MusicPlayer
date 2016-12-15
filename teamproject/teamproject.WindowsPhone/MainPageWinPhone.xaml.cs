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

namespace teamproject
{
   
    public sealed partial class MainPage : Page
    {

        private List<string> _scope = new List<string> { VKScope.AUDIO };

        public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            VKSDK.Initialize("5765082");
            VKSDK.Authorize(_scope, false, false);
            
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        void Image_Loaded(string Icon)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(ImagePlay.BaseUri, Icon);
            ImagePlay.Source = bitmapImage;
        }

        private void PlayTrack(string tempur1)
        {
            Player.Source = new Uri(GetTrueUrl(tempur1));
            Player.Play();
        }

        public string GetTrueUrl(string InputString)
        {
            return InputString.Substring(0, InputString.IndexOf('?'));
        }

        private void TextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        { 
            PlayTrack((sender as TextBlock).Tag.ToString());
            Image_Loaded(@"images/pause.png");
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

            if (audioView.SelectedIndex != 0 && audioView.Items.Count != 0)
            {
                PlayTrack((audioView.Items[Url_Validation(--audioView.SelectedIndex)] as VKAudio).url);
            }
            

        }

        private void ImageNext_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(audioView.SelectedIndex != audioView.Items.Count && audioView.Items.Count != 0)
            {
                PlayTrack((audioView.Items[Url_Validation(++audioView.SelectedIndex)] as VKAudio).url);
            }
        }

        private void ImagePlay_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (audioView.Items.Count != 0)
            {
                if (Player.CurrentState == MediaElementState.Playing)
                {
                    Player.Pause();
                    Image_Loaded(@"images/play.png");
                }
                else
                {
                    Player.Play();
                    Image_Loaded(@"images/pause.png");
                }
            }
           
        }
        private void Player_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (audioView.Items.Count != 0)
            {
                PlayTrack((audioView.Items[Url_Validation(++audioView.SelectedIndex)] as VKAudio).url);
            }
                
        }
    }
}
