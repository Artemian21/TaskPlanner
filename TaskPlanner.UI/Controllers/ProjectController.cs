using Microsoft.AspNetCore.Mvc;
using TaskPlanner.DataAccess;
using TaskPlanner.DataAccess.Repositories;
using TaskPlanner.Domain.Abstraction;
using TaskPlanner.Domain.Models;

namespace TaskPlanner.UI.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            this._projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetAllProjects();
            return View(projects);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await _projectService.GetProjectById(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await _projectService.GetProjectById(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = await _projectService.GetProjectById(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            if (!ModelState.IsValid)
            {
                return View(project);
            }

            var (newProject, errors) = Project.Create(project.Id, project.Name, project.Description, project.Deadline, project.Tasks);

            if (errors.Any())
            {
                ModelState.AddModelError(string.Empty, string.Join(", ", errors));
                return View(project);
            }

            await _projectService.AddProject(newProject);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isDeleted = await _projectService.DeleteProject(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Project project)
        {

            if (id != project.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                await _projectService.UpdateProject(id, project.Name, project.Description, project.Deadline);
                return RedirectToAction(nameof(Details), new { id = project.Id });
            }
            return View(project);
        }
    }
}
