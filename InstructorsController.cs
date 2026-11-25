
using InstructorInfoSystem.Data;
using InstructorInfoSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InstructorInfoSystem.Controllers
{
    [Authorize]
    public class InstructorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InstructorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // READ: Display all instructors.


        public async Task<IActionResult> Index(
            string? searchString,
            int page = 1,
            string? sortOrder = null)
        {
            const int pageSize = 10;

            var instructors = _context.Instructors
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                var search = searchString.Trim().ToLower();

                instructors = instructors.Where(instructor =>
                    instructor.FullName.ToLower().Contains(search) ||
                    instructor.Designation.ToLower().Contains(search) ||
                    instructor.Majorship.ToLower().Contains(search) ||
                    instructor.BachelorDegree.ToLower().Contains(search) ||
                    instructor.Address.ToLower().Contains(search) ||
                    instructor.ContactNumber.ToLower().Contains(search) ||
                    instructor.Email.ToLower().Contains(search));
            }

            var totalCount = await instructors.CountAsync();
            var totalPages = (int)Math.Ceiling(
                totalCount / (double)pageSize);

            if (totalPages > 0)
            {
                page = Math.Clamp(page, 1, totalPages);
            }
            else
            {
                page = 1;
            }

            instructors = sortOrder switch
            {
                "fullName" => instructors.OrderBy(i => i.FullName),
                "designation" => instructors.OrderBy(i => i.Designation),
                "email" => instructors.OrderBy(i => i.Email),
                "fullName_desc" => instructors.OrderByDescending(i => i.FullName),
                "designation_desc" => instructors.OrderByDescending(i => i.Designation),
                "email_desc" => instructors.OrderByDescending(i => i.Email),
                _ => instructors.OrderBy(i => i.FullName)
            };

            var results = await instructors
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["CurrentSort"] = sortOrder;

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;
            ViewData["TotalCount"] = totalCount;

            return View(results);
        }



        // CREATE: Display the form.
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Instructor());
        }

        // CREATE: Save a new instructor.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Instructor instructor)
        {
            if (!ModelState.IsValid)
            {
                return View(instructor);
            }

            bool emailExists = await _context.Instructors
                .AnyAsync(existing =>
                    existing.Email.ToLower() == instructor.Email.Trim().ToLower());

            if (emailExists)
            {
                ModelState.AddModelError(
                    nameof(instructor.Email),
                    "An instructor with this email already exists.");

                return View(instructor);
            }

            TrimFields(instructor);

            _context.Instructors.Add(instructor);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Instructor added successfully.";

            return RedirectToAction(nameof(Index));
        }



        // UPDATE: Display the edit form.
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FindAsync(id.Value);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // UPDATE: Save the edited instructor.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, Instructor instructor)
        {
            if (id != instructor.InstructorId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(instructor);
            }

            var email = instructor.Email.Trim();

            bool emailExists = await _context.Instructors
                .AnyAsync(existing =>
                    existing.Email.ToLower() == email.ToLower() &&
                    existing.InstructorId != id);

            if (emailExists)
            {
                ModelState.AddModelError(
                    nameof(instructor.Email),
                    "Another instructor already uses this email address.");

                return View(instructor);
            }

            var existingInstructor = await _context.Instructors
                .FindAsync(id);

            if (existingInstructor == null)
            {
                return NotFound();
            }

            TrimFields(instructor);

            existingInstructor.FullName = instructor.FullName;
            existingInstructor.Age = instructor.Age;
            existingInstructor.Address = instructor.Address;
            existingInstructor.Designation = instructor.Designation;
            existingInstructor.Gender = instructor.Gender;
            existingInstructor.ContactNumber = instructor.ContactNumber;
            existingInstructor.Majorship = instructor.Majorship;
            existingInstructor.BachelorDegree = instructor.BachelorDegree;
            existingInstructor.Email = email;

            await _context.SaveChangesAsync();

            TempData["Message"] = "Instructor updated successfully.";

            return RedirectToAction(nameof(Index));
        }



        // DELETE: Display the confirmation page.
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    instructor => instructor.InstructorId == id);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // DELETE: Remove the instructor from the database.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = await _context.Instructors
                .FindAsync(id);

            if (instructor == null)
            {
                return NotFound();
            }

            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Instructor deleted successfully.";

            return RedirectToAction(nameof(Index));
        }




        // READ: Display one instructor's details.
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    instructor => instructor.InstructorId == id);

            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }



        // Remove extra spaces before saving.
        private static void TrimFields(Instructor instructor)
        {
            instructor.FullName = instructor.FullName.Trim();
            instructor.Address = instructor.Address.Trim();
            instructor.Designation = instructor.Designation.Trim();
            instructor.Gender = instructor.Gender.Trim();
            instructor.ContactNumber = instructor.ContactNumber.Trim();
            instructor.Majorship = instructor.Majorship.Trim();
            instructor.BachelorDegree = instructor.BachelorDegree.Trim();
            instructor.Email = instructor.Email.Trim();
        }
    }
}
