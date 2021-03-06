﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Common;
using Vision;

namespace Motion
{
    /// <summary>
    /// 过程数据，不需要存文件，但流程运行中会改变的数据 
    /// </summary>
    public class ProcessDataDef
    {

        /// <summary>
        /// 打磨点拍照
        /// </summary>
        public List<wPointF>[] wPointFs_PolishF = new List<wPointF>[] { new List<wPointF>(), new List<wPointF>() };
        public List<wPointF>[] wPointFs_PolishV = new List<wPointF>[] { new List<wPointF>(), new List<wPointF>() };
        
        /// <summary>
        /// 上锡点拍照
        /// </summary>
        public List<wPointF>[] wPointFs_SolderF = new List<wPointF>[] { new List<wPointF>(), new List<wPointF>() };
        public List<wPointF>[] wPointFs_SolderV = new List<wPointF>[] { new List<wPointF>(), new List<wPointF>() };

        /// <summary>
        /// 工作点
        /// </summary>
        public List<SolderPosdata>[] SolderList = new List<SolderPosdata>[] { new List<SolderPosdata>(), new List<SolderPosdata>() };
        public List<PolishPosdata>[] PolishList = new List<PolishPosdata>[] { new List<PolishPosdata>(), new List<PolishPosdata>() };

        public List<bool>[] SolderList_EnableF = new List<bool>[] { new List<bool>(), new List<bool>() };
        public List<bool>[] SolderList_EnableV = new List<bool>[] { new List<bool>(), new List<bool>() };


        public ProcessDataDef()
        {
            wPointFs_PolishF = new List<wPointF>[] { new List<wPointF>(), new List<wPointF>() };
            wPointFs_PolishV = new List<wPointF>[] { new List<wPointF>(), new List<wPointF>() };
            wPointFs_SolderF = new List<wPointF>[] { new List<wPointF>(), new List<wPointF>() };
            wPointFs_SolderV = new List<wPointF>[] { new List<wPointF>(), new List<wPointF>() };

            SolderList = new List<SolderPosdata>[] { new List<SolderPosdata>(), new List<SolderPosdata>() };
            PolishList = new List<PolishPosdata>[] { new List<PolishPosdata>(), new List<PolishPosdata>() };
        }

    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class PolishPosdata
    {
        public PointF4 pos { get; set; }
        public PolishDef polishDef { get; set; }
         
        public PolishPosdata()
        {
            pos = new PointF4();
            polishDef = new PolishDef();
        }

        public override string ToString()
        {
            return pos.ToString();
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public class SolderPosdata
    {
        public PointF4 pos { get; set; }
        public SolderDef solderDef { get; set; }
        public bool rinse { get; set; }

        public SolderPosdata()
        {
            pos = new PointF4();
            solderDef = new SolderDef();
            rinse = new bool();
        }

        public override string ToString()
        {
            return pos.ToString();
        }
    }

}
