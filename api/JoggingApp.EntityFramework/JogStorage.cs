using JoggingApp.Core;
using JoggingApp.Core.Jogs;
using Microsoft.EntityFrameworkCore;

namespace JoggingApp.EntityFramework
{
    public class JogStorage : IJogStorage
    {
        private readonly JoggingAppDbContext _context;
        public JogStorage(JoggingAppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Jog>> SearchAsync(Guid? userId, DateTime? from, DateTime? to, CancellationToken cancellation = default)
        {
            return await _context.Jogs
                .Include(jog => jog.JogLocation)
                .Where(jog => 
            (!userId.HasValue || jog.UserId == userId)
            && 
            (!from.HasValue || jog.Date >= from)
            && (!to.HasValue || jog.Date <= to))
                .OrderByDescending(jog => jog.Date)
                .ToListAsync(cancellation);
        }

        public async Task<Jog> GetByUserIdJogIdAsync(Guid userId, Guid jogId, CancellationToken cancellation = default)
        {
            return await _context.Jogs.SingleOrDefaultAsync(jog => jog.UserId == userId && jog.Id == jogId, cancellation);
        }
        public async Task<Jog> GetByJogIdAsync(Guid jogId, CancellationToken cancellation = default)
        {
            return await _context.Jogs.SingleOrDefaultAsync(jog => jog.Id == jogId, cancellation);
        }

        public async Task DeleteAsync(Jog jogToDelete, CancellationToken cancellation = default)
        {
            _context.Jogs.Remove(jogToDelete);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task InsertAsync(Jog jogToInsert, CancellationToken cancellation = default)
        {
            await _context.Jogs.AddAsync(jogToInsert, cancellation);
            await _context.SaveChangesAsync(cancellation);
        }

        public async Task UpdateAsync(Jog jogToUpdate, CancellationToken cancellation = default)
        {
            _context.ChangeTracker.Clear();
            _context.Jogs.Update(jogToUpdate);
            await _context.SaveChangesAsync(cancellation);
        }
    }
}
