using Match_Animal.MyControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System.Threading;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Match_Animal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int gravity = GameGravity.GRAVITY_NONE;

        Random rnd = new Random();
        const int ROW = 8;
        const int COL = 16;

        const int TOTAL = 128; //16*8
        const int COUNT_IMAGE = 13;
        const int TOTAL_IMAGE = 36;
        int grid_alive = 128;

        List<MyBox> listBox = new List<MyBox>();
        MyBox[,] mapBox;
        MyGrid[,] mapGrid;

        int currentIdSelected = 0;
        MyGrid gridFirstSelected;

        private BitmapImage[] bitmap;

        private DispatcherTimer dispatcherTimer;
        DateTimeOffset startTime;
        DateTimeOffset lastTime;
        DateTimeOffset stopTime;
        int timesTicked = 1;
        int timesToTick = 180;

        public MainPage()
        {
            this.InitializeComponent();
            setWindowsSize();
            initGame();
        }

        private void initGame()
        {
            currentIdSelected = 0;
            timesTicked = 1;
            grid_alive = 128;
            progress_bar.Value = 100;
            gridView_container.Items.Clear();
            initImage();

            gravity = GameGravity.randomGravity();

            listBox = generateOrder();
            //listBox = MyTest.generateMockup(ROW,COL);

            initGridView(listBox);
            DispatcherTimerSetup();
        }

        private void initImage()
        {
            List<int> listImageId = new List<int>();
            for (int i = 1; i <= TOTAL_IMAGE; i++)
            {
                listImageId.Add(i);
            }

            // random image
            for (int i = 0; i < 50; i++)
            {
                int random_index_a = rnd.Next(0, TOTAL_IMAGE - 1);
                int random_index_b = rnd.Next(0, TOTAL_IMAGE - 1);
                Swap(listImageId, random_index_a, random_index_b);
            }

            bitmap = new BitmapImage[COUNT_IMAGE];
            for (int i = 0; i < COUNT_IMAGE; i++)
            {
                int index_image = listImageId[i];
                bitmap[i] = new BitmapImage(new Uri(this.BaseUri, "/Assets/animal/" + index_image + ".png"));
            }
        }

        private List<MyBox> generateOrder()
        {
            int random_id = 0;
            listBox = new List<MyBox>();

            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {

                    MyBox box = new MyBox();
                    box.Alive = true;

                    if (j % 2 == 0)
                    {
                        random_id = rnd.Next(1, COUNT_IMAGE);
                        box.ImageId = random_id;
                    }
                    else
                    {
                        box.ImageId = random_id;
                    }
                    listBox.Add(box);
                }
            }

            // swap.
            for (int i = 0; i < 150; i++)
            {
                int random_a = rnd.Next(0, listBox.Count - 1);
                int random_b = rnd.Next(0, listBox.Count - 1);

                if (random_a == random_b)
                {
                    i--;
                    continue;
                }

                Swap(listBox, random_a, random_b);
            }

            return listBox;
        }

        private void initGridView(List<MyBox> listBox)
        {
            mapBox = new MyBox[ROW, COL];
            mapGrid = new MyGrid[ROW, COL];
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    MyBox box = listBox[(i * COL) + j];
                    box.Row = i;
                    box.Col = j;
                    mapBox[i, j] = box;

                    MyGrid grid = new MyGrid();
                    //grid.setSize(40, 40);
                    grid.Box = box;
                    grid.Box.Alive = box.Alive;
                    grid.setImage(bitmap[box.ImageId]);
                    mapGrid[i, j] = grid;

                    Button btn = grid.getButton();
                    btn.Tag = grid;
                    btn.Click += Btn_Click;

                    if (!box.Alive)
                    {
                        btn.Visibility = Visibility.Collapsed;
                    }

                    gridView_container.Items.Add(grid);
                }
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = ((Button)sender);
            MyGrid grid = (MyGrid)btn.Tag;
            MyBox box = grid.Box;

            int imageId = box.ImageId;
            if (!grid.Box.Alive)
            {
                MySound.play(MySound.CANCEL);
                currentIdSelected = 0;
                if (gridFirstSelected != null)
                {
                    gridFirstSelected.setSelected(false);
                }
                return;
            }

            if (currentIdSelected == 0)
            {
                MySound.play(MySound.SELECT);
                currentIdSelected = imageId;
                gridFirstSelected = grid;
                grid.setSelected(true);
            }
            else
            {
                if (!isSame(gridFirstSelected.Box, grid.Box))
                {
                    if (currentIdSelected == imageId)
                    {

                        List<Point> path = canKill(gridFirstSelected.Box, grid.Box);
                        if (path != null)
                        {
                            MySound.play(MySound.IMPACT);

                            // make hide.
                            btn.Visibility = Visibility.Collapsed;
                            gridFirstSelected.getButton().Visibility = Visibility.Collapsed;

                            // make dead.
                            grid.Box.Alive = false;
                            gridFirstSelected.Box.Alive = false;

                            currentIdSelected = 0;
                            gridFirstSelected.setSelected(false);

                            showLine(path);
                            GameGravity.moveTable(ROW,COL,gravity , gridView_container,ref mapBox, grid , gridFirstSelected);
                            
                            grid_alive -= 2;

                            System.Diagnostics.Debug.WriteLine("grid_alive = "+ grid_alive);
                            if (grid_alive <= 0)
                            {
                                dispatcherTimer.Stop();
                                win();
                            }
                        }
                        else
                        {
                            MySound.play(MySound.CANCEL);
                            currentIdSelected = 0;
                            if (gridFirstSelected != null)
                            {
                                gridFirstSelected.setSelected(false);
                            }
                        }
                    }
                    else
                    {
                        MySound.play(MySound.CANCEL);
                        currentIdSelected = 0;
                        if (gridFirstSelected != null)
                        {
                            gridFirstSelected.setSelected(false);
                        }
                    }
                }
            }
        }

        private bool isSame(MyBox g1, MyBox g2)
        {
            if (g1.Row == g2.Row && g1.Col == g2.Col)
            {
                return true;
            }
            return false;
        }

        private List<Point> canKill(MyBox b1, MyBox b2)
        {
            int row_a = b1.Row;
            int col_a = b1.Col;

            int row_b = b2.Row;
            int col_b = b2.Col;

            return Game.isWin(mapBox, row_a, col_a, row_b, col_b, ROW, COL);
        }

        private async void showLine(List<Point> path)
        {
            Polyline myPolyline = new Polyline();
            myPolyline.Stroke = new SolidColorBrush(Colors.Black);
            myPolyline.StrokeThickness = 3;
            myPolyline.FillRule = FillRule.EvenOdd;

        
            PointCollection myPointCollection = new PointCollection();
            foreach (Point p in path)
            {
                System.Diagnostics.Debug.WriteLine("X = " + p.X + " , Y = " + p.Y);
                myPointCollection.Add(p);
            }

            myPolyline.Points = myPointCollection;
            grid_container.Children.Add(myPolyline);

            await Task.Delay(TimeSpan.FromSeconds(0.3));
            grid_container.Children.Remove(myPolyline);
        }

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            setWindowsSize();
        }

        private static void setWindowsSize()
        {
            ApplicationView.PreferredLaunchViewSize = new Size(1000, 640);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
        }

        public static void Swap(List<MyBox> list, int indexA, int indexB)
        {
            MyBox tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        public static void Swap(List<int> list, int indexA, int indexB)
        {
            int tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        public void DispatcherTimerSetup()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            //IsEnabled defaults to false

            startTime = DateTimeOffset.Now;
            lastTime = startTime;

            dispatcherTimer.Start();
            //IsEnabled should now be true after calling start
        }

        void dispatcherTimer_Tick(object sender, object e)
        {
            DateTimeOffset time = DateTimeOffset.Now;
            TimeSpan span = time - lastTime;
            lastTime = time;

            int percent = (int)(((timesToTick - timesTicked)*(1.0) / timesToTick) * 100);
            progress_bar.Value = percent;

            //Time since last tick should be very very close to Interval
            timesTicked++;
            if (timesTicked > timesToTick)
            {
                stopTime = time;
                dispatcherTimer.Stop();
                //IsEnabled should now be false after calling stop
                span = stopTime - startTime;
                lose();
            }
        }

        private async void win()
        {
            MessageDialog msgDialog = new MessageDialog("", "Great!! You win.");

            //OK Button
            UICommand okBtn = new UICommand("Play again");
            okBtn.Invoked = OkBtnClick;
            msgDialog.Commands.Add(okBtn);

            //Show message
            await msgDialog.ShowAsync();
        }

        private async void lose()
        {
            MessageDialog msgDialog = new MessageDialog("", " You lose");

            //OK Button
            UICommand okBtn = new UICommand("Play again");
            okBtn.Invoked = OkBtnClick;
            msgDialog.Commands.Add(okBtn);

            //Show message
            await msgDialog.ShowAsync();
        }

        private void OkBtnClick(IUICommand command)
        {
            initGame();
        }

        private void btn_print_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {
                    MyBox box = mapBox[i, j];
                    if (!box.Alive)
                    {
                        System.Diagnostics.Debug.WriteLine("alive["+i+","+j+"] = DEAD;");
                    }
                    else
                    {
                        if (box.ImageId != 1)
                        {
                            System.Diagnostics.Debug.WriteLine("imageId["+i+","+j+"] = "+box.ImageId+";");
                        }
                    }
                }
            }
        }
    }

    public class MyBox
    {
        int row = 0;
        int col = 0;
        int imageId = 0;
        bool alive;

        public int Row
        {
            get
            {
                return row;
            }

            set
            {
                row = value;
            }
        }

        public int Col
        {
            get
            {
                return col;
            }

            set
            {
                col = value;
            }
        }

        public int ImageId
        {
            get
            {
                return imageId;
            }

            set
            {
                imageId = value;
            }
        }

        public bool Alive
        {
            get
            {
                return alive;
            }

            set
            {
                alive = value;
            }
        }
    }
}
