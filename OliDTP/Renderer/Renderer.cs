using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Immutable;
using Microsoft.FSharp.Collections;
using System.Drawing;
using System.Collections.Immutable;

namespace Rendering {
  public class RenderInfo {
    public RenderInfo(Layer layer, Element element, Rectangle rect) {
      Layer = layer;
      Element = element;
      Rect = rect;
    }
    public Layer Layer { get; }
    public Element Element { get; }
    public Rectangle Rect { get; }
  }
  public class Renderer {
    public (Bitmap bm, ImmutableList<RenderInfo> ril) 
      Render(Data.Mutable.Document doc, float dpix, float dpiy) {
      var cdoc = Clone(doc);
      return Render(cdoc, dpix, dpiy);
    }

    (Bitmap bm, ImmutableList<RenderInfo> ril) 
      Render(Document doc, float dpix, float dpiy) {
      var bm = new Bitmap((int) (doc.Width * dpix) + 1,
        (int) (doc.Height * dpiy) + 1);
      using (var gr = Graphics.FromImage(bm)) {
        return (bm,
          doc.Layers.
            Where(l => l.Visible).
            OrderBy(l => l.ZOrder).
            // Layer rendering can now happen in parallel:
            //AsParallel().
            Select(l => RenderLayer(l, doc, dpix, dpiy)).
            // Recommendation: convert the sequence back to a non-parallel one explicitly
            // at this point. As it is, Aggregate below will not be parallelized anyway,
            // but since we explicitly don't want it to parallelize, it seems a good
            // idea to indicate this clearly.
            //AsEnumerable().
            Aggregate(ImmutableList<RenderInfo>.Empty,
            (cril, ld) => {
              gr.DrawImage(ld.bm, 0, 0);
              ld.bm.Dispose();
              return cril.AddRange(ld.ril);
            })         
        );
      }
    }

    (Bitmap bm, ImmutableList<RenderInfo> ril) 
      RenderLayer(Layer l, Document doc, float dpix, float dpiy) {
      var bm = new Bitmap((int) (doc.Width * dpix) + 1,
        (int) (doc.Height * dpiy) + 1);
      using (var gr = Graphics.FromImage(bm)) {
        ImmutableList<RenderInfo> RenderElement (
          ImmutableList<RenderInfo> ril, Element e)
        {
          var rect = new Rectangle((int) (e.X * dpix), 
            (int) (e.Y * dpiy), 
            (int) (e.Width * dpix), 
            (int) (e.Height * dpiy));
          if (e.Shape.IsRectangle)
            gr.DrawRectangle(Pens.Black, rect);
          else if (e.Shape.IsEllipse)
            gr.DrawEllipse(Pens.Black, rect);
          else if (e.Shape.IsBitmapImage) {
            var filename = ((ShapeInfo.BitmapImage) e.Shape).Item;
            using (var image = new Bitmap(filename)) {
              gr.DrawImage(image, rect);
            }
          }
          return ril.Add(new RenderInfo(l, e, rect));
        }

        return (bm,
          l.Elements.OrderBy(e => e.ZOrder).Aggregate(
            ImmutableList<RenderInfo>.Empty, RenderElement));
      }
    }

    Document Clone(Data.Mutable.Document doc) {
      ShapeInfo GetShapeInfo(Data.Mutable.Element e)
      {
        switch (e) {
          case Data.Mutable.Rectangle r:
            return ShapeInfo.Rectangle;
          case Data.Mutable.Ellipse el:
            return ShapeInfo.Ellipse;
          case Data.Mutable.BitmapImage i:
            return ShapeInfo.NewBitmapImage(i.Filename);
          default:
            return ShapeInfo.Unknown;
        }
      }

      Element CloneElement(Data.Mutable.Element e) =>
        new Element(e, GetShapeInfo(e), e.Location.X,
          e.Location.Y, e.Size.Width, e.Size.Height, e.ZOrder);

      Layer CloneLayer(Data.Mutable.Layer l) =>
        new Layer(l,
          ListModule.OfSeq(l.Elements.Select(CloneElement)),
          l.ZOrder, l.Visible);

      lock (doc.Lock) {
        return new Document(doc,
          ListModule.OfSeq(doc.Layers.Select(CloneLayer)),
          doc.Size.Width,
          doc.Size.Height);
      }
    }
  }
}