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

        public async Task<IEnumerable<Jog>> SearchAsync(Guid userId, DateTime? from, DateTime? to)
        {
            return await _context.Jogs
                .Include(jog => jog.JogLocation)
                .Where(jog => 
            jog.UserId == userId && 
            (!from.HasValue || jog.Date >= from)
            && (!to.HasValue || jog.Date <= to))
                .OrderByDescending(jog => jog.Date)
                .ToListAsync();
        }

        public async Task<Jog> GetByUserIdJogIdAsync(Guid userId, Guid jogId)
        {
            return await _context.Jogs.SingleOrDefaultAsync(jog => jog.UserId == userId && jog.Id == jogId);
        }
        public async Task<Jog> GetByJogId(Guid jogId)
        {
            return await _context.Jogs.SingleOrDefaultAsync(jog => jog.Id == jogId);
        }

        public async Task DeleteAsync(Jog jogToDelete)
        {
            _context.Jogs.Remove(jogToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task InsertAsync(Jog jogToInsert)
        {
            await _context.Jogs.AddAsync(jogToInsert);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Jog jogToUpdate)
        {
            _context.Jogs.Update(jogToUpdate);
            await _context.SaveChangesAsync();
        }
    }
}
