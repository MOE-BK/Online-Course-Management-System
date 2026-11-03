using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Online_Course_Management_System.Data;
using Online_Course_Management_System.Models;
using Online_Course_Management_System.ViewModel;
using Online_Course_Management_System.ViewModel;

namespace Online_Course_Management_System.Controllers
{
    
    public class CoursesController(AppDbContext context) : Controller
    {
        private readonly AppDbContext _context = context;

        // Create - GET
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await LoadCourseLists();

            return View();
        }

        // Create - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateViewModel model)
        {

            if (!ModelState.IsValid)
            {
                await LoadCourseLists();

                return View(model);
            }

            string? imagePath = null;
            // Handle image upload
            if (model.Image != null)
            {
                var uploadsFolder= Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images","courses");
                Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists
                // Generate a unique file name to avoid overwriting existing files
                var fileName =Guid.NewGuid().ToString()+Path.GetExtension(model.Image.FileName);
                // Combine the folder path and file name
                var filePath = Path.Combine(uploadsFolder, fileName);
                // Stream the uploaded file to the specified path
                using var stream= new FileStream(filePath, FileMode.Create);
                // Save the relative path to the database
                await model.Image.CopyToAsync(stream);
                // Store the relative path to the image in the database
                imagePath= "/uploads/courses/" + fileName;



            }


            var course = new Course
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Duration = model.Duration,
                CreatedAt = DateTime.Now,
                CategoryId = model.CategoryId,
                InstructorId = model.InstructorId,
                ImagePath = imagePath
            };

            _context.Courses.Add(course);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Course created successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Load Categories and Instructors
        private async Task LoadCourseLists()
        {
            ViewBag.Categories = new SelectList(
                await _context.Categories
                    .AsNoTracking()
                    .ToListAsync(),
                "Id",
                "Name");

            ViewBag.Instructors = new SelectList(
                await _context.Instructors
                    .AsNoTracking()
                    .ToListAsync(),
                "Id",
                "Name");
        }

        // Details
        // Details
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
                return NotFound();

            return View(course);
        }

        // Edit - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _context.Courses
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            var model = new CourseEditViewModel
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description,
                Price = course.Price,
                Duration = course.Duration,
                CategoryId = course.CategoryId,
                InstructorId = course.InstructorId
            };

            await LoadCourseLists();

            return View(model);
        }

        // Edit - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CourseEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadCourseLists();

                return View(model);
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == model.Id);

            if (course == null)
            {
                return NotFound();
            }

            course.Name = model.Name;
            course.Description = model.Description;
            course.Price = model.Price;
            course.Duration = model.Duration;
            course.CategoryId = model.CategoryId;
            course.InstructorId = model.InstructorId;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Course updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        // Delete - GET
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // Delete - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Course deleted successfully.";

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Index(

            string? search,
             int? categoryId,
             string? sort,
             int page = 1,
             int pageSize = 5


            )
        {
            // IQueryable<Course> query = _context.Courses
            //filitering

            var query = _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Instructor)
                .AsNoTracking()
                .AsQueryable();

            // Search 
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c => c.Name.Contains(search) || c.Description.Contains(search));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(c => c.CategoryId == categoryId.Value);
            }
            // sorting 
            query = sort switch
            {
                "price_asc" => query.OrderBy(c => c.Price),
                "price_desc" => query.OrderByDescending(c => c.Price),
                "name_desc" => query.OrderByDescending(c => c.Name),
                _ => query.OrderBy(c => c.Name)
            };
            // pagination

            var totalCourses = await query.CountAsync();
            totalCourses = 12;
            // Calculate total pages
            var totalPages = (int)Math.Ceiling(totalCourses / (double)pageSize);

            if (page < 1)
            {
                page = 1;

            }
            else if (page > totalPages)
            {
                page = totalPages;

            }
            // Fetch the courses for the current page
            var courses = await query
                .Skip((page - 1) * pageSize)//page = 2 | pageSize = 5 => skip 5
                .Take(pageSize)//page = 2 | pageSize = 5 => take 5
                .ToListAsync();
            // Prepare the view model
            ViewBag.Categories = new SelectList
                (
                    await _context.Categories
                    .AsNoTracking()
                     .ToListAsync(),
                    "Id",
                    "Name",
                    categoryId


                );
            // Pass the current search, categoryId, sort, page, and pageSize to the view for maintaining state
            ViewBag.Search = search;
            ViewBag.CategoryId = categoryId;
            ViewBag.Sort = sort;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return View(courses);


        }


    }


    }