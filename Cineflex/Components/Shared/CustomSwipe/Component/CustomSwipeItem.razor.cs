using Cineflex.Components.Shared.CustomSwipe.Service;
using Microsoft.AspNetCore.Components;

namespace Cineflex.Components.Shared.CustomSwipe.Component
{
    public partial class CustomSwipeItem: IAsyncDisposable
    {
        [CascadingParameter] public CustomSwipeParameter ParentParameter { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public string Image { get; set; }
        [Parameter] public string Style { get; set; }

        /// <summary>
        /// Hex code for color
        /// </summary>
        [Parameter] public string TitleColor { get; set; }
        private string _style = string.Empty;
       
        private string _titleStyle { get; set; } = "border-radius:25px;";

        public ValueTask DisposeAsync()
        {
            
            _titleStyle = "border-radius:25px;";
            return ValueTask.CompletedTask;
        }

        protected override Task OnInitializedAsync()
        {
            if(!string.IsNullOrEmpty(Style))
            {
                _style = _style + Style;
            }
            if(!string.IsNullOrEmpty(Title))
            {
                _titleStyle += $"color:{TitleColor};";
            }
            _style += $"max-width:{100-10 / ParentParameter.GetCurrentParameter().ShowItems}%;";
            return base.OnInitializedAsync();
        }
        
    }
}