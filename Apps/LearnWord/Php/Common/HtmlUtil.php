<?php

include('Html/simple_html_dom.php');

class HtmlUtil
{

    private static $main;

    public static function Main()
    {
        if (!self::$main) {
            self::$main = new HtmlUtil();
        }
        return self::$main;
    }
    function __construct()
    {
    }

    public function get_html($url)
    {
        $html = new simple_html_dom();

        // // 从url中加载  
        // $html->load_file('http://www.jb51.net');  

        // // 从字符串中加载  
        // $html->load('<html><body>从字符串中加载html文档演示</body></html>');  

        //从文件中加载  
        $html->load_file($url);

        return $html;
    }

    /*
删除HTML标签和标签的内容。

使用方法：strip_html_tags($tags,$str)

$tags：需要删除的标签（数组格式）
$str：需要处理的字符串；
*/
    public function strip_html_tags($tags, $str)
    {
        $html = array();
        foreach ($tags as $tag) {
            $html[] = '/<' . $tag . '.*?>[\s|\S]*?<\/' . $tag . '>/';
            $html[] = '/<' . $tag . '.*?>/';
        }
        $data = preg_replace($html, '', $str);
        return $data;
    }
    //

    //删除标签
    public function strip_tags($str)
    {
        //删除标签 
        return strip_tags($str);
    }
    function RemoveHtmlSpace($str)
    {
        $ret = str_replace("&nbsp;", "", $str);
        $ret = str_replace(" ", "", $ret);
        return  $ret;
    }
}
