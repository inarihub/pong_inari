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
using Cfg = pong_inari.GameConfig;

namespace pong_inari
{
    public partial class App : Application
    {
        public static int GlobalVolumePercent { get; set; }
        public GameAudio BackgroundAudio { get; private set; }
        private App()
        {
            GlobalVolumePercent = 50;
            BackgroundAudio = GameAudio.GetAudio("background", true);
            this.Startup += BGSoundStartHandleAsync;
            this.Exit += BGSoundStopHandleAsync;
        }
        private async void BGSoundStartHandleAsync(object? sender, StartupEventArgs e)
        {
            await Task.Run(() => BackgroundAudio.PlayAsync());
        }
        private async void BGSoundStopHandleAsync(object sender, ExitEventArgs e)
        {
            await Task.Run(() => BackgroundAudio.StopAsync());
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
