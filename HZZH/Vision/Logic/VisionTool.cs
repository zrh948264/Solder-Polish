using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Vision.Tool.Calibrate;
using Vision.Tool.Model;

namespace HZZH.Vision.Logic
{
    [Serializable]
    class VisionTool
    {
        /// <summary>
        /// 焊锡左边平台
        /// </summary>
        public List<LocationShapePoint> SolderLeft = new List<LocationShapePoint>();
        /// <summary>
        /// 焊锡右边平台
        /// </summary>
        public List<LocationShapePoint> SolderRight = new List<LocationShapePoint>();

        /// <summary>
        /// 打磨左边平台
        /// </summary>
        public List<LocationShapePoint> PolishLeft = new List<LocationShapePoint>();
        /// <summary>
        /// 打磨右边平台
        /// </summary>
        public List<LocationShapePoint> PolishRight = new List<LocationShapePoint>();



        public VisionTool()
        {

        }


        [OnDeserialized()]
        private void OnDeserializedMethod(StreamingContext context)
        {

        }

    }

    /// <summary>
    /// 一组模板和定位点的组合
    /// </summary>
    [Serializable]
    public class LocationShapePoint
    {
        //public string Name = "";
        public NCCModel Shape = new NCCModel();
        [NonSerialized]
        public LocationCrossCollection locationCross = new LocationCrossCollection();

        [NonSerialized]
        public LocationCrossCollection TransLocationCross = new LocationCrossCollection();

        public LocationShapePoint()
        {
            //Shape.shapeParam.mStartingAngle = -10 * Math.PI / 180;
            //Shape.shapeParam.mAngleExtent = 10 * Math.PI / 180 * 2;
            //Shape.shapeParam.mNumMatches = 1;
            //Shape.shapeParam.mMaxOverlap = 0.1;
            //Shape.shapeParam.mGreediness = 0.1;

            Shape.nCCParam.mStartingAngle = -10 * Math.PI / 180;
            Shape.nCCParam.mAngleExtent = 10 * Math.PI / 180 * 2;
            Shape.nCCParam.NumMatches = 1;
            Shape.nCCParam.mMaxOverlap = 0.1;
        }

        public void AffineTransPoint()
        {
            TransLocationCross.Clear();
            ShapeMatchResult matchResult = Shape.OutputResult;
            for (int i = 0; i < matchResult.Count; i++)
            {
                HHomMat2D mat2d = new HHomMat2D();
                mat2d.VectorAngleToRigid(Shape.ModelImgRow, Shape.ModelImgCol, Shape.ModelimgAng,
                    matchResult.Row[i].D, matchResult.Col[i].D, matchResult.Angle[i].D);
                PointF[] fs = Array.ConvertAll<Cross, PointF>(locationCross.GetAllPointFs(), e => e.Pixel);
                HTuple tx, ty;
                HOperatorSet.AffineTransPoint2d(mat2d,
                    Array.ConvertAll<PointF, double>(fs, e => e.Y).ToArray(),
                    Array.ConvertAll<PointF, double>(fs, e => e.X).ToArray(),
                    out ty,
                    out tx);

                for (int m = 0; m < locationCross.Count; m++)
                {
                    Cross cross = new Cross();
                    cross.Pixel = new PointF(tx[m].F, ty[m].F);
                    cross.Angle = matchResult.Angle[m].D;
                    cross.Color = "green";
                    TransLocationCross.Add(cross);
                }
            }
        }

        public Size ImageSize
        {
            get
            {
                Size size = new Size();
                if (Shape.ModelImg != null && Shape.ModelImg.IsInitialized() == true)
                {
                    int width, height;
                    Shape.ModelImg.GetImageSize(out width, out height);
                    size.Width = width;
                    size.Height = height;
                }
                return size;
            }
        }

