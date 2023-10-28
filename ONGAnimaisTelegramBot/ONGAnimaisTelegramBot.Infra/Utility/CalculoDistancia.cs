using ONGAnimaisTelegramBot.Infra.Vendors.Entities;
using DecimalMath;
public class Local
{
    public string Nome { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}

public static class CalculoDistancia
{
    public static List<ONG> ObterOngsProximas(decimal usuarioLatitude, decimal usuarioLongitude, List<ONG> ongsGeolocalizacao)
    {
        // Coordenadas da sua casa
        var suaCasa = new Local()
        {
            Latitude = usuarioLatitude,
            Longitude = usuarioLongitude
        };

        // Lista de locais com suas coordenadas
        var listaDeLocais = ongsGeolocalizacao.Select(o => new Local
        {
            Nome = o.Nome,
            Latitude = o.Geolocalizacao.Latitude,
            Longitude = o.Geolocalizacao.Longitude,
        }).ToList();

        // Classifique os locais com base na distância para sua casa
        listaDeLocais.Sort((a, b) =>
        {
            var distanciaA = CalcularDistancia(suaCasa, a);
            var distanciaB = CalcularDistancia(suaCasa, b);
            return distanciaA.CompareTo(distanciaB);
        });

        var listaOngs = new List<ONG>();

        foreach(var local in listaDeLocais)
            listaOngs.Add(ongsGeolocalizacao.First(o => o.Nome == local.Nome));

        return listaOngs;

        //// Os locais na listaDeLocais estão agora classificados por proximidade.
        //Console.WriteLine("Locais mais próximos da sua casa:");
        //foreach (var local in listaDeLocais)
        //{
        //    decimal distanciaKm = CalcularDistancia(suaCasa, local);
        //    Console.WriteLine($"{local.Nome}: Distância {distanciaKm} km");
        //}
    }

    public static List<Evento> ObterEventosProximos(decimal usuarioLatitude, decimal usuarioLongitude, List<Evento> ongsGeolocalizacao)
    {
        // Coordenadas da sua casa
        var suaCasa = new Local()
        {
            Latitude = usuarioLatitude,
            Longitude = usuarioLongitude
        };

        // Lista de locais com suas coordenadas
        var listaDeLocais = ongsGeolocalizacao.Select(e => new Local
        {
            Nome = e.Nome,
            Latitude = e.Geolocalizacao.Latitude,
            Longitude = e.Geolocalizacao.Longitude,
        }).ToList();

        // Classifique os locais com base na distância para sua casa
        listaDeLocais.Sort((a, b) =>
        {
            var distanciaA = CalcularDistancia(suaCasa, a);
            var distanciaB = CalcularDistancia(suaCasa, b);
            return distanciaA.CompareTo(distanciaB);
        });

        var listaEventos = new List<Evento>();

        foreach (var local in listaDeLocais)
            listaEventos.Add(ongsGeolocalizacao.First(e => e.Nome == local.Nome));

        return listaEventos;
    }

    // Função para calcular a distância entre duas coordenadas em quilômetros usando a fórmula de Haversine
    private static decimal CalcularDistancia(Local coord1, Local coord2)
    {
        decimal earthRadius = 6371; // Raio médio da Terra em quilômetros

        decimal dLat = Convert.ToDecimal(Math.PI) * (coord2.Latitude - coord1.Latitude) / 180.0m;
        decimal dLon = Convert.ToDecimal(Math.PI) * (coord2.Longitude - coord1.Longitude) / 180.0m;

        decimal a = DecimalEx.Sin(dLat / 2m) * DecimalEx.Sin(dLat / 2m) +
                   DecimalEx.Cos(Convert.ToDecimal(Math.PI) * coord1.Latitude / 180.0m) * DecimalEx.Cos(Convert.ToDecimal(Math.PI) * coord2.Latitude / 180.0m) *
                   DecimalEx.Sin(dLon / 2m) * DecimalEx.Sin(dLon / 2m);

        decimal c = 2 * DecimalEx.ATan2(DecimalEx.Sqrt(a), DecimalEx.Sqrt(1 - a));

        decimal distance = earthRadius * c;
        return distance;
    }
}