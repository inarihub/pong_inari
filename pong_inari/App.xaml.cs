using pong_inari.engine;
using pong_inari.xaudio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace pong_inari
{
    public partial class App : Application
    {
        public GameAudio BackgroundAudio { get; private set; }
        private App()
        {
            BackgroundAudio = GameAudio.GetAudio("background", 0.5f);
            this.Startup += BGSoundStartHandleAsync;
            this.Exit += BGSoundStopHandleAsync;
        }

        private void BGSoundStartHandleAsync(object? sender, StartupEventArgs e)
        {
            Task.Run(() => BackgroundAudio.PlayAsync());
        }
        private void BGSoundStopHandleAsync(object sender, ExitEventArgs e)
        {
            BackgroundAudio.StopAsync().GetAwaiter();
        }
    }
    public static class AppExtensionMethods
    {
        public static void SetVisible(this UIElement element)
        {
            if (element.Visibility != Visibility.Visible)
            {
                element.Visibility = Visibility.Visible;
            }
        }
        public static void SetHidden(this UIElement element)
        {
            if (element.Visibility != Visibility.Hidden)
            {
                element.Visibility = Visibility.Hidden;
            }
        }
    }
}
