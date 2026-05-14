namespace BrahmanGan.Domain.Common;

public sealed class ControlPreventivoId : IntId         { private ControlPreventivoId(int v) : base(v) { } public static ControlPreventivoId From(int v) => new(v); public static ControlPreventivoId New() => new(0); }
