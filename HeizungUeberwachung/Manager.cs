using HeizungUeberwachung.ServiceDto;

namespace HeizungUeberwachung;

public class Manager
{
    private Repository repository;

    public Manager(Repository repository)
    {
        this.repository = repository;
    }

    public Status GetStatus()
    {
        double temperature = repository.GetTemperature();
        return new Status(State.Off.ToString(), temperature);
    }

    public void SetNewTemperature(double temperature)
    {
        TemperatureLog log = new(DateTime.Now, temperature);
        repository.LogTemperature(log);
    }
}
