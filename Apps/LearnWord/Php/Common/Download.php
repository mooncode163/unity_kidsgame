<?php
class Download
{

    private  static $main;
    public static function Main()
    {
        if (!self::$main) {
            self::$main = new Download();
        }
        return self::$main;
    }
    function __construct()
    { }


    public function DownloadFile($url, $savepath)
    {
        $ch = curl_init($url);
        $fp = fopen($savepath, "w");
        curl_setopt($ch, CURLOPT_FILE, $fp);
        curl_setopt($ch, CURLOPT_HEADER, 0);
        curl_exec($ch);
        curl_close($ch);
        fclose($fp);
    }
}
