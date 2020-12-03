#if UNITY_IPHONE && !UNITY_EDITOR
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections;
 
	internal class TTSiOSWrapper : TTSBasePlatformWrapper
	{
		 

		[DllImport ("__Internal")]
		public static extern void TTS_speekText(string text);

	 

		public override void Speak(string text)
		 {

			 TTS_speekText(text); 
		 }	
		 
		 
	}

#endif