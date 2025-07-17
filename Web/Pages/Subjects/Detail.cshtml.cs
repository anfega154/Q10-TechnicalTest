using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Q10_TechnicalTest.Domain.Entities;
using Q10_TechnicalTest.Domain.Interfaces;

namespace Q10_TechnicalTest.Web.Pages.Subjects
{
    public class DetailsModel : PageModel
    {
        private readonly ISubjectService _subjectService;

        public DetailsModel(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        public Subject Subject { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Subject = await _subjectService.GetById(id);
            if (Subject == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}