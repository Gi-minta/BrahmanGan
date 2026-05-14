using BrahmanGan.Domain.Modulos.Inventario;

namespace BrahmanGan.Application.DTOs;

public record CrearRazaRequest(string Codigo, string Nombre, PropositoRaza? Proposito);
