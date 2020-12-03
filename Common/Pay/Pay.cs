using UnityEngine;
using System.Collections;


namespace Moonma.Pay
{
    public class Pay
    {
        BasePlatformWrapper platformWrapper;
        static private Pay _main = null;
        public static Pay main
        {
            get
            {
                if (_main == null)
                {
                    _main = new Pay();
                    _main.InitValue();
                }
                return _main;
            }
        }
        void InitValue()
        {
            platformWrapper = PlatformWrapper.platform;
        }

        public void Init(string source, string appId, string appKey)
        {

            platformWrapper.Init(source, appId, appKey);
        }

        public void PayInfo(string title, string pic)
        {
            platformWrapper.PayInfo(title, pic);
        }
    }
}
