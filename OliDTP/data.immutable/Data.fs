// Copyright 2010 Oliver Sturm <oliver@oliversturm.com> All rights reserved. 

namespace Data.Immutable

type ShapeInfo = 
    | Unknown
    | Rectangle
    | Ellipse
    | BitmapImage of string

type Element =
    { Source: Data.Mutable.Element; Shape: ShapeInfo; X: single; Y: single; Width: single; Height: single; ZOrder: int}

type Layer =
    { Source: Data.Mutable.Layer; Elements: Element list; ZOrder: int; Visible: bool }

type Document =
    { Source: Data.Mutable.Document; Layers: Layer list; Width: single; Height: single }
