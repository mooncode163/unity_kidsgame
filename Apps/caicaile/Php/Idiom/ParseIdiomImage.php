<?php
header("Content-type: text/html; charset=utf-8");

include('ParseIdiomItem.php');
include_once('IdiomItemInfo.php');
include_once('../Common/Download.php');
include_once('../Common/Common.php');


class IdiomImageParser //extends Thread
{

    public $url;
    public $id;
    public $channel;
    public $listItem;
    public  $page_total = 10; //10
    public $htmlItem;
    public $ROOT_SAVE_DIR = "Data";
    public $WEB_HOME = "http://www.hydcd.com/cy";
    public $IMAGE_DIR = "Image";
    public $PIC_DIR = "Pic";
    public function DoPaser()
    {
        $this->listItem =   array();
        $this->htmlItem =   new simple_html_dom();

        $save_dir = $this->ROOT_SAVE_DIR . "/" . $this->IMAGE_DIR;
        if (!is_dir($save_dir)) {
            mkdir($save_dir);
        }

        $dir = $this->ROOT_SAVE_DIR . "/" . $this->IMAGE_DIR . "/" . $this->PIC_DIR;
        if (!is_dir($dir)) {
            mkdir($dir);
        }

        $url = "http://www.hydcd.com/cy/fkccy/index.htm";
        for ($i = 0; $i < 10; $i++) {
            if ($i > 0) {
                $url = "http://www.hydcd.com/cy/fkccy/index" . ($i + 1) . ".htm";
            }
            $this->PaserImageList0($url);
        }

        //save
        $savefilepath = $save_dir . "/idiom_image_list.json";
        $ret = file_exists($savefilepath);
        if ($ret) {
            // return;
        }

        $count = count($this->listItem);
        echo "count =" . $count . "\n";
        if ($count) {

            $element = array(
                'items' => ($this->listItem),
            );
            //JSON_UNESCAPED_SLASHES json去除反斜杠 JSON_UNESCAPED_UNICODE中文不用\u格式
            $jsn = urldecode(json_encode($element, JSON_UNESCAPED_SLASHES | JSON_UNESCAPED_UNICODE));

            // "[  ]"
            //$jsn = str_replace("\"[", "[", $jsn);
            //$jsn = str_replace("]\"", "]", $jsn);

            $fp = fopen($savefilepath, "w");
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
    }


    //疯狂猜成语图片答案、疯狂猜成语所有答案 500个  http://www.hydcd.com/cy/fkccy/index.htm
    //http://www.caichengyu.com/
    function PaserImageList0($url)
    {

        $html = get_html($url);
        if (!$html) {
            echo "PaserImageList0 html fail\n";
            return;
        }
        //table : id table1
        $table_main = $html->find('table[id=table1]', 0);
        if (!$table_main) {
            echo "PaserImageList0 find div_main fail\n";
            return;
        }
        $str = $table_main->innertext;
        //echo "div_main =\n" . $str; 
        $arry_td = $table_main->find('td');
        foreach ($arry_td as $td) {
            $img = $td->find('img', 0);
            $a = $td->find('a', 0);
            $url_a = null;
            if ($a != null) {
                $url_a = $this->WEB_HOME . substr($a->href, 2);
            }
            $pic = "http://www.hydcd.com/cy/fkccy/" . $img->src;
            $name =  $img->alt;
            $name = str_replace("答案是:", "", $name);

            //echo "img:  " . $pic . " name:" . $name . "\n";
            if (BlankString($name)) {
                $name =  $a->plaintext;
                // $url_a = $this->WEB_HOME . substr($a->href, 2);

            }
            if (BlankString($name)) {
                echo "BlankString img:  " . $pic . " name:" . $name . " url_a=" . $url_a . "\n";
            }


            $filepath1 = $this->ROOT_SAVE_DIR . "/All_Idiom/" . $name . ".json";
            $filepath2 = $this->ROOT_SAVE_DIR . "/" . $this->IMAGE_DIR . "/" . $name . ".json";
            if (file_exists($filepath1)) {
                if (copy($filepath1, $filepath2)) {
                    $savepic = $this->ROOT_SAVE_DIR . "/" . $this->IMAGE_DIR . "/" . $this->PIC_DIR . "/" . $name . ".png";
                    if (!file_exists($savepic)) {
                        Download::Main()->DownloadFile($pic, $savepic);
                    }

                    $element = array(
                        //'pic' => $pic, //谜面
                        'id' => $name,

                    );
                    array_push($this->listItem, $element);
                }
            } else {

                if ($url_a != null) {
                    echo "BlankString url_a:  " . $url_a . "\n";
                }

                echo "idiom json file is not exist "  . " name:" . $name . "\n";
                // continue;
            }
        }
    }


    //http://www.caichengyu.com/ktccy/
    function PaserImageList1($html)
    { }
}

$parse = new IdiomImageParser();
$parse->DoPaser();
echo 'done<br>';
