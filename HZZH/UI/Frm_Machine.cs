using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common;
using Device;
using Logic;
using Motion;
using UI;

namespace HZZH.UI
{
    public partial class Frm_Machine : Form
    {
        BoardCtrllerManager movedriverZm;
        TeachingMechinePra TeachingMechine;

        public Frm_Machine()
        {
            InitializeComponent();
        }

        int xID = 0;
        int yID = 1;
        int zID = 2;
        int rID = 3;
        int tID = 4;

        int _id = 0;
        float SafeZ = 0;

        public Frm_Machine(BoardCtrllerManager movedriverZm, TeachingMechinePra TeachingMechinePra, UsingPlatformSelect usingPlatform)
        {
            InitializeComponent();


            this.movedriverZm = movedriverZm;
            this.TeachingMechine= TeachingMechinePra;
            if(usingPlatform == UsingPlatformSelect.Left)
            {
                this.xID = (int)AxisDef.AxX1;
                this.yID = (int)AxisDef.AxY1;
                this.zID = (int)AxisDef.AxZ1;
                this.rID = (int)AxisDef.AxR1;
                this.tID = (int)AxisDef.AxT1;
                _id = 0;
                SafeZ = FormMain.RunProcess.LogicData.slaverData.basics.Safe_ZL;
            }
            else
            {
                this.xID = (int)AxisDef.AxX2;
                this.yID = (int)AxisDef.AxY2;
                this.zID = (int)AxisDef.AxZ2;
                this.rID = (int)AxisDef.AxR2;
                this.tID = (int)AxisDef.AxT2;
                _id = 1;
                SafeZ = FormMain.RunProcess.LogicData.slaverData.basics.Safe_ZR;
            }
            Binding();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            TeachingMechine.ZeroPostion.X = movedriverZm.CurrentPos.FloatValue[xID];
            TeachingMechine.ZeroPostion.Y = movedriverZm.CurrentPos.FloatValue[yID];
            numericUpDown4.DataBindings["Value"].ReadValue();
            numericUpDown5.DataBindings["Value"].ReadValue();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TeachingMechine.ReversePostion.X = movedriverZm.CurrentPos.FloatValue[xID];
            TeachingMechine.ReversePostion.Y = movedriverZm.CurrentPos.FloatValue[yID];
            numericUpDown7.DataBindings["Value"].ReadValue();
            numericUpDown6.DataBindings["Value"].ReadValue();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            float x = (TeachingMechine.ZeroPostion.X + TeachingMechine.ReversePostion.X) / 2.0f;
            float y = (TeachingMechine.ZeroPostion.Y + TeachingMechine.ReversePostion.Y) / 2.0f;
            TeachingMechine.RotatePostion.X = x;
            TeachingMechine.RotatePostion.Y = y;

            TeachingMechine.Radius = (float)Math.Sqrt(Math.Pow((x - TeachingMechine.ZeroPostion.X), 2) + Math.Pow((y - TeachingMechine.ZeroPostion.Y), 2));
            
            double ang = Math.Atan(y / x);
            TeachingMechine.RotatePostionStartAngle = (float)(ang * 180 / Math.PI);

            numericUpDown9.DataBindings["Value"].ReadValue();
            numericUpDown8.DataBindings["Value"].ReadValue();
            numericUpDown1.DataBindings["Value"].ReadValue();


        }

        private void Binding()
        {
            
            Functions.SetBinding(numericUpDown4, "Value", TeachingMechine.ZeroPostion, "X");
            Functions.SetBinding(numericUpDown5, "Value", TeachingMechine.ZeroPostion, "Y");

            Functions.SetBinding(numericUpDown7, "Value", TeachingMechine.ReversePostion, "X");
            Functions.SetBinding(numericUpDown6, "Value", TeachingMechine.ReversePostion, "Y");

            Functions.SetBinding(numericUpDown9, "Value", TeachingMechine.RotatePostion, "X");
            Functions.SetBinding(numericUpDown8, "Value", TeachingMechine.RotatePostion, "Y");

            Functions.SetBinding(numericUpDown1, "Value", TeachingMechine, "Radius");


            Functions.SetBinding(numericUpDown3, "Value", TeachingMechine.CameraRotatePostion, "X");
            Functions.SetBinding(numericUpDown2, "Value", TeachingMechine.CameraRotatePostion, "Y");

            Functions.SetBinding(numericUpDown12, "Value", TeachingMechine.RotatePstionCameraSize, "X");
            Functions.SetBinding(numericUpDown11, "Value", TeachingMechine.RotatePstionCameraSize, "Y");


        }

