using DominosCutScreen.Shared;

namespace Tests
{
    public class OrderTests
    {
        [Fact]
        public void IsLive_TimedOrder_ShouldBeTrue()
        {
            // Arrange
            var data = new MakeLineOrder()
            {
                IsBumped = false,
                IsTimedOrder = true,
                SavedAt = DateTime.Now
            };

            // Act

            // Assert
            Assert.True(data.IsLive());
        }

        [Fact]
        public void IsLive_ASAPOrder_ShouldBeTrue()
        {
            // Arrange
            var data = new MakeLineOrder()
            {
                IsBumped = false,
                IsTimedOrder = false,
                ActualOrderedAt = DateTime.Now
            };

            // Act

            // Assert
            Assert.True(data.IsLive());
        }

        [Fact]
        public void IsLive_TimedOrder_ShouldBeFalse()
        {
            // Arrange
            var data = new MakeLineOrder()
            {
                IsBumped = false,
                IsTimedOrder = true,
                SavedAt = DateTime.Now + TimeSpan.FromSeconds(1)
            };

            // Act

            // Arrange
            Assert.False(data.IsLive());
        }

        [Fact]
        public void IsLive_ASAPBumped_ShouldBeFalse()
        {
            // Arrange
            var data = new MakeLineOrder()
            {
                IsBumped = true,
                IsTimedOrder = false,
                ActualOrderedAt = DateTime.Now
            };

            // Act

            // Arrange
            Assert.False(data.IsLive());
        }



        public static IEnumerable<object[]> LiveOrderData()
        {
            yield return new[] { new MakeLineOrder { IsBumped = false, IsTimedOrder = true, SavedAt = DateTime.Now } };
            yield return new[] { new MakeLineOrder { IsBumped = false, IsTimedOrder = false, ActualOrderedAt = DateTime.Now } };
        }
        public static IEnumerable<object[]> TimedOrderData()
        {
            yield return new[] { new MakeLineOrder { IsBumped = false, IsTimedOrder = true, SavedAt = DateTime.Now + TimeSpan.FromSeconds(1) } };
            yield return new[] { new MakeLineOrder { IsBumped = false, IsTimedOrder = false, ActualOrderedAt = DateTime.Now + TimeSpan.FromSeconds(1) } };
        }

        [Theory]
        [MemberData(nameof(LiveOrderData))]
        public void IsLive_Many_ShouldBeTrue(MakeLineOrder order)
        {
            // Arrange

            // Act

            // Assert
            Assert.True(order.IsLive());
        }

        [Theory]
        [MemberData(nameof(TimedOrderData))]
        public void IsLive_Many_ShouldBeFalse(MakeLineOrder order)
        {
            // Arrange

            // Act

            // Assert
            Assert.False(order.IsLive());
        }
    }
}