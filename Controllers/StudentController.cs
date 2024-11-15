
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using School.Models;
using School.Models.Repositories;

namespace School.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class StudentController : Controller
    {
        private readonly IStudentRepository StudentRepository;
        private readonly IUniversityRepository UniversityRepository;
        private readonly IWebHostEnvironment HostingEnvironment;

        public StudentController(IStudentRepository StudentRepository, IUniversityRepository UniversityRepository, IWebHostEnvironment hostingEnvironment)
        {
            this.StudentRepository = StudentRepository;
            this.UniversityRepository = UniversityRepository;
            HostingEnvironment = hostingEnvironment;
        }
        [AllowAnonymous]
        // GET: Student
        public ActionResult Index(int? universityID)
        {
           
            ViewBag.UniversityID = new SelectList(UniversityRepository.GetAll(), "UniversityID", "UniversityName");

            var studentsQuery = StudentRepository.GetAll().AsQueryable();

            if (universityID.HasValue)
            {
                studentsQuery = studentsQuery.Where(s => s.UniversityID == universityID);
            }
            var viewModel = new StudentListViewModel
            {
                Students = studentsQuery.ToList(),
                UniversityID = universityID
            };

            return View(viewModel);
        }


        // GET: Student/Details/{id}
        public ActionResult Details(int id)
        {
            var Student = StudentRepository.GetById(id);
            if (Student == null)
            {
                return NotFound();
            }
            return View(Student);
        }


        public ActionResult Create()
        {
            ViewBag.UniversityID = new SelectList(UniversityRepository.GetAll(), "UniversityID", "UniversityName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student Student)
        {
            ViewBag.UniversityID = new SelectList(UniversityRepository.GetAll(), "UniversityID", "UniversityName");
            try
            {
                StudentRepository.Add(Student);
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }


        public ActionResult Search(string name, int? UniversityID)
        {
            var studentsQuery = StudentRepository.GetAll().AsQueryable();
            if (!string.IsNullOrEmpty(name))
            {
                studentsQuery = studentsQuery.Where(s => s.StudentName.Contains(name)); // Filter by student name
            }
            if (UniversityID.HasValue)
            {
                studentsQuery = studentsQuery.Where(s => s.UniversityID == UniversityID);
            }
            var viewModel = new StudentListViewModel
            {
                Students = studentsQuery.ToList(),
                UniversityID = UniversityID
            };
            ViewBag.UniversityID = new SelectList(UniversityRepository.GetAll(), "UniversityID", "UniversityName", UniversityID);
            return View("Index", viewModel);
        }



        public ActionResult Edit(int id)
        {
            ViewBag.UniversityID = new SelectList(UniversityRepository.GetAll(), "UniversityID", "UniversityName");
            var Student = StudentRepository.GetById(id);
            if (Student == null)
            {
                return NotFound();
            }
            return View(Student);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student Student)
        {
            ViewBag.UniversityID = new SelectList(UniversityRepository.GetAll(), "UniversityID", "UniversityName");
            try
            {
                StudentRepository.Edit(Student);
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }


        public ActionResult Delete(int id)
        {
            var Student = StudentRepository.GetById(id);
            if (Student == null)
            {
                return NotFound();
            }
            return View(Student);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var Student = StudentRepository.GetById(id);
            StudentRepository.Delete(Student);
            return RedirectToAction(nameof(Index));
        }

    }
}
