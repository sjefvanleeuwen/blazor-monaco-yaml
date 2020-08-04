using BlazorMonaco;
using BlazorMonaco.Bridge;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlazorMonacoYaml
{
    public partial class MonacoDiffEditorYaml
    {
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

        private MonacoDiffEditor _monacoDiffEditor { get; set; }

        private const string _language = "yaml";

        private DiffEditorConstructionOptions DiffEditorConstructionOptions(MonacoDiffEditor editor)
        {
            return new DiffEditorConstructionOptions
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
                await MonacoEditorBase.GetModel(originalId) ??
                await MonacoEditorBase.CreateModel(OriginalValue, _language, originalId);

            var modifiedModel =
                await MonacoEditorBase.GetModel(modifiedId) ??
                await MonacoEditorBase.CreateModel(ModifiedValue, _language, modifiedId);

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
