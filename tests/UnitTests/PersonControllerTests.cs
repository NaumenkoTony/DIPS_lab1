namespace webapi.Tests;

using Xunit;
using Moq;
using DIPS_lab01.Controllers;
using DIPS_lab01.Models;
using DIPS_lab01.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using DIPS_lab01.Data.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;

public class PersonControllerTests
{
    private readonly Mock<IRepository<Person>> mockRepository;
    private readonly PersonsController controller;

    public PersonControllerTests()
    {
        mockRepository = new Mock<IRepository<Person>>();
        controller = new PersonsController(mockRepository.Object);
    }

    [Fact]
    public void GetAll()
    {
        //Arrange
        IEnumerable<Person> persons = new List<Person>
        {
            new() { Id = 1, Name = "Tony", Age = 20, Address = "Moscow", Work = "Developer" },
            new() { Id = 2, Name = "Alex", Age = 30, Address = "Moscow", Work = "Doctor" }
        };
        mockRepository.Setup(repository => repository.Read()).Returns(persons);
        
        var expected = persons.ToList();

        //Act
        var result = controller.GetAll();

        // Assert
        var httpResponse = Assert.IsType<OkObjectResult>(result.Result);
        var actual = Assert.IsType<List<Person>>(httpResponse.Value);
        Assert.Equal(expected.Count, actual.Count);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetByIdOk()
    {
        //Arrange
        Person person = new() { Id = 1, Name = "Tony", Age = 20, Address = "Moscow", Work = "Developer" };
    
        mockRepository.Setup(repository => repository.Read(1)).Returns(person);
        
        var expected = person;

        //Act
        var result = controller.GetById(1);

        // Assert
        var httpResponse = Assert.IsType<OkObjectResult>(result.Result);
        var actual = Assert.IsType<Person>(httpResponse.Value);
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void GetByIdNotFound()
    {
        //Arrange
        Person person = new() { Id = 1, Name = "Tony", Age = 20, Address = "Moscow", Work = "Developer" };
    
        mockRepository.Setup(repository => repository.Read(1)).Returns(person);

        //Act
        var result = controller.GetById(2);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public void CreateOk()
    {
        // Arrange
        Person person = new() { Name = "Tony", Age = 20, Address = "Moscow", Work = "Developer" };

        Person expected = new() { Id = 1, Name = "Tony", Age = 20, Address = "Moscow", Work = "Developer" };

        mockRepository.Setup(repository => repository.Create(It.IsAny<Person>())).Callback<Person>(p => expected.Id = p.Id);

        // Act
        var result = controller.Create(person);

        // Assert
        var httpResponse = Assert.IsType<CreatedAtActionResult>(result);

        var expectedRouteName = nameof(PersonsController.GetById);
        Assert.Equal(expectedRouteName, httpResponse.ActionName);
    }

    [Fact]
    public void CreateBadRequest()
    {
        // Arrange
        Person person = new() { Age = 20, Address = "Moscow", Work = "Developer" };

        // Act
        var result = controller.Create(person);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void DeleteOk()
    {
        // Arrange
        var personId = 1;
        mockRepository.Setup(repository => repository.Read(personId)).Returns(new Person { Id = personId });

        // Act
        var result = controller.Delete(personId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        mockRepository.Verify(repository => repository.Delete(personId), Times.Once);
    }

    [Fact]
    public void DeleteNotFound()
    {
        // Arrange
        var personId = 1;
        mockRepository.Setup(repository => repository.Read(personId)).Returns(new Person { Id = personId });

        // Act
        var result = controller.Delete(2);

        // Assert
        var httpResponse = Assert.IsType<NotFoundObjectResult>(result);
        mockRepository.Verify(repository => repository.Delete(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void UpdateOk()
    {
        // Arrange
        var personId = 1;
        Person oldPerson = new() { Id = personId, Name = "Alex" };
        Person newPerson = new() { Id = personId, Name = "Tony", Age = 20, Address = "Moscow", Work = "Developer" };

        mockRepository.Setup(repository => repository.Read(personId)).Returns(oldPerson);
        mockRepository.Setup(repository => repository.Update(It.IsAny<Person>()));

        // Act
        var result = controller.Update(personId, newPerson);

        // Assert
        var httpResponse = Assert.IsType<OkObjectResult>(result);
        var updatedPerson = Assert.IsType<Person>(httpResponse.Value);
        Assert.Equal(newPerson.Id, updatedPerson.Id);
        Assert.Equal(newPerson.Name, updatedPerson.Name);
        Assert.Equal(newPerson.Age, updatedPerson.Age);
        Assert.Equal(newPerson.Address, updatedPerson.Address);
        Assert.Equal(newPerson.Work, updatedPerson.Work);
        mockRepository.Verify(repository => repository.Update(It.IsAny<Person>()), Times.Once);
    }

    [Fact]
    public void UpdateNotFound()
    {
        // Arrange
        var personId = 1;
        Person oldPerson = new() { Id = personId, Name = "Alex" };
        Person newPerson = new() { Id = personId, Name = "Tony", Age = 20, Address = "Moscow", Work = "Developer" };
        mockRepository.Setup(repository => repository.Read(personId)).Returns(oldPerson);

        // Act
        var result = controller.Update(2, newPerson);

        // Assert
        var httpResponse = Assert.IsType<NotFoundObjectResult>(result);
        mockRepository.Verify(repository => repository.Update(It.IsAny<Person>()), Times.Never);
    }

    [Fact]
    public void UpdateBadRequest()
    {
        // Arrange
        var personId = 1;
        Person oldPerson = new() { Id = personId, Name = "Alex" };
        Person newPerson = new() { Id = personId, Age = 20, Address = "Moscow", Work = "Developer" };

        mockRepository.Setup(repository => repository.Read(personId)).Returns(oldPerson);

        // Act
        var result = controller.Update(personId, newPerson);

        // Assert
        var httpResponse = Assert.IsType<BadRequestObjectResult>(result);
        mockRepository.Verify(repository => repository.Update(It.IsAny<Person>()), Times.Never);
    }
}