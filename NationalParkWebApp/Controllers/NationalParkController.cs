using Microsoft.AspNetCore.Mvc;
using NationalParkWebApp.Models;
using NationalParkWebApp.Repository.IRepository;

namespace NationalParkWebApp.Controllers
{
    public class NationalParkController : Controller
    {
        private readonly INationalParkRepository _NationalParkRepository;
        public NationalParkController(INationalParkRepository nationalParkRepository)
        {
            _NationalParkRepository = nationalParkRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region APIs
        public async Task<IActionResult> GetAll()
        {
            return Json(new { data = await _NationalParkRepository.GetAllAsync(SD.NationalParkAPIPath) });
        }
        [HttpDelete]
        public async Task<IActionResult>Delete(int id)
        {
            var status=await _NationalParkRepository.DeleteAsync(SD.NationalParkAPIPath, id);
            if(status)
                return Json(new {success= true,message="data successfully delete!!!"});
            return Json(new { success = false, message = "Somthing went Worng while deleted data!!!" });
        }
        #endregion
        public async Task<IActionResult>Upsert(int? id)
        {
            NationalPark nationalPark = new NationalPark();
            if (id == null) return View(nationalPark);
            nationalPark = await _NationalParkRepository.GetAsync(SD.NationalParkAPIPath, id.GetValueOrDefault());
            if (nationalPark == null) return NotFound();
            return View(nationalPark);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark nationalPark)
        {
            if(ModelState.IsValid)
            {
                var files=HttpContext.Request.Form.Files;
                if(files.Count()>0)
                {
                    byte[] p1 = null;//Array
                    using(var fs1 = files[0].OpenReadStream())
                    {
                        using(var ms1=new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1= ms1.ToArray();
                        }
                    } 
                    nationalPark.Picture = p1;    //Save To Picture               
                }
                else
                {
                    var nationalParkInDb=await _NationalParkRepository.GetAsync(SD.NationalParkAPIPath,nationalPark.Id);
                    nationalPark.Picture=nationalParkInDb.Picture;
                }
                if (nationalPark.Id == 0)
                    await _NationalParkRepository.CreateAsync(SD.NationalParkAPIPath, nationalPark);
                else
                    await _NationalParkRepository.UpdateAsync(SD.NationalParkAPIPath, nationalPark);
                return RedirectToAction("Index");
                
            }
            else
            {
                return View(nationalPark);
            }
        }
    }
}
