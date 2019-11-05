using LevelCounter.Controllers;
using LevelCounter.Exceptions;
using LevelCounter.Models;
using LevelCounter.Services;
using LevelCounterTests.UnitTests.TestUtils;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace LevelCounterTests
{
    public class GameControllerTest
    {
        [Fact]
        public async Task StartGame_WithValidInput_ReturnsOk()
        {
            // Arrange
            var gameServiceMock = new Mock<IGameService>();
            var controllerContext = ControllerContextProvider.GetDefault();
            var controller = new GameController(gameServiceMock.Object)
            {
                ControllerContext = controllerContext
            };
            var gameId = 1;
            var userId = "userid";
            var game = new Game();

            gameServiceMock.Setup(x => x.CheckHostId(gameId, userId))
                .Returns(true);
            gameServiceMock.Setup(x => x.StartGameAsync(gameId, userId))
                .Returns(Task.FromResult(game));

            // Act
            var result = await controller.StartGame(gameId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task StartGame_WithNotExistingInput_ReturnsBadRequest()
        {
            // Arrange
            var gameServiceMock = new Mock<IGameService>();
            var controllerContext = ControllerContextProvider.GetDefault();
            var controller = new GameController(gameServiceMock.Object)
            {
                ControllerContext = controllerContext
            };
            var gameId = 1;
            var userId = "userid";

            gameServiceMock.Setup(x => x.CheckHostId(gameId, userId))
                .Returns(true);
            gameServiceMock.Setup(x => x.StartGameAsync(gameId, userId))
                .Throws(new ItemNotFoundException());

            // Act
            var result = await controller.StartGame(gameId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task StartGame_WithInvalidHostId_ReturnsBadRequest()
        {
            // Arrange
            var gameServiceMock = new Mock<IGameService>();
            var controllerContext = ControllerContextProvider.GetDefault();
            var controller = new GameController(gameServiceMock.Object)
            {
                ControllerContext = controllerContext
            };
            var gameId = 1;
            var userId = "test-userid";
            
            gameServiceMock.Setup(x => x.CheckHostId(gameId, userId))
                .Returns(false);

            // Act
            var result = await controller.StartGame(gameId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);

        }
    }
}
