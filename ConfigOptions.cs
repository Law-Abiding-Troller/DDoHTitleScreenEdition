using Nautilus.Options.Attributes;
using Nautilus.Json;

namespace HoverfishTitleScreen;

[Menu("Daily Dose of Hoverfish, title screen edition")]
public class ConfigOptions : ConfigFile
{
    [Slider("Amount of Hoverfish (requires restart)", 0, 1000, DefaultValue = 27, Tooltip = "WARNING: High numbers of HoverFish may slow down or even crash your game! DDOH is not responsible for purposeful slowing or crashing of your game.")]
    public int HoverFishCount = 27;
}