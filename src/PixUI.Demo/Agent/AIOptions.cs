namespace PixUI.Demo.Agent;

public static class AIOptions
{
    public static string Provider { get; set; } = "ollama";
    public static string Url { get; set; } = "http://localhost:11434/";
    public static string Model { get; set; } = "qwen3.8:27b-dflash2-nvfp4";
}