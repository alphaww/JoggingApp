namespace JoggingApp.Core.Jogs
{
    public interface IJogStorage
    {
        Task<IEnumerable<Jog>> SearchAsync(Guid userId, DateTime? from, DateTime? to);
        Task<Jog> GetByUserIdJogIdAsync(Guid userId, Guid jogId);
        Task<Jog> GetByJogId(Guid jogId);
        Task InsertAsync(Jog jogToInsert);
        Task UpdateAsync(Jog jogToUpdate);
        Task DeleteAsync(Jog jogToDelete);

    }
}
