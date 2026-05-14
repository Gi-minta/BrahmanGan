namespace BrahmanGan.Domain.Common;

public sealed class RegistroICAId : IntId       { private RegistroICAId(int v) : base(v) { } public static RegistroICAId From(int v) => new(v); public static RegistroICAId New() => new(0); }
