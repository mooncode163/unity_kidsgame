using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine;
public class GuankaJsonItemInfo
{
    //public string pic;//0,0,100,100
    public string id;
}
public class AutoGuankaJson : ScriptBase
{
    List<GuankaJsonItemInfo> listGuankaJson;

    List<ItemInfo> listImage;
    string dirRootImage;
    string dirRootImageThumb;
    string dirRootGuanka;
    // Use this for initialization
    void Start()
    {
        listImage = new List<ItemInfo>();
        List<object> listPlace = GameLevelParse.main.listPlace;
        dirRootImage = Application.streamingAssetsPath + "/" + CloudRes.main.rootPathGameRes +"/image/";
        dirRootImageThumb = Application.streamingAssetsPath + "/" + CloudRes.main.rootPathGameRes +"/image_thumb/";

        dirRootGuanka = Application.streamingAssetsPath + "/" + CloudRes.main.rootPathGameRes +"/guanka_new/";
        LevelManager.main.ParsePlaceList();
        for (int i = 0; i < listPlace.Count; i++)
        {
            ItemInfo infoPlace = listPlace[i] as ItemInfo;
            ItemInfo info = new ItemInfo();
            info.id = infoPlace.id;
            listImage.Add(info);
        }




    }

    // Update is called once per frame
    void Update()
    {

    }


    void ConvertImage(string pathImgae, string savedir, GuankaJsonItemInfo info, string ext, int w, int h)
    {
        Texture2D tex = LoadTexture.LoadFromFile(pathImgae);
        float scale = 1f;
        {
            scale = Common.GetBestFitScale(tex.width, tex.height, w, h);
            w = (int)(tex.width * scale);
            h = (int)(tex.height * scale);
            Debug.Log("autoguankajson scale=" + scale + " tex.width=" + tex.width);
            Texture2D texNew = TextureUtil.ConvertSize(tex, w, h);
            FileUtil.CreateDir(savedir);
            string filepath_new = savedir + "/" + info.id + "." + ext;
            TextureUtil.SaveTextureToFile(texNew, filepath_new);
        }
    }

    void CreateGuankaJsonFile(ItemInfo infoImage)
    {
        string path = dirRootImage + infoImage.id;
        string pathThumb = dirRootImageThumb + infoImage.id;
        string strPlace = infoImage.id;
        //  string path = Application.streamingAssetsPath + "/" + CloudRes.main.rootPathGameRes +"/image/" + strPlace;

        // int width_save = 1024;
        // int height_save = 768;

        string strLanguageContent = "KEY,CN,EN\n";
        listGuankaJson = new List<GuankaJsonItemInfo>();
        // C#遍历指定文件夹中的所有文件 
        DirectoryInfo TheFolder = new DirectoryInfo(path);
        int idx = 0;
        // //遍历文件
        foreach (FileInfo NextFile in TheFolder.GetFiles())
        {
            string fullpath = NextFile.ToString();
            //1.jpg
            // Debug.Log(NextFile.Name);
            string ext = FileUtil.GetFileExt(fullpath);
            if ((ext == "png") || (ext == "jpg"))
            {
                string name = idx.ToString() + "." + ext;
                string filepath_new = NextFile.ToString();
                GuankaJsonItemInfo info = new GuankaJsonItemInfo();
                //info.pic = NextFile.Name;
                info.id = FileUtil.GetFileName(NextFile.Name);
                //将网上下载的图片 统一调整分辨率
                //
                ConvertImage(fullpath, path, info, ext, 1024, 1024);

                //thumb
                ConvertImage(fullpath, pathThumb, info, ext, 256, 256);


                //重命名
                //filepath_new = path + "/" + name;
                // NextFile.MoveTo(filepath_new);


                listGuankaJson.Add(info);
                strLanguageContent += info.id + ",,\n";

                idx++;
            }

        }
        string pathGuanka = dirRootGuanka;
        //创建文件夹 
        FileUtil.CreateDir(pathGuanka);
        //save guanka json
        {

            Hashtable data = new Hashtable();
            data["type"] = strPlace;
            data["items"] = listGuankaJson;
            string strJson = JsonMapper.ToJson(data);
            //Debug.Log(strJson);
            string filepath = pathGuanka + "/item_" + strPlace + ".json";
            byte[] bytes = Encoding.UTF8.GetBytes(strJson);
            System.IO.File.WriteAllBytes(filepath, bytes);
        }

        //save language
        {
            string filepath = pathGuanka + "/" + strPlace + ".csv";
            SaveString(filepath, strLanguageContent);
        }

        Debug.Log("CreateGuankaJsonFile Finished");

    }

    public void SaveString(string Path, string content)
    {
        FileStream aFile = new FileStream(Path, FileMode.OpenOrCreate);
        StreamWriter sw = new StreamWriter(aFile);
        sw.Write(content);
        sw.Close();
        sw.Dispose();
    }
    public void OnClickBtnGuanka()
    {

        foreach (ItemInfo pic in listImage)
        {
            CreateGuankaJsonFile(pic);
        }
    }



}
