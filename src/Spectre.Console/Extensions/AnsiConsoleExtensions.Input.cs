namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IAnsiConsole"/>.
    /// </summary>
    public static partial class AnsiConsoleExtensions
    {
        internal static async Task<string> ReadLine(this IAnsiConsole console, Style? style, bool secret, char? mask, IEnumerable<string>? items = null, CancellationToken cancellationToken = default)
        {
            if (console is null)
            {
                throw new ArgumentNullException(nameof(console));
            }

            style ??= Style.Plain;
            var text = string.Empty;
            var cursorLeft = 0;
            var cursorTop = 0;

            var autocomplete = new List<string>(items ?? Enumerable.Empty<string>());

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var rawKey = await console.Input.ReadKeyAsync(true, cancellationToken).ConfigureAwait(false);
                if (rawKey == null)
                {
                    continue;
                }

                var key = rawKey.Value;
                if (key.Key == ConsoleKey.Enter)
                {
                    return text;
                }

                if (key.Key == ConsoleKey.Tab && autocomplete.Count > 0)
                {
                    var autoCompleteDirection = key.Modifiers.HasFlag(ConsoleModifiers.Shift)
                        ? AutoCompleteDirection.Backward
                        : AutoCompleteDirection.Forward;
                    var replace = AutoComplete(autocomplete, text, autoCompleteDirection);
                    if (!string.IsNullOrEmpty(replace))
                    {
                        // Render the suggestion
                        console.Write("\b \b".Repeat(text.Length), style);
                        console.Write(replace);
                        text = replace;
                        continue;
                    }
                }

                var lastNewLineIndex = -1;
                if (key.Key == ConsoleKey.Backspace)
                {
                    if (text.Length > 0)
                    {
                        // Remove the last character from the text
                        text = text.Substring(0, text.Length - 1);

                        // Move the cursor back one position
                        cursorLeft--;
                        if (cursorLeft < 0)
                        {
                            cursorTop--;
                            if (cursorTop < 0)
                            {
                                cursorTop = 0;
                                cursorLeft = 0;
                            }
                            else
                            {
                                // Move to the end of the previous line
                                lastNewLineIndex = text.LastIndexOf('\n', text.Length - 1);
                                if (lastNewLineIndex >= 0)
                                {
                                    var secondLastNewLineIndex = text.LastIndexOf('\n', lastNewLineIndex - 1);
                                    if (secondLastNewLineIndex >= 0)
                                    {
                                        cursorLeft = lastNewLineIndex - secondLastNewLineIndex - 1;
                                    }
                                    else
                                    {
                                        cursorLeft = lastNewLineIndex;
                                    }
                                }
                                else
                                {
                                    cursorLeft = text.Length;
                                }
                            }
                        }

                        // Clear the character at the current cursor position
                        console.Write("\b \b");
                        console.Cursor.SetPosition(cursorLeft, cursorTop);
                    }

                    // Debugging: Log cursor position and text state
                    console.WriteLine($"[DEBUG] Cursor Position: ({cursorLeft}, {cursorTop})");
                    console.WriteLine($"[DEBUG] Text: {text}");
                    console.WriteLine($"[DEBUG] Last NewLine Index: {lastNewLineIndex}");

                    continue;
                }

                if (!char.IsControl(key.KeyChar))
                {
                    text += key.KeyChar.ToString();
                    var output = key.KeyChar.ToString();
                    console.Write(secret ? output.Mask(mask) : output, style);
                    cursorLeft++;
                    if (cursorLeft >= console.Profile.Width)
                    {
                        cursorLeft = 0;
                        cursorTop++;
                    }
                }
            }
        }

        private static string AutoComplete(List<string> autocomplete, string text, AutoCompleteDirection autoCompleteDirection)
        {
            var found = autocomplete.Find(i => i == text);
            var replace = string.Empty;

            if (found == null)
            {
                // Get the closest match
                var next = autocomplete.Find(i => i.StartsWith(text, true, CultureInfo.InvariantCulture));
                if (next != null)
                {
                    replace = next;
                }
                else if (string.IsNullOrEmpty(text))
                {
                    // Use the first item
                    replace = autocomplete[0];
                }
            }
            else
            {
                // Get the next match
                replace = GetAutocompleteValue(autoCompleteDirection, autocomplete, found);
            }

            return replace;
        }

        private static string GetAutocompleteValue(AutoCompleteDirection autoCompleteDirection, IList<string> autocomplete, string found)
        {
            var foundAutocompleteIndex = autocomplete.IndexOf(found);
            var index = autoCompleteDirection switch
            {
                AutoCompleteDirection.Forward => foundAutocompleteIndex + 1,
                AutoCompleteDirection.Backward => foundAutocompleteIndex - 1,
                _ => throw new ArgumentOutOfRangeException(nameof(autoCompleteDirection), autoCompleteDirection, null),
            };

            if (index >= autocomplete.Count)
            {
                index = 0;
            }

            if (index < 0)
            {
                index = autocomplete.Count - 1;
            }

            return autocomplete[index];
        }

        private enum AutoCompleteDirection
        {
            Forward,
            Backward,
        }
    }
}
