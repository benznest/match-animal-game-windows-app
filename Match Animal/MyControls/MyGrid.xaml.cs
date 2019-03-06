using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Match_Animal.MyControls
{
    public sealed partial class MyGrid : UserControl
    {
        MyBox box;

        public MyBox Box
        {
            get
            {
                return box;
            }

            set
            {
                box = value;
            }
        }

        public MyGrid()
        {
            this.InitializeComponent();
        }

        public void setImage(BitmapImage bitmap)
        {
            my_image.Source = bitmap;
        }

        public void setEventClick(RoutedEventHandler ev)
        {
            btn.Click += ev;
        }

        public Button getButton()
        {
            return btn;
        }

        public Button setButton(Button b)
        {
            return btn = b;
        }

        public void setSelected(bool selected)
        {
            if (selected)
            {
                btn.Background = new SolidColorBrush(Color.FromArgb(255, 238, 51, 55));
            }else
            {
                btn.Background = new SolidColorBrush(Color.FromArgb(255,255,208,219));
            }
        }

        public void setSize(int w,int h)
        {
            my_grid.Width = w;
            my_grid.Height = h;

            container.Width = w;
            container.Height = h;
        }
    }
}
