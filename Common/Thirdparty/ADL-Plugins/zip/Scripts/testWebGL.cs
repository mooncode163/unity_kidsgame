using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;




public class testWebGL : MonoBehaviour
{
#if UNITY_WEBGL

	private string myFile = "testLZ4b.png.gz", myFile2 = "gzipMerged.jpg", myFile3 = "test2Zip.zip";

	//an output Buffer for the decompressed gz buffer
	private byte[] outbuffer = null, outbuffer2 = null;
	private Texture2D tex = null, tex2 = null, tex3 = null, tex4 = null, tex5 = null, tex6 = null, tex7 = null;

	#if UNITY_EDITOR
		byte[] wwb = null, wwb3 = null, zipwww = null;
	#else
		private WWW ww2 = null, ww3 = null, zipwww = null;
	#endif
	
	string path = "";

	private bool downloadDone2;

	private string log = "";

    //log for output of results
    void plog(string t)
    {
        log += t + "\n"; ;
    }

	void Start(){
		path = Application.streamingAssetsPath;

		tex = new Texture2D(1,1,TextureFormat.RGBA32, false);
        tex2 = new Texture2D(1,1,TextureFormat.RGBA32, false);
        tex3 = new Texture2D(1,1,TextureFormat.RGBA32, false);
        tex4 = new Texture2D(1,1,TextureFormat.RGBA32, false);
        tex5 = new Texture2D(1,1,TextureFormat.RGBA32, false);
        tex6 = new Texture2D(1,1,TextureFormat.RGBA32, false);
        tex7 = new Texture2D(1,1,TextureFormat.RGBA32, false);

		//get a gz file from StreamingAssets
		#if !UNITY_EDITOR
			StartCoroutine( getFromStreamingAssets() );		
		#else
			StartCoroutine( getFromStreamingAssetsEditor() );
		#endif
		
    }




