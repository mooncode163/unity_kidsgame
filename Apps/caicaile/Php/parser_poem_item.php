<?php
header("Content-type: text/html; charset=utf-8");

include('./simple_html_dom.php');

define("NAME_YUANWEN", "作品原文");
define("NAME_TRANSLATION", "白话译文");
define("NAME_JIANSHUANG", "整体赏析");
define("NAME_JIANSHUANG2", "文学赏析");
define("NAME_AUTHOR", "作者简介");


define("INFO_TITLE", "作品名称");
define("INFO_AUTHOR", "作者");
define("INFO_YEAR", "创作年代");
define("INFO_STYLE", "文学体裁");
define("INFO_ALBUM", "作品出处");

$is_find_poem = false;
$gTitle = "";
$gYear = "";
$gStyle = "";
$gAlbum = "";
$gAuthor = "";
$gUrl = "";
$gIntro = "";
$gYuanwen = "";
$gTranslation = "";
$gAuthorIntro = "";
$gJianShuang = "";

$listPoemContent = array();

// "title": "送别",
// "author": "王维",
// "year": "唐代",
// "style": "五言古诗",
// "album": "全唐诗",
/*
    百度百科 唐诗三百首
    https://baike.baidu.com/item/%E5%94%90%E8%AF%97%E4%B8%89%E7%99%BE%E9%A6%96/18677

    item:
    https://baike.baidu.com/item/%E8%B4%BC%E9%80%80%E7%A4%BA%E5%AE%98%E5%90%8F/6960505?fromtitle=%E8%B4%BC%E9%80%80%E7%A4%BA%E5%AE%98%E5%90%8F%C2%B7%E5%B9%B6%E5%BA%8F&fromid=9424895
    
    */

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
        echo "BlankString null ";
        return true;
    }

    return false;
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


function ParserPoemInfo($html, $type)
{
    $str_ret = "";
    $div_main_left = $html->find('dl[class=basicInfo-block basicInfo-left]', 0);
    if (!$div_main_left) {
        echo "ParserPoemInfo find div_main fail\n";
        return;
    }

    $div_main_right = $html->find('dl[class=basicInfo-block basicInfo-right]', 0);
    if (!$div_main_right) {
        echo "ParserPoemInfo find div_main fail";
        return;
    }

    $listtmp = array();
    foreach ($div_main_left->children as $children) {
        array_push($listtmp, $children);
    }
    foreach ($div_main_right->children as $children) {
        array_push($listtmp, $children);
    }


    $is_find_item = false;
    foreach ($listtmp as $children) {
        if ($children->class == "basicInfo-item name") {
            $str = $children->innertext;
            $str = removeHtmlSpace($str);
            if ($str == $type) {
                //  echo "ParserPoemInfo is_find_item \n";
                $is_find_item = true;
            }
        }
        if ($is_find_item) {
            if ($children->class == "basicInfo-item value") {
                $str = $children->innertext;
                //删除标签
                $str = strip_tags($str);
                $str = removeHtmlSpace($str);
                $str_ret = $str;
                echo $type . ":" . $str;
                echo "\n";
                break;
            }
        }
    }

    return $str_ret;
}

function parserIntro($html)
{
    global $gIntro;


    $div_main = $html->find('div[class=lemma-summary]', 0);
    if (!$div_main) {
        echo "parserIntro find div_main fail\n";
        return;
    }
    $div_intro = $div_main->find('div[class=para]', 0);
    if ($div_intro) {
        $str = $div_intro->innertext;
        $str = strip_tags($str);
        $gIntro = $str;
        echo $str;
        echo "\n";
    }
}


function parserContent($html, $name)
{
    $listContent = array();
    $str_ret = "";
    //<table class="table media-table js-media-details">
    $div_main = $html->find('div[class=main-content]', 0);
    if (!$div_main) {
        echo "find div_main fail";
        return;
    }

    //原文
    //start <div class="anchor-list">
    //end  <div class="anchor-list">
    $class_start = "anchor-list";
    $isFindItem = false;
    $indexClass = 0;

    $array_div = $div_main->find('div');
    if (!$array_div) {
        echo "find array_div fail";
        return;
    }

    $is_find_content = false;
    foreach ($array_div as $div) {
        if ($div->class ==  $class_start) {
            $str = $div->innertext;
            $str_name = strstr($str, $name);

            if ($str_name == false) {
                if ($is_find_content) {
                    //find end
                    break;
                }
                $isFindItem = false;
            } else {
                $isFindItem = true;
            }

            if ($isFindItem) {
                // echo $str;
                //  echo "\n";
            }

            $indexClass++;
        }
        /*
tag – 返回html标签名

innertext – 返回innerHTML

outertext – 返回outerHTML

plaintext – 返回html标签中的文本
*/

        if ($isFindItem) {
            if ($div->class ==  "para") {
                // echo $div->plaintext;
                $str = $div->innertext;
                //删除相关标签和内容
                if ($name == constant("NAME_YUANWEN")) {
                    $str = strip_html_tags(array('sup', 'a', 'b'), $str);
                } else {

                    $str = strip_html_tags(array('sup', 'b'), $str);
                }

                //删除标签:i
                $str = strip_tags($str);
                $str = removeHtmlSpace($str);
                $str_ret = $str_ret . $str;
                $is_find_content = true;
                if (!BlankString($str)) {
                    array_push($listContent, $str);
                }
            }
        }
    }
    echo $str_ret;
    echo "\n";
    return $listContent;
}
function parserYuanwen($html)
{
    global $gYuanwen;
    global $listPoemContent;

    $listContent  = parserContent($html,  constant("NAME_YUANWEN"));

    //清空数组

    array_splice($listPoemContent, 0, count($listPoemContent));

    foreach ($listContent as $str) {
        $str_new = str_replace(" ", "", $str);
        array_push($listPoemContent, $str_new);
        $gYuanwen = $gYuanwen . $str;
    }
}

