namespace DIPS_lab01.Data;

public interface IRepository<Model> where Model : class
{
    void Create(Model model);

    IEnumerable<Model> Read();

    Model? Read(int id);

    void Update(Model model);

    void Delete(int id);
}
