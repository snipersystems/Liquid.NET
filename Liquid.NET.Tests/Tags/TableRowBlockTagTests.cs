﻿using System;
using Liquid.NET.Constants;
using Liquid.NET.Tests.Ruby;
using Xunit;

namespace Liquid.NET.Tests.Tags
{
    
    public class TableRowBlockTagTests
    {
        [Fact]
        public void It_Should_Render_A_Table_Row()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("numbers", DictionaryFactory.CreateArrayFromJson("[1, 2, 3, 4, 5, 6]"));
            var template = LiquidTemplate.Create(@"{% tablerow n in numbers cols:2%}ITER{{n}}{% endtablerow %}");
            
            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;
            Logger.Log(result);
            String expected =
                "<tr class=\"row1\">\r\n" +
                @"<td class=""col1"">ITER1</td><td class=""col2"">ITER2</td></tr>"+"\r\n"+
                @"<tr class=""row2""><td class=""col1"">ITER3</td><td class=""col2"">ITER4</td></tr>" + "\r\n"+
                @"<tr class=""row3""><td class=""col1"">ITER5</td><td class=""col2"">ITER6</td></tr>";
            // Act

            // Assert
            Assert.Contains(expected, result);

        }

        [Fact]
        public void It_Should_Render_A_Table_Row_From_A_String()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            ctx.DefineLocalVariable("numbers", DictionaryFactory.CreateArrayFromJson("[1, 2, 3, 4, 5, 6]"));
            var template = LiquidTemplate.Create(@"{% tablerow n in ""Test"" cols:2%}ITER{{n}}{% endtablerow %}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;
            Logger.Log(result);
            String expected =
                "<tr class=\"row1\">\r\n<td class=\"col1\">ITERTest</td></tr>";
            // Act

            // Assert
            Assert.Contains(expected, result);

        }


        [Fact]
        public void It_Should_Render_A_Table_Row_With_A_Generator()
        {
            // Arrange
            TemplateContext ctx = new TemplateContext();
            var template = LiquidTemplate.Create(@"{% tablerow n in (1..6) cols:2%}ITER{{n}}{% endtablerow %}");

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;
            Logger.Log(result);
            String expected =
                "<tr class=\"row1\">\r\n" +
                @"<td class=""col1"">ITER1</td><td class=""col2"">ITER2</td></tr>" + "\r\n" +
                @"<tr class=""row2""><td class=""col1"">ITER3</td><td class=""col2"">ITER4</td></tr>" + "\r\n" +
                @"<tr class=""row3""><td class=""col1"">ITER5</td><td class=""col2"">ITER6</td></tr>";
            // Act

            // Assert
            Assert.Contains(expected, result);

        }

        [Fact]
        public void It_Should_Allow_Variables_In_Args()
        {
            // Arrange
            const string templateString = "Result : {% tablerow i in array cols: x limit: y offset: z %}{{ i }}{% endtablerow %}";
            TemplateContext ctx = new TemplateContext();
            var arr = new LiquidCollection();
            for (int i = 1; i < 10; i++)
            {
                arr.Add(LiquidNumeric.Create(i));
            }

            ctx.DefineLocalVariable("array", arr);
            ctx.DefineLocalVariable("x", LiquidNumeric.Create(2));
            ctx.DefineLocalVariable("y", LiquidNumeric.Create(3));
            ctx.DefineLocalVariable("z", LiquidNumeric.Create(1));
            var template = LiquidTemplate.Create(templateString);

            // Act
            String result = template.LiquidTemplate.Render(ctx).Result;
            Logger.Log(result);

            // Assert
            Assert.Contains("<tr class=\"row1\">", result);
            Assert.Contains("<tr class=\"row2\">", result);
            Assert.DoesNotContain("<tr class=\"row3\">", result);
            Assert.DoesNotContain(">1</td>", result);
            Assert.Contains(">2</td>", result);
            Assert.Contains(">4</td>", result);
            Assert.DoesNotContain(">5</td>", result);
            
        }

    }
}
