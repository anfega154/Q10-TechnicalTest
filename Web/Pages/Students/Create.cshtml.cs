using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Q10_TechnicalTest.Domain.Interfaces;

namespace Q10_TechnicalTest.Web.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly IStudentService _studentService;

        public CreateModel(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [BindProperty]
        public StudentDto Student { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
          
                await _studentService.Create(Student);
                TempData["SuccessMessage"] = "Estudiante creado exitosamente!";
                return RedirectToPage("./Index");
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }
        }
    }
}