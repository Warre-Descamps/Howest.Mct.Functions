namespace Howest.Mct.Services;

public static class CsvHelper
{
    public static string WriteCsv<T>(IEnumerable<T> objects)
    {
        var props = typeof(T).GetProperties();
        
        var header = string.Join(',', props.Select(p => p.Name));

        var csv = header + Environment.NewLine;

        foreach (var obj in objects)
        {
            foreach (var prop in props)
            {
                csv += prop.GetValue(obj)?.ToString() + ',';
            }

            if (csv.EndsWith(','))
                csv = csv[..^1];
            csv += Environment.NewLine;
        }
        
        var filename = $"registrations{DateTime.Now:yyyyMMddHHmmss}.csv";
        File.WriteAllText(filename,csv);

        return filename;
    }
}