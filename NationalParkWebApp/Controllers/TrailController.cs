using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NationalParkWebApp.Models;
using NationalParkWebApp.Models.ViewModels;
using NationalParkWebApp.Repository;
using NationalParkWebApp.Repository.IRepository;

namespace NationalParkWebApp.Controllers
{
    public class TrailController : Controller
    {
        private readonly ITrailRepository _TrailRepository;
        private readonly INationalParkRepository _NationalParkRepository;
        public TrailController(ITrailRepository TrailRepository, INationalParkRepository NationalParkRepository)
        {
            _TrailRepository = TrailRepository;
            _NationalParkRepository = NationalParkRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _TrailRepository.GetAllAsync(SD.TrailAPIPath) });
        }
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _TrailRepository.DeleteAsync(SD.TrailAPIPath, id);
            if (status)
                return Json(new { success = true, message = "data successfully delete!!!" });
            return Json(new { success = false, message = "Somthing went Worng while deleted data!!!" });
        }
        #endregion
        public async Task<IActionResult> Upsert(int? id)
        {
            var nationalParks = await _NationalParkRepository.GetAllAsync(SD.NationalParkAPIPath);
            TrailVM trailVM = new TrailVM()
            {
                Trail = new Trail(),
                NationalParkList = nationalParks.Select(np => new SelectListItem()
                {
                    Text = np.Name,
                    Value = np.Id.ToString()
                })
            };
            if (id == null) return View(trailVM);
            trailVM.Trail = await _TrailRepository.GetAsync(SD.TrailAPIPath, id.GetValueOrDefault());
            if (trailVM == null) return NotFound();
            return View(trailVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailVM trailVM)
        {
            if(ModelState.IsValid)
            {
                if (trailVM.Trail.Id == 0)
                    await _TrailRepository.CreateAsync(SD.TrailAPIPath, trailVM.Trail);
                else
                    await _TrailRepository.UpdateAsync(SD.TrailAPIPath, trailVM.Trail);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var nationalParks = await _NationalParkRepository.GetAllAsync(SD.NationalParkAPIPath);
                trailVM = new TrailVM()
                {
                    Trail = new Trail(),
                    NationalParkList=nationalParks.Select(np=>new SelectListItem()
                    {
                        Text=np.Name,
                        Value=np.Id.ToString()
                    })
                };
                return View(trailVM);
            }
        }
        
        
    }
}
