<?php
header("Content-type: text/html; charset=utf-8");

// 疯狂猜图  http://www.caichengyu.com/fkct/
class ScanDir //extends Thread
{

    public $listItem = array();
    public $IMAGE_DIR = "CrazyImage";
    public $PIC_DIR = "Pic";
    public function DoScan()
    {
        $this->ScanMainPath("./");
    }

    public function ScanMainPath($path)
    {
        if (!is_dir($path)) {
            return;
        }
        $handle = opendir($path);

        //  closedir($handle);
        while (false !== $file = readdir($handle)) {
            if (($file == ".") || ($file == "..")) {
                continue;
            }
            $path_child = $path . "/" . $file;
            if (is_dir($path_child)) {
                $this->listItem = array();
                $this->ScanPath($path_child);
                $this->SaveImageListJson("./",  $file);
            }
        }


        closedir($handle);
    }


    public function ScanPath($path)
    {
        if (!is_dir($path)) {
            return;
        }
        $handle = opendir($path);

        //  closedir($handle);
        while (false !== $file = readdir($handle)) {
            if (($file == ".") || ($file == "..")) {
                continue;
            }
            $path_child = $path . "/" . $file;
            if (is_dir($path_child)) {
                $this->ScanPath($path_child);
            }

            //file
            $element = array(
                'id' => $this->GetFileName($file),
            );
            //
            array_push($this->listItem, $element);
        }


        closedir($handle);
    }


    function GetFileName($file)
    {
        $pos = strpos($file, ".");
        $str = substr($file, 0, $pos);
        return $str;
    }

    public function SaveImageListJson($save_dir, $id)
    {
        //save sort
        $savefilepath = $save_dir . "/item_" . $id . ".json";
        $ret = file_exists($savefilepath);
        if ($ret) {
            // return;
        }
        $count = count($this->listItem);
        echo "SaveImageListJson count =" . $count . "\n";
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
                echo "写入文件失败:" . $savefilepath . "\n" . " jsn=" . $jsn . "\n";
            }
            fclose($fp);
        }
    }
}


$p = new ScanDir();
$p->DoScan();

echo 'done<br>';
