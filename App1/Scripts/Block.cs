using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace App1.Scripts
{
    public class Block
    {
        private string _link;
        private Grid _grid;
        private Image _image;
        private TimeSpan _timeTaken;
        private TextBlock _timeTextBlock;
        private bool _isAvailable;
        private Button _closeButton;

        public string Link { get { return _link; } }
        public Grid BlockGrid { get { return _grid; } }
        public Image BlockImage { get { return _image; } }
        public TimeSpan TimeTaken { get { return _timeTaken; } set { _timeTaken = value; } }
        public TextBlock TimeTextBlock { get { return _timeTextBlock; } set { _timeTextBlock = value; } }
        public bool IsAvailable { get { return _isAvailable; } set { _isAvailable = value; } }
        public Button CloseButton { get { return _closeButton; } set { _closeButton = value; } }


        public Block(string link)
        {
            _link = link;
        }
        //Tuple
        public (Grid grid, Image img) initBlock()
        {
            Grid grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition());
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            TextBlock textBlock = new TextBlock
            {
                Text = _link,
                FontSize = 14,
                Foreground = new SolidColorBrush("#1da1f2".GetColorFromHex()),
                Style = (Style)Application.Current.Resources["HeaderTextBlockStyle"],
            };

            Grid.SetColumn(textBlock, 1);
            grid.Children.Add(textBlock);

            Image img = new Image
            {
                Source = new BitmapImage(new Uri("ms-appx:///Assets/loading.gif")),
                Width = 20,
                Margin = new Thickness(0)
            };

            Grid.SetColumn(img, 2);
            grid.Children.Add(img);

            TextBlock timeBlock = new TextBlock
            {
                Text = "00:00",
                FontSize = 14,
                Padding = new Thickness(10, 0, 0, 0),
                Foreground = new SolidColorBrush("#1da1f2".GetColorFromHex()),
                Style = (Style)Application.Current.Resources["HeaderTextBlockStyle"],
            };

            _timeTextBlock = timeBlock;

            Grid.SetColumn(timeBlock, 3);
            grid.Children.Add(timeBlock);

            Button closeBtn = new Button
            {
                Content = new Image
                {
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/close.png")),
                    Width = 20,
                    Margin = new Thickness(0)
                },
                Margin = new Thickness(0),
                Padding = new Thickness(0)
                
            };
            
            Grid.SetColumn(closeBtn, 4);
            grid.Children.Add(closeBtn);

            this._closeButton = closeBtn;
            this._grid = grid;
            this._image = img;

            return (grid, img);
        }

    }
}
