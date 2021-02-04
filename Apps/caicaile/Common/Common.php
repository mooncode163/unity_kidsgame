<?php
include_once('Html/simple_html_dom.php');
function FileIsExist($filepath)
{
    $ret = file_exists($filepath);
    return $ret;
}

function BlankString($str)
{
    if (!is_string($str)) {
        return false; //判断是否是字符串类型
    }
    // if (empty($str)) { //判断是否已定义字符串
    //     echo "BlankString empty ";
    //     return true;
    // }
    if ($str == "") {
        // echo "BlankString null ";
        return true;
    }

    return false;
}

function RemoveHtmlSpace($str)
{
    $ret = str_replace("&nbsp;", "", $str);
    $ret = str_replace(" ", "", $ret);
    $ret = DeleteHtml($ret);
    return  $ret;
}

//实用的php清除html,换行，空格类,php去除空格与换行,php清除空白行和换行,提取页面纯文本内容
function DeleteHtml($str) 
{ 
    $str = trim($str); //清除字符串两边的空格
    $str = strip_tags($str,""); //利用php自带的函数清除html格式
    $str = preg_replace("/\t/","",$str); //使用正则表达式替换内容，如：空格，换行，并将替换为空。
    $str = preg_replace("/\r\n/","",$str); 
    $str = preg_replace("/\r/","",$str); 
    $str = preg_replace("/\n/","",$str); 
    $str = preg_replace("/ /","",$str);
    $str = preg_replace("/  /","",$str);  //匹配html中的空格
    return trim($str); //返回字符串
}


function get_html($url)
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
function strip_html_tags($tags, $str)
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
