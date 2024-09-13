using AcmeCorp.Data.Models;
using AcmeCorp.Data.Models.Entities;
using AcmeCorp.Data.Repositories;
using AcmeCorp.Service.Services;
using Moq;

namespace AcmeCorp.Test
{

    public class EntryServiceTest
    {
        private readonly Mock<IEntryRepository> _entryRepositoryMock;
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly EntryService _entryService;

        public EntryServiceTest()
        {
            _entryRepositoryMock = new Mock<IEntryRepository>();
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _entryService = new EntryService(_entryRepositoryMock.Object, _customerRepositoryMock.Object);
        }

        [Fact]
        public async Task ValidateEntryAsync_ShouldReturnTrue_WhenModelIsValid()
        {
            var entry = new AddEntryViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@doe.com",
                Serial = "ACME-JID-IUZ-66X",
                DateOfBirth = DateTime.Now.AddYears(-20)
            };

            var result = await _entryService.ValidateModelAsync(entry);

            Assert.True(result);
        }

        [Fact]
        public async Task ValidateEntryAsync_ShouldReturnFalse_WhenFirstNameIsEmpty()
        {
            var entry = new AddEntryViewModel
            {
                FirstName = "",
                LastName = "Doe",
                Email = "john@doe.com",
                Serial = "ACME-JID-IUZ-66X",
                DateOfBirth = DateTime.Now.AddYears(-20)
            };

            var result = await _entryService.ValidateModelAsync(entry);

            Assert.False(result);
        }

        [Fact]
        public async Task ValidateEntryAsync_ShouldReturnFalse_WhenFirstNameIsTooLong()
        {
            var entry = new AddEntryViewModel
            {
                FirstName = "Thisnameiswaytoolongandshouldnotbeacceptedbythesystem",
                LastName = "Doe",
                Email = "john@doe.com",
                Serial = "ACME-JID-IUZ-66X",
                DateOfBirth = DateTime.Now.AddYears(-20)
            };

            var result = await _entryService.ValidateModelAsync(entry);
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateEntryAsync_ShouldReturnFalse_WhenFirstContainsNumbers()
        {
            var entry = new AddEntryViewModel
            {
                FirstName = "John123",
                LastName = "Doe",
                Email = "john123@doe.com",
                Serial = "ACME-JID-IUZ-66X",
                DateOfBirth = DateTime.Now.AddYears(-20)
            };

            var result = await _entryService.ValidateModelAsync(entry);
            Assert.False(result);
        }

        [Fact]
        public async Task ValidateEntryAsync_ShouldReturnFalse_WhenLastNameIsEmpty()
        {
            var entry = new AddEntryViewModel
            {
                FirstName = "John",
                LastName = "",
                Email = "john@doe.com",
                Serial = "ACME-JID-IUZ-66X",
                DateOfBirth = DateTime.Now.AddYears(-20)
            };

            var result = await _entryService.ValidateModelAsync(entry);

            Assert.False(result);
        }


        [Fact]
        public async Task ValidateEntryAsync_ShouldReturnFalse_WhenLastNameIsTooLong()
        {
            var entry = new AddEntryViewModel
            {
                FirstName = "John",
                LastName = "Thisnameiswaytoolongandshouldnotbeacceptedbythesystem",
                Email = "john@doe.com",
                Serial = "ACME-JID-IUZ-66X",
                DateOfBirth = DateTime.Now.AddYears(-20)
            };

            var result = await _entryService.ValidateModelAsync(entry);

            Assert.False(result);
        }

        [Fact]
        public async Task ValidateEntryAsync_ShouldReturnFalse_WhenLastContainsNumbers()
        {
            var entry = new AddEntryViewModel
            {
                FirstName = "John",
                LastName = "Doe123",
                Email = "john@doe.com",
                Serial = "ACME-JID-IUZ-66X",
                DateOfBirth = DateTime.Now.AddYears(-20)
            };

            var result = await _entryService.ValidateModelAsync(entry);

            Assert.False(result);
        }

        [Fact]
        public async Task ValidateEntryAsync_ShouldReturnFalse_WhenEmailIsEmpty()
        {
            var entry = new AddEntryViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "",
                Serial = "ACME-JID-IUZ-66X",
                DateOfBirth = DateTime.Now.AddYears(-20)
            };

            var result = await _entryService.ValidateModelAsync(entry);

            Assert.False(result);
        }

        [Fact]
        public async Task ValidateEntryAsync_ShouldReturnFalse_WhenEmailIsInvalid()
        {
            var entry = new AddEntryViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@doe",
                Serial = "ACME-JID-IUZ-66X",
                DateOfBirth = DateTime.Now.AddYears(-20)
            };

            var result = await _entryService.ValidateModelAsync(entry);

            Assert.False(result);
        }

        [Fact]
        public async Task ValidateEntryAsync_ShouldReturnFalse_WhenSerialIsInvalid()
        {
            var entry = new AddEntryViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@doe.com",
                Serial = "1234-987-464-987",
                DateOfBirth = DateTime.Now.AddYears(-20)
            };

            var result = await _entryService.ValidateModelAsync(entry);

            Assert.False(result);
        }

        [Fact]
        public async Task ValidateEntryAsync_ShouldReturnFalse_WhenDateOfBirthIsInvalid()
        {
            var entry = new AddEntryViewModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john@doe.com",
                Serial = "ACME-JID-IUZ-66X",
                DateOfBirth = DateTime.Now.AddYears(-17)
            };

            var result = await _entryService.ValidateModelAsync(entry);

            Assert.False(result);
        }
    }
}