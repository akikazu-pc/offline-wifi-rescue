namespace AkikazuHardwareCheck.Core.Models;

public sealed record ComputerInfo(string? Manufacturer, string? Model, bool IsLaptop);

public sealed record BatteryInfo(ulong DesignedMilliwattHours, ulong FullMilliwattHours, int HealthPercent);

public enum NetworkKind
{
    WiFi,
    Wired,
    None,
}

/// <summary>How the PC reaches the network now. Addresses and network names are deliberately absent.</summary>
public sealed record NetworkConnection(NetworkKind Kind, string? Adapter, long? LinkMegabits, int? SignalPercent = null);