	void OnGUI(){
		
		if (downloadDone2 == true) {
		
		    if(tex != null) GUI.DrawTexture(new Rect(200, 10, 185, 140), tex);
            if(tex2 != null) GUI.DrawTexture(new Rect(395, 10, 185, 140), tex2);
            if(tex3 != null) GUI.DrawTexture(new Rect(590, 10,185, 140), tex3);		
            if(tex4 != null) GUI.DrawTexture(new Rect(785, 10,185, 140), tex4);
            if(tex5 != null) GUI.DrawTexture(new Rect(980, 10,185, 140), tex5);
            if(tex6 != null) GUI.DrawTexture(new Rect(200, 160,125, 100), tex6);
            if(tex7 != null) GUI.DrawTexture(new Rect(335, 160,125, 100), tex7);

            // gzip read from buffer and decompress. gzip compress a buffer.
			if (GUI.Button(new Rect(5, 5, 180, 40), "StreamingAssets gz test")) {
                log = "";
                byte[] buf = null;
                #if !UNITY_EDITOR
                    buf = ww2.bytes;
                #else
                    buf = wwb;
                #endif
                //decompress a gzip buffer.
				plog("ungzip2: "+lzip.unGzip2(buf, outbuffer).ToString()+" bytes"+ "\n");

				if(outbuffer != null) {
                    tex.LoadImage(outbuffer);
                    //compress a buffer to a gzip buffer.
                    plog("gzip a buffer");
                    var tempBuffer = new byte[outbuffer.Length + 18]; // a temp buffer where the compression will occur.
                    int res = lzip.gzip(outbuffer, tempBuffer, 9); // do compression
                    var gzippedBuffer = new byte[res]; // create the final gzipped buffer
                    Buffer.BlockCopy(tempBuffer, 0, gzippedBuffer, 0, res); // copy the compressed data to it.
                    tempBuffer = null; // erase the the temp buffer. (or it could be a reusable fixed buffer to avoid allocations.)
                    plog(outbuffer.Length.ToString() + " -> " + res.ToString()); // show decompresed and compressed sizes
                    gzippedBuffer = null; // erase gzipped buffer
                }
			}

            // find a gzip file merged in a buffer and decompress. The gzip attached must be created with the plugin's gzip function and the overrideDateTimeWithLength = true.
            if (GUI.Button(new Rect(5, 50, 180, 40), "start merged gzip test")) {
                log = "";
                byte[] buf = null;
                #if !UNITY_EDITOR
                    buf = ww3.bytes;
                #else
                    buf = wwb3;
                #endif

                int offset = lzip.findGzStart(buf);
                int gzipSize = lzip.gzipCompressedSize(buf, offset); 
                if(gzipSize > 0) plog("Compresed size: " + gzipSize.ToString()); else plog("No compressed size stored in gzip.");
                plog("Start of gzip File: " + offset.ToString());
                plog("unGzip2Merged: "+lzip.unGzip2Merged(buf, offset, buf.Length - offset, outbuffer2).ToString()+" bytes");

                if(outbuffer2 != null) tex2.LoadImage(outbuffer2);
            }

            //check the compress/decompress zlib buffers section in the lzip.cs section for more variations
            if(outbuffer != null) {
                if (GUI.Button(new Rect(5, 95, 180, 40), "zlib buffer tests")) {
                    log = "";
                    var nt = lzip.compressBuffer(outbuffer, 9);
                    if(nt != null){
                        plog("compressed zlib buffer size: " + nt.Length.ToString());

                        var cb = lzip.decompressBuffer(nt);

                        if(cb != null) {
                            plog("decompressed zlib buffer size: " + cb.Length.ToString());
                            tex3.LoadImage(cb);
                        }
                        cb = null; nt = null;
                    }
                }
            }


            //--------------------------------------------------------------------------------------------------------------
            //
            // ZIP tests peformed in byte buffers. All operations are supported except those that need a file system. 
            // (Refer to the testZip.cs script for more examples that do not use the file system!)
            //
            //--------------------------------------------------------------------------------------------------------------

            if (GUI.Button(new Rect(5, 140, 180, 40), "zip tests 1")) {
                log = "";
                byte[] buf = null;
                #if !UNITY_EDITOR
                    buf = zipwww.bytes;
                #else
                    buf = zipwww;
                #endif

                // Handle regular zip in buffer
                //
                // get info of a zip file using the getFileInfo method.
                plog("Get Info of zip in Buffer: " + lzip.getFileInfo(null, buf).ToString());
                if(lzip.ninfo != null && lzip.ninfo.Count > 0) {
                    for(int i=0; i < lzip.ninfo.Count; i++){
                        plog("Entry no: " + (i+1).ToString() + 
                            "   " + lzip.ninfo[i] + 
                            "  uncompressed: " + lzip.uinfo[i].ToString() + 
                            "  compressed: " + lzip.cinfo[i].ToString());
                    }
                }

                //validate a zip file in a buffer and extract an entry from it to a byte buffer.
                plog("Validate zip file in Buffer: " + lzip.validateFile(null, buf).ToString());
                var ob = lzip.entry2Buffer(null, "dir1/dir2/dir3/Unity_1.jpg", buf);
                plog("entry2Buffer: " + ob.Length.ToString());
                if(ob != null) tex4.LoadImage(ob);

                byte [] merged = null;

                //create an inMemory zip file.
                if(ob != null) {
                    lzip.inMemory t = new lzip.inMemory();
                    // compress a buffer to an in memory zip with a password. bz2 compression method is not supported.
                    // The function returns the pointer to the in memory zip buffer. But you can get this also through t.pointer.
                    // This function is slow when adding multiple buffers to the inMemory zip. It is better to use the low level functions below this example.
                    lzip.compress_Buf2Mem(t,  9, ob, "inmem/test.jpg", null,"1234");
                    // print the in memory zip size in bytes.
                    plog("Create inMemory zip size: " + t.size().ToString());

                    // a buffer to perform decompression operations from an in memory zip to it.
                    byte[] bf = null;
                    // a function that decompresses an entry from an in memory zip to a buffer that will get resized to fit the output.
                    // (you can use entry2FixedBufferMem to decompress to a fixed sixed buffer or the overloaded entry2BufferMem function that returns a new buffer.)
                    plog("entry2BufferMem: " + lzip.entry2BufferMem(t,"inmem/test.jpg", ref bf, "1234").ToString());

                    // (!) If you don't need anymore the inMemory object use the free_inmemory function to free the occupied memory by the zip (!)
                    //
                    lzip.free_inmemory(t);


                    // low level functions to create inMemory zip which are faster then using the above function. ------------------------------
                    //
                    // Create an in memory object to reference our in memory zip
                    lzip.inMemory t2 = new lzip.inMemory();
                    // Initiate an inMemory zip archive
                    lzip.inMemoryZipStart(t2);
                    // Add a buffer as a first entry in the inMemory zip
                    lzip.inMemoryZipAdd(t2, 9, ob, "test.jpg");
                    // Add a second buffer as the second entry.
                    lzip.inMemoryZipAdd(t2, 9, ob, "directory/test2.jpg");
                    // !!! -> After finishing adding buffer/files in the inMemory zip we must close it <- !!! 
                    // You can reopen it later with the inMemoryZipStart function to append more entries to it.
                    lzip.inMemoryZipClose(t2);
                    // Write out the compressed size of the inMemory created zip
                    plog("Size of Low Level inMemory zip: " + t2.size().ToString());

                    byte[] bf2 = null;
                    plog("entry2BufferMem low: " + lzip.entry2BufferMem(t2,"directory/test2.jpg", ref bf2).ToString());
                    // make sure we got a valid inMemory zip by extracting an image out of it and loading it to a texture.
                    if(bf2 != null) { tex7.LoadImage(bf2); bf2 = null; }

                    // free the t2 inMemory object if you don't need it anymore.
                    lzip.free_inmemory(t2);
                    // end low memory functions -------------------------------------------------------------------------------------------------


                    if(bf != null) {
                        tex5.LoadImage(bf);
                        //create a mereged zip file for the next test
                        merged  = new byte[bf.Length + buf.Length];
                        Array.Copy(bf, 0, merged, 0, bf.Length);
                        Array.Copy(buf, 0, merged, bf.Length, buf.Length);
                    }
                }

                // Merged zip file in buffer examples.
                //
                // -----------------------------------
                if (merged != null) {
                    plog("");
                    // get info of a zip file using the merged method.
                    plog("Get Info of merged zip in Buffer: " + lzip.getZipInfoMerged( merged ).ToString());
                    if(lzip.zinfo != null && lzip.zinfo.Count > 0) {
                        for(int i=0; i < lzip.zinfo.Count; i++){
                            plog("Entry no: " + (i+1).ToString() + 
                                "   " + lzip.zinfo[i].filename + 
                                "  uncompressed: " + lzip.zinfo[i].UncompressedSize.ToString() + 
                                "  compressed: " + lzip.zinfo[i].CompressedSize.ToString());
                        }
                    }

                    // get the position and size of the merged zip in a buffer
                    int position = 0, size = 0;
                    lzip.getZipInfoMerged(merged, ref position, ref size, false);
                    plog("merged zip: position = " + position.ToString() + " , size = "+ size.ToString());

                    // get the pure zip file from where it is merged.
                    var pureZip = lzip.getMergedZip(merged);
                    if(pureZip != null) plog("got pure zip buffer with size: " + pureZip.Length.ToString());
                    pureZip = null;

                    // extract an entry from a merged zip
                    // see lzip.cs for more overloaded functions to extract to referenced or fixed sized buffers
                    var extractedData = lzip.entry2BufferMerged(merged, "dir1/dir2/dir3/Unity_1.jpg");
                    if(extractedData != null) {
                        tex6.LoadImage(extractedData);
                        plog("entry2BufferMerged: " + extractedData.Length.ToString());
                    }
                }

            }
		}



		GUI.TextArea(new Rect(10, 270, Screen.width - 20, Screen.height - 280), log);
				
	}



