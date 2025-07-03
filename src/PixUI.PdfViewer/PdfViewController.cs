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
            SKSurface surface = null!; //TODO: 考虑使用PictureRecorder
            Task.Run(async () =>
            {
                await _document.RenderPageToAsync(pageIndex + 1, (rect, pageRotationMatrix) =>
                {
                    var (width, height) = _document.ScalePageToRequestedSize(rect, new Vector2(-1, -1));
                    surface = SKSurface.Create(new ImageInfo() { Width = width, Height = height });
                    var target = new SkiaRenderTarget(surface.Canvas);
                    _document.InitializeRenderTarget(target, rect, width, height, pageRotationMatrix);
                    return target;
                }).ConfigureAwait(false);

                UIApplication.Current.BeginInvoke(() =>
                {
                    _pagesCache[pageIndex] = surface.Snapshot();
                    surface.Dispose();
                    _view?.Repaint();
                });
            });
            return false;
        }

        pageImage = _pagesCache[pageIndex]!;
        return true;
    }
}