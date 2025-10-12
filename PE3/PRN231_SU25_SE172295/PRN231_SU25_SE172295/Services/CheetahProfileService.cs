using BOs;
using Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface ICheetahProfileService
    {
        Task<List<CheetahProfile>> GetAll();
        Task<CheetahProfile> GetById(int id);
        Task<CheetahProfile> Add(CheetahProfile cheetahProfile);
        Task<CheetahProfile> Update(int id, CheetahProfile cheetahProfile);
        Task<CheetahProfile> Delete(int id);
    }
    public class CheetahProfileService : ICheetahProfileService
    {
        private readonly ICheetahProfileRepo _repo;
        public CheetahProfileService(ICheetahProfileRepo cheetahProfileRepo)
        {
            _repo = cheetahProfileRepo;
        }

        public async Task<CheetahProfile> Add(CheetahProfile cheetahProfile)
        {
            return await _repo.Add(cheetahProfile);
        }

        public async Task<CheetahProfile> Delete(int id)
        {
            return await _repo.Delete(id);
        }

        public async Task<CheetahProfile> GetById(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task<List<CheetahProfile>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<CheetahProfile> Update(int id, CheetahProfile cheetahProfile)
        {
            return await _repo.Update(id, cheetahProfile);
        }
    }
}