	// ============================================================================================================================================================= 

	

	#if !UNITY_EDITOR
    IEnumerator getFromStreamingAssets() {
		plog("getting buffer from StreamingAssets ...");

        ww2 = new WWW(path +"/"+ myFile);
        yield return ww2;

        ww3 = new WWW(path +"/"+ myFile2);
        yield return ww3;

        zipwww = new WWW(path +"/"+ myFile3);
        yield return zipwww;

        if (ww2.error != null) plog("Streaming Assets Error: " + ww2.error.ToString());
		else outbuffer = new byte[ lzip.gzipUncompressedSize(ww2.bytes) ];
		plog("Got buffer");
		downloadDone2 = true;

        if (ww3.error != null) plog("Streaming Assets Error: " + ww3.error.ToString());
		else outbuffer2 = new byte[ lzip.gzipUncompressedSize(ww3.bytes) ];

        if (zipwww.error != null) plog("Streaming Assets Error: " + zipwww.error.ToString());

    }
	#else

	IEnumerator getFromStreamingAssetsEditor() {
		plog("getting buffer from StreamingAssets ...");
		yield return true;

		if(System.IO.File.Exists(path+"/"+myFile)) {
			wwb = System.IO.File.ReadAllBytes(path+"/"+myFile);
		}


		if(wwb == null) plog("Could not find file: " + myFile + " in StreamingAssets");
		else outbuffer = new byte[ lzip.gzipUncompressedSize(wwb) ];
		plog("Got buffer");
		downloadDone2 = true;

        if(System.IO.File.Exists(path+"/" + myFile2)) {
			wwb3 = System.IO.File.ReadAllBytes(path + "/" + myFile2);
		}

        outbuffer2 = new byte[ lzip.gzipUncompressedSize(wwb3) ];

        if(System.IO.File.Exists(path + "/" + myFile3)) {
			zipwww = System.IO.File.ReadAllBytes(path + "/" + myFile3);
		}
	}
	#endif
#else
    void Start(){
        Debug.Log("Only for WebGL");
    }
#endif

}

