using UnityEngine;
using System;
using System.Text;
using System.Threading;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class testZip : MonoBehaviour
{
#if !UNITY_WEBGL || UNITY_EDITOR

	// we use some integer to get error codes from the lzma library (look at lzma.cs for the meaning of these error codes)
	private int zres=0;
	
	private string myFile;
    private string log;
	private string ppath;
	
	private bool compressionStarted, pass;
	private bool downloadDone;

	// reusable buffers
    private byte[] reusableBuffer, reusableBuffer2, reusableBuffer3;

	// fixed size buffers, that don't get resized, to perform compression/decompression of buffers in them and avoid memory allocations.
	private byte[] fixedInBuffer = new byte[1024 * 256];
	private byte[] fixedOutBuffer = new byte[1024 * 768];
	private byte[] fixedBuffer = new byte[1024 * 1024];

    // A single item integer array that changes to the current number of file that get uncompressed of a zip archive.
    // When running the decompress_File function, compare this int to the total number of files returned by the getTotalFiles function
    // to get the progress of the extraction if the zip contains multiple files.
    // If you use multiple threads, remember to use other progress integers for the other threads, otherwise there will be a sharing violation.
    //
    private int[] progress = new int[1];

	// individual file progress (in bytes)
	private ulong[] progress2 = new ulong[1];


    // log for output of results
    void plog(string t)
    {
        log += t + "\n"; ;
    }

    void Start() {

        ppath = Application.persistentDataPath;

#if UNITY_STANDALONE_OSX && !UNITY_EDITOR
		     ppath=".";
#endif

        Debug.Log("persistentDataPath: " + ppath);

        // various byte buffers for testing
        reusableBuffer = new byte[4096];
        reusableBuffer2 = new byte[0];
        reusableBuffer3 = new byte[0];

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // call the download coroutine to download a test file
        StartCoroutine(DownloadZipFile());
   }




	void Update(){
		if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
	}
	
	
	void OnGUI(){
		
		if (downloadDone == true) {
			GUI.Label(new Rect(10, 0, 250, 30), "package downloaded, ready to extract");
			GUI.Label(new Rect(10, 50, 650, 100), ppath);
		}
		
		if (compressionStarted){
            GUI.TextArea(new Rect(10, 160, Screen.width - 20, Screen.height - 170), log);
            GUI.Label(new Rect(Screen.width - 30, 0, 80,40), progress[0].ToString());
			GUI.Label(new Rect(Screen.width - 140, 0, 80,40), progress2[0].ToString());
		}

        if (downloadDone) {
		    if (GUI.Button(new Rect(10, 90, 230, 50), "start zip test")) {
                log = "";
			    compressionStarted = true;
			    DoDecompression();
		    }
			#if (UNITY_IPHONE || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_ANDROID || UNITY_STANDALONE_LINUX || UNITY_EDITOR  || UNITY_STANDALONE_WIN)
				if (GUI.Button(new Rect(260, 90, 230, 50), "start File Buffer test")) {
					log = "";
					compressionStarted = true;
					DoDecompression_FileBuffer();
				}
			#endif

            if(GUI.Button(new Rect(510, 90, 150, 50),"Merged zip Tests")){
                 log = "";
			    compressionStarted = true;
                
                // create a combination of a jpg and a zip file attached to it from existing files.

                // if the merged file does not exist, create it.
                if(!File.Exists(ppath + "/merged.jpg")) {
                    if(!File.Exists(ppath + "/dir1/dir2/dir3/Unity_1.jpg")){
                        lzip.decompress_File(ppath + "/testZip.zip", ppath+"/", progress,null, progress2);
                    }

                    var bf1 = File.ReadAllBytes(ppath + "/dir1/dir2/dir3/Unity_1.jpg");
                    var bf2 = File.ReadAllBytes(ppath + "/testZip.zip");

                    byte[] fb = new byte[bf1.Length + bf2.Length];

                    Array.Copy(bf1, 0, fb, 0, bf1.Length);
                    Array.Copy(bf2, 0, fb, bf1.Length, bf2.Length);

                    File.WriteAllBytes(ppath + "/merged.jpg", fb);

                    bf1 = null; bf2 = null; fb = null;
                // if the merged file exists, perfom some operations with it.
                }

                DoDecompression_Merged();
            }
        }
		
	}


    // Test all the plugin functions.
    //
    void DoDecompression(){

        //----------------------------------------------------------------------------------------------------------------
        // Commented out example on how to set the permissions of a MacOSX executable that has been unzipped so it can run.
        //
        //lzip.setFilePermissions(ppath + "/Untitled.app", "rwx","rx","rx");
        //lzip.setFilePermissions(ppath + "/Untitled.app/Contents/MacOS/Untitled", "rwx","rx","rx");
        //
        //----------------------------------------------------------------------------------------------------------------
        
		// Windows  only (see lzip.cs for more info)
		#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
		lzip.setEncoding(1);//CP_UTF8 = 65001  // CP_OEMCP/UNICODE = 1
		#endif



		// validate sanity of a zip archive
		plog("Validate: "+ lzip.validateFile(ppath + "/testZip.zip").ToString());

        // decompress the downloaded file
        zres = lzip.decompress_File(ppath + "/testZip.zip", ppath+"/", progress,null, progress2);
		plog("decompress: "+zres.ToString());
		plog("");

        // get the true total files of the zip
        plog("true total files: "+lzip.getTotalFiles(ppath + "/testZip.zip"));


        // get the total entries of the zip
        plog("true total entries: "+lzip.getTotalEntries(ppath + "/testZip.zip"));


        // entry exists
        bool eres = lzip.entryExists(ppath + "/testZip.zip", "dir1/dir2/test2.bmp");
        plog("entry exists: " + eres.ToString());

       
        // get entry dateTime
        plog("");
        plog("DateTime: " + lzip.entryDateTime(ppath + "/testZip.zip", "dir1/dir2/test2.bmp").ToString());


        // extract an entry
        zres = lzip.extract_entry(ppath + "/testZip.zip", "dir1/dir2/test2.bmp", ppath + "/test22P.bmp", null, progress2);
        plog("extract entry: " + zres.ToString());

        plog("");

        
        // compress a file and add it to a new zip
        zres = lzip.compress_File(9, ppath + "/test2Zip.zip", ppath + "/dir1/dir2/test2.bmp",false, "dir1/dir2/test2.bmp");
        plog("compress: " + zres.ToString());


        // append a file to it
        // Appending to an existing file can be slow. For faster results, multiple files should be added using the compressDir or compress_File_List functions !
        
        zres = lzip.compress_File(9, ppath + "/test2Zip.zip", ppath + "/dir1/dir2/dir3/Unity_1.jpg",true, "dir1/dir2/dir3/Unity_1.jpg","ccc");
        plog("append: " + zres.ToString());

        plog("");


        //------------------------------------------------------------------------------------------------------------------------------
        // SPANNED archives
        //------------------------------------------------------------------------------------------------------------------------------

        zres = lzip.compress_File(9, ppath + "/test2ZipSPAN.zip", ppath + "/dir1/dir2/test2.bmp", false, "dir1/dir2/test2.bmp", null, null, false, 20000);

        
        plog("compress SPAN: " + zres.ToString());

        // add a second file in the split disk archive
		zres = lzip.compress_File(9, ppath + "/test2ZipSPAN.zip", ppath + "/dir1/dir2/dir3/Unity_1.jpg", true, "dir1/dir2/dir3/Unity_1.jpg", null, null, false, 20000);
		
        plog("compress SPAN 2: " + zres.ToString());

        // decompress spanned archive
        zres = lzip.decompress_File(ppath + "/test2ZipSPAN.zip", ppath + "/SPANNED/", progress, null, progress2);
        plog("decompress SPAN: " + zres.ToString());


        plog("");

        //------------------------------------------------------------------------------------------------------------------------------

        //------------------------------------------------------------------------------------------------------------------------------

        bool useBz2 = true;
        // macos/ios/tvos do not support bz2 compression method.
        #if (UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_IOS || UNITY_TVOS)
        useBz2 = false;
        #endif

        // compress multiple files added in some lists, and protect them with a password
        //
        // create a list of files to get compressed
        List<string> myFiles = new List<string>();
        myFiles.Add(ppath + "/test22P.bmp");
        myFiles.Add(ppath + "/dir1/dir2/test2.bmp");
        // create an optional list with new names for the above listings
        List<string> myNames = new List<string>();
        myNames.Add("NEW_test22P.bmp");
        myNames.Add("dir13/dir23/New_test2.bmp");
        
        // use password and bz2 method
        zres = lzip.compress_File_List(9, ppath+"/fileList.zip", myFiles.ToArray(), progress, false, myNames.ToArray(),"password", useBz2);
        plog("MultiFile Compress password: " + zres.ToString());
        myFiles.Clear(); myFiles = null; myNames.Clear(); myNames = null;

        // decompress a password protected archive
        zres = lzip.decompress_File(ppath + "/fileList.zip", ppath+"/", progress,null, progress2,"password");
        plog("decompress password: " + zres.ToString());
                   
        plog("");


        // compress a buffer to a file and name it.
        plog( "Buffer2File: "+ lzip.buffer2File(9, ppath + "/test3Zip.zip", "buffer.bin", reusableBuffer).ToString());

        // compress a buffer, name it and append it to an existing zip archive
        plog("Buffer2File append: " + lzip.buffer2File(9, ppath + "/test3Zip.zip", "dir4/buffer.bin", reusableBuffer, useBz2).ToString());
        // Debug.Log(reusableBuffer.Length);
        plog("");

        

        // get the uncompressed size of a specific file in the zip archive
        plog("get entry size: " + lzip.getEntrySize(ppath + "/testZip.zip", "dir1/dir2/test2.bmp").ToString());
        plog("");

        
        // extract a file in a zip archive to a byte buffer (referenced buffer method)
        plog("entry2Buffer1: "+ lzip.entry2Buffer(ppath + "/testZip.zip","dir1/dir2/test2.bmp",ref reusableBuffer2).ToString() );
        // File.WriteAllBytes(ppath + "/out.bmp", reusableBuffer2);
        plog("");

        // extract an entry in a zip archive to a fixed size buffer
        plog("entry2FixedBuffer: " + lzip.entry2FixedBuffer(ppath + "/testZip.zip", "dir1/dir2/test2.bmp", ref fixedBuffer).ToString());
        plog("");


        //------------------------------------------------------------------------------------------------------------------------------
        //IN MEMORY zip functions 
        //------------------------------------------------------------------------------------------------------------------------------

        // first we create an in memory object to reference our in memory zip
        // (it is important to delete this object later. see below.)
        lzip.inMemory t = new lzip.inMemory();

        // read a file to a buffer
        byte[] reusableBuffer2a = File.ReadAllBytes(ppath + "/dir1/dir2/dir3/Unity_1.jpg");

        // compress a buffer to an in memory zip using bz2 method.
        // The function returns the pointer to the in memory zip buffer. But you can get this also through t.pointer.
        lzip.compress_Buf2Mem(t,  9, reusableBuffer2a, "inmem/Unity_1.jpg", null,"1234", true);

        reusableBuffer2a = null;

        // print the in memory zip size in bytes.
        plog("inMemory zip size: " + t.size().ToString());

        // add a second buffer to the in memory zip. If you use the same inMemory object that already has created an inMemory zip file ,
        // it will append the next one to it, resizing it!
        // Appending on an existing inMemory zip is slow as more files are added to it.
        // For really fast inMemory zip creation the lower level inMemory functions should be used. (See below.)
        lzip.compress_Buf2Mem(t, 9, reusableBuffer2, "inmem/test.bmp", null, "1234", true);

        // print the in memory zip size in bytes.
        plog("inMemory zip size: " + t.info[0].ToString());

        // Example on how to save an in memory zip file to disk.
        //
        // use the struct public function to get the gyte buffer where the inmemory zip was created
        var p = t.getZipBuffer();

        // and save the buffer to disk.
        File.WriteAllBytes(ppath+"/MEM.zip", p);
        p = null;

        int[] pr = new int[1];

        // Example on how to decompress an in memory zip to disk
        plog("decompress_Mem2File: " + lzip.decompress_Mem2File(t, ppath+"/", pr,null,"1234").ToString());

        pr = null;

        // a function to get info from an in memory zip.
        lzip.getFileInfoMem(t);

        plog("");

        // print out the name, uncompressed and compressed sizes.
        if (lzip.ninfo != null) {
            for (int i = 0; i < lzip.ninfo.Count; i++) {
                log += lzip.ninfo[i] + " - " + lzip.uinfo[i] + " / " + lzip.cinfo[i] + "\n";
            }
        }

        plog("");

        // a buffer to perform decompression operations from an in memory zip to it.
        byte[] bf = null;

        // a function that decompresses an entry from an in memory zip to a buffer that will get resized to fit the output.
        plog("entry2BufferMem: " + lzip.entry2BufferMem(t,"inmem/test.bmp", ref bf, "1234").ToString());

        // a function that decompresses an entry from an in memory zip to a fixed size buffer.
        plog("entry2FixedBufferMem: " + lzip.entry2FixedBufferMem(t, "inmem/test.bmp", ref bf, "1234").ToString());

        // a function that decompresses an entry from an in memory zip to a new created buffer and returns it.
        var bt = lzip.entry2BufferMem(t, "inmem/test.bmp",  "1234");
        plog("entry2BufferMem new buffer: " + bt.Length.ToString());

        // (!) If you don't need anymore the inMemory object use the free_inmemory function to free the occupied memory by the zip (!)
        //
        lzip.free_inmemory(t);

        plog("");
        // -----------------------------------------------------------------------------------------------------------------------------


        // Lower level inMemory functions for Faster! compressing multiple files/buffers to memory.

        // Create an in memory object to reference our in memory zip
        lzip.inMemory t2 = new lzip.inMemory();

        // Initiate an inMemory zip archive
        lzip.inMemoryZipStart(t2);

        // Read some file to a byte[] buffer (as an example only).
        reusableBuffer2a = File.ReadAllBytes(ppath + "/dir1/dir2/dir3/Unity_1.jpg");

        // Add a buffer as a first entry in the inMemory zip
        lzip.inMemoryZipAdd(t2, 9, reusableBuffer2a, "test.jpg");

        // Add a second buffer as the second entry.
        lzip.inMemoryZipAdd(t2, 9, reusableBuffer2, "directory/test.bmp");

        // !!! -> After finishing adding buffer/files in the inMemory zip we must close it <- !!! 
        lzip.inMemoryZipClose(t2);

        // Reopen the t2 inMemory zip archive and add 2 more files. They will automatically get appended.
        lzip.inMemoryZipStart(t2);
        lzip.inMemoryZipAdd(t2, 9, reusableBuffer2a, "newDir/test2.jpg");
        lzip.inMemoryZipAdd(t2, 9, reusableBuffer2, "directory2/test2.bmp");
        lzip.inMemoryZipClose(t2);

        // Write out the compressed size of the inMemory created zip
        plog("Size of Low Level inMemory zip: " + t2.size().ToString());

        // Write a file to disk to make sure that everything was done correct.
        File.WriteAllBytes(ppath+"/MEM2.zip", t2.getZipBuffer());

        // nullify the temp buffer
        reusableBuffer2a = null;

        // (!) If you don't need anymore the inMemory object use the free_inmemory function to free the occupied memory by the zip (!)
        lzip.free_inmemory(t2);

        plog("");

        //------------------------------------------------------------------------------------------------------------------------------
        // END IN MEMORY zip functions 
        //------------------------------------------------------------------------------------------------------------------------------


        //-------------------------------------------------------------------------------------------------------------------------
        // GZIP TESTS
        //-------------------------------------------------------------------------------------------------------------------------

        // create a buffer that will store the compressed gzip data.
        // it should be at least the size of the input buffer +18 bytes.
        var btt = new byte[reusableBuffer2.Length+18];

        // compress a buffer to gzip format and add gzip header and footer
        // returns total size of compressed buffer.
        //
        // set the last parameter to true if you want to include the compressed size in the gzip and use the lzip. (use the lzip.gzipCompressedSize to get it).
        int rr = lzip.gzip(reusableBuffer2,btt,9, true, true);
        plog("gzip compressed size: "+ rr);


        // create a buffer to store the compressed data (optional, if you want to write out a gzip file)
        var bt2 = new byte[rr];
        // copy the data to the new buffer
        Buffer.BlockCopy(btt, 0, bt2, 0, rr);

        // write a .gz file
        File.WriteAllBytes(ppath+"/test2.bmp.gz", bt2);

        // create a buffer to decompress a gzip buffer
        var bt3 = new byte[lzip.gzipUncompressedSize(bt2)];

        // decompress the gzip compressed buffer
        int gzres = lzip.unGzip(bt2, bt3);

        if(gzres > 0) { File.WriteAllBytes(ppath+"/test2GZIP.bmp", bt3); plog("gzip decompression: success "+gzres.ToString());}
        else {plog("gzip decompression error: "+gzres.ToString());}

        btt=null; bt2=null; bt3=null;
        plog("");

        //-------------------------------------------------------------------------------------------------------------------------
        // END GZIP TESTS
        //-------------------------------------------------------------------------------------------------------------------------

        // extract a file in a zip archive to a byte buffer (new buffer method)
        var newBuffer = lzip.entry2Buffer(ppath + "/testZip.zip", "dir1/dir2/test2.bmp");
        zres = 0;
        if (newBuffer != null) zres = 1;
        plog("entry2Buffer2: "+ zres.ToString());
        // write a file out to confirm all was ok
        //File.WriteAllBytes(ppath + "/out.bmp", newBuffer);
        plog("");


        // FIXED BUFFER FUNCTIONS:
        int compressedSize = lzip.compressBufferFixed(newBuffer, ref fixedInBuffer, 9);
        plog(" # Compress Fixed size Buffer: " + compressedSize.ToString());

        if(compressedSize>0){
            int decommpressedSize = lzip.decompressBufferFixed(fixedInBuffer, ref fixedOutBuffer);
            if(decommpressedSize > 0) plog(" # Decompress Fixed size Buffer: " + decommpressedSize.ToString());
        }
        plog("");


        // compress a buffer into a referenced buffer
        pass = lzip.compressBuffer(reusableBuffer2, ref reusableBuffer3, 9);
        plog("compressBuffer1: " + pass.ToString());
        // write a file out to confirm all was ok
        //File.WriteAllBytes(ppath + "/out.bin", reusableBuffer3);


        // compress a buffer and return a new buffer with the compresed data.
        newBuffer = lzip.compressBuffer(reusableBuffer2,9);
        zres = 0;
        if (newBuffer != null) zres = 1;
        plog("compressBuffer2: " + zres.ToString());
        plog("");


        // decompress a previously compressed buffer into a referenced buffer
        pass = lzip.decompressBuffer(reusableBuffer3, ref reusableBuffer2);
        plog("decompressBuffer1: " + pass.ToString());
        //Debug.Log(reusableBuffer2.Length);
        // write a file out to confirm all was ok
        File.WriteAllBytes(ppath + "/out.bmp", reusableBuffer2);
        zres = 0;
        if (newBuffer != null) zres = 1;


        // decompress a previously compressed buffer into a new returned buffer
        newBuffer = lzip.decompressBuffer(reusableBuffer3);
        plog("decompressBuffer2: " + zres.ToString());
        plog("");


        // get file info of the zip file (names, uncompressed and compressed sizes)
        plog( "total bytes: " + lzip.getFileInfo(ppath + "/testZip.zip").ToString());



        // Look through the ninfo, uinfo and cinfo Lists where the file names and sizes are stored.
        if (lzip.ninfo != null) {
            for (int i = 0; i < lzip.ninfo.Count; i++) {
                log += lzip.ninfo[i] + " - " + lzip.uinfo[i] + " / " + lzip.cinfo[i] + "\n";
            }
        }
        plog("");



        // Recursively compress a directory
        int[] dirProgress = new int[1];
        lzip.compressDir(ppath + "/dir1", 9, ppath + "/recursive.zip", false, dirProgress);
        plog("recursive - no. of files: " + dirProgress[0].ToString());


        // decompress the above compressed zip to make sure all was ok.
        zres = lzip.decompress_File(ppath + "/recursive.zip", ppath+"/recursive/", progress, null, progress2);
        plog("decompress recursive: "+zres.ToString());


        // multithreading example to show progress of extraction, using the ref progress int
        // in this example it happens to fast, because I didn't want the user to download a big file with many entrie.
        Thread th = new Thread(decompressFunc); th.Start();


        // delete/replace entry example
        if(File.Exists(ppath+"/test-Zip.zip")) File.Delete(ppath+"/test-Zip.zip");
        if(File.Exists(ppath+"/testZip.zip")) File.Copy(ppath+"/testZip.zip", ppath+"/test-Zip.zip");



        // replace an entry with a byte buffer
        var newBuffer3 = lzip.entry2Buffer(ppath + "/testZip.zip", "dir1/dir2/test2.bmp");
        plog("replace entry: "+lzip.replace_entry(ppath+"/test-Zip.zip", "dir1/dir2/test2.bmp", newBuffer3, 9, null).ToString() );


        // replace an entry with a file in the disk (ability to asign a password or bz2 compression)
        plog("replace entry 2: "+lzip.replace_entry(ppath+"/test-Zip.zip", "dir1/dir2/test2.bmp",ppath+"/dir1/dir2/test2.bmp", 9, null, null).ToString() );


        // delete an entry in the zip
        plog("delete entry: "+lzip.delete_entry(ppath+"/test-Zip.zip", "dir1/dir2/test2.bmp").ToString() );

        
    }

    void decompressFunc()
    {
        int res = lzip.decompress_File(ppath + "/recursive.zip", ppath + "/recursive/", progress,null, progress2);
        if (res == 1) plog("multithreaded ok"); else plog("multithreaded error: "+res.ToString());
    }






	// these functions demonstrate how to extract data from a zip file in a byte buffer.
	//
	void DoDecompression_FileBuffer() {
		#if (UNITY_IPHONE || UNITY_IOS || UNITY_STANDALONE_OSX || UNITY_ANDROID || UNITY_STANDALONE_LINUX || UNITY_EDITOR || UNITY_STANDALONE_WIN)
			// we read a downloaded zip from the Persistent data path. It could be also a file in a www.bytes buffer.
			var fileBuffer = File.ReadAllBytes(ppath + "/testZip.zip");

			plog("Validate: "+ lzip.validateFile(null, fileBuffer).ToString());


			// decompress the downloaded file
			zres = lzip.decompress_File(null, ppath+"/", progress, fileBuffer, progress2);
			plog("decompress: " + zres.ToString());


			plog("true total files: " + lzip.getTotalFiles(null, fileBuffer) );
			plog("total entries: " + lzip.getTotalEntries(null, fileBuffer) );


			// entry exists
			bool eres = lzip.entryExists(null, "dir1/dir2/test2.bmp", fileBuffer);
			plog("entry exists: " + eres.ToString());


			// extract an entry
			zres = lzip.extract_entry(null, "dir1/dir2/test2.bmp", ppath + "/test22B.bmp", fileBuffer, progress2);
			plog("extract entry: " + zres.ToString());
			plog("");


			// get the uncompressed size of a specific file in the zip archive
			plog("get entry size: " + lzip.getEntrySize(null, "dir1/dir2/test2.bmp", fileBuffer).ToString());
			plog("");


			// extract a file in a zip archive to a byte buffer (referenced buffer method)
			plog("entry2Buffer1: "+ lzip.entry2Buffer(null,"dir1/dir2/test2.bmp",ref reusableBuffer2, fileBuffer).ToString() );
		    //File.WriteAllBytes(ppath + "/out.bmp", reusableBuffer2);


			// extract a file in a zip archive to a byte buffer (new buffer method)
			var newBuffer = lzip.entry2Buffer(null, "dir1/dir2/test2.bmp", fileBuffer);
			zres = 0;
			if (newBuffer != null) zres = 1;
			plog("entry2Buffer2: "+ zres.ToString());
			// write a file out to confirm all was ok
			// File.WriteAllBytes(ppath + "/out.bmp", reusableBuffer2);
			plog("");

			
			// get file info of the zip file (names, uncompressed and compressed sizes)
			plog( "total bytes: " + lzip.getFileInfo(null, fileBuffer).ToString());

			// Look through the ninfo, uinfo and cinfo Lists where the file names and sizes are stored.
			if (lzip.ninfo != null) {
				for (int i = 0; i < lzip.ninfo.Count; i++) {
					log += lzip.ninfo[i] + " - " + lzip.uinfo[i] + " / " + lzip.cinfo[i] + "\n";
				}
			}
			plog("");

		#endif
	}

    // The merged/hidden zip file tests.
    void DoDecompression_Merged() {
        
        // get entry info of the merged zip. (Can be used also for regular zip archives to get extended info on the entries. After calling this iterate through the lzip.zinfo List.)
        plog("Get Info of merged zip: " + lzip.getZipInfo(ppath + "/merged.jpg").ToString());

        // if the zip has entries
        if(lzip.zinfo != null && lzip.zinfo.Count > 0) {
            for(int i=0; i < lzip.zinfo.Count; i++){
                plog("Entry no: " + (i+1).ToString() + "   " + lzip.zinfo[i].filename +"  uncompressed: " + lzip.zinfo[i].UncompressedSize.ToString() + "  compressed: " + lzip.zinfo[i].CompressedSize.ToString());
            }
        }
        plog("");

        // Extract the whole zip to disk
        int[] prog = new int[1];
        plog("Decompress to disk from merged file: " + lzip.decompressZipMerged(ppath + "/merged.jpg", ppath + "/mergedTests/",  prog).ToString());

        // extract an entry from the merged zip that resides in the file system and override the entry name.
        plog("Extract entry to disk from merged file: " + lzip.entry2FileMerged(ppath + "/merged.jpg", "dir1/dir2/dir3/Unity_1.jpg", ppath + "/mergedTests", "overriden.jpg"));

        plog("");
        // now let's say we have the above archive in a memory buffer.
        var tempBuffer = File.ReadAllBytes(ppath + "/merged.jpg");

        // get the zip entries from the merged archive in the buffer
        plog("Get Info of merged zip in Buffer: " + lzip.getZipInfoMerged(tempBuffer).ToString());

        if(lzip.zinfo != null && lzip.zinfo.Count > 0) {
            for(int i=0; i < lzip.zinfo.Count; i++){
                plog("Entry no: " + (i+1).ToString() + "   " + lzip.zinfo[i].filename +"  uncompressed: " + lzip.zinfo[i].UncompressedSize.ToString() + "  compressed: " + lzip.zinfo[i].CompressedSize.ToString());
            }
        }

        plog("");

        // decompress to disk from a merged file that resides in a buffer.
        plog("Decompress to disk from merged buffer: " + lzip.decompressZipMerged(tempBuffer, ppath + "/mergedTests/",  prog).ToString());

        // extract an entry from the merged zip in the buffer to disk.
        plog("Entry2File from merged buffer: " + lzip.entry2FileMerged( tempBuffer, "dir1/dir2/dir3/Unity_1.jpg", ppath + "/mergedTests").ToString());

        plog("");

        // extract an entry from a merged file in the file system to a new buffer.
        var newBuffer = lzip.entry2BufferMerged(ppath + "/merged.jpg", "dir1/dir2/dir3/Unity_1.jpg");
        plog("Size of entry in new buffer 1: " + newBuffer.Length);
        newBuffer = null;

        // extract an entry from a merged file in the file system to a fixed sized buffer.
        byte[] fixedSizedBuffer = new byte[11 * 1024];
        plog("Size of entry in fixed buffer 1: " + lzip.entry2FixedBufferMerged(ppath + "/merged.jpg", "dir1/dir2/dir3/Unity_1.jpg", ref fixedSizedBuffer).ToString());

        plog("");
        // extract an entry from a merged file in a buffer to a new buffer.
        var newBuffer2 = lzip.entry2BufferMerged(tempBuffer, "dir1/dir2/dir3/Unity_1.jpg");
        plog("Size of entry in new buffer 2: " + newBuffer2.Length);

        // extract an entry from a merged file in a buffer to a fixed sized buffer.
        plog("Size of entry in fixed buffer 2: " + lzip.entry2FixedBufferMerged(tempBuffer, "dir1/dir2/dir3/Unity_1.jpg", ref fixedSizedBuffer).ToString());

        fixedSizedBuffer = null;
        tempBuffer = null;
    }

    // ============================================================================================================================================================= 


    IEnumerator DownloadZipFile() {

        myFile = "testZip.zip";

        // make sure a previous zip file having the same name with the one we want to download does not exist in the ppath folder
        if (File.Exists(ppath + "/" + myFile)) { downloadDone = true; yield break; }//File.Delete(ppath + "/" + myFile);

        Debug.Log("starting download");

        // replace the link to the zip file with your own (although this will work also)
        using (UnityWebRequest www = UnityWebRequest.Get("https://dl.dropboxusercontent.com/s/xve34ldz3pqvmh1/" + myFile))
        {
            yield return www.SendWebRequest();

            if (www.error != null)
            {
                Debug.Log(www.error);
            } else {
                downloadDone = true;

                // write the downloaded zip file to the ppath directory so we can have access to it
                // depending on the Install Location you have set for your app, set the Write Access accordingly!
                File.WriteAllBytes(ppath + "/" + myFile, www.downloadHandler.data);

                Debug.Log("download done");
            }
        }
    }

#else
    void Start(){
        Debug.Log("Does not work on WebPlayer!");
    }
#endif

}

