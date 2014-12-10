# -*- coding: utf-8 -*-
from PIL import Image
from PIL import ImageDraw
from PIL import ImageFont
import StringIO

def toImg(SRC):
    SRC = SRC.upper()
    Color = [int(SRC[0:2],16),int(SRC[2:4],16),int(SRC[4:],16)]
    FC = []
    for i in Color:
        if i > 128:
            FC.append(i-128)
        else:
            FC.append(i+128)
    im = Image.new("RGB", (360, 200), tuple(Color))
    draw = ImageDraw.Draw(im)
    draw.text((90,50), unicode("#"+SRC), font=ImageFont.truetype("msyh_3.ttf", 40),fill = tuple(FC))
    S = StringIO.StringIO()
    im.save(S ,"jpeg",quality = 100)
    S.seek(0)
    return S