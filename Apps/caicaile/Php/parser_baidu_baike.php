<?php
header("Content-type: text/html; charset=utf-8");

//include('./simple_html_dom.php');
include('./parser_poem_item.php');

$is_find_poem = false;
$listPoem = array();
$indexPoem = 0;
/*
    百度百科 唐诗三百首
    https://baike.baidu.com/item/%E5%94%90%E8%AF%97%E4%B8%89%E7%99%BE%E9%A6%96/18677

    item:
    https://baike.baidu.com/item/%E8%B4%BC%E9%80%80%E7%A4%BA%E5%AE%98%E5%90%8F/6960505?fromtitle=%E8%B4%BC%E9%80%80%E7%A4%BA%E5%AE%98%E5%90%8F%C2%B7%E5%B9%B6%E5%BA%8F&fromid=9424895
    
    */

function IsPoemInList($element)
{
    global $listPoem;
    foreach ($listPoem as $item) {
        if ($item["id"]  == $element["id"]) {
            return true;
        }
    }
    return false;
}
function AddPoem($element)
{
    global $listPoem;
    if (IsPoemInList($element)) {
        return;
    }
    array_push($listPoem, $element);
}



function GetPoemItem($div)
{
    global $is_find_poem;
    global $listPoem;
    global $indexPoem;

    $a = $div->find('a[target=_blank]', 0);
    if (!$a) {
        //  echo "find array_a fail \n";
        return;
    }
    //<b>五言古诗</b>
    // foreach ($array_a as $a) 
    {
        $b = $a->find('b', 0);
        if ($b) {
            $is_find_poem = true;
            // echo $a->innertext;
            echo $b->innertext;
            echo "\n";
        } else {
            if ($is_find_poem) {
                $url = "https://baike.baidu.com" . $a->href;
                $id = $a->innertext;

                // echo $url;
                // echo "\n";


                $element = array(
                    'id' => urlencode($id),
                    'url' => urlencode($url),
                );


                // $filepath = "poem/" . $id . $indexPoem . ".json";
                $filepath = "poem/" . $id . ".json";
                if (!file_exists($filepath)) {
                    $ret =  ParserPoemItem($url, $filepath);
                    if (!$ret) {
                        echo "  error url:" . $url . "\n";
                    } else {
                        AddPoem($element);
                    }
                } else {
                    AddPoem($element);
                }


                if ($a->innertext == "同题仙游观") {
                    //end
                    $is_find_poem = false;
                }
            }
        }
    }
}
function parserHtml($url, $save_file)
{
    // get DOM from URL or file
    $html = get_html($url);
    if (!$html) {
        echo "open html fail";
        return;
    }
    //<table class="table media-table js-media-details">
    $div_main = $html->find('div[class=main-content]', 0);
    if (!$div_main) {
        echo "find div_main fail";
        return;
    }
    echo "find div_main\n\n";


    //解析广告位

    //tbody/tr
    $array_div = $div_main->find('div[class=para]');

    if (!$array_div) {
        echo "find array_div fail";
        return;
    }
    global $indexPoem;
    $indexPoem = 0;
    foreach ($array_div as $div) { {
            GetPoemItem($div);
            $indexPoem++;
        }
    }



    global $listPoem;
    echo "Count = " . count($listPoem) . "\n";
    $arr = array('Item' => $listPoem);
    $jsn = urldecode(json_encode($arr));

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
}



parserHtml('https://baike.baidu.com/item/%E5%94%90%E8%AF%97%E4%B8%89%E7%99%BE%E9%A6%96/18677', "poem.json");
//parserAd('../gdt_hd.html',"../gdt_hd.json");
echo 'done<br>';
