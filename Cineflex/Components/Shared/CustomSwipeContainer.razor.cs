
using Microsoft.AspNetCore.Components;

namespace Cineflex.Components.Shared
{
    public partial class CustomSwipeContainer<TData> : IAsyncDisposable
    {
        /// <summary>
        /// Items to be displayed in the carousel.
        /// </summary>
        [Parameter] public IEnumerable<TData> Items { get; set; }
        [Parameter] public RenderFragment<TData>? ItemTemplate { get; set; }

        /// <summary>
        /// The value to how many items being shown in the carousel.
        /// Default is 3.
        /// </summary>
        [Parameter] public int ShowItems { get; set; } = 3;
        [Parameter] public string Height { get; set; } = "20vh";
        [Parameter] public RenderFragment? ChildContent { get; set; }

        private bool _disposing;
        protected override Task OnInitializedAsync()
        {

            return base.OnInitializedAsync();
        }
        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
        }
    }
}