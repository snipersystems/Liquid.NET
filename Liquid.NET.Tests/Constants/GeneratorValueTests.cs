﻿using System.Collections.Generic;
using System.Linq;

using Liquid.NET.Constants;

using NUnit.Framework;

namespace Liquid.NET.Tests.Constants
{
    [TestFixture]
    public class GeneratorValueTests
    {
        [Test]
        [TestCase(3, 7, new []{3,4,5,6,7})]
        [TestCase(7, 3, new[] { 7,6,5,4,3 })]
        [TestCase(0, 0, new [] { 0 })]
        public void It_Should_Generate_Some_Values(int start, int end, int[] expected )
        {
            // Arrange
            var generatorValue = new GeneratorValue(new NumericValue(start), new NumericValue(end));

            // Act
            var result = generatorValue.AsEnumerable();

            // Assert
            Assert.That(result.Select(x => x.Value), Is.EqualTo(expected.ToList()));

        }

        [Test]
        public void It_Should_Generate_Some_Values_Descending()
        {
            // Arrange
            var generatorValue = new GeneratorValue(new NumericValue(5), new NumericValue(2));

            // Act
            var result = generatorValue.AsEnumerable();

            // Assert
            Assert.That(result.Select(x => x.Value), Is.EqualTo(new List<int> { 5,4,3,2 }));

        }

        [Test]
        public void It_Should_Determine_The_Length_Of_a_Generator()
        {
            // Arrange
            var generatorValue = new GeneratorValue(new NumericValue(2), new NumericValue(5));

            // Assert
            Assert.That(generatorValue.Length, Is.EqualTo(4));

        }

        [Test]
        public void It_Should_Determine_The_Length_Of_a_Descending_Generator()
        {
            // Arrange
            var generatorValue = new GeneratorValue(new NumericValue(5), new NumericValue(2));
            
            // Assert
            Assert.That(generatorValue.Length, Is.EqualTo(4));

        }


    }
}
