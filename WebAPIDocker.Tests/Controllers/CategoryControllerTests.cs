using Application.DTOs;
using Application.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIDocker.Controllers;

namespace WebAPIDocker.Tests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> _mockService;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _mockService = new Mock<ICategoryService>();
            _controller = new CategoryController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsListOfCategories()
        {
            // Arrange
            var categories = new List<CategoryDto>
            {
                new CategoryDto(1, "Ăn uống", "EXPENSE", 1, System.DateTime.Now),
                new CategoryDto(2, "Lương", "INCOME", 1, System.DateTime.Now)
            };
            _mockService
                .Setup(s => s.GetAll(It.IsAny<int>()))
                .ReturnsAsync(Result<IEnumerable<CategoryDto>>.Success(categories, "OK"));

            // Act
            var result = await _controller.GetAll(1);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.Equal(2, ((List<CategoryDto>)result.Data).Count);
        }

        [Fact]
        public async Task GetById_ReturnsOk_WhenCategoryExists()
        {
            // Arrange
            var dto = new CategoryDto(1, "Ăn uống", "EXPENSE", 1, System.DateTime.Now);
            _mockService.Setup(s => s.GetById(1, 1)).ReturnsAsync(dto);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var category = Assert.IsType<CategoryDto>(okResult.Value);
            Assert.Equal("Ăn uống", category.Name);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenCategoryNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.GetById(99, 1)).ReturnsAsync((CategoryDto)null);

            // Act
            var result = await _controller.GetById(99);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsFailure_WhenUserIdIsNull()
        {
            // Act
            var result = await _controller.Create(null, new CreateCategoryDto("Test", "EXPENSE"));

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal("Cần đăng nhập để tạo mới", result.Message);
        }

        [Fact]
        public async Task Create_ReturnsSuccess_WhenValid()
        {
            // Arrange
            _mockService.Setup(s => s.Create(It.IsAny<CreateCategoryDto>(), 1))
                .ReturnsAsync(Result<int>.Success(1, "Thêm dữ liệu thành công"));

            // Act
            var result = await _controller.Create(1, new CreateCategoryDto("Ăn uống", "EXPENSE"));

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Thêm dữ liệu thành công", result.Message);
        }

        [Fact]
        public async Task Update_ReturnsSuccess_WhenValid()
        {
            // Arrange
            var dto = new UpdateCategoryDto("Ăn uống", "EXPENSE");
            _mockService.Setup(s => s.Update(1, dto, 1))
                .ReturnsAsync(Result<int>.Success(1, "Cập nhật dữ liệu thành công"));

            // Act
            var result = await _controller.Update(1, dto, 1);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Cập nhật dữ liệu thành công", result.Message);
        }

        [Fact]
        public async Task Delete_ReturnsSuccess_WhenValid()
        {
            // Arrange
            _mockService.Setup(s => s.Delete(1, 1))
                .ReturnsAsync(Result<int>.Success(1, "Xóa dữ liệu thành công"));

            // Act
            var result = await _controller.Delete(1, 1);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal("Xóa dữ liệu thành công", result.Message);
        }
    }
}
