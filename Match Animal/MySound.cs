using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Match_Animal
{
    class MySound
    {
        public static string IMPACT = "impact.wav";
        public static string SELECT = "select.wav";
        public static string CANCEL = "beat.wav";

        public static async void play(string sound)
        {
            var element = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Sounds");
            var file = await folder.GetFileAsync(sound);
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            element.SetSource(stream, "");
            element.Play();
        }
    }
}
