using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class lzip {

#if !UNITY_WEBPLAYER || UNITY_EDITOR


#if (UNITY_IPHONE || UNITY_IOS || UNITY_TVOS || UNITY_WEBGL) && !UNITY_EDITOR

#if (UNITY_IPHONE || UNITY_IOS || UNITY_TVOS) && !UNITY_WEBGL
		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		private static extern int zsetPermissions(string filePath, string _user, string _group, string _other);

		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		private static extern int zipCD(int levelOfCompression, string zipArchive, string inFilePath, string fileName, string comment, [MarshalAs(UnmanagedType.LPStr)]  string password, bool useBz2, int diskSize);


        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		private static extern bool zipBuf2File(int levelOfCompression, string zipArchive, string arc_filename, IntPtr buffer, int bufferSize, string comment,[MarshalAs(UnmanagedType.LPStr)]  string password, bool useBz2);
		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		private static extern int zipDeleteFile(string zipArchive, string arc_filename, string tempArchive);

        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
        private static extern int zipCDList(int levelOfCompression, string zipArchive, IntPtr filename, int arrayLength, IntPtr prog, IntPtr filenameForced, [MarshalAs(UnmanagedType.LPStr)]  string password, bool useBz2, int diskSize);

#endif
#if (UNITY_IPHONE || UNITY_IOS || UNITY_TVOS || UNITY_WEBGL)
		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		private static extern void releaseBuffer(IntPtr buffer);
		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr zipCompressBuffer(IntPtr source, int sourceLen, int levelOfCompression, ref int v);
		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr zipDecompressBuffer(IntPtr source, int sourceLen, ref int v);

		//gzip section
		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int zipGzip(IntPtr source, int sourceLen, IntPtr outBuffer, int levelOfCompression, bool addHeader, bool addFooter);
		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int zipUnGzip(IntPtr source, int sourceLen, IntPtr outBuffer, int outLen, bool hasHeader, bool hasFooter);
		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		internal static extern int zipUnGzip2(IntPtr source, int sourceLen, IntPtr outBuffer, int outLen);
        //

        //inMemory section
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
        private static extern int freeMemStruct(IntPtr buffer);
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr zipCDMem(IntPtr info, IntPtr pnt, int levelOfCompression, IntPtr source, int sourceLen, string fileName, string comment, [MarshalAs(UnmanagedType.LPStr)]  string password, bool useBz2);
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr initMemStruct();
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr initFileStruct();
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
        private static extern int freeMemZ(IntPtr pointer);
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
        private static extern int freeFileZ(IntPtr pointer);
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr zipCDMemStart(IntPtr info, IntPtr pnt, IntPtr fileStruct, IntPtr memStruct);
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
        private static extern int zipCDMemAdd(IntPtr zf, int levelOfCompression, IntPtr source, int sourceLen, string fileName, string comment, [MarshalAs(UnmanagedType.LPStr)]  string password, bool useBz2);
        [DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr zipCDMemClose(IntPtr zf, IntPtr memStruct, IntPtr info, int err);
        //

		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		private static extern int zipEX(string zipArchive, string outPath, IntPtr progress, IntPtr FileBuffer, int fileBufferLength, IntPtr proc, [MarshalAs(UnmanagedType.LPStr)] string password);
		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		private static extern int zipEntry(string zipArchive, string arc_filename, string outpath, IntPtr FileBuffer,int fileBufferLength, IntPtr proc, [MarshalAs(UnmanagedType.LPStr)] string password);
		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		private static extern uint getEntryDateTime(string zipArchive, string arc_filename, IntPtr FileBuffer,int fileBufferLength);

		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		private static extern int zipGetTotalFiles(string zipArchive, IntPtr FileBuffer, int fileBufferLength);
		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		private static extern int zipGetTotalEntries(string zipArchive, IntPtr FileBuffer, int fileBufferLength);
		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		private static extern int zipGetInfoA(string zipArchive, IntPtr total, IntPtr FileBuffer, int fileBufferLength);
		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		private static extern IntPtr zipGetInfo(string zipArchive, int size, IntPtr unc, IntPtr comp, IntPtr FileBuffer, int fileBufferLength);


		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		private static extern bool zipEntryExists(string zipArchive, string entry, IntPtr FileBuffer, int fileBufferLength);

		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		private static extern bool zipValidateFile(string zip_Archive, IntPtr FileBuffer, int fileBufferLength);
		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		private static extern int zipGetEntrySize(string zipArchive, string entry, IntPtr FileBuffer, int fileBufferLength);
		[DllImport("__Internal", CallingConvention = CallingConvention.Cdecl)]
		private static extern int zipEntry2Buffer(string zipArchive, string entry, IntPtr buffer, int bufferSize, IntPtr FileBuffer, int fileBufferLength, [MarshalAs(UnmanagedType.LPStr)] string password);
#endif
#endif

#if UNITY_5_4_OR_NEWER
#if (UNITY_ANDROID || UNITY_STANDALONE_LINUX || UNITY_WEBGL) && !UNITY_EDITOR || UNITY_EDITOR_LINUX
		private const string libname = "zipw";
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
    private const string libname = "libzipw";
#endif
#else
#if (UNITY_ANDROID || UNITY_STANDALONE_LINUX || UNITY_WEBGL) && !UNITY_EDITOR
		private const string libname = "zipw";
#endif
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX
    private const string libname = "libzipw";
#endif
#endif

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_ANDROID || UNITY_STANDALONE_LINUX
#if (!UNITY_WEBGL || UNITY_EDITOR)

#if (UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX || UNITY_ANDROID || UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX) && !UNITY_EDITOR_WIN
		[DllImport(libname, EntryPoint = "zsetPermissions"
#if (UNITY_ANDROID && !UNITY_EDITOR)
		, CallingConvention = CallingConvention.Cdecl
#endif
		)]
		internal static extern int zsetPermissions(string filePath, string _user, string _group, string _other);
#endif

	//Windows only function to set encoding of reading/writing files
	//CP_ACP = 0
	//CP_OEMCP/UNICODE = 1
	//CP_UTF8 = 65001
	//CP_WINUNICODE = 1200

	//Default is CP_UTF8 = 65001
    #if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
		[DllImport(libname, EntryPoint = "setEncoding"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    #if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
		,  CharSet = CharSet.Auto
    #endif
		)]
		public static extern bool setEncoding(uint encoding);
    #endif

    [DllImport(libname, EntryPoint = "zipValidateFile"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    #if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
    , CharSet = CharSet.Auto
    #endif
	)]
    internal static extern bool zipValidateFile(
        #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
        #endif
        string zipArchive, IntPtr FileBuffer,int fileBufferLength);

    [DllImport(libname, EntryPoint = "zipGetTotalFiles"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    #if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
    , CharSet = CharSet.Auto
    #endif
	)]
    internal static extern int zipGetTotalFiles(
        #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
        #endif
        string zipArchive, IntPtr FileBuffer, int fileBufferLength);


	[DllImport(libname, EntryPoint = "zipGetTotalEntries"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    #if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
    , CharSet = CharSet.Auto
    #endif
	)]
    internal static extern int zipGetTotalEntries(
        #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
        #endif
         string zipArchive, IntPtr FileBuffer, int fileBufferLength);


    [DllImport(libname, EntryPoint = "zipGetInfoA"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    #if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
    , CharSet = CharSet.Auto
    #endif
	)]
	internal static extern int zipGetInfoA(
        #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
        #endif
        string zipArchive,  IntPtr total, IntPtr FileBuffer, int fileBufferLength);




	[DllImport(libname, EntryPoint = "zipGetInfo"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    #if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
        , CharSet = CharSet.Auto
    #endif
	)]
	internal static extern IntPtr zipGetInfo(
        #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
        #endif
         string zipArchive, int size, IntPtr unc, IntPtr comp, IntPtr FileBuffer, int fileBufferLength);


    [DllImport(libname, EntryPoint = "releaseBuffer"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    )]
    internal static extern void releaseBuffer(IntPtr buffer);

    [DllImport(libname, EntryPoint = "zipGetEntrySize"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    #if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
    , CharSet = CharSet.Auto
    #endif
	)]
    internal static extern int zipGetEntrySize(
        #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
        #endif
        string zipArchive,
        #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
        #endif
        string entry, IntPtr FileBuffer, int fileBufferLength);

    [DllImport(libname, EntryPoint = "zipEntryExists"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    #if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
    , CharSet = CharSet.Auto
    #endif
	)]
    internal static extern bool zipEntryExists(
        #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
        #endif
        string zipArchive,
        #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
        #endif
        string entry, IntPtr FileBuffer, int fileBufferLength);

    [DllImport(libname, EntryPoint = "zipCD"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    #if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
    , CharSet = CharSet.Auto
    #endif
	)]
    internal static extern int zipCD(
        int levelOfCompression,
        #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
        #endif
        string zipArchive,
        #if (UNITY_STANDALONE_WIN)
                [MarshalAs(UnmanagedType.LPWStr)]
        # endif
        string inFilePath,
        #if (UNITY_STANDALONE_WIN)
                [MarshalAs(UnmanagedType.LPWStr)]
        #endif
        string fileName,
        #if (UNITY_STANDALONE_WIN)
                [MarshalAs(UnmanagedType.LPWStr)]
        #endif
        string comment, [MarshalAs(UnmanagedType.LPStr)]  string password, bool useBz2, int diskSize
    );

    [DllImport(libname, EntryPoint = "zipCDList"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    #if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
    , CharSet = CharSet.Auto
    #endif
	)]
    internal static extern int zipCDList(
        int levelOfCompression,
        #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
        #endif
        string zipArchive, IntPtr filename, int arrayLength, IntPtr prog, IntPtr filenameForced, [MarshalAs(UnmanagedType.LPStr)]  string password, bool useBz2, int diskSize
    );

    [DllImport(libname, EntryPoint = "zipBuf2File"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    #if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
    , CharSet = CharSet.Auto
    #endif
	)]
    internal static extern bool zipBuf2File(int levelOfCompression,
    #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
    #endif
        string zipArchive,
    #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
    # endif
        string arc_filename, IntPtr buffer, int bufferSize,
    #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
    #endif
        string comment,[MarshalAs(UnmanagedType.LPStr)]  string password, bool useBz2
	);

    [DllImport(libname, EntryPoint = "zipDeleteFile"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    #if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
    , CharSet = CharSet.Auto
    #endif
	)]
    internal static extern int zipDeleteFile(
    #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
    #endif
        string zipArchive,
    #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
    #endif
        string arc_filename,
    #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
    #endif
        string tempArchive);


    [DllImport(libname, EntryPoint = "zipEntry2Buffer"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    #if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
    , CharSet = CharSet.Auto
    #endif
	)]
    internal static extern int zipEntry2Buffer(
    #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
    #endif
        string zipArchive,
    #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
    #endif
        string entry, IntPtr buffer, int bufferSize, IntPtr FileBuffer, int fileBufferLength, [MarshalAs(UnmanagedType.LPStr)] string password);

    [DllImport(libname, EntryPoint = "zipCompressBuffer"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    )]
    internal static extern IntPtr zipCompressBuffer(IntPtr source, int sourceLen, int levelOfCompression, ref int v);

    [DllImport(libname, EntryPoint = "zipDecompressBuffer"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    )]
    internal static extern IntPtr zipDecompressBuffer(IntPtr source, int sourceLen, ref int v);

    [DllImport(libname, EntryPoint = "zipEX"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    #if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
    , CharSet = CharSet.Auto
    #endif
	)]
	internal static extern int zipEX(
    #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
    #endif
        string zipArchive,
    #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
    #endif
        string outPath, IntPtr progress, IntPtr FileBuffer, int fileBufferLength, IntPtr proc, [MarshalAs(UnmanagedType.LPStr)] string password);


    [DllImport(libname, EntryPoint = "zipEntry"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    #if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
    , CharSet = CharSet.Auto
    #endif
	)]
    internal static extern int zipEntry(
    #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
    #endif
        string zipArchive,
    #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
    #endif
        string arc_filename,
    #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
    #endif
        string outpath, IntPtr FileBuffer,int fileBufferLength, IntPtr proc, [MarshalAs(UnmanagedType.LPStr)] string password);


    [DllImport(libname, EntryPoint = "getEntryDateTime"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    #if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
    , CharSet = CharSet.Auto
    #endif
	)]
    internal static extern uint getEntryDateTime(
    #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
    #endif
        string zipArchive,
    #if (UNITY_STANDALONE_WIN)
        [MarshalAs(UnmanagedType.LPWStr)]
    #endif
        string arc_filename, IntPtr FileBuffer,int fileBufferLength);

    //inMemory section
    [DllImport(libname, EntryPoint = "freeMemStruct"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
        , CallingConvention = CallingConvention.Cdecl
    #endif
    )]
    internal static extern int freeMemStruct(IntPtr buffer);

    [DllImport(libname, EntryPoint = "zipCDMem"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
        , CallingConvention = CallingConvention.Cdecl
    #endif
    #if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
    , CharSet = CharSet.Auto
    #endif
    )]
    internal static extern IntPtr zipCDMem(IntPtr info, IntPtr pnt, int levelOfCompression, IntPtr source, int sourceLen,
    #if (UNITY_STANDALONE_WIN)
                [MarshalAs(UnmanagedType.LPWStr)]
    #endif
        string fileName,
    #if (UNITY_STANDALONE_WIN)
                [MarshalAs(UnmanagedType.LPWStr)]
    #endif
        string comment, [MarshalAs(UnmanagedType.LPStr)]  string password, bool useBz2
    );


    [DllImport(libname, EntryPoint = "initMemStruct"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
        , CallingConvention = CallingConvention.Cdecl
    #endif
    )]
    internal static extern IntPtr initMemStruct();

    [DllImport(libname, EntryPoint = "initFileStruct"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
        , CallingConvention = CallingConvention.Cdecl
    #endif
    )]
    internal static extern IntPtr initFileStruct();


    [DllImport(libname, EntryPoint = "freeMemZ"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
        , CallingConvention = CallingConvention.Cdecl
    #endif
    )]
    internal static extern int freeMemZ(IntPtr pointer);

    [DllImport(libname, EntryPoint = "freeFileZ"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
        , CallingConvention = CallingConvention.Cdecl
    #endif
    )]
    internal static extern int freeFileZ(IntPtr pointer);


    [DllImport(libname, EntryPoint = "zipCDMemStart"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
        , CallingConvention = CallingConvention.Cdecl
    #endif
    )]
    internal static extern IntPtr zipCDMemStart(IntPtr info, IntPtr pnt, IntPtr fileStruct, IntPtr memStruct);


    [DllImport(libname, EntryPoint = "zipCDMemAdd"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
        , CallingConvention = CallingConvention.Cdecl
    #endif
    #if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN)
    , CharSet = CharSet.Auto
    #endif
    )]
    internal static extern int zipCDMemAdd(IntPtr zf, int levelOfCompression, IntPtr source, int sourceLen,
    #if (UNITY_STANDALONE_WIN)
                [MarshalAs(UnmanagedType.LPWStr)]
    #endif
        string fileName,
    #if (UNITY_STANDALONE_WIN)
                [MarshalAs(UnmanagedType.LPWStr)]
    #endif
        string comment, [MarshalAs(UnmanagedType.LPStr)]  string password, bool useBz2
    );

    [DllImport(libname, EntryPoint = "zipCDMemClose"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
        , CallingConvention = CallingConvention.Cdecl
    #endif
    )]
    internal static extern IntPtr zipCDMemClose(IntPtr zf, IntPtr memStruct, IntPtr info, int err);



    //gzip section
    [DllImport(libname, EntryPoint = "zipGzip"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    )]
    internal static extern int zipGzip(IntPtr source, int sourceLen, IntPtr outBuffer, int levelOfCompression, bool addHeader, bool addFooter);

    #endif

    [DllImport(libname, EntryPoint = "zipUnGzip"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    )]

    internal static extern int zipUnGzip(IntPtr source, int sourceLen, IntPtr outBuffer,int outLen, bool hasHeader, bool hasFooter);
    [DllImport(libname, EntryPoint = "zipUnGzip2"
    #if (UNITY_STANDALONE_WIN && ENABLE_IL2CPP) || UNITY_ANDROID
		, CallingConvention = CallingConvention.Cdecl
    #endif
    )]
    internal static extern int zipUnGzip2(IntPtr source, int sourceLen, IntPtr outBuffer, int outLen);

#endif




#if !UNITY_WEBGL || UNITY_EDITOR
    // set permissions of a file in user, group, other.
    // Each string should contain any or all chars of "rwx".
    // returns 0 on success
    //
    public static int setFilePermissions(string filePath, string _user, string _group, string _other){
		#if (UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX || UNITY_ANDROID || UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX || UNITY_IOS || UNITY_TVOS || UNITY_IPHONE) && !UNITY_EDITOR_WIN
			return zsetPermissions(filePath, _user, _group, _other);
		#else
			return -1;
		#endif
	}
#endif



    // A function that returns the total number of files in a zip archive (files only, no folders).
    //
    // zipArchive       : the zip to be checked
	// FileBuffer		: A buffer that holds a zip file. When assigned the function will read from this buffer and will ignore the filePath.
    //
    // ERROR CODES
    //                  : -1 = failed to access zip archive
    //                  :  any number>0 = the number of files in the zip archive
    //
    public static int getTotalFiles(string zipArchive, byte[] FileBuffer = null) {
		int res = 0;
		
		if(FileBuffer!= null) {
			GCHandle fbuf = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
			res = zipGetTotalFiles(null, fbuf.AddrOfPinnedObject(), FileBuffer.Length);

			fbuf.Free();
			return res;
		}

        #if !UNITY_WEBGL || UNITY_EDITOR
        res = zipGetTotalFiles(@zipArchive, IntPtr.Zero, 0);
        #endif

		return res;
    }

	// A function that will return the total entries in a zip arcive. (files + folders)
	//
    // zipArchive       : the zip to be checked
	// FileBuffer		: A buffer that holds a zip file. When assigned the function will read from this buffer and will ignore the filePath.
    //
    // ERROR CODES
    //                  : -2 = failed to access zip archive
    //                  :  any number>0 = the number of entries in the zip archive
    //
    public static int getTotalEntries(string zipArchive, byte[] FileBuffer = null) {
		int res = 0;
		
		if(FileBuffer!= null) {
			GCHandle fbuf = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
			res = zipGetTotalEntries(null, fbuf.AddrOfPinnedObject(), FileBuffer.Length);

			fbuf.Free();
			return res;
		}

        #if !UNITY_WEBGL || UNITY_EDITOR
        res = zipGetTotalEntries(@zipArchive, IntPtr.Zero, 0);
        #endif

		return res;
    }

    
    // Lists get filled with filenames (including path if the file is in a folder) and uncompressed file sizes
    // Call getFileInfo(string zipArchive, string path) to get them filled. After that you can iterate through them to get the info you want.
    public static List<string> ninfo = new List<string>();//filenames
    public static List<long> uinfo = new List<long>();//uncompressed file sizes
    public static List<long> cinfo = new List<long>();//compressed file sizes
    public static int zipFiles, zipFolders;// global integers that store the number of files and folders in a zip file.

    // This function fills the Lists with the filenames and file sizes that are in the zip file
    // Returns			: the total size of uncompressed bytes of the files in the zip archive 
    //
    // zipArchive		: the full path to the archive, including the archives name. (/myPath/myArchive.zip)
	// FileBuffer		: A buffer that holds a zip file. When assigned the function will read from this buffer and will ignore the filePath.
    //
    // ERROR CODES      : -1 = Input file not found
    //                  : -2 = Could not get info
    //
    public static long getFileInfo(string zipArchive,  byte[] FileBuffer = null) {
        ninfo.Clear(); uinfo.Clear(); cinfo.Clear();
        zipFiles = 0; zipFolders = 0;

		int res = 0;

		int[] tt = new int[1];
		GCHandle tb = GCHandle.Alloc(tt, GCHandleType.Pinned);

		if(FileBuffer!= null) {
			GCHandle fbuf = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
			res = zipGetInfoA(null,tb.AddrOfPinnedObject(), fbuf.AddrOfPinnedObject(), FileBuffer.Length);
			fbuf.Free();
		} else {
            #if !UNITY_WEBGL || UNITY_EDITOR
			res = zipGetInfoA(@zipArchive, tb.AddrOfPinnedObject(),IntPtr.Zero, 0);
            #endif
		}

		tb.Free();

        if (res <=0) {  return -1; }

		IntPtr uni = IntPtr.Zero;

		uint[] unc = new uint[tt[0]];
		uint[] comp = new uint[tt[0]];

		GCHandle uncb = GCHandle.Alloc(unc, GCHandleType.Pinned);
		GCHandle compb = GCHandle.Alloc(comp, GCHandleType.Pinned);

		if(FileBuffer!= null) {
			GCHandle fbuf = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
			uni = zipGetInfo(null, res, uncb.AddrOfPinnedObject(), compb.AddrOfPinnedObject(), fbuf.AddrOfPinnedObject() ,FileBuffer.Length);
			fbuf.Free();
		} else {
            #if !UNITY_WEBGL || UNITY_EDITOR
			uni = zipGetInfo(@zipArchive, res, uncb.AddrOfPinnedObject(), compb.AddrOfPinnedObject(),IntPtr.Zero, 0);
            #endif
		}

		if(uni == IntPtr.Zero) { uncb.Free(); compb.Free(); return -2; }

		string str = Marshal.PtrToStringAuto ( uni );
		StringReader r = new StringReader(str);

        string line;
		long sum = 0;

		for(int i=0; i<tt[0]; i++) {
			if( ( line = r.ReadLine() ) != null)  ninfo.Add(line); 
			if(unc != null) {
				uinfo.Add((long)unc[i]);
				sum += unc[i];
				if (unc[i] > 0) zipFiles++; else zipFolders++;
			}
			if(comp != null) cinfo.Add((long)comp[i]);
		}

        r.Close(); 
        r.Dispose();
		uncb.Free(); compb.Free();
		releaseBuffer(uni);
		tt = null;
		unc = null;
		comp = null;

        return sum;
    }




    // A function that returns the uncompressed size of a file in a zip archive.
    //
    // zipArchive    : the zip archive to get the info from.
    // entry         : the entry for which we want to know it uncompressed size.
	// FileBuffer	 : A buffer that holds a zip file. When assigned the function will read from this buffer and will ignore the filePath. 
    //
    public static int getEntrySize(string zipArchive, string entry, byte[] FileBuffer = null) {
		int res = 0;
		if(FileBuffer!= null) {
			GCHandle fbuf = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
			res = zipGetEntrySize(null, entry, fbuf.AddrOfPinnedObject(), FileBuffer.Length);

			fbuf.Free();
			return res;
		}
        #if !UNITY_WEBGL || UNITY_EDITOR
        res = zipGetEntrySize(@zipArchive, entry, IntPtr.Zero, 0);
        #endif
		return res;
    }


	// A function that tells if an entry in zip archive exists.
	//
	// Returns true or false.
	//
    // zipArchive    : the zip archive to get the info from.
    // entry         : the entry for which we want to know if it exists.
	// FileBuffer	 : A buffer that holds a zip file. When assigned the function will read from this buffer and will ignore the filePath.
	//
    public static bool entryExists(string zipArchive, string entry, byte[] FileBuffer = null) {
		bool res = false;
		if(FileBuffer!= null) {
			GCHandle fbuf = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
			res = zipEntryExists(null, entry, fbuf.AddrOfPinnedObject(), FileBuffer.Length);

			fbuf.Free();
			return res;
		}
        #if !UNITY_WEBGL || UNITY_EDITOR
        res = zipEntryExists(@zipArchive, entry, IntPtr.Zero, 0);
        #endif
		return res;
    }



#if !UNITY_WEBGL || UNITY_EDITOR

    // A function that compresses a byte buffer and writes it to a zip file. I you set the append flag to true, the output will get appended to an existing zip archive.
    //
    // levelOfCompression   : (0-9) recommended 9 for maximum.
    // zipArchive           : the full path to the zip archive to be created or append to.
    // arc_filename         : the name of the file that will be written to the archive.
    // buffer               : the buffer that will be compressed and will be put in the zip archive.
    // append               : set to true if you want the output to be appended to an existing zip archive.
	// comment				: an optional comment for this entry.
	// password				: an optional password to encrypt this entry.
	// useBz2				: set to true if you want bz2 compression instead of zlib. (not available for MacOS/iOS/tvOS)
    //
    // ERROR CODES          : true  = success
    //                      : false = failed
    //
    public static bool buffer2File(int levelOfCompression, string zipArchive, string arc_filename, byte[] buffer, bool append=false, string comment=null, string password=null, bool useBz2 = false
	) {
        if (!append) { if (File.Exists(@zipArchive)) File.Delete(@zipArchive); }
        GCHandle sbuf = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        if (levelOfCompression < 0) levelOfCompression = 0; if (levelOfCompression > 9) levelOfCompression = 9;
        if(password == "") password = null;
		if(comment == "") comment = null;
        bool res = zipBuf2File(levelOfCompression, @zipArchive, arc_filename, sbuf.AddrOfPinnedObject(), buffer.Length, comment, password, useBz2);
        sbuf.Free();
        return res;
    }


	// A function that deletes a file in a zip archive. It creates a temp file where the compressed data of the old archive is copied except the one that needs to be deleted.
	// After that the old zip archive is deleted and the temp file gets renamed to the original zip archive.
	// You can delete directories too if they are empty.
	//
	// zipArchive           : the full path to the zip archive
	// arc_filename         : the name of the file that will be deleted.
	//
	// ERROR CODES			:  1 = success
	//						: -1 = failed to open zip
	//						: -2 = failed to locate the archive to be deleted in the zip file
	//						: -3 = error copying compressed data from original zip
	//						: -4 = failed to create temp zip file.
	//
	public static int delete_entry(string zipArchive, string arc_filename) {
		string tmp = @zipArchive+".tmp";
		int res = zipDeleteFile(@zipArchive, @arc_filename, tmp);

		if(res>0) {
			File.Delete(@zipArchive);
			File.Move(tmp, @zipArchive);
		}else {
			if(File.Exists(tmp)) File.Delete(tmp);
		}

		return res;
	}


	// A function that replaces an entry in a zip archive with a file that lies in a path. The original name of the archive will be used.
	//
	// zipArchive           : the full path to the zip archive
	// arc_filename         : the name of the file that will be replaced.
	// newFilePath			: a path to the file that will replace the original entry.
	// level:				: the level of compression of the new entry.
    // comment				: add a comment for the file in the zip file header.
	// password				: set the password to protect this file. (only ascii characters)
	// useBz2				: use the bz2 compression algorithm. If false the zlib deflate algorithm will be used. (not available for MacOS/iOS/tvOS)
	//
    // ERROR CODES
    //						:  -1 = could not create or append
    //						:  -2 = error during operation
    //						:  -3 = failed to delete original entry
	//

	public static int replace_entry(string zipArchive, string arc_filename, string newFilePath, int level = 9, string comment=null, string password = null, bool useBz2=false) {
		int res = delete_entry(@zipArchive, @arc_filename);
		if(res<0) return -3;
		if(password == "") password = null;
		if(comment == "") comment = null;
		return zipCD(level, @zipArchive, newFilePath, @arc_filename, comment, password, useBz2, 0);
	}

	// A function that replaces an entry in a zip archive with a buffer. The original name of the archive will be used.
	//
	// zipArchive           : the full path to the zip archive
	// arc_filename         : the name of the file that will be replaced.
	// newFileBuffer		: a byte buffer that will replace the original entry.
	// level:				: the level of compression of the new entry.
	// password				: set the password to protect this file. (only ascii characters)
	// useBz2				: use the bz2 compression algorithm. If false the zlib deflate algorithm will be used. (not available for MacOS/iOS/tvOS)
	//
    // ERROR CODES
	//                    :  1 = success
	//					  : -5 = failed to delete the original file
	//					  : -6 = failed to append the buffer to the zip

	public static int replace_entry(string zipArchive, string arc_filename, byte[] newFileBuffer, int level = 9, string password = null, bool useBz2 = false) {
		int res = delete_entry(@zipArchive, @arc_filename);
		if(res<0) return -5;

		if( buffer2File(level, @zipArchive, @arc_filename, newFileBuffer, true, null, password, useBz2) ) return 1; else return - 6;
	}


    // A function that will extract only the specified file that resides in a zip archive.
    //
    // zipArchive       : the full path to the zip archive from which we want to extract the specific file.
    // arc_filename     : the specific file we want to extract. (If the file resides in a  directory, the directory path should be included. like dir1/dir2/myfile.bin)
	//					:  --> on some zip files the internal dir structure uses \\ instead of / characters for directories separators. In that case use the appropriate
	//					:  --> chars that will allow the file to be extracted.
    // outpath          : the full path to where the file should be extracted + the desired name for it.
	// FileBuffer		: A buffer that holds a zip file. When assigned the function will read from this buffer and will ignore the filePath. 
	// proc:			: a single item ulong array that gets updated with the progress of the decompression in bytes.
	//					  (100% is reached when the compressed size of the file is reached.)
	// password			: if needed, the password to decrypt the entry.
    //
    // ERROR CODES      : -1 = extraction failed
    //                  : -2 = could not initialize zip archive.
	//					: -3 = could not locate entry
	//					: -4 = could not get entry info
	//					: -5 = password error
    //                  :  1 = success
    //
    public static int extract_entry(string zipArchive, string arc_filename, string outpath, byte[] FileBuffer = null, ulong[] proc = null, string password = null) {

		if(!Directory.Exists( Path.GetDirectoryName(outpath) ) ) return -1;

		int res=-1;
		if(proc == null) proc = new ulong[1];
		GCHandle pbuf = GCHandle.Alloc(proc, GCHandleType.Pinned);

			#if (UNITY_IPHONE || UNITY_IOS || UNITY_TVOS || UNITY_STANDALONE_OSX || UNITY_ANDROID || UNITY_STANDALONE_LINUX || UNITY_EDITOR || UNITY_STANDALONE_WIN)
			if(FileBuffer!= null) {
				GCHandle fbuf = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);

				if(proc != null) res = zipEntry(null, arc_filename, @outpath, fbuf.AddrOfPinnedObject(), FileBuffer.Length, pbuf.AddrOfPinnedObject(), password);
				else res = zipEntry(null, arc_filename, @outpath, fbuf.AddrOfPinnedObject(), FileBuffer.Length, IntPtr.Zero, password);

				fbuf.Free();
				pbuf.Free();
				return res;
			}
			#endif

		if(proc != null) res = zipEntry(@zipArchive, arc_filename, @outpath, IntPtr.Zero, 0, pbuf.AddrOfPinnedObject(), password);
		else res = zipEntry(@zipArchive, arc_filename, @outpath, IntPtr.Zero, 0, IntPtr.Zero, password);
		pbuf.Free();
		return res;
			
    }



    // A function that decompresses a zip file. If the zip contains directories, they will be created.
    //
    // zipArchive       : the full path to the zip archive that will be decompressed.
    // outPath          : the directory in which the zip contents will be extracted.
    // progress         : provide a single item integer array to write the current index of the file getting extracted. To use it in realtime, call
    //                  : this function in a separate thread.
	// FileBuffer		: A buffer that holds a zip file. When assigned the function will read from this buffer and will ignore the filePath. 
	// proc:			: a single item ulong array that gets updated with the progress of the decompression in bytes.
	//					  (100% is reached when the compressed size of the file is reached.)
	// password			: if needed, the password to decrypt the archive.
    //
    // ERROR CODES
    //                  : -1 = could not initialize zip archive.
    //                  : -2 = failed extraction
    //                  :  1 = success
    //
    public static int decompress_File(string zipArchive, string outPath, int[] progress,  byte[] FileBuffer = null, ulong[] proc = null, string password = null) {
        //make a check if the last '/' exists at the end of the exctractionPath and add it if it is missing
        if (outPath.Substring(outPath.Length - 1, 1) != "/") { outPath += "/"; }
		int res;
		GCHandle ibuf = GCHandle.Alloc(progress, GCHandleType.Pinned);
		if(proc == null) proc = new ulong[1];
		GCHandle pbuf = GCHandle.Alloc(proc, GCHandleType.Pinned);
		
			#if (UNITY_IPHONE || UNITY_IOS || UNITY_TVOS || UNITY_STANDALONE_OSX || UNITY_ANDROID || UNITY_STANDALONE_LINUX || UNITY_EDITOR || UNITY_STANDALONE_WIN)
			if(FileBuffer!= null) {
				GCHandle fbuf = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);

				if(proc != null) res = zipEX(null, @outPath, ibuf.AddrOfPinnedObject(), fbuf.AddrOfPinnedObject(), FileBuffer.Length, pbuf.AddrOfPinnedObject(), password);
				else res = zipEX(null, @outPath, ibuf.AddrOfPinnedObject(), fbuf.AddrOfPinnedObject(), FileBuffer.Length, IntPtr.Zero, password);

				fbuf.Free(); ibuf.Free(); pbuf.Free(); return res;
			}
			#endif

		if(proc != null) res = zipEX(@zipArchive, @outPath, ibuf.AddrOfPinnedObject(), IntPtr.Zero, 0, pbuf.AddrOfPinnedObject(), password);
		else res = zipEX(@zipArchive, @outPath, ibuf.AddrOfPinnedObject(), IntPtr.Zero, 0, IntPtr.Zero, password);
		ibuf.Free(); pbuf.Free();
		return res;
    }



    // A function that compresses a file to a zip file. If the flag append is set to true then it will get appended to an existing zip file.
    //
    // levelOfCompression : (0-9) recommended 9 for maximum (10 is highest but slower and not zlib compatible)
    // zipArchive         : the full path to the zip archive that will be created
    // inFilePath         : the full path to the file that should be compressed and added to the zip file.
    // append             : set to true if you want the input file to get appended to an existing zip file. (if the zip file does not exist it will be created.)
    // filename           : if you want the name of your file to be different then the one it has, set it here. If you add a folder structure to it,
    //                      like (dir1/dir2/myfile.bin) the directories will be created in the zip file.
    // comment            : add a comment for the file in the zip file header.
    // password			  : set the password to protect this file. (only ascii characters)
    // useBz2			  : use the bz2 compression algorithm. If false the zlib deflate algorithm will be used. (not available for MacOS/iOS/tvOS)
    // disksize           : if a disksize is used > 0 then the zip archive will be split to the assigned disksize (in bytes).
    //
    // ERROR CODES
    //					  :   1 = success
    //                    :  -1 = could not create or append
    //                    :  -2 = error during operation
    //
    public static int compress_File(int levelOfCompression, string zipArchive, string inFilePath, bool append = false, string fileName = "", string comment = null, string password = null, bool useBz2 = false, int diskSize = 0) {
        if (!File.Exists(@inFilePath)) return -10;
        if (!append) { if (File.Exists(@zipArchive)) File.Delete(@zipArchive); }

        if (fileName == null || fileName == "") fileName = Path.GetFileName(@inFilePath);
        if (levelOfCompression < 0) levelOfCompression = 0; if (levelOfCompression > 9) levelOfCompression = 9;
        if (password == "") password = null;
        if (comment == "") comment = null;

        int res = zipCD(levelOfCompression, @zipArchive, @inFilePath, fileName, comment, password, useBz2, diskSize);

        return res;
    }

    // A function that compresses a list of files to a zip file. Use this function to compress multiple files fast instead of appending to existing files with the compress_File function.
    //
    // levelOfCompression : (0-9) recommended 9 for maximum (10 is highest but slower and not zlib compatible)
    // zipArchive         : the full path to the zip archive that will be created
    // inFilePath[]       : an array of the full paths to the files that should be compressed and added to the zip file.
    // progress			  : this var will increment until the number of the input files and this are equal.
    // append             : set to true if you want the input files to get appended to an existing zip file. (if the zip file does not exist it will be created.)
    // filename[]         : if you want the names of your files to be different then the one they have, set it here. If you add a folder structure to it,
    //                      like (dir1/dir2/myfile.bin) the directories will be created in the zip file.
    // password			  : set the password to protect this file. (only ascii characters)
    // useBz2			  : use the bz2 compression algorithm. If false the zlib deflate algorithm will be used. (not available for MacOS/iOS/tvOS)
    // diskSize           : if a disksize is used > 0 then the zip archive will be split to the assigned disksize (in bytes).
    //
    // ERROR CODES
    //					  :   1 = success
    //                    :  -1 = could not create or append
    //                    :  -2 = error during operation
    //
    public static int compress_File_List(int levelOfCompression, string zipArchive, string[] inFilePath, int[] progress = null, bool append = false, string[] fileName = null, string password = null, bool useBz2 = false, int diskSize = 0) {

		if (inFilePath == null) return -3;

		if(fileName !=null && fileName.Length != inFilePath.Length) return -4;

		for(int i=0; i<inFilePath.Length; i++) {
			if (!File.Exists(inFilePath[i])) return -10;
		}

        if (!append) { if (File.Exists(@zipArchive)) File.Delete(@zipArchive); }

        if (levelOfCompression < 0) levelOfCompression = 0; if (levelOfCompression > 9) levelOfCompression = 9;
        if(password == "") password = null;

		int res = 0;

		string[] fna = null;
		string path = Path.GetDirectoryName(@zipArchive);

		if(fileName == null) {
			fna = new string[inFilePath.Length];
			for(int i = 0; i < inFilePath.Length; i++) { fna[i] = inFilePath[i].Replace(path, "");  }
		} else {
			fna = fileName;
		}

		for(int i = 0; i < inFilePath.Length; i++) { if(fna[i] == null) fna[i] = inFilePath[i].Replace(path, "");   }

        IntPtr[] fp = new IntPtr[inFilePath.Length];
        IntPtr[] np = new IntPtr[inFilePath.Length];

		for(int i = 0; i < inFilePath.Length; i++) {
			inFilePath[i] = inFilePath[i].Replace("\\", "/");
            fna[i] = fna[i].Replace("\\", "/");

            fp[i] = Marshal.StringToCoTaskMemAuto(inFilePath[i]);
            np[i] = Marshal.StringToCoTaskMemAuto(fna[i]);
		}


        GCHandle faBuf = GCHandle.Alloc(fp, GCHandleType.Pinned);
        GCHandle naBuf = GCHandle.Alloc(np, GCHandleType.Pinned);
        GCHandle prog = GCHandle.Alloc(progress, GCHandleType.Pinned);
        
        res = zipCDList(levelOfCompression, @zipArchive, faBuf.AddrOfPinnedObject(), inFilePath.Length, prog.AddrOfPinnedObject(), naBuf.AddrOfPinnedObject(), password, useBz2, diskSize);

        for (int i = 0; i < inFilePath.Length; i++) {
            Marshal.FreeCoTaskMem(fp[i]);
            Marshal.FreeCoTaskMem(np[i]);
        }

        faBuf.Free(); fp = null;
        naBuf.Free(); np = null;
        
        prog.Free();
		path = null;

		return res;
    }


    // Compress a directory with all its files and subfolders to a zip file. This function is way faster when adding manually multiple files to a zip with the compress_File function.
    //
    // sourceDir           : the directory you want to compress
    // levelOfCompression  : the level of compression (0-9).
    // zipArchive          : the full path+name to the zip file to be created .
    // includeRoot         : set to true if you want the root folder of the directory to be included in the zip archive. Otherwise leave it to false.
    // progress            : provide a single item integer array to write the current index of the files getting compressed.
    // password			   : set the password to protect this file. (only ascii characters)
    // useBz2			   : use the bz2 compression algorithm. If false the zlib deflate algorithm will be used. (not available for MacOS/iOS/tvOS)
    // disksize            : if a disksize is used > 0 then the zip archive will be split to the assigned disksize (in bytes).
    // append              : set to true if you want the input files to get appended to an existing zip file. (if the zip file does not exist it will be created.)
    //
    // If you want to get the progress of compression, call the getAllFiles function to get the total number of files
    // in a directory and its subdirectories. The compressDir when called from a separate thread will update the progress[0] provided parameter.
    // Divide this with the total number of files (as floats) and you have the % of the procedure.
    //
    public static int compressDir(string sourceDir, int levelOfCompression, string zipArchive, bool includeRoot = false, int[] progress = null, string password = null, bool useBz2 = false, int diskSize = 0, bool append = false) {
		string fdir = @sourceDir.Replace("\\", "/");

        if(sourceDir.Substring(sourceDir.Length - 1) != "/") fdir += "/";

        int totalFiles = getAllFiles(fdir);

        if (totalFiles == 0) return 0;

        IntPtr[] fp = new IntPtr[totalFiles];
        IntPtr[] np = new IntPtr[totalFiles];

		if(Directory.Exists(fdir)) {

			if (!append) { if (File.Exists(@zipArchive)) File.Delete(@zipArchive); }
			string[] ss = fdir.Split('/');
			string rdir = ss[ss.Length - 1];
			string root = rdir;

            if(ss.Length>1 && includeRoot) root = rdir = ss[ss.Length - 2] + "/";

			if (levelOfCompression < 0) levelOfCompression = 0; if (levelOfCompression > 9) levelOfCompression = 9;

            int l = 0;

			foreach (string f in Directory.GetFiles(fdir, "*", SearchOption.AllDirectories)) {
				string s = f.Replace(fdir, rdir).Replace("\\", "/").Replace("//", "/");

				if (!includeRoot) {
                    s = s.Substring(root.Length);
                    if(s.Substring(0, 1) == "/") s = s.Substring(1, s.Length-1);
                }

                fp[l] = Marshal.StringToCoTaskMemAuto(f);
                np[l] = Marshal.StringToCoTaskMemAuto(s);

                l++;
			}
		}

        GCHandle faBuf = GCHandle.Alloc(fp, GCHandleType.Pinned);
        GCHandle naBuf = GCHandle.Alloc(np, GCHandleType.Pinned);

        int res = 0;

        if (totalFiles > 0) {
            if(progress != null) {
                GCHandle prog = GCHandle.Alloc(progress, GCHandleType.Pinned);
                res = zipCDList(levelOfCompression, @zipArchive, faBuf.AddrOfPinnedObject(), totalFiles, prog.AddrOfPinnedObject(), naBuf.AddrOfPinnedObject(), password, useBz2, diskSize);
                prog.Free();
            } else {
                res = zipCDList(levelOfCompression, @zipArchive, faBuf.AddrOfPinnedObject(), totalFiles, IntPtr.Zero, naBuf.AddrOfPinnedObject(), password, useBz2, diskSize);
            }
        } else {
            Debug.Log("Directory has no files.");
        }


        for (int i = 0; i < totalFiles; i++) {
            Marshal.FreeCoTaskMem(fp[i]);
            Marshal.FreeCoTaskMem(np[i]);
        }

        faBuf.Free(); fp = null;
        naBuf.Free(); np = null;

        return res;
	}


	//Use this function to get the total files of a directory and its subdirectories.
	public static int getAllFiles(string Dir) {
		string[] filePaths = Directory.GetFiles(@Dir, "*", SearchOption.AllDirectories);
		int res = filePaths.Length;
		filePaths = null;
		return res;
	}
