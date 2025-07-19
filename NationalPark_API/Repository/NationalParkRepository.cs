using NationalPark_API.Data;
using NationalPark_API.Models;
using NationalPark_API.Repository.IRepository;

namespace NationalPark_API.Repository
{
    public class NationalParkRepository:INationalParkRepository
    {
        private readonly ApplicationDbContext _Context;
        public NationalParkRepository(ApplicationDbContext context)
        {
            _Context = context;
        }

        public bool CreateNationalPark(NationalPark nationalPark)
        {
            _Context.NationalParks.Add(nationalPark);
            return Save();
        }

        public bool DeleteNationalPark(NationalPark nationalPark)
        {
            _Context.NationalParks.Remove(nationalPark);
            return Save();
        }

        public NationalPark GetNationalPark(int nationalParkId)
        {
            return _Context.NationalParks.Find(nationalParkId);
        }

        public ICollection<NationalPark> GetNationalParks()
        {
            return _Context.NationalParks.ToList();
        }

        public bool NationalParkExists(int nationalParkId)
        {
            return _Context.NationalParks.Any(np=>np.Id== nationalParkId);
        }

        public bool NationalParkExists(string nationalParkName)
        {
          return _Context.NationalParks.Any(np=>np.Name== nationalParkName);
        }

        public bool Save()
        {
            return _Context.SaveChanges()==1?true:false;
        }

        public bool UpdateNationalPark(NationalPark nationalPark)
        {
            _Context.NationalParks.Update(nationalPark);
            return Save();
        }
    }
}
