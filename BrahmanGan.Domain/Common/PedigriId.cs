namespace BrahmanGan.Domain.Common;

public sealed class PedigriId : IntId         { private PedigriId(int v) : base(v) { } public static PedigriId From(int v) => new(v); public static PedigriId New() => new(0); }
