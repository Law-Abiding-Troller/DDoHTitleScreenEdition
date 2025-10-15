using Nautilus.Options.Attributes;
using Nautilus.Json;
using Nautilus.Options;

namespace HoverfishTitleScreen;

[Menu("Daily Dose of Hoverfish, title screen edition")]
public class ConfigOptions : ConfigFile
{
    [Slider("Amount of Hoverfish (requires restart)", 0, 1000, DefaultValue = 27, Tooltip = "WARNING: High numbers of HoverFish may slow down or even crash your game! DDOH is not responsible for purposeful slowing or crashing of your game."), OnChange(nameof(SliderChangedEvent))]
    public int HoverFishCount = 27;
    [Slider("Multiplier for Hoverfish (requires restart)", 0 ,100, DefaultValue = 1, Tooltip = "WARNING: High mutliplier will lead to high numbers of Hoverfish. See Tooltip for 'Amount of Hoverfish (requires restart)'"), OnChange(nameof(SliderChangedEvent))]
    public int Multiplier = 1;
    [Toggle("Fail (requires restart)", Tooltip = "Note: This will disable all of the hoverfish.")]
    public bool CauseException = false;

    void SliderChangedEvent(SliderChangedEventArgs e)
    {
        TitleScreen.HoverfishWTOBH.LessOrMoreHoverfish(HoverFishCount*Multiplier);
    }
}