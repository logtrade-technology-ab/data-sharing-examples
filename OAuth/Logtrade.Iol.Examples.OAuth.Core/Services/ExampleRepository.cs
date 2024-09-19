using Logtrade.Iol.Examples.OAuth.Core.Models.ExampleRepository;

namespace Logtrade.Iol.Examples.OAuth.Core.Services;
public class ExampleRepository
{
    private readonly List<ConnectionRecord> inMemoryDatabase = [];

    public void Insert(ConnectionRecord model)
    {
        model.ConnectionRecordId = inMemoryDatabase.Count != 0 ? inMemoryDatabase.Max(m => m.ConnectionRecordId) + 1 : 1;
        inMemoryDatabase.Add(model);
    }

    public ConnectionRecord? Get(int id)
    {
        return inMemoryDatabase.FirstOrDefault(c => c.ConnectionRecordId == id);
    }

    public ConnectionRecord? GetByState(string state)
    {
        return inMemoryDatabase.FirstOrDefault(c => c.State == state);
    }

    public IEnumerable<ConnectionRecord> GetAll() => inMemoryDatabase;
}