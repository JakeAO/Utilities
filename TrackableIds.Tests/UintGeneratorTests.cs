using System;
using NUnit.Framework;
using SadPumpkin.Util.TrackableIds;

namespace SadPumpkin.Util.TrackableIds.Tests
{
    [TestFixture]
    public class UintGeneratorTests
    {
        [Test]
        public void Constructor_InitializesPropertiesCorrectly()
        {
            // Arrange
            uint min = 100;
            uint count = 500;
            bool wrapIds = true;
            
            // Act
            var generator = new UintGenerator(min, count, wrapIds);
            
            // Assert
            Assert.AreEqual(min, generator.Min);
            Assert.AreEqual(min + count - 1, generator.Max);
            Assert.AreEqual(min, generator.Current);
        }
        
        [Test]
        public void GetNext_ReturnsSequentialValues()
        {
            // Arrange
            uint min = 100;
            uint count = 500;
            var generator = new UintGenerator(min, count);
            
            // Act & Assert
            for (uint i = 0; i < 10; i++)
            {
                uint expected = min + i;
                uint actual = generator.GetNext();
                Assert.AreEqual(expected, actual);
                Assert.AreEqual(expected + 1, generator.Current);
            }
        }
        
        [Test]
        public void GetNext_WithWrapTrue_WrapsAroundToMin()
        {
            // Arrange
            uint min = 100;
            uint count = 5;
            var generator = new UintGenerator(min, count, true);
            
            // Set Current to the max value
            for (int i = 0; i < count - 1; i++)
            {
                generator.GetNext();
            }
            Assert.AreEqual(min + count - 1, generator.Current);
            
            // Act - get the max value
            uint lastValue = generator.GetNext();
            
            // Assert - should wrap back to min
            Assert.AreEqual(min + count - 1, lastValue);
            Assert.AreEqual(min, generator.GetNext());
        }
        
        [Test]
        public void GetNext_WithWrapFalse_ThrowsException()
        {
            // Arrange
            uint min = 100;
            uint count = 5;
            var generator = new UintGenerator(min, count, false);
            
            // Set Current to max value
            for (int i = 0; i < count - 1; i++)
            {
                generator.GetNext();
            }
            // Get the last valid value
            generator.GetNext();
            
            // Act & Assert
            Assert.Throws<IndexOutOfRangeException>(() => generator.GetNext());
        }
        
        [Test]
        public void GetNext_WithMaximumRange_Works()
        {
            // Arrange
            uint min = 0;
            uint count = 10; // Small count for test efficiency
            var generator = new UintGenerator(min, count);
            
            // Act & Assert
            for (uint i = 0; i < count; i++)
            {
                uint value = generator.GetNext();
                Assert.AreEqual(i, value);
            }
        }
        
        [Test]
        public void Constructor_WithDefaultParameters_SetsCorrectDefaults()
        {
            // Arrange & Act
            var generator = new UintGenerator(1);
            
            // Assert
            Assert.AreEqual(1u, generator.Min);
            Assert.AreEqual(1000u, generator.Max - generator.Min + 1); // Count is 1000
            Assert.AreEqual(1u, generator.Current);
        }
    }
}
