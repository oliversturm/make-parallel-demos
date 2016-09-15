// Copyright 2010 Oliver Sturm <oliver@oliversturm.com> All rights reserved. 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Data.Mutable;
using System.Collections;

using Rendering;

namespace OliDTP {
  public class Selection {
    // Currently just one selected item supported - it's just a demo :-)

    public Selection(PresentationModel presentationModel, IList<ActiveItem> activeItems) {
      this.presentationModel = presentationModel;  
      this.activeItems = activeItems;
    }

    PresentationModel presentationModel;
    IList<ActiveItem> activeItems;

    RenderInfo selectedRenderInfo;
    public RenderInfo SelectedRenderInfo {
      get { return selectedRenderInfo; }
    }
    public bool HasSelection {
      get { return selectedRenderInfo != null; }
    }
    
    System.Drawing.Rectangle selectionRectangle;

    public void SetSelection(RenderInfo newSelection) {
      selectedRenderInfo = newSelection;
      if (selectedRenderInfo != null)
        selectionRectangle = System.Drawing.Rectangle.Inflate(selectedRenderInfo.Rect, 2, 2);
      else
        selectionRectangle = System.Drawing.Rectangle.Empty;
      UpdateSelectionGrabHandles( );
    }

    public void UpdateSelectionFromRenderInfo(IEnumerable<RenderInfo> renderInfo) {
      // This is called when the renderinfo has changed because the image has been
      // rerendered. In that case I need to find the new renderInfo that relates
      // to the object that was previously selected.
      if (selectedRenderInfo != null) {
        var newSelection = renderInfo.FirstOrDefault(ri => object.ReferenceEquals(ri.Element.Source, selectedRenderInfo.Element.Source));
        SetSelection(newSelection);
      }
    }

    Pen selectionRectPen;
    private Pen SelectionRectPen {
      get {
        if (selectionRectPen == null) {
          selectionRectPen = new Pen(Color.Black);
          selectionRectPen.DashPattern = new float[] { 2f, 2f };
        }
        return selectionRectPen;
      }
    }

    public void DrawSelections(Graphics gr) {
      // I could use methods from ControlPaint to draw a selection rect. These are not
      // the most flexible ones in the world though, plus it's good to know exactly
      // where all the items are so I can react to them later.

      if (selectedRenderInfo != null) {
        gr.DrawRectangle(SelectionRectPen, selectionRectangle);
      }
    }

    public void UpdateSelectionGrabHandles( ) {
      var handles = activeItems.OfType<ElementSizeChangeActiveItem>( ).ToList( );
      foreach (var handle in handles)
        activeItems.Remove(handle);
      var movers = activeItems.OfType<ElementMoveActiveItem>( ).ToList( );
      foreach (var mover in movers)
        activeItems.Remove(mover);

      if (selectedRenderInfo != null) {
        activeItems.Add(new BorderGrabHandle(presentationModel, selectedRenderInfo, selectionRectangle, BorderGrabHandleType.Top));
        activeItems.Add(new BorderGrabHandle(presentationModel, selectedRenderInfo, selectionRectangle, BorderGrabHandleType.Bottom));
        activeItems.Add(new BorderGrabHandle(presentationModel, selectedRenderInfo, selectionRectangle, BorderGrabHandleType.Left));
        activeItems.Add(new BorderGrabHandle(presentationModel, selectedRenderInfo, selectionRectangle, BorderGrabHandleType.Right));
        activeItems.Add(new CornerGrabHandle(presentationModel, selectedRenderInfo, selectionRectangle, CornerGrabHandleType.BottomLeft));
        activeItems.Add(new CornerGrabHandle(presentationModel, selectedRenderInfo, selectionRectangle, CornerGrabHandleType.BottomRight));
        activeItems.Add(new CornerGrabHandle(presentationModel, selectedRenderInfo, selectionRectangle, CornerGrabHandleType.TopLeft));
        activeItems.Add(new CornerGrabHandle(presentationModel, selectedRenderInfo, selectionRectangle, CornerGrabHandleType.TopRight));
        activeItems.Add(new ElementMoveActiveItem(presentationModel, selectedRenderInfo));
      }
    }

    public void NotifyLeftMouseButtonClicked(int x, int y, List<RenderInfo> renderInfo) {
      var containingItems =
        (from ri in renderInfo
         where ri.Rect.Contains(x, y)
         orderby ri.Layer.ZOrder, ri.Element.ZOrder
         select ri).ToList( );
      if (containingItems.Count > 0) {
        // I have found candidates for selection
        if (containingItems.Count > 1 && selectedRenderInfo != null) {
          // I have found more than one candidate, and there's already a selection now

          var selectionIndex = containingItems.FindIndex(ri => object.ReferenceEquals(ri, selectedRenderInfo));
          // SelectionIndex now tells me whether the existing selection is part of the sorted
          // list of candidates I got back. In that case I'll try to cycle the selection
          // to allow the end user to select items that sit on top of one another.
          if (selectionIndex > 0)
            SetSelection(containingItems[selectionIndex - 1]);
          else if (selectionIndex == 0)
            SetSelection(containingItems[containingItems.Count - 1]);
          else
            SetSelection(containingItems[0]);
        }
        else {
          SetSelection(containingItems[0]);
        }
      }
      else {
        SetSelection(null);
      }
    }
  }
}
