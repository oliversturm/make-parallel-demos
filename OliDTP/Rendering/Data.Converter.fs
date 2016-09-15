// Copyright 2010 Oliver Sturm <oliver@oliversturm.com> All rights reserved. 

module Data.Converter

open Data.Immutable

let clone (doc:Data.Mutable.Document) = 
    let getShapeInfo (e: Mutable.Element) = 
        match e with 
        | :? Mutable.Rectangle -> Rectangle
        | :? Mutable.Ellipse -> Ellipse
        | :? Mutable.BitmapImage as i -> BitmapImage(i.Filename)
        | _ -> Unknown

    let cloneElement (e: Mutable.Element) =
        { Source = e;
            Shape = getShapeInfo e;
            X = e.Location.X;
            Y = e.Location.Y;
            Width = e.Size.Width;
            Height = e.Size.Height;
            ZOrder = e.ZOrder }

    let cloneLayer (l: Mutable.Layer) = 
        { Source = l;
            Elements = [for e in l.Elements -> cloneElement e]; 
            ZOrder = l.ZOrder;
            Visible = l.Visible }

    lock doc.Lock (fun () ->
                    { Source = doc;
                        Layers = [for l in doc.Layers -> cloneLayer l];
                        Width = doc.Size.Width;
                        Height = doc.Size.Height })
