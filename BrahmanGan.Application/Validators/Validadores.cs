using FluentValidation;
using BrahmanGan.Application.DTOs;

namespace BrahmanGan.Application.Validators;

// ===== Fase 1 =====
public sealed class CrearAnimalRequestValidator : AbstractValidator<CrearAnimalRequest>
{
    public CrearAnimalRequestValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.IdRaza).GreaterThan(0).WithMessage("Raza requerida");
        RuleFor(x => x.IdFinca).GreaterThan(0).WithMessage("Finca requerida");
        RuleFor(x => x.Nombre).MaximumLength(80);
        RuleFor(x => x.PesoNacimiento).GreaterThanOrEqualTo(0).When(x => x.PesoNacimiento.HasValue);
        RuleFor(x => x.FechaNacimiento)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .When(x => x.FechaNacimiento.HasValue)
            .WithMessage("La fecha de nacimiento no puede ser futura");
        RuleFor(x => x.Observaciones).MaximumLength(500);
    }
}

public sealed class CrearRazaRequestValidator : AbstractValidator<CrearRazaRequest>
{
    public CrearRazaRequestValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(80);
    }
}

public sealed class RegistrarPesajeRequestValidator : AbstractValidator<RegistrarPesajeRequest>
{
    public RegistrarPesajeRequestValidator()
    {
        RuleFor(x => x.IdAnimal).GreaterThan(0);
        RuleFor(x => x.PesoKg).GreaterThan(0).WithMessage("El peso debe ser > 0");
        RuleFor(x => x.CondicionCorporal).InclusiveBetween(1, 9)
            .When(x => x.CondicionCorporal.HasValue)
            .WithMessage("Condición corporal en escala 1-9");
    }
}

public sealed class TrasladarAnimalRequestValidator : AbstractValidator<TrasladarAnimalRequest>
{
    public TrasladarAnimalRequestValidator() { RuleFor(x => x.NuevaFinca).GreaterThan(0); }
}

// ===== Fase 2 =====
public sealed class CrearFincaRequestValidator : AbstractValidator<CrearFincaRequest>
{
    public CrearFincaRequestValidator()
    {
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
        RuleFor(x => x.NIT).MaximumLength(20);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.AreaHectareas).GreaterThanOrEqualTo(0).When(x => x.AreaHectareas.HasValue);
    }
}

public sealed class CrearPotreroRequestValidator : AbstractValidator<CrearPotreroRequest>
{
    public CrearPotreroRequestValidator()
    {
        RuleFor(x => x.IdFinca).GreaterThan(0);
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(15);
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(80);
        RuleFor(x => x.AreaHectareas).GreaterThanOrEqualTo(0).When(x => x.AreaHectareas.HasValue);
    }
}

// ===== Fase 3 =====
public sealed class RegistrarMontaRequestValidator : AbstractValidator<RegistrarMontaRequest>
{
    public RegistrarMontaRequestValidator()
    {
        RuleFor(x => x.IdHembra).GreaterThan(0);
        RuleFor(x => x.IdToro).GreaterThan(0);
        RuleFor(x => x).Must(x => x.IdHembra != x.IdToro).WithMessage("Hembra y toro deben ser diferentes");
    }
}

public sealed class RegistrarIaRequestValidator : AbstractValidator<RegistrarIaRequest>
{
    public RegistrarIaRequestValidator()
    {
        RuleFor(x => x.IdHembra).GreaterThan(0);
        RuleFor(x => x.IdSemen).GreaterThan(0);
    }
}

public sealed class IniciarGestacionRequestValidator : AbstractValidator<IniciarGestacionRequest>
{
    public IniciarGestacionRequestValidator() { RuleFor(x => x.IdAnimal).GreaterThan(0); }
}

// ===== Fase 4 =====
public sealed class CrearMedicamentoRequestValidator : AbstractValidator<CrearMedicamentoRequest>
{
    public CrearMedicamentoRequestValidator()
    {
        RuleFor(x => x.Codigo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Nombre).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PrecioUnitario).GreaterThanOrEqualTo(0).When(x => x.PrecioUnitario.HasValue);
        RuleFor(x => x.TiempoCarne).GreaterThanOrEqualTo(0).When(x => x.TiempoCarne.HasValue);
        RuleFor(x => x.TiempoLeche).GreaterThanOrEqualTo(0).When(x => x.TiempoLeche.HasValue);
    }
}

