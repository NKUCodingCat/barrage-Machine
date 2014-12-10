# -*- coding=utf-8 -*-
import kvpo,colorpro,re,Img
import sae
import urlparse
import xml.etree.ElementTree as ET
C = re.compile("\A/[0-9a-fA-F]{6}\.jpg\Z")
def app(environ, start_response):
    status = '200 OK'
    if environ ["PATH_INFO"]  == "/dm":
        response_headers = [('Content-type', 'text/html; charset=utf-8')]
        start_response(status, response_headers)
        method=environ['REQUEST_METHOD']
        if method=="GET":
            query=environ['QUERY_STRING']
            echostr=urlparse.parse_qs(query)['echostr']
            return echostr
        elif method=="POST":
            post=environ['wsgi.input']        
            root = ET.parse(post)
            fromUser=root.findtext(".//FromUserName")
            toUser=root.findtext(".//ToUserName")
            CreateTime=root.findtext(".//CreateTime")
            Text=root.findtext(".//Content")
            texttpl='''<xml>
            <ToUserName>'''+fromUser+'''</ToUserName>
            <FromUserName>'''+toUser+'''</FromUserName>
            <CreateTime>'''+CreateTime+'''</CreateTime>
            <MsgType><![CDATA[text]]></MsgType>
            <Content><![CDATA[''' + kvpo.Click(fromUser,Text) + ''']]></Content>
            </xml>'''
            return texttpl
    if environ ["PATH_INFO"]  == "/color":
        response_headers = [('Content-type', 'text/html; charset=utf-8')]
        start_response(status, response_headers)
        method=environ['REQUEST_METHOD']
        if method=="GET":
            query=environ['QUERY_STRING']
            echostr=urlparse.parse_qs(query)['echostr']
            return echostr
        elif method=="POST":
            post=environ['wsgi.input']        
            root = ET.parse(post)
            fromUser=root.findtext(".//FromUserName")
            toUser=root.findtext(".//ToUserName")
            CreateTime=root.findtext(".//CreateTime")
            Text=root.findtext(".//Content")
            texttpl= """<xml>
            <ToUserName><![CDATA["""+fromUser+"""]]></ToUserName>
            <FromUserName><![CDATA["""+toUser+"""]]></FromUserName>
            <CreateTime>"""+CreateTime+"""</CreateTime>
            <MsgType><![CDATA[news]]></MsgType>
            <ArticleCount>1</ArticleCount>
            <Articles>
            <item>
            <Title><![CDATA[默认弹幕颜色修改成功~]]></Title> 
            <Description><![CDATA["""+colorpro.SetColor(fromUser,Text)+"""\n图片的背景颜色才是你的弹幕颜色啊喂！]]></Description>
            <PicUrl><![CDATA[http://302.nktwclick.sinaapp.com/"""+Text[-6:].upper()+""".jpg]]></PicUrl>
            <Url><![CDATA[http://302.nktwclick.sinaapp.com/"""+Text[-6:].upper()+""".jpg]]></Url>
            </item>
            </Articles>
            </xml> """
            return texttpl
    if environ ["PATH_INFO"]  == "/check":
        response_headers = [('Content-type', 'text/html; charset=utf-8')]
        start_response(status, response_headers)
        method=environ['REQUEST_METHOD']
        if method=="GET":
            query=environ['QUERY_STRING']
            echostr=urlparse.parse_qs(query)['echostr']
            return echostr
        elif method=="POST":
            post=environ['wsgi.input']        
            root = ET.parse(post)
            fromUser=root.findtext(".//FromUserName")
            toUser=root.findtext(".//ToUserName")
            CreateTime=root.findtext(".//CreateTime")
            Text=root.findtext(".//Content")
            texttpl= """<xml>
            <ToUserName><![CDATA["""+fromUser+"""]]></ToUserName>
            <FromUserName><![CDATA["""+toUser+"""]]></FromUserName>
            <CreateTime>"""+CreateTime+"""</CreateTime>
            <MsgType><![CDATA[news]]></MsgType>
            <ArticleCount>1</ArticleCount>
            <Articles>
            <item>
            <Title><![CDATA[默认弹幕颜色~]]></Title> 
            <Description><![CDATA[默认弹幕颜色为"""+str(colorpro.GetColor(fromUser))+"""\n图片的背景颜色才是你的弹幕颜色啊喂！]]></Description>
            <PicUrl><![CDATA[http://302.nktwclick.sinaapp.com/"""+str(colorpro.GetColor(fromUser))+""".jpg]]></PicUrl>
            <Url><![CDATA[http://302.nktwclick.sinaapp.com/"""+str(colorpro.GetColor(fromUser))+""".jpg]]></Url>
            </item>
            </Articles>
            </xml> """
            return texttpl
    if environ ["PATH_INFO"]  == "/tojson":
        response_headers = [('Content-type', 'text/html; charset=utf-8')]
        start_response(status, response_headers)
        return kvpo.tojson()
    if C.findall(environ ["PATH_INFO"]) != []:
        response_headers = [('Content-type', ' image/jpeg')]
        start_response(status, response_headers)
        return Img.toImg(environ ["PATH_INFO"][1:7])
    else:
        response_headers = [('Content-type', 'text/html; charset=utf-8')]
        start_response(status, response_headers)
        return "Who's your Daddy"
application = sae.create_wsgi_app(app)