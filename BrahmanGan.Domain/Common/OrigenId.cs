namespace BrahmanGan.Domain.Common;

public sealed class OrigenId : IntId          { private OrigenId(int v) : base(v) { } public static OrigenId From(int v) => new(v); public static OrigenId New() => new(0); }
