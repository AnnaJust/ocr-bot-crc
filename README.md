# OCR Discord Bot

A Discord bot built with .NET 9 and DSharpPlus that performs Optical Character Recognition with OCR.Space API and manages user profiles in a PostgreSQL database.

## Features
- **OCR Extraction**: Upload an image and receive extracted text.
- **User Profiles**: Save, update, and retrieve user default language preferences.
- **Help Command**: Dynamic listing of all available commands.

## Commands

| Command                    | Description                                                      |
| -------------------------- | ---------------------------------------------------------------- |
| `!hello`                   | Bot description.                                                 |
| `!ocr [language]`          | Extracts text from the attached image. Optional `language`.      |
| `!save-profile [language]` | Saves your profile with a default OCR language (default: `eng`). |
| `!my-profile`              | Displays your saved profile information.                         |
| `!user-profile <name>`     | Displays another user’s profile by username.                     |
| `!set-language <language>` | Updates your default OCR language.                               |
| `!help`                    | Lists all commands with descriptions.                            |
