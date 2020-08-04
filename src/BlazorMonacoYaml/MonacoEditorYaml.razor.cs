using BlazorMonaco;
using BlazorMonaco.Bridge;
using BlazorMonacoYaml.Helpers;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorMonacoYaml
{
    public partial class MonacoEditorYaml
    {
        [Parameter]
        public string Id { get; set; }
        [Parameter]
        public string CssClass { get; set; }
        [Parameter]
        public string Value { get; set; }
        [Parameter]
        public EventCallback<KeyboardEvent> OnKeyUp { get; set; }
        [Parameter]
        public EventCallback<PasteEvent> OnDidPaste { get; set; }
        [Parameter]
        public EventCallback OnDidInit { get; set; }

        private string[] DeltaDecorations { get; set; }
        public MonacoEditor MonacoEditor { get; set; }

        private const string _language = "yaml";

        private StandaloneEditorConstructionOptions EditorConstructionOptions(MonacoEditor editor)
        {
            return new StandaloneEditorConstructionOptions
            {
                AutomaticLayout = true,
                Language = _language,
                GlyphMargin = true,
                Value = Value
            };
        }

        public async Task DoOnDidPaste(PasteEvent arg)
        {
            if (!OnDidPaste.HasDelegate)
            {
                await ResetDeltaDecorations();
            }
            else
            {
                await OnDidPaste.InvokeAsync(arg);
            }
        }

        public async Task DoOnDidInit()
        {
            await OnDidInit.InvokeAsync(null);
        }

        public async Task<ModelDeltaDecoration> BuildDeltaDecoration(BlazorMonaco.Bridge.Range range, string message)
        {
            return await DeltaDecorationHelper.BuildDeltaDecoration(MonacoEditor, range, message);
        }

        public async Task SetDeltaDecoration(IEnumerable<ModelDeltaDecoration> deltaDecorations)
        {
            DeltaDecorations = await DeltaDecorationHelper.SetDeltaDecorations(MonacoEditor, DeltaDecorations, deltaDecorations.ToArray());
        }

        public async Task ResetDeltaDecorations()
        {
            DeltaDecorations = await DeltaDecorationHelper.ResetDeltaDecorations(MonacoEditor);
        }

        public async Task SetValue(string value)
        {
            await MonacoEditor.SetValue(value);
        }

        public async Task<string> GetValue()
        {
            return await MonacoEditor.GetValue();
        }

        public async Task Layout()
        {
            await MonacoEditor.Layout();
        }
    }
}
