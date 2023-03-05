namespace pong_inari.uicontrols
{
    public enum Switcher
    {
        Left,
        Right,
        None
    }
    public interface IGameMenu
    {
        MenuControl MenuControl { get; set; }
    }
}
