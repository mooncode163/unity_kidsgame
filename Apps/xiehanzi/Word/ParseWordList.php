<?php
header("Content-type: text/html; charset=utf-8");

include('../Common/HtmlUtil.php');
include('WordItemInfo.php');
include('ParseWordItem.php');

// include('../Common/Common.php');


class ParseWordList
{

    //poem
    public  function ParseList()
    {
        $json_file = "Data/word_list.json";
        $fiel_exist = file_exists($json_file);
        if ($fiel_exist) {
            $json_string = file_get_contents($json_file);
            $root = json_decode($json_string, true);
            $data = $root['items'];
            foreach ($data as $item) {
                $word =  $item['word']; 
                echo $word . "\n";
                $parseitem = new ParseWordItem();
                $parseitem->ParseWordItemInfo($word);
            }
        }
    }
}






$p = new ParseWordList();
$p->ParseList();

echo 'done<br>';
