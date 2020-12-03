
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using System.Collections;

namespace Moonma.Media
{
internal class AndroidWrapper : BasePlatformWrapper
	{
	    public const string JAVA_CLASS = "com.moonma.common.MediaPlayer"; 
	    public override void Open(string url)
        {
		    using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
            { 
                javaClass.CallStatic("Open",url);
            }
        }

        public override void Close()
        {
		    using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
            { 
                javaClass.CallStatic("Close");
            }
        }

        public override void Play()
        {
		    using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
            { 
                javaClass.CallStatic("Play");
            }
        }
        public override void Pause()
        {
		    using(var javaClass = new AndroidJavaClass(JAVA_CLASS))
            { 
                javaClass.CallStatic("Pause");
            }
        }

    }
}
#endif