//译文

function parserTranslation($html)
{
    global $gTranslation;
    $listContent  = parserContent($html,  constant("NAME_TRANSLATION"));
    $index = 0;
    foreach ($listContent as $str) {
        if ((count($listContent) - 1) == $index) {
            $gTranslation = $gTranslation . $str;
        } else {
            $gTranslation = $gTranslation . $str . "\\n";
        }
        $index++;
    }
}

//鉴赏

function parserJianShuang($html)
{
    global $gJianShuang;
    $listContent  = parserContent($html,  constant("NAME_JIANSHUANG"));
    $index = 0;
    foreach ($listContent as $str) {
        if ((count($listContent) - 1) == $index) {
            $gJianShuang = $gJianShuang . $str;
        } else {
            $gJianShuang = $gJianShuang . $str . "\\n";
        }
        $index++;
    }
    $index = 0;
    $listContent  = parserContent($html,  constant("NAME_JIANSHUANG2"));
    foreach ($listContent as $str) {
        if ((count($listContent) - 1) == $index) {
            $gJianShuang = $gJianShuang . $str;
        } else {
            $gJianShuang = $gJianShuang . $str . "\\n";
        }
        $index++;
    }
}

//作者简介
function parserAuthor($html)
{
    global $gAuthorIntro;
    $listContent  = parserContent($html,  constant("NAME_AUTHOR"));
    $index = 0;
    foreach ($listContent as $str) {
        if ((count($listContent) - 1) == $index) {
            $gAuthorIntro = $gAuthorIntro . $str;
        } else {
            $gAuthorIntro = $gAuthorIntro . $str . "\\n";
        }
        $index++;
    }
}

function GetJsonStrOfPoem()
{
    global $listPoemContent;
    $listtmp = array();

    foreach ($listPoemContent as $str) {
        $element = array(
            'content' => urlencode($str),
            'pinyin' => urlencode(""),
            'skip' => urlencode(false),
        );
        array_push($listtmp, $element);
    }
    $jsn = urldecode(json_encode($listtmp));

    echo "\n\n jsn =" . $jsn . "\n";

    return $jsn;
}
//poem
function ParserPoemItem($url, $save_file)
{
    global $gUrl;
    global $gTitle;
    global $gYear;
    global $gStyle;
    global $gAlbum;
    global $gAuthor;
    global $gIntro;
    global $gTranslation;
    global $gJianShuang;
    global $gYuanwen;
    global $gAuthorIntro;


    $gIntro = "";
    $gTranslation = "";
    $gJianShuang = "";
    $gYuanwen = "";
    $gAuthorIntro = "";

    $gUrl = $url;

    // get DOM from URL or file
    $html = get_html($url);
    if (!$html) {
        echo "open html fail\n";
        return false;
    }
    $gTitle = ParserPoemInfo($html, constant("INFO_TITLE"));
    $gYear = ParserPoemInfo($html, constant("INFO_YEAR"));
    $gStyle = ParserPoemInfo($html, constant("INFO_STYLE"));

    //《全唐诗》
    $gAlbum = ParserPoemInfo($html, constant("INFO_ALBUM"));
    $gAlbum = str_replace("《", "", $gAlbum);
    $gAlbum = str_replace("》", "", $gAlbum);


    $gAuthor = ParserPoemInfo($html, constant("INFO_AUTHOR"));

    if ($gTitle == "") {
        return false;
    }
    echo "\n\n year:" . $gYear . "\n";
    parserIntro($html);
    echo "\n\n";
    parserYuanwen($html);
    echo "\n\n";
    parserTranslation($html);
    echo "\n\n";
    parserJianShuang($html);
    echo "\n\n";
    parserAuthor($html);
    echo "\n\n";

    $listPoem = array();


    $element = array(
        'title' => urlencode($gTitle),
        'author' => urlencode($gAuthor),
        'year' => urlencode($gYear),
        'style' => urlencode($gStyle),
        'album' => urlencode($gAlbum),
        'url' => urlencode($gUrl),

        'poem' => urlencode(GetJsonStrOfPoem()),

        'intro' => urlencode($gIntro),
        'translation' => urlencode($gTranslation),
        'appreciation' => urlencode($gJianShuang),



    );
    array_push($listPoem, $element);
    //$arr = array('Item' => $listPoem);
    $jsn = urldecode(json_encode($element));

    // "[  ]"
    $jsn = str_replace("\"[", "[", $jsn);
    $jsn = str_replace("]\"", "]", $jsn);

    $fp = fopen($save_file, "w");
    if (!$fp) {
        echo "打开文件失败<br>";
        return;
    }
    $flag = fwrite($fp, $jsn);
    if (!$flag) {
        echo "写入文件失败<br>";
    }
    fclose($fp);

    return true;
}

$url = "https://baike.baidu.com/item/%E5%90%8C%E4%BB%8E%E5%BC%9F%E5%8D%97%E6%96%8B%E7%8E%A9%E6%9C%88%E5%BF%86%E5%B1%B1%E9%98%B4%E5%B4%94%E5%B0%91%E5%BA%9C";
//$url = "https://baike.baidu.com/item/%E8%B4%BC%E9%80%80%E7%A4%BA%E5%AE%98%E5%90%8F%C2%B7%E5%B9%B6%E5%BA%8F";

//ng:
/*
https://baike.baidu.com/item/%E8%B4%AB%E5%A5%B3
*/

ParserPoemItem($url, "poem_item.json");
echo 'done<br>';
