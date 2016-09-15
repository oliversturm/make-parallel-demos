// Copyright 2010 Oliver Sturm <oliver@oliversturm.com> All rights reserved. 

using Data.Mutable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Rendering;

namespace OliDTP {
  // This class holds references to information about the rendering input and output.
  // It reacts to notifications from the UI and triggers rendering, then notifies 
  // the UI (or whoever wants to listen) that the results have changed.
  // 
  // The implementation is not all too performant in some ways, but that's mainly
  // due to the handling of large bitmap images. The focus is on showing the elements
  // of a possible real-world architecture.
  public class PresentationModel {
    public PresentationModel(Document document, ZoomMode zoomMode, float zoomPercentage, int viewContainerWidth, int viewContainerHeight) {
      this.document = document;
      this.zoomMode = zoomMode;
      this.zoomPercentage = zoomPercentage;
      this.viewContainerWidth = viewContainerWidth;
      this.viewContainerHeight = viewContainerHeight;

      this.activeItems = new List<ActiveItem>();
      this.selection = new Selection(this, activeItems);

      UpdateRenderImage();
    }

    #region Input properties
    private Document document;
    public Document Document {
      get { return document; }
      set {
        if (!object.ReferenceEquals(document, value)) {
          document = value;
          UpdateRenderImage();
        }
      }
    }

    private ZoomMode zoomMode;
    public ZoomMode ZoomMode {
      get { return zoomMode; }
      set {
        if (zoomMode != value) {
          zoomMode = value;
          UpdateRenderImage();
        }
      }
    }

    private float zoomPercentage;
    public float ZoomPercentage {
      get { return zoomPercentage; }
      set {
        if (zoomPercentage != value) {
          zoomPercentage = value;
          UpdateRenderImage();
        }
      }
    }

    private int viewContainerWidth;
    public int ViewContainerWidth {
      get { return viewContainerWidth; }
      set {
        if (viewContainerWidth != value) {
          viewContainerWidth = value;
          if (zoomMode != OliDTP.ZoomMode.Percentage)
            UpdateRenderImage();
        }
      }
    }

    private int viewContainerHeight;
    public int ViewContainerHeight {
      get { return viewContainerHeight; }
      set {
        if (viewContainerHeight != value) {
          viewContainerHeight = value;
          if (zoomMode != OliDTP.ZoomMode.Percentage)
            UpdateRenderImage();
        }
      }
    }

    public void NotifyDocumentChanged() {
      UpdateRenderImage();
    }

    #endregion

    #region Output properties
    private Bitmap bitmap;
    public Bitmap Bitmap {
      get { return bitmap; }
    }

    private float dPIX;
    public float DPIX {
      get { return dPIX; }
    }

    private float dPIY;
    public float DPIY {
      get { return dPIY; }
    }
    #endregion

    #region Housekeeping
    Selection selection;
    List<ActiveItem> activeItems;
    #endregion

    #region Bitmap calculation and update
    private void CalcDPI() {
      switch (zoomMode) {
        case ZoomMode.WholePage:
          dPIX = dPIY = Math.Min(
            viewContainerWidth / document.Size.Width,
            viewContainerHeight / document.Size.Height);
          break;

        case ZoomMode.Percentage:
          var dpi = WinAPIHelpers.GetScreenDPI();
          dPIX = dpi.Width * zoomPercentage / 100.0f;
          dPIY = dpi.Height * zoomPercentage / 100.0f;
          break;

        case ZoomMode.PageWidth:
          dPIX = dPIY = viewContainerWidth / document.Size.Width;
          break;

        case ZoomMode.Disproportional:
          dPIX = viewContainerWidth / document.Size.Width;
          dPIY = viewContainerHeight / document.Size.Height;
          break;
      }
    }

    List<RenderInfo> renderInfo;
    Bitmap renderImage;

    public void UpdateRenderImage() {
      if (updateLock == 0) {
        CalcDPI();
        var renderer = new Renderer();
        var renderResult = renderer.Render(document, dPIX, dPIY);
        if (renderImage != null) {
          renderImage.Dispose();
          renderImage = null;
        }
        renderImage = renderResult.Item1;
        renderInfo = new List<RenderInfo>(renderResult.Item2);
        selection.UpdateSelectionFromRenderInfo(renderInfo);
        UpdateAdornments();
      }
    }

