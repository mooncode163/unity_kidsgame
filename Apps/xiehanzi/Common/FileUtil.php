<?php 
class FileUtil
{

    private static $main;

    public static function Main()
    {
        if (!self::$main) {
            self::$main = new FileUtil();
        }
    }
    function __construct()
    { }


    public function FileIsExist($filepath)
    {
        $ret = file_exists($filepath);
        return $ret;
    }
}
