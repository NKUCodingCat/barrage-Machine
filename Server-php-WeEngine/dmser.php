<?php
$_GET['weid']=0;
require 'source/bootstrap.inc.php';
function microtime_float()
{
   list($usec, $sec) = explode(" ", microtime());
   return ((float)$usec + (float)$sec);
}
$sql = "DELETE FROM dm WHERE time <".(microtime_float()-60);
pdo_query($sql);
$sql = "SELECT * FROM dm";
$res = pdo_fetchall($sql);
$json_arr = array();
foreach($res as $k=>$v){ 
	$row = array(
	'Content' => $v['content'],
	'Time' => $v['time'],
	'Color' => $v['color']
	);
	$json_arr[$k] = $row;
} 
echo json_encode((object)$json_arr);