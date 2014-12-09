#-*- coding=utf-8 -*-
import pub
import time, MySQLdb 
import re
C = re.compile("\A[Cc][Oo][Ll][Oo][Rr][0-9a-fA-F]{6}\Z")
def SetColor ( user , color ):
    if (C.findall(color) == []):
        return "指令错误"
    color = color[-6:].upper()
    sq = pub.SQL()
    db = MySQLdb.connect(host = sq[3],user = sq[1] , passwd = sq[2],db = sq[0],port = sq[4],charset='utf8')
    cursor = db.cursor()
    cursor.execute("""insert into color (name , color) values (%s,%s) ON DUPLICATE KEY UPDATE color = %s""",(user,color,color))
    return "设置成功~~现在氖发出的弹幕颜色默认为"+str(color)
def GetColor ( user):
    user = str(user)
    sq = pub.SQL()
    db = MySQLdb.connect(host = sq[3],user = sq[1] , passwd = sq[2],db = sq[0],port = sq[4],charset='utf8')
    cursor = db.cursor()
    cursor.execute("select * from color where name = %s", user)
    datas = cursor.fetchall()
    for i in datas:
        if i[0] == user:
            return i[1]
    return "FFFFFF"
def UserCheck(user):
    return None