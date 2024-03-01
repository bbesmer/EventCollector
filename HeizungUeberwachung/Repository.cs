namespace HeizungUeberwachung;

public class Repository
{
    private Context context;

    public Repository(Context context)
    {
        this.context = context;
    }

    public void LogTemperature(TemperatureLog log)
    {
        context.TemperatureLogs.Add(log);
        context.SaveChanges();
    }

    public double GetTemperature()
    {
        return context.TemperatureLogs.OrderByDescending(c => c.DateTime).FirstOrDefault()?.Temperature ?? 0;
    }
}
