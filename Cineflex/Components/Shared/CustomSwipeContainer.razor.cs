
using Microsoft.AspNetCore.Components;

namespace Cineflex.Components.Shared
{
    public partial class CustomSwipeContainer<T> : IAsyncDisposable
        where T : class
    {
        /// <summary>
        /// Items to be displayed in the carousel.
        /// </summary>
        [Parameter] public List<T> Items { get; set; } = new();

        /// <summary>
        /// The value to how many items being shown in the carousel.
        /// Default is 3.
        /// </summary>
        [Parameter] public int ShowItems { get; set; } = 3;
        
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