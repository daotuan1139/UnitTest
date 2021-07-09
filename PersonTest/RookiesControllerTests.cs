using PersonUT.Controllers;
using PersonUT.Services;
using PersonUT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonTest
{
    public class RookiesControllerTests
    {
        private static List<Person> _members = new List<Person>
        {
            new Person{
                Email = "daotuan1139@gmail.com",
                FirstName = "Dao",
                LastName = "Tuan",
                Gender = true,
                DateOfBirth = new DateTime(1999,1,1),
                Phone= 123456789,
                BirthPlace = "Ha Noi",
                Age = 22,
                IsGradated = true,
            },
            new Person{
                Email = "datu@gmail.com",
                FirstName = "Da",
                LastName = "Tu",
                Gender = false,
                DateOfBirth = new DateTime(1999,1,1),
                Phone= 123456789,
                BirthPlace = "Ha Noi",
                Age = 22,
                IsGradated = true,
            }
        };
        private Mock<ILogger<RookiesController>> _loggerMock;
        private Mock<IPersonService> _serviceMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<RookiesController>>();

            _serviceMock = new Mock<IPersonService>();
            _serviceMock.Setup(s => s.GetList()).Returns(_members);
        }

        [Test]
        public void Index_ReturnsAViewResult_WithAListOfPerson()
        {
            // Arrange
            var controller = new RookiesController(_loggerMock.Object, _serviceMock.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            Assert.IsAssignableFrom<List<Person>>(((ViewResult)result).ViewData.Model);
            Assert.AreEqual(2, ((List<Person>)((ViewResult)result).ViewData.Model).Count);
        }

        [Test]
        public void Detail_ReturnsHttpNotFound_ForInvalidId()
        {
            // Arrange
            const string personEmail = "dat";
            _serviceMock.Setup(s => s.Detail(personEmail)).Returns((Person)null);
            var controller = new RookiesController(_loggerMock.Object, _serviceMock.Object);

            // Act
            var result = controller.Detail(personEmail);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Detail_ReturnsAPerson()
        {
            // Arrange
            const string personEmail = "daotuan1139@gmail.com";
            _serviceMock.Setup(service => service.Detail(personEmail)).Returns(_members.First());
            var controller = new RookiesController(_loggerMock.Object, _serviceMock.Object);

            // Act
            var result = controller.Detail(personEmail);

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var view = (ViewResult)result;
            Assert.IsAssignableFrom<Person>(view.ViewData.Model);
            var person = (Person)view.ViewData.Model;
            Assert.AreEqual(personEmail, person.Email);
        }

        [Test]
        public void Create_ReturnsAViewResult_WithoutParameter()
        {
            // Arrange
            var controller = new RookiesController(_loggerMock.Object, _serviceMock.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOf<ViewResult>(result);
            var view = (ViewResult)result;
            Assert.IsAssignableFrom<List<Person>>(view.ViewData.Model);
            var addPerson = (List<Person>)view.ViewData.Model;
            Assert.AreEqual(2, addPerson.Count);
        }

        [Test]
        public void Create_ReturnsRedirectToIndex_ANewPerson()
        {
            // Arrange
            const string AddEmail = "dao@gmail.com";
            const string AddFirstName = "Dao";
            const string AddLastName = "Tuan";
            const bool AddGender = true;
            DateTime AddDateOfBirth = new DateTime(1999, 10, 10);
            const int AddPhone = 123456789;
            const string AddBirthPlace = "Ha Noi";
            const int AddAge = 22;
            const bool AddIsGradated = true;
            var controller = new RookiesController(_loggerMock.Object, _serviceMock.Object);
            var newPerson = new Person()
            {
                Email = AddEmail,
                FirstName = AddFirstName,
                LastName = AddLastName,
                Gender = AddGender,
                DateOfBirth = AddDateOfBirth,
                Phone = AddPhone,
                BirthPlace = AddBirthPlace,
                Age = AddAge,
                IsGradated = AddIsGradated,
            };

            // Act
            var result = (RedirectToActionResult) controller.Create(newPerson);

            // Assert

            Assert.IsTrue(newPerson.Email != null);

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.Null(result.ControllerName);
            Assert.AreEqual("Index", result.ActionName);
            _serviceMock.Verify();

        }

        [Test]
        public void Create_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            const string message = "some error";
            var controller = new RookiesController(_loggerMock.Object, _serviceMock.Object);
            controller.ModelState.AddModelError("error", message);

            // Act
            var result = controller.Create(member: null);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.IsAssignableFrom<SerializableError>(((BadRequestObjectResult)result).Value);

            var error = (SerializableError)((BadRequestObjectResult)result).Value;
            Assert.AreEqual(1, error.Count);

            error.TryGetValue("error", out var msg);
            Assert.IsNotNull(msg);
            Assert.AreEqual(message, ((string[])msg).First());
        }

        [Test]
        public void Update_ReturnsBadRequest_GivenInvalidModel()
        {
            // Arrange
            const string personEmail = "dat";
            _serviceMock.Setup(s => s.Detail(personEmail)).Returns((Person)null);
            var controller = new RookiesController(_loggerMock.Object, _serviceMock.Object);

            // Act
            var result = controller.Detail(personEmail);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Update_ReturnsRedirectToIndex_EditAPerson()
        {
            //Arrange
            const string FindEmail = "daotuan1139@gmail.com";
            const string UpdateFirstName = "a";
            const string UpdateLastName = "a";
            const bool UpdateGender = false;
            DateTime UpdateDateOfBirth = new DateTime(1999, 10, 10);
            const int UpdatePhone = 123456789;
            const string UpdateBirthPlace = "Ha Noi";
            const int UpdatedAge = 22;
            const bool UpdateIsGradated = false;
            _serviceMock.Setup(s => s.Detail(FindEmail)).Returns(_members.First());
            var controller = new RookiesController(_loggerMock.Object, _serviceMock.Object);
            var updatePerson = new Person()
            {
                Email = FindEmail,
                FirstName = UpdateFirstName,
                LastName = UpdateLastName,
                Gender = UpdateGender,
                DateOfBirth = UpdateDateOfBirth,
                Phone = UpdatePhone,
                BirthPlace = UpdateBirthPlace,
                Age = UpdatedAge,
                IsGradated = UpdateIsGradated,
            };

            //Act
            var update = (RedirectToActionResult) controller.Edit(updatePerson);

            //Assert
            Assert.IsTrue(updatePerson.Email == FindEmail);

            Assert.IsInstanceOf<RedirectToActionResult>(update);
            Assert.Null(update.ControllerName);
            Assert.AreEqual("Index", update.ActionName);
            _serviceMock.Verify();
        }

        [Test]
        public void Delete_ReturnsBadRequest_ModelNull()
        {
            // Arrange
            const string personEmail = "dat";
            _serviceMock.Setup(s => s.Detail(personEmail)).Returns((Person)null);
            var controller = new RookiesController(_loggerMock.Object, _serviceMock.Object);

            // Act
            var result = controller.Detail(personEmail);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void Delete_ReturnsRedirectToIndex_DeleteAPerson()
        {
            // Arrange
            const string personEmail = "daotuan1139@gmail.com";
            _serviceMock.Setup(s => s.Detail(personEmail)).Returns(_members.First());
            var controller = new RookiesController(_loggerMock.Object, _serviceMock.Object);

            // Act
            var result = (RedirectToActionResult) controller.Delete(personEmail);

            // Assert
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            Assert.Null(result.ControllerName);
            Assert.AreEqual("Index", result.ActionName);
            _serviceMock.Verify();
        }
    }
}