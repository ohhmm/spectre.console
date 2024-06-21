# Add AskMultiLine Method to AnsiConsole

## Summary
This pull request adds the `AskMultiLine` method to the `AnsiConsole` class in the Spectre.Console library. The method allows users to input multiple lines of text until a specified delimiter is encountered. The implementation is asynchronous and includes diagnostic output to assist in debugging.

## Changes
- Implemented the `AskMultiLine` method in `AnsiConsole.cs`.
- Added unit tests for the `AskMultiLine` method in `AskMultiLineTests.cs`.
- Updated `global.json` to specify the correct .NET SDK version.

## Testing
The following test cases have been added to verify the functionality of the `AskMultiLine` method:
- `Should_Return_Multiline_Input`: Verifies that the method returns the correct multiline input.
- `Should_Return_Empty_String_If_No_Input`: Verifies that the method returns an empty string if no input is provided.
- `Should_Include_Blank_Lines_In_Result`: Verifies that the method correctly includes blank lines in the returned string.

## Link to Devin run
[Devin run link](https://preview.devin.ai/devin/fa0ff25a1f22487f8e69e8ba8f9e318c)

Please review the changes and provide feedback.
