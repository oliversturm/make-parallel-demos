// Copyright 2010 Oliver Sturm <oliver@oliversturm.com> All rights reserved. 

module Renderer

open System.Drawing
open Data.Immutable

type RenderInfo =
    { Layer: Layer; Element: Element; Rect: Rectangle }

let private render (doc: Document) dpix dpiy = 
    // Render one layer. 
    let renderLayer l = 
        let bm = new Bitmap(int(doc.Width * dpix) + 1, int(doc.Height * dpiy) + 1)
        use gr = Graphics.FromImage bm

        // Render an element based on its shape. The information about the actual
        // size the element is taking in the result image is added to a list of 
        // RenderInfo items that is passed in. The signature if this function
        // is that of a fold aggregator.
        let renderElement ril e = 
            let r = new Rectangle(int (e.X * dpix), int (e.Y * dpiy), int (e.Width * dpix), int (e.Height * dpiy))
            match e.Shape with
            | Rectangle -> gr.DrawRectangle(Pens.Black, r)
            | Ellipse -> gr.DrawEllipse(Pens.Black, r)
            | BitmapImage(filename) -> 
                use image = new Bitmap(filename)
                gr.DrawImage(image, r)
            | _ -> ignore e
            { Layer = l; Element = e; Rect = r } :: ril

        // Return a tuple of the result bitmap for the layer and the
        // list of RenderInfo instances relating to the elements in this layer
        (bm, Seq.sortBy (fun (e: Element) -> e.ZOrder) l.Elements |>
                Seq.fold renderElement [])

    let bm = new Bitmap((int (doc.Width * dpix)) + 1, (int (doc.Height * dpiy)) + 1)
    use gr = Graphics.FromImage(bm)

    (bm, Seq.filter (fun l -> l.Visible) doc.Layers |>
         Seq.sortBy (fun l -> l.ZOrder) |>
         Seq.map renderLayer |>
         Seq.fold (fun cril (lbm, ril) -> 
                     gr.DrawImage(lbm, 0, 0)
                     lbm.Dispose()
                     cril @ ril) [])

let Render(doc: Data.Mutable.Document, dpix: single, dpiy: single) =
    let cdoc = Data.Converter.clone doc
    render cdoc dpix dpiy
