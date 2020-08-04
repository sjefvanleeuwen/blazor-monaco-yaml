using BlazorMonaco;
using BlazorMonaco.Bridge;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMonacoYaml.Helpers
{
    public static class DeltaDecorationHelper
    {
        public static async Task<string[]> SetDeltaDecorations(MonacoEditor monacoEditor, string[] deltaDecorationIds, BlazorMonaco.Bridge.Range range, ModelDecorationOptions options)
        {
            if (monacoEditor == null)
            {
                throw new NullReferenceException("MonacoEditor has not been set");
            }

            return await monacoEditor.DeltaDecorations(deltaDecorationIds ?? new string[] { }, new ModelDeltaDecoration[] { new ModelDeltaDecoration { Range = range, Options = options } });
        }
        
        public static async Task<string[]> SetDeltaDecorations(MonacoEditor monacoEditor, string[] deltaDecorationIds, ModelDeltaDecoration[] deltaDecorations)
        {
            if (monacoEditor == null)
            {
                throw new NullReferenceException("MonacoEditor has not been set");
            }

            return await monacoEditor.DeltaDecorations(deltaDecorationIds ?? new string[] { }, deltaDecorations);
        }

        public static async Task<string[]> ResetDeltaDecorations(MonacoEditor monacoEditor)
        {
            if (monacoEditor == null)
            {
                throw new NullReferenceException("MonacoEditor has not been set");
            }
            await monacoEditor.ResetDeltaDecorations();
            return null;
        }

        public static async Task<ModelDeltaDecoration> BuildDeltaDecoration(MonacoEditor monacoEditor, BlazorMonaco.Bridge.Range range, string message)
        {
            var isWholeLine = false;

            range.StartLineNumber = Math.Max(range.StartLineNumber, 1);
            range.StartColumn = Math.Max(range.StartColumn, 1);
            range.EndLineNumber = Math.Max(range.EndLineNumber, 1);
            if (range.EndColumn == 0)
            {
                range.EndColumn = range.StartColumn;
                var content = await monacoEditor.GetValue();
                var contentLines = content.Split("\n");
                range.EndColumn = (contentLines.ElementAt(Math.Min(contentLines.Length - 1, range.EndLineNumber - 1))?.Trim().Length ?? 0) + 1;
                isWholeLine = true;
            }

            var options = new ModelDecorationOptions
            {
                IsWholeLine = isWholeLine,
                InlineClassName = "editorError",
                InlineClassNameAffectsLetterSpacing = false,
                ClassName = "editorError",
                HoverMessage = new MarkdownString[] { new MarkdownString { Value = $"**Error**\r\n\r\n{message}" } },
                GlyphMarginClassName = "editorErrorGlyph fa fa-exclamation-circle",
                GlyphMarginHoverMessage = new MarkdownString[] { new MarkdownString { Value = $"**Error**\r\n\r\n{message}" } }
            };

            return new ModelDeltaDecoration { Range = range, Options = options };
        }
    }
}
