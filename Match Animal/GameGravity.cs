using Match_Animal.MyControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Match_Animal
{
    class GameGravity
    {
        public static int GRAVITY_NONE = 0;
        public static int GRAVITY_LEFT = 1;
        public static int GRAVITY_RIGHT = 2;

        public static int randomGravity()
        {
            Random rnd = new Random();
            return rnd.Next(0, 2);
        }

        public static void moveTable(int ROW, int COL, int gravity, GridView gridView, ref MyBox[,] mapBox,
            MyGrid grid, MyGrid gridFirstSelected)
        {
            int row_a = gridFirstSelected.Box.Row;
            int col_a = gridFirstSelected.Box.Col;

            int row_b = grid.Box.Row;
            int col_b = grid.Box.Col;

            if (gravity == GRAVITY_LEFT)
            {
                moveToLeft(ROW, COL, gridView, ref mapBox, row_a, col_a);
                moveToLeft(ROW, COL, gridView, ref mapBox, row_b, col_b);
                return;
            }


            if (gravity == GRAVITY_RIGHT)
            {
                moveToRight(ROW, COL, gridView, ref mapBox, row_a, col_a);
                moveToRight(ROW, COL, gridView, ref mapBox, row_b, col_b);
                return;
            }
        }

        private static void moveToLeft(int ROW,int COL, GridView gridView, ref MyBox[,] mapBox, int row, int col)
        {
            for (int i = 0; i < COL; i++)
            {
                MyGrid grid = (MyGrid)gridView.Items[row * COL + i];
                if (!grid.Box.Alive)
                {
                    MyGrid grid_temp = (MyGrid)gridView.Items[row * COL + i];
                    gridView.Items.RemoveAt(row * COL + i);
                    gridView.Items.Insert(row * COL + (COL - 1), grid_temp);
                }
            }

            for (int i = 0; i < COL; i++)
            {
                MyGrid grid = (MyGrid)gridView.Items[row * COL + i];
                grid.Box.Row = row;
                grid.Box.Col = i;

                mapBox[row, i] = grid.Box;

                if (grid.Box.Alive)
                {
                    Button btn = grid.getButton();
                    btn.Tag = grid;
                }
            }
        }

        private static void moveToRight(int ROW, int COL, GridView gridView, ref MyBox[,] mapBox, int row, int col)
        {
            for (int i = COL - 1; i >= 0; i--)
            {
                MyGrid grid = (MyGrid)gridView.Items[row * COL + i];
                if (!grid.Box.Alive)
                {
                    MyGrid grid_temp = (MyGrid)gridView.Items[row * COL + i];
                    gridView.Items.RemoveAt(row * COL + i);
                    gridView.Items.Insert(row * COL, grid_temp);
                }
            }

            for (int i = 0; i < COL; i++)
            {
                MyGrid grid = (MyGrid)gridView.Items[row * COL + i];
                grid.Box.Row = row;
                grid.Box.Col = i;

                mapBox[row, i] = grid.Box;

                if (grid.Box.Alive)
                {
                    Button btn = grid.getButton();
                    btn.Tag = grid;
                }
            }
        }

        public static void Swap(ref int a, ref int b)
        {
            int tmp = a;
            a = b;
            b = tmp;
        }

        public static void Swap(ref MyBox a, ref MyBox b)
        {
            MyBox tmp = a;
            a = b;
            b = tmp;
        }

        public static void Swap(ref MyGrid a, ref MyGrid b)
        {
            MyGrid tmp = a;
            a = b;
            b = tmp;
        }
    }
}
