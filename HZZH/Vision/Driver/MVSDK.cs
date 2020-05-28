/*
 * ��ҵ���C# SDK��������
 * �������ϣ���ο�SDK�����ֲ�
 */

//BIG5 TRANS ALLOWED

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

//��������SDK֧��ͬʱ�򿪶��������øþ�����ֶ����
using CameraHandle = System.Int32;


namespace MVSDK
{

    //SDK�ӿڵķ���ֵ����������
    public enum CameraSdkStatus
    {
        CAMERA_STATUS_SUCCESS = 0,   // �����ɹ�
        CAMERA_STATUS_FAILED = -1,   // ����ʧ��
        CAMERA_STATUS_intERNAL_ERROR = -2,   // �ڲ�����
        CAMERA_STATUS_UNKNOW = -3,   // δ֪����
        CAMERA_STATUS_NOT_SUPPORTED = -4,   // ��֧�ָù���
        CAMERA_STATUS_NOT_INITIALIZED = -5,   // ��ʼ��δ���
        CAMERA_STATUS_PARAMETER_INVALID = -6,   // ������Ч
        CAMERA_STATUS_PARAMETER_OUT_OF_BOUND = -7,   // ����Խ��
        CAMERA_STATUS_UNENABLED = -8,   // δʹ��
        CAMERA_STATUS_USER_CANCEL = -9,   // �û��ֶ�ȡ���ˣ�����roi�����ȡ��������
        CAMERA_STATUS_PATH_NOT_FOUND = -10,  // ע�����û���ҵ���Ӧ��·��
        CAMERA_STATUS_SIZE_DISMATCH = -11,  // ���ͼ�����ݳ��ȺͶ���ĳߴ粻ƥ��
        CAMERA_STATUS_TIME_OUT = -12,  // ��ʱ����
        CAMERA_STATUS_IO_ERROR = -13,  // Ӳ��IO����
        CAMERA_STATUS_COMM_ERROR = -14,  // ͨѶ����
        CAMERA_STATUS_BUS_ERROR = -15,  // ���ߴ���
        CAMERA_STATUS_NO_DEVICE_FOUND = -16,  // û�з����豸
        CAMERA_STATUS_NO_LOGIC_DEVICE_FOUND = -17,  // δ�ҵ��߼��豸
        CAMERA_STATUS_DEVICE_IS_OPENED = -18,  // �豸�Ѿ���
        CAMERA_STATUS_DEVICE_IS_CLOSED = -19,  // �豸�Ѿ��ر�
        CAMERA_STATUS_DEVICE_VEDIO_CLOSED = -20,  // û�д��豸��Ƶ������¼����صĺ���ʱ����������Ƶû�д򿪣���ط��ظô���
        CAMERA_STATUS_NO_MEMORY = -21,  // û���㹻ϵͳ�ڴ�
        CAMERA_STATUS_FILE_CREATE_FAILED = -22,  // �����ļ�ʧ��
        CAMERA_STATUS_FILE_INVALID = -23,  // �ļ���ʽ��Ч
        CAMERA_STATUS_WRITE_PROTECTED = -24,  // д����������д
        CAMERA_STATUS_GRAB_FAILED = -25,  // ���ݲɼ�ʧ��
        CAMERA_STATUS_LOST_DATA = -26,  // ���ݶ�ʧ��������
        CAMERA_STATUS_EOF_ERROR = -27,  // δ���յ�֡������
        CAMERA_STATUS_BUSY = -28,  // ��æ(��һ�β������ڽ�����)���˴β������ܽ���
        CAMERA_STATUS_WAIT = -29,  // ��Ҫ�ȴ�(���в���������������)�������ٴγ���
        CAMERA_STATUS_IN_PROCESS = -30,  // ���ڽ��У��Ѿ���������
        CAMERA_STATUS_IIC_ERROR = -31,  // IIC�������
        CAMERA_STATUS_SPI_ERROR = -32,  // SPI�������
        CAMERA_STATUS_USB_CONTROL_ERROR = -33,  // USB���ƴ������
        CAMERA_STATUS_USB_BULK_ERROR = -34,  // USB BULK�������
        CAMERA_STATUS_SOCKET_INIT_ERROR = -35,  // ���紫���׼���ʼ��ʧ��
        CAMERA_STATUS_GIGE_FILTER_INIT_ERROR = -36,  // ��������ں˹���������ʼ��ʧ�ܣ������Ƿ���ȷ��װ���������������°�װ��
        CAMERA_STATUS_NET_SEND_ERROR = -37,  // �������ݷ��ʹ���
        CAMERA_STATUS_DEVICE_LOST = -38,  // ���������ʧȥ���ӣ�������ⳬʱ
        CAMERA_STATUS_DATA_RECV_LESS = -39,  // ���յ����ֽ������������ 
        CAMERA_STATUS_FUNCTION_LOAD_FAILED = -40,  // ���ļ��м��س���ʧ��
        CAMERA_STATUS_CRITICAL_FILE_LOST = -41,  // ����������������ļ���ʧ��
        CAMERA_STATUS_SENSOR_ID_DISMATCH = -42,  // �̼��ͳ���ƥ�䣬ԭ���������˴���Ĺ̼���
        CAMERA_STATUS_OUT_OF_RANGE = -43,  // ����������Ч��Χ��   
        CAMERA_STATUS_REGISTRY_ERROR = -44,  // ��װ����ע����������°�װ���򣬻������а�װĿ¼Setup/Installer.exe
        CAMERA_STATUS_ACCESS_DENY = -45,  // ��ֹ���ʡ�ָ������Ѿ�����������ռ��ʱ����������ʸ�������᷵�ظ�״̬��(һ��������ܱ��������ͬʱ����) 
        //AIA�ı�׼���ݵĴ�����
        CAMERA_AIA_PACKET_RESEND = 0x0100, //��֡��Ҫ�ش�
        CAMERA_AIA_NOT_IMPLEMENTED = 0x8001, //�豸��֧�ֵ�����
        CAMERA_AIA_INVALID_PARAMETER = 0x8002, //��������Ƿ�
        CAMERA_AIA_INVALID_ADDRESS = 0x8003, //���ɷ��ʵĵ�ַ
        CAMERA_AIA_WRITE_PROTECT = 0x8004, //���ʵĶ��󲻿�д
        CAMERA_AIA_BAD_ALIGNMENT = 0x8005, //���ʵĵ�ַû�а���Ҫ�����
        CAMERA_AIA_ACCESS_DENIED = 0x8006, //û�з���Ȩ��
        CAMERA_AIA_BUSY = 0x8007, //�������ڴ�����
        CAMERA_AIA_DEPRECATED = 0x8008, //0x8008-0x0800B  0x800F  ��ָ���Ѿ�����
        CAMERA_AIA_PACKET_UNAVAILABLE = 0x800C, //����Ч
        CAMERA_AIA_DATA_OVERRUN = 0x800D, //���������ͨ�����յ������ݱ���Ҫ�Ķ�
        CAMERA_AIA_INVALID_HEADER = 0x800E, //���ݰ�ͷ����ĳЩ������Э�鲻ƥ��
        CAMERA_AIA_PACKET_NOT_YET_AVAILABLE = 0x8010, //ͼ��ְ����ݻ�δ׼���ã������ڴ���ģʽ��Ӧ�ó�����ʳ�ʱ
        CAMERA_AIA_PACKET_AND_PREV_REMOVED_FROM_MEMORY = 0x8011, //��Ҫ���ʵķְ��Ѿ������ڡ��������ش�ʱ�����Ѿ����ڻ�������
        CAMERA_AIA_PACKET_REMOVED_FROM_MEMORY = 0x8012, //CAMERA_AIA_PACKET_AND_PREV_REMOVED_FROM_MEMORY
        CAMERA_AIA_NO_REF_TIME = 0x0813, //û�вο�ʱ��Դ��������ʱ��ͬ��������ִ��ʱ
        CAMERA_AIA_PACKET_TEMPORARILY_UNAVAILABLE = 0x0814, //�����ŵ��������⣬��ǰ�ְ���ʱ�����ã����Ժ���з���
        CAMERA_AIA_OVERFLOW = 0x0815, //�豸�����������ͨ���Ƕ�������
        CAMERA_AIA_ACTION_LATE = 0x0816, //����ִ���Ѿ�������Ч��ָ��ʱ��
        CAMERA_AIA_ERROR = 0x8FFF   //����
    }

    /*
       
    //tSdkResolutionRange�ṹ����SKIP�� BIN��RESAMPLEģʽ������ֵ
    MASK_2X2_HD   (1<<0)    //Ӳ��SKIP��BIN���ز��� 2X2
    MASK_3X3_HD   (1<<1)
    MASK_4X4_HD   (1<<2)
    MASK_5X5_HD   (1<<3)
    MASK_6X6_HD   (1<<4)
    MASK_7X7_HD   (1<<5)
    MASK_8X8_HD   (1<<6)
    MASK_9X9_HD   (1<<7)      
    MASK_10X10_HD   (1<<8)
    MASK_11X11_HD   (1<<9)
    MASK_12X12_HD   (1<<10)
    MASK_13X13_HD   (1<<11)
    MASK_14X14_HD   (1<<12)
    MASK_15X15_HD   (1<<13)
    MASK_16X16_HD   (1<<14)
    MASK_17X17_HD   (1<<15)
    MASK_2X2_SW   (1<<16) //Ӳ��SKIP��BIN���ز��� 2X2
    MASK_3X3_SW   (1<<17)
    MASK_4X4_SW   (1<<18)
    MASK_5X5_SW   (1<<19)
    MASK_6X6_SW   (1<<20)
    MASK_7X7_SW   (1<<21)
    MASK_8X8_SW   (1<<22)
    MASK_9X9_SW   (1<<23)     
    MASK_10X10_SW   (1<<24)
    MASK_11X11_SW   (1<<25)
    MASK_12X12_SW   (1<<26)
    MASK_13X13_SW   (1<<27)
    MASK_14X14_SW   (1<<28)
    MASK_15X15_SW   (1<<29)
    MASK_16X16_SW   (1<<30)
    MASK_17X17_SW   (1<<31) 
     */



    //ͼ����任�ķ�ʽ
    public enum emSdkLutMode
    {
        LUTMODE_PARAM_GEN = 0,//ͨ�����ڲ�����̬����LUT��
        LUTMODE_PRESET,     //ʹ��Ԥ���LUT��
        LUTMODE_USER_DEF    //ʹ���û��Զ����LUT��
    }

    //�������Ƶ������
    public enum emSdkRunMode
    {
        RUNMODE_PLAY = 0,    //����Ԥ��������ͼ�����ʾ�������������ڴ���ģʽ�����ȴ�����֡�ĵ�����
        RUNMODE_PAUSE,     //��ͣ������ͣ�����ͼ�������ͬʱҲ����ȥ����ͼ��
        RUNMODE_STOP       //ֹͣ�������������ʼ��������ʹ���ֹͣģʽ
    }

    //SDK�ڲ���ʾ�ӿڵ���ʾ��ʽ
    public enum emSdkDisplayMode
    {
        DISPLAYMODE_SCALE = 0,  //������ʾģʽ�����ŵ���ʾ�ؼ��ĳߴ�
        DISPLAYMODE_REAL      //1:1��ʾģʽ����ͼ��ߴ������ʾ�ؼ��ĳߴ�ʱ��ֻ��ʾ�ֲ�  
    }

    //¼��״̬
    public enum emSdkRecordMode
    {
        RECORD_STOP = 0,  //ֹͣ
        RECORD_START,     //¼����
        RECORD_PAUSE      //��ͣ
    }

    //ͼ��ľ������
    public enum emSdkMirrorDirection
    {
        MIRROR_DIRECTION_HORIZONTAL = 0,//ˮƽ����
        MIRROR_DIRECTION_VERTICAL       //��ֱ����
    }

    //�����Ƶ��֡��
    public enum emSdkFrameSpeed
    {
        FRAME_SPEED_LOW = 0,  //����ģʽ
        FRAME_SPEED_NORMAL,   //��ͨģʽ
        FRAME_SPEED_HIGH,     //����ģʽ(��Ҫ�ϸߵĴ������,���豸���������ʱ���֡�ʵ��ȶ�����Ӱ��)
        FRAME_SPEED_SUPER     //������ģʽ(��Ҫ�ϸߵĴ������,���豸���������ʱ���֡�ʵ��ȶ�����Ӱ��)
    }

    //�����ļ��ĸ�ʽ����
    public enum emSdkFileType
    {
        FILE_JPG = 1,//JPG
        FILE_BMP = 2,//BMP
        FILE_RAW = 4,//��������bayer��ʽ�ļ�,���ڲ�֧��bayer��ʽ���������޷�����Ϊ�ø�ʽ
        FILE_PNG = 8 //PNG
    }

    //����е�ͼ�񴫸����Ĺ���ģʽ
    public enum emSdkSnapMode
    {
        CONTINUATION = 0,//�����ɼ�ģʽ
        SOFT_TRIGGER,    //�������ģʽ�����������ָ��󣬴�������ʼ�ɼ�ָ��֡����ͼ�񣬲ɼ���ɺ�ֹͣ���
        EXTERNAL_TRIGGER //Ӳ������ģʽ�������յ��ⲿ�źţ���������ʼ�ɼ�ָ��֡����ͼ�񣬲ɼ���ɺ�ֹͣ���
    }

    //�Զ��ع�ʱ��Ƶ����Ƶ��
    public enum emSdkLightFrequency
    {
        LIGHT_FREQUENCY_50HZ = 0,//50HZ,һ��ĵƹⶼ��50HZ
        LIGHT_FREQUENCY_60HZ     //60HZ,��Ҫ��ָ��ʾ����
    }

    //��������ò�������ΪA,B,C,D 4����б��档
    public enum emSdkParameterTeam
    {
        PARAMETER_TEAM_DEFAULT = 0xff,
        PARAMETER_TEAM_A = 0,
        PARAMETER_TEAM_B = 1,
        PARAMETER_TEAM_C = 2,
        PARAMETER_TEAM_D = 3
    }

    //�����������ģʽ���������ط�Ϊ���ļ��ʹ��豸�������ַ�ʽ
    public enum emSdkParameterMode
    {
        PARAM_MODE_BY_MODEL = 0,  //��������ͺ������ļ��м��ز���������MV-U300
        PARAM_MODE_BY_NAME,       //�����豸�ǳ�(tSdkCameraDevInfo.acFriendlyName)���ļ��м��ز���������MV-U300,���ǳƿ��Զ���
        PARAM_MODE_BY_SN,         //�����豸��Ψһ���кŴ��ļ��м��ز��������к��ڳ���ʱ�Ѿ�д���豸��ÿ̨���ӵ�в�ͬ�����кš�
        PARAM_MODE_IN_DEVICE      //���豸�Ĺ�̬�洢���м��ز������������е��ͺŶ�֧�ִ�����ж�д�����飬��tSdkCameraCapbility.bParamInDevice����
    }

    //SDK���ɵ��������ҳ������ֵ
    public enum emSdkPropSheetMask
    {
        PROP_SHEET_INDEX_EXPOSURE = 0,
        PROP_SHEET_INDEX_ISP_COLOR,
        PROP_SHEET_INDEX_ISP_LUT,
        PROP_SHEET_INDEX_ISP_SHAPE,
        PROP_SHEET_INDEX_VIDEO_FORMAT,
        PROP_SHEET_INDEX_RESOLUTION,
        PROP_SHEET_INDEX_IO_CTRL,
        PROP_SHEET_INDEX_TRIGGER_SET,
        PROP_SHEET_INDEX_OVERLAY
    }

    //SDK���ɵ��������ҳ��Ļص���Ϣ����
    public enum emSdkPropSheetMsg
    {
        SHEET_MSG_LOAD_PARAM_DEFAULT = 0, //�������ָ���Ĭ�Ϻ󣬴�������Ϣ
        SHEET_MSG_LOAD_PARAM_GROUP,       //����ָ�������飬��������Ϣ
        SHEET_MSG_LOAD_PARAM_FROMFILE,    //��ָ���ļ����ز����󣬴�������Ϣ
        SHEET_MSG_SAVE_PARAM_GROUP        //��ǰ�����鱻����ʱ����������Ϣ
    }

    //���ӻ�ѡ��ο����ڵ�����
    public enum emSdkRefWintype
    {
        REF_WIN_AUTO_EXPOSURE = 0,
        REF_WIN_WHITE_BALANCE,
    }

    //������źſ��Ʒ�ʽ
    public enum emStrobeControl
    {
        STROBE_SYNC_WITH_TRIG_AUTO = 0,    //�ʹ����ź�ͬ������������������ع�ʱ���Զ�����STROBE�źš���ʱ����Ч���Կ�����(CameraSetStrobePolarity)��
        STROBE_SYNC_WITH_TRIG_MANUAL,      //�ʹ����ź�ͬ����������STROBE��ʱָ����ʱ���(CameraSetStrobeDelayTime)���ٳ���ָ��ʱ�������(CameraSetStrobePulseWidth)����Ч���Կ�����(CameraSetStrobePolarity)��
        STROBE_ALWAYS_HIGH,                //ʼ��Ϊ�ߣ�����STROBE�źŵ���������
        STROBE_ALWAYS_LOW                  //ʼ��Ϊ�ͣ�����STROBE�źŵ���������
    }

    //Ӳ���ⴥ�����ź�����
    public enum emExtTrigSignal
    {
        EXT_TRIG_LEADING_EDGE = 0,     //�����ش�����Ĭ��Ϊ�÷�ʽ
        EXT_TRIG_TRAILING_EDGE,        //�½��ش���  
        EXT_TRIG_HIGH_LEVEL,           //�ߵ�ƽ����,��ƽ��Ⱦ����ع�ʱ�䣬�������ͺŵ����֧�ֵ�ƽ������ʽ��
        EXT_TRIG_LOW_LEVEL             //�͵�ƽ����,
    }

    //Ӳ���ⴥ��ʱ�Ŀ��ŷ�ʽ
    public enum emExtTrigShutterMode
    {
        EXT_TRIG_EXP_STANDARD = 0,     //��׼��ʽ��Ĭ��Ϊ�÷�ʽ��
        EXT_TRIG_EXP_GRR,              //ȫ�ָ�λ��ʽ�����ֹ������ŵ�CMOS�ͺŵ����֧�ָ÷�ʽ������ⲿ��е���ţ����Դﵽȫ�ֿ��ŵ�Ч�����ʺ��ĸ����˶�������
    }

    public enum emImageFormat
    {
        CAMERA_MEDIA_TYPE_MONO = 0x01000000,
        CAMERA_MEDIA_TYPE_RGB = 0x02000000,
        CAMERA_MEDIA_TYPE_COLOR = 0x02000000,
        CAMERA_MEDIA_TYPE_OCCUPY1BIT = 0x00010000,
        CAMERA_MEDIA_TYPE_OCCUPY2BIT = 0x00020000,
        CAMERA_MEDIA_TYPE_OCCUPY4BIT = 0x00040000,
        CAMERA_MEDIA_TYPE_OCCUPY8BIT = 0x00080000,
        CAMERA_MEDIA_TYPE_OCCUPY10BIT = 0x000A0000,
        CAMERA_MEDIA_TYPE_OCCUPY12BIT = 0x000C0000,
        CAMERA_MEDIA_TYPE_OCCUPY16BIT = 0x00100000,
        CAMERA_MEDIA_TYPE_OCCUPY24BIT = 0x00180000,
        CAMERA_MEDIA_TYPE_OCCUPY32BIT = 0x00200000,
        CAMERA_MEDIA_TYPE_OCCUPY36BIT = 0x00240000,
        CAMERA_MEDIA_TYPE_OCCUPY48BIT = 0x00300000,
        CAMERA_MEDIA_TYPE_EFFECTIVE_PIXEL_SIZE_MASK = 0x00FF0000,
        CAMERA_MEDIA_TYPE_EFFECTIVE_PIXEL_SIZE_SHIFT = 16,


        CAMERA_MEDIA_TYPE_ID_MASK = 0x0000FFFF,
        CAMERA_MEDIA_TYPE_COUNT = 0x46,

