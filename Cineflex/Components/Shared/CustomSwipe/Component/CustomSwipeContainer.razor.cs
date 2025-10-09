using Microsoft.AspNetCore.Components;
using Cineflex.Components.Shared.CustomSwipe.Service;
namespace Cineflex.Components.Shared.CustomSwipe.Component
{
    public partial class CustomSwipeContainer<TData> : IAsyncDisposable
    {
        /// <summary>
        /// Items to be displayed in the carousel.
        /// </summary>
        [Parameter] public IEnumerable<TData> Items { get; set; }
        [Parameter] public RenderFragment<TData> ItemTemplate { get; set; }

        /// <summary>
        /// The value to how many items being shown in the carousel.
        /// Default is 3.
        /// </summary>
        [Parameter] public int ShowItems { get; set; } = 3;
        [Parameter] public string Style { get; set; }
        [Parameter] public string Height { get; set; } = "200px";

        private CustomSwipeParameter _service = new CustomSwipeParameter();
        private string _itemStyle { get; set; } = string.Empty; 

        private string _style = string.Empty;
        private bool _disposing;
        protected override Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(Style))
            {
                _style = $"{Style}height:{Height};";
            }
            SetParameter();
            return base.OnInitializedAsync();
        }
        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
        }
        private void SetParameter()
        {
            var parameter = new Parameter
            {
                ShowItems = ShowItems
            };
            _service.SetCurrentParameter(parameter);
        }
    }
}