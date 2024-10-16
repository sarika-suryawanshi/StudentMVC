using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models;
using StudentPortal.Web.Models.Entities;

namespace StudentPortal.Web.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDBContext _dbContext;
        public StudentController(ApplicationDBContext dbcontext)
        {
            _dbContext = dbcontext;  
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddStudentViewModel model)
        {
            var duplicate = _dbContext.Students.Where(x=>x.Phone ==model.Phone).FirstOrDefaultAsync();
            if(duplicate != null)
            {
                ViewBag.Message = "A student with this phone number already exists.";
                return View(new AddStudentViewModel());
            }
            var student = new Student
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Subscribed = model.Subscribed,
            };
            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();
            ViewBag.Message = "Student added successfully.";
            return View(new AddStudentViewModel());
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
          var stdlist= await _dbContext.Students.ToListAsync();
            return View(stdlist);
        }
    }
}
