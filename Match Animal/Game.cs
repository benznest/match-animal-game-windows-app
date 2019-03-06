using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Match_Animal
{
    class Game
    {
        private static int abs(int a, int b)
        {
            int result = a - b;
            if (result < 0)
            {
                result *= -1;
            }

            return result;
        }

        private static double calPointY(double row)
        {
            return (row * 50) + 130 + (row * 4) + 25;
        }

        private static double calPointX(double col)
        {
            return (col * 50) + 70 + (col * 4) + 25;
        }

        private static Point calPoint(double row, double col)
        {
            // Swap when display on UI.
            // col = X
            // row = Y
            return new Point(calPointX(col), calPointY(row));
        }

        public static List<Point> isWin(MyBox[,] mapBox, int row_a, int col_a, int row_b, int col_b, int ROW, int COL)
        {
            List<Point> path = new List<Point>();

            // case nearly.
            //
            //    AB
            //

            if ((abs(row_a, row_b) == 0 && abs(col_a, col_b) == 1) || (abs(row_a, row_b) == 1 && abs(col_a, col_b) == 0))
            {
                Point p1 = calPoint(row_a, col_a);
                Point p2 = calPoint(row_b, col_b);

                path.Add(p1);
                path.Add(p2);
                return path;
            }

            // case on border.
            //             ________
            //             |      |
            //      XXXXXXXAXXXXXXBXXXXX
            //      XXXXXXXXXXXXXXXXXXXXX
            //
            //  If A and B is same on row or col. It possible for kill.

            if (row_a == 0 && row_b == 0)
            {
                Point p1 = calPoint(row_a, col_a);
                Point p2 = calPoint(row_a - 0.7, col_a);
                Point p3 = calPoint(row_b - 0.7, col_b);
                Point p4 = calPoint(row_b, col_b);

                path.Add(p1);
                path.Add(p2);
                path.Add(p3);
                path.Add(p4);
                return path;
            }
            else if (row_a == ROW - 1 && row_b == ROW - 1)
            {
                Point p1 = calPoint(row_a, col_a);
                Point p2 = calPoint(row_a + 0.7, col_a);
                Point p3 = calPoint(row_b + 0.7, col_b);
                Point p4 = calPoint(row_b, col_b);

                path.Add(p1);
                path.Add(p2);
                path.Add(p3);
                path.Add(p4);
                return path;
            }
            else if (col_a == 0 && col_b == 0)
            {
                Point p1 = calPoint(row_a, col_a);
                Point p2 = calPoint(row_a, col_a - 0.7);
                Point p3 = calPoint(row_b, col_b - 0.7);
                Point p4 = calPoint(row_b, col_b);

                path.Add(p1);
                path.Add(p2);
                path.Add(p3);
                path.Add(p4);
                return path;
            }
            else if (col_a == COL - 1 && col_b == COL - 1)
            {
                Point p1 = calPoint(row_a, col_a);
                Point p2 = calPoint(row_a, col_a + 0.7);
                Point p3 = calPoint(row_b, col_b + 0.7);
                Point p4 = calPoint(row_b, col_b);

                path.Add(p1);
                path.Add(p2);
                path.Add(p3);
                path.Add(p4);
                return path;
            }

            // case 1 corner.
            //
            //        XXXXXXXXXXXXXXXX
            //        XXXXXXXX|XXXXXXX
            //        |XXXXXXX|XXXXXXX
            //  ------A-------O----XXX
            //        |XXXXXXX|XXXXXXX
            //        |XXXXXXX|XXXXXXX   
            //        |XXXXXXX|XXXXXXX
            //        XXXXXX--B----XXX
            //       XXXXXXX  |
            //            XXXXXXXXXX
            //
            // When draw line from ordinate Vertical and Horizontal
            // If line A and B can across. It is posible to kill.

            bool[,] tableA = new bool[ROW, COL];
            int top_A = 0;
            int down_A = 0;
            int left_A = 0;
            int right_A = 0;
            // walk top from A.
            int i = 0;
            for (i = row_a - 1; i >= 0; i--)
            {
                if (!mapBox[i, col_a].Alive)
                {
                    tableA[i, col_a] = true;
                }
                else
                {
                    break;
                }
            }
            top_A = i + 1;

            // walk down from A
            for (i = row_a + 1; i < ROW; i++)
            {
                if (!mapBox[i, col_a].Alive)
                {
                    tableA[i, col_a] = true;
                }
                else
                {
                    break;
                }
            }
            down_A = i - 1;

            // walk left from A
            for (i = col_a - 1; i >= 0; i--)
            {
                if (!mapBox[row_a, i].Alive)
                {
                    tableA[row_a, i] = true;
                }
                else
                {
                    break;
                }
            }
            left_A = i + 1;

            // walk right from A
            for (i = col_a + 1; i < COL; i++)
            {
                if (!mapBox[row_a, i].Alive)
                {
                    tableA[row_a, i] = true;
                }
                else
                {
                    break;
                }
            }
            right_A = i - 1;


            //compare table A with B

            bool[,] tableB = new bool[ROW, COL];
            int top_B = 0;
            int down_B = 0;
            int left_B = 0;
            int right_B = 0;

            // walk top from b
            for (i = row_b - 1; i >= 0; i--)
            {
                if (!mapBox[i, col_b].Alive)
                {
                    tableB[i, col_b] = true;
                    if (tableA[i, col_b])
                    {
                        Point p1 = calPoint(row_a, col_a);
                        Point p2 = calPoint(i, col_b);
                        Point p3 = calPoint(row_b, col_b);

                        path.Add(p1);
                        path.Add(p2);
                        path.Add(p3);
                        return path;
                    }
                }
                else
                {
                    break;
                }
            }
            top_B = i + 1;

            // walk down from b
            for (i = row_b + 1; i < ROW; i++)
            {
                if (!mapBox[i, col_b].Alive)
                {
                    tableB[i, col_b] = true;
                    if (tableA[i, col_b])
                    {
                        Point p1 = calPoint(row_a, col_a);
                        Point p2 = calPoint(i, col_b);
                        Point p3 = calPoint(row_b, col_b);

                        path.Add(p1);
                        path.Add(p2);
                        path.Add(p3);
                        return path;
                    }
                }
                else
                {
                    break;
                }
            }
            down_B = i - 1;

            // walk left from b
            for (i = col_b - 1; i >= 0; i--)
            {
                if (!mapBox[row_b, i].Alive)
                {
                    tableB[row_b, i] = true;
                    if (tableA[row_b, i])
                    {
                        Point p1 = calPoint(row_a, col_a);
                        Point p2 = calPoint(row_b, i);
                        Point p3 = calPoint(row_b, col_b);

                        path.Add(p1);
                        path.Add(p2);
                        path.Add(p3);
                        return path;
                    }
                }
                else
                {
                    break;
                }
            }
            left_B = i + 1;

            // wlak right from B
            for (i = col_b + 1; i < COL; i++)
            {
                if (!mapBox[row_b, i].Alive)
                {
                    tableB[row_b, i] = true;
                    if (tableA[row_b, i])
                    {
                        Point p1 = calPoint(row_a, col_a);
                        Point p2 = calPoint(row_b, i);
                        Point p3 = calPoint(row_b, col_b);

                        path.Add(p1);
                        path.Add(p2);
                        path.Add(p3);
                        return path;
                    }
                }
                else
                {
                    break;
                }
            }
            right_B = i - 1;

            // case 2 corner without border 
            // 
            //
            //               -----> . . .. . . . . . . . . .  . . 
            //                    X                    X
            //                    X                    | <- This top_B
            //  This is top_A  -> |    This is right_A |
            //                    |       |            | 
            //                    |       v            |
            //    X---------------A--------XXX---------B-----------X
            //     ^              |           ^        |
            // This is left_A     |   This is left_A   |
            //                    |                    | <- down_A
            //  This is down_A -> |                    X
            //                    X
            //
            //
            //               -----> . . .. . . . . . . . . .  . . 
            //                    X                    X
            //                    X                    | <- This top_B
            //  This is top_A  -> |------------------->|
            //                    |------------------->| 
            //                    |          X         |
            //                    A XXXXXXXXXXXXXXXXXX B
            //
            // If you can find line for point on A to B without Alive node. 
            // It's posible for kill.
            //

            if (top_A == 0 && top_B == 0)
            {
                Point p1 = calPoint(row_a, col_a);
                Point p2 = calPoint(-0.7, col_a);
                Point p3 = calPoint(-0.7, col_b);
                Point p4 = calPoint(row_b, col_b);

                path.Add(p1);
                path.Add(p2);
                path.Add(p3);
                path.Add(p4);
                return path;
            }
            else if (down_A == ROW - 1 && down_B == ROW - 1)
            {
                Point p1 = calPoint(row_a, col_a);
                Point p2 = calPoint((ROW - 1) + 0.7, col_a);
                Point p3 = calPoint((ROW - 1) + 0.7, col_b);
                Point p4 = calPoint(row_b, col_b);

                path.Add(p1);
                path.Add(p2);
                path.Add(p3);
                path.Add(p4);
                return path;
            }
            else if (left_A == 0 && left_B == 0)
            {
                Point p1 = calPoint(row_a, col_a);
                Point p2 = calPoint(row_a, -0.7);
                Point p3 = calPoint(row_b, -0.7);
                Point p4 = calPoint(row_b, col_b);

                path.Add(p1);
                path.Add(p2);
                path.Add(p3);
                path.Add(p4);
                return path;
            }
            else if (right_A == COL - 1 && right_B == COL - 1)
            {
                Point p1 = calPoint(row_a, col_a);
                Point p2 = calPoint(row_a, (COL - 1) + 0.7);
                Point p3 = calPoint(row_b, (COL - 1) + 0.7);
                Point p4 = calPoint(row_b, col_b);

                path.Add(p1);
                path.Add(p2);
                path.Add(p3);
                path.Add(p4);
                return path;
            }



            if (col_b < col_a )
            {
                swap(ref col_b, ref col_a);
                swap(ref row_b, ref row_a);

                swap(ref top_B, ref top_A);
                swap(ref down_B, ref down_A);
                swap(ref left_B, ref left_A);
                swap(ref right_B, ref right_A);

                for (int m = 0; m < ROW; m++)
                {
                    for (int j = 0; j < COL; j++)
                    {
                        swap(ref tableA[m,j], ref tableB[m, j]);
                    }
                }
            }

            // walk from A top
            int k;
            for (k = row_a - 1; k >= top_A; k--)
            {
                if (mapBox[k, col_a].Alive)
                {
                    break;
                }

                int j;
                for (j = col_a; j <= col_b; j++)
                {
                    if (mapBox[k, j].Alive)
                    {
                        j = 1000;
                        break;
                    }

                    if (j == col_b && (k >= top_B && k <= down_B))
                    {
                        Point p1 = calPoint(row_a, col_a);
                        Point p2 = calPoint(k, col_a);
                        Point p3 = calPoint(k, j);
                        Point p4 = calPoint(row_b, col_b);

                        path.Add(p1);
                        path.Add(p2);
                        path.Add(p3);
                        path.Add(p4);
                        return path;
                    }
                }
            }

            // walk from A down
            for (k = row_a + 1; k <= down_A; k++)
            {
                if (mapBox[k, col_a].Alive)
                {
                    break;
                }

                int j;
                for (j = col_a; j <= col_b; j++)
                {
                    if (mapBox[k, j].Alive)
                    {
                        j = 1000;
                        break;
                    }

                    if (j == col_b && (k >= top_B && k <= down_B))
                    {
                        Point p1 = calPoint(row_a, col_a);
                        Point p2 = calPoint(k, col_a);
                        Point p3 = calPoint(k, j);
                        Point p4 = calPoint(row_b, col_b);

                        path.Add(p1);
                        path.Add(p2);
                        path.Add(p3);
                        path.Add(p4);
                        return path;
                    }
                }


            }

            if (row_b < row_a)
            {
                swap(ref col_b, ref col_a);
                swap(ref row_b, ref row_a);

                swap(ref top_B, ref top_A);
                swap(ref down_B, ref down_A);
                swap(ref left_B, ref left_A);
                swap(ref right_B, ref right_A);

                for (int m = 0; m < ROW; m++)
                {
                    for (int j = 0; j < COL; j++)
                    {
                        swap(ref tableA[m, j], ref tableB[m, j]);
                    }
                }
            }

            // 
            for (k = col_a - 1; k >= left_A; k--)
            {
                if (mapBox[row_a, k].Alive)
                {
                    break;
                }

                int j;
                for (j = row_a; j <= row_b; j++)
                {
                    if (mapBox[j, k].Alive)
                    {
                        j = 1000;
                        break;
                    }

                    if (j == row_b && (k >= left_B && k <= right_B))
                    {
                        Point p1 = calPoint(row_a, col_a);
                        Point p2 = calPoint(row_a, k);
                        Point p3 = calPoint(j, k);
                        Point p4 = calPoint(row_b, col_b);

                        path.Add(p1);
                        path.Add(p2);
                        path.Add(p3);
                        path.Add(p4);
                        return path;
                    }
                }
            }

            for (k = col_a + 1; k <= right_A; k++)
            {
                if (mapBox[row_a, k].Alive)
                {
                    break;
                }

                int j;
                for (j = row_a; j <= row_b; j++)
                {
                    if (mapBox[j, k].Alive)
                    {
                        j = 1000;
                        break;
                    }

                    if (j == row_b && (k >= left_B && k <= right_B))
                    {
                        Point p1 = calPoint(row_a, col_a);
                        Point p2 = calPoint(row_a, k);
                        Point p3 = calPoint(j, k);
                        Point p4 = calPoint(row_b, col_b);

                        path.Add(p1);
                        path.Add(p2);
                        path.Add(p3);
                        path.Add(p4);
                        return path;
                    }
                }

         
            }

            return null;
        }

        private static void swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        private static void swap(ref bool a,ref bool b)
        {
            bool temp = a;
            a = b;
            b = temp;
        }
    }
}
