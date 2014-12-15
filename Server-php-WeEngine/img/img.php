<?php
$src = $_GET['color'];
if(preg_match("/^[0-9a-fA-F]{6}/i",$src))
{
	$arr = array(
		'R' => hexdec(substr($src,0,2)),
		'G' => hexdec(substr($src,2,2)),
		'B' => hexdec(substr($src,4,2))
	);
	$image=imagecreate(360,200);
	imagecolorallocate($image,$arr['R'],$arr['G'],$arr['B']);
	imagettftext($image, 40,0,60,110,
		imagecolorallocate($image,
			(($arr['R']>128)?($arr['R']-128):($arr['R']+128)),
			(($arr['G']>128)?($arr['G']-128):($arr['G']+128)),
			(($arr['B']>128)?($arr['B']-128):($arr['B']+128))
			),
		dirname(__FILE__)."/msyh_3.ttf","#".$src
	); 
	header("Content-type:image/jpeg");
	imagejpeg($image,'',100);//输出图像到浏览器
	imagedestroy($image);//释放资源
}
else
{
	echo "ERROR";
}
?>