using Microsoft.AspNetCore.Components;

namespace Cineflex.Components.Shared
{
    public partial class CustomSwipeItem : IAsyncDisposable
    {
        [Parameter] public string Title { get; set; }
        [Parameter] public string Image { get; set; }

        /// <summary>
        /// Hex code for color
        /// </summary>
        [Parameter] public string TitleColor { get; set; }

        private string _imageStyle { get; set; } = "min-height:80%; max-height:80%;";
        private string _titleStyle { get; set; } = "border-radius:25px;";

        public ValueTask DisposeAsync()
        {
            _imageStyle = "min-height:80%; max-height:80%;";
            _titleStyle = "border-radius:25px;";
            return ValueTask.CompletedTask;
        }

        protected override Task OnInitializedAsync()
        {
            if(!string.IsNullOrEmpty(Title))
            {
                _titleStyle += $"color:{TitleColor};";
            }
            return base.OnInitializedAsync();
        }
        
    }
}