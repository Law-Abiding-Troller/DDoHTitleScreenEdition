using Nautilus.Options.Attributes;
using Nautilus.Json;

namespace HoverfishTitleScreen;

[Menu("Daily Dose of Hoverfish, title screen edition")]
public class ConfigOptions : ConfigFile
{
    [Slider("Amount of Hoverfish (requires restart)")]
    public int HoverFishCount = 27;
}