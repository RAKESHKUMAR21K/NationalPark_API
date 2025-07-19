using Microsoft.EntityFrameworkCore;
using NationalPark_API.Data;
using NationalPark_API.Models;
using NationalPark_API.Repository.IRepository;

namespace NationalPark_API.Repository
{
    public class TrailRepository:ITrailRepository
    {
        private readonly ApplicationDbContext _Context;
        public TrailRepository(ApplicationDbContext context)
        {
            _Context = context;
        }
        public bool CreateTrail(Trail trail)
        {
            _Context.Trails.Add(trail);
            return Save();
        }

        public bool DeleteTrail(Trail trail)
        {
            _Context.Trails.Remove(trail);
            return Save();
        }

        public Trail GetTrail(int trailId)
        {
            return _Context.Trails.Include(t => t.NationalPark).FirstOrDefault(t => t.Id == trailId);
        }

        public ICollection<Trail> GetTrailNationalPark(int nationalParkId)
        {
            return _Context.Trails.Include(t => t.NationalPark).
                Where(t => t.NationalParkId == nationalParkId).ToList();
        }

        public ICollection<Trail> GetTrails()
        {
            return _Context.Trails.Include(t => t.NationalPark).ToList();
        }

        public bool Save()
        {
            return _Context.SaveChanges()==1?true:false;
        }

        public bool TrailExists(int trailId)
        {
            return _Context.Trails.Any(t => t.Id == trailId);
        }

        public bool TrailExists(string trailName)
        {
            return _Context.Trails.Any(t=>t.Name== trailName);
        }

        public bool UpdateTrail(Trail trail)
        {
            _Context.Trails.Update(trail);
            return Save();
        }
    }
}