public sealed class AplicarVacunaRequestValidator : AbstractValidator<AplicarVacunaRequest>
{
    public AplicarVacunaRequestValidator()
    {
        RuleFor(x => x.IdAnimal).GreaterThan(0);
        RuleFor(x => x.IdMedicamento).GreaterThan(0);
        RuleFor(x => x.Dosis).GreaterThanOrEqualTo(0).When(x => x.Dosis.HasValue);
        RuleFor(x => x.ProximaFecha).GreaterThanOrEqualTo(x => x.Fecha)
            .When(x => x.ProximaFecha.HasValue)
            .WithMessage("Próxima fecha debe ser >= fecha actual");
    }
}

// ===== Fase 5 =====
public sealed class RegistrarControlLecheRequestValidator : AbstractValidator<RegistrarControlLecheRequest>
{
    public RegistrarControlLecheRequestValidator()
    {
        RuleFor(x => x.IdAnimal).GreaterThan(0);
        RuleFor(x => x).Must(x => (x.Maniana ?? 0) + (x.Tarde ?? 0) + (x.Noche ?? 0) > 0)
            .WithMessage("Debe registrarse al menos un ordeño con litros > 0");
        RuleFor(x => x.Maniana).GreaterThanOrEqualTo(0).When(x => x.Maniana.HasValue);
        RuleFor(x => x.Tarde).GreaterThanOrEqualTo(0).When(x => x.Tarde.HasValue);
        RuleFor(x => x.Noche).GreaterThanOrEqualTo(0).When(x => x.Noche.HasValue);
    }
}

public sealed class RegistrarProduccionLecheRequestValidator : AbstractValidator<RegistrarProduccionLecheRequest>
{
    public RegistrarProduccionLecheRequestValidator()
    {
        RuleFor(x => x.IdFinca).GreaterThan(0);
        RuleFor(x => x.TotalLitros).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Vendidos).GreaterThanOrEqualTo(0).When(x => x.Vendidos.HasValue);
        RuleFor(x => x.Autoconsumo).GreaterThanOrEqualTo(0).When(x => x.Autoconsumo.HasValue);
        RuleFor(x => x.Merma).GreaterThanOrEqualTo(0).When(x => x.Merma.HasValue);
        RuleFor(x => x).Must(x => (x.Vendidos ?? 0) + (x.Autoconsumo ?? 0) + (x.Merma ?? 0) <= x.TotalLitros)
            .WithMessage("La suma de desgloses no puede superar el total");
    }
}

public sealed class RegistrarVentaLecheRequestValidator : AbstractValidator<RegistrarVentaLecheRequest>
{
    public RegistrarVentaLecheRequestValidator()
    {
        RuleFor(x => x.IdCliente).GreaterThan(0);
        RuleFor(x => x.Litros).GreaterThan(0);
        RuleFor(x => x.PrecioLitro).GreaterThan(0);
    }
}

// ===== Fase 6 =====
public sealed class CrearClienteRequestValidator : AbstractValidator<CrearClienteRequest>
{
    public CrearClienteRequestValidator()
    {
        RuleFor(x => x.Documento).NotEmpty().MaximumLength(20);
        RuleFor(x => x.RazonSocial).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.Telefono).MaximumLength(30);
    }
}

public sealed class CrearContratoRequestValidator : AbstractValidator<CrearContratoRequest>
{
    public CrearContratoRequestValidator()
    {
        RuleFor(x => x.IdCliente).GreaterThan(0);
        RuleFor(x => x.Tipo).NotEmpty().MaximumLength(20);
        RuleFor(x => x.PrecioAcordado).GreaterThanOrEqualTo(0).When(x => x.PrecioAcordado.HasValue);
        RuleFor(x => x.FechaFin).GreaterThanOrEqualTo(x => x.FechaInicio)
            .When(x => x.FechaFin.HasValue);
    }
}

public sealed class CrearCotizacionRequestValidator : AbstractValidator<CrearCotizacionRequest>
{
    public CrearCotizacionRequestValidator()
    {
        RuleFor(x => x.IdCliente).GreaterThan(0);
        RuleFor(x => x.PrecioOfertado).GreaterThan(0);
        RuleFor(x => x.FechaVigencia).GreaterThanOrEqualTo(x => x.Fecha).When(x => x.FechaVigencia.HasValue);
    }
}
