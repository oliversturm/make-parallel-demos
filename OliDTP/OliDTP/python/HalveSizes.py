# Copyright 2010 Oliver Sturm <oliver@oliversturm.com> All rights reserved. 

from Extensibility.Python import PythonDocumentAction
from System.Drawing import SizeF

class HalveSizes(PythonDocumentAction):
    def GetName(self):
        return "Halve all sizes"

    def Execute(self, document):
        for layer in document.Layers:
            for element in layer.Elements:
                element.Size = SizeF(element.Size.Width / 2, element.Size.Height / 2)
        
        return True
