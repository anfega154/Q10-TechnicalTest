using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Q10_TechnicalTest.Domain.Entities;
using Q10_TechnicalTest.Domain.Interfaces;
using Q10_TechnicalTest.Utils;

namespace Q10_TechnicalTest.Web.Pages.Subjects
{
    public class IndexModel : PageModel
    {
        private readonly ISubjectService _subjectService;

        public IndexModel(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        public PaginatedList<Subject> Subjects { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 5;

        public async Task OnGetAsync()
        {
            var query = _subjectService.GetAllQueryable();

            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(s => s.Name.Contains(SearchString)
                                    || s.Code.Contains(SearchString));
            }

            Subjects = await PaginatedList<Subject>.CreateAsync(query, PageNumber, PageSize, SearchString);
        }
    }
}