        /*mono*/
        CAMERA_MEDIA_TYPE_MONO1P = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY1BIT | 0x0037),
        CAMERA_MEDIA_TYPE_MONO2P = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY2BIT | 0x0038),
        CAMERA_MEDIA_TYPE_MONO4P = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY4BIT | 0x0039),
        CAMERA_MEDIA_TYPE_MONO8 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY8BIT | 0x0001),
        CAMERA_MEDIA_TYPE_MONO8S = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY8BIT | 0x0002),
        CAMERA_MEDIA_TYPE_MONO10 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0003),
        CAMERA_MEDIA_TYPE_MONO10_PACKED = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x0004),
        CAMERA_MEDIA_TYPE_MONO12 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0005),
        CAMERA_MEDIA_TYPE_MONO12_PACKED = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x0006),
        CAMERA_MEDIA_TYPE_MONO14 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0025),
        CAMERA_MEDIA_TYPE_MONO16 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0007),

        /*Bayer */
        CAMERA_MEDIA_TYPE_BAYGR8 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY8BIT | 0x0008),
        CAMERA_MEDIA_TYPE_BAYRG8 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY8BIT | 0x0009),
        CAMERA_MEDIA_TYPE_BAYGB8 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY8BIT | 0x000A),
        CAMERA_MEDIA_TYPE_BAYBG8 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY8BIT | 0x000B),

        CAMERA_MEDIA_TYPE_BAYGR10_MIPI = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY10BIT | 0x0026),
        CAMERA_MEDIA_TYPE_BAYRG10_MIPI = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY10BIT | 0x0027),
        CAMERA_MEDIA_TYPE_BAYGB10_MIPI = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY10BIT | 0x0028),
        CAMERA_MEDIA_TYPE_BAYBG10_MIPI = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY10BIT | 0x0029),


        CAMERA_MEDIA_TYPE_BAYGR10 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x000C),
        CAMERA_MEDIA_TYPE_BAYRG10 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x000D),
        CAMERA_MEDIA_TYPE_BAYGB10 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x000E),
        CAMERA_MEDIA_TYPE_BAYBG10 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x000F),

        CAMERA_MEDIA_TYPE_BAYGR12 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0010),
        CAMERA_MEDIA_TYPE_BAYRG12 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0011),
        CAMERA_MEDIA_TYPE_BAYGB12 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0012),
        CAMERA_MEDIA_TYPE_BAYBG12 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0013),


        CAMERA_MEDIA_TYPE_BAYGR10_PACKED = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x0026),
        CAMERA_MEDIA_TYPE_BAYRG10_PACKED = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x0027),
        CAMERA_MEDIA_TYPE_BAYGB10_PACKED = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x0028),
        CAMERA_MEDIA_TYPE_BAYBG10_PACKED = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x0029),

        CAMERA_MEDIA_TYPE_BAYGR12_PACKED = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x002A),
        CAMERA_MEDIA_TYPE_BAYRG12_PACKED = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x002B),
        CAMERA_MEDIA_TYPE_BAYGB12_PACKED = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x002C),
        CAMERA_MEDIA_TYPE_BAYBG12_PACKED = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x002D),

        CAMERA_MEDIA_TYPE_BAYGR16 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x002E),
        CAMERA_MEDIA_TYPE_BAYRG16 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x002F),
        CAMERA_MEDIA_TYPE_BAYGB16 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0030),
        CAMERA_MEDIA_TYPE_BAYBG16 = (CAMERA_MEDIA_TYPE_MONO | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0031),

        /*RGB */
        CAMERA_MEDIA_TYPE_RGB8 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY24BIT | 0x0014),
        CAMERA_MEDIA_TYPE_BGR8 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY24BIT | 0x0015),
        CAMERA_MEDIA_TYPE_RGBA8 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY32BIT | 0x0016),
        CAMERA_MEDIA_TYPE_BGRA8 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY32BIT | 0x0017),
        CAMERA_MEDIA_TYPE_RGB10 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY48BIT | 0x0018),
        CAMERA_MEDIA_TYPE_BGR10 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY48BIT | 0x0019),
        CAMERA_MEDIA_TYPE_RGB12 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY48BIT | 0x001A),
        CAMERA_MEDIA_TYPE_BGR12 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY48BIT | 0x001B),
        CAMERA_MEDIA_TYPE_RGB16 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY48BIT | 0x0033),
        CAMERA_MEDIA_TYPE_RGB10V1_PACKED = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY32BIT | 0x001C),
        CAMERA_MEDIA_TYPE_RGB10P32 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY32BIT | 0x001D),
        CAMERA_MEDIA_TYPE_RGB12V1_PACKED = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY36BIT | 0X0034),
        CAMERA_MEDIA_TYPE_RGB565P = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0035),
        CAMERA_MEDIA_TYPE_BGR565P = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0X0036),

        /*YUV and YCbCr*/
        CAMERA_MEDIA_TYPE_YUV411_8_UYYVYY = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x001E),
        CAMERA_MEDIA_TYPE_YUV422_8_UYVY = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x001F),
        CAMERA_MEDIA_TYPE_YUV422_8 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0032),
        CAMERA_MEDIA_TYPE_YUV8_UYV = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY24BIT | 0x0020),
        CAMERA_MEDIA_TYPE_YCBCR8_CBYCR = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY24BIT | 0x003A),
        //CAMERA_MEDIA_TYPE_YCBCR422_8 : YYYYCbCrCbCr
        CAMERA_MEDIA_TYPE_YCBCR422_8 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x003B),
        CAMERA_MEDIA_TYPE_YCBCR422_8_CBYCRY = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0043),
        CAMERA_MEDIA_TYPE_YCBCR411_8_CBYYCRYY = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x003C),
        CAMERA_MEDIA_TYPE_YCBCR601_8_CBYCR = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY24BIT | 0x003D),
        CAMERA_MEDIA_TYPE_YCBCR601_422_8 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x003E),
        CAMERA_MEDIA_TYPE_YCBCR601_422_8_CBYCRY = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0044),
        CAMERA_MEDIA_TYPE_YCBCR601_411_8_CBYYCRYY = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x003F),
        CAMERA_MEDIA_TYPE_YCBCR709_8_CBYCR = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY24BIT | 0x0040),
        CAMERA_MEDIA_TYPE_YCBCR709_422_8 = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0041),
        CAMERA_MEDIA_TYPE_YCBCR709_422_8_CBYCRY = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY16BIT | 0x0045),
        CAMERA_MEDIA_TYPE_YCBCR709_411_8_CBYYCRYY = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY12BIT | 0x0042),

        /*RGB Planar */
        CAMERA_MEDIA_TYPE_RGB8_PLANAR = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY24BIT | 0x0021),
        CAMERA_MEDIA_TYPE_RGB10_PLANAR = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY48BIT | 0x0022),
        CAMERA_MEDIA_TYPE_RGB12_PLANAR = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY48BIT | 0x0023),
        CAMERA_MEDIA_TYPE_RGB16_PLANAR = (CAMERA_MEDIA_TYPE_COLOR | CAMERA_MEDIA_TYPE_OCCUPY48BIT | 0x0024),
    }




    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPFILEHEADER
    {
        public ushort bfType;
        public uint bfSize;
        public ushort bfReserved1;
        public ushort bfReserved2;
        public uint bfOffBits;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPINFOHEADER
    {
        public uint biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public uint biCompression;
        public uint biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RGBQUAD
    {
        public byte rgbBlue;
        public byte rgbGreen;
        public byte rgbRed;
        public byte rgbReserved;
    }

    //������豸��Ϣ��ֻ����Ϣ�������޸�
    public struct tSdkCameraDevInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acProductSeries; // ��Ʒϵ��
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acProductName;    // ��Ʒ����
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acFriendlyName;   // �ǳƣ����#��������������
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acLinkName;       // �豸�������������ڲ�ʹ��
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acDriverVersion;  // �����汾
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acSensorType;     // sensor����
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acPortType;       // �ӿ�����  
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acSn;             // ��ƷΨһ���к�
        public uint uInstance;        // ���ͺ�����ڸõ����ϵ�ʵ�������ţ���������ͬ�ͺŶ����

    }

    //����ķֱ����趨��Χ
    public struct tSdkResolutionRange
    {
        public int iHeightMax;            //ͼ�����߶�
        public int iHeightMin;            //ͼ����С�߶�
        public int iWidthMax;             //ͼ�������
        public int iWidthMin;             //ͼ����С���
        public int uSkipModeMask;         //SKIPģʽ���룬Ϊ0����ʾ��֧��SKIP ��bit0Ϊ1,��ʾ֧��SKIP 2x2 ;bit1Ϊ1����ʾ֧��SKIP 3x3....
        public int uBinSumModeMask;       //BIN(���)ģʽ���룬Ϊ0����ʾ��֧��BIN ��bit0Ϊ1,��ʾ֧��BIN 2x2 ;bit1Ϊ1����ʾ֧��BIN 3x3....
        public int uBinAverageModeMask;   //BIN(���ֵ)ģʽ���룬Ϊ0����ʾ��֧��BIN ��bit0Ϊ1,��ʾ֧��BIN 2x2 ;bit1Ϊ1����ʾ֧��BIN 3x3....
        public int uResampleMask;         //Ӳ���ز���������
    }

    //����ķֱ�������
    public struct tSdkImageResolution
    {
        public int iIndex;               // �����ţ�[0,N]��ʾԤ��ķֱ���(N ΪԤ��ֱ��ʵ���������һ�㲻����20),OXFF ��ʾ�Զ���ֱ���(ROI)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acDescription;        // �÷ֱ��ʵ�������Ϣ����Ԥ��ֱ���ʱ����Ϣ��Ч���Զ���ֱ��ʿɺ��Ը���Ϣ
        public uint uBinSumMode;          // BIN(���)��ģʽ,��Χ���ܳ���tSdkResolutionRange��uBinSumModeMask
        public uint uBinAverageMode;      // BIN(���ֵ)��ģʽ,��Χ���ܳ���tSdkResolutionRange��uBinAverageModeMask
        public uint uSkipMode;            // �Ƿ�SKIP�ĳߴ磬Ϊ0��ʾ��ֹSKIPģʽ����Χ���ܳ���tSdkResolutionRange��uSkipModeMask
        public uint uResampleMask;        // Ӳ���ز���������
        public int iHOffsetFOV;        // �ɼ��ӳ������Sensor����ӳ����ϽǵĴ�ֱƫ��
        public int iVOffsetFOV;        // �ɼ��ӳ������Sensor����ӳ����Ͻǵ�ˮƽƫ��
        public int iWidthFOV;          // �ɼ��ӳ��Ŀ�� 
        public int iHeightFOV;         // �ɼ��ӳ��ĸ߶�
        public int iWidth;             // ������������ͼ��Ŀ��
        public int iHeight;            // ������������ͼ��ĸ߶�
        public int iWidthZoomHd;       // Ӳ�����ŵĿ��,����Ҫ���д˲����ķֱ��ʣ��˱�������Ϊ0.
        public int iHeightZoomHd;      // Ӳ�����ŵĸ߶�,����Ҫ���д˲����ķֱ��ʣ��˱�������Ϊ0.
        public int iWidthZoomSw;       // ������ŵĿ��,����Ҫ���д˲����ķֱ��ʣ��˱�������Ϊ0.
        public int iHeightZoomSw;      // ������ŵĸ߶�,����Ҫ���д˲����ķֱ��ʣ��˱�������Ϊ0.
    }

    //�����ƽ��ģʽ������Ϣ
    public struct tSdkColorTemperatureDes
    {
        public int iIndex;              // ģʽ������
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acDescription;    // ������Ϣ
    }

    //���֡��������Ϣ
    public struct tSdkFrameSpeed
    {
        public int iIndex;            // ֡�������ţ�һ��0��Ӧ�ڵ���ģʽ��1��Ӧ����ͨģʽ��2��Ӧ�ڸ���ģʽ      
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acDescription;  // ������Ϣ      
    }

    //����ع⹦�ܷ�Χ����
    public struct tSdkExpose
    {
        public uint uiTargetMin;       //�Զ��ع�����Ŀ����Сֵ     
        public uint uiTargetMax;       //�Զ��ع�����Ŀ�����ֵ         
        public uint uiAnalogGainMin;   //ģ���������Сֵ����λΪfAnalogGainStep�ж���      
        public uint uiAnalogGainMax;   //ģ����������ֵ����λΪfAnalogGainStep�ж���        
        public float fAnalogGainStep;   //ģ������ÿ����1����Ӧ�����ӵķŴ��������磬uiAnalogGainMinһ��Ϊ16��fAnalogGainStepһ��Ϊ0.125����ô��С�Ŵ�������16*0.125 = 2��
        public uint uiExposeTimeMin;   //�ֶ�ģʽ�£��ع�ʱ�����Сֵ����λ:�С�����CameraGetExposureLineTime���Ի��һ�ж�Ӧ��ʱ��(΢��),�Ӷ��õ���֡���ع�ʱ��    
        public uint uiExposeTimeMax;   //�ֶ�ģʽ�£��ع�ʱ������ֵ����λ:��        
    }

    //����ģʽ����
    public struct tSdkTrigger
    {
        public int iIndex;             //ģʽ������      
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acDescription;      //��ģʽ��������Ϣ    
    }

    //����ְ���С����(��Ҫ��������������Ч)
    public struct tSdkPackLength
    {
        public int iIndex;        //�ְ���С������      
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acDescription; //��Ӧ��������Ϣ     
        public uint iPackSize;
    }

    //Ԥ���LUT������
    public struct tSdkPresetLut
    {
        public int iIndex;             //���     
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acDescription;    //������Ϣ
    }

    //AE�㷨����
    public struct tSdkAeAlgorithm
    {
        public int iIndex;                 //���   
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acDescription;        //������Ϣ
    }

    //RAWתRGB�㷨����
    public struct tSdkBayerDecodeAlgorithm
    {
        public int iIndex;                 //���  
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acDescription;        //������Ϣ
    }

    //֡��ͳ����Ϣ
    public struct tSdkFrameStatistic
    {
        public int iTotal;        //��ǰ�ɼ�����֡������������֡��
        public int iCapture;      //��ǰ�ɼ�����Ч֡������    
        public int iLost;         //��ǰ��֡������    
    }

    //��������ͼ�����ݸ�ʽ
    public struct tSdkMediaType
    {
        public int iIndex;                 //��ʽ������
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] acDescription;          //������Ϣ
        public uint iMediaType;             //��Ӧ��ͼ���ʽ���룬��CAMERA_MEDIA_TYPE_BAYGR8���ڱ��ļ����ж��塣
    }

    //٤����趨��Χ
    public struct tGammaRange
    {
        public int iMin;       //��Сֵ
        public int iMax;       //���ֵ
    }

    //�Աȶȵ��趨��Χ
    public struct tContrastRange
    {
        public int iMin;    //��Сֵ
        public int iMax;    //���ֵ
    }

    //RGB��ͨ������������趨��Χ
    public struct tRgbGainRange
    {
        public int iRGainMin;   //��ɫ�������Сֵ
        public int iRGainMax;   //��ɫ��������ֵ
        public int iGGainMin;   //��ɫ�������Сֵ
        public int iGGainMax;   //��ɫ��������ֵ
        public int iBGainMin;   //��ɫ�������Сֵ
        public int iBGainMax;   //��ɫ��������ֵ
    }

    //���Ͷ��趨�ķ�Χ
    public struct tSaturationRange
    {
        public int iMin;    //��Сֵ
        public int iMax;    //���ֵ
    }

    //�񻯵��趨��Χ
    public struct tSharpnessRange
    {
        public int iMin;    //��Сֵ
        public int iMax;    //���ֵ
    }

    //ISPģ���ʹ����Ϣ
    public struct tSdkIspCapacity
    {
        public uint bMonoSensor;        //��ʾ���ͺ�����Ƿ�Ϊ�ڰ����,����Ǻڰ����������ɫ��صĹ��ܶ��޷�����
        public uint bWbOnce;            //��ʾ���ͺ�����Ƿ�֧���ֶ���ƽ�⹦��  
        public uint bAutoWb;            //��ʾ���ͺ�����Ƿ�֧���Զ���ƽ�⹦��
        public uint bAutoExposure;      //��ʾ���ͺ�����Ƿ�֧���Զ��ع⹦��
        public uint bManualExposure;    //��ʾ���ͺ�����Ƿ�֧���ֶ��ع⹦��
        public uint bAntiFlick;         //��ʾ���ͺ�����Ƿ�֧�ֿ�Ƶ������
        public uint bDeviceIsp;         //��ʾ���ͺ�����Ƿ�֧��Ӳ��ISP����
        public uint bForceUseDeviceIsp; //bDeviceIsp��bForceUseDeviceIspͬʱΪTRUEʱ����ʾǿ��ֻ��Ӳ��ISP������ȡ����
        public uint bZoomHD;            //���Ӳ���Ƿ�֧��ͼ���������(ֻ������С)��
    }

    /* �������ϵ��豸������Ϣ����Щ��Ϣ�������ڶ�̬����UI */
    public struct tSdkCameraCapbility
    {
        public IntPtr pTriggerDesc;
        public int iTriggerDesc;           //����ģʽ�ĸ�������pTriggerDesc����Ĵ�С

        public IntPtr pImageSizeDesc;         //Ԥ��ֱ���ѡ��
        public int iImageSizeDesc;         //Ԥ��ֱ��ʵĸ�������pImageSizeDesc����Ĵ�С 

        public IntPtr pClrTempDesc;           //Ԥ��ɫ��ģʽ�����ڰ�ƽ��
        public int iClrTempDesc;

        public IntPtr pMediaTypeDesc;         //������ͼ���ʽ
        public int iMediaTypdeDesc;        //������ͼ���ʽ�������������pMediaTypeDesc����Ĵ�С��

        public IntPtr pFrameSpeedDesc;        //�ɵ���֡�����ͣ���Ӧ��������ͨ ���� �ͳ��������ٶ�����
        public int iFrameSpeedDesc;        //�ɵ���֡�����͵ĸ�������pFrameSpeedDesc����Ĵ�С��

        public IntPtr pPackLenDesc;           //��������ȣ�һ�����������豸
        public int iPackLenDesc;           //�ɹ�ѡ��Ĵ���ְ����ȵĸ�������pPackLenDesc����Ĵ�С�� 

        public int iOutputIoCounts;        //�ɱ�����IO�ĸ���
        public int iInputIoCounts;         //�ɱ������IO�ĸ���

        public IntPtr pPresetLutDesc;         //���Ԥ���LUT��
        public int iPresetLut;             //���Ԥ���LUT��ĸ�������pPresetLutDesc����Ĵ�С

        public int iUserDataMaxLen;        //ָʾ����������ڱ����û�����������󳤶ȡ�Ϊ0��ʾ�ޡ�
        public uint bParamInDevice;         //ָʾ���豸�Ƿ�֧�ִ��豸�ж�д�����顣1Ϊ֧�֣�0��֧�֡�

        public IntPtr pAeAlmSwDesc;           //����Զ��ع��㷨����
        public int iAeAlmSwDesc;           //����Զ��ع��㷨����

        public IntPtr pAeAlmHdDesc;           //Ӳ���Զ��ع��㷨������ΪNULL��ʾ��֧��Ӳ���Զ��ع�
        public int iAeAlmHdDesc;           //Ӳ���Զ��ع��㷨������Ϊ0��ʾ��֧��Ӳ���Զ��ع�

        public IntPtr pBayerDecAlmSwDesc;     //���Bayerת��ΪRGB���ݵ��㷨����
        public int iBayerDecAlmSwDesc;     //���Bayerת��ΪRGB���ݵ��㷨����

        public IntPtr pBayerDecAlmHdDesc;     //Ӳ��Bayerת��ΪRGB���ݵ��㷨������ΪNULL��ʾ��֧��
        public int iBayerDecAlmHdDesc;     //Ӳ��Bayerת��ΪRGB���ݵ��㷨������Ϊ0��ʾ��֧��

        /* ͼ������ĵ��ڷ�Χ����,���ڶ�̬����UI*/
        public tSdkExpose sExposeDesc;      //�ع�ķ�Χֵ
        public tSdkResolutionRange sResolutionRange; //�ֱ��ʷ�Χ����  
        public tRgbGainRange sRgbGainRange;    //ͼ���������淶Χ����  
        public tSaturationRange sSaturationRange; //���Ͷȷ�Χ����  
        public tGammaRange sGammaRange;      //٤��Χ����  
        public tContrastRange sContrastRange;   //�Աȶȷ�Χ����  
        public tSharpnessRange sSharpnessRange;  //�񻯷�Χ����  
        public tSdkIspCapacity sIspCapacity;     //ISP��������

    }

    //ͼ��֡ͷ��Ϣ
    public struct tSdkFrameHead
    {
        public uint uiMediaType;       // ͼ���ʽ,Image Format
        public uint uBytes;            // ͼ�������ֽ���,Total bytes
        public int iWidth;            // ��� Image height
        public int iHeight;           // �߶� Image width
        public int iWidthZoomSw;      // ������ŵĿ��,����Ҫ��������ü���ͼ�񣬴˱�������Ϊ0.
        public int iHeightZoomSw;     // ������ŵĸ߶�,����Ҫ��������ü���ͼ�񣬴˱�������Ϊ0.
        public uint bIsTrigger;        // ָʾ�Ƿ�Ϊ����֡ is trigger 
        public uint uiTimeStamp;       // ��֡�Ĳɼ�ʱ�䣬��λ0.1���� 
        public uint uiExpTime;         // ��ǰͼ����ع�ֵ����λΪ΢��us
        public float fAnalogGain;       // ��ǰͼ���ģ�����汶��
        public int iGamma;            // ��֡ͼ���٤���趨ֵ������LUTģʽΪ��̬��������ʱ��Ч������ģʽ��Ϊ-1
        public int iContrast;         // ��֡ͼ��ĶԱȶ��趨ֵ������LUTģʽΪ��̬��������ʱ��Ч������ģʽ��Ϊ-1
        public int iSaturation;       // ��֡ͼ��ı��Ͷ��趨ֵ�����ںڰ���������壬Ϊ0
        public float fRgain;            // ��֡ͼ����ĺ�ɫ�������汶�������ںڰ���������壬Ϊ1
        public float fGgain;            // ��֡ͼ�������ɫ�������汶�������ںڰ���������壬Ϊ1
        public float fBgain;            // ��֡ͼ�������ɫ�������汶�������ںڰ���������壬Ϊ1
    }

    //ͼ��֡����
    public struct tSdkFrame
    {
        public tSdkFrameHead head;        //֡ͷ
        public IntPtr pBuffer;     //������
    }

    //ͼ�񲶻�Ļص���������
    public delegate void CAMERA_SNAP_PROC(CameraHandle hCamera, IntPtr pFrameBuffer, ref tSdkFrameHead pFrameHead, IntPtr pContext);

    //SDK���ɵ��������ҳ�����Ϣ�ص���������
    public delegate void CAMERA_PAGE_MSG_PROC(CameraHandle hCamera, uint MSG, uint uParam, IntPtr pContext);

    // �������״̬�ص�
	// MSGȡֵ:
	//		0: ������ӶϿ�
	//		1: ������ӻָ�
	// USB���uParamȡֵ��
	//		δ����
	// �������uParamȡֵ��
	//		��MSG=0ʱ��δ����
	//		��MSG=1ʱ��
	//			0���ϴε���ԭ������ͨѶʧ��
	//			1���ϴε���ԭ���������
	public delegate void CAMERA_CONNECTION_STATUS_CALLBACK(CameraHandle hCamera, uint MSG,uint uParam,IntPtr pContext);


    // Grabberͳ����Ϣ
    public struct tSdkGrabberStat
    {
        public int Width, Height;	// ֡ͼ���С
        public int Disp;			// ��ʾ֡����
        public int Capture;		    // �ɼ�����Ч֡������
        public int Lost;			// ��֡������
        public int Error;			// ��֡������
        public float DispFps;		// ��ʾ֡��
        public float CapFps;		// ����֡��
    }

    // Grabberͼ�񲶻�Ļص���������
    public delegate void pfnCameraGrabberFrameCallback(
        IntPtr Grabber,
        IntPtr pFrameBuffer,
        ref tSdkFrameHead pFrameHead,
        IntPtr Context);

    // Grabber֡������������
    public delegate int pfnCameraGrabberFrameListener(
        IntPtr Grabber,
        int Phase,
        IntPtr pFrameBuffer,
        ref tSdkFrameHead pFrameHead,
        IntPtr Context);

    // Grabber�첽ץͼ�Ļص���������
    public delegate void pfnCameraGrabberSaveImageComplete(
        IntPtr Grabber,
        IntPtr Image,	// ��Ҫ����CameraImage_Destroy�ͷ�
        CameraSdkStatus Status,
        IntPtr Context);


    static public class MvApi
    {
        public static byte[] StringToCStr(string str)
        { 
            return Encoding.Default.GetBytes(str.Insert(str.Length, "\0"));
        }

        public static string CStrToString(byte[] cstr)
        {
            String str = Encoding.Default.GetString(cstr);
            return str.Substring(0, str.IndexOf('\0'));
        }

        /******************************************************/
        // ������   : CameraSdkInit
        // �������� : ���SDK��ʼ�����ڵ����κ�SDK�����ӿ�ǰ������
        //        �ȵ��øýӿڽ��г�ʼ�����ú�����������������
        //        �ڼ�ֻ��Ҫ����һ�Ρ�   
        // ����     : iLanguageSel ����ѡ��SDK�ڲ���ʾ��Ϣ�ͽ��������,
        //               0:��ʾӢ��,1:��ʾ���ġ�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSdkInit(
            int iLanguageSel
        );

        /******************************************************/
        // ������   : CameraEnumerateDevice
        // �������� : ö���豸���������豸�б��ڵ���CameraInit
        //        ֮ǰ��������øú���������豸����Ϣ��    
        // ����     : pCameraList �豸�б�����ָ�롣
        //             piNums        �豸�ĸ���ָ�룬����ʱ����pCameraList
        //                            �����Ԫ�ظ�������������ʱ������ʵ���ҵ����豸������
        //              ע�⣬piNumsָ���ֵ�����ʼ�����Ҳ�����pCameraList����Ԫ�ظ�����
        //              �����п�������ڴ������
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraEnumerateDevice(
            IntPtr pCameraList,
            ref int piNums
        );

        public static CameraSdkStatus CameraEnumerateDevice(IntPtr pCameraList, ref int piNums) 
        {
            return _CameraEnumerateDevice(pCameraList, ref piNums);
        }

        public static CameraSdkStatus CameraEnumerateDevice(out tSdkCameraDevInfo[] CameraList)
        {
            int CameraCount = 256;
            CameraSdkStatus status;
            IntPtr ptr;

            CameraList = null;

            ptr = Marshal.AllocHGlobal(Marshal.SizeOf(new tSdkCameraDevInfo()) * CameraCount);
            status = CameraEnumerateDevice(ptr, ref CameraCount);
            if (status == CameraSdkStatus.CAMERA_STATUS_SUCCESS && CameraCount > 0)
            {
                CameraList = new tSdkCameraDevInfo[CameraCount];
                for (int i = 0; i < CameraCount; ++i)
                {
                    CameraList[i] = (tSdkCameraDevInfo)Marshal.PtrToStructure((IntPtr)((Int64)ptr + i * Marshal.SizeOf(new tSdkCameraDevInfo())), typeof(tSdkCameraDevInfo));
                }
            }

            Marshal.FreeHGlobal(ptr);
            return status;
        }

        /******************************************************/
        // ������   : CameraInit
        // �������� : �����ʼ������ʼ���ɹ��󣬲��ܵ����κ�����
        //        �����صĲ����ӿڡ�    
        // ����     : pCameraInfo    ��������豸������Ϣ����CameraEnumerateDevice
        //               ������á� 
        //            iParamLoadMode  �����ʼ��ʱʹ�õĲ������ط�ʽ��-1��ʾʹ���ϴ��˳�ʱ�Ĳ������ط�ʽ��
        //            emTeam         ��ʼ��ʱʹ�õĲ����顣-1��ʾ�����ϴ��˳�ʱ�Ĳ����顣
        //            pCameraHandle  ����ľ��ָ�룬��ʼ���ɹ��󣬸�ָ��
        //               ���ظ��������Ч������ڵ����������
        //               ��صĲ����ӿ�ʱ������Ҫ����þ������Ҫ
        //               ���ڶ����֮������֡�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraInit(
        ref tSdkCameraDevInfo pCameraInfo,
            int emParamLoadMode,
            int emTeam,
        ref CameraHandle pCameraHandle
        );

        /******************************************************/
        // ������   : CameraSetCallbackFunction
        // �������� : ����ͼ�񲶻�Ļص��������������µ�ͼ������֡ʱ��
        //        pCallBack��ָ��Ļص������ͻᱻ���á� 
        // ����     : hCamera ����ľ������CameraInit������á�
        //            pCallBack �ص�����ָ�롣
        //            pContext  �ص������ĸ��Ӳ������ڻص�����������ʱ
        //            �ø��Ӳ����ᱻ���룬����ΪNULL��������
        //            ������ʱЯ��������Ϣ��
        //            pCallbackOld  ���ڱ��浱ǰ�Ļص�����������ΪNULL��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetCallbackFunction(
            CameraHandle hCamera,
            CAMERA_SNAP_PROC pCallBack,
            IntPtr pContext,
        ref CAMERA_SNAP_PROC pCallbackOld
        );

        /******************************************************/
        // ������   : CameraUnInit
        // �������� : �������ʼ�����ͷ���Դ��
        // ����     : hCamera ����ľ������CameraInit������á�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraUnInit(
            CameraHandle hCamera
        );

        /******************************************************/
        // ������   : CameraGetInformation
        // �������� : ��������������Ϣ
        // ����     : hCamera ����ľ������CameraInit������á�
        //            pbuffer ָ�����������Ϣָ���ָ�롣
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetInformation(
            CameraHandle hCamera,
            ref IntPtr pbuffer
        );

        /******************************************************/
        // ������   : CameraImageProcess
        // �������� : ����õ����ԭʼ���ͼ�����ݽ��д������ӱ��Ͷȡ�
        //        ��ɫ�����У��������ȴ���Ч�������õ�RGB888
        //        ��ʽ��ͼ�����ݡ�  
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            pbyIn    ����ͼ�����ݵĻ�������ַ������ΪNULL�� 
        //            pbyOut   �����ͼ������Ļ�������ַ������ΪNULL��
        //            pFrInfo  ����ͼ���֡ͷ��Ϣ��������ɺ�֡ͷ��Ϣ
        //             �е�ͼ���ʽuiMediaType����֮�ı䡣
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraImageProcess(
            CameraHandle hCamera,
            IntPtr pbyIn,
            IntPtr pbyOut,
        ref tSdkFrameHead pFrInfo
        );

        /******************************************************/
        // ������   : CameraDisplayInit
        // �������� : ��ʼ��SDK�ڲ�����ʾģ�顣�ڵ���CameraDisplayRGB24
        //        ǰ�����ȵ��øú�����ʼ����������ڶ��ο����У�
        //        ʹ���Լ��ķ�ʽ����ͼ����ʾ(������CameraDisplayRGB24)��
        //        ����Ҫ���ñ�������  
        // ����     : hCamera   ����ľ������CameraInit������á�
        //            IntPtrDisplay ��ʾ���ڵľ����һ��Ϊ���ڵ�m_IntPtr��Ա��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraDisplayInit(
            CameraHandle hCamera,
            IntPtr IntPtrDisplay
        );

        /******************************************************/
        // ������   : CameraDisplayRGB24
        // �������� : ��ʾͼ�񡣱�����ù�CameraDisplayInit����
        //        ��ʼ�����ܵ��ñ�������  
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            pbyRGB24 ͼ������ݻ�������RGB888��ʽ��
        //            pFrInfo  ͼ���֡ͷ��Ϣ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraDisplayRGB24(
            CameraHandle hCamera,
            IntPtr pbyRGB24,
        ref tSdkFrameHead pFrInfo
        );

        /******************************************************/
        // ������   : CameraSetDisplayMode
        // �������� : ������ʾ��ģʽ��������ù�CameraDisplayInit
        //        ���г�ʼ�����ܵ��ñ�������
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            iMode    ��ʾģʽ��DISPLAYMODE_SCALE����
        //             DISPLAYMODE_REAL,����μ�CameraDefine.h
        //             ��emSdkDisplayMode�Ķ��塣    
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetDisplayMode(
            CameraHandle hCamera,
            int iMode
        );

        /******************************************************/
        // ������   : CameraSetDisplayOffset
        // �������� : ������ʾ����ʼƫ��ֵ��������ʾģʽΪDISPLAYMODE_REAL
        //        ʱ��Ч��������ʾ�ؼ��Ĵ�СΪ320X240����ͼ���
        //        �ĳߴ�Ϊ640X480����ô��iOffsetX = 160,iOffsetY = 120ʱ
        //        ��ʾ���������ͼ��ľ���320X240��λ�á�������ù�
        //        CameraDisplayInit���г�ʼ�����ܵ��ñ�������
        // ����     : hCamera   ����ľ������CameraInit������á�
        //            iOffsetX  ƫ�Ƶ�X���ꡣ
        //            iOffsetY  ƫ�Ƶ�Y���ꡣ
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetDisplayOffset(
            CameraHandle hCamera,
            int iOffsetX,
            int iOffsetY
        );

        /******************************************************/
        // ������   : CameraSetDisplaySize
        // �������� : ������ʾ�ؼ��ĳߴ硣������ù�
        //        CameraDisplayInit���г�ʼ�����ܵ��ñ�������
        // ����     : hCamera   ����ľ������CameraInit������á�
        //            iWidth    ���
        //            iHeight   �߶�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetDisplaySize(
            CameraHandle hCamera,
            int iWidth,
            int iHeight
        );

        /******************************************************/
        // ������   : CameraGetImageBuffer
        // �������� : ���һ֡ͼ�����ݡ�Ϊ�����Ч�ʣ�SDK��ͼ��ץȡʱ�������㿽�����ƣ�
        //        CameraGetImageBufferʵ�ʻ�����ں��е�һ����������ַ��
        //        �ú����ɹ����ú󣬱������CameraReleaseImageBuffer�ͷ���
        //        CameraGetImageBuffer�õ��Ļ�����,�Ա����ں˼���ʹ��
        //        �û�������  
        // ����     : hCamera   ����ľ������CameraInit������á�
        //            pFrameInfo  ͼ���֡ͷ��Ϣָ�롣
        //            pbyBuffer   ָ��ͼ������ݵĻ�����ָ�롣����
        //              �������㿽�����������Ч�ʣ����
        //              ����ʹ����һ��ָ��ָ���ָ�롣
        //            uint wTimes ץȡͼ��ĳ�ʱʱ�䡣��λ���롣��
        //              wTimesʱ���ڻ�δ���ͼ����ú���
        //              �᷵�س�ʱ��Ϣ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetImageBuffer(
            CameraHandle hCamera,
        out tSdkFrameHead pFrameInfo,
        out IntPtr pbyBuffer,
            uint wTimes
        );

        /******************************************************/
        // ������   : CameraSnapToBuffer
        // �������� : ץ��һ��ͼ�񵽻������С���������ץ��ģʽ������
        //        �Զ��л���ץ��ģʽ�ķֱ��ʽ���ͼ�񲶻�Ȼ��
        //        ���񵽵����ݱ��浽�������С�
        //        �ú����ɹ����ú󣬱������CameraReleaseImageBuffer
        //        �ͷ���CameraSnapToBuffer�õ��Ļ�������������ο�
        //        CameraGetImageBuffer�����Ĺ����������֡�  
        // ����     : hCamera   ����ľ������CameraInit������á�
        //            pFrameInfo  ָ�룬����ͼ���֡ͷ��Ϣ��
        //            pbyBuffer   ָ��ָ���ָ�룬��������ͼ�񻺳����ĵ�ַ��
        //            uWaitTimeMs ��ʱʱ�䣬��λ���롣�ڸ�ʱ���ڣ������Ȼû��
        //              �ɹ���������ݣ��򷵻س�ʱ��Ϣ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSnapToBuffer(
            CameraHandle hCamera,
        out tSdkFrameHead pFrameInfo,
        out IntPtr pbyBuffer,
            uint uWaitTimeMs
        );

        /******************************************************/
        // ������   : CameraReleaseImageBuffer
        // �������� : �ͷ���CameraGetImageBuffer��õĻ�������
        // ����     : hCamera   ����ľ������CameraInit������á�
        //            pbyBuffer   ��CameraGetImageBuffer��õĻ�������ַ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraReleaseImageBuffer(
            CameraHandle hCamera,
            IntPtr pbyBuffer
        );

        /******************************************************/
        // ������   : CameraPlay
        // �������� : ��SDK���빤��ģʽ����ʼ��������������͵�ͼ��
        //        ���ݡ������ǰ����Ǵ���ģʽ������Ҫ���յ�
        //        ����֡�Ժ�Ż����ͼ��
        // ����     : hCamera   ����ľ������CameraInit������á�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraPlay(
            CameraHandle hCamera
        );

        /******************************************************/
        // ������   : CameraPause
        // �������� : ��SDK������ͣģʽ�����������������ͼ�����ݣ�
        //        ͬʱҲ�ᷢ�������������ͣ������ͷŴ������
        //        ��ͣģʽ�£����Զ�����Ĳ����������ã���������Ч��  
        // ����     : hCamera   ����ľ������CameraInit������á�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraPause(
            CameraHandle hCamera
        );

        /******************************************************/
        // ������   : CameraStop
        // �������� : ��SDK����ֹͣ״̬��һ���Ƿ���ʼ��ʱ���øú�����
        //        �ú��������ã������ٶ�����Ĳ����������á�
        // ����     : hCamera   ����ľ������CameraInit������á�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraStop(
            CameraHandle hCamera
        );

        /******************************************************/
        // ������   : CameraInitRecord
        // �������� : ��ʼ��һ��¼��
        // ����     : hCamera   ����ľ������CameraInit������á�
        //            iFormat   ¼��ĸ�ʽ����ǰֻ֧�ֲ�ѹ����MSCV���ַ�ʽ��  
        //              0:��ѹ����1:MSCV��ʽѹ����
        //            pcSavePath  ¼���ļ������·����
        //            b2GLimit    ���ΪTRUE,���ļ�����2Gʱ�Զ��ָ
        //            dwQuality   ¼����������ӣ�Խ��������Խ�á���Χ1��100.
        //            iFrameRate  ¼���֡�ʡ������趨�ı�ʵ�ʲɼ�֡�ʴ�
        //              �����Ͳ���©֡��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraInitRecord(
            CameraHandle hCamera,
            int iFormat,
            string pcSavePath,
            uint b2GLimit,
            uint dwQuality,
            int iFrameRate
        );

        /******************************************************/
        // ������   : CameraStopRecord
        // �������� : ��������¼�񡣵�CameraInitRecord�󣬿���ͨ���ú���
        //        ������һ��¼�񣬲�����ļ����������
        // ����     : hCamera   ����ľ������CameraInit������á�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraStopRecord(
            CameraHandle hCamera
        );

        /******************************************************/
        // ������   : CameraPushFrame
        // �������� : ��һ֡���ݴ���¼�����С��������CameraInitRecord
        //        ���ܵ��øú�����CameraStopRecord���ú󣬲����ٵ���
        //        �ú������������ǵ�֡ͷ��Ϣ��Я����ͼ��ɼ���ʱ���
        //        ��Ϣ�����¼����Ծ�׼��ʱ��ͬ����������֡�ʲ��ȶ�
        //        ��Ӱ�졣
        // ����     : hCamera     ����ľ������CameraInit������á�
        //            pbyImageBuffer    ͼ������ݻ�������������RGB��ʽ��
        //            pFrInfo           ͼ���֡ͷ��Ϣ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraPushFrame(
            CameraHandle hCamera,
            IntPtr pbyImageBuffer,
            ref tSdkFrameHead pFrInfo
        );

        /******************************************************/
        // ������   : CameraSaveImage
        // �������� : ��ͼ�񻺳��������ݱ����ͼƬ�ļ���
        // ����     : hCamera    ����ľ������CameraInit������á�
        //            lpszFileName   ͼƬ�����ļ�����·����
        //            pbyImageBuffer ͼ������ݻ�������
        //            pFrInfo        ͼ���֡ͷ��Ϣ��
        //            byFileType     ͼ�񱣴�ĸ�ʽ��ȡֵ��Χ�μ�CameraDefine.h
        //               ��emSdkFileType�����Ͷ��塣Ŀǰ֧��  
        //               BMP��JPG��PNG��RAW���ָ�ʽ������RAW��ʾ
        //               ��������ԭʼ���ݣ�����RAW��ʽ�ļ�Ҫ��
        //               pbyImageBuffer��pFrInfo����CameraGetImageBuffer
        //               ��õ����ݣ�����δ��CameraImageProcessת��
        //               ��BMP��ʽ����֮�����Ҫ�����BMP��JPG����
        //               PNG��ʽ����pbyImageBuffer��pFrInfo����
        //               CameraImageProcess������RGB��ʽ���ݡ�
        //                 �����÷����Բο�Advanced�����̡�   
        //            byQuality      ͼ�񱣴���������ӣ���������ΪJPG��ʽ
        //                 ʱ�ò�����Ч����Χ1��100�������ʽ
        //                           ����д��0��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSaveImage(
            CameraHandle hCamera,
            string lpszFileName,
            IntPtr pbyImageBuffer,
        ref tSdkFrameHead pFrInfo,
            emSdkFileType byFileType,
            Byte byQuality
        );

        public delegate CameraSdkStatus pfnCameraSaveImageEx(
	        CameraHandle hCamera,
            string lpszFileName,
            IntPtr pbyImageBuffer,
	        uint uImageFormat,
	        int	iWidth,
	        int	iHeight,
            emSdkFileType byFileType,
            Byte byQuality
	    );

        // ��֡ͼ��ת����C# Image��ʽ
        public static System.Drawing.Image CSharpImageFromFrame(IntPtr pFrameBuffer, ref tSdkFrameHead tFrameHead)
        {
            BITMAPINFOHEADER bmi;
            BITMAPFILEHEADER bmfi;
            RGBQUAD[] bmiColors = new RGBQUAD[256];
            Boolean bMono8 = (tFrameHead.uiMediaType == (uint)emImageFormat.CAMERA_MEDIA_TYPE_MONO8);
            uint HeadTotalSize = (uint)(bMono8 ? 54 + 1024 : 54);

            bmfi.bfType = ((int)'M' << 8) | ((int)'B');
            bmfi.bfOffBits = HeadTotalSize;
            bmfi.bfSize = (uint)(HeadTotalSize + tFrameHead.uBytes);
            bmfi.bfReserved1 = 0;
            bmfi.bfReserved2 = 0;

            bmi.biBitCount = (ushort)(bMono8 ? 8 : 24);
            bmi.biClrImportant = 0;
            bmi.biClrUsed = 0;
            bmi.biCompression = 0;
            bmi.biPlanes = 1;
            bmi.biSize = 40;
            bmi.biHeight = tFrameHead.iHeight;
            bmi.biWidth = tFrameHead.iWidth;
            bmi.biXPelsPerMeter = 0;
            bmi.biYPelsPerMeter = 0;
            bmi.biSizeImage = 0;

            if (bMono8)
            {
                for (int i = 0; i < 256; ++i)
                {
                    bmiColors[i].rgbReserved = 0;
                    bmiColors[i].rgbBlue =
                        bmiColors[i].rgbGreen =
                        bmiColors[i].rgbRed = (byte)i;
                }
            }

            MemoryStream stream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(stream);
            byte[] data = new byte[14];
            IntPtr ptr = Marshal.AllocHGlobal((int)HeadTotalSize);
            Marshal.StructureToPtr((object)bmfi, ptr, false);
            Marshal.Copy(ptr, data, 0, data.Length);
            bw.Write(data);
            data = new byte[40];
            Marshal.StructureToPtr((object)bmi, ptr, false);
            Marshal.Copy(ptr, data, 0, data.Length);
            bw.Write(data);
            if (bMono8)
            {
                data = new byte[1024];
                for (int i = 0; i < 256; ++i)
                    Marshal.StructureToPtr(bmiColors[i], (IntPtr)((Int64)ptr + i * 4), false);
                Marshal.Copy(ptr, data, 0, data.Length);
                bw.Write(data);
            }
            data = new byte[tFrameHead.uBytes];
            Marshal.Copy(pFrameBuffer, data, 0, data.Length);
            bw.Write(data);
            Marshal.FreeHGlobal(ptr);

            return System.Drawing.Image.FromStream(stream);
        }

        /******************************************************/
        // ������   : CameraGetImageResolution
        // �������� : ��õ�ǰԤ���ķֱ��ʡ�
        // ����     : hCamera    ����ľ������CameraInit������á�
        //            psCurVideoSize �ṹ��ָ�룬���ڷ��ص�ǰ�ķֱ��ʡ�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetImageResolution(
            CameraHandle hCamera,
        out tSdkImageResolution psCurVideoSize
        );

        /******************************************************/
        // ������   : CameraSetImageResolution
        // �������� : ����Ԥ���ķֱ��ʡ�
        // ����     : hCamera      ����ľ������CameraInit������á�
        //            pImageResolution �ṹ��ָ�룬���ڷ��ص�ǰ�ķֱ��ʡ�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetImageResolution(
            CameraHandle hCamera,
        ref tSdkImageResolution pImageResolution
        );
		
		public delegate CameraSdkStatus pfnCameraSetImageResolutionEx(
			CameraHandle            hCamera, 
			int						iIndex,
			int						Mode,
			uint					ModeSize,
			int						x,
			int						y,
			int						width,
			int						height,
			int						ZoomWidth,
			int						ZoomHeight
			);

        /******************************************************/
        // ������   : CameraGetMediaType
        // �������� : ��������ǰ���ԭʼ���ݵĸ�ʽ�����š�
        // ����     : hCamera   ����ľ������CameraInit������á�
        //            piMediaType   ָ�룬���ڷ��ص�ǰ��ʽ���͵������š�
        //              ��CameraGetCapability�����������ԣ�
        //              ��tSdkCameraCapbility�ṹ���е�pMediaTypeDesc
        //              ��Ա�У����������ʽ���������֧�ֵĸ�ʽ��
        //              piMediaType��ָ��������ţ����Ǹ�����������š�
        //              pMediaTypeDesc[*piMediaType].iMediaType���ʾ��ǰ��ʽ�� 
        //              ���롣�ñ�����μ�CameraDefine.h��[ͼ���ʽ����]���֡�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetMediaType(
            CameraHandle hCamera,
        ref int piMediaType
        );

        /******************************************************/
        // ������   : CameraSetMediaType
        // �������� : ������������ԭʼ���ݸ�ʽ��
        // ����     : hCamera   ����ľ������CameraInit������á�
        //            iMediaType  ��CameraGetCapability�����������ԣ�
        //              ��tSdkCameraCapbility�ṹ���е�pMediaTypeDesc
        //              ��Ա�У����������ʽ���������֧�ֵĸ�ʽ��
        //              iMediaType���Ǹ�����������š�
        //              pMediaTypeDesc[iMediaType].iMediaType���ʾ��ǰ��ʽ�� 
        //              ���롣�ñ�����μ�CameraDefine.h��[ͼ���ʽ����]���֡�   
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetMediaType(
            CameraHandle hCamera,
            int iMediaType
        );

        /******************************************************/
        // ������   : CameraSetAeState
        // �������� : ��������ع��ģʽ���Զ������ֶ���
        // ����     : hCamera   ����ľ������CameraInit������á�
        //            bAeState    TRUE��ʹ���Զ��ع⣻FALSE��ֹͣ�Զ��ع⡣
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetAeState(
            CameraHandle hCamera,
            uint bAeState
        );

        /******************************************************/
        // ������   : CameraGetAeState
        // �������� : ��������ǰ���ع�ģʽ��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            pAeState   ָ�룬���ڷ����Զ��ع��ʹ��״̬��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetAeState(
            CameraHandle hCamera,
        ref uint pAeState
        );

        /******************************************************/
        // ������   : CameraSetSharpness
        // �������� : ����ͼ��Ĵ�����񻯲�����
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            iSharpness �񻯲�������Χ��CameraGetCapability
        //               ��ã�һ����[0,100]��0��ʾ�ر��񻯴���
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetSharpness(
            CameraHandle hCamera,
            int iSharpness
        );

        /******************************************************/
        // ������   : CameraGetSharpness
        // �������� : ��ȡ��ǰ���趨ֵ��
        // ����     : hCamera   ����ľ������CameraInit������á�
        //            piSharpness ָ�룬���ص�ǰ�趨���񻯵��趨ֵ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetSharpness(
            CameraHandle hCamera,
        ref int piSharpness
        );

        /******************************************************/
        // ������   : CameraSetLutMode
        // �������� : ��������Ĳ��任ģʽLUTģʽ��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            emLutMode  LUTMODE_PARAM_GEN ��ʾ��٤��ͶԱȶȲ�����̬����LUT��
        //             LUTMODE_PRESET    ��ʾʹ��Ԥ���LUT��
        //             LUTMODE_USER_DEF  ��ʾʹ���û��Զ���LUT��
        //             LUTMODE_PARAM_GEN�Ķ���ο�CameraDefine.h��emSdkLutMode���͡�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetLutMode(
            CameraHandle hCamera,
            int emLutMode
        );

        /******************************************************/
        // ������   : CameraGetLutMode
        // �������� : �������Ĳ��任ģʽLUTģʽ��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            pemLutMode ָ�룬���ص�ǰLUTģʽ��������CameraSetLutMode
        //             ��emLutMode������ͬ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetLutMode(
            CameraHandle hCamera,
        ref int pemLutMode
        );

        /******************************************************/
        // ������   : CameraSelectLutPreset
        // �������� : ѡ��Ԥ��LUTģʽ�µ�LUT��������ʹ��CameraSetLutMode
        //        ��LUTģʽ����ΪԤ��ģʽ��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            iSel     ��������š���ĸ�����CameraGetCapability
        //             ��á�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSelectLutPreset(
            CameraHandle hCamera,
            int iSel
        );

        /******************************************************/
        // ������   : CameraGetLutPresetSel
        // �������� : ���Ԥ��LUTģʽ�µ�LUT�������š�
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            piSel      ָ�룬���ر�������š�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetLutPresetSel(
            CameraHandle hCamera,
        ref int piSel
        );

        /******************************************************/
        // ������   : CameraSetCustomLut
        // �������� : �����Զ����LUT��������ʹ��CameraSetLutMode
        //        ��LUTģʽ����Ϊ�Զ���ģʽ��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //             iChannel ָ��Ҫ�趨��LUT��ɫͨ������ΪLUT_CHANNEL_ALLʱ��
        //                      ����ͨ����LUT����ͬʱ�滻��
        //                      �ο�CameraDefine.h��emSdkLutChannel���塣
        //            pLut     ָ�룬ָ��LUT��ĵ�ַ��LUT��Ϊ�޷��Ŷ��������飬�����СΪ
        //           4096���ֱ������ɫͨ����0��4096(12bit��ɫ����)��Ӧ��ӳ��ֵ�� 
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetCustomLut(
            CameraHandle hCamera,
            int iChannel,
            ushort[] pLut
        );

        /******************************************************/
        // ������   : CameraGetCustomLut
        // �������� : ��õ�ǰʹ�õ��Զ���LUT��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //             iChannel ָ��Ҫ��õ�LUT��ɫͨ������ΪLUT_CHANNEL_ALLʱ��
        //                      ���غ�ɫͨ����LUT��
        //                      �ο�CameraDefine.h��emSdkLutChannel���塣
        //            pLut     ָ�룬ָ��LUT��ĵ�ַ��LUT��Ϊ�޷��Ŷ��������飬�����СΪ
        //           4096���ֱ������ɫͨ����0��4096(12bit��ɫ����)��Ӧ��ӳ��ֵ�� 
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetCustomLut(
            CameraHandle hCamera,
            int iChannel,
        ref ushort[] pLut
        );

        /******************************************************/
        // ������   : CameraGetCurrentLut
        // �������� : ��������ǰ��LUT�����κ�LUTģʽ�¶����Ե���,
        //        ����ֱ�۵Ĺ۲�LUT���ߵı仯��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //             iChannel ָ��Ҫ��õ�LUT��ɫͨ������ΪLUT_CHANNEL_ALLʱ��
        //                      ���غ�ɫͨ����LUT��
        //                      �ο�CameraDefine.h��emSdkLutChannel���塣
        //            pLut     ָ�룬ָ��LUT��ĵ�ַ��LUT��Ϊ�޷��Ŷ��������飬�����СΪ
        //           4096���ֱ������ɫͨ����0��4096(12bit��ɫ����)��Ӧ��ӳ��ֵ�� 
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetCurrentLut(
            CameraHandle hCamera,
            int iChannel,
        ref ushort[] pLut
        );

        /******************************************************/
        // ������   : CameraSetWbMode
        // �������� : ���������ƽ��ģʽ����Ϊ�ֶ����Զ����ַ�ʽ��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            bAuto      TRUE�����ʾʹ���Զ�ģʽ��
        //             FALSE�����ʾʹ���ֶ�ģʽ��ͨ������
        //                 CameraSetOnceWB������һ�ΰ�ƽ�⡣        
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetWbMode(
            CameraHandle hCamera,
            uint bAuto
        );

        /******************************************************/
        // ������   : CameraGetWbMode
        // �������� : ��õ�ǰ�İ�ƽ��ģʽ��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            pbAuto   ָ�룬����TRUE��ʾ�Զ�ģʽ��FALSE
        //             Ϊ�ֶ�ģʽ�� 
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetWbMode(
            CameraHandle hCamera,
        ref uint pbAuto
        );

        /******************************************************/
        // ������   : CameraSetPresetClrTemp
        // �������� : ����ɫ��ģʽ
        // ����     : hCamera  ����ľ������CameraInit������á�
        //             iSel     �����š�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetPresetClrTemp(
            CameraHandle hCamera,
            int iSel
        );

        /******************************************************/
        // ������   : CameraGetPresetClrTemp
        // �������� : 
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            int* piSel
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetPresetClrTemp(
            CameraHandle hCamera,
        ref int piSel
        );
		
		/******************************************************/
		// ������   : CameraSetUserClrTempGain
		// �������� : �����Զ���ɫ��ģʽ�µ���������
		// ����     : hCamera  ����ľ������CameraInit������á�
		//            iRgain  ��ɫ���棬��Χ0��400����ʾ0��4��
		//            iGgain  ��ɫ���棬��Χ0��400����ʾ0��4��
		//            iBgain  ��ɫ���棬��Χ0��400����ʾ0��4��
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
		public delegate CameraSdkStatus pfnCameraSetUserClrTempGain(
		  CameraHandle  hCamera,
		  int       iRgain,
		  int       iGgain,
		  int       iBgain
		);


		/******************************************************/
		// ������   : CameraGetUserClrTempGain
		// �������� : ����Զ���ɫ��ģʽ�µ���������
		// ����     : hCamera  ����ľ������CameraInit������á�
		//            piRgain  ָ�룬���غ�ɫ���棬��Χ0��400����ʾ0��4��
		//            piGgain  ָ�룬������ɫ���棬��Χ0��400����ʾ0��4��
		//            piBgain  ָ�룬������ɫ���棬��Χ0��400����ʾ0��4��
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
		public delegate CameraSdkStatus pfnCameraGetUserClrTempGain(
		  CameraHandle  hCamera,
		  out int      piRgain,
		  out int      piGgain,
		  out int      piBgain
		);

		/******************************************************/
		// ������   : CameraSetUserClrTempMatrix
		// �������� : �����Զ���ɫ��ģʽ�µ���ɫ����
		// ����     : hCamera  ����ľ������CameraInit������á�
		//            pMatrix ָ��һ��float[3][3]������׵�ַ
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
		public delegate CameraSdkStatus pfnCameraSetUserClrTempMatrix(
		  CameraHandle  hCamera,
		  float[,]     pMatrix
		);


		/******************************************************/
		// ������   : CameraGetUserClrTempMatrix
		// �������� : ����Զ���ɫ��ģʽ�µ���ɫ����
		// ����     : hCamera  ����ľ������CameraInit������á�
		//            pMatrix ָ��һ��float[3][3]������׵�ַ
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
		public delegate CameraSdkStatus pfnCameraGetUserClrTempMatrix(
		  CameraHandle  hCamera,
		  float[,]      pMatrix
		);

		/******************************************************/
		// ������   : CameraSetClrTempMode
		// �������� : ���ð�ƽ��ʱʹ�õ�ɫ��ģʽ��
		//              ֧�ֵ�ģʽ�����֣��ֱ����Զ���Ԥ����Զ��塣
		//              �Զ�ģʽ�£����Զ�ѡ����ʵ�ɫ��ģʽ
		//              Ԥ��ģʽ�£���ʹ���û�ָ����ɫ��ģʽ
		//              �Զ���ģʽ�£�ʹ���û��Զ����ɫ����������;���
		// ����     : hCamera ����ľ������CameraInit������á�
		//            iMode ģʽ��ֻ����emSdkClrTmpMode�ж����һ��
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
		public delegate CameraSdkStatus pfnCameraSetClrTempMode(
		  CameraHandle  hCamera,
		  int       iMode
		);

		/******************************************************/
		// ������   : CameraGetClrTempMode
		// �������� : ��ð�ƽ��ʱʹ�õ�ɫ��ģʽ���ο�CameraSetClrTempMode
		//              �й����������֡�
		// ����     : hCamera ����ľ������CameraInit������á�
		//            pimode ָ�룬����ģʽѡ�񣬲ο�emSdkClrTmpMode���Ͷ���
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
		public delegate CameraSdkStatus pfnCameraGetClrTempMode(
		  CameraHandle  hCamera,
		  out int       pimode
		);

        /******************************************************/
        // ������   : CameraSetOnceWB
        // �������� : ���ֶ���ƽ��ģʽ�£����øú��������һ�ΰ�ƽ�⡣
        //        ��Ч��ʱ��Ϊ���յ���һ֡ͼ������ʱ��
        // ����     : hCamera  ����ľ������CameraInit������á�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetOnceWB(
            CameraHandle hCamera
        );

        /******************************************************/
        // ������   : CameraSetOnceBB
        // �������� : ִ��һ�κ�ƽ�������
        // ����     : hCamera  ����ľ������CameraInit������á�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetOnceBB(
            CameraHandle hCamera
        );


        /******************************************************/
        // ������   : CameraSetAeTarget
        // �������� : �趨�Զ��ع������Ŀ��ֵ���趨��Χ��CameraGetCapability
        //        ������á�
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            iAeTarget  ����Ŀ��ֵ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetAeTarget(
            CameraHandle hCamera,
            int iAeTarget
        );

        /******************************************************/
        // ������   : CameraGetAeTarget
        // �������� : ����Զ��ع������Ŀ��ֵ��
        // ����     : hCamera   ����ľ������CameraInit������á�
        //            *piAeTarget ָ�룬����Ŀ��ֵ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetAeTarget(
            CameraHandle hCamera,
        ref int piAeTarget
        );

        /******************************************************/
		// ������   : CameraSetAeExposureRange
		// �������� : �趨�Զ��ع�ģʽ���ع�ʱ����ڷ�Χ
		// ����     : hCamera  ����ľ������CameraInit������á�
		//           fMinExposureTime ��С�ع�ʱ�䣨΢�룩
		//			 fMaxExposureTime ����ع�ʱ�䣨΢�룩
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraSetAeExposureRange(
			CameraHandle    hCamera, 
			double          fMinExposureTime,
			double			fMaxExposureTime
			);

		/******************************************************/
		// ������   : CameraGetAeExposureRange
		// �������� : ����Զ��ع�ģʽ���ع�ʱ����ڷ�Χ
		// ����     : hCamera   ����ľ������CameraInit������á�
		//           fMinExposureTime ��С�ع�ʱ�䣨΢�룩
		//			 fMaxExposureTime ����ع�ʱ�䣨΢�룩
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraGetAeExposureRange(
			CameraHandle    hCamera, 
			out double      fMinExposureTime,
			out double	    fMaxExposureTime
			);

		/******************************************************/
		// ������   : CameraSetAeAnalogGainRange
		// �������� : �趨�Զ��ع�ģʽ��������ڷ�Χ
		// ����     : hCamera  ����ľ������CameraInit������á�
		//           iMinAnalogGain ��С����
		//			 iMaxAnalogGain �������
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraSetAeAnalogGainRange(
			CameraHandle    hCamera, 
			int				iMinAnalogGain,
			int				iMaxAnalogGain
			);

		/******************************************************/
		// ������   : CameraGetAeAnalogGainRange
		// �������� : ����Զ��ع�ģʽ��������ڷ�Χ
		// ����     : hCamera   ����ľ������CameraInit������á�
		//           iMinAnalogGain ��С����
		//			 iMaxAnalogGain �������
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraGetAeAnalogGainRange(
			CameraHandle    hCamera, 
			out int			iMinAnalogGain,
			out int			iMaxAnalogGain
			);

        /******************************************************/
		// ������   : CameraSetAeThreshold
		// �������� : �����Զ��ع�ģʽ�ĵ�����ֵ
		// ����     : hCamera   ����ľ������CameraInit������á�
		//           iThreshold   ��� abs(Ŀ������-ͼ������) < iThreshold ��ֹͣ�Զ�����
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraSetAeThreshold(
			CameraHandle    hCamera, 
			int				iThreshold
			);

		/******************************************************/
		// ������   : CameraGetAeThreshold
		// �������� : ��ȡ�Զ��ع�ģʽ�ĵ�����ֵ
		// ����     : hCamera   ����ľ������CameraInit������á�
		//           iThreshold   ��ȡ���ĵ�����ֵ
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraGetAeThreshold(
			CameraHandle    hCamera, 
			out int			iThreshold
			);

        /******************************************************/
        // ������   : CameraSetExposureTime
        // �������� : �����ع�ʱ�䡣��λΪ΢�롣����CMOS�����������ع�
        //        �ĵ�λ�ǰ�����������ģ���ˣ��ع�ʱ�䲢������΢��
        //        ���������ɵ������ǻᰴ��������ȡ�ᡣ�ڵ���
        //        �������趨�ع�ʱ��󣬽����ٵ���CameraGetExposureTime
        //        �����ʵ���趨��ֵ��
        // ����     : hCamera      ����ľ������CameraInit������á�
        //            fExposureTime �ع�ʱ�䣬��λ΢�롣
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetExposureTime(
            CameraHandle hCamera,
            double fExposureTime
        );

        /******************************************************/
        // ������   : CameraGetExposureLineTime
        // �������� : ���һ�е��ع�ʱ�䡣����CMOS�����������ع�
        //        �ĵ�λ�ǰ�����������ģ���ˣ��ع�ʱ�䲢������΢��
        //        ���������ɵ������ǻᰴ��������ȡ�ᡣ���������
        //          ���þ��Ƿ���CMOS����ع�һ�ж�Ӧ��ʱ�䡣
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            pfLineTime ָ�룬����һ�е��ع�ʱ�䣬��λΪ΢�롣
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetExposureLineTime(
            CameraHandle hCamera,
			ref double pfLineTime
        );

        /******************************************************/
        // ������   : CameraGetExposureTime
        // �������� : ���������ع�ʱ�䡣��μ�CameraSetExposureTime
        //        �Ĺ���������
        // ����     : hCamera        ����ľ������CameraInit������á�
        //            pfExposureTime   ָ�룬���ص�ǰ���ع�ʱ�䣬��λ΢�롣
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetExposureTime(
            CameraHandle hCamera,
			ref double pfExposureTime
        );
		
		/******************************************************/
		// ������   : CameraGetExposureTimeRange
		// �������� : ���������ع�ʱ�䷶Χ
		// ����     : hCamera        ����ľ������CameraInit������á�
		//            pfMin			ָ�룬�����ع�ʱ�����Сֵ����λ΢�롣
		//            pfMax			ָ�룬�����ع�ʱ������ֵ����λ΢�롣
		//            pfStep		ָ�룬�����ع�ʱ��Ĳ���ֵ����λ΢�롣
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
		public delegate CameraSdkStatus pfnCameraGetExposureTimeRange(
			CameraHandle    hCamera, 
			ref double      pfMin,
			ref double      pfMax,
			ref double      pfStep
		);

        /******************************************************/
        // ������   : CameraSetAnalogGain
        // �������� : ���������ͼ��ģ������ֵ����ֵ����CameraGetCapability���
        //        ��������Խṹ����sExposeDesc.fAnalogGainStep����
        //        �õ�ʵ�ʵ�ͼ���źŷŴ�����
        // ����     : hCamera   ����ľ������CameraInit������á�
        //            iAnalogGain �趨��ģ������ֵ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetAnalogGain(
            CameraHandle hCamera,
            int iAnalogGain
        );

        /******************************************************/
        // ������   : CameraGetAnalogGain
        // �������� : ���ͼ���źŵ�ģ������ֵ���μ�CameraSetAnalogGain
        //        ��ϸ˵����
        // ����     : hCamera    ����ľ������CameraInit������á�
        //            piAnalogGain ָ�룬���ص�ǰ��ģ������ֵ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetAnalogGain(
            CameraHandle hCamera,
        ref int piAnalogGain
        );

        /******************************************************/
        // ������   : CameraSetGain
        // �������� : ����ͼ����������档�趨��Χ��CameraGetCapability
        //        ��õ�������Խṹ����sRgbGainRange��Ա������
        //        ʵ�ʵķŴ������趨ֵ/100��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            iRGain   ��ɫͨ��������ֵ�� 
        //            iGGain   ��ɫͨ��������ֵ��
        //            iBGain   ��ɫͨ��������ֵ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetGain(
            CameraHandle hCamera,
            int iRGain,
            int iGGain,
            int iBGain
        );


        /******************************************************/
        // ������   : CameraGetGain
        // �������� : ���ͼ������������档������μ�CameraSetGain
        //        �Ĺ����������֡�
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            piRGain  ָ�룬���غ�ɫͨ������������ֵ��
        //            piGGain    ָ�룬������ɫͨ������������ֵ��
        //            piBGain    ָ�룬������ɫͨ������������ֵ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetGain(
            CameraHandle hCamera,
        ref int piRGain,
        ref int piGGain,
        ref int piBGain
        );


        /******************************************************/
        // ������   : CameraSetGamma
        // �������� : �趨LUT��̬����ģʽ�µ�Gammaֵ���趨��ֵ��
        //        ���ϱ�����SDK�ڲ�������ֻ�е�������ڶ�̬
        //        �������ɵ�LUTģʽʱ���Ż���Ч����ο�CameraSetLutMode
        //        �ĺ���˵�����֡�
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            iGamma     Ҫ�趨��Gammaֵ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetGamma(
            CameraHandle hCamera,
            int iGamma
        );

        /******************************************************/
        // ������   : CameraGetGamma
        // �������� : ���LUT��̬����ģʽ�µ�Gammaֵ����ο�CameraSetGamma
        //        �����Ĺ���������
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            piGamma    ָ�룬���ص�ǰ��Gammaֵ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetGamma(
            CameraHandle hCamera,
        ref int piGamma
        );

        /******************************************************/
        // ������   : CameraSetContrast
        // �������� : �趨LUT��̬����ģʽ�µĶԱȶ�ֵ���趨��ֵ��
        //        ���ϱ�����SDK�ڲ�������ֻ�е�������ڶ�̬
        //        �������ɵ�LUTģʽʱ���Ż���Ч����ο�CameraSetLutMode
        //        �ĺ���˵�����֡�
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            iContrast  �趨�ĶԱȶ�ֵ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetContrast(
            CameraHandle hCamera,
            int iContrast
        );

        /******************************************************/
        // ������   : CameraGetContrast
        // �������� : ���LUT��̬����ģʽ�µĶԱȶ�ֵ����ο�
        //        CameraSetContrast�����Ĺ���������
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            piContrast ָ�룬���ص�ǰ�ĶԱȶ�ֵ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetContrast(
            CameraHandle hCamera,
        ref int piContrast
        );

        /******************************************************/
        // ������   : CameraSetSaturation
        // �������� : �趨ͼ����ı��Ͷȡ��Ժڰ������Ч��
        //        �趨��Χ��CameraGetCapability��á�100��ʾ
        //        ��ʾԭʼɫ�ȣ�����ǿ��
        // ����     : hCamera    ����ľ������CameraInit������á�
        //            iSaturation  �趨�ı��Ͷ�ֵ�� 
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetSaturation(
            CameraHandle hCamera,
            int iSaturation
        );

        /******************************************************/
        // ������   : CameraGetSaturation
        // �������� : ���ͼ����ı��Ͷȡ�
        // ����     : hCamera    ����ľ������CameraInit������á�
        //            piSaturation ָ�룬���ص�ǰͼ����ı��Ͷ�ֵ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetSaturation(
            CameraHandle hCamera,
        ref int piSaturation
        );

        /******************************************************/
        // ������   : CameraSetMonochrome
        // �������� : ���ò�ɫתΪ�ڰ׹��ܵ�ʹ�ܡ�
        // ����     : hCamera ����ľ������CameraInit������á�
        //            bEnable   TRUE����ʾ����ɫͼ��תΪ�ڰס�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetMonochrome(
            CameraHandle hCamera,
            uint bEnable
        );

        /******************************************************/
        // ������   : CameraGetMonochrome
        // �������� : ��ò�ɫת���ڰ׹��ܵ�ʹ��״����
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            pbEnable   ָ�롣����TRUE��ʾ�����˲�ɫͼ��
        //             ת��Ϊ�ڰ�ͼ��Ĺ��ܡ�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetMonochrome(
            CameraHandle hCamera,
        ref uint pbEnable
        );

        /******************************************************/
        // ������   : CameraSetInverse
        // �������� : ���ò�ͼ����ɫ��ת���ܵ�ʹ�ܡ�
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            bEnable    TRUE����ʾ����ͼ����ɫ��ת���ܣ�
        //             ���Ի�����ƽ����Ƭ��Ч����
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetInverse(
            CameraHandle hCamera,
            uint bEnable
        );

        /******************************************************/
        // ������   : CameraGetInverse
        // �������� : ���ͼ����ɫ��ת���ܵ�ʹ��״̬��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            pbEnable   ָ�룬���ظù���ʹ��״̬�� 
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetInverse(
            CameraHandle hCamera,
        ref uint pbEnable
        );

        /******************************************************/
        // ������   : CameraSetAntiFlick
        // �������� : �����Զ��ع�ʱ��Ƶ�����ܵ�ʹ��״̬�������ֶ�
        //        �ع�ģʽ����Ч��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            bEnable    TRUE��������Ƶ������;FALSE���رոù��ܡ�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetAntiFlick(
            CameraHandle hCamera,
            uint bEnable
        );

        /******************************************************/
        // ������   : CameraGetAntiFlick
        // �������� : ����Զ��ع�ʱ��Ƶ�����ܵ�ʹ��״̬��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            pbEnable   ָ�룬���ظù��ܵ�ʹ��״̬��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetAntiFlick(
            CameraHandle hCamera,
        ref uint pbEnable
        );

        /******************************************************/
        // ������   : CameraGetLightFrequency
        // �������� : ����Զ��ع�ʱ����Ƶ����Ƶ��ѡ��
        // ����     : hCamera      ����ľ������CameraInit������á�
        //            piFrequencySel ָ�룬����ѡ��������š�0:50HZ 1:60HZ
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetLightFrequency(
            CameraHandle hCamera,
        ref int piFrequencySel
        );

        /******************************************************/
        // ������   : CameraSetLightFrequency
        // �������� : �����Զ��ع�ʱ��Ƶ����Ƶ�ʡ�
        // ����     : hCamera     ����ľ������CameraInit������á�
        //            iFrequencySel 0:50HZ , 1:60HZ 
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetLightFrequency(
            CameraHandle hCamera,
            int iFrequencySel
        );

        /******************************************************/
        // ������   : CameraSetFrameSpeed
        // �������� : �趨������ͼ���֡�ʡ�����ɹ�ѡ���֡��ģʽ��
        //        CameraGetCapability��õ���Ϣ�ṹ����iFrameSpeedDesc
        //        ��ʾ���֡��ѡ��ģʽ������
        // ����     : hCamera   ����ľ������CameraInit������á�
        //            iFrameSpeed ѡ���֡��ģʽ�����ţ���Χ��0��
        //              CameraGetCapability��õ���Ϣ�ṹ����iFrameSpeedDesc - 1   
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetFrameSpeed(
            CameraHandle hCamera,
            int iFrameSpeed
        );

        /******************************************************/
        // ������   : CameraGetFrameSpeed
        // �������� : ���������ͼ���֡��ѡ�������š������÷��ο�
        //        CameraSetFrameSpeed�����Ĺ����������֡�
        // ����     : hCamera    ����ľ������CameraInit������á�
        //            piFrameSpeed ָ�룬����ѡ���֡��ģʽ�����š� 
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetFrameSpeed(
            CameraHandle hCamera,
        ref int piFrameSpeed
        );


        /******************************************************/
        // ������   : CameraSetParameterMode
        // �������� : �趨������ȡ��Ŀ�����
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            iMode  ������ȡ�Ķ��󡣲ο�
        //          emSdkParameterMode�����Ͷ��塣
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetParameterMode(
            CameraHandle hCamera,
            int iTarget
        );

        /******************************************************/
        // ������   : CameraGetParameterMode
        // �������� : 
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            int* piTarget
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetParameterMode(
            CameraHandle hCamera,
        ref int piTarget
        );

        /******************************************************/
        // ������   : CameraSetParameterMask
        // �������� : ���ò�����ȡ�����롣�������غͱ���ʱ����ݸ�
        //        ��������������ģ��������Ƿ���ػ��߱��档
        // ����     : hCamera ����ľ������CameraInit������á�
        //            uMask     ���롣�ο�CameraDefine.h��PROP_SHEET_INDEX
        //            ���Ͷ��塣
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetParameterMask(
            CameraHandle hCamera,
            uint uMask
        );

        /******************************************************/
        // ������   : CameraSaveParameter
        // �������� : ���浱ǰ���������ָ���Ĳ������С�����ṩ��A,B,C,D
        //        A,B,C,D����ռ������в����ı��档 
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            iTeam      PARAMETER_TEAM_A ���浽A����,
        //             PARAMETER_TEAM_B ���浽B����,
        //             PARAMETER_TEAM_C ���浽C����,
        //             PARAMETER_TEAM_D ���浽D����
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSaveParameter(
            CameraHandle hCamera,
            int iTeam
        );

        /******************************************************/
        // ������   : CameraSaveParameterToFile
        // �������� : ���浱ǰ���������ָ�����ļ��С����ļ����Ը��Ƶ�
        //        ��ĵ����Ϲ�����������أ�Ҳ���������������á�
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            sFileName  �����ļ�������·����
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSaveParameterToFile(
            CameraHandle hCamera,
            string sFileName
        );

        /******************************************************/
        // ������   : CameraReadParameterFromFile
        // �������� : ��PC��ָ���Ĳ����ļ��м��ز������ҹ�˾�������
        //        ������PC��Ϊ.config��׺���ļ���λ�ڰ�װ�µ�
        //        Camera\Configs�ļ����С�
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            *sFileName �����ļ�������·����
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraReadParameterFromFile(
            CameraHandle hCamera,
            string sFileName
        );

        /******************************************************/
        // ������   : CameraLoadParameter
        // �������� : ����ָ����Ĳ���������С�
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            iTeam    PARAMETER_TEAM_A ����A�����,
        //             PARAMETER_TEAM_B ����B�����,
        //             PARAMETER_TEAM_C ����C�����,
        //             PARAMETER_TEAM_D ����D�����,
        //             PARAMETER_TEAM_DEFAULT ����Ĭ�ϲ�����    
        //             ���Ͷ���ο�CameraDefine.h��emSdkParameterTeam����
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraLoadParameter(
            CameraHandle hCamera,
            int iTeam
        );

        /******************************************************/
        // ������   : CameraGetCurrentParameterGroup
        // �������� : ��õ�ǰѡ��Ĳ����顣
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            piTeam     ָ�룬���ص�ǰѡ��Ĳ����顣����ֵ
        //             �ο�CameraLoadParameter��iTeam������
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetCurrentParameterGroup(
            CameraHandle hCamera,
        ref int piTeam
        );

        /******************************************************/
        // ������   : CameraSetTransPackLen
        // �������� : �����������ͼ�����ݵķְ���С��
        //        Ŀǰ��SDK�汾�У��ýӿڽ���GIGE�ӿ������Ч��
        //        �����������紫��ķְ���С������֧�־�֡��������
        //        ���ǽ���ѡ��8K�ķְ���С��������Ч�Ľ��ʹ���
        //        ��ռ�õ�CPU����ʱ�䡣
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            iPackSel   �ְ�����ѡ��������š��ְ����ȿ���
        //             ���������Խṹ����pPackLenDesc��Ա������
        //             iPackLenDesc��Ա���ʾ����ѡ�ķְ�ģʽ������
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetTransPackLen(
            CameraHandle hCamera,
            int iPackSel
        );

        /******************************************************/
        // ������   : CameraGetTransPackLen
        // �������� : ��������ǰ����ְ���С��ѡ�������š�
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            piPackSel  ָ�룬���ص�ǰѡ��ķְ���С�����š�
        //             �μ�CameraSetTransPackLen��iPackSel��
        //             ˵����
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetTransPackLen(
            CameraHandle hCamera,
        ref int piPackSel
        );

        /******************************************************/
        // ������   : CameraIsAeWinVisible
        // �������� : ����Զ��ع�ο����ڵ���ʾ״̬��
        // ����     : hCamera    ����ľ������CameraInit������á�
        //            pbIsVisible  ָ�룬����TRUE�����ʾ��ǰ���ڻ�
        //               ��������ͼ�������ϡ�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraIsAeWinVisible(
            CameraHandle hCamera,
        ref uint pbIsVisible
        );

        /******************************************************/
        // ������   : CameraSetAeWinVisible
        // �������� : �����Զ��ع�ο����ڵ���ʾ״̬�������ô���״̬
        //        Ϊ��ʾ������CameraImageOverlay���ܹ�������λ��
        //        �Ծ��εķ�ʽ������ͼ���ϡ�
        // ����     : hCamera   ����ľ������CameraInit������á�
        //            bIsVisible  TRUE������Ϊ��ʾ��FALSE������ʾ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetAeWinVisible(
            CameraHandle hCamera,
            uint bIsVisible
        );

        /******************************************************/
        // ������   : CameraGetAeWindow
        // �������� : ����Զ��ع�ο����ڵ�λ�á�
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            piHOff     ָ�룬���ش���λ�����ϽǺ�����ֵ��
        //            piVOff     ָ�룬���ش���λ�����Ͻ�������ֵ��
        //            piWidth    ָ�룬���ش��ڵĿ�ȡ�
        //            piHeight   ָ�룬���ش��ڵĸ߶ȡ�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetAeWindow(
            CameraHandle hCamera,
        ref int piHOff,
        ref int piVOff,
        ref int piWidth,
        ref int piHeight
        );

        /******************************************************/
        // ������   : CameraSetAeWindow
        // �������� : �����Զ��ع�Ĳο����ڡ�
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            iHOff    �������Ͻǵĺ�����
        //            iVOff      �������Ͻǵ�������
        //            iWidth     ���ڵĿ�� 
        //            iHeight    ���ڵĸ߶�
        //        ���iHOff��iVOff��iWidth��iHeightȫ��Ϊ0����
        //        ��������Ϊÿ���ֱ����µľ���1/2��С����������
        //        �ֱ��ʵı仯������仯�����iHOff��iVOff��iWidth��iHeight
        //        �������Ĵ���λ�÷�Χ�����˵�ǰ�ֱ��ʷ�Χ�ڣ� 
        //          ���Զ�ʹ�þ���1/2��С���ڡ�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetAeWindow(
            CameraHandle hCamera,
            int iHOff,
            int iVOff,
            int iWidth,
            int iHeight
        );

        /******************************************************/
        // ������   : CameraSetMirror
        // �������� : ����ͼ������������������Ϊˮƽ�ʹ�ֱ��������
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            iDir     ��ʾ����ķ���0����ʾˮƽ����1����ʾ��ֱ����
        //            bEnable  TRUE��ʹ�ܾ���;FALSE����ֹ����
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetMirror(
            CameraHandle hCamera,
            int iDir,
            uint bEnable
        );

        /******************************************************/
        // ������   : CameraGetMirror
        // �������� : ���ͼ��ľ���״̬��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            iDir     ��ʾҪ��õľ�����
        //             0����ʾˮƽ����1����ʾ��ֱ����
        //            pbEnable   ָ�룬����TRUE�����ʾiDir��ָ�ķ���
        //             ����ʹ�ܡ�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetMirror(
            CameraHandle hCamera,
            int iDir,
        ref uint pbEnable
        );

        /******************************************************/
        // ������   : CameraSetRotate
        // �������� : ����ͼ����ת����
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            iRot     ��ʾ��ת�ĽǶȣ���ʱ�뷽�򣩣�0������ת 1:90�� 2:180�� 3:270�ȣ�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetRotate(
	        CameraHandle    hCamera, 
	        int             iRot 
	    );

        /******************************************************/
        // ������   : CameraGetRotate
        // �������� : ���ͼ�����ת״̬��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            iRot     ��ʾҪ��õ���ת����
        //               ����ʱ�뷽�򣩣�0������ת 1:90�� 2:180�� 3:270�ȣ�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetRotate(
	        CameraHandle    hCamera, 
	        out int         iRot 
	    );

        /******************************************************/
        // ������   : CameraGetWbWindow
        // �������� : ��ð�ƽ��ο����ڵ�λ�á�
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            PiHOff   ָ�룬���زο����ڵ����ϽǺ����� ��
        //            PiVOff     ָ�룬���زο����ڵ����Ͻ������� ��
        //            PiWidth    ָ�룬���زο����ڵĿ�ȡ�
        //            PiHeight   ָ�룬���زο����ڵĸ߶ȡ�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetWbWindow(
            CameraHandle hCamera,
        ref int PiHOff,
        ref int PiVOff,
        ref int PiWidth,
        ref int PiHeight
        );

        /******************************************************/
        // ������   : CameraSetWbWindow
        // �������� : ���ð�ƽ��ο����ڵ�λ�á�
        // ����     : hCamera ����ľ������CameraInit������á�
        //            iHOff   �ο����ڵ����ϽǺ����ꡣ
        //            iVOff     �ο����ڵ����Ͻ������ꡣ
        //            iWidth    �ο����ڵĿ�ȡ�
        //            iHeight   �ο����ڵĸ߶ȡ�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetWbWindow(
            CameraHandle hCamera,
            int iHOff,
            int iVOff,
            int iWidth,
            int iHeight
        );

        /******************************************************/
        // ������   : CameraIsWbWinVisible
        // �������� : ��ð�ƽ�ⴰ�ڵ���ʾ״̬��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            pbShow   ָ�룬����TRUE�����ʾ�����ǿɼ��ġ� 
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraIsWbWinVisible(
            CameraHandle hCamera,
        ref uint pbShow
        );

        /******************************************************/
        // ������   : CameraSetWbWinVisible
        // �������� : ���ð�ƽ�ⴰ�ڵ���ʾ״̬��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            bShow      TRUE�����ʾ����Ϊ�ɼ����ڵ���
        //             CameraImageOverlay��ͼ�������Ͻ��Ծ���
        //             �ķ�ʽ���Ӱ�ƽ��ο����ڵ�λ�á�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetWbWinVisible(
            CameraHandle hCamera,
            uint bShow
        );

        /******************************************************/
        // ������   : CameraImageOverlay
        // �������� : �������ͼ�������ϵ���ʮ���ߡ���ƽ��ο����ڡ�
        //        �Զ��ع�ο����ڵ�ͼ�Ρ�ֻ������Ϊ�ɼ�״̬��
        //        ʮ���ߺͲο����ڲ��ܱ������ϡ�
        //        ע�⣬�ú���������ͼ�������RGB��ʽ��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            pRgbBuffer ͼ�����ݻ�������
        //            pFrInfo    ͼ���֡ͷ��Ϣ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraImageOverlay(
            CameraHandle hCamera,
            IntPtr pRgbBuffer,
        ref tSdkFrameHead pFrInfo
        );

        /******************************************************/
        // ������   : CameraSetCrossLine
        // �������� : ����ָ��ʮ���ߵĲ�����
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            iLine    ��ʾҪ���õڼ���ʮ���ߵ�״̬����ΧΪ[0,8]����9����    
        //            x          ʮ��������λ�õĺ�����ֵ��
        //            y      ʮ��������λ�õ�������ֵ��
        //            uColor     ʮ���ߵ���ɫ����ʽΪ(R|(G<<8)|(B<<16))
        //            bVisible   ʮ���ߵ���ʾ״̬��TRUE����ʾ��ʾ��
        //             ֻ������Ϊ��ʾ״̬��ʮ���ߣ��ڵ���
        //             CameraImageOverlay��Żᱻ���ӵ�ͼ���ϡ�     
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetCrossLine(
            CameraHandle hCamera,
            int iLine,
            int x,
            int y,
            uint uColor,
            uint bVisible
        );

        /******************************************************/
        // ������   : CameraGetCrossLine
        // �������� : ���ָ��ʮ���ߵ�״̬��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            iLine    ��ʾҪ��ȡ�ĵڼ���ʮ���ߵ�״̬����ΧΪ[0,8]����9����  
        //            px     ָ�룬���ظ�ʮ��������λ�õĺ����ꡣ
        //            py     ָ�룬���ظ�ʮ��������λ�õĺ����ꡣ
        //            pcolor     ָ�룬���ظ�ʮ���ߵ���ɫ����ʽΪ(R|(G<<8)|(B<<16))��
        //            pbVisible  ָ�룬����TRUE�����ʾ��ʮ���߿ɼ���
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetCrossLine(
            CameraHandle hCamera,
            int iLine,
        ref int px,
        ref int py,
        ref uint pcolor,
        ref uint pbVisible
        );

        /******************************************************/
        // ������   : CameraGetCapability
        // �������� : �����������������ṹ�塣�ýṹ���а��������
        //        �����õĸ��ֲ����ķ�Χ��Ϣ����������غ����Ĳ���
        //        ���أ�Ҳ�����ڶ�̬������������ý��档
        // ����     : hCamera   ����ľ������CameraInit������á�
        //            pCameraInfo ָ�룬���ظ�������������Ľṹ�塣
        //                        tSdkCameraCapbility��CameraDefine.h�ж��塣
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetCapability(
            CameraHandle hCamera,
            IntPtr pCameraInfo
        );

        public static CameraSdkStatus CameraGetCapability(CameraHandle hCamera, IntPtr pCameraInfo)
        {
            return _CameraGetCapability(hCamera, pCameraInfo);
        }

        public static CameraSdkStatus CameraGetCapability(CameraHandle hCamera, out tSdkCameraCapbility cap)
        {
            CameraSdkStatus status;
            IntPtr ptr;

            ptr = Marshal.AllocHGlobal(Marshal.SizeOf(new tSdkCameraCapbility()));
            status = MvApi.CameraGetCapability(hCamera, ptr);
            cap = (tSdkCameraCapbility)Marshal.PtrToStructure(ptr, typeof(tSdkCameraCapbility));
            Marshal.FreeHGlobal(ptr);

            return status;
        }

        /******************************************************/
        // ������   : CameraWriteSN
        // �������� : ������������кš��ҹ�˾������кŷ�Ϊ3����
        //        0�������ҹ�˾�Զ����������кţ�����ʱ�Ѿ�
        //        �趨�ã�1����2���������ο���ʹ�á�ÿ������
        //        �ų��ȶ���32���ֽڡ�
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            pbySN    ���кŵĻ������� 
        //            iLevel   Ҫ�趨�����кż���ֻ����1����2��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraWriteSN(
            CameraHandle hCamera,
            Byte[] pbySN,
            int iLevel
        );

        /******************************************************/
        // ������   : CameraReadSN
        // �������� : ��ȡ���ָ����������кš����кŵĶ�����ο�
        //          CameraWriteSN�����Ĺ����������֡�
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            pbySN    ���кŵĻ�������
        //            iLevel     Ҫ��ȡ�����кż���ֻ����1��2��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraReadSN(
            CameraHandle hCamera,
            Byte[] pbySN,
            int iLevel
        );
        /******************************************************/
        // ������   : CameraSetTriggerDelayTime
        // �������� : ����Ӳ������ģʽ�µĴ�����ʱʱ�䣬��λ΢�롣
        //        ��Ӳ�����ź����ٺ󣬾���ָ������ʱ���ٿ�ʼ�ɼ�
        //        ͼ�񡣽������ͺŵ����֧�ָù��ܡ�������鿴
        //        ��Ʒ˵���顣
        // ����     : hCamera    ����ľ������CameraInit������á�
        //            uDelayTimeUs Ӳ������ʱ����λ΢�롣
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetTriggerDelayTime(
            CameraHandle hCamera,
            uint uDelayTimeUs
        );

        /******************************************************/
        // ������   : CameraGetTriggerDelayTime
        // �������� : ��õ�ǰ�趨��Ӳ������ʱʱ�䡣
        // ����     : hCamera     ����ľ������CameraInit������á�
        //            puDelayTimeUs ָ�룬������ʱʱ�䣬��λ΢�롣
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetTriggerDelayTime(
            CameraHandle hCamera,
        ref uint puDelayTimeUs
        );

        /******************************************************/
        // ������   : CameraSetTriggerCount
        // �������� : ���ô���ģʽ�µĴ���֡���������������Ӳ������
        //        ģʽ����Ч��Ĭ��Ϊ1֡����һ�δ����źŲɼ�һ֡ͼ��
        // ����     : hCamera ����ľ������CameraInit������á�
        //            iCount    һ�δ����ɼ���֡����
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetTriggerCount(
            CameraHandle hCamera,
            int iCount
        );

        /******************************************************/
        // ������   : CameraGetTriggerCount
        // �������� : ���һ�δ�����֡����
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            int* piCount
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetTriggerCount(
            CameraHandle hCamera,
        ref int piCount
        );

        /******************************************************/
        // ������   : CameraSoftTrigger
        // �������� : ִ��һ��������ִ�к󣬻ᴥ����CameraSetTriggerCount
        //          ָ����֡����
        // ����     : hCamera  ����ľ������CameraInit������á�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSoftTrigger(
            CameraHandle hCamera
        );

        /******************************************************/
        // ������   : CameraSetTriggerMode
        // �������� : ��������Ĵ���ģʽ��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            iModeSel   ģʽѡ�������š����趨��ģʽ��
        //             CameraGetCapability������ȡ����ο�
        //               CameraDefine.h��tSdkCameraCapbility�Ķ��塣
        //             һ�������0��ʾ�����ɼ�ģʽ��1��ʾ
        //             �������ģʽ��2��ʾӲ������ģʽ��  
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetTriggerMode(
            CameraHandle hCamera,
            int iModeSel
        );

        /******************************************************/
        // ������   : CameraGetTriggerMode
        // �������� : �������Ĵ���ģʽ��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            piModeSel  ָ�룬���ص�ǰѡ����������ģʽ�������š�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetTriggerMode(
            CameraHandle hCamera,
        ref int piModeSel
        );


        /******************************************************/
        // ������ 	: CameraSetStrobeMode
        // ��������	: ����IO���Ŷ����ϵ�STROBE�źš����źſ���������ƿ��ƣ�Ҳ�������ⲿ��е���ſ��ơ�
        // ����	    : hCamera ����ľ������CameraInit������á�
        //             iMode   ��ΪSTROBE_SYNC_WITH_TRIG_AUTO      �ʹ����ź�ͬ������������������ع�ʱ���Զ�����STROBE�źš�
        //                                                         ��ʱ����Ч���Կ�����(CameraSetStrobePolarity)��
        //                     ��ΪSTROBE_SYNC_WITH_TRIG_MANUALʱ���ʹ����ź�ͬ����������STROBE��ʱָ����ʱ���(CameraSetStrobeDelayTime)��
        //                                                         �ٳ���ָ��ʱ�������(CameraSetStrobePulseWidth)��
        //                                                         ��Ч���Կ�����(CameraSetStrobePolarity)��
        //                     ��ΪSTROBE_ALWAYS_HIGHʱ��STROBE�źź�Ϊ��,������������
        //                     ��ΪSTROBE_ALWAYS_LOWʱ��STROBE�źź�Ϊ��,������������
        //
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetStrobeMode(
            CameraHandle hCamera,
            int iMode
        );

        /******************************************************/
        // ������ 	: CameraGetStrobeMode
        // ��������	: ���ߵ�ǰSTROBE�ź����õ�ģʽ��
        // ����	    : hCamera ����ľ������CameraInit������á�
        //             piMode  ָ�룬����STROBE_SYNC_WITH_TRIG_AUTO,STROBE_SYNC_WITH_TRIG_MANUAL��STROBE_ALWAYS_HIGH����STROBE_ALWAYS_LOW��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetStrobeMode(
            CameraHandle hCamera,
        ref int piMode
        );

        /******************************************************/
        // ������ 	: CameraSetStrobeDelayTime
        // ��������	: ��STROBE�źŴ���STROBE_SYNC_WITH_TRIGʱ��ͨ���ú�����������Դ����ź���ʱʱ�䡣
        // ����	    : hCamera       ����ľ������CameraInit������á�
        //             uDelayTimeUs  ��Դ����źŵ���ʱʱ�䣬��λΪus������Ϊ0��������Ϊ������ 
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetStrobeDelayTime(
            CameraHandle hCamera,
            uint uDelayTimeUs
        );

        /******************************************************/
        // ������ 	: CameraGetStrobeDelayTime
        // ��������	: ��STROBE�źŴ���STROBE_SYNC_WITH_TRIGʱ��ͨ���ú����������Դ����ź���ʱʱ�䡣
        // ����	    : hCamera           ����ľ������CameraInit������á�
        //             upDelayTimeUs     ָ�룬������ʱʱ�䣬��λus��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetStrobeDelayTime(
            CameraHandle hCamera,
        ref uint upDelayTimeUs
        );

        /******************************************************/
        // ������ 	: CameraSetStrobePulseWidth
        // ��������	: ��STROBE�źŴ���STROBE_SYNC_WITH_TRIGʱ��ͨ���ú��������������ȡ�
        // ����	    : hCamera       ����ľ������CameraInit������á�
        //             uTimeUs       ����Ŀ�ȣ���λΪʱ��us��  
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetStrobePulseWidth(
            CameraHandle hCamera,
            uint uTimeUs
        );

        /******************************************************/
        // ������ 	: CameraGetStrobePulseWidth
        // ��������	: ��STROBE�źŴ���STROBE_SYNC_WITH_TRIGʱ��ͨ���ú�������������ȡ�
        // ����	    : hCamera   ����ľ������CameraInit������á�
        //             upTimeUs  ָ�룬���������ȡ���λΪʱ��us��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetStrobePulseWidth(
            CameraHandle hCamera,
        ref uint upTimeUs
        );

        /******************************************************/
        // ������ 	: CameraSetStrobePolarity
        // ��������	: ��STROBE�źŴ���STROBE_SYNC_WITH_TRIGʱ��ͨ���ú�����������Ч��ƽ�ļ��ԡ�Ĭ��Ϊ����Ч���������źŵ���ʱ��STROBE�źű����ߡ�
        // ����	    : hCamera   ����ľ������CameraInit������á�
        //             iPolarity STROBE�źŵļ��ԣ�0Ϊ�͵�ƽ��Ч��1Ϊ�ߵ�ƽ��Ч��Ĭ��Ϊ�ߵ�ƽ��Ч��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetStrobePolarity(
            CameraHandle hCamera,
            int uPolarity
        );

        /******************************************************/
        // ������ 	: CameraGetStrobePolarity
        // ��������	: ��������ǰSTROBE�źŵ���Ч���ԡ�Ĭ��Ϊ�ߵ�ƽ��Ч��
        // ����	    : hCamera       ����ľ������CameraInit������á�
        //             ipPolarity    ָ�룬����STROBE�źŵ�ǰ����Ч���ԡ�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetStrobePolarity(
            CameraHandle hCamera,
        ref int upPolarity
        );

        /******************************************************/
        // ������ 	: CameraSetExtTrigSignalType
        // ��������	: ��������ⴥ���źŵ����ࡣ�ϱ��ء��±��ء����߸ߡ��͵�ƽ��ʽ��
        // ����	    : hCamera   ����ľ������CameraInit������á�
        //             iType     �ⴥ���ź����࣬����ֵ�ο�CameraDefine.h��
        //                       emExtTrigSignal���Ͷ��塣
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetExtTrigSignalType(
            CameraHandle hCamera,
            int iType
        );

        /******************************************************/
        // ������ 	: CameraGetExtTrigSignalType
        // ��������	: ��������ǰ�ⴥ���źŵ����ࡣ
        // ����	    : hCamera   ����ľ������CameraInit������á�
        //             ipType    ָ�룬�����ⴥ���ź����࣬����ֵ�ο�CameraDefine.h��
        //                       emExtTrigSignal���Ͷ��塣
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetExtTrigSignalType(
            CameraHandle hCamera,
        ref int ipType
        );

        /******************************************************/
        // ������ 	: CameraSetExtTrigShutterType
        // ��������	: �����ⴥ��ģʽ�£�������ŵķ�ʽ��Ĭ��Ϊ��׼���ŷ�ʽ��
        //              ���ֹ������ŵ�CMOS���֧��GRR��ʽ��
        // ����	    : hCamera   ����ľ������CameraInit������á�
        //             iType     �ⴥ�����ŷ�ʽ���ο�CameraDefine.h��emExtTrigShutterMode���͡�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetExtTrigShutterType(
            CameraHandle hCamera,
            int iType
        );

        /******************************************************/
        // ������ 	: CameraSetExtTrigShutterType
        // ��������	: ����ⴥ��ģʽ�£�������ŵķ�ʽ��Ĭ��Ϊ��׼���ŷ�ʽ��
        //              ���ֹ������ŵ�CMOS���֧��GRR��ʽ��
        // ����	    : hCamera   ����ľ������CameraInit������á�
        //             ipType    ָ�룬���ص�ǰ�趨���ⴥ�����ŷ�ʽ������ֵ�ο�
        //                       CameraDefine.h��emExtTrigShutterMode���͡�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetExtTrigShutterType(
            CameraHandle hCamera,
        ref int ipType
        );

        /******************************************************/
        // ������ 	: CameraSetExtTrigDelayTime
        // ��������	: �����ⴥ���ź���ʱʱ�䣬Ĭ��Ϊ0����λΪ΢�롣 
        //              �����õ�ֵuDelayTimeUs��Ϊ0ʱ��������յ��ⴥ���źź󣬽���ʱuDelayTimeUs��΢����ٽ���ͼ�񲶻�
        // ����	    : hCamera       ����ľ������CameraInit������á�
        //             uDelayTimeUs  ��ʱʱ�䣬��λΪ΢�룬Ĭ��Ϊ0.
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetExtTrigDelayTime(
            CameraHandle hCamera,
            uint uDelayTimeUs
        );

        /******************************************************/
        // ������ 	: CameraGetExtTrigDelayTime
        // ��������	: ������õ��ⴥ���ź���ʱʱ�䣬Ĭ��Ϊ0����λΪ΢�롣 
        // ����	    : hCamera   ����ľ������CameraInit������á�
        //            ref uint  upDelayTimeUs
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetExtTrigDelayTime(
            CameraHandle hCamera,
        ref uint upDelayTimeUs
        );

        /******************************************************/
        // ������ 	: CameraSetExtTrigJitterTime
        // ��������	: ��������ⴥ���źŵ�����ʱ�䡣Ĭ��Ϊ0����λΪ΢�롣
        // ����	    : hCamera   ����ľ������CameraInit������á�
        //            uint uTimeUs
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetExtTrigJitterTime(
            CameraHandle hCamera,
            uint uTimeUs
        );

        /******************************************************/
        // ������ 	: CameraGetExtTrigJitterTime
        // ��������	: ������õ�����ⴥ������ʱ�䣬Ĭ��Ϊ0.��λΪ΢��
        // ����	    : hCamera   ����ľ������CameraInit������á�
        //            ref uint      upTimeUs
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetExtTrigJitterTime(
            CameraHandle hCamera,
        ref uint upTimeUs
        );

        /******************************************************/
        // ������ 	: CameraGetExtTrigCapability
        // ��������	: �������ⴥ������������
        // ����	    : hCamera           ����ľ������CameraInit������á�
        //             puCapabilityMask  ָ�룬���ظ�����ⴥ���������룬����ο�CameraDefine.h��
        //                               EXT_TRIG_MASK_ ��ͷ�ĺ궨�塣   
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetExtTrigCapability(
            CameraHandle hCamera,
        ref uint puCapabilityMask
        );

        public delegate CameraSdkStatus pfnCameraPauseLevelTrigger(
	        CameraHandle hCamera
	    );

        /******************************************************/
        // ������   : CameraGetResolutionForSnap
        // �������� : ���ץ��ģʽ�µķֱ���ѡ�������š�
        // ����     : hCamera        ����ľ������CameraInit������á�
        //            pImageResolution ָ�룬����ץ��ģʽ�ķֱ��ʡ� 
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetResolutionForSnap(
            CameraHandle hCamera,
        ref tSdkImageResolution pImageResolution
        );

        /******************************************************/
        // ������   : CameraSetResolutionForSnap
        // �������� : ����ץ��ģʽ��������ͼ��ķֱ��ʡ�
        // ����     : hCamera       ����ľ������CameraInit������á�
        //            pImageResolution ���pImageResolution->iWidth 
        //                 �� pImageResolution->iHeight��Ϊ0��
        //                         ���ʾ�趨Ϊ���浱ǰԤ���ֱ��ʡ�ץ
        //                         �µ���ͼ��ķֱ��ʻ�͵�ǰ�趨�� 
        //                 Ԥ���ֱ���һ����
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetResolutionForSnap(
            CameraHandle hCamera,
        ref tSdkImageResolution pImageResolution
        );

        /******************************************************/
        // ������   : CameraCustomizeResolution
        // �������� : �򿪷ֱ����Զ�����壬��ͨ�����ӻ��ķ�ʽ
        //        ������һ���Զ���ֱ��ʡ�
        // ����     : hCamera    ����ľ������CameraInit������á�
        //            pImageCustom ָ�룬�����Զ���ķֱ��ʡ�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraCustomizeResolution(
            CameraHandle hCamera,
        ref tSdkImageResolution pImageCustom
        );

        /******************************************************/
        // ������   : CameraCustomizeReferWin
        // �������� : �򿪲ο������Զ�����塣��ͨ�����ӻ��ķ�ʽ��
        //        ���һ���Զ��崰�ڵ�λ�á�һ�������Զ����ƽ��
        //        ���Զ��ع�Ĳο����ڡ�
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            iWintype   Ҫ���ɵĲο����ڵ���;��0,�Զ��ع�ο����ڣ�
        //             1,��ƽ��ο����ڡ�
        //            hParent    ���øú����Ĵ��ڵľ��������ΪNULL��
        //            piHOff     ָ�룬�����Զ��崰�ڵ����ϽǺ����ꡣ
        //            piVOff     ָ�룬�����Զ��崰�ڵ����Ͻ������ꡣ
        //            piWidth    ָ�룬�����Զ��崰�ڵĿ�ȡ� 
        //            piHeight   ָ�룬�����Զ��崰�ڵĸ߶ȡ�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraCustomizeReferWin(
            CameraHandle hCamera,
            int iWintype,
            IntPtr hParent,
        ref int piHOff,
        ref int piVOff,
        ref int piWidth,
        ref int piHeight
        );

        /******************************************************/
        // ������   : CameraShowSettingPage
        // �������� : ��������������ô�����ʾ״̬�������ȵ���CameraCreateSettingPage
        //        �ɹ���������������ô��ں󣬲��ܵ��ñ���������
        //        ��ʾ��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            bShow    TRUE����ʾ;FALSE�����ء�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraShowSettingPage(
            CameraHandle hCamera,
            uint bShow
        );

        /******************************************************/
        // ������   : CameraCreateSettingPage
        // �������� : ������������������ô��ڡ����øú�����SDK�ڲ���
        //        ������������������ô��ڣ�ʡȥ�������¿������
        //        ���ý����ʱ�䡣ǿ�ҽ���ʹ����ʹ�øú�����
        //        SDKΪ�����������ô��ڡ�
        // ����     : hCamera     ����ľ������CameraInit������á�
        //            hParent       Ӧ�ó��������ڵľ��������ΪNULL��
        //            pWintext      �ַ���ָ�룬������ʾ�ı�������
        //            pCallbackFunc ������Ϣ�Ļص�����������Ӧ���¼�����ʱ��
        //              pCallbackFunc��ָ��ĺ����ᱻ���ã�
        //              �����л��˲���֮��Ĳ���ʱ��pCallbackFunc
        //              ���ص�ʱ������ڲ�����ָ������Ϣ���͡�
        //              �������Է������Լ������Ľ�����������ɵ�UI
        //              ֮�����ͬ�����ò�������ΪNULL��    
        //            pCallbackCtx  �ص������ĸ��Ӳ���������ΪNULL��pCallbackCtx
        //              ����pCallbackFunc���ص�ʱ����Ϊ����֮һ���롣
        //              ������ʹ�øò�������һЩ�����жϡ�
        //            uReserved     Ԥ������������Ϊ0��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraCreateSettingPage(
            CameraHandle hCamera,
            IntPtr hParent,
            byte[] pWintext,
            CAMERA_PAGE_MSG_PROC pCallbackFunc,
            IntPtr pCallbackCtx,
            uint uReserved
        );

        public static CameraSdkStatus CameraCreateSettingPage(
            CameraHandle hCamera,
            IntPtr hParent,
            byte[] pWintext,
            CAMERA_PAGE_MSG_PROC pCallbackFunc,
            IntPtr pCallbackCtx,
            uint uReserved
        )
        {
            return _CameraCreateSettingPage(hCamera, hParent, pWintext, pCallbackFunc, pCallbackCtx, uReserved);
        }

        public static CameraSdkStatus CameraCreateSettingPage(
            CameraHandle hCamera,
            IntPtr hParent,
            string strWintext,
            CAMERA_PAGE_MSG_PROC pCallbackFunc,
            IntPtr pCallbackCtx,
            uint uReserved
        )
        {
            return CameraCreateSettingPage(hCamera, hParent, StringToCStr(strWintext), pCallbackFunc, pCallbackCtx, uReserved);
        }

        /******************************************************/
        // ������   : CameraSetActiveSettingSubPage
        // �������� : ����������ô��ڵļ���ҳ�档������ô����ж��
        //        ��ҳ�湹�ɣ��ú��������趨��ǰ��һ����ҳ��
        //        Ϊ����״̬����ʾ����ǰ�ˡ�
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            index      ��ҳ��������š��ο�CameraDefine.h��
        //             PROP_SHEET_INDEX�Ķ��塣
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetActiveSettingSubPage(
            CameraHandle hCamera,
            int index
        );

        public delegate CameraSdkStatus pfnCameraSetSettingPageParent(
	        CameraHandle    hCamera,
	        IntPtr          hParentWnd,
	        uint			Flags
	    );

        public delegate CameraSdkStatus pfnCameraGetSettingPageHWnd(
	        CameraHandle    hCamera,
	        out IntPtr      hWnd
	    );

        /******************************************************/
        // ������   : CameraSpecialControl
        // �������� : ���һЩ�������������õĽӿڣ����ο���ʱһ�㲻��Ҫ
        //        ���á�
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            dwCtrlCode �����롣
        //            dwParam    �������룬��ͬ��dwCtrlCodeʱ�����岻ͬ��
        //            lpData     ���Ӳ�������ͬ��dwCtrlCodeʱ�����岻ͬ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSpecialControl(
            CameraHandle hCamera,
            uint dwCtrlCode,
            uint dwParam,
            IntPtr lpData
        );

        /******************************************************/
        // ������   : CameraGetFrameStatistic
        // �������� : ����������֡�ʵ�ͳ����Ϣ����������֡�Ͷ�֡�������
        // ����     : hCamera        ����ľ������CameraInit������á�
        //            psFrameStatistic ָ�룬����ͳ����Ϣ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetFrameStatistic(
            CameraHandle hCamera,
        out tSdkFrameStatistic psFrameStatistic
        );

        /******************************************************/
        // ������   : CameraSetNoiseFilter
        // �������� : ����ͼ����ģ���ʹ��״̬��
        // ����     : hCamera ����ľ������CameraInit������á�
        //            bEnable   TRUE��ʹ�ܣ�FALSE����ֹ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetNoiseFilter(
            CameraHandle hCamera,
            uint bEnable
        );

        /******************************************************/
        // ������   : CameraGetNoiseFilterState
        // �������� : ���ͼ����ģ���ʹ��״̬��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //            *pEnable   ָ�룬����״̬��TRUE��Ϊʹ�ܡ�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetNoiseFilterState(
            CameraHandle hCamera,
        ref uint pEnable
        );

        /******************************************************/
        // ������   : CameraRstTimeStamp
        // �������� : ��λͼ��ɼ���ʱ�������0��ʼ��
        // ����     : CameraHandle hCamera
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraRstTimeStamp(
            CameraHandle hCamera
        );

        /******************************************************/
        // ������   : CameraSaveUserData
        // �������� : ���û��Զ�������ݱ��浽����ķ����Դ洢���С�
        //              ÿ���ͺŵ��������֧�ֵ��û���������󳤶Ȳ�һ����
        //              ���Դ��豸�����������л�ȡ�ó�����Ϣ��
        // ����     : hCamera    ����ľ������CameraInit������á�
        //            uStartAddr  ��ʼ��ַ����0��ʼ��
        //            pbData      ���ݻ�����ָ��
        //            ilen        д�����ݵĳ��ȣ�ilen + uStartAddr����
        //                        С���û�����󳤶�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSaveUserData(
            CameraHandle hCamera,
            uint uStartAddr,
            Byte[] pbData,
            int ilen
        );

        /******************************************************/
        // ������   : CameraLoadUserData
        // �������� : ������ķ����Դ洢���ж�ȡ�û��Զ�������ݡ�
        //              ÿ���ͺŵ��������֧�ֵ��û���������󳤶Ȳ�һ����
        //              ���Դ��豸�����������л�ȡ�ó�����Ϣ��
        // ����     : hCamera    ����ľ������CameraInit������á�
        //            uStartAddr  ��ʼ��ַ����0��ʼ��
        //            pbData      ���ݻ�����ָ�룬���ض��������ݡ�
        //            ilen        ��ȡ���ݵĳ��ȣ�ilen + uStartAddr����
        //                        С���û�����󳤶�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraLoadUserData(
            CameraHandle hCamera,
            uint uStartAddr,
            Byte[] pbData,
            int ilen
        );

        /******************************************************/
        // ������ : CameraGetFriendlyName
        // �������� : ��ȡ�û��Զ�����豸�ǳơ�
        // ����   : hCamera  ����ľ������CameraInit������á�
        //        pName    ָ�룬����ָ��0��β���ַ�����
        //             �豸�ǳƲ�����32���ֽڣ���˸�ָ��
        //             ָ��Ļ�����������ڵ���32���ֽڿռ䡣
        // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //        �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetFriendlyName(
          CameraHandle hCamera,
          Byte[] pName
        );

        /******************************************************/
        // ������ : CameraSetFriendlyName
        // �������� : �����û��Զ�����豸�ǳơ�
        // ����   : hCamera  ����ľ������CameraInit������á�
        //        pName    ָ�룬ָ��0��β���ַ�����
        //             �豸�ǳƲ�����32���ֽڣ���˸�ָ��
        //             ָ���ַ�������С�ڵ���32���ֽڿռ䡣
        // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //        �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetFriendlyName(
          CameraHandle hCamera,
          Byte[] pName
        );

        /******************************************************/
        // ������ : CameraSdkGetVersionString
        // �������� : 
        // ����   : pVersionString ָ�룬����SDK�汾�ַ�����
        //                ��ָ��ָ��Ļ�������С�������
        //                32���ֽ�
        // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //        �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSdkGetVersionString(
            Byte[] pVersionString
        );

        /******************************************************/
        // ������ : CameraCheckFwUpdate
        // �������� : ���̼��汾���Ƿ���Ҫ������
        // ����   : hCamera ����ľ������CameraInit������á�
        //        pNeedUpdate ָ�룬���ع̼����״̬��TRUE��ʾ��Ҫ����
        // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //        �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraCheckFwUpdate(
          CameraHandle hCamera,
        ref uint pNeedUpdate
        );

        /******************************************************/
        // ������ : CameraGetFirmwareVision
        // �������� : ��ù̼��汾���ַ���
        // ����   : hCamera ����ľ������CameraInit������á�
        //        pVersion ����ָ��һ������32�ֽڵĻ�������
        //            ���ع̼��İ汾�ַ�����
        // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //        �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetFirmwareVision(
          CameraHandle hCamera,
          Byte[] pVersion
        );

        /******************************************************/
        // ������ : CameraGetEnumInfo
        // �������� : ���ָ���豸��ö����Ϣ
        // ����   : hCamera ����ľ������CameraInit������á�
        //        pCameraInfo ָ�룬�����豸��ö����Ϣ��
        // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //        �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetEnumInfo(
          CameraHandle hCamera,
        ref tSdkCameraDevInfo pCameraInfo
        );

        /******************************************************/
        // ������ : CameraGetInerfaceVersion
        // �������� : ���ָ���豸�ӿڵİ汾
        // ����   : hCamera ����ľ������CameraInit������á�
        //        pVersion ָ��һ������32�ֽڵĻ����������ؽӿڰ汾�ַ�����
        // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //        �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetInerfaceVersion(
          CameraHandle hCamera,
          Byte[] pVersion
        );

        /******************************************************/
        // ������ : CameraSetIOState
        // �������� : ����ָ��IO�ĵ�ƽ״̬��IOΪ�����IO�����
        //        Ԥ���ɱ�����IO�ĸ�����tSdkCameraCapbility��
        //        iOutputIoCounts������
        // ����   : hCamera ����ľ������CameraInit������á�
        //        iOutputIOIndex IO�������ţ���0��ʼ��
        //        uState Ҫ�趨��״̬��1Ϊ�ߣ�0Ϊ��
        // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //        �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetIOState(
          CameraHandle hCamera,
          int iOutputIOIndex,
          uint uState
        );

        /******************************************************/
        // ������ : CameraGetIOState
        // �������� : ����ָ��IO�ĵ�ƽ״̬��IOΪ������IO�����
        //        Ԥ���ɱ�����IO�ĸ�����tSdkCameraCapbility��
        //        iInputIoCounts������
        // ����   : hCamera ����ľ������CameraInit������á�      
        //        iInputIOIndex IO�������ţ���0��ʼ��
        //        puState ָ�룬����IO״̬,1Ϊ�ߣ�0Ϊ��
        // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //        �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetIOState(
          CameraHandle hCamera,
          int iInputIOIndex,
        ref uint puState
        );

        /******************************************************/
        // ������   : CameraSetInPutIOMode
        // �������� : ��������IO��ģʽ�����
        //              Ԥ���ɱ�����IO�ĸ�����tSdkCameraCapbility��
        //              iInputIoCounts������
        // ����     : hCamera ����ľ������CameraInit������á�          
        //            iInputIOIndex IO�������ţ���0��ʼ��
        //            iMode IOģʽ,�ο�CameraDefine.h��emCameraGPIOMode
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetInPutIOMode(
			CameraHandle    hCamera,
			int         iInputIOIndex,
			int			iMode
			);

		/******************************************************/
		// ������   : CameraSetOutPutIOMode
		// �������� : �������IO��ģʽ�����
		//              Ԥ���ɱ�����IO�ĸ�����tSdkCameraCapbility��
		//              iOutputIoCounts������
		// ����     : hCamera ����ľ������CameraInit������á�          
		//            iOutputIOIndex IO�������ţ���0��ʼ��
		//            iMode IOģʽ,�ο�CameraDefine.h��emCameraGPIOMode
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraSetOutPutIOMode(
			CameraHandle    hCamera,
			int         iOutputIOIndex,
			int			iMode
			);

		/******************************************************/
		// ������   : CameraSetOutPutPWM
		// �������� : ����PWM������Ĳ��������
		//              Ԥ���ɱ�����IO�ĸ�����tSdkCameraCapbility��
		//              iOutputIoCounts������
		// ����     : hCamera ����ľ������CameraInit������á�          
		//            iOutputIOIndex IO�������ţ���0��ʼ��
		//            iCycle PWM�����ڣ���λ(us)
		//			  uDuty  ռ�ñȣ�ȡֵ1%~99%
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraSetOutPutPWM(
			CameraHandle    hCamera,
			int         iOutputIOIndex,
			uint		iCycle,
			uint		uDuty
			);

        /******************************************************/
        // ������ : CameraSetAeAlgorithm
        // �������� : �����Զ��ع�ʱѡ����㷨����ͬ���㷨������
        //        ��ͬ�ĳ�����
        // ����   : hCamera     ����ľ������CameraInit������á� 
        //        iIspProcessor   ѡ��ִ�и��㷨�Ķ��󣬲ο�CameraDefine.h
        //                emSdkIspProcessor�Ķ���
        //        iAeAlgorithmSel Ҫѡ����㷨��š���0��ʼ�����ֵ��tSdkCameraCapbility
        //                ��iAeAlmSwDesc��iAeAlmHdDesc������  
        // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //        �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetAeAlgorithm(
          CameraHandle hCamera,
          int iIspProcessor,
          int iAeAlgorithmSel
        );

        /******************************************************/
        // ������ : CameraGetAeAlgorithm
        // �������� : ��õ�ǰ�Զ��ع���ѡ����㷨
        // ����   : hCamera     ����ľ������CameraInit������á� 
        //        iIspProcessor   ѡ��ִ�и��㷨�Ķ��󣬲ο�CameraDefine.h
        //                emSdkIspProcessor�Ķ���
        //        piAeAlgorithmSel  ���ص�ǰѡ����㷨��š���0��ʼ�����ֵ��tSdkCameraCapbility
        //                ��iAeAlmSwDesc��iAeAlmHdDesc������  
        // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //        �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetAeAlgorithm(
          CameraHandle hCamera,
          int iIspProcessor,
        ref int piAlgorithmSel
        );

        /******************************************************/
        // ������ : CameraSetBayerDecAlgorithm
        // �������� : ����Bayer����ת��ɫ���㷨��
        // ����   : hCamera     ����ľ������CameraInit������á� 
        //        iIspProcessor   ѡ��ִ�и��㷨�Ķ��󣬲ο�CameraDefine.h
        //                emSdkIspProcessor�Ķ���
        //        iAlgorithmSel   Ҫѡ����㷨��š���0��ʼ�����ֵ��tSdkCameraCapbility
        //                ��iBayerDecAlmSwDesc��iBayerDecAlmHdDesc������    
        // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //        �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetBayerDecAlgorithm(
          CameraHandle hCamera,
          int iIspProcessor,
          int iAlgorithmSel
        );

        /******************************************************/
        // ������ : CameraGetBayerDecAlgorithm
        // �������� : ���Bayer����ת��ɫ��ѡ����㷨��
        // ����   : hCamera     ����ľ������CameraInit������á� 
        //        iIspProcessor   ѡ��ִ�и��㷨�Ķ��󣬲ο�CameraDefine.h
        //                emSdkIspProcessor�Ķ���
        //        piAlgorithmSel  ���ص�ǰѡ����㷨��š���0��ʼ�����ֵ��tSdkCameraCapbility
        //                ��iBayerDecAlmSwDesc��iBayerDecAlmHdDesc������  
        // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //        �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetBayerDecAlgorithm(
          CameraHandle hCamera,
          int iIspProcessor,
        ref int piAlgorithmSel
        );

        /******************************************************/
        // ������ : CameraSetIspProcessor
        // �������� : ����ͼ����Ԫ���㷨ִ�ж�����PC�˻��������
        //        ��ִ���㷨�����������ִ��ʱ���ή��PC�˵�CPUռ���ʡ�
        // ����   : hCamera   ����ľ������CameraInit������á� 
        //        iIspProcessor �ο�CameraDefine.h��
        //              emSdkIspProcessor�Ķ��塣
        // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //        �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetIspProcessor(
          CameraHandle hCamera,
          int iIspProcessor
        );

        /******************************************************/
        // ������ : CameraGetIspProcessor
        // �������� : ���ͼ����Ԫ���㷨ִ�ж���
        // ����   : hCamera    ����ľ������CameraInit������á� 
        //        piIspProcessor ����ѡ��Ķ��󣬷���ֵ�ο�CameraDefine.h��
        //               emSdkIspProcessor�Ķ��塣
        // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //        �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetIspProcessor(
          CameraHandle hCamera,
        ref int piIspProcessor
        );

        /******************************************************/
        // ������ : CameraSetBlackLevel
        // �������� : ����ͼ��ĺڵ�ƽ��׼��Ĭ��ֵΪ0
        // ����   : hCamera   ����ľ������CameraInit������á� 
        //        iBlackLevel Ҫ�趨�ĵ�ƽֵ����ΧΪ0��255��  
        // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //        �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetBlackLevel(
          CameraHandle hCamera,
          int iBlackLevel
        );

        /******************************************************/
        // ������ : CameraGetBlackLevel
        // �������� : ���ͼ��ĺڵ�ƽ��׼��Ĭ��ֵΪ0
        // ����   : hCamera    ����ľ������CameraInit������á� 
        //        piBlackLevel ���ص�ǰ�ĺڵ�ƽֵ����ΧΪ0��255��
        // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //        �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetBlackLevel(
          CameraHandle hCamera,
        ref int piBlackLevel
        );

        /******************************************************/
        // ������ : CameraSetWhiteLevel
        // �������� : ����ͼ��İ׵�ƽ��׼��Ĭ��ֵΪ255
        // ����   : hCamera   ����ľ������CameraInit������á� 
        //        iWhiteLevel Ҫ�趨�ĵ�ƽֵ����ΧΪ0��255��  
        // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //        �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetWhiteLevel(
          CameraHandle hCamera,
          int iWhiteLevel
        );

        /******************************************************/
        // ������ : CameraGetWhiteLevel
        // �������� : ���ͼ��İ׵�ƽ��׼��Ĭ��ֵΪ255
        // ����   : hCamera    ����ľ������CameraInit������á� 
        //        piWhiteLevel ���ص�ǰ�İ׵�ƽֵ����ΧΪ0��255��
        // ����ֵ : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //        ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraSdkStatus�����Ͷ���
        //        �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetWhiteLevel(
            CameraHandle hCamera,
            ref int piWhiteLevel
        );

        /******************************************************/
		// ������   : CameraIsOpened
		// �������� : ����豸�Ƿ��Ѿ�������Ӧ�ó���򿪡��ڵ���CameraInit
		//        ֮ǰ������ʹ�øú������м�⣬����Ѿ����򿪣�����
		//        CameraInit�᷵���豸�Ѿ����򿪵Ĵ����롣    
		// ����     : pCameraList �豸��ö����Ϣ�ṹ��ָ�룬��CameraEnumerateDevice��á�
		//            pOpened       �豸��״ָ̬�룬�����豸�Ƿ񱻴򿪵�״̬��TRUEΪ�򿪣�FALSEΪ���С�          
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraIsOpened(
		    ref tSdkCameraDevInfo pCameraList,
		    out bool pOpened
		);

        /******************************************************/
        // ������   : CameraEnumerateDeviceEx
        // �������� : ö���豸���������豸�б��ڵ���CameraInitEx
        //            ֮ǰ��������øú���ö���豸��
        // ����      : 
        // ����ֵ     : �����豸������0��ʾ�ޡ�
        /******************************************************/
        public delegate int pfnCameraEnumerateDeviceEx(
        );


        /******************************************************/
        // ������ 	: CameraInitEx
        // ��������	: �����ʼ������ʼ���ɹ��󣬲��ܵ����κ�����
        //			  �����صĲ����ӿڡ�		
        // ����	    : iDeviceIndex    ����������ţ�CameraEnumerateDeviceEx�������������	
        //            iParamLoadMode  �����ʼ��ʱʹ�õĲ������ط�ʽ��-1��ʾʹ���ϴ��˳�ʱ�Ĳ������ط�ʽ��
        //            emTeam         ��ʼ��ʱʹ�õĲ����顣-1��ʾ�����ϴ��˳�ʱ�Ĳ����顣
        //            pCameraHandle  ����ľ��ָ�룬��ʼ���ɹ��󣬸�ָ��
        //							 ���ظ��������Ч������ڵ����������
        //							 ��صĲ����ӿ�ʱ������Ҫ����þ������Ҫ
        //							 ���ڶ����֮������֡�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraInitEx(
            int iDeviceIndex,
            int iParamLoadMode,
            int emTeam,
        ref CameraHandle pCameraHandle
        );


        /******************************************************/
        // ������ 	: CameraInitEx2
        // ��������	: �����ʼ������ʼ���ɹ��󣬲��ܵ����κ�����
        //			  �����صĲ����ӿڡ�		
        // ����	    : CameraName     �������
        //            pCameraHandle  ����ľ��ָ�룬��ʼ���ɹ��󣬸�ָ��
        //							 ���ظ��������Ч������ڵ����������
        //							 ��صĲ����ӿ�ʱ������Ҫ����þ������Ҫ
        //							 ���ڶ����֮������֡�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraInitEx2(
            string CameraName,
            out CameraHandle pCameraHandle
        );

        /******************************************************/
        // ������   : CameraGetImageBufferEx
        // �������� : ���һ֡ͼ�����ݡ��ýӿڻ�õ�ͼ���Ǿ���������RGB��ʽ���ú������ú�
        //            ����Ҫ���� CameraReleaseImageBuffer �ͷţ�Ҳ��Ҫ����free֮��ĺ����ͷ�
        //              ���ͷŸú������ص�ͼ�����ݻ�������
        // ����     : hCamera     ����ľ������CameraInit������á�
        //            piWidth    ����ָ�룬����ͼ��Ŀ��
        //            piHeight   ����ָ�룬����ͼ��ĸ߶�
        //            uint wTimes ץȡͼ��ĳ�ʱʱ�䡣��λ���롣��
        //                        wTimesʱ���ڻ�δ���ͼ����ú���
        //                        �᷵�س�ʱ��Ϣ��
        // ����ֵ   : �ɹ�ʱ������RGB���ݻ��������׵�ַ;
        //            ���򷵻�0��
        /******************************************************/
        public delegate IntPtr pfnCameraGetImageBufferEx(
            CameraHandle hCamera,
        ref int piWidth,
        ref int piHeight,
            uint wTimes
        );

        /******************************************************/
        // ������   : CameraImageProcessEx
        // �������� : ����õ����ԭʼ���ͼ�����ݽ��д������ӱ��Ͷȡ�
        //            ��ɫ�����У��������ȴ���Ч�������õ�RGB888
        //            ��ʽ��ͼ�����ݡ�  
        // ����     : hCamera      ����ľ������CameraInit������á�
        //            pbyIn      ����ͼ�����ݵĻ�������ַ������ΪNULL�� 
        //            pbyOut        �����ͼ������Ļ�������ַ������ΪNULL��
        //            pFrInfo       ����ͼ���֡ͷ��Ϣ��������ɺ�֡ͷ��Ϣ
        //            uOutFormat    �������ͼ��������ʽ������CAMERA_MEDIA_TYPE_MONO8 CAMERA_MEDIA_TYPE_RGB CAMERA_MEDIA_TYPE_RGBA8������һ�֡�
        //                          pbyIn��Ӧ�Ļ�������С�������uOutFormatָ���ĸ�ʽ��ƥ�䡣
        //            uReserved     Ԥ����������������Ϊ0     
        //                     �е�ͼ���ʽuiMediaType����֮�ı䡣
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraImageProcessEx(
            CameraHandle hCamera,
            IntPtr pbyIn,
            IntPtr pbyOut,
        ref tSdkFrameHead pFrInfo,
            uint uOutFormat,
            uint uReserved
        );

        /******************************************************/
        // ������ 	: CameraSetIspOutFormat
        // ��������	: ����CameraGetImageBuffer������ͼ����������ʽ��֧��
        //              CAMERA_MEDIA_TYPE_MONO8��CAMERA_MEDIA_TYPE_RGB8��CAMERA_MEDIA_TYPE_RGBA8
        //              �Լ�CAMERA_MEDIA_TYPE_BGR8��CAMERA_MEDIA_TYPE_BGRA8
        //              (��CameraDefine.h�ж���)5�֣��ֱ��Ӧ8λ�Ҷ�ͼ���24RGB��32λRGB��24λBGR��32λBGR��ɫͼ��
        // ����	    : hCamera		����ľ������CameraInit������á� 
        //             uFormat	Ҫ�趨��ʽ��CAMERA_MEDIA_TYPE_MONO8����CAMERA_MEDIA_TYPE_RGB8��CAMERA_MEDIA_TYPE_RGBA8	
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetIspOutFormat(
            CameraHandle hCamera,
            uint uFormat
        );

        /******************************************************/
        // ������ 	: CameraGetIspOutFormat
        // ��������	: ���CameraGetImageBuffer����ͼ����������ʽ��֧��
        //              CAMERA_MEDIA_TYPE_MONO8��CAMERA_MEDIA_TYPE_RGB8��CAMERA_MEDIA_TYPE_RGBA8
        //              �Լ�CAMERA_MEDIA_TYPE_BGR8��CAMERA_MEDIA_TYPE_BGRA8
        //              (��CameraDefine.h�ж���)���֣��ֱ��Ӧ8λ�Ҷ�ͼ���24RGB��32λRGB��24λBGR��32λBGR��ɫͼ��
        // ����	    : hCamera		����ľ������CameraInit������á� 
        //             puFormat	���ص�ǰ�趨�ĸ�ʽ��CAMERA_MEDIA_TYPE_MONO8����CAMERA_MEDIA_TYPE_RGB8��CAMERA_MEDIA_TYPE_RGBA8	
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetIspOutFormat(
            CameraHandle hCamera,
        ref uint puFormat
        );

        /******************************************************/
        // ������ 	: CameraReConnect
        // ��������	: ���������豸������USB�豸������ߺ�����
        // ����	    : hCamera	   ����ľ������CameraInit������á� 
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraReConnect(
            CameraHandle hCamera
        );

        /******************************************************/
        // ������ 	: CameraConnectTest
        // ��������	: �������������״̬�����ڼ������Ƿ����
        // ����	    : hCamera	   ����ľ������CameraInit������á� 
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)����ʾ�������״̬����;
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraConnectTest(
            CameraHandle hCamera
        );
		
		/******************************************************/
		// ������ 	: CameraSetLedEnable
		// ��������	: ���������LEDʹ��״̬������LED���ͺţ��˺������ش�����룬��ʾ��֧�֡�
		// ����	    : hCamera	   ����ľ������CameraInit������á� 
		//             index       LED�Ƶ������ţ���0��ʼ�����ֻ��һ���ɿ������ȵ�LED����ò���Ϊ0 ��
		//             enable      ʹ��״̬
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)����ʾ�������״̬����;
		//            ���򷵻� ��0ֵ���ο�CameraStatus.h�д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraSetLedEnable(
			CameraHandle    hCamera,
			int             index,
			int             enable
			);

		/******************************************************/
		// ������ 	: CameraGetLedEnable
		// ��������	: ��������LEDʹ��״̬������LED���ͺţ��˺������ش�����룬��ʾ��֧�֡�
		// ����	    : hCamera	   ����ľ������CameraInit������á� 
		//             index       LED�Ƶ������ţ���0��ʼ�����ֻ��һ���ɿ������ȵ�LED����ò���Ϊ0 ��
		//             enable      ָ�룬����LEDʹ��״̬
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)����ʾ�������״̬����;
		//            ���򷵻� ��0ֵ���ο�CameraStatus.h�д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraGetLedEnable(
			CameraHandle    hCamera,
			int             index,
			out int         enable
			);

		/******************************************************/
		// ������ 	: CameraSetLedOnOff
		// ��������	: ���������LED����״̬������LED���ͺţ��˺������ش�����룬��ʾ��֧�֡�
		// ����	    : hCamera	   ����ľ������CameraInit������á� 
		//             index       LED�Ƶ������ţ���0��ʼ�����ֻ��һ���ɿ������ȵ�LED����ò���Ϊ0 ��
		//             onoff	   LED����״̬
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)����ʾ�������״̬����;
		//            ���򷵻� ��0ֵ���ο�CameraStatus.h�д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraSetLedOnOff(
			CameraHandle    hCamera,
			int             index,
			int             onoff
			);

		/******************************************************/
		// ������ 	: CameraGetLedOnOff
		// ��������	: ��������LED����״̬������LED���ͺţ��˺������ش�����룬��ʾ��֧�֡�
		// ����	    : hCamera	   ����ľ������CameraInit������á� 
		//             index       LED�Ƶ������ţ���0��ʼ�����ֻ��һ���ɿ������ȵ�LED����ò���Ϊ0 ��
		//             onoff	   ָ�룬����LED����״̬
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)����ʾ�������״̬����;
		//            ���򷵻� ��0ֵ���ο�CameraStatus.h�д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraGetLedOnOff(
			CameraHandle    hCamera,
			int             index,
			out int         onoff
			);

		/******************************************************/
		// ������ 	: CameraSetLedDuration
		// ��������	: ���������LED����ʱ�䣬����LED���ͺţ��˺������ش�����룬��ʾ��֧�֡�
		// ����	    : hCamera	    ����ľ������CameraInit������á� 
		//             index        LED�Ƶ������ţ���0��ʼ�����ֻ��һ���ɿ������ȵ�LED����ò���Ϊ0 ��
		//             duration		LED����ʱ�䣬��λ����
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)����ʾ�������״̬����;
		//            ���򷵻� ��0ֵ���ο�CameraStatus.h�д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraSetLedDuration(
			CameraHandle    hCamera,
			int             index,
			uint            duration
			);

		/******************************************************/
		// ������ 	: CameraGetLedDuration
		// ��������	: ��������LED����ʱ�䣬����LED���ͺţ��˺������ش�����룬��ʾ��֧�֡�
		// ����	    : hCamera	    ����ľ������CameraInit������á� 
		//             index        LED�Ƶ������ţ���0��ʼ�����ֻ��һ���ɿ������ȵ�LED����ò���Ϊ0 ��
		//             duration		ָ�룬����LED����ʱ�䣬��λ����
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)����ʾ�������״̬����;
		//            ���򷵻� ��0ֵ���ο�CameraStatus.h�д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraGetLedDuration(
			CameraHandle    hCamera,
			int             index,
			out uint        duration
			);

        /******************************************************/
        // ������ 	: CameraSetLedBrightness
        // ��������	: ���������LED���ȣ�����LED���ͺţ��˺������ش�����룬��ʾ��֧�֡�
        // ����	    : hCamera	   ����ľ������CameraInit������á� 
        //             index      LED�Ƶ������ţ���0��ʼ�����ֻ��һ���ɿ������ȵ�LED����ò���Ϊ0 ��
        //             uBrightness LED����ֵ����Χ0��255. 0��ʾ�رգ�255������
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)����ʾ�������״̬����;
        //            ���򷵻� ��0ֵ���ο�CameraStatus.h�д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetLedBrightness(
            CameraHandle hCamera,
            int index,
            uint uBrightness
        );

        /******************************************************/
        // ������ 	: CameraGetLedBrightness
        // ��������	: ��������LED���ȣ�����LED���ͺţ��˺������ش�����룬��ʾ��֧�֡�
        // ����	    : hCamera	   ����ľ������CameraInit������á� 
        //             index      LED�Ƶ������ţ���0��ʼ�����ֻ��һ���ɿ������ȵ�LED����ò���Ϊ0 ��
        //             uBrightness ָ�룬����LED����ֵ����Χ0��255. 0��ʾ�رգ�255������
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)����ʾ�������״̬����;
        //            ���򷵻� ��0ֵ���ο�CameraStatus.h�д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetLedBrightness(
			CameraHandle hCamera,
			int index,
			ref uint uBrightness
        );

        /******************************************************/
        // ������ 	: CameraEnableTransferRoi
        // ��������	: ʹ�ܻ��߽�ֹ����Ķ������书�ܣ������ù��ܵ��ͺţ��˺������ش�����룬��ʾ��֧�֡�
        //              �ù�����Ҫ����������˽��ɼ������������з֣�ֻ����ָ���Ķ����������ߴ���֡�ʡ�
        //              ��������䵽PC�Ϻ󣬻��Զ�ƴ�ӳ��������棬û�б�����Ĳ��֣����ú�ɫ��䡣
        // ����	    : hCamera	    ����ľ������CameraInit������á� 
        //             index       ROI����������ţ���0��ʼ��
        //             uEnableMask ����ʹ��״̬���룬��Ӧ�ı���λΪ1��ʾʹ�ܡ�0Ϊ��ֹ��ĿǰSDK֧��4���ɱ༭����index��ΧΪ0��3����bit0 ��bit1��bit2��bit3����4�������ʹ��״̬��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)����ʾ�������״̬����;
        //            ���ڲ�֧�ֶ�����ROI������ͺţ��ú����᷵�� CAMERA_STATUS_NOT_SUPPORTED(-4) ��ʾ��֧��   
        //            ������0ֵ���ο�CameraStatus.h�д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraEnableTransferRoi(
            CameraHandle hCamera,
            uint uEnableMask
        );

        /******************************************************/
        // ������ 	: CameraSetTransferRoi
        // ��������	: �����������Ĳü�����������ˣ�ͼ��Ӵ������ϱ��ɼ��󣬽��ᱻ�ü���ָ�������������ͣ��˺������ش�����룬��ʾ��֧�֡�
        // ����	    : hCamera	   ����ľ������CameraInit������á� 
        //             index      ROI����������ţ���0��ʼ��
        //             X1,Y1      ROI��������Ͻ�����
        //             X2,Y2      ROI��������Ͻ�����
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)����ʾ�������״̬����;
        //            ���ڲ�֧�ֶ�����ROI������ͺţ��ú����᷵�� CAMERA_STATUS_NOT_SUPPORTED(-4) ��ʾ��֧��   
        //            ������0ֵ���ο�CameraStatus.h�д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetTransferRoi(
			CameraHandle hCamera,
			int index,
			uint X1,
			uint Y1,
			uint X2,
			uint Y2
        );

        /******************************************************/
        // ������ 	: CameraGetTransferRoi
        // ��������	: �����������Ĳü�����������ˣ�ͼ��Ӵ������ϱ��ɼ��󣬽��ᱻ�ü���ָ�������������ͣ��˺������ش�����룬��ʾ��֧�֡�
        // ����	    : hCamera	   ����ľ������CameraInit������á� 
        //             index      ROI����������ţ���0��ʼ��
        //             pX1,pY1      ROI��������Ͻ�����
        //             pX2,pY2      ROI��������Ͻ�����
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)����ʾ�������״̬����;
        //            ���ڲ�֧�ֶ�����ROI������ͺţ��ú����᷵�� CAMERA_STATUS_NOT_SUPPORTED(-4) ��ʾ��֧��   
        //            ������0ֵ���ο�CameraStatus.h�д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetTransferRoi(
            CameraHandle hCamera,
            int index,
            ref uint pX1,
            ref uint pY1,
            ref uint pX2,
            ref uint pY2
        );

        /******************************************************/
        // ������ 	: CameraAlignMalloc
        // ��������	: ����һ�ζ�����ڴ�ռ䡣���ܺ�malloc���ƣ���
        //			  �Ƿ��ص��ڴ�����alignָ�����ֽ�������ġ�
        // ����	    : size	   �ռ�Ĵ�С�� 
        //            align    ��ַ������ֽ�����
        // ����ֵ   : �ɹ�ʱ�����ط�0ֵ����ʾ�ڴ��׵�ַ��ʧ�ܷ���NULL��
        /******************************************************/
        public delegate IntPtr pfnCameraAlignMalloc(
            int size,
            int align
        );

        /******************************************************/
        // ������ 	: CameraAlignFree
        // ��������	: �ͷ���CameraAlignMalloc����������ڴ�ռ䡣
        // ����	    : membuffer	   ��CameraAlignMalloc���ص��ڴ��׵�ַ�� 
        // ����ֵ   : �ޡ�
        /******************************************************/
        public delegate void pfnCameraAlignFree(
            IntPtr membuffer
        );

        /******************************************************/
        // ������ 	: CameraSetAutoConnect
        // ��������	: �����Զ�ʹ������
        // ����	    : hCamera	   ����ľ������CameraInit������á� 
        //			  bEnable	   ʹ�������������λTRUEʱ��SDK�ڲ��Զ��������Ƿ���ߣ����ߺ��Լ�������
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)����ʾ�������״̬����;
        //            ���ڲ�֧�ֵ��ͺţ��ú����᷵�� CAMERA_STATUS_NOT_SUPPORTED(-4) ��ʾ��֧��   
        //            ������0ֵ���ο�CameraStatus.h�д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetAutoConnect(CameraHandle hCamera, int bEnable);

        /******************************************************/
        // ������ 	: CameraGetReConnectCounts
        // ��������	: �������Զ������Ĵ�����ǰ����CameraSetAutoConnect ʹ������Զ��������ܡ�Ĭ����ʹ�ܵġ�
        // ����	    : hCamera	   ����ľ������CameraInit������á� 
        //			 puCounts	   ���ص����Զ������Ĵ���
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)����ʾ�������״̬����;
        //            ���ڲ�֧�ֵ��ͺţ��ú����᷵�� CAMERA_STATUS_NOT_SUPPORTED(-4) ��ʾ��֧��   
        //            ������0ֵ���ο�CameraStatus.h�д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetReConnectCounts(CameraHandle hCamera, ref uint puCounts);

        /******************************************************/
        // ������   : CameraEvaluateImageDefinition
        // �������� : ͼƬ����������
        // ����     : hCamera  ����ľ������CameraInit������á�
        //			  iAlgorithSel ʹ�õ������㷨,���emEvaluateDefinitionAlgorith�еĶ���
        //            pbyIn    ����ͼ�����ݵĻ�������ַ������ΪNULL�� 
        //            pFrInfo  ����ͼ���֡ͷ��Ϣ
        //			  DefinitionValue ���ص������ȹ�ֵ��Խ��Խ������
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraEvaluateImageDefinition(
            CameraHandle hCamera,
            int iAlgorithSel,
            IntPtr pbyIn,
            ref tSdkFrameHead pFrInfo,
            out double DefinitionValue
        );

        /******************************************************/
        // ������   : CameraDrawText
        // �������� : �������ͼ�������л�������
        // ����     : pRgbBuffer ͼ�����ݻ�����
        //			  pFrInfo ͼ���֡ͷ��Ϣ
        //			  pFontFileName �����ļ���
        //			  FontWidth ������
        //			  FontHeight ����߶�
        //			  pText Ҫ���������
        //			  (Left, Top, Width, Height) ���ֵ��������
        //			  TextColor ������ɫRGB
        //			  uFlags �����־,���emCameraDrawTextFlags�еĶ���
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraDrawText(
            IntPtr pRgbBuffer,
            ref tSdkFrameHead pFrInfo,
            string pFontFileName,
            uint FontWidth,
            uint FontHeight,
            string pText,
            int Left,
            int Top,
            uint Width,
            uint Height,
            uint TextColor,
            uint uFlags
        );

        /******************************************************/
        // ������   : CameraGigeGetIp
        // �������� : ��ȡGIGE�����IP��ַ
        // ����     : pCameraInfo ������豸������Ϣ������CameraEnumerateDevice������á� 
        //			  CamIp ���IP(ע�⣺���뱣֤����Ļ��������ڵ���16�ֽ�)
        //			  CamMask �����������(ע�⣺���뱣֤����Ļ��������ڵ���16�ֽ�)
        //			  CamGateWay �������(ע�⣺���뱣֤����Ļ��������ڵ���16�ֽ�)
        //			  EtIp ����IP(ע�⣺���뱣֤����Ļ��������ڵ���16�ֽ�)
        //			  EtMask ������������(ע�⣺���뱣֤����Ļ��������ڵ���16�ֽ�)
        //			  EtGateWay ��������(ע�⣺���뱣֤����Ļ��������ڵ���16�ֽ�)
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGigeGetIp(
            ref tSdkCameraDevInfo pCameraInfo,
            byte[] CamIp,
            byte[] CamMask,
            byte[] CamGateWay,
            byte[] EtIp,
            byte[] EtMask,
            byte[] EtGateWay
        );

        public static CameraSdkStatus CameraGigeGetIp(
            ref tSdkCameraDevInfo pCameraInfo,
            out string CamIp,
            out string CamMask,
            out string CamGateWay,
            out string EtIp,
            out string EtMask,
            out string EtGateWay
        )
        {
            byte[] CamIpBuf = new byte[16];
            byte[] CamMaskBuf = new byte[16];
            byte[] CamGateWayBuf = new byte[16];
            byte[] EtIpBuf = new byte[16];
            byte[] EtMaskBuf = new byte[16];
            byte[] EtGateWayBuf = new byte[16];
            CameraSdkStatus status = _CameraGigeGetIp(ref pCameraInfo,
                CamIpBuf, CamMaskBuf, CamGateWayBuf, EtIpBuf, EtMaskBuf, EtGateWayBuf);
            if (status == 0)
            {
                CamIp = CStrToString(CamIpBuf);
                CamMask = CStrToString(CamMaskBuf);
                CamGateWay = CStrToString(CamGateWayBuf);
                EtIp = CStrToString(EtIpBuf);
                EtMask = CStrToString(EtMaskBuf);
                EtGateWay = CStrToString(EtGateWayBuf);
            }
            else
            {
                CamIp = "";
                CamMask = "";
                CamGateWay = "";
                EtIp = "";
                EtMask = "";
                EtGateWay = "";
            }
            return status;
        }

        /******************************************************/
        // ������   : CameraGigeSetIp
        // �������� : ����GIGE�����IP��ַ
        // ����     : pCameraInfo ������豸������Ϣ������CameraEnumerateDevice������á� 
        //			  Ip ���IP(�磺192.168.1.100)
        //			  SubMask �����������(�磺255.255.255.0)
        //			  GateWay �������(�磺192.168.1.1)
        //			  bPersistent TRUE: �������Ϊ�̶�IP��FALSE����������Զ�����IP�����Բ���Ip, SubMask, GateWay��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGigeSetIp(
            ref tSdkCameraDevInfo pCameraInfo,
            string Ip,
            string SubMask,
            string GateWay,
            int bPersistent
        );

        /******************************************************/
        // ������   : CameraGigeGetMac
        // �������� : ��ȡGIGE�����MAC��ַ
        // ����     : pCameraInfo ������豸������Ϣ������CameraEnumerateDevice������á� 
        //			  CamMac ���MAC(ע�⣺���뱣֤����Ļ��������ڵ���18�ֽ�)
        //			  EtMac ����MAC(ע�⣺���뱣֤����Ļ��������ڵ���18�ֽ�)
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGigeGetMac(
	        ref tSdkCameraDevInfo pCameraInfo,
	        byte[] CamMac,
	        byte[] EtMac
	    );

        public static CameraSdkStatus CameraGigeGetMac(
            ref tSdkCameraDevInfo pCameraInfo,
            out string CamMac,
            out string EtMac
        )
        {
            byte[] CamMacBuf = new byte[32];
            byte[] EtMacBuf = new byte[32];
            CameraSdkStatus status = _CameraGigeGetMac(ref pCameraInfo, CamMacBuf, EtMacBuf);
            if (status == 0)
            {
                CamMac = CStrToString(CamMacBuf);
                EtMac = CStrToString(EtMacBuf);
            }
            else
            {
                CamMac = "";
                EtMac = "";
            }
            return status;
        }

        /******************************************************/
        // ������   : CameraEnableFastResponse
        // �������� : ʹ�ܿ�����Ӧ
        // ����     : hCamera  ����ľ������CameraInit������á�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraEnableFastResponse(
	        CameraHandle hCamera
	    );

        /******************************************************/
        // ������   : CameraSetCorrectDeadPixel
        // �������� : ʹ�ܻ�������
        // ����     : hCamera  ����ľ������CameraInit������á�
        //				bEnable     TRUE: ʹ�ܻ�������   FALSE: �رջ�������
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetCorrectDeadPixel(
	        CameraHandle hCamera,
	        int bEnable
	    );

        /******************************************************/
        // ������   : CameraGetCorrectDeadPixel
        // �������� : ��ȡ��������ʹ��״̬
        // ����     : hCamera  ����ľ������CameraInit������á�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetCorrectDeadPixel(
	        CameraHandle hCamera,
	        out int pbEnable
	    );

        /******************************************************/
        // ������   : CameraFlatFieldingCorrectSetEnable
        // �������� : ʹ��ƽ��У��
        // ����     : hCamera  ����ľ������CameraInit������á�
        //				bEnable     TRUE: ʹ��ƽ��У��   FALSE: �ر�ƽ��У��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraFlatFieldingCorrectSetEnable(
	        CameraHandle hCamera,
	        int bEnable
	    );

        /******************************************************/
        // ������   : CameraFlatFieldingCorrectGetEnable
        // �������� : ��ȡƽ��У��ʹ��״̬
        // ����     : hCamera  ����ľ������CameraInit������á�
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraFlatFieldingCorrectGetEnable(
	        CameraHandle hCamera,
	        out int pbEnable
	    );

        /******************************************************/
        // ������   : CameraFlatFieldingCorrectSetParameter
        // �������� : ����ƽ��У������
        // ����     :	hCamera  ����ľ������CameraInit������á�
        //				pDarkFieldingImage ����ͼƬ
        //				pDarkFieldingFrInfo ����ͼƬ��Ϣ
        //				pLightFieldingImage ����ͼƬ
        //				pLightFieldingFrInfo ����ͼƬ��Ϣ
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraFlatFieldingCorrectSetParameter(
	        CameraHandle hCamera,
	        IntPtr pDarkFieldingImage,
	        ref tSdkFrameHead pDarkFieldingFrInfo,
	        IntPtr pLightFieldingImage,
	        ref tSdkFrameHead pLightFieldingFrInfo
	    );

        /******************************************************/
        // ������   : CameraFlatFieldingCorrectSaveParameterToFile
        // �������� : ����ƽ��У���������ļ�
        // ����     :	hCamera  ����ľ������CameraInit������á�
        //				pszFileName �ļ�·��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraFlatFieldingCorrectSaveParameterToFile(
	        CameraHandle hCamera,
	        string pszFileName
	    );

        /******************************************************/
        // ������   : CameraFlatFieldingCorrectLoadParameterFromFile
        // �������� : ���ļ��м���ƽ��У������
        // ����     :	hCamera  ����ľ������CameraInit������á�
        //				pszFileName �ļ�·��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraFlatFieldingCorrectLoadParameterFromFile(
	        CameraHandle hCamera,
	        string pszFileName
	    );

        /******************************************************/
        // ������   : CameraCommonCall
        // �������� : �����һЩ���⹦�ܵ��ã����ο���ʱһ�㲻��Ҫ���á�
        // ����     : hCamera   ����ľ������CameraInit������á�
        //            pszCall   ���ܼ�����
        //            pszResult ���ý������ͬ��pszCallʱ�����岻ͬ��
        //            uResultBufSize pszResultָ��Ļ��������ֽڴ�С
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraCommonCall(
	        CameraHandle    hCamera, 
	        byte[]		    pszCall,
	        byte[]	   		pszResult,
	        UInt32			uResultBufSize
	    );

        public static CameraSdkStatus CameraCommonCall(
            CameraHandle hCamera,
            string strCall,
            out string strResult
        )
        {
            UInt32 ResultBufSize = 1024;
            byte[] ResultBuf = new byte[ResultBufSize];
            CameraSdkStatus status = _CameraCommonCall(hCamera, StringToCStr(strCall), ResultBuf, ResultBufSize);
            if (status == 0)
            {
                strResult = CStrToString(ResultBuf);
            }
            else
            {
                strResult = "";
            }
            return status;
        }

        /******************************************************/
        // ������   : CameraSetDenoise3DParams
        // �������� : ����3D�������
        // ����     : hCamera   ����ľ������CameraInit������á�
        //            bEnable  ���û����
        //            nCount   ʹ�ü���ͼƬ���н���(2-8��)
        //            Weights  ����Ȩ��
        //					   �統ʹ��3��ͼƬ���н���������������Դ���3������(0.3,0.3,0.4)�����һ��ͼƬ��Ȩ�ش���ǰ2��
        //					   �������Ҫʹ��Ȩ�أ���������������0����ʾ����ͼƬ��Ȩ����ͬ(0.33,0.33,0.33)
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraSetDenoise3DParams(
	        CameraHandle    hCamera, 
	        int			    bEnable,
	        int				nCount,
	        float[]			Weights
	    );

        /******************************************************/
        // ������   : CameraGetDenoise3DParams
        // �������� : ��ȡ��ǰ��3D�������
        // ����     : hCamera   ����ľ������CameraInit������á�
        //            bEnable  ���û����
        //            nCount   ʹ���˼���ͼƬ���н���
        //			  bUseWeight �Ƿ�ʹ���˽���Ȩ��
        //            Weights  ����Ȩ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetDenoise3DParams(
	        CameraHandle    hCamera, 
	        out int			bEnable,
	        out int         nCount,
	        out	int		    bUseWeight,
	        float[]		    Weights
	    );

        /******************************************************/
        // ������   : CameraManualDenoise3D
        // �������� : ��һ��֡����һ�ν��봦��
        // ����     : InFramesHead  ����֡ͷ
        //			  InFramesData  ����֡����
        //            nCount   ����֡������
        //            Weights  ����Ȩ��
        //					   �統ʹ��3��ͼƬ���н���������������Դ���3������(0.3,0.3,0.4)�����һ��ͼƬ��Ȩ�ش���ǰ2��
        //					   �������Ҫʹ��Ȩ�أ���������������0����ʾ����ͼƬ��Ȩ����ͬ(0.33,0.33,0.33)
        //			  OutFrameHead ���֡ͷ
        //			  OutFrameData ���֡����
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraManualDenoise3D(
	        tSdkFrameHead[]	InFramesHead,
	        IntPtr[]		InFramesData,
	        int				nCount,
	        float[]			Weights,
	        ref tSdkFrameHead OutFrameHead,
	        IntPtr			OutFrameData
	    );

        /******************************************************/
		// ������   : CameraCustomizeDeadPixels
		// �������� : �򿪻���༭���
		// ����     : hCamera    ����ľ������CameraInit������á�
		//            hParent    ���øú����Ĵ��ڵľ��������ΪNULL��
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraCustomizeDeadPixels(
			CameraHandle	hCamera,
			IntPtr			hParent
			);

		/******************************************************/
		// ������   : CameraReadDeadPixels
		// �������� : ��ȡ�������
		// ����     : hCamera   ����ľ������CameraInit������á�
		//			  pRows ����y����
		//			  pCols ����x����
		//			  pNumPixel ����ʱ��ʾ���л������Ĵ�С������ʱ��ʾ���л������з��صĻ���������
		//			  ��pRows����pColsΪNULLʱ������������ǰ�Ļ������ͨ��pNumPixel����
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraReadDeadPixels(
			CameraHandle    hCamera,
			ushort[]		pRows,
			ushort[]		pCols,
			ref uint		pNumPixel
			);

		/******************************************************/
		// ������   : CameraAddDeadPixels
		// �������� : ����������
		// ����     : hCamera   ����ľ������CameraInit������á�
		//			  pRows ����y����
		//			  pCols ����x����
		//			  NumPixel ���л������еĻ������
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraAddDeadPixels(
			CameraHandle    hCamera,
			ushort[]		pRows,
			ushort[]		pCols,
			uint			NumPixel
			);

		/******************************************************/
		// ������   : CameraRemoveDeadPixels
		// �������� : ɾ�����ָ������
		// ����     : hCamera   ����ľ������CameraInit������á�
		//			  pRows ����y����
		//			  pCols ����x����
		//			  NumPixel ���л������еĻ������
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraRemoveDeadPixels(
			CameraHandle    hCamera,
			ushort[]		pRows,
			ushort[]		pCols,
			uint			NumPixel
			);

		/******************************************************/
		// ������   : CameraRemoveAllDeadPixels
		// �������� : ɾ����������л���
		// ����     : hCamera   ����ľ������CameraInit������á�
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraRemoveAllDeadPixels(
			CameraHandle    hCamera
			);

		/******************************************************/
		// ������   : CameraSaveDeadPixels
		// �������� : ����������㵽����洢��
		// ����     : hCamera   ����ľ������CameraInit������á�
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraSaveDeadPixels(
			CameraHandle    hCamera
			);

		/******************************************************/
		// ������   : CameraSaveDeadPixelsToFile
		// �������� : ����������㵽�ļ���
		// ����     : hCamera   ����ľ������CameraInit������á�
		//			  sFileName  �����ļ�������·����
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraSaveDeadPixelsToFile(
			CameraHandle    hCamera,
            string          sFileName
			);

		/******************************************************/
		// ������   : CameraLoadDeadPixelsFromFile
		// �������� : ���ļ������������
		// ����     : hCamera   ����ľ������CameraInit������á�
		//			  sFileName  �����ļ�������·����
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraLoadDeadPixelsFromFile(
			CameraHandle    hCamera,
			string		    sFileName
			);
			
		/******************************************************/
		// ������   : CameraGetImageBufferPriority
		// �������� : ���һ֡ͼ�����ݡ�Ϊ�����Ч�ʣ�SDK��ͼ��ץȡʱ�������㿽�����ƣ�
		//        CameraGetImageBufferʵ�ʻ�����ں��е�һ����������ַ��
		//        �ú����ɹ����ú󣬱������CameraReleaseImageBuffer�ͷ���
		//        CameraGetImageBuffer�õ��Ļ�����,�Ա����ں˼���ʹ��
		//        �û�������  
		// ����     : hCamera   ����ľ������CameraInit������á�
		//            pFrameInfo  ͼ���֡ͷ��Ϣָ�롣
		//            pbyBuffer   ָ��ͼ������ݵĻ�����ָ�롣����
		//              �������㿽�����������Ч�ʣ����
		//              ����ʹ����һ��ָ��ָ���ָ�롣
		//            wTimes ץȡͼ��ĳ�ʱʱ�䡣��λ���롣��
		//              wTimesʱ���ڻ�δ���ͼ����ú���
		//              �᷵�س�ʱ��Ϣ��
		//			  Priority ȡͼ���ȼ� �����emCameraGetImagePriority
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraGetImageBufferPriority(
			CameraHandle        hCamera, 
			out tSdkFrameHead   pFrameInfo, 
			out IntPtr          pbyBuffer,
			uint                wTimes,
			uint				Priority
			);

		/******************************************************/
		// ������ 	: CameraGetImageBufferPriorityEx
		// ��������	: ���һ֡ͼ�����ݡ��ýӿڻ�õ�ͼ���Ǿ���������RGB��ʽ���ú������ú�
		//			  ����Ҫ���� CameraReleaseImageBuffer �ͷţ�Ҳ��Ҫ����free֮��ĺ����ͷ�
		//              ���ͷŸú������ص�ͼ�����ݻ�������
		// ����	    : hCamera	  ����ľ������CameraInit������á�
		//            piWidth    ����ָ�룬����ͼ��Ŀ��
		//            piHeight   ����ָ�룬����ͼ��ĸ߶�
		//            UINT wTimes ץȡͼ��ĳ�ʱʱ�䡣��λ���롣��
		//						  wTimesʱ���ڻ�δ���ͼ����ú���
		//						  �᷵�س�ʱ��Ϣ��
		//			  Priority   ȡͼ���ȼ� �����emCameraGetImagePriority
		// ����ֵ   : �ɹ�ʱ������RGB���ݻ��������׵�ַ;
		//            ���򷵻�0��
		/******************************************************/
        public delegate IntPtr pfnCameraGetImageBufferPriorityEx(
			CameraHandle        hCamera, 
			out int             piWidth,
			out int             piHeight,
			uint                wTimes,
			uint				Priority
			);

		/******************************************************/
		// ������ 	: CameraGetImageBufferPriorityEx2
		// ��������	: ���һ֡ͼ�����ݡ��ýӿڻ�õ�ͼ���Ǿ���������RGB��ʽ���ú������ú�
		//			  ����Ҫ���� CameraReleaseImageBuffer �ͷţ�Ҳ��Ҫ����free֮��ĺ����ͷ�
		//              ���ͷŸú������ص�ͼ�����ݻ�������
		// ����	    : hCamera	    ����ľ������CameraInit������á�
		//             pImageData  ����ͼ�����ݵĻ���������С�����uOutFormatָ���ĸ�ʽ��ƥ�䣬�������ݻ����
		//             piWidth     ����ָ�룬����ͼ��Ŀ��
		//             piHeight    ����ָ�룬����ͼ��ĸ߶�
		//             wTimes      ץȡͼ��ĳ�ʱʱ�䡣��λ���롣��
		//						wTimesʱ���ڻ�δ���ͼ����ú���
		//						�᷵�س�ʱ��Ϣ��
		//			  Priority	   ȡͼ���ȼ� �����emCameraGetImagePriority
		// ����ֵ   : �ɹ�ʱ������RGB���ݻ��������׵�ַ;
		//            ���򷵻�0��
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraGetImageBufferPriorityEx2(
			CameraHandle    hCamera, 
			IntPtr          pImageData,
			uint            uOutFormat,
			out int         piWidth,
			out int         piHeight,
			uint            wTimes,
			uint			Priority
			);

		/******************************************************/
		// ������ 	: CameraGetImageBufferPriorityEx3
		// ��������	: ���һ֡ͼ�����ݡ��ýӿڻ�õ�ͼ���Ǿ���������RGB��ʽ���ú������ú�
		//			  ����Ҫ���� CameraReleaseImageBuffer �ͷ�.
		//              uOutFormat 0 : 8 BIT gray 1:rgb24 2:rgba32 3:bgr24 4:bgra32
		// ����	    : hCamera	    ����ľ������CameraInit������á�
		//             pImageData  ����ͼ�����ݵĻ���������С�����uOutFormatָ���ĸ�ʽ��ƥ�䣬�������ݻ����
		//            piWidth      ����ָ�룬����ͼ��Ŀ��
		//            piHeight     ����ָ�룬����ͼ��ĸ߶�
		//            puTimeStamp  �޷������Σ�����ͼ��ʱ��� 
		//            UINT wTimes  ץȡͼ��ĳ�ʱʱ�䡣��λ���롣��
		//			  wTimes       ʱ���ڻ�δ���ͼ����ú����᷵�س�ʱ��Ϣ��
		//			  Priority	   ȡͼ���ȼ� �����emCameraGetImagePriority
		// ����ֵ   : �ɹ�ʱ������RGB���ݻ��������׵�ַ;
		//            ���򷵻�0��
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraGetImageBufferPriorityEx3(
			CameraHandle hCamera, 
			IntPtr pImageData,
			uint uOutFormat,
			out int piWidth,
			out int piHeight,
			out uint puTimeStamp,
			uint wTimes,
			uint Priority
			);

        /******************************************************/
		// ������   : CameraClearBuffer
		// �������� : ���������ѻ��������֡
		// ����     : hCamera  ����ľ������CameraInit������á�
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraClearBuffer(
			CameraHandle hCamera
			);

		/******************************************************/
		// ������   : CameraSoftTriggerEx
		// �������� : ִ��һ��������ִ�к󣬻ᴥ����CameraSetTriggerCount
		//          ָ����֡����
		// ����     : hCamera  ����ľ������CameraInit������á�
		//			  uFlags ���ܱ�־,���emCameraSoftTriggerExFlags�еĶ���
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraSoftTriggerEx(
			CameraHandle hCamera,
			uint uFlags
			);

        /******************************************************/
		// ������ 	: CameraSetHDR
		// ��������	: ���������HDR����Ҫ���֧�֣�����HDR���ܵ��ͺţ��˺������ش�����룬��ʾ��֧�֡�
		// ����	    : hCamera	   ����ľ������CameraInit������á� 
		//            value		   HDRϵ������Χ0.0��1.0
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)����ʾ�������״̬����;
		//            ���򷵻� ��0ֵ���ο�CameraStatus.h�д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraSetHDR(
			CameraHandle    hCamera,
			float           value
			);

		/******************************************************/
		// ������ 	: CameraGetHDR
		// ��������	: ��ȡ�����HDR����Ҫ���֧�֣�����HDR���ܵ��ͺţ��˺������ش�����룬��ʾ��֧�֡�
		// ����	    : hCamera	   ����ľ������CameraInit������á� 
		//            value		   HDRϵ������Χ0.0��1.0
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)����ʾ�������״̬����;
		//            ���򷵻� ��0ֵ���ο�CameraStatus.h�д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraGetHDR(
			CameraHandle    hCamera,
			out float       value
			);

		/******************************************************/
		// ������ 	: CameraGetFrameID
		// ��������	: ��ȡ��ǰ֡��ID�������֧��(����ȫϵ��֧��)���˺������ش�����룬��ʾ��֧�֡�
		// ����	    : hCamera	   ����ľ������CameraInit������á� 
		//            id		   ֡ID
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)����ʾ�������״̬����;
		//            ���򷵻� ��0ֵ���ο�CameraStatus.h�д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraGetFrameID(
			CameraHandle    hCamera,
			out uint        id
			);

        /******************************************************/
        // ������ 	: CameraGetFrameTimeStamp
        // ��������	: ��ȡ��ǰ֡��ʱ���(��λ΢��)
        // ����	    : hCamera	   ����ľ������CameraInit������á� 
        //            TimeStampL   ʱ�����32λ
        //			  TimeStampH   ʱ�����32λ
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)����ʾ�������״̬����;
        //            ���򷵻� ��0ֵ���ο�CameraStatus.h�д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGetFrameTimeStamp(
	        CameraHandle    hCamera,
	        out uint        TimeStampL,
            out uint        TimeStampH
	        );

        /******************************************************/
		// ������ 	: CameraSetHDRGainMode
		// ��������	: �������������ģʽ����Ҫ���֧�֣���������ģʽ�л����ܵ��ͺţ��˺������ش�����룬��ʾ��֧�֡�
		// ����	    : hCamera	   ����ľ������CameraInit������á� 
		//            value		   0��������    1��������
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)����ʾ�������״̬����;
		//            ���򷵻� ��0ֵ���ο�CameraStatus.h�д�����Ķ��塣
		/******************************************************/
		public delegate CameraSdkStatus pfnCameraSetHDRGainMode(
			CameraHandle    hCamera,
			int				value
			);

		/******************************************************/
		// ������ 	: CameraGetHDRGainMode
		// ��������	: ��ȡ���������ģʽ����Ҫ���֧�֣���������ģʽ�л����ܵ��ͺţ��˺������ش�����룬��ʾ��֧�֡�
		// ����	    : hCamera	   ����ľ������CameraInit������á� 
		//            value		   0��������    1��������
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)����ʾ�������״̬����;
		//            ���򷵻� ��0ֵ���ο�CameraStatus.h�д�����Ķ��塣
		/******************************************************/
		public delegate CameraSdkStatus pfnCameraGetHDRGainMode(
			CameraHandle    hCamera,
			out int			value
			);

		/******************************************************/
		// ������ 	: CameraDrawFrameBuffer
		// ��������	: ����֡��ָ������
		// ����	    : pFrameBuffer: ֡����
		//			  pFrameHead: ֡ͷ
		//			  hWnd: Ŀ�Ĵ���
		//			  Algorithm �����㷨  0�����ٵ������Բ�  1���ٶ�����������
		//			  Mode ����ģʽ   0: �ȱ�����  1����������
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)
		//            ���򷵻� ��0ֵ���ο�CameraStatus.h�д�����Ķ��塣
		/******************************************************/
		public delegate CameraSdkStatus pfnCameraDrawFrameBuffer(
			IntPtr pFrameBuffer, 
			ref tSdkFrameHead pFrameHead,
			IntPtr hWnd,
			int Algorithm,
			int Mode
			);

		/******************************************************/
		// ������ 	: CameraFlipFrameBuffer
		// ��������	: ��ת֡����
		// ����	    : pFrameBuffer: ֡����
		//			  pFrameHead: ֡ͷ
		//			  Flags: 1:����   2������    3�����¡����ҽ���һ�η�ת(�൱����ת180��)
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)
		//            ���򷵻� ��0ֵ���ο�CameraStatus.h�д�����Ķ��塣
		/******************************************************/
		public delegate CameraSdkStatus pfnCameraFlipFrameBuffer(
			IntPtr pFrameBuffer, 
			ref tSdkFrameHead pFrameHead,
			int Flags
			);

        /******************************************************/
		// ������ 	: CameraConvertFrameBufferFormat
		// ��������	: ת��֡���ݸ�ʽ
		// ����	    : hCamera: ������
		//			  pInFrameBuffer: ����֡����
		//			  pOutFrameBuffer: ���֡����
		//			  outWidth: ������
		//			  outHeight: ����߶�
		//			  outMediaType: �����ʽ
		//			  pFrameHead: ֡ͷ��Ϣ��ת���ɹ����������Ϣ�ᱻ�޸�Ϊ���֡����Ϣ��
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0)
		//            ���򷵻� ��0ֵ���ο�CameraStatus.h�д�����Ķ��塣
		/******************************************************/
		public delegate CameraSdkStatus pfnCameraConvertFrameBufferFormat(
			CameraHandle hCamera,
			IntPtr pInFrameBuffer, 
			IntPtr pOutFrameBuffer, 
			int outWidth,
			int outHeight,
			uint outMediaType,
			ref tSdkFrameHead pFrameHead
			);

        /******************************************************/
		// ������   : CameraSetConnectionStatusCallback
		// �������� : �����������״̬�ı�Ļص�֪ͨ��������������ߡ�����ʱ��
		//        pCallBack��ָ��Ļص������ͻᱻ���á� 
		// ����     : hCamera ����ľ������CameraInit������á�
		//            pCallBack �ص�����ָ�롣
		//            pContext  �ص������ĸ��Ӳ������ڻص�����������ʱ
		//				�ø��Ӳ����ᱻ���룬����ΪNULL��������
		//				������ʱЯ��������Ϣ��
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
		public delegate CameraSdkStatus pfnCameraSetConnectionStatusCallback(
			CameraHandle        hCamera,
			CAMERA_CONNECTION_STATUS_CALLBACK pCallBack,
			IntPtr              pContext
			);
			
		/******************************************************/
		// ������   : CameraSetLightingControllerMode
		// �������� : ���ù�Դ�����������ģʽ���������ϵ������ҪӲ��֧�֣�
		// ����     : hCamera ����ľ������CameraInit������á�
		//            index ����������
		//            mode ���ģʽ��0:��������� 1:�ֶ���
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
		public delegate CameraSdkStatus pfnCameraSetLightingControllerMode(
			CameraHandle        hCamera,
			int					index,
			int					mode
			);

		/******************************************************/
		// ������   : CameraSetLightingControllerState
		// �������� : ���ù�Դ�����������״̬���������ϵ������ҪӲ��֧�֣�
		// ����     : hCamera ����ľ������CameraInit������á�
		//            index ����������
		//            state ���״̬��0:�ر�  1���򿪣�
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
		public delegate CameraSdkStatus pfnCameraSetLightingControllerState(
			CameraHandle        hCamera,
			int					index,
			int					state
			);
			
		public delegate CameraSdkStatus pfnCameraSetFrameResendCount(
			CameraHandle        hCamera,
			int					count
			);

        /******************************************************/
        // ������   : CameraGrabber_CreateFromDevicePage
        // �������� : ��������б����û�ѡ��Ҫ�򿪵����
        // ����     : �������ִ�гɹ����غ���������Grabber
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGrabber_CreateFromDevicePage(
            out IntPtr Grabber
        );

        /******************************************************/
		// ������   : CameraGrabber_CreateByIndex
		// �������� : ʹ������б���������Grabber
		// ����     : Grabber    �������ִ�гɹ����غ���������Grabber
		//			  Index		�������
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraGrabber_CreateByIndex(
			out IntPtr Grabber,
			int Index
		);

		/******************************************************/
		// ������   : CameraGrabber_CreateByName
		// �������� : ʹ��������ƴ���Grabber
		// ����     : Grabber    �������ִ�гɹ����غ���������Grabber
		//			  Name		�������
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraGrabber_CreateByName(
			out IntPtr Grabber,
			string Name
		);

        /******************************************************/
        // ������   : CameraGrabber_Create
        // �������� : ���豸������Ϣ����Grabber
        // ����     : Grabber    �������ִ�гɹ����غ���������Grabber����
        //			  pDevInfo	��������豸������Ϣ����CameraEnumerateDevice������á� 
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGrabber_Create(
            out IntPtr Grabber,
            ref tSdkCameraDevInfo pDevInfo
        );

        /******************************************************/
        // ������   : CameraGrabber_Destroy
        // �������� : ����Grabber
        // ����     : Grabber
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGrabber_Destroy(
            IntPtr Grabber
        );

        /******************************************************/
        // ������	: CameraGrabber_SetHWnd
        // ��������	: ����Ԥ����Ƶ����ʾ����
        // ����		: Grabber
        //			  hWnd  ���ھ��
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGrabber_SetHWnd(
            IntPtr Grabber,
            IntPtr hWnd
        );

        /******************************************************/
		// ������	: CameraGrabber_SetPriority
		// ��������	: ����ȡͼʹ�õ����ȼ�
		// ����		: Grabber
		//			  Priority  ȡͼ���ȼ� �����emCameraGetImagePriority
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraGrabber_SetPriority(
			IntPtr Grabber,
			uint Priority
		);

        /******************************************************/
        // ������	: CameraGrabber_StartLive
        // ��������	: ����Ԥ��
        // ����		: Grabber
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGrabber_StartLive(
            IntPtr Grabber
        );

        /******************************************************/
        // ������	: CameraGrabber_StopLive
        // ��������	: ֹͣԤ��
        // ����		: Grabber
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGrabber_StopLive(
            IntPtr Grabber
        );

        /******************************************************/
        // ������	: CameraGrabber_SaveImage
        // ��������	: ץͼ
        // ����		: Grabber
        //			  Image ����ץȡ����ͼ����Ҫ����CameraImage_Destroy�ͷţ�
        //			  TimeOut ��ʱʱ�䣨���룩
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGrabber_SaveImage(
            IntPtr Grabber,
            out IntPtr Image,
            uint TimeOut
        );

        /******************************************************/
        // ������	: CameraGrabber_SaveImageAsync
        // ��������	: �ύһ���첽��ץͼ�����ύ�ɹ����ץͼ��ɻ�ص��û����õ���ɺ���
        // ����		: Grabber
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGrabber_SaveImageAsync(
            IntPtr Grabber
        );

        /******************************************************/
        // ������	: CameraGrabber_SaveImageAsyncEx
        // ��������	: �ύһ���첽��ץͼ�����ύ�ɹ����ץͼ��ɻ�ص��û����õ���ɺ���
        // ����		: Grabber
        //			  UserData ��ʹ��CameraImage_GetUserData��Image��ȡ��ֵ
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGrabber_SaveImageAsyncEx(
            IntPtr Grabber,
            IntPtr UserData
	    );

        /******************************************************/
        // ������	: CameraGrabber_SetSaveImageCompleteCallback
        // ��������	: �����첽��ʽץͼ����ɺ���
        // ����		: Grabber
        //			  Callback ����ץͼ�������ʱ������
        //			  Context ��Callback������ʱ����Ϊ��������Callback
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGrabber_SetSaveImageCompleteCallback(
            IntPtr Grabber,
            pfnCameraGrabberSaveImageComplete Callback,
            IntPtr Context
        );

        /******************************************************/
        // ������	: CameraGrabber_SetFrameListener
        // ��������	: ����֡��������
        // ����		: Grabber
        //			  Listener �����������˺�������0��ʾ������ǰ֡
        //			  Context ��Listener������ʱ����Ϊ��������Listener
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGrabber_SetFrameListener(
            IntPtr Grabber,
            pfnCameraGrabberFrameListener Listener,
            IntPtr Context
        );

        /******************************************************/
		// ������	: CameraGrabber_SetRawCallback
		// ��������	: ����RAW�ص�����
		// ����		: Grabber
		//			  Callback Raw�ص�����
		//			  Context ��Callback������ʱ����Ϊ��������Callback
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraGrabber_SetRawCallback(
			IntPtr Grabber,
			pfnCameraGrabberFrameCallback Callback,
			IntPtr Context
			);

		/******************************************************/
		// ������	: CameraGrabber_SetRGBCallback
		// ��������	: ����RGB�ص�����
		// ����		: Grabber
		//			  Callback RGB�ص�����
		//			  Context ��Callback������ʱ����Ϊ��������Callback
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
        public delegate CameraSdkStatus pfnCameraGrabber_SetRGBCallback(
			IntPtr Grabber,
			pfnCameraGrabberFrameCallback Callback,
			IntPtr Context
			);

        /******************************************************/
        // ������	: CameraGrabber_GetCameraHandle
        // ��������	: ��ȡ������
        // ����		: Grabber
        //			  hCamera ���ص�������
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGrabber_GetCameraHandle(
            IntPtr Grabber,
            out CameraHandle hCamera
        );

        /******************************************************/
        // ������	: CameraGrabber_GetStat
        // ��������	: ��ȡ֡ͳ����Ϣ
        // ����		: Grabber
        //			  stat ���ص�ͳ����Ϣ
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGrabber_GetStat(
            IntPtr Grabber,
            out tSdkGrabberStat stat
        );

        /******************************************************/
        // ������	: CameraGrabber_GetCameraDevInfo
        // ��������	: ��ȡ���DevInfo
        // ����		: Grabber
        //			  DevInfo ���ص����DevInfo
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraGrabber_GetCameraDevInfo(
            IntPtr Grabber,
            out tSdkCameraDevInfo DevInfo
        );

        /******************************************************/
        // ������	: CameraImage_Create
        // ��������	: ����һ���µ�Image
        // ����		: Image
        //			  pFrameBuffer ֡���ݻ�����
        //			  pFrameHead ֡ͷ
        //			  bCopy TRUE: ���Ƴ�һ���µ�֡����  FALSE: �����ƣ�ֱ��ʹ��pFrameBufferָ��Ļ�����
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraImage_Create(
            out IntPtr Image,
            IntPtr pFrameBuffer,
            ref tSdkFrameHead pFrameHead,
            int bCopy
        );

        /******************************************************/
        // ������	: CameraImage_CreateEmpty
        // ��������	: ����һ���յ�Image
        // ����		: Image
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraImage_CreateEmpty(
	        out IntPtr Image
	    );

        /******************************************************/
        // ������	: CameraImage_Destroy
        // ��������	: ����Image
        // ����		: Image
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraImage_Destroy(
            IntPtr Image
        );

        /******************************************************/
        // ������	: CameraImage_GetData
        // ��������	: ��ȡImage����
        // ����		: Image
        //			  DataBuffer ͼ������
        //			  Head ͼ����Ϣ
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraImage_GetData(
            IntPtr Image,
            out IntPtr DataBuffer,
            out IntPtr Head
        );

        /******************************************************/
        // ������	: CameraImage_GetUserData
        // ��������	: ��ȡImage���û��Զ�������
        // ����		: Image
        //			  UserData �����û��Զ�������
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraImage_GetUserData(
	        IntPtr Image,
	        out IntPtr UserData
	    );

        /******************************************************/
        // ������	: CameraImage_SetUserData
        // ��������	: ����Image���û��Զ�������
        // ����		: Image
        //			  UserData �û��Զ�������
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraImage_SetUserData(
	        IntPtr Image,
	        IntPtr UserData
	    );

        /******************************************************/
        // ������	: CameraImage_IsEmpty
        // ��������	: �ж�һ��Image�Ƿ�Ϊ��
        // ����		: Image
        //			  IsEmpty Ϊ�շ���:TRUE(1)  ���򷵻�:FALSE(0)
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraImage_IsEmpty(
	        IntPtr Image,
	        out int IsEmpty
	    );

        /******************************************************/
        // ������	: CameraImage_Draw
        // ��������	: ����Image��ָ������
        // ����		: Image
        //			  hWnd Ŀ�Ĵ���
        //			  Algorithm �����㷨  0�����ٵ������Բ�  1���ٶ�����������
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraImage_Draw(
            IntPtr Image,
            IntPtr hWnd,
            int Algorithm
        );

        /******************************************************/
		// ������	: CameraImage_DrawFit
		// ��������	: ��������Image��ָ������
		// ����		: Image
		//			  hWnd Ŀ�Ĵ���
		//			  Algorithm �����㷨  0�����ٵ������Բ�  1���ٶ�����������
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
		public delegate CameraSdkStatus pfnCameraImage_DrawFit(
			IntPtr Image,
			IntPtr hWnd,
			int Algorithm
			);

		/******************************************************/
		// ������	: CameraImage_DrawToDC
		// ��������	: ����Image��ָ��DC
		// ����		: Image
		//			  hDC Ŀ��DC
		//			  Algorithm �����㷨  0�����ٵ������Բ�  1���ٶ�����������
		//			  xDst,yDst: Ŀ����ε����Ͻ�����
		//			  cxDst,cyDst: Ŀ����εĿ��
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
		public delegate CameraSdkStatus pfnCameraImage_DrawToDC(
			IntPtr Image,
			IntPtr hDC,
			int Algorithm,
			int xDst,
			int yDst,
			int cxDst,
			int cyDst
			);

		/******************************************************/
		// ������	: CameraImage_DrawToDCFit
		// ��������	: ��������Image��ָ��DC
		// ����		: Image
		//			  hDC Ŀ��DC
		//			  Algorithm �����㷨  0�����ٵ������Բ�  1���ٶ�����������
		//			  xDst,yDst: Ŀ����ε����Ͻ�����
		//			  cxDst,cyDst: Ŀ����εĿ��
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
		public delegate CameraSdkStatus pfnCameraImage_DrawToDCFit(
			IntPtr Image,
			IntPtr hDC,
			int Algorithm,
			int xDst,
			int yDst,
			int cxDst,
			int cyDst
			);

        /******************************************************/
        // ������	: CameraImage_BitBlt
        // ��������	: ����Image��ָ�����ڣ������ţ�
        // ����		: Image
        //			  hWnd Ŀ�Ĵ���
        //			  xDst,yDst: Ŀ����ε����Ͻ�����
        //			  cxDst,cyDst: Ŀ����εĿ��
        //			  xSrc,ySrc: ͼ����ε����Ͻ�����
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraImage_BitBlt(
            IntPtr Image,
            IntPtr hWnd,
            int xDst,
            int yDst,
            int cxDst,
            int cyDst,
            int xSrc,
            int ySrc
        );

        /******************************************************/
		// ������	: CameraImage_BitBltToDC
		// ��������	: ����Image��ָ��DC�������ţ�
		// ����		: Image
		//			  hDC Ŀ��DC
		//			  xDst,yDst: Ŀ����ε����Ͻ�����
		//			  cxDst,cyDst: Ŀ����εĿ��
		//			  xSrc,ySrc: ͼ����ε����Ͻ�����
		// ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
		//            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
		//            �д�����Ķ��塣
		/******************************************************/
		public delegate CameraSdkStatus pfnCameraImage_BitBltToDC(
			IntPtr Image,
			IntPtr hDC,
			int xDst,
			int yDst,
			int cxDst,
			int cyDst,
			int xSrc,
			int ySrc
			);

        /******************************************************/
        // ������	: CameraImage_SaveAsBmp
        // ��������	: ��bmp��ʽ����Image
        // ����		: Image
        //			  FileName �ļ���
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraImage_SaveAsBmp(
            IntPtr Image,
            string FileName
        );

        /******************************************************/
        // ������	: CameraImage_SaveAsJpeg
        // ��������	: ��jpg��ʽ����Image
        // ����		: Image
        //			  FileName �ļ���
        //			  Quality ��������(1-100)��100Ϊ������ѵ��ļ�Ҳ���
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraImage_SaveAsJpeg(
            IntPtr Image,
            string FileName,
            byte Quality
        );

        /******************************************************/
        // ������	: CameraImage_SaveAsPng
        // ��������	: ��png��ʽ����Image
        // ����		: Image
        //			  FileName �ļ���
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraImage_SaveAsPng(
            IntPtr Image,
            string FileName
        );

        /******************************************************/
        // ������	: CameraImage_SaveAsRaw
        // ��������	: ����raw Image
        // ����		: Image
        //			  FileName �ļ���
        //			  Format 0: 8Bit Raw     1: 16Bit Raw
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraImage_SaveAsRaw(
            IntPtr Image,
            string FileName,
            int Format
        );

        /******************************************************/
        // ������	: CameraImage_IPicture
        // ��������	: ��Image����һ��IPicture
        // ����		: Image
        //			  Picture �´�����IPicture
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate CameraSdkStatus pfnCameraImage_IPicture(
            IntPtr Image,
            out Object NewPic
        );



        /******************************************************/
        // ������ 	: CameraGetErrorString
        // ��������	: 
        // ����	    : CameraSdkStatus iStatusCode
        // ����ֵ   : �ɹ�ʱ������CAMERA_STATUS_SUCCESS (0);
        //            ���򷵻ط�0ֵ�Ĵ�����,��ο�CameraStatus.h
        //            �д�����Ķ��塣
        /******************************************************/
        public delegate string pfnCameraGetErrorString(
            CameraSdkStatus status
        );

        ////////////////////////////////////////////////////////////////////////////////////////////////

        public static pfnCameraSdkInit CameraSdkInit;
        public static pfnCameraEnumerateDevice _CameraEnumerateDevice;
        public static pfnCameraInit CameraInit;
        public static pfnCameraSetCallbackFunction CameraSetCallbackFunction;
        public static pfnCameraUnInit CameraUnInit;
        public static pfnCameraGetInformation CameraGetInformation;
        public static pfnCameraImageProcess CameraImageProcess;
        public static pfnCameraDisplayInit CameraDisplayInit;
        public static pfnCameraDisplayRGB24 CameraDisplayRGB24;
        public static pfnCameraSetDisplayMode CameraSetDisplayMode;
        public static pfnCameraSetDisplayOffset CameraSetDisplayOffset;
        public static pfnCameraSetDisplaySize CameraSetDisplaySize;
        public static pfnCameraGetImageBuffer CameraGetImageBuffer;
        public static pfnCameraSnapToBuffer CameraSnapToBuffer;
        public static pfnCameraReleaseImageBuffer CameraReleaseImageBuffer;
        public static pfnCameraPlay CameraPlay;
        public static pfnCameraPause CameraPause;
        public static pfnCameraStop CameraStop;
        public static pfnCameraInitRecord CameraInitRecord;
        public static pfnCameraStopRecord CameraStopRecord;
        public static pfnCameraPushFrame CameraPushFrame;
        public static pfnCameraSaveImage CameraSaveImage;
        public static pfnCameraSaveImageEx CameraSaveImageEx;
        public static pfnCameraGetImageResolution CameraGetImageResolution;
        public static pfnCameraSetImageResolution CameraSetImageResolution;
		public static pfnCameraSetImageResolutionEx CameraSetImageResolutionEx;
        public static pfnCameraGetMediaType CameraGetMediaType;
        public static pfnCameraSetMediaType CameraSetMediaType;
        public static pfnCameraSetAeState CameraSetAeState;
        public static pfnCameraGetAeState CameraGetAeState;
        public static pfnCameraSetSharpness CameraSetSharpness;
        public static pfnCameraGetSharpness CameraGetSharpness;
        public static pfnCameraSetLutMode CameraSetLutMode;
        public static pfnCameraGetLutMode CameraGetLutMode;
        public static pfnCameraSelectLutPreset CameraSelectLutPreset;
        public static pfnCameraGetLutPresetSel CameraGetLutPresetSel;
        public static pfnCameraSetCustomLut CameraSetCustomLut;
        public static pfnCameraGetCustomLut CameraGetCustomLut;
        public static pfnCameraGetCurrentLut CameraGetCurrentLut;
        public static pfnCameraSetWbMode CameraSetWbMode;
        public static pfnCameraGetWbMode CameraGetWbMode;
        public static pfnCameraSetPresetClrTemp CameraSetPresetClrTemp;
        public static pfnCameraGetPresetClrTemp CameraGetPresetClrTemp;
		public static pfnCameraSetUserClrTempGain CameraSetUserClrTempGain;
		public static pfnCameraGetUserClrTempGain CameraGetUserClrTempGain;
		public static pfnCameraSetUserClrTempMatrix CameraSetUserClrTempMatrix;
		public static pfnCameraGetUserClrTempMatrix CameraGetUserClrTempMatrix;
		public static pfnCameraSetClrTempMode CameraSetClrTempMode;
		public static pfnCameraGetClrTempMode CameraGetClrTempMode;
        public static pfnCameraSetOnceWB CameraSetOnceWB;
        public static pfnCameraSetOnceBB CameraSetOnceBB;
        public static pfnCameraSetAeTarget CameraSetAeTarget;
        public static pfnCameraGetAeTarget CameraGetAeTarget;
        public static pfnCameraSetAeExposureRange CameraSetAeExposureRange;
        public static pfnCameraGetAeExposureRange CameraGetAeExposureRange;
        public static pfnCameraSetAeAnalogGainRange CameraSetAeAnalogGainRange;
        public static pfnCameraGetAeAnalogGainRange CameraGetAeAnalogGainRange;
        public static pfnCameraSetAeThreshold CameraSetAeThreshold;
        public static pfnCameraGetAeThreshold CameraGetAeThreshold;
        public static pfnCameraSetExposureTime CameraSetExposureTime;
        public static pfnCameraGetExposureLineTime CameraGetExposureLineTime;
        public static pfnCameraGetExposureTime CameraGetExposureTime;
        public static pfnCameraGetExposureTimeRange CameraGetExposureTimeRange;
        public static pfnCameraSetAnalogGain CameraSetAnalogGain;
        public static pfnCameraGetAnalogGain CameraGetAnalogGain;
        public static pfnCameraSetGain CameraSetGain;
        public static pfnCameraGetGain CameraGetGain;
        public static pfnCameraSetGamma CameraSetGamma;
        public static pfnCameraGetGamma CameraGetGamma;
        public static pfnCameraSetContrast CameraSetContrast;
        public static pfnCameraGetContrast CameraGetContrast;
        public static pfnCameraSetSaturation CameraSetSaturation;
        public static pfnCameraGetSaturation CameraGetSaturation;
        public static pfnCameraSetMonochrome CameraSetMonochrome;
        public static pfnCameraGetMonochrome CameraGetMonochrome;
        public static pfnCameraSetInverse CameraSetInverse;
        public static pfnCameraGetInverse CameraGetInverse;
        public static pfnCameraSetAntiFlick CameraSetAntiFlick;
        public static pfnCameraGetAntiFlick CameraGetAntiFlick;
        public static pfnCameraGetLightFrequency CameraGetLightFrequency;
        public static pfnCameraSetLightFrequency CameraSetLightFrequency;
        public static pfnCameraSetFrameSpeed CameraSetFrameSpeed;
        public static pfnCameraGetFrameSpeed CameraGetFrameSpeed;
        public static pfnCameraSetParameterMode CameraSetParameterMode;
        public static pfnCameraGetParameterMode CameraGetParameterMode;
        public static pfnCameraSetParameterMask CameraSetParameterMask;
        public static pfnCameraSaveParameter CameraSaveParameter;
        public static pfnCameraSaveParameterToFile CameraSaveParameterToFile;
        public static pfnCameraReadParameterFromFile CameraReadParameterFromFile;
        public static pfnCameraLoadParameter CameraLoadParameter;
        public static pfnCameraGetCurrentParameterGroup CameraGetCurrentParameterGroup;
        public static pfnCameraSetTransPackLen CameraSetTransPackLen;
        public static pfnCameraGetTransPackLen CameraGetTransPackLen;
        public static pfnCameraIsAeWinVisible CameraIsAeWinVisible;
        public static pfnCameraSetAeWinVisible CameraSetAeWinVisible;
        public static pfnCameraGetAeWindow CameraGetAeWindow;
        public static pfnCameraSetAeWindow CameraSetAeWindow;
        public static pfnCameraSetMirror CameraSetMirror;
        public static pfnCameraGetMirror CameraGetMirror;
        public static pfnCameraSetRotate CameraSetRotate;
        public static pfnCameraGetRotate CameraGetRotate;
        public static pfnCameraGetWbWindow CameraGetWbWindow;
        public static pfnCameraSetWbWindow CameraSetWbWindow;
        public static pfnCameraIsWbWinVisible CameraIsWbWinVisible;
        public static pfnCameraSetWbWinVisible CameraSetWbWinVisible;
        public static pfnCameraImageOverlay CameraImageOverlay;
        public static pfnCameraSetCrossLine CameraSetCrossLine;
        public static pfnCameraGetCrossLine CameraGetCrossLine;
        public static pfnCameraGetCapability _CameraGetCapability;
        public static pfnCameraWriteSN CameraWriteSN;
        public static pfnCameraReadSN CameraReadSN;
        public static pfnCameraSetTriggerDelayTime CameraSetTriggerDelayTime;
        public static pfnCameraGetTriggerDelayTime CameraGetTriggerDelayTime;
        public static pfnCameraSetTriggerCount CameraSetTriggerCount;
        public static pfnCameraGetTriggerCount CameraGetTriggerCount;
        public static pfnCameraSoftTrigger CameraSoftTrigger;
        public static pfnCameraSetTriggerMode CameraSetTriggerMode;
        public static pfnCameraGetTriggerMode CameraGetTriggerMode;
        public static pfnCameraSetStrobeMode CameraSetStrobeMode;
        public static pfnCameraGetStrobeMode CameraGetStrobeMode;
        public static pfnCameraSetStrobeDelayTime CameraSetStrobeDelayTime;
        public static pfnCameraGetStrobeDelayTime CameraGetStrobeDelayTime;
        public static pfnCameraSetStrobePulseWidth CameraSetStrobePulseWidth;
        public static pfnCameraGetStrobePulseWidth CameraGetStrobePulseWidth;
        public static pfnCameraSetStrobePolarity CameraSetStrobePolarity;
        public static pfnCameraGetStrobePolarity CameraGetStrobePolarity;
        public static pfnCameraSetExtTrigSignalType CameraSetExtTrigSignalType;
        public static pfnCameraGetExtTrigSignalType CameraGetExtTrigSignalType;
        public static pfnCameraSetExtTrigShutterType CameraSetExtTrigShutterType;
        public static pfnCameraGetExtTrigShutterType CameraGetExtTrigShutterType;
        public static pfnCameraSetExtTrigDelayTime CameraSetExtTrigDelayTime;
        public static pfnCameraGetExtTrigDelayTime CameraGetExtTrigDelayTime;
        public static pfnCameraSetExtTrigJitterTime CameraSetExtTrigJitterTime;
        public static pfnCameraGetExtTrigJitterTime CameraGetExtTrigJitterTime;
        public static pfnCameraGetExtTrigCapability CameraGetExtTrigCapability;
        public static pfnCameraPauseLevelTrigger CameraPauseLevelTrigger;
        public static pfnCameraGetResolutionForSnap CameraGetResolutionForSnap;
        public static pfnCameraSetResolutionForSnap CameraSetResolutionForSnap;
        public static pfnCameraCustomizeResolution CameraCustomizeResolution;
        public static pfnCameraCustomizeReferWin CameraCustomizeReferWin;
        public static pfnCameraShowSettingPage CameraShowSettingPage;
        public static pfnCameraCreateSettingPage _CameraCreateSettingPage;
        public static pfnCameraSetActiveSettingSubPage CameraSetActiveSettingSubPage;
        public static pfnCameraSetSettingPageParent CameraSetSettingPageParent;
        public static pfnCameraGetSettingPageHWnd CameraGetSettingPageHWnd;
        public static pfnCameraSpecialControl CameraSpecialControl;
        public static pfnCameraGetFrameStatistic CameraGetFrameStatistic;
        public static pfnCameraSetNoiseFilter CameraSetNoiseFilter;
        public static pfnCameraGetNoiseFilterState CameraGetNoiseFilterState;
        public static pfnCameraRstTimeStamp CameraRstTimeStamp;
        public static pfnCameraSaveUserData CameraSaveUserData;
        public static pfnCameraLoadUserData CameraLoadUserData;
        public static pfnCameraGetFriendlyName CameraGetFriendlyName;
        public static pfnCameraSetFriendlyName CameraSetFriendlyName;
        public static pfnCameraSdkGetVersionString CameraSdkGetVersionString;
        public static pfnCameraCheckFwUpdate CameraCheckFwUpdate;
        public static pfnCameraGetFirmwareVision CameraGetFirmwareVision;
        public static pfnCameraGetEnumInfo CameraGetEnumInfo;
        public static pfnCameraGetInerfaceVersion CameraGetInerfaceVersion;
        public static pfnCameraSetIOState CameraSetIOState;
        public static pfnCameraGetIOState CameraGetIOState;
        public static pfnCameraSetInPutIOMode CameraSetInPutIOMode;
        public static pfnCameraSetOutPutIOMode CameraSetOutPutIOMode;
        public static pfnCameraSetOutPutPWM CameraSetOutPutPWM;
        public static pfnCameraSetAeAlgorithm CameraSetAeAlgorithm;
        public static pfnCameraGetAeAlgorithm CameraGetAeAlgorithm;
        public static pfnCameraSetBayerDecAlgorithm CameraSetBayerDecAlgorithm;
        public static pfnCameraGetBayerDecAlgorithm CameraGetBayerDecAlgorithm;
        public static pfnCameraSetIspProcessor CameraSetIspProcessor;
        public static pfnCameraGetIspProcessor CameraGetIspProcessor;
        public static pfnCameraSetBlackLevel CameraSetBlackLevel;
        public static pfnCameraGetBlackLevel CameraGetBlackLevel;
        public static pfnCameraSetWhiteLevel CameraSetWhiteLevel;
        public static pfnCameraGetWhiteLevel CameraGetWhiteLevel;
        public static pfnCameraIsOpened CameraIsOpened;
        public static pfnCameraEnumerateDeviceEx CameraEnumerateDeviceEx;
        public static pfnCameraInitEx CameraInitEx;
        public static pfnCameraInitEx2 CameraInitEx2;
        public static pfnCameraGetImageBufferEx CameraGetImageBufferEx;
        public static pfnCameraImageProcessEx CameraImageProcessEx;
        public static pfnCameraSetIspOutFormat CameraSetIspOutFormat;
        public static pfnCameraGetIspOutFormat CameraGetIspOutFormat;
        public static pfnCameraReConnect CameraReConnect;
        public static pfnCameraConnectTest CameraConnectTest;
        public static pfnCameraSetLedEnable CameraSetLedEnable;
        public static pfnCameraGetLedEnable CameraGetLedEnable;
        public static pfnCameraSetLedOnOff CameraSetLedOnOff;
        public static pfnCameraGetLedOnOff CameraGetLedOnOff;
        public static pfnCameraSetLedDuration CameraSetLedDuration;
        public static pfnCameraGetLedDuration CameraGetLedDuration;
        public static pfnCameraSetLedBrightness CameraSetLedBrightness;
        public static pfnCameraGetLedBrightness CameraGetLedBrightness;
        public static pfnCameraEnableTransferRoi CameraEnableTransferRoi;
        public static pfnCameraSetTransferRoi CameraSetTransferRoi;
        public static pfnCameraGetTransferRoi CameraGetTransferRoi;
        public static pfnCameraAlignMalloc CameraAlignMalloc;
        public static pfnCameraAlignFree CameraAlignFree;
        public static pfnCameraSetAutoConnect CameraSetAutoConnect;
        public static pfnCameraGetReConnectCounts CameraGetReConnectCounts;
        public static pfnCameraEvaluateImageDefinition CameraEvaluateImageDefinition;
        public static pfnCameraDrawText CameraDrawText;
        public static pfnCameraGigeGetIp _CameraGigeGetIp;
        public static pfnCameraGigeSetIp CameraGigeSetIp;
        public static pfnCameraGigeGetMac _CameraGigeGetMac;
        public static pfnCameraEnableFastResponse CameraEnableFastResponse;
        public static pfnCameraSetCorrectDeadPixel CameraSetCorrectDeadPixel;
        public static pfnCameraGetCorrectDeadPixel CameraGetCorrectDeadPixel;
        public static pfnCameraFlatFieldingCorrectSetEnable CameraFlatFieldingCorrectSetEnable;
        public static pfnCameraFlatFieldingCorrectGetEnable CameraFlatFieldingCorrectGetEnable;
        public static pfnCameraFlatFieldingCorrectSetParameter CameraFlatFieldingCorrectSetParameter;
        public static pfnCameraFlatFieldingCorrectSaveParameterToFile CameraFlatFieldingCorrectSaveParameterToFile;
        public static pfnCameraFlatFieldingCorrectLoadParameterFromFile CameraFlatFieldingCorrectLoadParameterFromFile;
        public static pfnCameraCommonCall _CameraCommonCall;
        public static pfnCameraSetDenoise3DParams CameraSetDenoise3DParams;
        public static pfnCameraGetDenoise3DParams CameraGetDenoise3DParams;
        public static pfnCameraManualDenoise3D CameraManualDenoise3D;
        public static pfnCameraCustomizeDeadPixels CameraCustomizeDeadPixels;
        public static pfnCameraReadDeadPixels CameraReadDeadPixels;
        public static pfnCameraAddDeadPixels CameraAddDeadPixels;
        public static pfnCameraRemoveDeadPixels CameraRemoveDeadPixels;
        public static pfnCameraRemoveAllDeadPixels CameraRemoveAllDeadPixels;
        public static pfnCameraSaveDeadPixels CameraSaveDeadPixels;
        public static pfnCameraSaveDeadPixelsToFile CameraSaveDeadPixelsToFile;
        public static pfnCameraLoadDeadPixelsFromFile CameraLoadDeadPixelsFromFile;
        public static pfnCameraGetImageBufferPriority CameraGetImageBufferPriority;
        public static pfnCameraGetImageBufferPriorityEx CameraGetImageBufferPriorityEx;
        public static pfnCameraGetImageBufferPriorityEx2 CameraGetImageBufferPriorityEx2;
        public static pfnCameraGetImageBufferPriorityEx3 CameraGetImageBufferPriorityEx3;
        public static pfnCameraClearBuffer CameraClearBuffer;
        public static pfnCameraSoftTriggerEx CameraSoftTriggerEx;
        public static pfnCameraSetHDR CameraSetHDR;
        public static pfnCameraGetHDR CameraGetHDR;
        public static pfnCameraGetFrameID CameraGetFrameID;
        public static pfnCameraGetFrameTimeStamp CameraGetFrameTimeStamp;
        public static pfnCameraSetHDRGainMode CameraSetHDRGainMode;
        public static pfnCameraGetHDRGainMode CameraGetHDRGainMode;
        public static pfnCameraDrawFrameBuffer CameraDrawFrameBuffer;
        public static pfnCameraFlipFrameBuffer CameraFlipFrameBuffer;
        public static pfnCameraConvertFrameBufferFormat CameraConvertFrameBufferFormat;
        public static pfnCameraSetConnectionStatusCallback CameraSetConnectionStatusCallback;
		public static pfnCameraSetLightingControllerMode CameraSetLightingControllerMode;
		public static pfnCameraSetLightingControllerState CameraSetLightingControllerState;
		public static pfnCameraSetFrameResendCount CameraSetFrameResendCount;
        public static pfnCameraGrabber_CreateFromDevicePage CameraGrabber_CreateFromDevicePage;
        public static pfnCameraGrabber_CreateByIndex CameraGrabber_CreateByIndex;
        public static pfnCameraGrabber_CreateByName CameraGrabber_CreateByName;
        public static pfnCameraGrabber_Create CameraGrabber_Create;
        public static pfnCameraGrabber_Destroy CameraGrabber_Destroy;
        public static pfnCameraGrabber_SetHWnd CameraGrabber_SetHWnd;
        public static pfnCameraGrabber_SetPriority CameraGrabber_SetPriority;
        public static pfnCameraGrabber_StartLive CameraGrabber_StartLive;
        public static pfnCameraGrabber_StopLive CameraGrabber_StopLive;
        public static pfnCameraGrabber_SaveImage CameraGrabber_SaveImage;
        public static pfnCameraGrabber_SaveImageAsync CameraGrabber_SaveImageAsync;
        public static pfnCameraGrabber_SaveImageAsyncEx CameraGrabber_SaveImageAsyncEx;
        public static pfnCameraGrabber_SetSaveImageCompleteCallback CameraGrabber_SetSaveImageCompleteCallback;
        public static pfnCameraGrabber_SetFrameListener CameraGrabber_SetFrameListener;
        public static pfnCameraGrabber_SetRawCallback CameraGrabber_SetRawCallback;
        public static pfnCameraGrabber_SetRGBCallback CameraGrabber_SetRGBCallback;
        public static pfnCameraGrabber_GetCameraHandle CameraGrabber_GetCameraHandle;
        public static pfnCameraGrabber_GetStat CameraGrabber_GetStat;
        public static pfnCameraGrabber_GetCameraDevInfo CameraGrabber_GetCameraDevInfo;
        public static pfnCameraImage_Create CameraImage_Create;
        public static pfnCameraImage_CreateEmpty CameraImage_CreateEmpty;
        public static pfnCameraImage_Destroy CameraImage_Destroy;
        public static pfnCameraImage_GetData CameraImage_GetData;
        public static pfnCameraImage_GetUserData CameraImage_GetUserData;
        public static pfnCameraImage_SetUserData CameraImage_SetUserData;
        public static pfnCameraImage_IsEmpty CameraImage_IsEmpty;
        public static pfnCameraImage_Draw CameraImage_Draw;
        public static pfnCameraImage_DrawFit CameraImage_DrawFit;
        public static pfnCameraImage_DrawToDC CameraImage_DrawToDC;
        public static pfnCameraImage_DrawToDCFit CameraImage_DrawToDCFit;
        public static pfnCameraImage_BitBlt CameraImage_BitBlt;
        public static pfnCameraImage_BitBltToDC CameraImage_BitBltToDC;
        public static pfnCameraImage_SaveAsBmp CameraImage_SaveAsBmp;
        public static pfnCameraImage_SaveAsJpeg CameraImage_SaveAsJpeg;
        public static pfnCameraImage_SaveAsPng CameraImage_SaveAsPng;
        public static pfnCameraImage_SaveAsRaw CameraImage_SaveAsRaw;
        public static pfnCameraImage_IPicture CameraImage_IPicture;
        public static pfnCameraGetErrorString CameraGetErrorString;

        [DllImport("kernel32.dll")]
        static private extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll")]
        static private extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        [DllImport("kernel32", EntryPoint = "FreeLibrary", SetLastError = true)]
        static private extern bool FreeLibrary(IntPtr hModule);

        static private Delegate GetFunctionAddress(IntPtr dllModule, string functionName, Type t)
        {
            IntPtr address = GetProcAddress(dllModule, functionName);
            if (address == IntPtr.Zero)
                return null;
            else
                return Marshal.GetDelegateForFunctionPointer(address, t);
        }

        static MvApi()
        {
            IntPtr hSdkDll;
            if (IntPtr.Size == 8)
                hSdkDll = LoadLibrary("MVCAMSDK_X64.dll");
            else
                hSdkDll = LoadLibrary("MVCAMSDK.dll");

            CameraSdkInit = (pfnCameraSdkInit)GetFunctionAddress(hSdkDll, "CameraSdkInit", typeof(pfnCameraSdkInit));
            _CameraEnumerateDevice = (pfnCameraEnumerateDevice)GetFunctionAddress(hSdkDll, "CameraEnumerateDevice", typeof(pfnCameraEnumerateDevice));
            CameraInit = (pfnCameraInit)GetFunctionAddress(hSdkDll, "CameraInit", typeof(pfnCameraInit));
            CameraSetCallbackFunction = (pfnCameraSetCallbackFunction)GetFunctionAddress(hSdkDll, "CameraSetCallbackFunction", typeof(pfnCameraSetCallbackFunction));
            CameraUnInit = (pfnCameraUnInit)GetFunctionAddress(hSdkDll, "CameraUnInit", typeof(pfnCameraUnInit));
            CameraGetInformation = (pfnCameraGetInformation)GetFunctionAddress(hSdkDll, "CameraGetInformation", typeof(pfnCameraGetInformation));
            CameraImageProcess = (pfnCameraImageProcess)GetFunctionAddress(hSdkDll, "CameraImageProcess", typeof(pfnCameraImageProcess));
            CameraDisplayInit = (pfnCameraDisplayInit)GetFunctionAddress(hSdkDll, "CameraDisplayInit", typeof(pfnCameraDisplayInit));
            CameraDisplayRGB24 = (pfnCameraDisplayRGB24)GetFunctionAddress(hSdkDll, "CameraDisplayRGB24", typeof(pfnCameraDisplayRGB24));
            CameraSetDisplayMode = (pfnCameraSetDisplayMode)GetFunctionAddress(hSdkDll, "CameraSetDisplayMode", typeof(pfnCameraSetDisplayMode));
            CameraSetDisplayOffset = (pfnCameraSetDisplayOffset)GetFunctionAddress(hSdkDll, "CameraSetDisplayOffset", typeof(pfnCameraSetDisplayOffset));
            CameraSetDisplaySize = (pfnCameraSetDisplaySize)GetFunctionAddress(hSdkDll, "CameraSetDisplaySize", typeof(pfnCameraSetDisplaySize));
            CameraGetImageBuffer = (pfnCameraGetImageBuffer)GetFunctionAddress(hSdkDll, "CameraGetImageBuffer", typeof(pfnCameraGetImageBuffer));
            CameraSnapToBuffer = (pfnCameraSnapToBuffer)GetFunctionAddress(hSdkDll, "CameraSnapToBuffer", typeof(pfnCameraSnapToBuffer));
            CameraReleaseImageBuffer = (pfnCameraReleaseImageBuffer)GetFunctionAddress(hSdkDll, "CameraReleaseImageBuffer", typeof(pfnCameraReleaseImageBuffer));
            CameraPlay = (pfnCameraPlay)GetFunctionAddress(hSdkDll, "CameraPlay", typeof(pfnCameraPlay));
            CameraPause = (pfnCameraPause)GetFunctionAddress(hSdkDll, "CameraPause", typeof(pfnCameraPause));
            CameraStop = (pfnCameraStop)GetFunctionAddress(hSdkDll, "CameraStop", typeof(pfnCameraStop));
            CameraInitRecord = (pfnCameraInitRecord)GetFunctionAddress(hSdkDll, "CameraInitRecord", typeof(pfnCameraInitRecord));
            CameraStopRecord = (pfnCameraStopRecord)GetFunctionAddress(hSdkDll, "CameraStopRecord", typeof(pfnCameraStopRecord));
            CameraPushFrame = (pfnCameraPushFrame)GetFunctionAddress(hSdkDll, "CameraPushFrame", typeof(pfnCameraPushFrame));
            CameraSaveImage = (pfnCameraSaveImage)GetFunctionAddress(hSdkDll, "CameraSaveImage", typeof(pfnCameraSaveImage));
            CameraSaveImageEx = (pfnCameraSaveImageEx)GetFunctionAddress(hSdkDll, "CameraSaveImageEx", typeof(pfnCameraSaveImageEx));
            CameraGetImageResolution = (pfnCameraGetImageResolution)GetFunctionAddress(hSdkDll, "CameraGetImageResolution", typeof(pfnCameraGetImageResolution));
            CameraSetImageResolution = (pfnCameraSetImageResolution)GetFunctionAddress(hSdkDll, "CameraSetImageResolution", typeof(pfnCameraSetImageResolution));
			CameraSetImageResolutionEx = (pfnCameraSetImageResolutionEx)GetFunctionAddress(hSdkDll, "CameraSetImageResolutionEx", typeof(pfnCameraSetImageResolutionEx));
            CameraGetMediaType = (pfnCameraGetMediaType)GetFunctionAddress(hSdkDll, "CameraGetMediaType", typeof(pfnCameraGetMediaType));
            CameraSetMediaType = (pfnCameraSetMediaType)GetFunctionAddress(hSdkDll, "CameraSetMediaType", typeof(pfnCameraSetMediaType));
            CameraSetAeState = (pfnCameraSetAeState)GetFunctionAddress(hSdkDll, "CameraSetAeState", typeof(pfnCameraSetAeState));
            CameraGetAeState = (pfnCameraGetAeState)GetFunctionAddress(hSdkDll, "CameraGetAeState", typeof(pfnCameraGetAeState));
            CameraSetSharpness = (pfnCameraSetSharpness)GetFunctionAddress(hSdkDll, "CameraSetSharpness", typeof(pfnCameraSetSharpness));
            CameraGetSharpness = (pfnCameraGetSharpness)GetFunctionAddress(hSdkDll, "CameraGetSharpness", typeof(pfnCameraGetSharpness));
            CameraSetLutMode = (pfnCameraSetLutMode)GetFunctionAddress(hSdkDll, "CameraSetLutMode", typeof(pfnCameraSetLutMode));
            CameraGetLutMode = (pfnCameraGetLutMode)GetFunctionAddress(hSdkDll, "CameraGetLutMode", typeof(pfnCameraGetLutMode));
            CameraSelectLutPreset = (pfnCameraSelectLutPreset)GetFunctionAddress(hSdkDll, "CameraSelectLutPreset", typeof(pfnCameraSelectLutPreset));
            CameraGetLutPresetSel = (pfnCameraGetLutPresetSel)GetFunctionAddress(hSdkDll, "CameraGetLutPresetSel", typeof(pfnCameraGetLutPresetSel));
            CameraSetCustomLut = (pfnCameraSetCustomLut)GetFunctionAddress(hSdkDll, "CameraSetCustomLut", typeof(pfnCameraSetCustomLut));
            CameraGetCustomLut = (pfnCameraGetCustomLut)GetFunctionAddress(hSdkDll, "CameraGetCustomLut", typeof(pfnCameraGetCustomLut));
            CameraGetCurrentLut = (pfnCameraGetCurrentLut)GetFunctionAddress(hSdkDll, "CameraGetCurrentLut", typeof(pfnCameraGetCurrentLut));
            CameraSetWbMode = (pfnCameraSetWbMode)GetFunctionAddress(hSdkDll, "CameraSetWbMode", typeof(pfnCameraSetWbMode));
            CameraGetWbMode = (pfnCameraGetWbMode)GetFunctionAddress(hSdkDll, "CameraGetWbMode", typeof(pfnCameraGetWbMode));
            CameraSetPresetClrTemp = (pfnCameraSetPresetClrTemp)GetFunctionAddress(hSdkDll, "CameraSetPresetClrTemp", typeof(pfnCameraSetPresetClrTemp));
            CameraGetPresetClrTemp = (pfnCameraGetPresetClrTemp)GetFunctionAddress(hSdkDll, "CameraGetPresetClrTemp", typeof(pfnCameraGetPresetClrTemp));
			CameraSetUserClrTempGain = (pfnCameraSetUserClrTempGain)GetFunctionAddress(hSdkDll, "CameraSetUserClrTempGain", typeof(pfnCameraSetUserClrTempGain));
			CameraGetUserClrTempGain = (pfnCameraGetUserClrTempGain)GetFunctionAddress(hSdkDll, "CameraGetUserClrTempGain", typeof(pfnCameraGetUserClrTempGain));
			CameraSetUserClrTempMatrix = (pfnCameraSetUserClrTempMatrix)GetFunctionAddress(hSdkDll, "CameraSetUserClrTempMatrix", typeof(pfnCameraSetUserClrTempMatrix));
			CameraGetUserClrTempMatrix = (pfnCameraGetUserClrTempMatrix)GetFunctionAddress(hSdkDll, "CameraGetUserClrTempMatrix", typeof(pfnCameraGetUserClrTempMatrix));
			CameraSetClrTempMode = (pfnCameraSetClrTempMode)GetFunctionAddress(hSdkDll, "CameraSetClrTempMode", typeof(pfnCameraSetClrTempMode));
			CameraGetClrTempMode = (pfnCameraGetClrTempMode)GetFunctionAddress(hSdkDll, "CameraGetClrTempMode", typeof(pfnCameraGetClrTempMode));
            CameraSetOnceWB = (pfnCameraSetOnceWB)GetFunctionAddress(hSdkDll, "CameraSetOnceWB", typeof(pfnCameraSetOnceWB));
            CameraSetOnceBB = (pfnCameraSetOnceBB)GetFunctionAddress(hSdkDll, "CameraSetOnceBB", typeof(pfnCameraSetOnceBB));
            CameraSetAeTarget = (pfnCameraSetAeTarget)GetFunctionAddress(hSdkDll, "CameraSetAeTarget", typeof(pfnCameraSetAeTarget));
            CameraGetAeTarget = (pfnCameraGetAeTarget)GetFunctionAddress(hSdkDll, "CameraGetAeTarget", typeof(pfnCameraGetAeTarget));
            CameraSetAeExposureRange = (pfnCameraSetAeExposureRange)GetFunctionAddress(hSdkDll, "CameraSetAeExposureRange", typeof(pfnCameraSetAeExposureRange));
            CameraGetAeExposureRange = (pfnCameraGetAeExposureRange)GetFunctionAddress(hSdkDll, "CameraGetAeExposureRange", typeof(pfnCameraGetAeExposureRange));
            CameraSetAeAnalogGainRange = (pfnCameraSetAeAnalogGainRange)GetFunctionAddress(hSdkDll, "CameraSetAeAnalogGainRange", typeof(pfnCameraSetAeAnalogGainRange));
            CameraGetAeAnalogGainRange = (pfnCameraGetAeAnalogGainRange)GetFunctionAddress(hSdkDll, "CameraGetAeAnalogGainRange", typeof(pfnCameraGetAeAnalogGainRange));
            CameraSetAeThreshold = (pfnCameraSetAeThreshold)GetFunctionAddress(hSdkDll, "CameraSetAeThreshold", typeof(pfnCameraSetAeThreshold));
            CameraGetAeThreshold = (pfnCameraGetAeThreshold)GetFunctionAddress(hSdkDll, "CameraGetAeThreshold", typeof(pfnCameraGetAeThreshold));
            CameraSetExposureTime = (pfnCameraSetExposureTime)GetFunctionAddress(hSdkDll, "CameraSetExposureTime", typeof(pfnCameraSetExposureTime));
            CameraGetExposureLineTime = (pfnCameraGetExposureLineTime)GetFunctionAddress(hSdkDll, "CameraGetExposureLineTime", typeof(pfnCameraGetExposureLineTime));
            CameraGetExposureTime = (pfnCameraGetExposureTime)GetFunctionAddress(hSdkDll, "CameraGetExposureTime", typeof(pfnCameraGetExposureTime));
            CameraGetExposureTimeRange = (pfnCameraGetExposureTimeRange)GetFunctionAddress(hSdkDll, "CameraGetExposureTimeRange", typeof(pfnCameraGetExposureTimeRange));
            CameraSetAnalogGain = (pfnCameraSetAnalogGain)GetFunctionAddress(hSdkDll, "CameraSetAnalogGain", typeof(pfnCameraSetAnalogGain));
            CameraGetAnalogGain = (pfnCameraGetAnalogGain)GetFunctionAddress(hSdkDll, "CameraGetAnalogGain", typeof(pfnCameraGetAnalogGain));
            CameraSetGain = (pfnCameraSetGain)GetFunctionAddress(hSdkDll, "CameraSetGain", typeof(pfnCameraSetGain));
            CameraGetGain = (pfnCameraGetGain)GetFunctionAddress(hSdkDll, "CameraGetGain", typeof(pfnCameraGetGain));
            CameraSetGamma = (pfnCameraSetGamma)GetFunctionAddress(hSdkDll, "CameraSetGamma", typeof(pfnCameraSetGamma));
            CameraGetGamma = (pfnCameraGetGamma)GetFunctionAddress(hSdkDll, "CameraGetGamma", typeof(pfnCameraGetGamma));
            CameraSetContrast = (pfnCameraSetContrast)GetFunctionAddress(hSdkDll, "CameraSetContrast", typeof(pfnCameraSetContrast));
            CameraGetContrast = (pfnCameraGetContrast)GetFunctionAddress(hSdkDll, "CameraGetContrast", typeof(pfnCameraGetContrast));
            CameraSetSaturation = (pfnCameraSetSaturation)GetFunctionAddress(hSdkDll, "CameraSetSaturation", typeof(pfnCameraSetSaturation));
            CameraGetSaturation = (pfnCameraGetSaturation)GetFunctionAddress(hSdkDll, "CameraGetSaturation", typeof(pfnCameraGetSaturation));
            CameraSetMonochrome = (pfnCameraSetMonochrome)GetFunctionAddress(hSdkDll, "CameraSetMonochrome", typeof(pfnCameraSetMonochrome));
            CameraGetMonochrome = (pfnCameraGetMonochrome)GetFunctionAddress(hSdkDll, "CameraGetMonochrome", typeof(pfnCameraGetMonochrome));
            CameraSetInverse = (pfnCameraSetInverse)GetFunctionAddress(hSdkDll, "CameraSetInverse", typeof(pfnCameraSetInverse));
            CameraGetInverse = (pfnCameraGetInverse)GetFunctionAddress(hSdkDll, "CameraGetInverse", typeof(pfnCameraGetInverse));
            CameraSetAntiFlick = (pfnCameraSetAntiFlick)GetFunctionAddress(hSdkDll, "CameraSetAntiFlick", typeof(pfnCameraSetAntiFlick));
            CameraGetAntiFlick = (pfnCameraGetAntiFlick)GetFunctionAddress(hSdkDll, "CameraGetAntiFlick", typeof(pfnCameraGetAntiFlick));
            CameraGetLightFrequency = (pfnCameraGetLightFrequency)GetFunctionAddress(hSdkDll, "CameraGetLightFrequency", typeof(pfnCameraGetLightFrequency));
            CameraSetLightFrequency = (pfnCameraSetLightFrequency)GetFunctionAddress(hSdkDll, "CameraSetLightFrequency", typeof(pfnCameraSetLightFrequency));
            CameraSetFrameSpeed = (pfnCameraSetFrameSpeed)GetFunctionAddress(hSdkDll, "CameraSetFrameSpeed", typeof(pfnCameraSetFrameSpeed));
            CameraGetFrameSpeed = (pfnCameraGetFrameSpeed)GetFunctionAddress(hSdkDll, "CameraGetFrameSpeed", typeof(pfnCameraGetFrameSpeed));
            CameraSetParameterMode = (pfnCameraSetParameterMode)GetFunctionAddress(hSdkDll, "CameraSetParameterMode", typeof(pfnCameraSetParameterMode));
            CameraGetParameterMode = (pfnCameraGetParameterMode)GetFunctionAddress(hSdkDll, "CameraGetParameterMode", typeof(pfnCameraGetParameterMode));
            CameraSetParameterMask = (pfnCameraSetParameterMask)GetFunctionAddress(hSdkDll, "CameraSetParameterMask", typeof(pfnCameraSetParameterMask));
            CameraSaveParameter = (pfnCameraSaveParameter)GetFunctionAddress(hSdkDll, "CameraSaveParameter", typeof(pfnCameraSaveParameter));
            CameraSaveParameterToFile = (pfnCameraSaveParameterToFile)GetFunctionAddress(hSdkDll, "CameraSaveParameterToFile", typeof(pfnCameraSaveParameterToFile));
            CameraReadParameterFromFile = (pfnCameraReadParameterFromFile)GetFunctionAddress(hSdkDll, "CameraReadParameterFromFile", typeof(pfnCameraReadParameterFromFile));
            CameraLoadParameter = (pfnCameraLoadParameter)GetFunctionAddress(hSdkDll, "CameraLoadParameter", typeof(pfnCameraLoadParameter));
            CameraGetCurrentParameterGroup = (pfnCameraGetCurrentParameterGroup)GetFunctionAddress(hSdkDll, "CameraGetCurrentParameterGroup", typeof(pfnCameraGetCurrentParameterGroup));
            CameraSetTransPackLen = (pfnCameraSetTransPackLen)GetFunctionAddress(hSdkDll, "CameraSetTransPackLen", typeof(pfnCameraSetTransPackLen));
            CameraGetTransPackLen = (pfnCameraGetTransPackLen)GetFunctionAddress(hSdkDll, "CameraGetTransPackLen", typeof(pfnCameraGetTransPackLen));
            CameraIsAeWinVisible = (pfnCameraIsAeWinVisible)GetFunctionAddress(hSdkDll, "CameraIsAeWinVisible", typeof(pfnCameraIsAeWinVisible));
            CameraSetAeWinVisible = (pfnCameraSetAeWinVisible)GetFunctionAddress(hSdkDll, "CameraSetAeWinVisible", typeof(pfnCameraSetAeWinVisible));
            CameraGetAeWindow = (pfnCameraGetAeWindow)GetFunctionAddress(hSdkDll, "CameraGetAeWindow", typeof(pfnCameraGetAeWindow));
            CameraSetAeWindow = (pfnCameraSetAeWindow)GetFunctionAddress(hSdkDll, "CameraSetAeWindow", typeof(pfnCameraSetAeWindow));
            CameraSetMirror = (pfnCameraSetMirror)GetFunctionAddress(hSdkDll, "CameraSetMirror", typeof(pfnCameraSetMirror));
            CameraGetMirror = (pfnCameraGetMirror)GetFunctionAddress(hSdkDll, "CameraGetMirror", typeof(pfnCameraGetMirror));
            CameraSetRotate = (pfnCameraSetRotate)GetFunctionAddress(hSdkDll, "CameraSetRotate", typeof(pfnCameraSetRotate));
            CameraGetRotate = (pfnCameraGetRotate)GetFunctionAddress(hSdkDll, "CameraGetRotate", typeof(pfnCameraGetRotate));
            CameraGetWbWindow = (pfnCameraGetWbWindow)GetFunctionAddress(hSdkDll, "CameraGetWbWindow", typeof(pfnCameraGetWbWindow));
            CameraSetWbWindow = (pfnCameraSetWbWindow)GetFunctionAddress(hSdkDll, "CameraSetWbWindow", typeof(pfnCameraSetWbWindow));
            CameraIsWbWinVisible = (pfnCameraIsWbWinVisible)GetFunctionAddress(hSdkDll, "CameraIsWbWinVisible", typeof(pfnCameraIsWbWinVisible));
            CameraSetWbWinVisible = (pfnCameraSetWbWinVisible)GetFunctionAddress(hSdkDll, "CameraSetWbWinVisible", typeof(pfnCameraSetWbWinVisible));
            CameraImageOverlay = (pfnCameraImageOverlay)GetFunctionAddress(hSdkDll, "CameraImageOverlay", typeof(pfnCameraImageOverlay));
            CameraSetCrossLine = (pfnCameraSetCrossLine)GetFunctionAddress(hSdkDll, "CameraSetCrossLine", typeof(pfnCameraSetCrossLine));
            CameraGetCrossLine = (pfnCameraGetCrossLine)GetFunctionAddress(hSdkDll, "CameraGetCrossLine", typeof(pfnCameraGetCrossLine));
            _CameraGetCapability = (pfnCameraGetCapability)GetFunctionAddress(hSdkDll, "CameraGetCapability", typeof(pfnCameraGetCapability));
            CameraWriteSN = (pfnCameraWriteSN)GetFunctionAddress(hSdkDll, "CameraWriteSN", typeof(pfnCameraWriteSN));
            CameraReadSN = (pfnCameraReadSN)GetFunctionAddress(hSdkDll, "CameraReadSN", typeof(pfnCameraReadSN));
            CameraSetTriggerDelayTime = (pfnCameraSetTriggerDelayTime)GetFunctionAddress(hSdkDll, "CameraSetTriggerDelayTime", typeof(pfnCameraSetTriggerDelayTime));
            CameraGetTriggerDelayTime = (pfnCameraGetTriggerDelayTime)GetFunctionAddress(hSdkDll, "CameraGetTriggerDelayTime", typeof(pfnCameraGetTriggerDelayTime));
            CameraSetTriggerCount = (pfnCameraSetTriggerCount)GetFunctionAddress(hSdkDll, "CameraSetTriggerCount", typeof(pfnCameraSetTriggerCount));
            CameraGetTriggerCount = (pfnCameraGetTriggerCount)GetFunctionAddress(hSdkDll, "CameraGetTriggerCount", typeof(pfnCameraGetTriggerCount));
            CameraSoftTrigger = (pfnCameraSoftTrigger)GetFunctionAddress(hSdkDll, "CameraSoftTrigger", typeof(pfnCameraSoftTrigger));
            CameraSetTriggerMode = (pfnCameraSetTriggerMode)GetFunctionAddress(hSdkDll, "CameraSetTriggerMode", typeof(pfnCameraSetTriggerMode));
            CameraGetTriggerMode = (pfnCameraGetTriggerMode)GetFunctionAddress(hSdkDll, "CameraGetTriggerMode", typeof(pfnCameraGetTriggerMode));
            CameraSetStrobeMode = (pfnCameraSetStrobeMode)GetFunctionAddress(hSdkDll, "CameraSetStrobeMode", typeof(pfnCameraSetStrobeMode));
            CameraGetStrobeMode = (pfnCameraGetStrobeMode)GetFunctionAddress(hSdkDll, "CameraGetStrobeMode", typeof(pfnCameraGetStrobeMode));
            CameraSetStrobeDelayTime = (pfnCameraSetStrobeDelayTime)GetFunctionAddress(hSdkDll, "CameraSetStrobeDelayTime", typeof(pfnCameraSetStrobeDelayTime));
            CameraGetStrobeDelayTime = (pfnCameraGetStrobeDelayTime)GetFunctionAddress(hSdkDll, "CameraGetStrobeDelayTime", typeof(pfnCameraGetStrobeDelayTime));
            CameraSetStrobePulseWidth = (pfnCameraSetStrobePulseWidth)GetFunctionAddress(hSdkDll, "CameraSetStrobePulseWidth", typeof(pfnCameraSetStrobePulseWidth));
            CameraGetStrobePulseWidth = (pfnCameraGetStrobePulseWidth)GetFunctionAddress(hSdkDll, "CameraGetStrobePulseWidth", typeof(pfnCameraGetStrobePulseWidth));
            CameraSetStrobePolarity = (pfnCameraSetStrobePolarity)GetFunctionAddress(hSdkDll, "CameraSetStrobePolarity", typeof(pfnCameraSetStrobePolarity));
            CameraGetStrobePolarity = (pfnCameraGetStrobePolarity)GetFunctionAddress(hSdkDll, "CameraGetStrobePolarity", typeof(pfnCameraGetStrobePolarity));
            CameraSetExtTrigSignalType = (pfnCameraSetExtTrigSignalType)GetFunctionAddress(hSdkDll, "CameraSetExtTrigSignalType", typeof(pfnCameraSetExtTrigSignalType));
            CameraGetExtTrigSignalType = (pfnCameraGetExtTrigSignalType)GetFunctionAddress(hSdkDll, "CameraGetExtTrigSignalType", typeof(pfnCameraGetExtTrigSignalType));
            CameraSetExtTrigShutterType = (pfnCameraSetExtTrigShutterType)GetFunctionAddress(hSdkDll, "CameraSetExtTrigShutterType", typeof(pfnCameraSetExtTrigShutterType));
            CameraGetExtTrigShutterType = (pfnCameraGetExtTrigShutterType)GetFunctionAddress(hSdkDll, "CameraGetExtTrigShutterType", typeof(pfnCameraGetExtTrigShutterType));
            CameraSetExtTrigDelayTime = (pfnCameraSetExtTrigDelayTime)GetFunctionAddress(hSdkDll, "CameraSetExtTrigDelayTime", typeof(pfnCameraSetExtTrigDelayTime));
            CameraGetExtTrigDelayTime = (pfnCameraGetExtTrigDelayTime)GetFunctionAddress(hSdkDll, "CameraGetExtTrigDelayTime", typeof(pfnCameraGetExtTrigDelayTime));
            CameraSetExtTrigJitterTime = (pfnCameraSetExtTrigJitterTime)GetFunctionAddress(hSdkDll, "CameraSetExtTrigJitterTime", typeof(pfnCameraSetExtTrigJitterTime));
            CameraGetExtTrigJitterTime = (pfnCameraGetExtTrigJitterTime)GetFunctionAddress(hSdkDll, "CameraGetExtTrigJitterTime", typeof(pfnCameraGetExtTrigJitterTime));
            CameraGetExtTrigCapability = (pfnCameraGetExtTrigCapability)GetFunctionAddress(hSdkDll, "CameraGetExtTrigCapability", typeof(pfnCameraGetExtTrigCapability));
            CameraPauseLevelTrigger = (pfnCameraPauseLevelTrigger)GetFunctionAddress(hSdkDll, "CameraPauseLevelTrigger", typeof(pfnCameraPauseLevelTrigger));
            CameraGetResolutionForSnap = (pfnCameraGetResolutionForSnap)GetFunctionAddress(hSdkDll, "CameraGetResolutionForSnap", typeof(pfnCameraGetResolutionForSnap));
            CameraSetResolutionForSnap = (pfnCameraSetResolutionForSnap)GetFunctionAddress(hSdkDll, "CameraSetResolutionForSnap", typeof(pfnCameraSetResolutionForSnap));
            CameraCustomizeResolution = (pfnCameraCustomizeResolution)GetFunctionAddress(hSdkDll, "CameraCustomizeResolution", typeof(pfnCameraCustomizeResolution));
            CameraCustomizeReferWin = (pfnCameraCustomizeReferWin)GetFunctionAddress(hSdkDll, "CameraCustomizeReferWin", typeof(pfnCameraCustomizeReferWin));
            CameraShowSettingPage = (pfnCameraShowSettingPage)GetFunctionAddress(hSdkDll, "CameraShowSettingPage", typeof(pfnCameraShowSettingPage));
            _CameraCreateSettingPage = (pfnCameraCreateSettingPage)GetFunctionAddress(hSdkDll, "CameraCreateSettingPage", typeof(pfnCameraCreateSettingPage));
            CameraSetActiveSettingSubPage = (pfnCameraSetActiveSettingSubPage)GetFunctionAddress(hSdkDll, "CameraSetActiveSettingSubPage", typeof(pfnCameraSetActiveSettingSubPage));
            CameraSetSettingPageParent = (pfnCameraSetSettingPageParent)GetFunctionAddress(hSdkDll, "CameraSetSettingPageParent", typeof(pfnCameraSetSettingPageParent));
            CameraGetSettingPageHWnd = (pfnCameraGetSettingPageHWnd)GetFunctionAddress(hSdkDll, "CameraGetSettingPageHWnd", typeof(pfnCameraGetSettingPageHWnd));
            CameraSpecialControl = (pfnCameraSpecialControl)GetFunctionAddress(hSdkDll, "CameraSpecialControl", typeof(pfnCameraSpecialControl));
            CameraGetFrameStatistic = (pfnCameraGetFrameStatistic)GetFunctionAddress(hSdkDll, "CameraGetFrameStatistic", typeof(pfnCameraGetFrameStatistic));
            CameraSetNoiseFilter = (pfnCameraSetNoiseFilter)GetFunctionAddress(hSdkDll, "CameraSetNoiseFilter", typeof(pfnCameraSetNoiseFilter));
            CameraGetNoiseFilterState = (pfnCameraGetNoiseFilterState)GetFunctionAddress(hSdkDll, "CameraGetNoiseFilterState", typeof(pfnCameraGetNoiseFilterState));
            CameraRstTimeStamp = (pfnCameraRstTimeStamp)GetFunctionAddress(hSdkDll, "CameraRstTimeStamp", typeof(pfnCameraRstTimeStamp));
            CameraSaveUserData = (pfnCameraSaveUserData)GetFunctionAddress(hSdkDll, "CameraSaveUserData", typeof(pfnCameraSaveUserData));
            CameraLoadUserData = (pfnCameraLoadUserData)GetFunctionAddress(hSdkDll, "CameraLoadUserData", typeof(pfnCameraLoadUserData));
            CameraGetFriendlyName = (pfnCameraGetFriendlyName)GetFunctionAddress(hSdkDll, "CameraGetFriendlyName", typeof(pfnCameraGetFriendlyName));
            CameraSetFriendlyName = (pfnCameraSetFriendlyName)GetFunctionAddress(hSdkDll, "CameraSetFriendlyName", typeof(pfnCameraSetFriendlyName));
            CameraSdkGetVersionString = (pfnCameraSdkGetVersionString)GetFunctionAddress(hSdkDll, "CameraSdkGetVersionString", typeof(pfnCameraSdkGetVersionString));
            CameraCheckFwUpdate = (pfnCameraCheckFwUpdate)GetFunctionAddress(hSdkDll, "CameraCheckFwUpdate", typeof(pfnCameraCheckFwUpdate));
            CameraGetFirmwareVision = (pfnCameraGetFirmwareVision)GetFunctionAddress(hSdkDll, "CameraGetFirmwareVision", typeof(pfnCameraGetFirmwareVision));
            CameraGetEnumInfo = (pfnCameraGetEnumInfo)GetFunctionAddress(hSdkDll, "CameraGetEnumInfo", typeof(pfnCameraGetEnumInfo));
            CameraGetInerfaceVersion = (pfnCameraGetInerfaceVersion)GetFunctionAddress(hSdkDll, "CameraGetInerfaceVersion", typeof(pfnCameraGetInerfaceVersion));
            CameraSetIOState = (pfnCameraSetIOState)GetFunctionAddress(hSdkDll, "CameraSetIOState", typeof(pfnCameraSetIOState));
            CameraGetIOState = (pfnCameraGetIOState)GetFunctionAddress(hSdkDll, "CameraGetIOState", typeof(pfnCameraGetIOState));
            CameraSetInPutIOMode = (pfnCameraSetInPutIOMode)GetFunctionAddress(hSdkDll, "CameraSetInPutIOMode", typeof(pfnCameraSetInPutIOMode));
            CameraSetOutPutIOMode = (pfnCameraSetOutPutIOMode)GetFunctionAddress(hSdkDll, "CameraSetOutPutIOMode", typeof(pfnCameraSetOutPutIOMode));
            CameraSetOutPutPWM = (pfnCameraSetOutPutPWM)GetFunctionAddress(hSdkDll, "CameraSetOutPutPWM", typeof(pfnCameraSetOutPutPWM));
            CameraSetAeAlgorithm = (pfnCameraSetAeAlgorithm)GetFunctionAddress(hSdkDll, "CameraSetAeAlgorithm", typeof(pfnCameraSetAeAlgorithm));
            CameraGetAeAlgorithm = (pfnCameraGetAeAlgorithm)GetFunctionAddress(hSdkDll, "CameraGetAeAlgorithm", typeof(pfnCameraGetAeAlgorithm));
            CameraSetBayerDecAlgorithm = (pfnCameraSetBayerDecAlgorithm)GetFunctionAddress(hSdkDll, "CameraSetBayerDecAlgorithm", typeof(pfnCameraSetBayerDecAlgorithm));
            CameraGetBayerDecAlgorithm = (pfnCameraGetBayerDecAlgorithm)GetFunctionAddress(hSdkDll, "CameraGetBayerDecAlgorithm", typeof(pfnCameraGetBayerDecAlgorithm));
            CameraSetIspProcessor = (pfnCameraSetIspProcessor)GetFunctionAddress(hSdkDll, "CameraSetIspProcessor", typeof(pfnCameraSetIspProcessor));
            CameraGetIspProcessor = (pfnCameraGetIspProcessor)GetFunctionAddress(hSdkDll, "CameraGetIspProcessor", typeof(pfnCameraGetIspProcessor));
            CameraSetBlackLevel = (pfnCameraSetBlackLevel)GetFunctionAddress(hSdkDll, "CameraSetBlackLevel", typeof(pfnCameraSetBlackLevel));
            CameraGetBlackLevel = (pfnCameraGetBlackLevel)GetFunctionAddress(hSdkDll, "CameraGetBlackLevel", typeof(pfnCameraGetBlackLevel));
            CameraSetWhiteLevel = (pfnCameraSetWhiteLevel)GetFunctionAddress(hSdkDll, "CameraSetWhiteLevel", typeof(pfnCameraSetWhiteLevel));
            CameraGetWhiteLevel = (pfnCameraGetWhiteLevel)GetFunctionAddress(hSdkDll, "CameraGetWhiteLevel", typeof(pfnCameraGetWhiteLevel));
            CameraIsOpened = (pfnCameraIsOpened)GetFunctionAddress(hSdkDll, "CameraIsOpened", typeof(pfnCameraIsOpened));
            CameraEnumerateDeviceEx = (pfnCameraEnumerateDeviceEx)GetFunctionAddress(hSdkDll, "CameraEnumerateDeviceEx", typeof(pfnCameraEnumerateDeviceEx));
            CameraInitEx = (pfnCameraInitEx)GetFunctionAddress(hSdkDll, "CameraInitEx", typeof(pfnCameraInitEx));
            CameraInitEx2 = (pfnCameraInitEx2)GetFunctionAddress(hSdkDll, "CameraInitEx2", typeof(pfnCameraInitEx2));
            CameraGetImageBufferEx = (pfnCameraGetImageBufferEx)GetFunctionAddress(hSdkDll, "CameraGetImageBufferEx", typeof(pfnCameraGetImageBufferEx));
            CameraImageProcessEx = (pfnCameraImageProcessEx)GetFunctionAddress(hSdkDll, "CameraImageProcessEx", typeof(pfnCameraImageProcessEx));
            CameraSetIspOutFormat = (pfnCameraSetIspOutFormat)GetFunctionAddress(hSdkDll, "CameraSetIspOutFormat", typeof(pfnCameraSetIspOutFormat));
            CameraGetIspOutFormat = (pfnCameraGetIspOutFormat)GetFunctionAddress(hSdkDll, "CameraGetIspOutFormat", typeof(pfnCameraGetIspOutFormat));
            CameraReConnect = (pfnCameraReConnect)GetFunctionAddress(hSdkDll, "CameraReConnect", typeof(pfnCameraReConnect));
            CameraConnectTest = (pfnCameraConnectTest)GetFunctionAddress(hSdkDll, "CameraConnectTest", typeof(pfnCameraConnectTest));
            CameraSetLedEnable = (pfnCameraSetLedEnable)GetFunctionAddress(hSdkDll, "CameraSetLedEnable", typeof(pfnCameraSetLedEnable));
            CameraGetLedEnable = (pfnCameraGetLedEnable)GetFunctionAddress(hSdkDll, "CameraGetLedEnable", typeof(pfnCameraGetLedEnable));
            CameraSetLedOnOff = (pfnCameraSetLedOnOff)GetFunctionAddress(hSdkDll, "CameraSetLedOnOff", typeof(pfnCameraSetLedOnOff));
            CameraGetLedOnOff = (pfnCameraGetLedOnOff)GetFunctionAddress(hSdkDll, "CameraGetLedOnOff", typeof(pfnCameraGetLedOnOff));
            CameraSetLedDuration = (pfnCameraSetLedDuration)GetFunctionAddress(hSdkDll, "CameraSetLedDuration", typeof(pfnCameraSetLedDuration));
            CameraGetLedDuration = (pfnCameraGetLedDuration)GetFunctionAddress(hSdkDll, "CameraGetLedDuration", typeof(pfnCameraGetLedDuration));
            CameraSetLedBrightness = (pfnCameraSetLedBrightness)GetFunctionAddress(hSdkDll, "CameraSetLedBrightness", typeof(pfnCameraSetLedBrightness));
            CameraGetLedBrightness = (pfnCameraGetLedBrightness)GetFunctionAddress(hSdkDll, "CameraGetLedBrightness", typeof(pfnCameraGetLedBrightness));
            CameraEnableTransferRoi = (pfnCameraEnableTransferRoi)GetFunctionAddress(hSdkDll, "CameraEnableTransferRoi", typeof(pfnCameraEnableTransferRoi));
            CameraSetTransferRoi = (pfnCameraSetTransferRoi)GetFunctionAddress(hSdkDll, "CameraSetTransferRoi", typeof(pfnCameraSetTransferRoi));
            CameraGetTransferRoi = (pfnCameraGetTransferRoi)GetFunctionAddress(hSdkDll, "CameraGetTransferRoi", typeof(pfnCameraGetTransferRoi));
            CameraAlignMalloc = (pfnCameraAlignMalloc)GetFunctionAddress(hSdkDll, "CameraAlignMalloc", typeof(pfnCameraAlignMalloc));
            CameraAlignFree = (pfnCameraAlignFree)GetFunctionAddress(hSdkDll, "CameraAlignFree", typeof(pfnCameraAlignFree));
            CameraSetAutoConnect = (pfnCameraSetAutoConnect)GetFunctionAddress(hSdkDll, "CameraSetAutoConnect", typeof(pfnCameraSetAutoConnect));
            CameraGetReConnectCounts = (pfnCameraGetReConnectCounts)GetFunctionAddress(hSdkDll, "CameraGetReConnectCounts", typeof(pfnCameraGetReConnectCounts));
            CameraEvaluateImageDefinition = (pfnCameraEvaluateImageDefinition)GetFunctionAddress(hSdkDll, "CameraEvaluateImageDefinition", typeof(pfnCameraEvaluateImageDefinition));
            CameraDrawText = (pfnCameraDrawText)GetFunctionAddress(hSdkDll, "CameraDrawText", typeof(pfnCameraDrawText));
            _CameraGigeGetIp = (pfnCameraGigeGetIp)GetFunctionAddress(hSdkDll, "CameraGigeGetIp", typeof(pfnCameraGigeGetIp));
            CameraGigeSetIp = (pfnCameraGigeSetIp)GetFunctionAddress(hSdkDll, "CameraGigeSetIp", typeof(pfnCameraGigeSetIp));
            _CameraGigeGetMac = (pfnCameraGigeGetMac)GetFunctionAddress(hSdkDll, "CameraGigeGetMac", typeof(pfnCameraGigeGetMac));
            CameraEnableFastResponse = (pfnCameraEnableFastResponse)GetFunctionAddress(hSdkDll, "CameraEnableFastResponse", typeof(pfnCameraEnableFastResponse));
            CameraSetCorrectDeadPixel = (pfnCameraSetCorrectDeadPixel)GetFunctionAddress(hSdkDll, "CameraSetCorrectDeadPixel", typeof(pfnCameraSetCorrectDeadPixel));
            CameraGetCorrectDeadPixel = (pfnCameraGetCorrectDeadPixel)GetFunctionAddress(hSdkDll, "CameraGetCorrectDeadPixel", typeof(pfnCameraGetCorrectDeadPixel));
            CameraFlatFieldingCorrectSetEnable = (pfnCameraFlatFieldingCorrectSetEnable)GetFunctionAddress(hSdkDll, "CameraFlatFieldingCorrectSetEnable", typeof(pfnCameraFlatFieldingCorrectSetEnable));
            CameraFlatFieldingCorrectGetEnable = (pfnCameraFlatFieldingCorrectGetEnable)GetFunctionAddress(hSdkDll, "CameraFlatFieldingCorrectGetEnable", typeof(pfnCameraFlatFieldingCorrectGetEnable));
            CameraFlatFieldingCorrectSetParameter = (pfnCameraFlatFieldingCorrectSetParameter)GetFunctionAddress(hSdkDll, "CameraFlatFieldingCorrectSetParameter", typeof(pfnCameraFlatFieldingCorrectSetParameter));
            CameraFlatFieldingCorrectSaveParameterToFile = (pfnCameraFlatFieldingCorrectSaveParameterToFile)GetFunctionAddress(hSdkDll, "CameraFlatFieldingCorrectSaveParameterToFile", typeof(pfnCameraFlatFieldingCorrectSaveParameterToFile));
            CameraFlatFieldingCorrectLoadParameterFromFile = (pfnCameraFlatFieldingCorrectLoadParameterFromFile)GetFunctionAddress(hSdkDll, "CameraFlatFieldingCorrectLoadParameterFromFile", typeof(pfnCameraFlatFieldingCorrectLoadParameterFromFile));
            _CameraCommonCall = (pfnCameraCommonCall)GetFunctionAddress(hSdkDll, "CameraCommonCall", typeof(pfnCameraCommonCall));
            CameraSetDenoise3DParams = (pfnCameraSetDenoise3DParams)GetFunctionAddress(hSdkDll, "CameraSetDenoise3DParams", typeof(pfnCameraSetDenoise3DParams));
            CameraGetDenoise3DParams = (pfnCameraGetDenoise3DParams)GetFunctionAddress(hSdkDll, "CameraGetDenoise3DParams", typeof(pfnCameraGetDenoise3DParams));
            CameraManualDenoise3D = (pfnCameraManualDenoise3D)GetFunctionAddress(hSdkDll, "CameraManualDenoise3D", typeof(pfnCameraManualDenoise3D));
            CameraCustomizeDeadPixels = (pfnCameraCustomizeDeadPixels)GetFunctionAddress(hSdkDll, "CameraCustomizeDeadPixels", typeof(pfnCameraCustomizeDeadPixels));
            CameraReadDeadPixels = (pfnCameraReadDeadPixels)GetFunctionAddress(hSdkDll, "CameraReadDeadPixels", typeof(pfnCameraReadDeadPixels));
            CameraAddDeadPixels = (pfnCameraAddDeadPixels)GetFunctionAddress(hSdkDll, "CameraAddDeadPixels", typeof(pfnCameraAddDeadPixels));
            CameraRemoveDeadPixels = (pfnCameraRemoveDeadPixels)GetFunctionAddress(hSdkDll, "CameraRemoveDeadPixels", typeof(pfnCameraRemoveDeadPixels));
            CameraRemoveAllDeadPixels = (pfnCameraRemoveAllDeadPixels)GetFunctionAddress(hSdkDll, "CameraRemoveAllDeadPixels", typeof(pfnCameraRemoveAllDeadPixels));
            CameraSaveDeadPixels = (pfnCameraSaveDeadPixels)GetFunctionAddress(hSdkDll, "CameraSaveDeadPixels", typeof(pfnCameraSaveDeadPixels));
            CameraSaveDeadPixelsToFile = (pfnCameraSaveDeadPixelsToFile)GetFunctionAddress(hSdkDll, "CameraSaveDeadPixelsToFile", typeof(pfnCameraSaveDeadPixelsToFile));
            CameraLoadDeadPixelsFromFile = (pfnCameraLoadDeadPixelsFromFile)GetFunctionAddress(hSdkDll, "CameraLoadDeadPixelsFromFile", typeof(pfnCameraLoadDeadPixelsFromFile));
            CameraGetImageBufferPriority = (pfnCameraGetImageBufferPriority)GetFunctionAddress(hSdkDll, "CameraGetImageBufferPriority", typeof(pfnCameraGetImageBufferPriority));
            CameraGetImageBufferPriorityEx = (pfnCameraGetImageBufferPriorityEx)GetFunctionAddress(hSdkDll, "CameraGetImageBufferPriorityEx", typeof(pfnCameraGetImageBufferPriorityEx));
            CameraGetImageBufferPriorityEx2 = (pfnCameraGetImageBufferPriorityEx2)GetFunctionAddress(hSdkDll, "CameraGetImageBufferPriorityEx2", typeof(pfnCameraGetImageBufferPriorityEx2));
            CameraGetImageBufferPriorityEx3 = (pfnCameraGetImageBufferPriorityEx3)GetFunctionAddress(hSdkDll, "CameraGetImageBufferPriorityEx3", typeof(pfnCameraGetImageBufferPriorityEx3));
            CameraClearBuffer = (pfnCameraClearBuffer)GetFunctionAddress(hSdkDll, "CameraClearBuffer", typeof(pfnCameraClearBuffer));
            CameraSoftTriggerEx = (pfnCameraSoftTriggerEx)GetFunctionAddress(hSdkDll, "CameraSoftTriggerEx", typeof(pfnCameraSoftTriggerEx));
            CameraSetHDR = (pfnCameraSetHDR)GetFunctionAddress(hSdkDll, "CameraSetHDR", typeof(pfnCameraSetHDR));
            CameraGetHDR = (pfnCameraGetHDR)GetFunctionAddress(hSdkDll, "CameraGetHDR", typeof(pfnCameraGetHDR));
            CameraGetFrameID = (pfnCameraGetFrameID)GetFunctionAddress(hSdkDll, "CameraGetFrameID", typeof(pfnCameraGetFrameID));
            CameraGetFrameTimeStamp = (pfnCameraGetFrameTimeStamp)GetFunctionAddress(hSdkDll, "CameraGetFrameTimeStamp", typeof(pfnCameraGetFrameTimeStamp));
            CameraSetHDRGainMode = (pfnCameraSetHDRGainMode)GetFunctionAddress(hSdkDll, "CameraSetHDRGainMode", typeof(pfnCameraSetHDRGainMode));
            CameraGetHDRGainMode = (pfnCameraGetHDRGainMode)GetFunctionAddress(hSdkDll, "CameraGetHDRGainMode", typeof(pfnCameraGetHDRGainMode));
            CameraDrawFrameBuffer = (pfnCameraDrawFrameBuffer)GetFunctionAddress(hSdkDll, "CameraDrawFrameBuffer", typeof(pfnCameraDrawFrameBuffer));
            CameraFlipFrameBuffer = (pfnCameraFlipFrameBuffer)GetFunctionAddress(hSdkDll, "CameraFlipFrameBuffer", typeof(pfnCameraFlipFrameBuffer));
            CameraConvertFrameBufferFormat = (pfnCameraConvertFrameBufferFormat)GetFunctionAddress(hSdkDll, "CameraConvertFrameBufferFormat", typeof(pfnCameraConvertFrameBufferFormat));
            CameraSetConnectionStatusCallback = (pfnCameraSetConnectionStatusCallback)GetFunctionAddress(hSdkDll, "CameraSetConnectionStatusCallback", typeof(pfnCameraSetConnectionStatusCallback));
			CameraSetLightingControllerMode = (pfnCameraSetLightingControllerMode)GetFunctionAddress(hSdkDll, "CameraSetLightingControllerMode", typeof(pfnCameraSetLightingControllerMode));
			CameraSetLightingControllerState = (pfnCameraSetLightingControllerState)GetFunctionAddress(hSdkDll, "CameraSetLightingControllerState", typeof(pfnCameraSetLightingControllerState));
			CameraSetFrameResendCount = (pfnCameraSetFrameResendCount)GetFunctionAddress(hSdkDll, "CameraSetFrameResendCount", typeof(pfnCameraSetFrameResendCount));
            CameraGrabber_CreateFromDevicePage = (pfnCameraGrabber_CreateFromDevicePage)GetFunctionAddress(hSdkDll, "CameraGrabber_CreateFromDevicePage", typeof(pfnCameraGrabber_CreateFromDevicePage));
            CameraGrabber_CreateByIndex = (pfnCameraGrabber_CreateByIndex)GetFunctionAddress(hSdkDll, "CameraGrabber_CreateByIndex", typeof(pfnCameraGrabber_CreateByIndex));
            CameraGrabber_CreateByName = (pfnCameraGrabber_CreateByName)GetFunctionAddress(hSdkDll, "CameraGrabber_CreateByName", typeof(pfnCameraGrabber_CreateByName));
            CameraGrabber_Create = (pfnCameraGrabber_Create)GetFunctionAddress(hSdkDll, "CameraGrabber_Create", typeof(pfnCameraGrabber_Create));
            CameraGrabber_Destroy = (pfnCameraGrabber_Destroy)GetFunctionAddress(hSdkDll, "CameraGrabber_Destroy", typeof(pfnCameraGrabber_Destroy));
            CameraGrabber_SetHWnd = (pfnCameraGrabber_SetHWnd)GetFunctionAddress(hSdkDll, "CameraGrabber_SetHWnd", typeof(pfnCameraGrabber_SetHWnd));
            CameraGrabber_SetPriority = (pfnCameraGrabber_SetPriority)GetFunctionAddress(hSdkDll, "CameraGrabber_SetPriority", typeof(pfnCameraGrabber_SetPriority));
            CameraGrabber_StartLive = (pfnCameraGrabber_StartLive)GetFunctionAddress(hSdkDll, "CameraGrabber_StartLive", typeof(pfnCameraGrabber_StartLive));
            CameraGrabber_StopLive = (pfnCameraGrabber_StopLive)GetFunctionAddress(hSdkDll, "CameraGrabber_StopLive", typeof(pfnCameraGrabber_StopLive));
            CameraGrabber_SaveImage = (pfnCameraGrabber_SaveImage)GetFunctionAddress(hSdkDll, "CameraGrabber_SaveImage", typeof(pfnCameraGrabber_SaveImage));
            CameraGrabber_SaveImageAsync = (pfnCameraGrabber_SaveImageAsync)GetFunctionAddress(hSdkDll, "CameraGrabber_SaveImageAsync", typeof(pfnCameraGrabber_SaveImageAsync));
            CameraGrabber_SaveImageAsyncEx = (pfnCameraGrabber_SaveImageAsyncEx)GetFunctionAddress(hSdkDll, "CameraGrabber_SaveImageAsyncEx", typeof(pfnCameraGrabber_SaveImageAsyncEx));
            CameraGrabber_SetSaveImageCompleteCallback = (pfnCameraGrabber_SetSaveImageCompleteCallback)GetFunctionAddress(hSdkDll, "CameraGrabber_SetSaveImageCompleteCallback", typeof(pfnCameraGrabber_SetSaveImageCompleteCallback));
            CameraGrabber_SetFrameListener = (pfnCameraGrabber_SetFrameListener)GetFunctionAddress(hSdkDll, "CameraGrabber_SetFrameListener", typeof(pfnCameraGrabber_SetFrameListener));
            CameraGrabber_SetRawCallback = (pfnCameraGrabber_SetRawCallback)GetFunctionAddress(hSdkDll, "CameraGrabber_SetRawCallback", typeof(pfnCameraGrabber_SetRawCallback));
            CameraGrabber_SetRGBCallback = (pfnCameraGrabber_SetRGBCallback)GetFunctionAddress(hSdkDll, "CameraGrabber_SetRGBCallback", typeof(pfnCameraGrabber_SetRGBCallback));
            CameraGrabber_GetCameraHandle = (pfnCameraGrabber_GetCameraHandle)GetFunctionAddress(hSdkDll, "CameraGrabber_GetCameraHandle", typeof(pfnCameraGrabber_GetCameraHandle));
            CameraGrabber_GetStat = (pfnCameraGrabber_GetStat)GetFunctionAddress(hSdkDll, "CameraGrabber_GetStat", typeof(pfnCameraGrabber_GetStat));
            CameraGrabber_GetCameraDevInfo = (pfnCameraGrabber_GetCameraDevInfo)GetFunctionAddress(hSdkDll, "CameraGrabber_GetCameraDevInfo", typeof(pfnCameraGrabber_GetCameraDevInfo));
            CameraImage_Create = (pfnCameraImage_Create)GetFunctionAddress(hSdkDll, "CameraImage_Create", typeof(pfnCameraImage_Create));
            CameraImage_CreateEmpty = (pfnCameraImage_CreateEmpty)GetFunctionAddress(hSdkDll, "CameraImage_CreateEmpty", typeof(pfnCameraImage_CreateEmpty));
            CameraImage_Destroy = (pfnCameraImage_Destroy)GetFunctionAddress(hSdkDll, "CameraImage_Destroy", typeof(pfnCameraImage_Destroy));
            CameraImage_GetData = (pfnCameraImage_GetData)GetFunctionAddress(hSdkDll, "CameraImage_GetData", typeof(pfnCameraImage_GetData));
            CameraImage_GetUserData = (pfnCameraImage_GetUserData)GetFunctionAddress(hSdkDll, "CameraImage_GetUserData", typeof(pfnCameraImage_GetUserData));
            CameraImage_SetUserData = (pfnCameraImage_SetUserData)GetFunctionAddress(hSdkDll, "CameraImage_SetUserData", typeof(pfnCameraImage_SetUserData));
            CameraImage_IsEmpty = (pfnCameraImage_IsEmpty)GetFunctionAddress(hSdkDll, "CameraImage_IsEmpty", typeof(pfnCameraImage_IsEmpty));
            CameraImage_Draw = (pfnCameraImage_Draw)GetFunctionAddress(hSdkDll, "CameraImage_Draw", typeof(pfnCameraImage_Draw));
            CameraImage_DrawFit = (pfnCameraImage_DrawFit)GetFunctionAddress(hSdkDll, "CameraImage_DrawFit", typeof(pfnCameraImage_DrawFit));
            CameraImage_DrawToDC = (pfnCameraImage_DrawToDC)GetFunctionAddress(hSdkDll, "CameraImage_DrawToDC", typeof(pfnCameraImage_DrawToDC));
            CameraImage_DrawToDCFit = (pfnCameraImage_DrawToDCFit)GetFunctionAddress(hSdkDll, "CameraImage_DrawToDCFit", typeof(pfnCameraImage_DrawToDCFit));
            CameraImage_BitBlt = (pfnCameraImage_BitBlt)GetFunctionAddress(hSdkDll, "CameraImage_BitBlt", typeof(pfnCameraImage_BitBlt));
            CameraImage_BitBltToDC = (pfnCameraImage_BitBltToDC)GetFunctionAddress(hSdkDll, "CameraImage_BitBltToDC", typeof(pfnCameraImage_BitBltToDC));
            CameraImage_SaveAsBmp = (pfnCameraImage_SaveAsBmp)GetFunctionAddress(hSdkDll, "CameraImage_SaveAsBmp", typeof(pfnCameraImage_SaveAsBmp));
            CameraImage_SaveAsJpeg = (pfnCameraImage_SaveAsJpeg)GetFunctionAddress(hSdkDll, "CameraImage_SaveAsJpeg", typeof(pfnCameraImage_SaveAsJpeg));
            CameraImage_SaveAsPng = (pfnCameraImage_SaveAsPng)GetFunctionAddress(hSdkDll, "CameraImage_SaveAsPng", typeof(pfnCameraImage_SaveAsPng));
            CameraImage_SaveAsRaw = (pfnCameraImage_SaveAsRaw)GetFunctionAddress(hSdkDll, "CameraImage_SaveAsRaw", typeof(pfnCameraImage_SaveAsRaw));
            CameraImage_IPicture = (pfnCameraImage_IPicture)GetFunctionAddress(hSdkDll, "CameraImage_IPicture", typeof(pfnCameraImage_IPicture));
            CameraGetErrorString = (pfnCameraGetErrorString)GetFunctionAddress(hSdkDll, "CameraGetErrorString", typeof(pfnCameraGetErrorString));
        }
    }
}