#endif

    // get the DateTime of an entry in a zip archive
    //
    // zipArchive       : the full path to the zip archive from which we want to extract the specific file.
    // entry		    : the specific entry we want to get the DateTime of. (If the entry resides in a  directory, the directory path should be included. like dir1/dir2/myfile.bin)
    // FileBuffer		: A buffer that holds a zip file. When assigned the function will read from this buffer and will ignore the zipArchive path.
    //
    // Returns the date and time of the entry in DateTime format.
    //
    // ERROR CODES
    //					: 0 = Cannot open zip Archive
    //					: 1 = entry not found
    //					: 2 = error reading entry
    //
    public static DateTime entryDateTime(string zipArchive, string entry, byte[] FileBuffer = null) {
        uint dosDateTime = 0;

        if (FileBuffer != null) {
            GCHandle fbuf = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
            dosDateTime = getEntryDateTime(null, entry, fbuf.AddrOfPinnedObject(), FileBuffer.Length);
            fbuf.Free();
        } else {
            #if !UNITY_WEBGL || UNITY_EDITOR
            dosDateTime = getEntryDateTime(zipArchive, entry, IntPtr.Zero, 0);
            #endif
        }

        var date = (dosDateTime & 0xFFFF0000) >> 16;
        var time = (dosDateTime & 0x0000FFFF);

        var year = (date >> 9) + 1980;
        var month = (date & 0x01e0) >> 5;
        var day = date & 0x1F;
        var hour = time >> 11;
        var minute = (time & 0x07e0) >> 5;
        var second = (time & 0x1F) * 2;

        if (dosDateTime == 0 || dosDateTime == 1 || dosDateTime == 2) { Debug.Log("Error in getting DateTime: " + dosDateTime); return DateTime.Now; }

        return new DateTime((int)year, (int)month, (int)day, (int)hour, (int)minute, (int)second);
    }




    //---------------------------------------------------------------------------------------------------------------------------
    // In Memory Functions
    //---------------------------------------------------------------------------------------------------------------------------

    // The inMemory class the holds the pointer to our in memory zip archive.
    // Use the size() function to get the size of the memory it occupies.
    // The getZipBuffer() function returns a new byte[] buffer that contains the inmemory zip.
    //
    public class inMemory
    {
        public IntPtr pointer = IntPtr.Zero;
        public IntPtr zf = IntPtr.Zero;
        public IntPtr memStruct = IntPtr.Zero;
        public IntPtr fileStruct = IntPtr.Zero;

        public int[] info = new int[3];

        public int lastResult = 0;

        // A bool that tells if an inMemory zip archive is open or closed. (Used with the low level functions only.)
        public bool isClosed = true;

        public int size()
        {
            return info[0];
        }

        public byte[] getZipBuffer() {
            if (pointer != IntPtr.Zero && info[0] > 0 ) {
                byte [] p = new byte[ info[0] ];
                Marshal.Copy(pointer, p, 0, info[0]);
                return p;
            } else {
                return null;
            }
        }
    }

    // Use this function to free the pointer and the object of the inMemory zip archive.
    // It is important to call this function after you don't need the in memory zip any more!
    //
    public static void free_inmemory(inMemory t) {
        if(t.info == null) { Debug.Log("inMemory object is null"); return; }
        if(freeMemStruct(t.pointer) != 1) Debug.Log("In memory pointer was not freed");
        t.info = null;
        if(t.memStruct != IntPtr.Zero && freeMemZ(t.memStruct) != 1) Debug.Log("MemStruct was not freed");
        if(t.fileStruct != IntPtr.Zero && freeFileZ(t.fileStruct) != 1) Debug.Log("FileStruct was not freed");
        t = null;
    }


    // Low level inMemory functions ----------------------------------------------------------------------------------------------------

    // A function that creates an inMemory zip archive
    //
    // t                  : the inMemory class that holds the pointer to our inMemory zip file. You can call this function again for more buffers with the same
    //                    : inMemory object and the next buffers will get appended to the in memory zip.
    //
    // Returns true on success.
    public static bool inMemoryZipStart(inMemory t) {
        if(t.info == null) { Debug.Log("inMemory object is null"); return false; }
        if(t.fileStruct == IntPtr.Zero) t.fileStruct = initFileStruct();
        if(t.memStruct == IntPtr.Zero) t.memStruct = initMemStruct();
        if(!t.isClosed) inMemoryZipClose(t);

        GCHandle inf = GCHandle.Alloc(t.info, GCHandleType.Pinned);

        t.zf = zipCDMemStart(inf.AddrOfPinnedObject(), t.pointer, t.fileStruct, t.memStruct);

        inf.Free();
        t.isClosed = false;

        if(t.zf != IntPtr.Zero) return true; else return false;
    }

    // A function that adds a buffer as a zip entry in an opened inMemory zip archive with the inMemoryZipStart function.
    //
    // t                  : the inMemory class that holds the pointer to our inMemory zip file.
    // levelOfCompression : (0-9) recommended 9 for maximum (10 is highest but slower and not zlib compatible)
    // buffer             : The byte[] buffer that should be added to the zip.
    // filename           : The name of the file added. If you add a folder structure to it,
    //                      like (dir1/dir2/myfile.bin) the directories will be created in the zip file.
    // comment            : add a comment for the file in the zip file header.
    // password			  : set the password to protect this file. (only ascii characters)
    // useBz2			  : use the bz2 compression algorithm. If false the zlib deflate algorithm will be used. (not available for MacOS/iOS/tvOS)
    //
    // Returns 0 on success. 
    public static int inMemoryZipAdd(inMemory t, int levelOfCompression, byte[] buffer, string fileName, string comment = null, string password = null, bool useBz2 = false) {
        if(t.info == null) { Debug.Log("inMemory object is null"); return -1; }
        if (t.isClosed) { Debug.Log("Can't add entry. inMemory zip is closed."); return -2; }
        if (password == "") password = null;
        if (comment == "") comment = null;
        if (fileName == null) fileName = "";     
        
        GCHandle fbuf = GCHandle.Alloc(buffer, GCHandleType.Pinned);

        int res = zipCDMemAdd(t.zf, levelOfCompression, fbuf.AddrOfPinnedObject(), buffer.Length, fileName, comment, password, useBz2);

        fbuf.Free();
        t.lastResult = res;

        return res;
    }

    // A function to close the inMemory zip archive that has been created with the inMemoryZipStart function.
    //
    // t                  : the inMemory class that holds the pointer to our inMemory zip file.
    //
    // If t.lastResult is different then 0 a null pointer will get returned.
    //
    public static IntPtr inMemoryZipClose(inMemory t) {
        if(t.info == null) { Debug.Log("inMemory object is null"); return IntPtr.Zero; }
        if (t.isClosed) { Debug.Log("Can't close zip. inMemory zip is closed."); return t.pointer; }
        GCHandle inf = GCHandle.Alloc(t.info, GCHandleType.Pinned);

        t.pointer = zipCDMemClose(t.zf, t.memStruct, inf.AddrOfPinnedObject(), t.lastResult);

        inf.Free();
        t.isClosed = true;

        return t.pointer;
    }

    // End low level inMemory functions ------------------------------------------------------------------------------------------------


    // A function that compresses a buffer to an inMemory zip file. Appending using this function can be slow. Use the low level functions for way faster proccessing.
    //
    // t                  : the inMemory class that holds the pointer to our inMemory zip file. You can call this function again for more buffers with the same
    //                    : inMemory object and the next buffers will get appended to the in memory zip.
    // levelOfCompression : (0-9) recommended 9 for maximum (10 is highest but slower and not zlib compatible)
    // buffer             : The byte[] buffer that should be added to the zip.
    // filename           : The name of the file added. If you add a folder structure to it,
    //                      like (dir1/dir2/myfile.bin) the directories will be created in the zip file.
    // comment            : add a comment for the file in the zip file header.
    // password			  : set the password to protect this file. (only ascii characters)
    // useBz2			  : use the bz2 compression algorithm. If false the zlib deflate algorithm will be used. (not available for MacOS/iOS/tvOS)
    //
    /// Although the inMemory t.pointer gets internally updated, the function returns an IntPtr of the inMemory zip file buffer.
    /// So to check if the operation was successful, check if the pointer returned is a non IntPtr.Zero pointer.
    public static IntPtr compress_Buf2Mem(inMemory t, int levelOfCompression, byte[] buffer, string fileName, string comment = null, string password = null, bool useBz2 = false) {
        if(t.info == null) { Debug.Log("inMemory object is null"); return IntPtr.Zero; }
        if (levelOfCompression < 0) levelOfCompression = 0; if (levelOfCompression > 9) levelOfCompression = 9;
        if (password == "") password = null;
        if (comment == "") comment = null;
        if (fileName == null) fileName = "";

        if (buffer == null || buffer.Length == 0) { Debug.Log("Buffer was null or zero size !"); return t.pointer; }

        GCHandle fbuf = GCHandle.Alloc(buffer, GCHandleType.Pinned);
        GCHandle inf = GCHandle.Alloc(t.info, GCHandleType.Pinned);

        t.pointer = zipCDMem(inf.AddrOfPinnedObject(), t.pointer, levelOfCompression, fbuf.AddrOfPinnedObject(), buffer.Length, fileName, comment, password, useBz2);

        fbuf.Free();
        inf.Free();

        return t.pointer;
    }

    #if !UNITY_WEBGL || UNITY_EDITOR
    // A function that decompresses a zip file from an inMemory pointer. If the zip contains directories, they will be created.
    //
    // t                : the inMemory class that holds the pointer to our inMemory zip file.
    // outPath          : the directory in which the zip contents will be extracted.
    // progress         : provide a single item integer array to write the current index of the file getting extracted. To use it in realtime, call
    //                  : this function in a separate thread.
    // proc:			: a single item integer array that gets updated with the progress of the decompression in bytes.
    //					  (100% is reached when the compressed size of the file is reached.)
    // password			: if needed, the password to decrypt the archive.
    //
    // ERROR CODES
    //                  : -1 = could not initialize zip archive.
    //                  : -2 = failed extraction
    //                  :  1 = success
    //
    public static int decompress_Mem2File(inMemory t, string outPath, int[] progress, int[] proc = null, string password = null) {
        if(t.info == null) { Debug.Log("inMemory object is null"); return -1; }
        //make a check if the last '/' exists at the end of the exctractionPath and add it if it is missing
        if (outPath.Substring(outPath.Length - 1, 1) != "/") { outPath += "/"; }
        int res = 0;
        GCHandle ibuf = GCHandle.Alloc(progress, GCHandleType.Pinned);
        if (proc == null) proc = new int[1];
        GCHandle pbuf = GCHandle.Alloc(proc, GCHandleType.Pinned);

        if (t != null) {
            if (proc != null) res = zipEX(null, @outPath, ibuf.AddrOfPinnedObject(), t.pointer, t.info[0], pbuf.AddrOfPinnedObject(), password);
            else res = zipEX(null, @outPath, ibuf.AddrOfPinnedObject(), t.pointer, t.info[0], IntPtr.Zero, password);

            ibuf.Free(); pbuf.Free(); return res;
        } else return 0;
    }
    #endif

    // A function that will decompress a file from an inmemory zip file pointer directly in a provided byte buffer.
    //
    // t                : the inMemory class that holds the pointer to our inMemory zip file.
    // entry            : the file we want to extract to a buffer. (If the file resides in a directory, the directory should be included.)
    // buffer           : a referenced byte buffer that will be resized and will be filled with the extraction data.
    // password			: If the archive is encrypted use a password.
    //
    // ERROR CODES      :  1 = success
    //                  : -2 = could not find/open zip file
    //					: -3 = could not locate entry
    //					: -4 = could not get entry info
    //					: -5 = password error
    //					: -18 = the entry has no size
    //					: -104 = internal memory error
    //
    public static int entry2BufferMem(inMemory t, string entry, ref byte[] buffer, string password = null) {
        if(t.info == null) return -2;

        int siz = 0;
        if (password == "") password = null;
        if (t != null) siz = zipGetEntrySize(null, entry, t.pointer, t.info[0]);

        if (siz <= 0) return -18;
        if(buffer == null) buffer = new byte[0];
        System.Array.Resize(ref buffer, siz);
        GCHandle sbuf = GCHandle.Alloc(buffer, GCHandleType.Pinned);

        int res = 0;
        if (t != null) res = zipEntry2Buffer(null, entry, sbuf.AddrOfPinnedObject(), siz, t.pointer, t.info[0], password);
        sbuf.Free();

        return res;
    }

    // A function that will decompress a file from an inmemory zip file pointer to a new created and returned byte buffer.
    //
    // t                : the inMemory class that holds the pointer to our inMemory zip file.
    // entry            : the file we want to extract to a buffer. (If the file resides in a directory, the directory should be included.
    // password			: If the archive is encrypted use a password.
    //
    // ERROR CODES		: non-null  = success
    //                  : null      = failed
    //
    public static byte[] entry2BufferMem(inMemory t, string entry,  string password = null) {
        if(t.info == null) return null;
        int siz = 0;
        if (password == "") password = null;
        if (t != null) siz = zipGetEntrySize(null, entry, t.pointer, t.info[0]);

        if (siz <= 0) return null;
        byte[] buffer = new byte[siz];
        GCHandle sbuf = GCHandle.Alloc(buffer, GCHandleType.Pinned);

        int res = 0;
        if (t != null) res = zipEntry2Buffer(null, entry, sbuf.AddrOfPinnedObject(), siz, t.pointer, t.info[0], password);

        sbuf.Free();

        if (res != 1) return null;
        else return buffer;
    }


    // A function that will decompress a file from an inmemory zip file pointer directly to a provided fixed size byte buffer.
    //
    // Returns the uncompressed size of the entry.
    //
    // t                : the inMemory class that holds the pointer to our inMemory zip file.
    // entry            : the file we want to extract to a buffer. (If the file resides in a directory, the directory should be included.)
    // buffer           : a referenced fixed size byte buffer that will be filled with the extraction data. It should be large enough to store the data.
    // password			: if the archive is encrypted use a password.
    //
    // ERROR CODES      :  1 = success
    //                  : -2 = could not find/open zip archive
    //					: -3 = could not locate entry
    //					: -4 = could not get entry info
    //					: -5 = password error
    //					: -18 = the entry has no size
    //					: -19 = the fixed size buffer is not big enough to store the uncompressed data
    //					: -104 = internal memory error
    //
    public static int entry2FixedBufferMem(inMemory t, string entry, ref byte[] fixedBuffer, string password = null) {
        if(t.info == null) return -2;
        int siz = 0;
        if (password == "") password = null;
        if (t != null) siz = zipGetEntrySize(null, entry, t.pointer, t.info[0]);

        if (siz <= 0) return -18;

        if (fixedBuffer.Length < siz) return -19;

        GCHandle sbuf = GCHandle.Alloc(fixedBuffer, GCHandleType.Pinned);

        int res = 0;
        if (t != null) res = zipEntry2Buffer(null, entry, sbuf.AddrOfPinnedObject(), siz, t.pointer, t.info[0], password);

        sbuf.Free();

        if (res != 1) return res;

        return siz;
    }

    // This function fills the Lists with the filenames and file sizes that are in the inMemory zip file.
    // Returns			: the total size of uncompressed bytes of the files in the zip archive.
    //
    // t                : the inMemory class that holds the pointer to our inMemory zip file.
    //
    // ERROR CODES      : -1 = Input file not found
    //                  : -2 = Could not get info
    //
    public static long getFileInfoMem(inMemory t) {
        if(t.info == null) return -1;
        ninfo.Clear(); uinfo.Clear(); cinfo.Clear();
        zipFiles = 0; zipFolders = 0;
        int res = 0;
        int[] tt = new int[1];
        GCHandle tb = GCHandle.Alloc(tt, GCHandleType.Pinned);

        if (t != null) res = zipGetInfoA(null, tb.AddrOfPinnedObject(), t.pointer, t.info[0]);
        tb.Free();

        if (res <= 0) { return -1; }

        IntPtr uni = IntPtr.Zero;

        uint[] unc = new uint[tt[0]];
        uint[] comp = new uint[tt[0]];

        GCHandle uncb = GCHandle.Alloc(unc, GCHandleType.Pinned);
        GCHandle compb = GCHandle.Alloc(comp, GCHandleType.Pinned);

        if (t != null) uni = zipGetInfo(null, res, uncb.AddrOfPinnedObject(), compb.AddrOfPinnedObject(), t.pointer, t.info[0]);

        if (uni == IntPtr.Zero) { uncb.Free(); compb.Free(); return -2; }

        string str = Marshal.PtrToStringAuto(uni);
        StringReader r = new StringReader(str);

        string line;
        long sum = 0;

        for (int i = 0; i < tt[0]; i++) {
            if ((line = r.ReadLine()) != null) ninfo.Add(line);
            if (unc != null) {
                uinfo.Add((long)unc[i]);
                sum += unc[i];
                if (unc[i] > 0) zipFiles++; else zipFolders++;
            }
            if (comp != null) cinfo.Add((long)comp[i]);
        }

        r.Close();
        r.Dispose();
        uncb.Free(); compb.Free();
        releaseBuffer(uni);
        tt = null;
        unc = null;
        comp = null;

        return sum;
    }

    //---------------------------------------------------------------------------------------------------------------------------
    // End in Memory Functions
    //---------------------------------------------------------------------------------------------------------------------------


    // A function that will decompress a file in a zip archive directly to a provided byte buffer.
    //
    // zipArchive       : the full path to the zip archive from which a specific file will be extracted to a byte buffer.
    // entry            : the file we want to extract to a buffer. (If the file resides in a directory, the directory should be included.)
    // buffer           : a referenced byte buffer that will be resized and will be filled with the extraction data.
	// FileBuffer		: A buffer that holds a zip file. When assigned the function will read from this buffer and will ignore the filePath. 
	// password			: If the archive is encrypted use a password.
    //
    // ERROR CODES      :  1 = success
    //                  : -2 = could not find/open zip archive
	//					: -3 = could not locate entry
	//					: -4 = could not get entry info
	//					: -5 = password error
	//					: -18 = the entry has no size
	//					: -104 = internal memory error
    //
    public static int entry2Buffer(string zipArchive, string entry, ref byte[] buffer, byte[] FileBuffer = null, string password = null) {

		int siz = 0;
		if(password == "") password = null;
		if(FileBuffer!= null) {
			GCHandle fbuf = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
			siz = zipGetEntrySize(null, entry, fbuf.AddrOfPinnedObject(), FileBuffer.Length);
			fbuf.Free();
		}else{
            #if !UNITY_WEBGL || UNITY_EDITOR
			siz = zipGetEntrySize( @zipArchive,  entry, IntPtr.Zero, 0);
            #endif
		}

        if (siz <= 0) return -18;

        System.Array.Resize(ref buffer, siz);

        GCHandle sbuf = GCHandle.Alloc(buffer, GCHandleType.Pinned);

		int res = 0;
		if(FileBuffer!= null) {
			GCHandle fbuf = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
			res = zipEntry2Buffer(null, entry, sbuf.AddrOfPinnedObject(), siz, fbuf.AddrOfPinnedObject(), FileBuffer.Length, password);
			fbuf.Free();
		}else{
            #if !UNITY_WEBGL || UNITY_EDITOR
			res = zipEntry2Buffer( @zipArchive, entry, sbuf.AddrOfPinnedObject(), siz, IntPtr.Zero, 0, password);
            #endif
		}

        sbuf.Free();

        return res;
    }

    // A function that will decompress a file in a zip archive directly to a provided fixed size byte buffer.
    //
	// Returns the uncompressed size of the entry.
	//
    // zipArchive       : the full path to the zip archive from which a specific file will be extracted to a byte buffer.
    // entry            : the file we want to extract to a buffer. (If the file resides in a directory, the directory should be included.)
    // buffer           : a referenced fixed size byte buffer that will be filled with the extraction data. It should be large enough to store the data.
	// FileBuffer		: a buffer that holds a zip file. When assigned the function will read from this buffer and will ignore the filePath. 
	// password			: if the archive is encrypted use a password.
    //
    // ERROR CODES      :  1 = success
    //                  : -2 = could not find/open zip archive
	//					: -3 = could not locate entry
	//					: -4 = could not get entry info
	//					: -5 = password error
	//					: -18 = the entry has no size
	//					: -19 = the fixed size buffer is not big enough to store the uncompressed data
	//					: -104 = internal memory error
    //
    public static int entry2FixedBuffer(string zipArchive, string entry, ref byte[] fixedBuffer, byte[] FileBuffer = null, string password = null) {

		int siz = 0;
		if(password == "") password = null;
		if(FileBuffer!= null) {
			GCHandle fbuf = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
			siz = zipGetEntrySize(null, entry, fbuf.AddrOfPinnedObject(), FileBuffer.Length);
			fbuf.Free();
		}else{
            #if !UNITY_WEBGL || UNITY_EDITOR
			siz = zipGetEntrySize( @zipArchive,  entry, IntPtr.Zero, 0);
            #endif
		}
        
        if (siz <= 0) return -18;

		if(fixedBuffer.Length < siz) return -19;

        GCHandle sbuf = GCHandle.Alloc(fixedBuffer, GCHandleType.Pinned);

		int res = 0;
		if(FileBuffer!= null) {
			GCHandle fbuf = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
			res = zipEntry2Buffer(null, entry, sbuf.AddrOfPinnedObject(), siz, fbuf.AddrOfPinnedObject(), FileBuffer.Length, password);
			fbuf.Free();
		}else{
            #if !UNITY_WEBGL || UNITY_EDITOR
			res = zipEntry2Buffer( @zipArchive, entry, sbuf.AddrOfPinnedObject(), siz, IntPtr.Zero, 0, password);
            #endif
		}

        sbuf.Free();

        if(res != 1) return res;

		return siz;
    }


    // A function that will decompress a file in a zip archive to a new created and returned byte buffer.
    //
    // zipArchive       : the full path to the zip archive from which a specific file will be extracted to a byte buffer.
    // entry            : the file we want to extract to a buffer. (If the file resides in a directory, the directory should be included.
	// FileBuffer		: A buffer that holds a zip file. When assigned the function will read from this buffer and will ignore the filePath. 
	// password			: If the archive is encrypted use a password.
    //
    // ERROR CODES		: non-null  = success
    //                  : null      = failed
    //
    public static byte[] entry2Buffer(string zipArchive, string entry, byte[] FileBuffer = null, string password = null) {
		int siz = 0;
		if(password == "") password = null;
		if(FileBuffer!= null) {
			GCHandle fbuf = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
			siz = zipGetEntrySize(null, entry, fbuf.AddrOfPinnedObject(), FileBuffer.Length);
			fbuf.Free();
		}else{
            #if !UNITY_WEBGL || UNITY_EDITOR
			siz = zipGetEntrySize( @zipArchive,  entry, IntPtr.Zero, 0);
            #endif
		}

        if (siz <= 0) return null;

        byte[] buffer = new byte[siz];

        GCHandle sbuf = GCHandle.Alloc(buffer, GCHandleType.Pinned);

		int res = 0;
		if(FileBuffer!= null) {
			GCHandle fbuf = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
			res = zipEntry2Buffer(null, entry, sbuf.AddrOfPinnedObject(), siz, fbuf.AddrOfPinnedObject(), FileBuffer.Length, password);
			fbuf.Free();
		}else{
            #if !UNITY_WEBGL || UNITY_EDITOR
			res = zipEntry2Buffer( @zipArchive, entry, sbuf.AddrOfPinnedObject(), siz, IntPtr.Zero, 0, password);
            #endif
		}

        sbuf.Free();
        if (res!=1) return null;
        else return buffer;
    }

    
	// A function that will validate a zip archive.
	//
    // zipArchive       : the zip to be checked
	// FileBuffer		: A buffer that holds a zip file. When assigned the function will read from this buffer and will ignore the filePath.
    // ERROR CODES
	//
	//					: true. The archive is ok.
	//					: false. The archive could not be validated.
	//
    public static bool validateFile(string zipArchive, byte[] FileBuffer = null) {
		bool res = false;
		
			if(FileBuffer != null) {
				GCHandle fbuf = GCHandle.Alloc(FileBuffer, GCHandleType.Pinned);
				res = zipValidateFile(null, fbuf.AddrOfPinnedObject(), FileBuffer.Length);
				fbuf.Free();
				return res;
			}

        #if !UNITY_WEBGL || UNITY_EDITOR
        res = zipValidateFile(@zipArchive, IntPtr.Zero, 0);
        #endif

		return res;
    }

    // ---------------------------------------------------------------------------------------------------------------------------------
    //
    // Hidden/merged zip and zipInfo functions
    //
    // ---------------------------------------------------------------------------------------------------------------------------------

    // a struct to store zip entry information
    public struct zipInfo{
        public short VersionMadeBy;
        public short MinimumVersionToExtract;
        public short BitFlag;
        public short CompressionMethod;
        public short FileLastModificationTime;
        public short FileLastModificationDate;
        public int CRC;
        public int CompressedSize;
        public int UncompressedSize;
        public short DiskNumberWhereFileStarts;
        public short InternalFileAttributes;
        public int ExternalFileAttributes;
        public int RelativeOffsetOfLocalFileHeader;
        public string filename;
        public string extraField;
        public string fileComment;
    };

    // a list that will be filled with info of all the zip archive entries.
    public static List<zipInfo> zinfo;

    #if !UNITY_WEBGL || UNITY_EDITOR
    // This function is used to get extended info of the entries in a zip archive in the file system.
    // Use this as an alternative function to get zip info with more information about entries.
    // Returns true on success.
    public static bool getZipInfo(string fileName) {

        if (!File.Exists(fileName)) { Debug.Log("File not found: "+ fileName); return false; }

        int pos =0, size = 0;

        using (var file = File.OpenRead(fileName)) {
            using (var reader = new BinaryReader(file)) { 
                if (findPK(reader)) {
                    int entryCount = findEnd(reader, ref pos, ref size);
                    if (entryCount > 0) {
                        getCentralDir(reader, entryCount);
                        return true;
                    } else {
                        Debug.Log("No Entries in zip");
                        return false;
                    }
                }
            }
        }
        return false;
    }

    // Get position, size and/or entry info of a zip archive in the file system.
    // This function is mainly useful to discover a zip archive hidden or merged in another bigger archive.
    // filename:    the path to the archive.
    // ref pos:     the position in bytes of the zip archive.
    // ref size:    the size of the zip archive.
    // If getCentralDirectory is set to true it will fill the zinfo List with extended entry information.
    // Returns true on success.
    public static bool getZipInfoMerged(string fileName, ref int pos, ref int size, bool getCentralDirectory = false) {

        if (!File.Exists(fileName)) { Debug.Log("File not found: "+ fileName); return false; }

        using (var file = File.OpenRead(fileName)) {
            using (var reader = new BinaryReader(file)) { 
                if (findPK(reader)) {
                    int entryCount = findEnd(reader, ref pos, ref size);
                    if (entryCount > 0) {
                        if(getCentralDirectory) getCentralDir(reader, entryCount);
                        return true;
                    } else {
                        Debug.Log("No Entries in zip");
                        return false;
                    }
                }
            }
        }
        return false;
    }
    #endif

    // Get position, size and/or entry info of a zip archive in a buffer.
    // This function is mainly useful to discover a zip archive hidden or merged in another bigger buffer.
    // buffer:      the buffer where the merged zip archive resides.
    // ref pos:     the position in bytes of the zip archive.
    // ref size:    the size of the zip archive.
    // If getCentralDirectory is set to true it will fill the zinfo List with extended entry information.
    // Returns true on success.
    public static bool getZipInfoMerged(byte[] buffer, ref int pos, ref int size, bool getCentralDirectory = false) {

        if (buffer == null) { Debug.Log("Buffer is null"); return false; }

        using (var file = new MemoryStream(buffer)) {
            using (var reader = new BinaryReader(file)) {
                if (findPK(reader)) {
                    int entryCount = findEnd(reader, ref pos, ref size);
                    if (entryCount > 0) {
                        if (getCentralDirectory) getCentralDir(reader, entryCount);
                        return true;
                    } else {
                        Debug.Log("No Entries in zip");
                        return false;
                    }
                }
            }
        }
        return false;
    }

    // This function is used to get extended info of the entries in a zip archive in a buffer.
    // buffer:      the buffer where the merged zip archive resides.
    // Returns true on success.
    public static bool getZipInfoMerged(byte[] buffer) {

        if (buffer == null) { Debug.Log("Buffer is null"); return false; }
        
        int pos =0, size = 0;

        using (var file = new MemoryStream(buffer)) {
            using (var reader = new BinaryReader(file)) {
                if (findPK(reader)) {
                    int entryCount = findEnd(reader, ref pos, ref size);
                    if (entryCount > 0) {
                        getCentralDir(reader, entryCount);
                        return true;
                    } else {
                        Debug.Log("No Entries in zip");
                        return false;
                    }
                }
            }
        }
        return false;
    }

    // Find Central directory position
    private static bool findPK(BinaryReader reader) {

        var p = reader.ReadByte();
        bool res = false;
        int i = 0;

        while (reader.BaseStream.Position < reader.BaseStream.Length-3) {
            i++;

            if (p == 0x50) {
                if(reader.ReadByte() == 0x4b && reader.ReadByte() == 0x05 && reader.ReadByte() == 0x06) { reader.BaseStream.Seek(reader.BaseStream.Position - 4, 0); res = true; break; }
                else{ reader.BaseStream.Seek(i, 0);}
            }
            p = reader.ReadByte();
        }
        return res;
    }

    // Read central directory basics and find position and size of the zip archive in stream. Returns entry count.
    private static int findEnd(BinaryReader reader, ref int pos, ref int size) {

    long origin = reader.BaseStream.Position;

    int result = 0;
        while (result == 0 && reader.BaseStream.Position < reader.BaseStream.Length) {

            var b = reader.ReadByte();

            while (b != 0x50 ) {
                if (reader.BaseStream.Position < reader.BaseStream.Length)
                    b = reader.ReadByte();
                else
                    break;
            }

            if (reader.BaseStream.Position >= reader.BaseStream.Length) break;

            if (reader.ReadByte() == 0x4b && reader.ReadByte() == 0x05 && reader.ReadByte() == 0x06) {

                /*int diskNumber =*/ reader.ReadInt16();
                /*int centralDirectoryStartDiskNumber =*/ reader.ReadInt16();
                /*int centralDirectoryCount =*/ reader.ReadInt16();
                int centralDirectoryTotal = reader.ReadInt16();
                result = centralDirectoryTotal;
                int centralDirectorySize = reader.ReadInt32();
                int centralDirectoryOffset = reader.ReadInt32();

                int commentLength = reader.ReadInt16();

                reader.ReadBytes(commentLength);
                //string comment = Encoding.UTF8.GetString(reader.ReadBytes(commentLength));

                pos = (int)reader.BaseStream.Position  - (centralDirectoryOffset + centralDirectorySize + 22);
                size = (int)reader.BaseStream.Position - pos;

                reader.BaseStream.Seek(pos + centralDirectoryOffset, 0);
                break;
            }
        }
        return result;
    }

    // Read info for each entry in the zip archive and store it in zinfo List.
    private static void getCentralDir(BinaryReader reader, int count) {
            
        if(zinfo != null && zinfo.Count > 0) zinfo.Clear();

        zinfo = new List<zipInfo>();

        for (int i = 0; i < count; i++) {
            var magic = reader.ReadInt32();

            if (magic == 0x02014b50) {
                zipInfo z = new zipInfo();

                z.VersionMadeBy = reader.ReadInt16();
                z.MinimumVersionToExtract = reader.ReadInt16();
                z.BitFlag = reader.ReadInt16();
                z.CompressionMethod = reader.ReadInt16();
                z.FileLastModificationTime = reader.ReadInt16();
                z.FileLastModificationDate = reader.ReadInt16();
                z.CRC = reader.ReadInt32();
                z.CompressedSize = reader.ReadInt32();
                z.UncompressedSize = reader.ReadInt32();
                short fileNameLength = reader.ReadInt16();
                short extraFieldLength = reader.ReadInt16();
                short fileCommentLength = reader.ReadInt16();
                z.DiskNumberWhereFileStarts = reader.ReadInt16();
                z.InternalFileAttributes = reader.ReadInt16();
                z.ExternalFileAttributes = reader.ReadInt32();
                z.RelativeOffsetOfLocalFileHeader = reader.ReadInt32();
                z.filename = Encoding.UTF8.GetString(reader.ReadBytes(fileNameLength));
                z.extraField = Encoding.ASCII.GetString(reader.ReadBytes(extraFieldLength));
                z.fileComment = Encoding.UTF8.GetString(reader.ReadBytes(fileCommentLength));

                zinfo.Add(z);
            }
        }
    }

    #if !UNITY_WEBGL || UNITY_EDITOR
    // Get the merged zip archive in a file system archive as a byte buffer and provide postion and size.
    // filePath:        the path to the archive.
    // ref position:    the position in bytes of the zip archive.
    // ref size:        the size of the zip archive.
    public static byte[] getMergedZip(string filePath, ref int position, ref int siz) {
        int pos = 0, size = 0;
        if (!File.Exists(filePath)) return null;

        getZipInfoMerged(filePath, ref pos, ref size);

        position = pos; siz = size;
        if(size == 0) return null;

        byte[] tempBuffer = new byte[size];

        using (var file = File.OpenRead(filePath)) {
            using (var reader = new BinaryReader(file)) {
                reader.BaseStream.Seek(pos, SeekOrigin.Begin);
                reader.Read(tempBuffer, 0, size);
            }
        }

        return tempBuffer;
    }

    // Get the merged zip archive in a file system archive as a byte buffer.
    // filePath:        the path to the archive.
    public static byte[] getMergedZip(string filePath) {
        int pos = 0, size = 0;
        if (!File.Exists(filePath)) return null;

        getZipInfoMerged(filePath, ref pos, ref size);
        if(size == 0) return null;

        byte[] tempBuffer = new byte[size];

        using (var file = File.OpenRead(filePath)) {
            using (var reader = new BinaryReader(file)) {
                reader.BaseStream.Seek(pos, SeekOrigin.Begin);
                reader.Read(tempBuffer, 0, size);
            }
        }

        return tempBuffer;
    }
    #endif

    // Get the merged zip archive in a buffer as a byte buffer.
    // buffer:          the buffer where the zip archive resides.
    // ref position:    the position in bytes of the zip archive.
    // ref size:        the size of the zip archive.
    public static byte[] getMergedZip(byte[] buffer, ref int position, ref int siz) {
        int pos = 0, size = 0;

        if (buffer == null) return null;

        getZipInfoMerged(buffer, ref pos, ref size);

        position = pos; siz = size;

        if(size == 0) return null;

        byte[] tempBuffer = new byte[size];

        using (var file = new MemoryStream(buffer)) {
            using (var reader = new BinaryReader(file)) {
                reader.BaseStream.Seek(pos, SeekOrigin.Begin);
                reader.Read(tempBuffer, 0, size);
            }
        }

        return tempBuffer;
    }
    // Get the merged zip archive in a buffer as a byte buffer.
    // buffer:          the buffer where the zip archive resides.
    public static byte[] getMergedZip(byte[] buffer) {
        int pos = 0, size = 0;
        if (buffer == null) return null;

        getZipInfoMerged(buffer, ref pos, ref size);
        if(size == 0) return null;
        byte[] tempBuffer = new byte[size];

        using (var file = new MemoryStream(buffer)) {
            using (var reader = new BinaryReader(file)) {
                reader.BaseStream.Seek(pos, SeekOrigin.Begin);
                reader.Read(tempBuffer, 0, size);
            }
        }

        return tempBuffer;
    }

    #if !UNITY_WEBGL || UNITY_EDITOR
    // A function that extracts all contents of a zip file that is merged in another file in the file system, to disk.
    // file             : the path to the file where the zip archive resides.
    // outPath          : the directory in which the zip contents will be extracted.
    // progress         : provide a single item integer array to write the current index of the file getting extracted. To use it in realtime, call
    //                  : this function in a separate thread.
    // proc:			: a single item integer array that gets updated with the progress of the decompression in bytes.
    //					  (100% is reached when the compressed size of the file is reached.)
    // password			: if needed, the password to decrypt the archive.
    // ERROR CODES
    //                  : -1 = could not initialize zip archive.
    //                  : -2 = failed extraction
    //                  :  1 = success
    public static int decompressZipMerged(string file, string outPath, int[] progress, int[] proc = null, string password = null) {
        
        if (!File.Exists(file)) return 0;
        outPath = outPath.Replace("//", "/");

        if (!Directory.Exists(outPath)) Directory.CreateDirectory(outPath);

        int pos = 0, size = 0, res = 0;

        var tempBuffer = getMergedZip(file, ref pos, ref size);

        if(tempBuffer != null) {
            inMemory t2 = new inMemory();
            GCHandle pinnedArray = GCHandle.Alloc(tempBuffer, GCHandleType.Pinned);
            t2.pointer = new IntPtr(pinnedArray.AddrOfPinnedObject().ToInt64() );
            t2.info[0] = size;

            res = decompress_Mem2File(t2, outPath, progress, proc, password);
        
            pinnedArray.Free();
            t2.info = null;
            t2.pointer = IntPtr.Zero;
            t2 = null;
            tempBuffer = null;
        }
        return res;
    }

    // A function that extracts all contents of a merged zip file that resides in a buffer to disk.
    // buffer           : the buffer where the zip archive resides.
    // outPath          : the directory in which the zip contents will be extracted.
    // progress         : provide a single item integer array to write the current index of the file getting extracted. To use it in realtime, call
    //                  : this function in a separate thread.
    // proc:			: a single item integer array that gets updated with the progress of the decompression in bytes.
    //					  (100% is reached when the compressed size of the file is reached.)
    // password			: if needed, the password to decrypt the archive.
    // ERROR CODES
    //                  : -1 = could not initialize zip archive.
    //                  : -2 = failed extraction
    //                  :  1 = success
    public static int decompressZipMerged(byte[] buffer, string outPath, int[] progress, int[] proc = null, string password = null) {
        
        if (buffer == null) return 0;
        outPath = outPath.Replace("//", "/");
        if (!Directory.Exists(outPath)) Directory.CreateDirectory(outPath);

        int pos = 0, size = 0, res = 0;

        if( getZipInfoMerged(buffer, ref pos, ref size)) {

            inMemory t2 = new inMemory();

            GCHandle pinnedArray = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            t2.pointer = new IntPtr(pinnedArray.AddrOfPinnedObject().ToInt64() + pos);
            t2.info[0] = size;

            res = decompress_Mem2File(t2, outPath, progress, proc, password);
        
            pinnedArray.Free();
            t2.info = null;
            t2.pointer = IntPtr.Zero;
            t2 = null;
        }

        return res;
    }

    private static void writeFile(byte[] tb, string entry, string outPath, string overrideEntryName, ref int res) {
        if(tb != null) {
            string fn;

            if(overrideEntryName == null){
                if(entry.Contains("/")) {
                    string[] fileName = entry.Split('/');
                    fn = fileName[fileName.Length-1];
                } else{
                    fn = entry;
                }
            } else {
                fn = overrideEntryName;
            }
            File.WriteAllBytes(outPath + "/" + fn, tb);
            res = 1; 
        } else {
            Debug.Log("Could not extract entry.");
        }
    }

    // Extract an entry from a merged zip that resides in the file system to disk.
    // Returns 1 on success.
    //
    // file                 : the path to the file where the zip archive resides.
    // entry                : the entry to extract.
    // outPath              : the path where the entry will be extracted.
    // overrideEntryName    : with this you can override the default entry name.
    // password			    : if needed, the password to decrypt the archive.
    public static int entry2FileMerged(string file, string entry, string outPath, string overrideEntryName = null, string password = null) {
        
        if (!File.Exists(file)) return -10;
        outPath = outPath.Replace("//", "/");

        int pos = 0, size = 0, res = 0;

        var tempBuffer = getMergedZip(file, ref pos, ref size);

        if(tempBuffer != null) {

            inMemory t2 = new inMemory();
            GCHandle pinnedArray = GCHandle.Alloc(tempBuffer, GCHandleType.Pinned);
            t2.pointer = new IntPtr(pinnedArray.AddrOfPinnedObject().ToInt64() );
            t2.info[0] = size;

            var tb = entry2BufferMem(t2, entry, password);
            pinnedArray.Free();

            writeFile(tb, entry, outPath, overrideEntryName, ref res);
        
            t2.info = null;
            t2.pointer = IntPtr.Zero;
            t2 = null;
            tempBuffer = null;
            tb = null;
        }

        return res;
    }


    // Extract an entry from a merged zip that resides in a buffer to disk.
    // Returns 1 on success.
    //
    // buffer               : the buffer where the zip archive resides.
    // entry                : the entry to extract.
    // outPath              : the path where the entry will be extracted.
    // overrideEntryName    : with this you can override the default entry name.
    // password			    : if needed, the password to decrypt the archive.
    public static int entry2FileMerged(byte[] buffer, string entry, string outPath, string overrideEntryName = null, string password = null) {
        
        if (buffer == null) return -10;

        outPath = outPath.Replace("//", "/");

        int pos = 0, size = 0, res = 0;

        if( getZipInfoMerged(buffer, ref pos, ref size)) {

            inMemory t2 = new inMemory();

            GCHandle pinnedArray = GCHandle.Alloc(buffer, GCHandleType.Pinned);

            t2.pointer = new IntPtr(pinnedArray.AddrOfPinnedObject().ToInt64() + pos);
            t2.info[0] = size;

            var tb = entry2BufferMem(t2, entry, password);
            pinnedArray.Free();

            writeFile(tb, entry, outPath, overrideEntryName, ref res);

            t2.info = null;
            t2.pointer = IntPtr.Zero;
            t2 = null;
            tb = null;
        }

        return res;
    }


    // A function that extracts an entry from a zip archive that is merged/hidden in the file system and returns the extracted data in a new buffer.
    //
    // file                 : the path to the file where the zip archive resides.
    // entry                : the entry to extract.
    // password			    : if needed, the password to decrypt the archive.
    public static byte[] entry2BufferMerged(string file, string entry, string password = null) {
        
        if (!File.Exists(file)) return null;

        int pos = 0, size = 0;

        var tempBuffer = getMergedZip(file, ref pos, ref size);

        if(tempBuffer != null) {
            inMemory t2 = new inMemory();
            GCHandle pinnedArray = GCHandle.Alloc(tempBuffer, GCHandleType.Pinned);
            t2.pointer = new IntPtr(pinnedArray.AddrOfPinnedObject().ToInt64());
            t2.info[0] = size;

            var res = entry2BufferMem(t2, entry, password);
        
            pinnedArray.Free();
            t2.info = null;
            t2.pointer = IntPtr.Zero;
            t2 = null;
            tempBuffer = null;
            return res;
        }
        return null;
    }

    // A function that extracts an entry from a zip archive that is merged/hidden in the file system and returns the extracted data in a referenced buffer that will get resized to fit the data.
    //
    // file                 : the path to the file where the zip archive resides.
    // entry                : the entry to extract.
    // refBuffer            : the referenced buffer that will get resized to store the decompressed data.
    // password			    : if needed, the password to decrypt the archive.
    public static int entry2BufferMerged(string file, string entry, ref byte[] refBuffer, string password = null) {
        
        if (!File.Exists(file)) return 0;

        int pos = 0, size = 0;

        var tempBuffer = getMergedZip(file, ref pos, ref size);

        if(tempBuffer != null) {
            inMemory t2 = new inMemory();
            GCHandle pinnedArray = GCHandle.Alloc(tempBuffer, GCHandleType.Pinned);
            t2.pointer = new IntPtr(pinnedArray.AddrOfPinnedObject().ToInt64());
            t2.info[0] = size;

            var res = entry2BufferMem(t2, entry,ref refBuffer, password);
        
            pinnedArray.Free();
            t2.info = null;
            t2.pointer = IntPtr.Zero;
            t2 = null;
            tempBuffer = null;
            return res;
        }
        return 0;
    }

    // A function that extracts an entry from a zip archive that is merged/hidden in the file system and writes the extracted data in a fixed size buffer.
    //
    // file                 : the path to the file where the zip archive resides.
    // entry                : the entry to extract.
    // fixedBuffer          : the fixed sized buffer where the data will be written.
    // password			    : if needed, the password to decrypt the archive.
    public static int entry2FixedBufferMerged(string file, string entry, ref byte[] fixedBuffer, string password = null) {
        
        if (!File.Exists(file)) return 0;

        int pos = 0, size = 0;

        var tempBuffer = getMergedZip(file, ref pos, ref size);

        if(tempBuffer != null) {
            inMemory t2 = new inMemory();
            GCHandle pinnedArray = GCHandle.Alloc(tempBuffer, GCHandleType.Pinned);
            t2.pointer = new IntPtr(pinnedArray.AddrOfPinnedObject().ToInt64());
            t2.info[0] = size;

            int res = entry2FixedBufferMem(t2, entry, ref fixedBuffer, password);
        
            pinnedArray.Free();
            t2.info = null;
            t2.pointer = IntPtr.Zero;
            t2 = null;
            tempBuffer = null;
            return res;
        }
        return 0;
    }
    #endif
    
    // A function that extracts an entry from a zip archive that is merged/hidden in a buffer and returns the extracted data in a new buffer.
    //
    // buffer               : the buffer where the zip archive resides.
    // entry                : the entry to extract.
    // password			    : if needed, the password to decrypt the archive.
    public static byte[] entry2BufferMerged(byte[] buffer, string entry, string password = null) {
        
        if (buffer == null) return null;

        int pos = 0, size = 0;

        if( getZipInfoMerged(buffer, ref pos, ref size)) {
            inMemory t2 = new inMemory();
            GCHandle pinnedArray = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            t2.pointer = new IntPtr(pinnedArray.AddrOfPinnedObject().ToInt64() + pos);
            t2.info[0] = size;

            var res = entry2BufferMem(t2, entry, password);
        
            pinnedArray.Free();
            t2.info = null;
            t2.pointer = IntPtr.Zero;
            t2 = null;
            return res;
        }
        return null;
    }

    // A function that extracts an entry from a zip archive that is merged/hidden in a buffer and returns the extracted data in a referenced buffer that will get resized to fit the data.
    //
    // buffer               : the buffer where the zip archive resides.
    // entry                : the entry to extract.
    // refBuffer            : the referenced buffer that will get resized to store the decompressed data.
    // password			    : if needed, the password to decrypt the archive.
    public static int entry2BufferMerged(byte[] buffer, string entry, ref byte[] refBuffer, string password = null) {
        
        if (buffer == null) return 0;

        int pos = 0, size = 0;

        if( getZipInfoMerged(buffer, ref pos, ref size)) {
            inMemory t2 = new inMemory();
            GCHandle pinnedArray = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            t2.pointer = new IntPtr(pinnedArray.AddrOfPinnedObject().ToInt64() + pos);
            t2.info[0] = size;

            var res = entry2BufferMem(t2, entry, ref refBuffer, password);
        
            pinnedArray.Free();
            t2.info = null;
            t2.pointer = IntPtr.Zero;
            t2 = null;
            return res;
        }
        return 0;
    }

    // A function that extracts an entry from a zip archive that is merged/hidden in a buffer and writes the extracted data in a fixed size buffer.
    // Returns the size of the uncompressed data.
    //
    // buffer               : the buffer where the zip archive resides.
    // entry                : the entry to extract.
    // fixedBuffer          : the fixed sized buffer where the data will be written.
    // password			    : if needed, the password to decrypt the archive.
    // ERROR CODES
    //                      : -1 = could not initialize zip archive.
    //                      : -2 = failed extraction
    //                      :  1 = success
    public static int entry2FixedBufferMerged(byte[] buffer, string entry, ref byte[] fixedBuffer, string password = null) {
        
        if (buffer == null) return 0;

        int pos = 0, size = 0;

        if( getZipInfoMerged(buffer, ref pos, ref size)) {
            inMemory t2 = new inMemory();
            GCHandle pinnedArray = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            t2.pointer = new IntPtr(pinnedArray.AddrOfPinnedObject().ToInt64() + pos);
            t2.info[0] = size;

            int res = entry2FixedBufferMem(t2, entry, ref fixedBuffer, password);
        
            pinnedArray.Free();
            t2.info = null;
            t2.pointer = IntPtr.Zero;
            t2 = null;
            return res;
        }
        return 0;
    }
    // ---------------------------------------------------------------------------------------------------------------------------------
    //
    // END hidden/merged zip and zipInfo functions
    //
    // ---------------------------------------------------------------------------------------------------------------------------------


    //---------------------------------------------------------------------------------------------------------------------------
    // ZLIB BUFFER SECTION
    //---------------------------------------------------------------------------------------------------------------------------

    // A function that compresses a byte buffer to a zlib stream compressed buffer. Provide a reference buffer to write to. This buffer will be resized.
    //
    // source                : the input buffer
    // outBuffer             : the referenced output buffer
    // levelOfCompression    : (0-10) recommended 9 for maximum (10 is highest but slower and not zlib compatible)
    //
    // ERROR CODES   : true  = success
    //               : false = failed
    //
    public static bool compressBuffer(byte[] source, ref byte[] outBuffer, int levelOfCompression) {
        if (levelOfCompression < 0) levelOfCompression = 0; if (levelOfCompression > 10) levelOfCompression = 10;

        GCHandle sbuf = GCHandle.Alloc(source, GCHandleType.Pinned);
        IntPtr ptr;
        int siz = 0;

        ptr = zipCompressBuffer(sbuf.AddrOfPinnedObject(), source.Length, levelOfCompression, ref siz);

        if (siz == 0 || ptr == IntPtr.Zero) { sbuf.Free(); releaseBuffer(ptr); return false; }

        System.Array.Resize(ref outBuffer, siz);
        Marshal.Copy(ptr, outBuffer, 0, siz);

        sbuf.Free();
        releaseBuffer(ptr);

        return true;
    }


	// same as the compressBuffer function, only this function will put the result in a fixed size buffer to avoid memory allocations.
	// the compressed size is returned so you can manipulate it at will.
	//
	// safe: if set to true the function will abort if the compressed resut is larger the the fixed size output buffer.
	// Otherwise compressed data will be written only until the end of the fixed output buffer.
	//
	public static int compressBufferFixed(byte[] source, ref byte[] outBuffer, int levelOfCompression, bool safe = true) {
        if (levelOfCompression < 0) levelOfCompression = 0; if (levelOfCompression > 10) levelOfCompression = 10;

        GCHandle sbuf = GCHandle.Alloc(source, GCHandleType.Pinned);
        IntPtr ptr;
        int siz = 0;

        ptr = zipCompressBuffer(sbuf.AddrOfPinnedObject(), source.Length, levelOfCompression, ref siz);

        if (siz == 0 || ptr == IntPtr.Zero) { sbuf.Free(); releaseBuffer(ptr); return 0; }

		if (siz>outBuffer.Length) {
			if(safe) { sbuf.Free(); releaseBuffer(ptr); return 0; } else { siz = outBuffer.Length; }
		}

        Marshal.Copy(ptr, outBuffer, 0, siz);

        sbuf.Free();
        releaseBuffer(ptr);

        return siz;
    }


    // A function that compresses a byte buffer to a zlib stream compressed buffer. Returns a new buffer with the compressed data.
    //
    // source                : the input buffer
    // levelOfCompression    : (0-10) recommended 9 for maximum (10 is highest but slower and not zlib compatible)
    //
    // ERROR CODES           : a valid byte buffer = success
    //                       : null                = failed
    //
    public static byte[] compressBuffer(byte[] source,  int levelOfCompression) {
        if (levelOfCompression < 0) levelOfCompression = 0; if (levelOfCompression > 10) levelOfCompression = 10;

        GCHandle sbuf = GCHandle.Alloc(source, GCHandleType.Pinned);
        IntPtr ptr;
        int siz = 0;

        ptr = zipCompressBuffer(sbuf.AddrOfPinnedObject(), source.Length, levelOfCompression, ref siz);

        if (siz == 0 || ptr == IntPtr.Zero) { sbuf.Free(); releaseBuffer(ptr); return null; }

        byte[] buffer = new byte[siz];
        Marshal.Copy(ptr, buffer, 0, siz);

        sbuf.Free();
        releaseBuffer(ptr);

        return buffer;
    }


    // A function that decompresses a zlib compressed buffer to a referenced outBuffer. The outbuffer will be resized.
    //
    // source            : a zlib compressed buffer.
    // outBuffer         : a referenced out buffer provided to extract the data. This buffer will be resized to fit the uncompressed data.
    //
    // ERROR CODES       : true  = success
    //                   : false = failed
    //
    public static bool decompressBuffer(byte[] source, ref byte[] outBuffer) {
        GCHandle sbuf = GCHandle.Alloc(source, GCHandleType.Pinned);
        IntPtr ptr;
        int siz = 0;

        ptr = zipDecompressBuffer(sbuf.AddrOfPinnedObject(), source.Length, ref siz);

        if (siz == 0 || ptr == IntPtr.Zero) { sbuf.Free(); releaseBuffer(ptr); return false; }

        System.Array.Resize(ref outBuffer, siz);
        Marshal.Copy(ptr, outBuffer, 0, siz);

        sbuf.Free();
        releaseBuffer(ptr);

        return true;
    }


	// same as the decompressBuffer function. Only this one outputs to a buffer of fixed which size isn't resized to avoid memory allocations.
	// The fixed buffer should have a size that will be able to hold the incoming decompressed data.
	// Returns the uncompressed size.
	//
	// safe: if set to true the function will abort if the decompressed resut is larger the the fixed size output buffer.
	// Otherwise decompressed data will be written only until the end of the fixed output buffer.
	//
    public static int decompressBufferFixed(byte[] source, ref byte[] outBuffer, bool safe = true) {
        GCHandle sbuf = GCHandle.Alloc(source, GCHandleType.Pinned);
        IntPtr ptr;
        int siz = 0;

        ptr = zipDecompressBuffer(sbuf.AddrOfPinnedObject(), source.Length, ref siz);

        if (siz == 0 || ptr == IntPtr.Zero) { sbuf.Free(); releaseBuffer(ptr); return 0; }

		if (siz>outBuffer.Length) {
			if(safe) { sbuf.Free(); releaseBuffer(ptr); return 0; } else { siz = outBuffer.Length; }
		}

        Marshal.Copy(ptr, outBuffer, 0, siz);

        sbuf.Free();
        releaseBuffer(ptr);

        return siz;
    }


    // A function that decompresses a zlib compressed buffer and creates a new buffer.  Returns a new buffer with the uncompressed data.
    //
    // source                : a zlib compressed buffer.
    //
    // ERROR CODES           : a valid byte buffer = success
    //                       : null                = failed
    //
    public static byte[] decompressBuffer(byte[] source) {
        GCHandle sbuf = GCHandle.Alloc(source, GCHandleType.Pinned);
        IntPtr ptr;
        int siz = 0;

        ptr = zipDecompressBuffer(sbuf.AddrOfPinnedObject(), source.Length, ref siz);

        if (siz == 0 || ptr == IntPtr.Zero) { sbuf.Free(); releaseBuffer(ptr); return null; }

        byte[] buffer = new byte[siz];
        Marshal.Copy(ptr, buffer, 0, siz);

        sbuf.Free();
        releaseBuffer(ptr);

        return buffer;
    }
    //---------------------------------------------------------------------------------------------------------------------------
    // END ZLIB BUFFER SECTION
    //---------------------------------------------------------------------------------------------------------------------------


    //---------------------------------------------------------------------------------------------------------------------------
    // GZIP SECTION
    //---------------------------------------------------------------------------------------------------------------------------

    // compress a byte buffer to gzip format.
    //
    // returns the size of the compressed buffer.
    //
    // source:		                the uncompressed input buffer.
    // outBuffer:	                the provided output buffer where the compressed data will be stored (it should be at least the size of the input buffer +18 bytes).
    // level:		                the level of compression (0-9)
    // addHeader:	                if a gzip header should be added. (recommended if you want to write out a gzip file)
    // addFooter:	                if a gzip footer should be added. (recommended if you want to write out a gzip file)
    // overrideDateTimeWithLength:  use the bytes 5-8 of the header to store the gzip length instead of DateTime modification. This useful when you want to know the
    //                              compresed size of a gzip buffer. Then use the gzipCompressedSize function to get this size.
    public static int gzip(byte[] source, byte[] outBuffer, int level, bool addHeader = true, bool addFooter = true, bool overrideDateTimeWithLength = false) {
        if(source == null || outBuffer == null) return 0;
		GCHandle sbuf = GCHandle.Alloc(source, GCHandleType.Pinned);
		GCHandle dbuf = GCHandle.Alloc(outBuffer, GCHandleType.Pinned);

		int res = zipGzip(sbuf.AddrOfPinnedObject(), source.Length, dbuf.AddrOfPinnedObject(), level, addHeader, addFooter);

		sbuf.Free(); dbuf.Free();
		int hf = 0;
		if(addHeader) hf += 10;
		if(addFooter) hf += 8;

        int compressedSize = res + hf;

        if(addHeader && overrideDateTimeWithLength) {
        	outBuffer[4] = (byte)(((uint)compressedSize >> 0)  & 0xff);
		    outBuffer[5] = (byte)(((uint)compressedSize >> 8)  & 0xff);
		    outBuffer[6] = (byte)(((uint)compressedSize >> 16) & 0xff);
		    outBuffer[7] = (byte)(((uint)compressedSize >> 24) & 0xff);
            // use the operating system flag to mark this gzip that it holds the compressed data size.
            outBuffer[9] = 0xfe;
        }
		return compressedSize;
	}


	// get the uncompressed size from a gzip buffer that has a footer included
	//
	// source:		the gzip compressed input buffer. (it should have at least a gzip footer)
	public static int gzipUncompressedSize(byte[] source) {
        if (source == null) return 0;
		int res = source.Length;
		uint size = ((uint)source[res-4] & 0xff) |
				    ((uint)source[res-3] & 0xff) << 8 |
				    ((uint)source[res-2] & 0xff) << 16 |
				    ((uint)source[res-1]& 0xff) << 24;
		return (int)size;
	}

    // get the compressed size of a gzip, if the compressed size of it has been written in the date header bytes and marked as such, with the gzip function above.
    //
    // source:		the gzip compressed input buffer.
	public static int gzipCompressedSize(byte[] source, int offset = 0) {
        if (source == null) return 0;

        if(source[offset + 9] != 0xfe){
            Debug.Log("Gzip has not been marked to have compressed size stored.");
            return 0;
        }

		int res = offset + 8;

		uint size = ((uint)source[res-4] & 0xff) |
				    ((uint)source[res-3] & 0xff) << 8 |
				    ((uint)source[res-2] & 0xff) << 16 |
				    ((uint)source[res-1]& 0xff) << 24;
		return (int)size;
	}
    
    // find where the merged gzip starts in a buffer.
    //
    // buffer: a memory buffer that has a gzip merged at the end of it.
	public static int findGzStart(byte[] buffer) {
        if(buffer == null) return 0;
        int res = 0;
        int i = 0;

        while (i < buffer.Length - 2) {
            if (buffer[i] == 0x1f) {
                if(buffer[i+1] == 0x8b && buffer[i+2] == 0x08 ) { res = i; break; }
            }
            i++;
        }
        return res;
    }

	// decompress a gzip buffer
	//
	// returns:		uncompressed size. negative error code on error. 
	//
	// source:		the gzip compressed input buffer.
	// outBuffer:	the provided output buffer where the uncompressed data will be stored.
	// hasHeader:	if the buffer has a header.
	// hasFooter:	if the buffer has a footer.
	//
	public static int unGzip(byte[] source, byte[] outBuffer, bool hasHeader = true, bool hasFooter = true) {
        if(source == null || outBuffer == null) return 0;
		GCHandle sbuf = GCHandle.Alloc(source, GCHandleType.Pinned);
		GCHandle dbuf = GCHandle.Alloc(outBuffer, GCHandleType.Pinned);
		
		int res = zipUnGzip( sbuf.AddrOfPinnedObject(), source.Length, dbuf.AddrOfPinnedObject(),outBuffer.Length, hasHeader, hasFooter );

		sbuf.Free(); dbuf.Free();
		return res;				
	}

	// decompress a gzip buffer (This function assumes that the gzip buffer has a gzip header !!!)
	//
	// returns:		uncompressed size. negative error code on error. 
	//
	// source:		the gzip compressed input buffer.
	// outBuffer:	the provided output buffer where the uncompressed data will be stored.
	//
	public static int unGzip2(byte[] source, byte[] outBuffer) {
        if(source == null || outBuffer == null) return 0;
		GCHandle sbuf = GCHandle.Alloc(source, GCHandleType.Pinned);
		GCHandle dbuf = GCHandle.Alloc(outBuffer, GCHandleType.Pinned);

		int res = zipUnGzip2( sbuf.AddrOfPinnedObject(), source.Length, dbuf.AddrOfPinnedObject(), outBuffer.Length );

		sbuf.Free(); dbuf.Free();
		return res;				
	}

	// decompress a gzip buffer that is merged in the end of a buffer (This function assumes that the gzip buffer has a gzip header !!!)
	//
	// returns:		    uncompressed size. negative error code on error. 
	//
	// source:		    the buffer where the gzip compressed input buffer resides. (at the end of it, or anywhere if you know the length of it)
    // offset:          the offset in bytes where the gzip starts.
    // bufferLength:    the length of the gzip buffer.
	// outBuffer:	    the provided output buffer where the uncompressed data will be stored.
	//
	public static int unGzip2Merged(byte[] source, int offset, int bufferLength, byte[] outBuffer) {
        if(source == null || outBuffer == null) return 0;
        if(bufferLength == 0 ) return 0;

		GCHandle sbuf = GCHandle.Alloc(source, GCHandleType.Pinned);
		GCHandle dbuf = GCHandle.Alloc(outBuffer, GCHandleType.Pinned);

        IntPtr p = new IntPtr(sbuf.AddrOfPinnedObject().ToInt64() + offset);

		int res = zipUnGzip2( p, bufferLength, dbuf.AddrOfPinnedObject(), outBuffer.Length );

		sbuf.Free(); dbuf.Free();
		return res;				
	}
    //---------------------------------------------------------------------------------------------------------------------------
    // END GZIP SECTION
    //---------------------------------------------------------------------------------------------------------------------------


#endif
}
