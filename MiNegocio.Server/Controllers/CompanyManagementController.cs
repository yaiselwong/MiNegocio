using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiNegocio.Server.Interfaces;
using MiNegocio.Shared.Dto.Request;
using MiNegocio.Shared.Models;

namespace MiNegocio.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CompanyManagementController : ControllerBase
    {
        private readonly ICompanyManagementService _companyService;

        public CompanyManagementController(ICompanyManagementService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Company>>> GetAllCompanies()
        {
            // Only admins can view all companies
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var companies = await _companyService.GetAllCompaniesAsync();
            return Ok(companies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            // Admins can view any company
            // System users can only view their own company
            if (!User.IsInRole("Admin"))
            {
                var userCompanyId = User.FindFirst("CompanyId")?.Value;
                if (string.IsNullOrEmpty(userCompanyId) || int.Parse(userCompanyId) != id)
                {
                    return Forbid();
                }
            }

            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }

        [HttpPost]
        public async Task<ActionResult<Company>> CreateCompany([FromBody] CreateCompanyRequest request)
        {
            // Only admins can create companies
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var company = await _companyService.CreateCompanyAsync(request);
            return CreatedAtAction(nameof(GetCompany), new { id = company.Id }, company);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Company>> UpdateCompany(int id, [FromBody] CreateCompanyRequest request)
        {
            // Only admins can update companies
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var company = await _companyService.UpdateCompanyAsync(id, request);
            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            // Only admins can delete companies
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var success = await _companyService.DeleteCompanyAsync(id);
            if (!success)
            {
                return BadRequest("Cannot delete company with associated users.");
            }

            return NoContent();
        }
    }
}
