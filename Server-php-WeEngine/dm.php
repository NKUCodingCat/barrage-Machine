<?php
function microtime_float()
{
   list($usec, $sec) = explode(" ", microtime());
   return ((float)$usec + (float)$sec);
}
//首先Check数据库
$sql = "create table if not exists dm (content TEXT(512), color CHAR(6),time double)";
pdo_query($sql);
$sql = "create table if not exists dm_color (usr CHAR(100), color CHAR(6))";
pdo_query($sql);
//存入数据库
$content = $this->message['content'];

//默认弹幕
if (eregi("^[Dd][Mm]",$content))
{
	$sql = "SELECT * FROM dm_color WHERE usr = '".$this->message['from']."'";
	$res = pdo_fetchall($sql);
	if ( !empty($res[0]['color']) )
	{
		$color = $res[0]['color'];
	}
	else
	{
		$color = "FFFFFF";
	}
	$sql = "INSERT INTO dm VALUES ('".substr($content,2)."','".$color."',".microtime_float().")";
	pdo_query($sql);
	return $this->respText("弹幕发送成功~");
}
	//指定弹幕颜色
elseif (eregi("^[a-fA-F0-9]{6}[Dd][Mm]",$content))
{
	$sql = "INSERT INTO dm VALUES ('".substr($content,8)."','".strtoupper(substr($content,0,6))."',".microtime_float().")";
	pdo_query($sql);
	return $this->respText("弹幕发送成功~");
}
//弹幕颜色
else
{
	if (eregi("^[Cc][Oo][Ll][Oo][Uu]?[Rr]",$content))
	{
		//查询
		if (eregi("^[Cc][Oo][Ll][Oo][Uu]?[Rr][ \n\t\r\f\v]{0,}$",$content))
		{
			$sql = "SELECT * FROM dm_color WHERE usr = '".$this->message['from']."'";
			$res = pdo_fetchall($sql);
			if ( !empty($res[0]['color']) )
			{
				return $this->respText("颜色为#".$res[0]['color']);
			}
			else
			{
				return $this->respText("颜色为#FFFFFF");
			}
		}
		//修改
		elseif (eregi("^[Cc][Oo][Ll][Oo][Uu]?[Rr][ \n\t\r\f\v]{0,}[a-fA-F0-9]{6}[ \n\t\r\f\v]{0,}$",$content))
		{
			preg_match_all("/[a-fA-F0-9]{6}/",$content,$match);
			$sql = "SELECT 1 FROM dm_color WHERE usr = '".$this->message['from']."' LIMIT 1";
			$res = pdo_fetchall($sql);
			if ($res[0][1] == 0)
			{
				$sql = "INSERT INTO dm_color VALUES ('".$this->message['from']."','".strtoupper((String)$match[0][0])."')";
			}
			else
			{
				$sql = "UPDATE dm_color SET color = '".strtoupper((String)$match[0][0])."' WHERE usr = '".$this->message['from']."'";
			}
			pdo_query($sql);
		    return $this->respText("修改成功~现在的默认颜色为#".strtoupper((String)$match[0][0]));
		}
		else
		{
			return $this->respText("弹幕颜色修改参数错误");
		}
	}
	else
	{
		return $this->respText("弹幕参数错误");
	}

}
?>