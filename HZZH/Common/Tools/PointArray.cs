using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{



    /// <summary>
    /// 两点阵列和三点阵列类
    /// </summary>
    public class PointArray
    {

        #region 两点阵列
        /// <summary>
        /// 两点阵列
        /// </summary>
        /// <param name="p1">点一</param>
        /// <param name="p2">点二</param>
        /// <param name="row">行数</param>
        /// <param name="column">列数</param>
        /// <param name="direction">阵列方向，行优先或者列优先</param>
        /// <returns></returns>
        public static List<PointF2> MatrixArrayList(PointF2 p1, PointF2 p2, int row, int column, int direction)
        {
            float deltaX1, deltaY1;
            if (row <= 0 || column <= 0 || (row == 1 && column == 1)) return null;
            if (column > 1)
            {
                deltaX1 = (p1.X - p2.X) / (column - 1);
            }
            else
            {
                deltaX1 = (p1.X - p2.X);
                deltaX1 /= (row - 1);
            }

            if (row > 1)
            {
                deltaY1 = (p1.Y - p2.Y) / (row - 1);
            }
            else
            {
                deltaY1 = (p1.Y - p2.Y);
                deltaY1 /= (column - 1);
            }


            #region 做点

            List<PointF2> termlist = new List<PointF2>();
            if (column == 1)
            {
                for (int i = 0; i < row; i++)
                {
                    PointF2 stickPoint = new PointF2();
                    stickPoint.X = p2.X + deltaX1 * i;
                    stickPoint.Y = p2.Y + deltaY1 * i;
                    termlist.Add(stickPoint);
                }
            }
            else if (row == 1)
            {
                for (int i = 0; i < column; i++)
                {
                    PointF2 stickPoint = new PointF2();
                    stickPoint.X = p2.X + deltaX1 * i;
                    stickPoint.Y = p2.Y + deltaY1 * i;
                    termlist.Add(stickPoint);
                }
            }
            else
            {
                if (direction == 0) //横向优先
                {
                    for (int i = 0; i < row; i++)  //行
                    {
                        for (int j = 0; j < column; j++)  //列
                        {
                            PointF2 stickPoint = new PointF2();
                            if (i % 2 == 0)
                            {
                                stickPoint.X = p2.X + deltaX1 * j;
                                stickPoint.Y = p2.Y + deltaY1 * i;
                            }
                            else
                            {
                                stickPoint.X = p2.X + deltaX1 * (column - 1 - j);
                                stickPoint.Y = p2.Y + deltaY1 * i;
                            }
                            termlist.Add(stickPoint);
                        }
                    }
                }
                else if (direction == 1) //纵向优先
                {
                    for (int i = 0; i < column; i++)  //列
                    {
                        for (int j = 0; j < row; j++)  //行
                        {
                            PointF2 stickPoint = new PointF2();
                            if (i % 2 == 0)
                            {
                                stickPoint.X = p2.X + deltaX1 * i;
                                stickPoint.Y = p2.Y + deltaY1 * j;
                            }
                            else
                            {
                                stickPoint.X = p2.X + deltaX1 * i;
                                stickPoint.Y = p2.Y + deltaY1 * (row - 1 - j);
                            }
                            termlist.Add(stickPoint);
                        }
                    }
                }
            }
            #endregion
            return termlist;
        }


        /// <summary>
        /// 两点阵列
        /// </summary>
        /// <param name="p1">点一</param>
        /// <param name="p2">点二</param>
        /// <param name="row">行数</param>
        /// <param name="column">列数</param>
        /// <param name="direction">阵列方向，行优先或者列优先</param>
        /// <returns></returns>
        public static List<PointF3> MatrixArrayList(PointF3 p1, PointF3 p2, int row, int column, int direction)
        {
            float deltaX1, deltaY1;
            if (row <= 0 || column <= 0 || (row == 1 && column == 1)) return null;
            if (column > 1)
            {
                deltaX1 = (p1.X - p2.X) / (column - 1);
            }
            else
            {
                deltaX1 = (p1.X - p2.X);
                deltaX1 /= (row - 1);
            }

            if (row > 1)
            {
                deltaY1 = (p1.Y - p2.Y) / (row - 1);
            }
            else
            {
                deltaY1 = (p1.Y - p2.Y);
                deltaY1 /= (column - 1);
            }


            #region 做点

            List<PointF3> termlist = new List<PointF3>();
            if (column == 1)
            {
                for (int i = 0; i < row; i++)
                {
                    PointF3 stickPoint = new PointF3();
                    stickPoint.X = p2.X + deltaX1 * i;
                    stickPoint.Y = p2.Y + deltaY1 * i;
                    stickPoint.Z = p2.Z;
                    termlist.Add(stickPoint);
                }
            }
            else if (row == 1)
            {
                for (int i = 0; i < column; i++)
                {
                    PointF3 stickPoint = new PointF3();
                    stickPoint.X = p2.X + deltaX1 * i;
                    stickPoint.Y = p2.Y + deltaY1 * i;
                    stickPoint.Z = p2.Z;
                    termlist.Add(stickPoint);
                }
            }
            else
            {
                if (direction == 0) //横向优先
                {
                    for (int i = 0; i < row; i++)  //行
                    {
                        for (int j = 0; j < column; j++)  //列
                        {
                            PointF3 stickPoint = new PointF3();
                            if (i % 2 == 0)
                            {
                                stickPoint.X = p2.X + deltaX1 * j;
                                stickPoint.Y = p2.Y + deltaY1 * i;
                                stickPoint.Z = p2.Z;
                            }
                            else
                            {
                                stickPoint.X = p2.X + deltaX1 * (column - 1 - j);
                                stickPoint.Y = p2.Y + deltaY1 * i;
                                stickPoint.Z = p2.Z;
                            }
                            termlist.Add(stickPoint);
                        }
                    }
                }
                else if (direction == 1) //纵向优先
                {
                    for (int i = 0; i < column; i++)  //列
                    {
                        for (int j = 0; j < row; j++)  //行
                        {
                            PointF3 stickPoint = new PointF3();
                            if (i % 2 == 0)
                            {
                                stickPoint.X = p2.X + deltaX1 * i;
                                stickPoint.Y = p2.Y + deltaY1 * j;
                                stickPoint.Z = p2.Z;
                            }
                            else
                            {
                                stickPoint.X = p2.X + deltaX1 * i;
                                stickPoint.Y = p2.Y + deltaY1 * (row - 1 - j);
                                stickPoint.Z = p2.Z;
                            }
                            termlist.Add(stickPoint);
                        }
                    }
                }
            }
            #endregion
            return termlist;
        }

        /// <summary>
        /// 两点阵列
        /// </summary>
        /// <param name="p1">点一</param>
        /// <param name="p2">点二</param>
        /// <param name="row">行数</param>
        /// <param name="column">列数</param>
        /// <param name="direction">阵列方向，行优先或者列优先</param>
        /// <returns></returns>
        public static List<PointF4> MatrixArrayList(PointF4 p1, PointF4 p2, int row, int column, int direction)
        {
            float deltaX1, deltaY1;
            if (row <= 0 || column <= 0 || (row == 1 && column == 1)) return null;
            if (column > 1)
            {
                deltaX1 = (p1.X - p2.X) / (column - 1);
            }
            else
            {
                deltaX1 = (p1.X - p2.X);
                deltaX1 /= (row - 1);
            }

            if (row > 1)
            {
                deltaY1 = (p1.Y - p2.Y) / (row - 1);
            }
            else
            {
                deltaY1 = (p1.Y - p2.Y);
                deltaY1 /= (column - 1);
            }


            #region 做点

            List<PointF4> termlist = new List<PointF4>();
            if (column == 1)
            {
                for (int i = 0; i < row; i++)
                {
                    PointF4 stickPoint = new PointF4();
                    stickPoint.X = p2.X + deltaX1 * i;
                    stickPoint.Y = p2.Y + deltaY1 * i;
                    stickPoint.Z = p2.Z;
                    stickPoint.R = p2.R;
                    termlist.Add(stickPoint);
                }
            }
            else if (row == 1)
            {
                for (int i = 0; i < column; i++)
                {
                    PointF4 stickPoint = new PointF4();
                    stickPoint.X = p2.X + deltaX1 * i;
                    stickPoint.Y = p2.Y + deltaY1 * i;
                    stickPoint.Z = p2.Z;
                    stickPoint.R = p2.R;
                    termlist.Add(stickPoint);
                }
            }
            else
            {
                if (direction == 0) //横向优先
                {
                    for (int i = 0; i < row; i++)  //行
                    {
                        for (int j = 0; j < column; j++)  //列
                        {
                            PointF4 stickPoint = new PointF4();
                            if (i % 2 == 0)
                            {
                                stickPoint.X = p2.X + deltaX1 * j;
                                stickPoint.Y = p2.Y + deltaY1 * i;
                                stickPoint.Z = p2.Z;
                                stickPoint.R = p2.R;
                            }
                            else
                            {
                                stickPoint.X = p2.X + deltaX1 * (column - 1 - j);
                                stickPoint.Y = p2.Y + deltaY1 * i;
                                stickPoint.Z = p2.Z;
                                stickPoint.R = p2.R;
                            }
                            termlist.Add(stickPoint);
                        }
                    }
                }
                else if (direction == 1) //纵向优先
                {
                    for (int i = 0; i < column; i++)  //列
                    {
                        for (int j = 0; j < row; j++)  //行
                        {
                            PointF4 stickPoint = new PointF4();
                            if (i % 2 == 0)
                            {
                                stickPoint.X = p2.X + deltaX1 * i;
                                stickPoint.Y = p2.Y + deltaY1 * j;
                                stickPoint.Z = p2.Z;
                                stickPoint.R = p2.R;
                            }
                            else
                            {
                                stickPoint.X = p2.X + deltaX1 * i;
                                stickPoint.Y = p2.Y + deltaY1 * (row - 1 - j);
                                stickPoint.Z = p2.Z;
                                stickPoint.R = p2.R;
                            }
                            termlist.Add(stickPoint);
                        }
                    }
                }
            }
            #endregion
            return termlist;
        }


        #endregion


        #region 三点阵列

        /// <summary>
        /// 三点阵列
        /// </summary>
        /// <param name="p1">点一</param>
        /// <param name="p2">点二</param>
        /// <param name="p3">点三</param>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        /// <param name="direction">阵列方向，行优先或者列优先</param>
        /// <returns></returns>
        public static List<PointF2> MatrixArrayList(PointF2 p1, PointF2 p2, PointF2 p3, int row, int column, int direction)
        {
            float deltaX1, deltaY1, deltaX2, deltaY2;
            if (row <= 0 || column <= 0 || (row == 1 && column == 1)) return null;
            if (column > 1)
            {
                deltaX1 = (p1.X - p2.X) / (column - 1);
                deltaY1 = (p1.Y - p2.Y) / (column - 1);
            }
            else
            {
                deltaX1 = p1.X - p2.X;
                deltaY1 = p1.Y - p2.Y;
            }

            if (row > 1)
            {
                deltaX2 = (p3.X - p2.X) / (row - 1);
                deltaY2 = (p3.Y - p2.Y) / (row - 1);
            }
            else
            {
                deltaX2 = p3.X - p2.X;
                deltaY2 = p3.Y - p2.Y;
            }

            #region 做点

            List<PointF2> termlist = new List<PointF2>();

            if (direction == 0) //横向优先
            {
                for (int i = 0; i < row; i++)  //行
                {
                    for (int j = 0; j < column; j++)  //列
                    {
                        PointF2 stickPoint = new PointF2();
                        if (i % 2 == 0)
                        {
                            stickPoint.X = p2.X + deltaX1 * j + deltaX2 * i;
                            stickPoint.Y = p2.Y + deltaY1 * j + deltaY2 * i;
                        }
                        else
                        {
                            stickPoint.X = p2.X + deltaX1 * (column - 1 - j) + deltaX2 * i;
                            stickPoint.Y = p2.Y + deltaY1 * (column - 1 - j) + deltaY2 * i;
                        }
                        termlist.Add(stickPoint);
                    }
                }
            }
            else if (direction == 1) //纵向优先
            {
                for (int i = 0; i < column; i++)  //列
                {
                    for (int j = 0; j < row; j++)  //行
                    {
                        PointF2 stickPoint = new PointF2();
                        if (i % 2 == 0)
                        {
                            stickPoint.X = p2.X + deltaX2 * j + deltaX1 * i;
                            stickPoint.Y = p2.Y + deltaY2 * j + deltaY1 * i;
                        }
                        else
                        {
                            stickPoint.X = p2.X + deltaX2 * (row - 1 - j) + deltaX1 * i;
                            stickPoint.Y = p2.Y + deltaY2 * (row - 1 - j) + deltaY1 * i;
                        }
                        termlist.Add(stickPoint);
                    }
                }
            }
            #endregion

            return termlist;
        }

        /// <summary>
        /// 三点阵列
        /// </summary>
        /// <param name="p1">点一</param>
        /// <param name="p2">点二</param>
        /// <param name="p3">点三</param>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        /// <param name="direction">阵列方向，行优先或者列优先</param>
        public static List<wPointF> MatrixArrayList(PointF3 p1, PointF3 p2, PointF3 p3, int row, int column, int direction)
        {
            float deltaX1, deltaY1, deltaZ1, deltaX2, deltaY2, deltaZ2;
            if (row <= 0 || column <= 0 || (row == 1 && column == 1)) return null;
            if (column > 1)
            {
                deltaX1 = (p1.X - p2.X) / (column - 1);
                deltaY1 = (p1.Y - p2.Y) / (column - 1);
                deltaZ1 = (p1.Z - p2.Z) / (column - 1);
            }
            else
            {
                deltaX1 = p1.X - p2.X;
                deltaY1 = p1.Y - p2.Y;
                deltaZ1 = p1.Z - p2.Z;
            }

            if (row > 1)
            {
                deltaX2 = (p3.X - p2.X) / (row - 1);
                deltaY2 = (p3.Y - p2.Y) / (row - 1);
                deltaZ2 = (p3.Z - p2.Z) / (row - 1);
            }
            else
            {
                deltaX2 = p3.X - p2.X;
                deltaY2 = p3.Y - p2.Y;
                deltaZ2 = p3.Z - p2.Z;
            }

            #region 做点

            List<wPointF> termlist = new List<wPointF>();

            if (direction == 0) //横向优先
            {
                for (int i = 0; i < row; i++)  //行
                {
                    for (int j = 0; j < column; j++)  //列
                    {
                        wPointF stickPoint = new wPointF();
                        if (i % 2 == 0)
                        {
                            stickPoint.X = p2.X + deltaX1 * j + deltaX2 * i;
                            stickPoint.Y = p2.Y + deltaY1 * j + deltaY2 * i;
                            //stickPoint.Z = p2.Z + deltaZ1 * j + deltaZ2 * i;
                        }
                        else
                        {
                            stickPoint.X = p2.X + deltaX1 * (column - 1 - j) + deltaX2 * i;
                            stickPoint.Y = p2.Y + deltaY1 * (column - 1 - j) + deltaY2 * i;
                            //stickPoint.Z = p2.Z + deltaZ1 * (column - 1 - j) + deltaZ2 * i;
                        }
                        termlist.Add(stickPoint);
                    }
                }
            }
            else if (direction == 1) //纵向优先
            {
                for (int i = 0; i < column; i++)  //列
                {
                    for (int j = 0; j < row; j++)  //行
                    {
                        wPointF stickPoint = new wPointF();
                        if (i % 2 == 0)
                        {
                            stickPoint.X = p2.X + deltaX2 * j + deltaX1 * i;
                            stickPoint.Y = p2.Y + deltaY2 * j + deltaY1 * i;
                            //stickPoint.Z = p2.Z + deltaZ2 * j + deltaZ1 * i;
                        }
                        else
                        {
                            stickPoint.X = p2.X + deltaX2 * (row - 1 - j) + deltaX1 * i;
                            stickPoint.Y = p2.Y + deltaY2 * (row - 1 - j) + deltaY1 * i;
                            //stickPoint.Z = p2.Z + deltaZ2 * (row - 1 - j) + deltaZ1 * i;
                        }
                        termlist.Add(stickPoint);
                    }
                }
            }
            #endregion
            return termlist;
        }

        /// <summary>
        /// 三点阵列
        /// </summary>
        /// <param name="p1">点一</param>
        /// <param name="p2">点二</param>
        /// <param name="p3">点三</param>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        /// <param name="direction">阵列方向，行优先或者列优先</param>
        public static List<PointF4> MatrixArrayList(PointF4 p1, PointF4 p2, PointF4 p3, int row, int column, int direction)
        {
            float deltaX1, deltaY1, deltaZ1, deltaR1, deltaX2, deltaY2, deltaZ2, deltaR2; ;
            if (row <= 0 || column <= 0 || (row == 1 && column == 1)) return null;
            if (column > 1)
            {
                deltaX1 = (p1.X - p2.X) / (column - 1);
                deltaY1 = (p1.Y - p2.Y) / (column - 1);
                deltaZ1 = (p1.Z - p2.Z) / (column - 1);
                deltaR1 = (p1.R - p2.R) / (column - 1);
            }
            else
            {
                deltaX1 = p1.X - p2.X;
                deltaY1 = p1.Y - p2.Y;
                deltaZ1 = p1.Z - p2.Z;
                deltaR1 = p1.R - p2.R;
            }

            if (row > 1)
            {
                deltaX2 = (p3.X - p2.X) / (row - 1);
                deltaY2 = (p3.Y - p2.Y) / (row - 1);
                deltaZ2 = (p3.Z - p2.Z) / (row - 1);
                deltaR2 = (p3.R - p2.R) / (row - 1);
            }
            else
            {
                deltaX2 = p3.X - p2.X;
                deltaY2 = p3.Y - p2.Y;
                deltaZ2 = p3.Z - p2.Z;
                deltaR2 = p3.R - p2.R;
            }

            #region 做点

            List<PointF4> termlist = new List<PointF4>();

            if (direction == 0) //横向优先
            {
                for (int i = 0; i < row; i++)  //行
                {
                    for (int j = 0; j < column; j++)  //列
                    {
                        PointF4 stickPoint = new PointF4();
                        if (i % 2 == 0)
                        {
                            stickPoint.X = p2.X + deltaX1 * j + deltaX2 * i;
                            stickPoint.Y = p2.Y + deltaY1 * j + deltaY2 * i;
                            stickPoint.Z = p2.Z + deltaZ1 * j + deltaZ2 * i;
                            stickPoint.R = p2.R + deltaR1 * j + deltaR2 * i;
                        }
                        else
                        {
                            stickPoint.X = p2.X + deltaX1 * (column - 1 - j) + deltaX2 * i;
                            stickPoint.Y = p2.Y + deltaY1 * (column - 1 - j) + deltaY2 * i;
                            stickPoint.Z = p2.Z + deltaZ1 * (column - 1 - j) + deltaZ2 * i;
                            stickPoint.R = p2.R + deltaR1 * (column - 1 - j) + deltaR2 * i;
                        }
                        termlist.Add(stickPoint);
                    }
                }
            }
            else if (direction == 1) //纵向优先
            {
                for (int i = 0; i < column; i++)  //列
                {
                    for (int j = 0; j < row; j++)  //行
                    {
                        PointF4 stickPoint = new PointF4();
                        if (i % 2 == 0)
                        {
                            stickPoint.X = p2.X + deltaX2 * j + deltaX1 * i;
                            stickPoint.Y = p2.Y + deltaY2 * j + deltaY1 * i;
                            stickPoint.Z = p2.Z + deltaZ2 * j + deltaZ1 * i;
                            stickPoint.R = p2.R + deltaR2 * j + deltaR1 * i;
                        }
                        else
                        {
                            stickPoint.X = p2.X + deltaX2 * (row - 1 - j) + deltaX1 * i;
                            stickPoint.Y = p2.Y + deltaY2 * (row - 1 - j) + deltaY1 * i;
                            stickPoint.Z = p2.Z + deltaZ2 * (row - 1 - j) + deltaZ1 * i;
                            stickPoint.R = p2.R + deltaR2 * (row - 1 - j) + deltaR1 * i;
                        }

                        termlist.Add(stickPoint);
                    }
                }
            }
            #endregion

            return termlist;
        }




        #endregion


    }

}
