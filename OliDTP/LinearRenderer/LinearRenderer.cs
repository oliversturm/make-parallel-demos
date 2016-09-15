using Data.Mutable;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rendering {
  public class RenderInfo {
    public RenderInfo(Layer layer, Element element, System.Drawing.Rectangle rect) {
      Layer = layer;
      Element = element;
      Rect = rect;
    }
    public Layer Layer { get; }
    public Element Element { get; }
    public System.Drawing.Rectangle Rect { get; }
  }

  public class Renderer {
    public (Bitmap bm, List<RenderInfo> ril)
      Render(Document doc, float dpix, float dpiy) {
      var bm = new Bitmap((int) (doc.Size.Width * dpix) + 1,
        (int) (doc.Size.Height * dpiy) + 1);
      var renderInfoList = new List<RenderInfo>();
      using (var gr = Graphics.FromImage(bm)) {
        //Parallel.ForEach(doc.Layers.
        //  Where(l => l.Visible).OrderBy(l => l.ZOrder), layer => {
        foreach (var layer in doc.Layers.
          Where(l => l.Visible).OrderBy(l => l.ZOrder)) {
          //Parallel.ForEach(layer.Elements.OrderBy(e => e.ZOrder), element => {
          foreach (var element in layer.Elements.OrderBy(e => e.ZOrder)) {
            var rect = new System.Drawing.Rectangle((int) (element.Location.X * dpix),
           (int) (element.Location.Y * dpiy),
           (int) (element.Size.Width * dpix),
           (int) (element.Size.Height * dpiy));
            switch (element) {
              case Data.Mutable.Rectangle r:
                gr.DrawRectangle(Pens.Black, rect);
                break;
              case Ellipse e:
                gr.DrawEllipse(Pens.Black, rect);
                break;
              case BitmapImage i:
                using (var image = new Bitmap(i.Filename)) {
                  gr.DrawImage(image, rect);
                }
                break;
            }
            renderInfoList.Add(new RenderInfo(layer, element, rect));
          }//);
        }//);
      }

      return (bm, renderInfoList);
    }
  }
}