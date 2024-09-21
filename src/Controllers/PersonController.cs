namespace DIPS_lab01.Controllers;

using DIPS_lab01.Models;
using DIPS_lab01.Data;
using Microsoft.AspNetCore.Mvc;

public class PersonsController(IRepository<Person> personRepository) : ControllerBase
{
    private readonly IRepository<Person> personRepository = personRepository;

    [Route("api/v1/[controller]")]
    [EndpointSummary("Get all Persons")]
    [HttpGet]
    public ActionResult<IEnumerable<Person>> GetAll()
    {
        var persons = personRepository.Read();
        return Ok(persons);
    }

    [Route("api/v1/[controller]/{id}")]
    [EndpointSummary("Get Person by ID")]
    [HttpGet]
    public ActionResult<Person> GetById(int id)
    {
        var person = personRepository.Read(id);
        if (person == null)
        {
            return NotFound(new { message = "Person not found" });
        }
        return Ok(person);
    }

    [Route("api/v1/[controller]")]
    [EndpointSummary("Create new Person")]
    [HttpPost]
    public ActionResult Create([FromBody] Person personRequest)
    {
        if (string.IsNullOrEmpty(personRequest.Name))
        {
            return BadRequest(new { message = "Invalid data", errors = new { name = "Name is required" } });
        }

        var person = new Person
        {
            Name = personRequest.Name,
            Age = personRequest.Age,
            Address = personRequest.Address,
            Work = personRequest.Work
        };

        personRepository.Create(person);

        return CreatedAtAction(nameof(GetById), new { id = person.Id }, null);
    }

    [Route("api/v1/[controller]/{id}")]
    [EndpointSummary("Remove Person by ID")]
    [HttpDelete]
    public ActionResult Delete(int id)
    {
        var person = personRepository.Read(id);
        if (person == null)
        {
            return NotFound(new { message = "Person not found" });
        }

        personRepository.Delete(id);
        return NoContent();
    }

    [Route("api/v1/[controller]/{id}")]
    [EndpointSummary("Patch Person by ID")]
    [HttpPatch]
    public ActionResult Patch(int id, [FromBody] Person personRequest)
    {
        if (personRequest == null)
        {
            return BadRequest(new { message = "Request body cannot be null." });
        }

        var person = personRepository.Read(id);
        if (person == null)
        {
            return NotFound(new { message = "Person not found" });
        }

        var properties = typeof(Person).GetProperties();

        foreach (var property in properties)
        {
            if (property.Name == nameof(Person.Id))
            {
                continue;
            }

            var newValue = property.GetValue(personRequest);
            if (newValue != null && !(newValue is string && string.IsNullOrEmpty((string)newValue)))
            {
                property.SetValue(person, newValue);
            }
        }

        personRepository.Update(person);

        return Ok(person);
    }
}
