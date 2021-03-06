﻿using System;
using System.Collections.Generic;
using System.Linq;
using Liquid.NET.Constants;
using Liquid.NET.Filters.Array;
using Xunit;

namespace Liquid.NET.Tests.Filters.Array
{
    
    public class SortFilterTests
    {
        [Fact]
        public void It_Should_Sort_An_Array_By_StringValues()
        {
            // Arrange
            LiquidCollection liquidCollection = new LiquidCollection{
                LiquidString.Create("a string"), 
                LiquidNumeric.Create(123), 
                LiquidNumeric.Create(456m),
                new LiquidBoolean(false)
            };
            var filter = new SortFilter(LiquidString.Create(""));

            // Act            
            var result = filter.Apply(new TemplateContext(), liquidCollection);
            var resultStrings = result.SuccessValue<LiquidCollection>().Select(ValueCaster.RenderAsString);
            
            // Assert
            Assert.Equal(new List<String>{"123", "456.0", "a string", "false"}, resultStrings);

        }


        [Fact]
        public void It_Should_Sort_Dictionaries_By_Field()
        {
            // Arrange
            SortFilter sizeFilter = new SortFilter(LiquidString.Create("field1"));

            // Act
            var result = sizeFilter.Apply(new TemplateContext(), CreateObjList());

            // Assert
            Assert.Equal("Aa", IdAt(result.SuccessValue<LiquidCollection>(), 0, "field1").Value);
            Assert.Equal("ab", IdAt(result.SuccessValue<LiquidCollection>(), 1, "field1").Value);
            Assert.Equal("b", IdAt(result.SuccessValue<LiquidCollection>(), 2, "field1").Value);
            Assert.Equal("Z", IdAt(result.SuccessValue<LiquidCollection>(), 3, "field1").Value);
        }

        [Fact]
        public void It_Should_Sort_Dictionaries_By_Field_From_Template()
        {
            // Arrange            
            ITemplateContext ctx = new TemplateContext()
                .WithAllFilters().DefineLocalVariable("arr", CreateObjList());

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% assign x = arr | sort: \"field1\" %}{{ x | map: \"field1\" }}", ctx);

            // Assert            
            Assert.Equal("Result : AaabbZ", result);
        }

        [Fact]
        public void It_Should_Ignore_Dictionaries_With_Missing_Fields()
        {
            // Arrange            
            ITemplateContext ctx = new TemplateContext()
                .WithAllFilters().DefineLocalVariable("arr", CreateObjList());

            // Act
            var result = RenderingHelper.RenderTemplate("Result : {% assign x = arr | sort: \"test\" %}{{ x | map: \"id\" }}", ctx);

            // Assert            
            Assert.Equal("Result : 1234", result);
        }

        [Fact]
        public void It_Should_Error_With_Dictionaries_With_Missing_Fields_When_Errors_On()
        {
            // Arrange            
            ITemplateContext ctx = new TemplateContext().ErrorWhenValueMissing()
                .WithAllFilters().DefineLocalVariable("arr", CreateObjList());

            // Act
            //var result = RenderingHelper.RenderTemplate("Result : {% assign x = arr | sort: \"test\" %}", ctx);
            var template = LiquidTemplate.Create("Result : {% assign x = arr | sort: \"test\" %}");
            var result = template.LiquidTemplate.Render(ctx);

            // Assert            
            Assert.Contains("an array element is missing the field \'test\'", result.Result);
        }


        private LiquidCollection CreateObjList()
        {
            return new LiquidCollection
            {
                DataFixtures.CreateDictionary(1, "Aa", "Value 1 B"), 
                DataFixtures.CreateDictionary(2, "Z", "Value 2 B"), 
                DataFixtures.CreateDictionary(3, "ab", "Value 3 B"), 
                DataFixtures.CreateDictionary(4, "b", "Value 4 B"),
            };
        }

        private static ILiquidValue IdAt(LiquidCollection result, int index, String field)
        {
            return ((LiquidHash)result[index].Value)[field].Value;
        }
    }
}
