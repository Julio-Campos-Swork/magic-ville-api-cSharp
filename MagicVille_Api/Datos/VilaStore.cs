using MagicVille_Api.Modelos.Dto;

namespace MagicVille_Api.Datos
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        {
            new VillaDto{Id = 1, Nombre = "Vista a la piscina", Ocupantes = 5, MetrosCuadrados = 5},
                new VillaDto{Id = 2, Nombre = "Vista a la playa", Ocupantes = 5, MetrosCuadrados = 5},
        };
    }
}
