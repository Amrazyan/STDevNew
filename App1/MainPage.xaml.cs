using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Data.SQLite;
using App1.Scripts;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //SortedSet<Block> dataCollection = new SortedSet<Block>(new SortListByName());  //DIDNT USE THIS BECAUSE IT WAS NECESSARY TO CHOOSE SORTING ALGORITHM AT RUNTIME
        private List<Block> dataCollectionList = new List<Block>();
        private bool nameSortOrder = true;
        private bool timeSortOrder = true;
        private bool availabilitySortOrder = true;


        public MainPage()
        {
            this.InitializeComponent();
        }

        private void onSubmitClick(object sender, RoutedEventArgs e)
        {

            addListItems(inputBox.Text);

        }

        private void sortByName(object sender, RoutedEventArgs e)
        {
            nameSortOrder = !nameSortOrder;
            sortList(nameSortOrder, new SortinListAlgorithms.SortListByName());
        }

        private void sortByTime(object sender, RoutedEventArgs e)
        {
            timeSortOrder = !timeSortOrder;
            sortList(timeSortOrder, new SortinListAlgorithms.SortListByTime());
        }

        private void sortByAvailability(object sender, RoutedEventArgs e)
        {
            availabilitySortOrder = !availabilitySortOrder;
            sortList(availabilitySortOrder, new SortinListAlgorithms.SortListByAvailability());
        }

        private void searchAll(object sender, RoutedEventArgs e)
        {
            checkAllAgain();
        }


        private void checkAllAgain()
        {
            List<Block> localDataCollectionList = new List<Block>(dataCollectionList);
            dataCollectionList.Clear();
            listView.Items.Clear();

            foreach (var item in localDataCollectionList)
            {
                addListItems(item.Link);
            }

        }
        private void addListItems(string link)
        {
            Block block = new Block(link);

            var data = block.initBlock();

            dataCollectionList.Add(block);/////////////////////////////////////////////////////////////////////

            listView.Items.Add(data.grid);

            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

            timer.Start();

            MakeRequest.UrlIsValid(link, () =>
            {
                timer.Stop();
                block.TimeTaken = timer.Elapsed;
                block.IsAvailable = true;
                block.CloseButton.Click += delegate (object sender, RoutedEventArgs e) { CloseButton_Click(sender, e, block); };
                ExpandingMethods.changeUiFromAnotherThread(() => infoMessageBox.Text = "Site is valid");
                ExpandingMethods.changeUiFromAnotherThread(() => block.TimeTextBlock.Text = block.TimeTaken.TotalSeconds.ToString("0.000"));
                ExpandingMethods.changeUiFromAnotherThread(() => block.BlockImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/success.png")));
            },
            () =>
            {
                timer.Stop();
                block.TimeTaken = timer.Elapsed;
                block.IsAvailable = false;
                block.CloseButton.Click += delegate (object sender, RoutedEventArgs e) { CloseButton_Click(sender, e, block); };
                ExpandingMethods.changeUiFromAnotherThread(() => infoMessageBox.Text = "Site is NOT valid");
                ExpandingMethods.changeUiFromAnotherThread(() => block.TimeTextBlock.Text = block.TimeTaken.TotalSeconds.ToString("0.000"));
                ExpandingMethods.changeUiFromAnotherThread(() => block.BlockImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/failed.png")));

            });
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e, Block myBlock)
        {

            dataCollectionList.Remove(myBlock);
            listView.Items.Clear();
            foreach (var item in dataCollectionList)
            {
                listView.Items.Add(item.BlockGrid);
            }

        }

        private void sortList(bool isAscending, SortinListAlgorithms.SortList sortListAlgorithm)
        {
            listView.Items.Clear();

            dataCollectionList.Sort(sortListAlgorithm);

            if (!isAscending)
            {
                dataCollectionList.Reverse();
            }

            foreach (var item in dataCollectionList)
            {
                listView.Items.Add(item.BlockGrid);
            }

        }

        private void onInsertToDb(object sender, RoutedEventArgs e)
        {
            Database db = new Database();
            db.createDB();
        }

    }
}