    private void UpdateAdornments() {
      if (updateLock == 0) {
        if (bitmap != null) {
          bitmap.Dispose();
          bitmap = null;
        }

        bitmap = new Bitmap(renderImage);
        using (var bgr = Graphics.FromImage(bitmap)) using (var adornmentImage = new Bitmap(bitmap.Width, bitmap.Height)) {
          using (var agr = Graphics.FromImage(adornmentImage)) {
            selection.DrawSelections(agr);
            DrawActiveItems(agr);
          }

          bgr.DrawImageUnscaled(adornmentImage, 0, 0);
        }

        OnBitmapChanged();
      }
    }

    private void DrawActiveItems(Graphics gr) {
      foreach (var item in activeItems)
        item.DrawAdornments(gr);
    }

    public event EventHandler BitmapChanged;

    protected virtual void OnBitmapChanged() {
      if (BitmapChanged != null)
        BitmapChanged(this, EventArgs.Empty);
    }
    #endregion

    #region Update locking
    int updateLock = 0;

    public void SuspendUpdate() {
      updateLock++;
    }

    public void ResumeUpdate(bool triggerUpdate) {
      updateLock--;
      if (triggerUpdate && updateLock == 0)
        UpdateRenderImage();
    }
    #endregion

    #region Click handling
    public void NotifyLeftMouseButtonClicked(int x, int y) {
      // the coordinates I get here are relative to the Bitmap
      //renderInfo[0].Rect.

      var relevantActiveItems =
        activeItems.Where(ai => ai.Rectangle.Contains(x, y));
      foreach (var activeItem in relevantActiveItems) if (activeItem.NotifyLeftMouseButtonClicked())
          return;

      var oldSelection = selection.SelectedRenderInfo;
      selection.NotifyLeftMouseButtonClicked(x, y, renderInfo);
      if (!object.ReferenceEquals(oldSelection, selection.SelectedRenderInfo))
        UpdateAdornments();
    }
    #endregion


    #region Mousing stuff
    public Cursor CalcMouseCursor(int x, int y) {
      if (dragStarted) return dragItem.MouseCursor;
      else {
        var activeItemUnderMouse =
          activeItems.FirstOrDefault(ai => ai.Rectangle.Contains(x, y));
        return activeItemUnderMouse != null ?
          activeItemUnderMouse.MouseCursor : Cursors.Default;
      }
    }

    bool dragStarted = false;
    ActiveItem dragItem = null;

    public bool NotifyLeftMouseDragStart(int x, int y) {
      var relevantActiveItems =
        activeItems.Where(ai => ai.Rectangle.Contains(x, y));
      foreach (var item in relevantActiveItems)
        if (item.NotifyLeftMouseDragStart(x, y)) {
          dragItem = item;
          dragStarted = true;
          UpdateAdornments();
          return true;
        }
      return false;
    }

    public bool NotifyLeftMouseDragMove(int x, int y) {
      var result = dragItem.NotifyLeftMouseDragMove(x, y);
      UpdateAdornments();
      return result;
    }

    public void NotifyLeftMouseDragEnd(int x, int y) {
      bool updateRequired =
        dragItem.NotifyLeftMouseDragEnd(x, y);
      dragStarted = false;
      dragItem = null;
      if (updateRequired)
        UpdateRenderImage();
      else
        UpdateAdornments();
    }

    #endregion

    #region Layer handling
    Layer activeLayer;
    public Layer ActiveLayer {
      get {
        if (activeLayer != null)
          return activeLayer;
        else if (document != null && document.Layers.Count > 0)
          return document.Layers[0];
        else
          return null;
      }
      set {
        if (activeLayer != value)
          activeLayer = value;
      }
    }

    public void SetLayerVisibility(Layer layer, bool visible) {
      if (layer.Visible != visible) {
        lock (document.Lock) layer.Visible = visible;
        UpdateRenderImage();
      }
    }
    #endregion

    #region Document modifications
    public void AddDocumentElement(Layer layer, Element element) {
      if (layer != null) {
        lock (document.Lock) layer.Elements.Add(element);
        UpdateRenderImage();
      }
    }

    public void AddDocumentElement(Element element) {
      AddDocumentElement(ActiveLayer, element);
    }
    #endregion

  }
}