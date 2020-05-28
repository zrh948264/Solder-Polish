using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProCommon.Communal
{
    [Serializable]
    public class Camera : CtrllerObj, System.ComponentModel.INotifyPropertyChanged
    {
        public virtual event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(String name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(name));
            }
        }

        private Camera()
        {
            MothballCalibrationSolutionList = new ProVision.Communal.CalibrationSolutionList();
        }

        /// <summary>
        /// 创建相机
        /// [注:同品牌同类型下的相机编号不允许相同]
        /// </summary>
        /// <param name="brand"></param>
        /// <param name="ctrllerType"></param>
        /// <param name="camIndex"></param>
        /// <param name="camName"></param>
        public Camera(CtrllerBrand brand, CtrllerType ctrllerType, int camIndex, string camName) : this()
        {
            this.CtrllerCategory = ProCommon.Communal.CtrllerCategory.Camera;
            this.CtrllerBrand = brand;
            this.Number = camIndex;
            this.Name = camName;
            this.Type = ctrllerType.ToString();
            this.ID = brand.ToString() + "-" + ctrllerType.ToString() + "_" + camIndex.ToString("00");
        }

        /// <summary>
        /// 属性：相机使用标定方案的ID
        /// </summary>
        public string CalibrationID
        {
            set;
            get;
        }

        /// <summary>
        /// 属性:标定方案
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public ProVision.Communal.CalibrationSolution CalibrationSolution
        {
            get
            {
                try
                {
                    if (MothballCalibrationSolutionList.Count > 0)
                    {
                        for (int i = 0; i < MothballCalibrationSolutionList.Count; i++)
                        {
                            var itm = MothballCalibrationSolutionList[i];
                            if (itm.IsActive) //选择激活方案
                            {
                                return itm;
                            }
                        }

                        //未有激活方案，默认第一个方案
                        return this.MothballCalibrationSolutionList[0];

                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 属性：标定方案列表实体(用于实体删减+查询)
        /// [备用的标定方案]
        /// </summary>      
        public ProVision.Communal.CalibrationSolutionList MothballCalibrationSolutionList
        {
            set; get;
        }

        /// <summary>
        /// 属性:标定方案实体列表(用于数据绑定+查询)
        /// [所有标定方案实体的列表]
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public System.ComponentModel.BindingList<ProVision.Communal.CalibrationSolution> CalibrationSolutionBList
        {
            get
            {
                System.ComponentModel.BindingList<ProVision.Communal.CalibrationSolution> calList = new System.ComponentModel.BindingList<ProVision.Communal.CalibrationSolution>();
                for (int i = 0; i < this.MothballCalibrationSolutionList.Count; i++)
                {
                    calList.Add(this.MothballCalibrationSolutionList[i]);
                }
                return calList;
            }
        }


        /// <summary>
        /// 属性：相机序列号
        /// </summary>
        public string SerialNo
        {
            set;
            get;
        }

        /// <summary>
        /// 属性：相机的视频格式
        /// </summary>
        public string VideoFormat
        {
            set;
            get;
        }

        /// <summary>
        /// 属性：相机帧率(Frame Per Second)
        /// </summary>
        public float FPS
        {
            set;
            get;
        }

        /// <summary>
        /// 属性：曝光时间(微秒)
        /// </summary>
        public float ExposureTime
        {
            set;
            get;
        }

        /// <summary>
        /// 属性：增益
        /// </summary>
        public float Gain { set; get; }

        /// <summary>
        /// 属性:相机是否连接
        /// </summary>
        private bool _isConnected;
        [System.Xml.Serialization.XmlIgnore]
        public bool IsConnected
        {
            set
            {
                //不能直接赋值，否则一直触发属性值事件(虽然未改变)
                if (this._isConnected != value)
                {
                    this._isConnected = value;
                    //调用方法：通知属性值改变
                    this.NotifyPropertyChanged("IsConnected");
                }
            }
            get
            {
                return _isConnected;
            }
        }
    }

    [Serializable]
    public class CameraList : System.Collections.ICollection
    {
        public CameraList()
        {
            _list = new System.Collections.SortedList();
        }

        private System.Collections.SortedList _list;

        /// <summary>
        /// 方法：添加相机实体
        /// </summary>
        /// <param name="cam"></param>
        public void Add(Camera cam)
        {
            if (!_list.ContainsKey(cam.ID))
            {
                _list.Add(cam.ID, cam);
            }
        }

        /// <summary>
        /// 方法：删除相机实体
        /// </summary>
        /// <param name="cam"></param>
        public void Delete(Camera cam)
        {
            if (_list.ContainsKey(cam.ID))
            {
                _list.Remove(cam.ID);
            }
        }

        public void Clear()
        {
            if (_list != null)
                _list.Clear();
        }

        /// <summary>
        /// 索引器：返回相机列表中的实体
        /// </summary>
        /// <param name="indx"></param>
        /// <returns></returns>
        public Camera this[int indx]
        {
            get
            {
                Camera cam = null;
                if (_list.Count > 0 && indx < _list.Count)
                {
                    cam = (Camera)_list.GetByIndex(indx);
                }
                return cam;
            }
        }

        /// <summary>
        /// 索引器：返回相机列表中的实体
        /// </summary>
        /// <param name="camID"></param>
        /// <returns></returns>
        public Camera this[string camID]
        {
            get
            {
                Camera cam = null;
                if (_list.ContainsKey(camID))
                {
                    cam = (Camera)_list[camID];
                }
                return cam;
            }
        }

        /// <summary>
        /// 方法：相机列表从指定索引开始复制相机实体到给定的一维数组
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="indx"></param>
        public void CopyTo(Array arr, int indx)
        {
            _list.CopyTo(arr, indx);
        }

        /// <summary>
        /// 属性：返回相机列表中实体的数量
        /// </summary>
        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        /// <summary>
        /// 属性：是否同步
        /// </summary>
        public bool IsSynchronized
        {
            get { return false; }
        }


        /// <summary>
        /// 属性：SyncRoot
        /// </summary>
        public object SyncRoot
        {
            get { return this; }

        }

        /// <summary>
        /// 方法：获取枚举器
        /// </summary>
        /// <returns></returns>
        public System.Collections.IEnumerator GetEnumerator()
        {
            return _list.GetEnumerator();
        }

    }

    /// <summary>
    /// 相机采集模式
    /// </summary>
    public enum AcquisitionMode : uint
    {
        Continue = 0,
        SoftTrigger = 1,
        ExternalTrigger = 2
    }




}
