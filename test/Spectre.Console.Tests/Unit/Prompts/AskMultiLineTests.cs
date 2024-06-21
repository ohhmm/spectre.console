using System;
using System.Text;
using Shouldly;
using Spectre.Console.Tests.Unit;
using Xunit;

namespace Spectre.Console.Tests.Unit.Prompts
{
    [ExpectationPath("Prompts/AskMultiLine")]
    public sealed class AskMultiLineTests
    {
        [Fact]
        public void Should_Return_Multiline_Input()
        {
            // Given
            var console = new TestConsole();
            console.Input.PushTextWithEnter("Line 1");
            console.Input.PushTextWithEnter("Line 2");
            console.Input.PushTextWithEnter("END");

            // When
            var result = AnsiConsole.AskMultiLine("Enter text (type 'END' to finish):", "END");

            // Then
            result.ShouldBe("Line 1\nLine 2\n");
        }

        [Fact]
        public void Should_Return_Empty_String_If_No_Input()
        {
            // Given
            var console = new TestConsole();
            console.Input.PushTextWithEnter("END");

            // When
            var result = AnsiConsole.AskMultiLine("Enter text (type 'END' to finish):", "END");

            // Then
            result.ShouldBe(string.Empty);
        }

        [Fact]
        public void Should_Include_Blank_Lines_In_Result()
        {
            // Given
            var console = new TestConsole();
            console.Input.PushTextWithEnter("Line 1");
            console.Input.PushTextWithEnter("");
            console.Input.PushTextWithEnter("Line 2");
            console.Input.PushTextWithEnter("END");

            // When
            var result = AnsiConsole.AskMultiLine("Enter text (type 'END' to finish):", "END");

            // Then
            result.ShouldBe("Line 1\n\nLine 2\n");
        }
    }
}
