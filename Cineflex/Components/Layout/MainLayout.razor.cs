using MudBlazor;

namespace Cineflex.Components.Layout
{
    public partial class MainLayout
    {
        private bool chairsVisible;
        private bool _isDarkMode = true;
        private bool _drawerOpen = false;
        private MudTheme? _theme = null;

        protected override void OnInitialized()
        {
            _theme = new MudTheme()
            {
                PaletteLight = _lightPalette,
                PaletteDark = _darkPalette,
                LayoutProperties = new LayoutProperties()
            };
        }


        private readonly PaletteLight _lightPalette = new PaletteLight()
        {
            PrimaryContrastText = "rgba(255,255,255,1)",
            SecondaryContrastText = "rgba(255,255,255,1)",
            SecondaryDarken = "rgb(255,31,105)",
            SecondaryLighten = "rgb(255,102,153)",
            Secondary = "rgba(255,64,129,1)",
            TertiaryContrastText = "rgba(255,255,255,1)",
            TertiaryLighten = "rgb(42,223,187)",
            Tertiary = "rgba(30,200,165,1)",
            TertiaryDarken = "rgb(25,169,140)",
            InfoContrastText = "rgba(255,255,255,1)",
            SuccessContrastText = "rgba(255,255,255,1)",
            WarningContrastText = "rgba(255,255,255,1)",
            ErrorContrastText = "rgba(255,255,255,1)",
            DarkContrastText = "rgba(255,255,255,1)",
            GrayDark = "#757575",
            White = "rgba(255,255,255,1)",
            OverlayLight = "rgba(255,255,255,0.4980392156862745)",
            OverlayDark = "rgba(33,33,33,0.4980392156862745)",
            GrayDarker = "#616161",
            TableHover = "rgba(0,0,0,0.0392156862745098)",
            GrayLighter = "#E0E0E0",
            GrayLight = "#BDBDBD",
            GrayDefault = "#9E9E9E",
            RippleOpacitySecondary = 0.2,
            RippleOpacity = 0.1,
            HoverOpacity = 0.06,
            Surface = "rgba(255,255,255,1)",
            Background = "rgba(255,255,255,1)",
            Info = "rgba(33,150,243,1)",
            Dark = "rgba(66,66,66,1)",
            BackgroundGray = "rgba(245,245,245,1)",
            DrawerBackground = "rgba(255,255,255,1)",
            AppbarBackground = "rgba(89,74,226,1)",
            Black = "rgba(89,74,226,1)",
            TextPrimary = "rgba(66,66,66,1)",
            AppbarText = "rgba(255,255,255,1)",
            TextSecondary = "rgba(0,0,0,0.5372549019607843)",
            DrawerText = "rgba(66,66,66,1)",
            DrawerIcon = "rgba(97,97,97,1)",
            LinesInputs = "rgba(189,189,189,1)",
            ActionDisabled = "rgba(0,0,0,0.25882352941176473)",
            TextDisabled = "rgba(0,0,0,0.3764705882352941)",
            TableStriped = "rgba(0,0,0,0.0196078431372549)",
            Divider = "rgba(224,224,224,1)",
            LinesDefault = "rgba(0,0,0,0.11764705882352941)",
            ActionDisabledBackground = "rgba(0,0,0,0.11764705882352941)",
            TableLines = "rgba(224,224,224,1)",
            DividerLight = "rgba(0,0,0,0.8)",
            Warning = "rgba(255,152,0,1)",
            Error = "rgba(244,67,54,1)",
            ActionDefault = "rgba(0,0,0,0.5372549019607843)",
            Primary = "rgba(89,74,226,1)",
            Success = "rgba(0,200,83,1)",
            InfoLighten = "rgb(71,167,245)",
            PrimaryDarken = "rgb(62,44,221)",
            SuccessDarken = "rgb(0,163,68)",
            DarkLighten = "rgb(87,87,87)",
            WarningLighten = "rgb(255,167,36)",
            ErrorLighten = "rgb(246,96,85)",
            ErrorDarken = "rgb(242,28,13)",
            DarkDarken = "rgb(46,46,46)",
            WarningDarken = "rgb(214,129,0)",
            PrimaryLighten = "rgb(118,106,231)",
            SuccessLighten = "rgb(0,235,98)",
            InfoDarken = "rgb(12,128,223)",
        };

        private readonly PaletteDark _darkPalette = new PaletteDark()
        {
            Surface = "rgba(55,55,64,1)",
            Background = "rgba(50,51,61,1)",
            Info = "rgba(50,153,255,1)",
            Dark = "rgba(39,39,47,1)",
            BackgroundGray = "rgba(39,39,47,1)",
            DrawerBackground = "rgba(39,39,47,1)",
            AppbarBackground = "#3f0101",

            Black = "rgba(39,39,47,1)",
            TextPrimary = "rgba(255,255,255,0.6980392156862745)",
            AppbarText = "rgba(255,255,255,0.6980392156862745)",
            TextSecondary = "rgba(255,255,255,0.4980392156862745)",
            DrawerText = "rgba(255,255,255,0.4980392156862745)",
            DrawerIcon = "rgba(255,255,255,0.4980392156862745)",
            LinesInputs = "rgba(255,255,255,0.2980392156862745)",
            ActionDisabled = "rgba(255,255,255,0.25882352941176473)",
            TextDisabled = "rgba(255,255,255,0.2)",
            TableStriped = "rgba(255,255,255,0.2)",
            Divider = "rgba(255,255,255,0.11764705882352941)",
            LinesDefault = "rgba(255,255,255,0.11764705882352941)",
            ActionDisabledBackground = "rgba(255,255,255,0.11764705882352941)",
            TableLines = "rgba(255,255,255,0.11764705882352941)",
            DividerLight = "rgba(255,255,255,0.058823529411764705)",
            Warning = "rgba(255,168,0,1)",
            Error = "rgba(246,78,98,1)",
            ActionDefault = "rgba(173,173,177,1)",
            Primary = "rgba(119,107,231,1)",
            Success = "rgba(11,186,131,1)",
            InfoLighten = "rgb(92,173,255)",
            PrimaryDarken = "rgb(90,75,226)",
            SuccessDarken = "rgb(9,154,108)",
            DarkLighten = "rgb(56,56,67)",
            WarningLighten = "rgb(255,182,36)",
            ErrorLighten = "rgb(248,119,134)",
            ErrorDarken = "rgb(244,47,70)",
            DarkDarken = "rgb(23,23,28)",
            WarningDarken = "rgb(214,143,0)",
            PrimaryLighten = "rgb(151,141,236)",
            SuccessLighten = "rgb(13,222,156)",
            InfoDarken = "rgb(10,133,255)",
        };
    }
}