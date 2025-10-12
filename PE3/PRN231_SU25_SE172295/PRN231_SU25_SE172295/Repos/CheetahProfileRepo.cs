using BOs;
using DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public interface ICheetahProfileRepo
    {
        Task<List<CheetahProfile>> GetAll();
        Task<CheetahProfile> GetById(int id);
        Task<CheetahProfile> Add(CheetahProfile cheetahProfile);
        Task<CheetahProfile> Update(int id, CheetahProfile cheetahProfile);
        Task<CheetahProfile> Delete(int id);
    }
    public class CheetahProfileRepo : ICheetahProfileRepo
    {
        public async Task<CheetahProfile> Add(CheetahProfile cheetahProfile)
        {
            return await CheetahProfileDAO.Instance.Add(cheetahProfile);
        }

        public async Task<CheetahProfile> Delete(int id)
        {
            return await CheetahProfileDAO.Instance.Delete(id);
        }

        public async Task<CheetahProfile> GetById(int id)
        {
            return await CheetahProfileDAO.Instance.GetById(id);
        }

        public async Task<List<CheetahProfile>> GetAll()
        {
            return await CheetahProfileDAO.Instance.GetAll();
        }

        public async Task<CheetahProfile> Update(int id, CheetahProfile cheetahProfile)
        {
            return await CheetahProfileDAO.Instance.Update(id, cheetahProfile);
        }
    }
}
