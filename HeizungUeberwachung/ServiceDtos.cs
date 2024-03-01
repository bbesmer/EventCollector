namespace HeizungUeberwachung.ServiceDto;

public record Temperature(double Value);

public record Status(string State, double Temperature);

