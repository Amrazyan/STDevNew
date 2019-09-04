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
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;

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
        private Action sortingAlgorithm;

        public MainPage()
        {
            this.InitializeComponent();
            initValues();
        }

        private void initValues()
        {
            List<UrlDataModel> urls = Database.Instance.getAllData();
            foreach (var item in urls)
            {
                Block block = createBlock(item.Url,(Block.isReachableEnum)item.IsReachable,item.ResponseTime);
                block.Id = item.Id;
                block.IsAvailable = Convert.ToBoolean( item.IsReachable );
                dataCollectionList.Add(block);
                listView.Items.Add(block.BlockGrid);
            }
        }
        private void onSubmitClick(object sender, RoutedEventArgs e)
        {
            addItemsToList(inputBox.Text);
        }

        private void sortByName(object sender, RoutedEventArgs e)
        {
            nameSortOrder = !nameSortOrder;
            sortingAlgorithm = () => sortList (nameSortOrder, new SortinListAlgorithms.SortListByName());
            sortingAlgorithm?.Invoke();
        }

        private void sortByTime(object sender, RoutedEventArgs e)
        {
            timeSortOrder = !timeSortOrder;
            sortingAlgorithm = () => sortList(timeSortOrder, new SortinListAlgorithms.SortListByTime());
            sortingAlgorithm?.Invoke();
        }

        private void sortByAvailability(object sender, RoutedEventArgs e)
        {
            availabilitySortOrder = !availabilitySortOrder;

            sortingAlgorithm = () => sortList(availabilitySortOrder, new SortinListAlgorithms.SortListByAvailability());
            sortingAlgorithm?.Invoke();
        }

        private void searchAll(object sender, RoutedEventArgs e)
        {
            checkAll();
        }


        private async void CloseButton_Click(object sender, RoutedEventArgs e, Block myBlock)
        {
            if (myBlock.TimeTaken == 0)
            {
                ContentDialog popUp = new ContentDialog
                {
                    Title = "Wait",
                    Content = "Please wait untill process was finished",
                    PrimaryButtonText = "Ok"
                };
                await popUp.ShowAsync();
            }
            else
            {
                infoMessageBox.Text = myBlock.Id.ToString();

                Database.Instance.deleteById(myBlock.Id);

                dataCollectionList.Remove(myBlock);
                listView.Items.Clear();
                foreach (var item in dataCollectionList)
                {
                    listView.Items.Add(item.BlockGrid);
                }
            }

        }

        private void checkAll()
        {
            listView.Items.Clear();

            foreach (var item in dataCollectionList)
            {
                checkValues(item);
            }
            sortingAlgorithm?.Invoke();

        }
        private void checkValues(Block block)
        {
    
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

            timer.Start();

            block.BlockImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/loading.gif"));
            block.TimeTaken = 0;

            listView.Items.Add(block.BlockGrid);
            checkUrlAvailability(block);

        }
        private void addItemsToList(string link)
        {

            Block block = createBlock(link, Block.isReachableEnum.DoesntCheckedYet,0);

            if (dataCollectionList.Contains(block))
            {
                infoMessageBox.Text = "Same value " + link;
                return;
            }

            dataCollectionList.Add(block);

            listView.Items.Add(block.BlockGrid);

            checkUrlAvailability(block, () => Database.Instance.insertIntoDB(block,true), () => Database.Instance.insertIntoDB(block, false));
            
        }
        private Block createBlock(string link,Block.isReachableEnum isReachable,float responseTime)
        {
            Block block = new Block(link,isReachable, responseTime);
            block.CloseButton.Click += delegate (object sender, RoutedEventArgs e) { CloseButton_Click(sender, e, block); };

            return block;

        }
        private void checkUrlAvailability(Block block,Action onSuccess = null,Action onFailed = null)
        {
            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

            timer.Start();

            MakeRequest.UrlIsValid(block.URL, () =>
            {
                timer.Stop();
                block.TimeTaken = (float)timer.Elapsed.TotalSeconds;
                block.IsAvailable = true;
                ExpandingMethods.changeUiFromAnotherThread(() => infoMessageBox.Text = "Site is valid");
                ExpandingMethods.changeUiFromAnotherThread(() => block.TimeTextBlock.Text = block.TimeTaken.ToString("0.000"));
                ExpandingMethods.changeUiFromAnotherThread(() => block.BlockImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/success.png")));
                onSuccess?.Invoke();
            },
           () =>
           {
               timer.Stop();
               block.TimeTaken = (float)timer.Elapsed.TotalSeconds;
               block.IsAvailable = false;
               ExpandingMethods.changeUiFromAnotherThread(() => infoMessageBox.Text = "Site is NOT valid");
               ExpandingMethods.changeUiFromAnotherThread(() => block.TimeTextBlock.Text = block.TimeTaken.ToString("0.000"));
               ExpandingMethods.changeUiFromAnotherThread(() => block.BlockImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/failed.png")));
               onFailed?.Invoke();

           });

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

        private void onSearchUrl(object sender, RoutedEventArgs e)
        {
            string chromePath = @"C:\ProgramFiles(x86)\Google\Chrome\Application\chrome.exe";
            string url = "http://stackoverflow.com";

            string browserPath = GetSystemDefaultBrowser();
            Process.Start(chromePath, url);
        }

        internal string GetSystemDefaultBrowser()
        {
            string name = string.Empty;
            RegistryKey regKey = null;

            try
            {
                //set the registry key we want to open
                regKey = Registry.ClassesRoot.OpenSubKey("HTTP\\shell\\open\\command", false);

                //get rid of the enclosing quotes
                name = regKey.GetValue(null).ToString().ToLower().Replace("" + (char)34, "");

                //check to see if the value ends with .exe (this way we can remove any command line arguments)
                if (!name.EndsWith("exe"))
                    //get rid of all command line arguments (anything after the .exe must go)
                    name = name.Substring(0, name.LastIndexOf(".exe") + 4);

            }
            catch (Exception ex)
            {
                name = string.Format("ERROR: An exception of type: {0} occurred in method: {1} in the following module: {2}", ex.GetType(), ex.TargetSite, this.GetType());
            }
            finally
            {
                //check and see if the key is still open, if so
                //then close it
                if (regKey != null)
                    regKey.Close();
            }
            //return the value
            return name;

        }
    }
}
