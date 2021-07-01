using Hy.FantasyGrpcEFCore.Domain;
using Hy.FantasyGrpcEFCore.IServices;
using Hy.FantasyGrpcEFCore.Repositorys;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hy.FantasyGrpcEFCore.Service
{
    public class GameAccountService : IBaseService
    {
        private readonly IRepository<GameAccount, int> _repository;

        public GameAccountService(IRepository<GameAccount, int> accountRepository)
        {
            _repository = accountRepository;
        }

        public async Task<List<GameAccount>> GetListAsync()
        {
            var result = _repository.GetAllList(n => n.Id > 100).ToList();
            return result;
        }
    }
}