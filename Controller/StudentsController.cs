using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q10_TechnicalTest.Data;
using Q10_TechnicalTest.Models;
using Q10_TechnicalTest.Services.Interfaces;

namespace Q10_TechnicalTest.Controllers;

public class StudentsController : Controller
{
   private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    public async Task<IActionResult> Index()
    {
        var students = await _studentService.GetAllAsync();
        return View(students);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Student student)
    {
        if (!ModelState.IsValid) return View(student);

        await _studentService.AddAsync(student);
        return RedirectToAction(nameof(Index));
    }
}
