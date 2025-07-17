using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Q10_TechnicalTest.Domain.Interfaces;

namespace Q10_TechnicalTest.Web.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly IStudentService _studentService;

        public EditModel(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [BindProperty]
        public StudentDto StudentDto { get; set; }  

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var student = await _studentService.GetById(id);
            if (student == null)
            {
                return NotFound();
            }


            StudentDto = new StudentDto(
                Id: student.Id,
                Name: student.Name,
                Document: student.Document,
                Email: student.Email
            );

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _studentService.Update(StudentDto); 
                TempData["SuccessMessage"] = "Estudiante actualizado exitosamente!";
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