using BlazorMonaco;
using BlazorMonaco.Editor;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace BlazorMonacoYaml
{
    public partial class MonacoDiffEditorYaml
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [Parameter]
        public string Id { get; set; }
        [Parameter]
        public string CssClass { get; set; }
        [Parameter]
        public string OriginalValue { get; set; }
        [Parameter]
        public string ModifiedValue { get; set; }
        [Parameter]
        public EventCallback<KeyboardEvent> OnKeyUp { get; set; }
        [Parameter]
        public EventCallback OnDidInit { get; set; }

        private StandaloneDiffEditor _monacoDiffEditor { get; set; }

        private const string _language = "yaml";

        private StandaloneDiffEditorConstructionOptions DiffEditorConstructionOptions(StandaloneDiffEditor editor)
        {
            return new StandaloneDiffEditorConstructionOptions
            {
                AutomaticLayout = true,
                GlyphMargin = true
            };
        }

        private async Task EditorOnDidInit()
        {
            var originalId = $"{Id}-originalModel";
            var modifiedId = $"{Id}-modifiedModel";

            var originalModel =
                await Global.CreateModel(JSRuntime, OriginalValue, _language, originalId);

            var modifiedModel =
                await Global.CreateModel(JSRuntime, ModifiedValue, _language, modifiedId);

            //initialte the 2 yaml files
            await _monacoDiffEditor.SetModel(new DiffEditorModel
            {
                Original = originalModel,
                Modified = modifiedModel
            });

            //do the parent on init callback
            await OnDidInit.InvokeAsync(this);
        }

        public async Task<string> GetOriginalValue()
        {
            return await _monacoDiffEditor.OriginalEditor.GetValue();
        }

        public async Task SetOriginalValue(string value)
        {
            await _monacoDiffEditor.OriginalEditor.SetValue(value);
        }

        public async Task<string> GetModifiedValue()
        {
            return await _monacoDiffEditor.ModifiedEditor.GetValue();
        }

        public async Task SetModifiedValue(string value)
        {
            await _monacoDiffEditor.ModifiedEditor.SetValue(value);
        }

        public async Task Layout()
        {
            await _monacoDiffEditor.Layout();
        }
    }
}
