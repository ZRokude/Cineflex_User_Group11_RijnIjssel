using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata.Ecma335;

namespace Cineflex.Components.Shared.CustomSwipe.Service
{
    public class CustomSwipeParameter
    {
        private Parameter _currentParameter = new();
        public void SetCurrentParameter(Parameter parameter) => _currentParameter = parameter;
        public Parameter GetCurrentParameter()
        {
            return _currentParameter;
        }

    }
}
