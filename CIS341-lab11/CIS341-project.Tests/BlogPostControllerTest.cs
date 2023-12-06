using CIS341_project.Controllers;
using CIS341_project.Data;
using CIS341_project.Models;
using CIS341_project.Services;
using CIS341_project.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CIS341_project.Tests
{
    public class BlogPostControllerTest
    {

        private readonly BlogContext _context;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<UserManager<IdentityUser>> _mockUserManager;
        private readonly BlogPostController _controller;

        public BlogPostControllerTest()
        {
            var options = new DbContextOptionsBuilder<BlogContext>()
                    .UseInMemoryDatabase(databaseName: "BlogTestDb")
                    .Options;

            _context = new BlogContext(options);

            _mockUserService = new Mock<IUserService>();
            _mockUserManager = new Mock<UserManager<IdentityUser>>(Mock.Of<IUserStore<IdentityUser>>(), null, null, null, null, null, null, null, null);

            _controller = new BlogPostController(_context, _mockUserService.Object, _mockUserManager.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResultWithBlogPosts()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task Details_WhenCalled_ReturnsViewResult()
        {
            // Arrange
            var testPost = new BlogPost
            {
                BlogPostId = 99,
                Title = "Test Post",
                Content = "Content",
                DatePublished = DateTime.Now,
                PostAuthor = "testauthor@lunchbox.com", 
            };

            _context.BlogPosts.Add(testPost);
            _context.SaveChanges();

            // Act
            var result = await _controller.Details(testPost.BlogPostId);

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task Create_PostValidBlogPost_ReturnsRedirectToActionResult()
        {
            // Arrange
            var blogPostDTO = new BlogPostDTO { Title = "New Post", Content = "Content" };

            var user = new IdentityUser { UserName = "TestUser" };
            _mockUserManager.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            var claims = new List<Claim> { new Claim(ClaimTypes.Role, "Admin") };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(m => m.User).Returns(claimsPrincipal);
            _controller.ControllerContext = new ControllerContext() { HttpContext = mockHttpContext.Object };

            // Act
            var result = await _controller.Create(blogPostDTO);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>();
            var redirectToActionResult = result as RedirectToActionResult;
            redirectToActionResult.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task Create_PostRequestWithInvalidData_ReturnsViewResultWithSameViewModel()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public void Edit_GetRequestWithValidId_ReturnsViewResultWithBlogPost()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public void Edit_GetRequestWithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task Edit_PostRequestWithValidData_ReturnsRedirectToActionResult()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task Edit_PostRequestWithInvalidData_ReturnsViewResultWithSameViewModel()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task Delete_GetRequestWithValidId_ReturnsViewResult()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task Delete_GetRequestWithInvalidId_ReturnsNotFoundResult()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task DeleteConfirmed_PostRequestWithValidId_ReturnsRedirectToActionResult()
        {
            // Arrange

            // Act

            // Assert
        }

        [Fact]
        public async Task ResetPostReactions_WithValidId_ReturnsRedirectToActionResult()
        {
            // Arrange

            // Act

            // Assert
        }

    }
}