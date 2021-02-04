<?php
header("Content-type: text/html; charset=utf-8");

include('./parser_chengyu_item.php');


function PaserList()
{
    $json_string = file_get_contents('chengyu_list.json');
    $data = json_decode($json_string, true);
    $array = $data['items'];

    $save_dir = "chengyu";
    if (!is_dir($save_dir)) {
        mkdir($save_dir);
    }

    foreach ($array as $item) {
        $url = $item['url'];
        if (!BlankString($url)) {
            ParserChengyuItem($url, $save_dir . "/" . $item['id'] . ".json");
        }
    }
}

PaserList();
echo 'done<br>';
