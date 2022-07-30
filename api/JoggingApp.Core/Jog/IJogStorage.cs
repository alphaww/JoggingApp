namespace JoggingApp.Core.Jogs
{
    public interface IJogStorage
    {
        Task<IEnumerable<Jog>> SearchAsync(Guid? userId, DateTime? from, DateTime? to, CancellationToken cancellation = default);
        Task<Jog> GetByUserIdJogIdAsync(Guid userId, Guid jogId, CancellationToken cancellation = default);
        Task<Jog> GetByJogIdAsync(Guid jogId, CancellationToken cancellation = default);
        Task InsertAsync(Jog jogToInsert, CancellationToken cancellation = default);
        Task UpdateAsync(Jog jogToUpdate, CancellationToken cancellation = default);
        Task DeleteAsync(Jog jogToDelete, CancellationToken cancellation = default);
    }
}
