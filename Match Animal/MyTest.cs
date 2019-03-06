using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Match_Animal
{
    class MyTest
    {
        public static List<MyBox> generateMockup(int ROW,int COL)
        {
            int ALIVE = 0;
            int DEAD = 1;
            List<MyBox> listBox = new List<MyBox>();

            int[,] imageId = new int[ROW, COL];
            int[,] alive = new int[ROW, COL];

            // test A
            // 
            //imageId[1, 3] = 2;
            //alive[1, 4] = DEAD;
            //imageId[1, 6] = 2;


            //imageId[4, 10] = 2;
            //alive[5, 10] = DEAD;
            //imageId[7, 10] = 2;

            // test B
            //alive[0, 2] = DEAD;
            //alive[1, 2] = DEAD;
            //imageId[1, 3] = 2;
            //imageId[2, 4] = 2;
            //alive[2, 5] = DEAD;
            //imageId[3, 3] = 2;
            //alive[3, 4] = DEAD;
            //alive[3, 9] = DEAD;
            //alive[3, 10] = DEAD;
            //alive[3, 11] = DEAD;
            //alive[4, 9] = DEAD;
            //alive[4, 11] = DEAD;
            //alive[4, 12] = DEAD;


            for (int i = 0; i < ROW; i++)
            {
                for (int j = 0; j < COL; j++)
                {

                    MyBox box = new MyBox();
                    if (alive[i, j] == ALIVE)
                    {
                        box.Alive = true;
                    }
                    else
                    {
                        box.Alive = false;
                    }

                    if (imageId[i, j] == 0)
                    {
                        box.ImageId = 1;
                    }
                    else
                    {
                        box.ImageId = imageId[i, j];
                    }

                    listBox.Add(box);
                }
            }
            return listBox;
        }
    }
}
