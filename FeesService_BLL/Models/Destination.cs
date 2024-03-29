﻿namespace FeesService_BLL.Models;

public class Destination
{
    public int Id { get; init; }
    public int SectionId { get; init; }
    public long Currency { get; init; }
    public int Priority { get; init; }
    public int FromType { get; init; }
    public int FromId { get; init; }
    public int ToType { get; init; }
    public int ToId { get; init; }
    public bool Blocked { get; init; }
    public int SectionPriority { get; init; }
    public Destination() { }
}
