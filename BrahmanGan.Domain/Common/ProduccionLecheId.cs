namespace BrahmanGan.Domain.Common;

public sealed class ProduccionLecheId : IntId     { private ProduccionLecheId(int v) : base(v) { } public static ProduccionLecheId From(int v) => new(v); public static ProduccionLecheId New() => new(0); }
