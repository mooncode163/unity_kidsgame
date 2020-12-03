using UnityEngine;
using System.Collections;
namespace Moonma.Html.HtmlWebView
{

    public class HtmlWebView
    {

        private BasePlatformWrapper _platform = null;
        public BasePlatformWrapper platform
        {
            get
            {

                if (_platform == null)
                {
#if UNITY_ANDROID && !UNITY_EDITOR
				_platform = new AndroidWrapper();
#elif UNITY_IPHONE && !UNITY_EDITOR
				_platform = new iOSWrapper();
#else
                    _platform = new BasePlatformWrapper();
#endif
                }
                return _platform;


            }
        }

        public void Load(string url)
        {
            this.platform.Load(url);
        }

        public void SetObjectInfo(string name, string methodFinish,string methodFail)
        {
            this.platform.SetObjectInfo(name,methodFinish,methodFail);
        }

    }


}
