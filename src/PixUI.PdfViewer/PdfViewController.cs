using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Melville.Pdf.Model;
using Melville.Pdf.Model.Renderers.DocumentRenderers;
using PixUI.PdfViewer.Drawing;

namespace PixUI.PdfViewer;

public sealed class PdfViewController
{
    private DocumentRenderer? _document;
    private PdfView? _view;
    private string? _openError;
    private Image?[]? _pagesCache;

    public int TotalPages => _document?.TotalPages ?? 0;

    internal void InitView(PdfView pdfView)
    {
        if (_view != null)
            throw new InvalidOperationException("PdfView has already been initialized");
        _view = pdfView;
    }

    public async Task OpenAsync(Stream stream)
    {
        try
        {
            _openError = null;
            _pagesCache = null;
            _document = await new PdfReader().ReadFromAsync(stream);
            _pagesCache = new Image[_document.TotalPages];
        }
        catch (Exception e)
        {
            _openError = e.Message;
        }

        _view?.Repaint();
    }

    internal bool TryGetRenderedPage(int pageIndex, [MaybeNullWhen(returnValue: false)] out Image pageImage)
    {
        pageImage = null;
        if (_document == null || _pagesCache == null)
            return false;

        if (_pagesCache[pageIndex] == null)
        {
            var windowRatio = UIWindow.Current.ScaleFactor;
            Task.Run(async () =>
            {
                SKSurface surface = null!;
                // var recorder = new PictureRecorder();
                await _document.RenderPageToAsync(pageIndex + 1, (rect, pageRotationMatrix) =>
                {
                    // var widthPx = (int)((rect.Width / 72f) * 96f * windowRatio);
                    // var heightPx = (int)((rect.Height / 72f) * 96f * windowRatio);
                    var widthPx = (int)(rect.Width * windowRatio);
                    var heightPx = (int)(rect.Height * windowRatio);
                    var (width, height) = _document.ScalePageToRequestedSize(rect, new Vector2(widthPx, heightPx));
                    surface = SKSurface.Create(new ImageInfo() { Width = widthPx, Height = heightPx });
                    var canvas = surface.Canvas;
                    // canvas.Scale(windowRatio, windowRatio);
                    var target = new SkiaRenderTarget(canvas);

                    // var canvas = recorder.BeginRecording(Rect.FromLTWH(0, 0, width, height));
                    // var target = new SkiaRenderTarget(canvas);
                    _document.InitializeRenderTarget(target, rect, width, height, pageRotationMatrix);
                    return target;
                }).ConfigureAwait(false);

                UIApplication.Current.BeginInvoke(() =>
                {
                    _pagesCache[pageIndex] = surface.Snapshot();
                    surface.Dispose();

                    // var picture = recorder.EndRecording();
                    // _pagesCache[pageIndex] = Image.FromPicture(picture, new SizeI(width, height));
                    _view?.Repaint();
                });
            });
            return false;
        }

        pageImage = _pagesCache[pageIndex]!;
        return true;
    }
}