using UnityEngine;
using System.Collections;

namespace Moonma.Tongji
{
  public class Tongji  
	{
        	public static void Init(string appKey)
		{
            TongjiBasePlatformWrapper platformWrapper = TongjiPlatformWrapper.platform;
            platformWrapper.Init(appKey);
		}

 
    }
}
