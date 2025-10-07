using Nautilus.Options.Attributes;
using Nautilus.Json;

namespace HoverfishTitleScreen;

[Menu("Daily Dose of Hoverfish, title screen edition")]
public class ConfigOptions : ConfigFile
{
    [Slider("Amount of Hoverfish (requires restart)", 0, 150, DefaultValue = 27, Tooltip = "Max is 150 to not crash your game (macOS sucks)")]
    public int HoverFishCount = 27;
}