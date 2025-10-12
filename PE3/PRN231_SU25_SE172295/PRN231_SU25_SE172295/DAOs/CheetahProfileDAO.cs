using BOs;
using Microsoft.EntityFrameworkCore;

namespace DAOs
{
    public class CheetahProfileDAO
    {
        private static CheetahProfileDAO instance = null;

        private CheetahProfileDAO()
        {

        }

        public static CheetahProfileDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CheetahProfileDAO();
                }
                return instance;
            }
        }

        public async Task<List<CheetahProfile>> GetAll()
        {
            using (var context = new Su25cheetahDbContext())
            {
                var w = await context.CheetahProfiles.Include(c => c.CheetahType).ToListAsync();
                return w;
            }
        }

        public async Task<CheetahProfile> GetById(int id)
        {
            using (var context = new Su25cheetahDbContext())
            {
                var c = await context.CheetahProfiles.Include(s => s.CheetahType).FirstOrDefaultAsync(h => h.CheetahProfileId == id);
                return c;
            }
        }

        public async Task<CheetahProfile> Add(CheetahProfile cheetahProfile)
        {
            try
            {
                using (var context = new Su25cheetahDbContext())
                {
                    await context.CheetahProfiles.AddAsync(cheetahProfile);
                    await context.SaveChangesAsync();
                    return cheetahProfile;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
        }

        public async Task<CheetahProfile> Update(int id, CheetahProfile cheetahProfile)
        {
            using (var context = new Su25cheetahDbContext())
            {
                var infor = await context.CheetahProfiles.FirstOrDefaultAsync(c => c.CheetahProfileId == id);
                if (infor == null)
                {
                    throw new Exception("Not found");
                }
                infor.CheetahProfileId = id;
                infor.CheetahTypeId = cheetahProfile.CheetahTypeId;
                infor.CheetahName = cheetahProfile.CheetahName;
                infor.Weight = cheetahProfile.Weight;
                infor.Characteristics = cheetahProfile.Characteristics;
                infor.CareNeeds = cheetahProfile.CareNeeds;
                infor.ModifiedDate = cheetahProfile.ModifiedDate;

                context.Update(infor);

                await context.SaveChangesAsync();
                return infor;
            }
        }

        public async Task<CheetahProfile> Delete(int id)
        {
            try
            {
                using (var context = new Su25cheetahDbContext())
                {
                    var delete = await context.CheetahProfiles.FirstOrDefaultAsync(c => c.CheetahProfileId.Equals(id));
                    if (delete == null)
                    {
                        throw new Exception("Infor not found");
                    }

                    context.CheetahProfiles.Remove(delete);
                    await context.SaveChangesAsync();

                    return delete;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
