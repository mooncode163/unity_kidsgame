using UnityEngine;
using System.Collections;

namespace Moonma.SysImageLib
{

    // public interface ISysImageLibDelegate
    // {
    //     void OnSysImageLibDidOpenFinish(string file, byte[] dataRGB);

    // }


    public class SysImageLib
    {
      //   public ISysImageLibDelegate iDelegate;
        public const string JAVA_CLASS = "com.moonma.common.ImageUtil"; 
        static private SysImageLib _main = null;
        public static SysImageLib main
        {
            get
            {
                if (_main == null)
                {
                    _main = new SysImageLib();

                }
                return _main;
            }
        }

        public void SetObjectInfo(string objName, string objMethod)
        {
            BasePlatformWrapper platformWrapper = PlatformWrapper.platform;
            platformWrapper.SetObjectInfo(objName, objMethod);
        }
         public void OpenImage()
        {
            BasePlatformWrapper platformWrapper = PlatformWrapper.platform;
            platformWrapper.OpenImage();
        }
         public void OpenCamera()
        {
            BasePlatformWrapper platformWrapper = PlatformWrapper.platform;
            platformWrapper.OpenCamera();
        }


    }
}
