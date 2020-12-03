using UnityEngine;
using System.Collections;

namespace Moonma.IAP
{
    public class IAP
    {
        //         public const string   ""
        // public const string   ""public const string   ""
        // public const string   ""
        public const string UNITY_CALLBACK_BUY_DID_FINISH = "BuyDidFinish";//购买成功
        public const string UNITY_CALLBACK_BUY_DID_Fail = "BuyDidFail";//购买失败
        public const string UNITY_CALLBACK_DID_BUY = "DidBuy";//已经购买项目
        public const string UNITY_CALLBACK_BUY_DID_RESTORE = "BuyDidRestore";//恢复购买
        public const string UNITY_CALLBACK_BUY_CANCEL_BY_USER = "BuyCancelByUser";

        public const string IAP_SOURCE_IOS_APPSTORE = "appstore";

        public const string IAP_PRODUCT_NOAD = "noad";

        public const string STR_KEYNAME_VIEWALERT_LOADING = "STR_KEYNAME_VIEWALERT_LOADING";
        static public string productIdNoAD
        {
            get
            {
                string str = IAP_PRODUCT_NOAD;
                if (Common.isiOS)
                {
                    //ios 不同的app的product id都不能相同
                    str = Config.main.IDNoadIAP;
                    if (Common.BlankString(str))
                    {
                        str = Common.GetAppPackage() + "." + IAP_PRODUCT_NOAD;
                    }

                }
                if (Common.isAndroid)
                {
                    str = IAP_PRODUCT_NOAD;
                }
                return str;
            }
        }

        static private IAP _main = null;
        public static IAP main
        {
            get
            {
                if (_main == null)
                {
                    _main = new IAP();
                    _main.SetSource(Config.main.sourceIAP);

                }
                return _main;
            }
        }

        public void SetObjectInfo(string objName, string objMethod)
        {
            BasePlatformWrapper platformWrapper = PlatformWrapper.platform;
            platformWrapper.SetObjectInfo(objName, objMethod);
        }
        public void SetSource(string source)
        {
            BasePlatformWrapper platformWrapper = PlatformWrapper.platform;
            platformWrapper.SetSource(source);

            if (source == Source.GP)
            {
                string appkey = Config.main.iapAppKeyGoogle;
                platformWrapper.SetAppKey(appkey);
            }
        }
        //consume 消费类型
        public void StartBuy(string product, bool isConsume)
        {
            BasePlatformWrapper platformWrapper = PlatformWrapper.platform;
            platformWrapper.StartBuy(product, isConsume);
        }

        public void StartBuy(string product)
        {
            StartBuy(product, true);
        }

        public void RestoreBuy(string product)
        {
            BasePlatformWrapper platformWrapper = PlatformWrapper.platform;
            platformWrapper.RestoreBuy(product);
        }

        public void StopBuy(string product)
        {
            BasePlatformWrapper platformWrapper = PlatformWrapper.platform;
            platformWrapper.StopBuy(product);
        }



        void OnUIViewAlertFinished(UIViewAlert alert, bool isYes)
        {
        }

        void ShowAlertRemoveAdFinish()
        {
            string title = Language.main.GetString("STR_UIVIEWALERT_TITLE_REMOVE_AD_FINISH");
            string msg = Language.main.GetString("STR_UIVIEWALERT_MSG_REMOVE_AD_FINISH");
            string yes = Language.main.GetString("STR_UIVIEWALERT_YES_REMOVE_AD_FINISH");
            string no = "no";
            ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_LOADING, OnUIViewAlertFinished);

        }

        public void IAPCallBackBase(string str)
        {
            Debug.Log("IAPCallBackBase::" + str);
            if (str == IAP.UNITY_CALLBACK_BUY_DID_FINISH)
            {
                //viewAlert.Hide();
                //去除广告
                Common.noad = true;
                Common.isRemoveAd = true;

                ShowAlertRemoveAdFinish();
            }
            if (str == IAP.UNITY_CALLBACK_BUY_DID_Fail)
            {
                string title = Language.main.GetString(AppString.STR_UIVIEWALERT_TITLE_SHOP_BUY_FAIL);
                string msg = Language.main.GetString(AppString.STR_UIVIEWALERT_MSG_SHOP_BUY_FAIL);
                string yes = Language.main.GetString(AppString.STR_UIVIEWALERT_YES_SHOP_BUY_FAIL);
                string no = yes;
                ViewAlertManager.main.ShowFull(title, msg, yes, no, false, STR_KEYNAME_VIEWALERT_LOADING, OnUIViewAlertFinished);

                // viewAlert.Show();
            }
            if (str == IAP.UNITY_CALLBACK_DID_BUY)
            {
                ViewAlertManager.main.Hide();
            }

            if (str == IAP.UNITY_CALLBACK_BUY_CANCEL_BY_USER)
            {
                ViewAlertManager.main.Hide();
            }
            if (str == IAP.UNITY_CALLBACK_BUY_DID_RESTORE)
            {
                ViewAlertManager.main.Hide();
            }
        }

    }
}
