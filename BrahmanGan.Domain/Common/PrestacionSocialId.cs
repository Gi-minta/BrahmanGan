namespace BrahmanGan.Domain.Common;

public sealed class PrestacionSocialId : IntId  { private PrestacionSocialId(int v) : base(v) { } public static PrestacionSocialId From(int v) => new(v); public static PrestacionSocialId New() => new(0); }
