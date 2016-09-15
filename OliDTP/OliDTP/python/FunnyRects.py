# Copyright 2010 Oliver Sturm <oliver@oliversturm.com> All rights reserved. 

from Extensibility.Python import PythonDocumentAction
from Data.Mutable import Rectangle, Layer
from System.Drawing import PointF, SizeF

import math

class FunnyRects(PythonDocumentAction):
    def GetName(self):
        return "Create funny rects"

    def Execute(self, document):
        l = Layer()
        l.Name = "Funny Rects"
        l.Visible = True
        l.ZOrder = 10
        
        pi = 3.141592
        usablewidth = document.Size.Width * .8
        docoffset = document.Size.Width * .1
        steps = 20
        docstep = usablewidth / steps
        pistep = pi / steps
        ybase = document.Size.Height / 2
        maxheight = ybase * .8
        
        for step in range(steps):
            s = math.sin(pistep * step)
            r = Rectangle()
            height = s * maxheight
            r.Location = PointF(docoffset + (docstep * step), ybase - height)
            r.Size = SizeF(docstep, height)
            l.Elements.Add(r)
        
        document.Layers.Add(l)
        
        return True
