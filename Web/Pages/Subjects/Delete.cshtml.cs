using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Q10_TechnicalTest.Domain.Entities;
using Q10_TechnicalTest.Domain.Interfaces;

namespace Q10_TechnicalTest.Web.Pages.Subjects
{
    public class DeleteModel : PageModel
    {
        private readonly ISubjectService _subjectService;

        public DeleteModel(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [BindProperty]
        public Subject Subject { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Subject = await _subjectService.GetById(id);
            if (Subject == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {
                await _subjectService.Delete(id);
                TempData["SuccessMessage"] = "Materia eliminada exitosamente!";
                return RedirectToPage("./Index");
            }
            catch (DomainException ex)
            {
                ErrorMessage = ex.Message;
                return RedirectToPage(new { id });
            }
            catch (System.Exception ex)
            {
                ErrorMessage = "Ocurri� un error inesperado al intentar eliminar la materia.";
                return RedirectToPage(new { id });
            }
        }
    }
}