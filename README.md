# Bullet List Formatter

Bullet List Formatter is a command-line tool that takes an HTML-formatted bullet list from the clipboard, such as a list copied from Microsoft Word, converts it to a plain-text formatted list with tabs for indentation, and copies the result back to the clipboard. The tool also retains any non-bullet text in the input.

## Features

- Extracts bullet lists from HTML content in the clipboard, including lists copied from Microsoft Word
- Preserves list indentation using tabs
- Keeps non-bullet text in the input
- Copies the formatted plain-text list back to the clipboard

## Usage

1. Copy an HTML-formatted bullet list to the clipboard, such as a list from Microsoft Word.
2. Run the BulletListFormatter executable.
3. The tool will process the list and copy the formatted plain-text version back to the clipboard.
4. You can now paste the formatted list into any plain-text editor, such as Notepad.

## Dependencies

- .NET Core
- AngleSharp
- AngleSharp.Css

## Code Structure

- `Program.cs` contains the main program, including the `Main` method and helper functions to interact with the clipboard and process bullet lists.
- `ProcessBulletList` function takes the HTML content and processes it to extract and format the bullet list.
- `ProcessList` is a recursive function that processes the elements of the HTML content and generates the formatted plain-text list.
- `ComputeIndentationFromMarginLeft` function calculates the indentation level of a list item based on its margin-left CSS property.

## License

This project is released under the MIT License. See [LICENSE](LICENSE) for details.
