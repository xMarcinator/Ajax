using backend.DB;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : Controller
{
    private readonly PersonContext _context;

    public PersonController(PersonContext personContext)
    {
        _context = personContext;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Person person)
    {
        await _context.Persons.AddAsync(person);
        await _context.SaveChangesAsync();

        return Ok(person.PersonId); // Assuming you want to return an HTTP 200 OK response
    }
    
    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> Get(int id)
    {
        var person = await _context.Persons.FirstOrDefaultAsync(p => p.PersonId == id);
        if (person == null)
            return NotFound();

        return Ok(person);
    }
    
    [HttpGet]
    [Route("all")]
    public async Task<IActionResult> GetAll()
    {
        var persons = await _context.Persons.ToListAsync();
        return Ok(persons);
    }
    
    [HttpDelete]
    [Route("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var personToDelete = await _context.Persons.FirstOrDefaultAsync(p => p.PersonId == id);
        if (personToDelete == null)
            return NotFound();

        _context.Persons.Remove(personToDelete);
        await _context.SaveChangesAsync();

        return Ok();
    }
    
    [HttpPatch]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PersonModel personModel)
    {
        var person = await _context.Persons.FirstOrDefaultAsync(p => p.PersonId == id);
        if (person == null)
            return NotFound();

        person.Name = personModel.Name ?? person.Name;
        person.Phone = personModel.Phone ?? person.Phone;

        await _context.SaveChangesAsync();

        return Ok();
    }
}

public class PersonModel
{
    public string? Name { get; set; }
    public string? Phone { get; set; }
}