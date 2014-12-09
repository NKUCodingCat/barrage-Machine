#-*- coding=utf-8 -*-
# [0]数据编号<int> [1]创建时间<float> [2]内容<string> [3]颜色<string(Syntax：[0-9a-fA-F]{6})>
import json,colorpro
import time
import re
import random
import sae.kvdb
import copy
kv = sae.kvdb.KVClient()
clo = re.compile("\A[0-9A-Fa-f]{6}[Dd]")
sta = re.compile("\A[Dd][Mm]")
mid = re.compile("[\t\r\n\f\v]")
def Click(usr,Text):
    Li = kv.get("DM")
    Now = kv.get("DM-Max")
    if not Li:
        Li = []
        kv.set("DM",[])
    if not Now:
        Now = 0
        kv.set("DM",0)
    #读取数据库数据
    T = time.time()
    No = Now+1;
    Arr = clo.findall(Text)
    if Arr != []:
        Color = Text[0:6].upper();
        Text = Text[6:]
    else:
        Color = colorpro.GetColor(str(usr))
        Text = Text[:]
    Text = sta.sub("",Text)
    Text = mid.sub(" ",Text)
    Li.append([No,T,Text,Color])
    kv.set("DM",Li)
    kv.set("DM-Max",Now+1)
    return "弹幕已发送~"
def tojson():
    Li = kv.get("DM")
    if not Li:
        Li = [ ]
        kv.set("DM",[])
    #读取数据库数据
    T = time.time()
    L = Li[:]
    Li = [i for i in L if (abs(i[1] - T) < 60)]
    kv.set("DM",Li)
    #清除已经过期的数据
    Di = {}
    List = []
    """
    for j in Li:
        Di["Num"] = j[0]
        Di["Time"] = j[1]
        Di["Content"] = j[2]
        List.append(copy.deepcopy(Di))
    """
    for j in Li:
        Di[j[0]] = {"Time":j[1],"Content":j[2],"Color":j[3]}
    return json.dumps(Di)