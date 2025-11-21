using FactorPolar.Application.Interfaces;
using FactorPolar.Domain.Entities;
using FactorPolar.Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;

namespace FactorPolar.Infrastructure.Repositories
{
    public class CredentialRepository(FactorDbContext context) : ICredentialRepository
    {
        private readonly FactorDbContext _context = context;
        public async Task<Credential> GetCredentialToken(string accessToken)
        {
            var credential = await _context.Credentials.FirstOrDefaultAsync(x => x.AccessToken == accessToken);
            return credential!;
        }

        public async Task<Credential> GetCredentialUserId(Guid userId)
        {
            var credential = await _context.Credentials.FirstOrDefaultAsync(x => x.UserId == userId);
            return credential!;
        }

        public async Task<bool> CreateCredential(Credential credential)
        {
            await _context.Credentials.AddAsync(credential);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCredential(string accessToken)
        {
            var credential = await _context.Credentials.FirstOrDefaultAsync(x => x.AccessToken == accessToken);
            _context.Credentials.Remove(credential);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
