using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SYMVLightDLL_CShare
{
    public class SYMVLightHeader
    {
        //ComPort Operation
        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_ComPort_Connect", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_ComPort_Connect();
        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_Disconnect", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_Disconnect();
        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_Set_PortNum_Config", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_Set_PortNum_Config(int PortNum);
        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_Get_PortNum_Config", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_Get_PortNum_Config(ref int PortNum);

        //Slave module oparation
        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_Slave_Init", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_Slave_Init(int SlaveIP);
        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Slave_ConnectSts", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Slave_ConnectSts();
        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_Slave_Connect", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_Slave_Connect(int SlaveIP);
        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_Set_SlaveIP", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_Set_SlaveIP(int SlaveIP, int NewIP);
        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_Get_SlaveIP", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_Get_SlaveIP(ref int SlaveIPIndex);
        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_Get_AllSlaveIP", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_Get_AllSlaveIP(int[] AllIP, ref int IPcount);

        //Flash Operation
        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_SaveParamToFlash", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_SaveParamToFlash(int SlaveIP);

        //Light Chanel Control
        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_Set_Intensity", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_Set_Intensity(int SlaveIP, int ChNum, int Intesity);
        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_Get_Intensity", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_Get_Intensity(int SlaveIP, int ChNum, ref int Intesity);
      
        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_SetChStsOnOff", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_SetChStsOnOff(int SlaveIP, int ChNum, int CHstatus);

        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_SetCHMode", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_SetCHMode(int SlaveIP, int ChNum, int Mode);
        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_GetCHMode", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_GetCHMode(int SlaveIP, int ChNum, ref int Mode);

        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_Set_TriggerDelay", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_Set_TriggerDelay(int SlaveIP, int ChNum, int StrobeTime);
        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_Get_TriggerDelay", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_Get_TriggerDelay(int SlaveIP, int ChNum, ref int StrobeTime);

        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_Set_StrobeDuration", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_Set_StrobeDuration(int SlaveIP, int ChNum, int HoldTime);
        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_Get_StrobeDuration", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_Get_StrobeDuration(int SlaveIP, int ChNum, ref int HoldTime);                      


        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_Set_CommMod", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_Set_CommMod(int SlaveIP, int CommMod);

        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_SelChn", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_SelChn(int SlaveIP, int ChNumSet);

        //Save & load Param
        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_LoadLightParam", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_LoadLightParam(int SlaveIP, System.IntPtr FilePath);
        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_SaveLightParam", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_SaveLightParam(int SlaveIP, System.IntPtr FilePath);

        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_Set_AllIntensity", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_Set_AllIntensity(int SlaveIP, int CH1Intesity, int CH12ntesity, int CH3Intesity, int CH4Intesity);


        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_Set_AllChStsOnOff", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_Set_AllChStsOnOff(int SlaveIP, int CHstatus);
        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_Get_AllChStsOnOff", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_Get_AllChStsOnOff(int SlaveIP, ref int pstatus);


        [DllImport("SYMVLight_D.dll", EntryPoint = "SY_MVD_Light_Set_Baudrate", CallingConvention = CallingConvention.StdCall)]
        public static extern int SY_MVD_Light_Set_Baudrate(int SlaveIP, int Baudrate_type);
    }
}
