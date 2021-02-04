
<?php
header("Content-type: text/html; charset=utf-8");
$thread = new class extends Thread
{
    public function run()
    {
        echo "Hello World\n";
    }
};
$thread->start() && $thread->join();
?> 
 

 

 

 
 
 
 
