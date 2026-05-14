namespace BrahmanGan.Domain.Common;

public sealed class CapturaCarbonoId : IntId    { private CapturaCarbonoId(int v) : base(v) { } public static CapturaCarbonoId From(int v) => new(v); public static CapturaCarbonoId New() => new(0); }