        public PointF GetShapeDeviation(CalibPointToPoint calib)
        {
            PointF imageCenter = new PointF();
            if (Shape.ModelImg != null && Shape.ModelImg.IsInitialized())
            {
                int width, height;
                Shape.ModelImg.GetImageSize(out width, out height);
                imageCenter.X = width / 2.0f;
                imageCenter.Y = height / 2.0f;
            }
            else
            {
                return new PointF();
            }

            PointF point = new PointF();
            PointF pixel = new PointF();
            pixel.X = Shape.ModelImgCol;
            pixel.Y = Shape.ModelImgRow;
            calib.PixelPointToWorldPoint(pixel, out point, imageCenter, new PointF());
            return point;
        }


        //public void AddPointInLocationCrossDisplay(float x, float y, CalibPointToPoint calib)
        //{ 
        //    PointF pixel;
        //    calib.WorldPointToPixelPoint(new PointF(x, y), out pixel, new PointF(ImageSize.Width / 2f, ImageSize.Width / 2f), new PointF());
        //    Cross cross=new Cross ();
        //    cross.Pixel=pixel;
        //    locationCross.Add(cross);
        //}
    }


   



    /// <summary>
    /// 十字叉
    /// </summary>
    [Serializable]
    public class Cross
    {
        public PointF Pixel;
        public int Size = 50;
        public double Angle = 0;
        public string Color = "green";
    }


    [Serializable]
    public class LocationCrossCollection
    {
        private const int FAR_ACTIVE_DISTANCE = 20;
        private List<Cross> crossList = new List<Cross>();

        public const string AcitveColor = "blue";
        public const string InActiveColor = "yellow";

        [field:NonSerialized]
        public event EventHandler MoveActiveCrossEvent;

        [NonSerialized]
        private int ActiveIndex = -1;

        public Cross ActiveCross
        {
            get
            {
                Cross cross = null;
                if (ActiveIndex >= 0)
                {
                    cross = crossList[ActiveIndex];
                }

                return cross;
            }
        }

        public int Count
        {
            get
            {
                return crossList.Count;
            }
        }

        public void SetCrossActive(double x, double y)
        {
            double[] val = new double[crossList.Count];
            for (int i = 0; i < crossList.Count; i++)
            {
                crossList[i].Color = InActiveColor;
                val[i] = HMisc.DistancePp(crossList[i].Pixel.Y, crossList[i].Pixel.X, y, x);
            }

            if (val.Length > 0 && val.Min() < FAR_ACTIVE_DISTANCE)
            {
                ActiveIndex = Array.IndexOf(val, val.Min());
                crossList[ActiveIndex].Color = AcitveColor;
            }
            else
            {
                ActiveIndex = -1;
            }
        }

        public void MoveActiveCross(double x, double y)
        {
            if (ActiveIndex >= 0)
            {
                crossList[ActiveIndex].Pixel = new PointF((float)x, (float)y);

                EventHandler handler = MoveActiveCrossEvent;
                if (handler != null)
                {
                    handler(this, new MoveActiveCrossEventArgs(ActiveIndex, crossList[ActiveIndex].Pixel));
                }

            }
        }

        public void Add(Cross cross)
        {
            crossList.Add(cross);
        }

        public void Clear()
        {
            crossList.Clear();
        }

        public void RemoveActive()
        {
            if (ActiveIndex >= 0)
            {
                crossList.RemoveAt(ActiveIndex);
                ActiveIndex = -1;
            }
        }

        public void Draw(HWindow hWindow)
        {
            hWindow.SetLineWidth(2);

            for (int i = 0; i < crossList.Count; i++)
            {
                hWindow.SetColor(crossList[i].Color);
                hWindow.DispCross((double)crossList[i].Pixel.Y, (double)crossList[i].Pixel.X, crossList[i].Size, 0);
            }

        }

        public Cross[] GetAllPointFs()
        {
            return crossList.ToArray();
        }
    }


    public class MoveActiveCrossEventArgs : EventArgs
    {
        public int Index { get; private set; }
        public PointF Pixel { get; private set; }

        public MoveActiveCrossEventArgs(int index, PointF pixel)
        {
            this.Index = index;
            this.Pixel = pixel;
        }
    }
}