        private void button8_Click(object sender, EventArgs e)
        {
            float X = TeachingMechine.ZeroPostion.X;
            float Y = TeachingMechine.ZeroPostion.Y;
            float R = 0;

            float x = 0;
            float y = 0;
            float aa = (float)(90 * Math.PI / 180);
            //FormMain.RunProcess.Transorm(Logic.UsingPlatformSelect.Right,TeachingMechine.CameraRotatePostion.X,TeachingMechine.CameraRotatePostion.Y,
            //    X, Y, R, aa, out x, out y);
            //while (!FormMain.RunProcess.LogicAPI.PlatformMove[_id].exe(xID, yID, zID, rID, tID, x, y, SafeZ, 90, 2, 0))
            //{
            //    Thread.Sleep(1);
            //}


            while (!FormMain.RunProcess.LogicAPI.PlatformMove[_id].exe(xID, yID, zID, rID, tID, X, Y, SafeZ, R, 2, 0))
            {
                Thread.Sleep(1);
            }
            Thread.Sleep(100);
            while (!(FormMain.RunProcess.LogicAPI.PlatformMove[_id].sta() && FormMain.RunProcess.LogicAPI.PlatformMove[_id].start != 1))
            {
                Thread.Sleep(1);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            float X = TeachingMechine.ReversePostion.X;
            float Y = TeachingMechine.ReversePostion.Y;
            float R = 180;
            //
            float x = 0;
            float y = 0;
            float aa = (float)(-90 * Math.PI /180 );
            //FormMain.RunProcess.Transorm(Logic.UsingPlatformSelect.Right, X, Y, R, aa, out x , out y);
            //while (!FormMain.RunProcess.LogicAPI.PlatformMove[_id].exe(xID, yID, zID, rID, tID, x, y, SafeZ, 90, 2, 0))
            //{
            //    Thread.Sleep(1);
            //}
            //
            while (!FormMain.RunProcess.LogicAPI.PlatformMove[_id].exe(xID, yID, zID, rID, tID, X, Y, SafeZ, R, 2, 0))
            {
                Thread.Sleep(1);
            }
            Thread.Sleep(100);
            while (!(FormMain.RunProcess.LogicAPI.PlatformMove[_id].sta() && FormMain.RunProcess.LogicAPI.PlatformMove[_id].start != 1))
            {
                Thread.Sleep(1);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TeachingMechine.CameraRotatePostion.X = movedriverZm.CurrentPos.FloatValue[xID];
            TeachingMechine.CameraRotatePostion.Y = movedriverZm.CurrentPos.FloatValue[yID];
            numericUpDown3.DataBindings["Value"].ReadValue();
            numericUpDown2.DataBindings["Value"].ReadValue();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            float X = TeachingMechine.CameraRotatePostion.X;
            float Y = TeachingMechine.CameraRotatePostion.Y;
            float R = 0;

            while (!FormMain.RunProcess.LogicAPI.PlatformMove[_id].exe(xID, yID, zID, rID, tID, X, Y, SafeZ, R, 2, 0))
            {
                Thread.Sleep(1);
            }
            Thread.Sleep(100);
            while (!(FormMain.RunProcess.LogicAPI.PlatformMove[_id].sta() && FormMain.RunProcess.LogicAPI.PlatformMove[_id].start != 1))
            {
                Thread.Sleep(1);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            float X = TeachingMechine.CameraRotatePostion.X - TeachingMechine.RotatePostion.X;
            float Y = TeachingMechine.CameraRotatePostion.Y - TeachingMechine.RotatePostion.Y;

            TeachingMechine.RotatePstionCameraSize.X = X;
            TeachingMechine.RotatePstionCameraSize.Y = Y;
            numericUpDown12.DataBindings["Value"].ReadValue();
            numericUpDown11.DataBindings["Value"].ReadValue();


        }
    }
}
