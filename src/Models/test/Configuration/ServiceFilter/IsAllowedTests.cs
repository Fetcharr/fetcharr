using Fetcharr.Models.Configuration;

namespace Fetcharr.Models.Tests.Configuration.ServiceFilterTests
{
    public class IsAllowedTests
    {
        [Fact]
        public void IsAllowed_ThrowsArgumentNullException_GivenNullString()
        {
            // Arrange
            ServiceFilter filter = [];

            // Act
            Action act = () => filter.IsAllowed(value: null!);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void IsAllowed_ThrowsArgumentException_GivenEmptyString()
        {
            // Arrange
            ServiceFilter filter = [];

            // Act
            Action act = () => filter.IsAllowed(string.Empty);

            // Assert
            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void IsAllowed_ThrowsArgumentNullException_GivenEmptyList()
        {
            // Arrange
            ServiceFilter filter = [];

            // Act
            Action act = () => filter.IsAllowed(values: []);

            // Assert
            act.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void IsAllowed_ReturnsImplicitlyAllowed_GivenEmptyFilter()
        {
            // Arrange
            ServiceFilter filter = [];

            // Act
            ServiceFilterAllowType allowance = filter.IsAllowed("value");

            // Assert
            allowance.Should().Be(ServiceFilterAllowType.ImplicitlyAllowed);
        }

        [Fact]
        public void IsAllowed_ReturnsNotAllowed_GivenNonmatchingFilter()
        {
            // Arrange
            ServiceFilter filter = ["anime"];

            // Act
            ServiceFilterAllowType allowance = filter.IsAllowed("animation");

            // Assert
            allowance.Should().Be(ServiceFilterAllowType.NotAllowed);
        }

        [Fact]
        public void IsAllowed_ReturnsExplicitlyAllowed_GivenMatchingFilter()
        {
            // Arrange
            ServiceFilter filter = ["anime"];

            // Act
            ServiceFilterAllowType allowance = filter.IsAllowed("anime");

            // Assert
            allowance.Should().Be(ServiceFilterAllowType.ExplicitlyAllowed);
        }

        [Fact]
        public void IsAllowed_ReturnsExplicitlyAllowed_GivenMatchingFilterWithMultipleItems()
        {
            // Arrange
            ServiceFilter filter = ["anime", "animation", "fantasy"];

            // Act
            ServiceFilterAllowType allowance = filter.IsAllowed("anime");

            // Assert
            allowance.Should().Be(ServiceFilterAllowType.ExplicitlyAllowed);
        }

        [Fact]
        public void IsAllowed_ReturnsExplicitlyAllowed_GivenMatchingFilterWithDifferentCasing()
        {
            // Arrange
            ServiceFilter filter = ["anime"];

            // Act
            ServiceFilterAllowType allowance = filter.IsAllowed("ANImE");

            // Assert
            allowance.Should().Be(ServiceFilterAllowType.ExplicitlyAllowed);
        }
    }
}