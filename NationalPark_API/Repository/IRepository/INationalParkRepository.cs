using NationalPark_API.Models;

namespace NationalPark_API.Repository.IRepository
{
    public interface INationalParkRepository
    {
        ICollection<NationalPark> GetNationalParks(); //display
        NationalPark GetNationalPark(int nationalParkId); //find
        bool NationalParkExists(int nationalParkId);
        bool NationalParkExists(string nationalParkName);
        bool CreateNationalPark(NationalPark nationalPark); //create
        bool UpdateNationalPark(NationalPark nationalPark); //update
        bool DeleteNationalPark(NationalPark nationalPark); //delete
        bool Save();
    }
}
