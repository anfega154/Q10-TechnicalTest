using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Q10_TechnicalTest.Application.DTO;
using Q10_TechnicalTest.Domain.Entities;
using Q10_TechnicalTest.Domain.Interfaces;

namespace Q10_TechnicalTest.Web.Pages.Students;
    public class DetailModel : PageModel
{
    private readonly IStudentService _studentService;
    private readonly IStudentSubjectService _studentSubjectService;

    public DetailModel(
        IStudentService studentService,
        IStudentSubjectService studentSubjectService)
    {
        _studentService = studentService;
        _studentSubjectService = studentSubjectService;
    }

    public Student Student { get; set; }
    public IEnumerable<SubjectDto> EnrolledSubjects { get; set; }
    public IEnumerable<SubjectDto> AvailableSubjects { get; set; }

    public async Task OnGetAsync(int id)
    {
        Student = await _studentService.GetById(id);
        EnrolledSubjects = await _studentSubjectService.GetEnrolledSubjects(id);
        AvailableSubjects = await _studentSubjectService.GetAvailableSubjects(id);
    }

    public async Task<IActionResult> OnPostEnroll(int studentId, int subjectId)
    {
        try
        {
            await _studentSubjectService.EnrollStudent(studentId, subjectId);
            TempData["SuccessMessage"] = "Materia asignada correctamente";
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToPage(new { id = studentId });
    }

    public async Task<IActionResult> OnPostUnenroll(int studentId, int subjectId)
    {
        try
        {
            await _studentSubjectService.UnenrollStudent(studentId, subjectId);
            TempData["SuccessMessage"] = "Materia desasignada correctamente";
        }
        catch (DomainException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        return RedirectToPage(new { id = studentId });
    }
}