# AiAgent Examples

This directory contains example applications demonstrating how to use the AiAgent library.

## CreateRepositoryTool Example

The main example (`Program.cs`) demonstrates how to use the `CreateRepositoryTool` to create GitHub repositories programmatically.

### Prerequisites

To run this example, you need:

1. **GitHub Personal Access Token** with `repo` scope
   - Create one at: https://github.com/settings/tokens
   - Ensure it has permission to create repositories

2. **OpenAI API Key**
   - Get one at: https://platform.openai.com/api-keys

### Setup

Set the required environment variables:

```bash
export GITHUB_TOKEN=your_github_personal_access_token
export OPENAI_API_KEY=your_openai_api_key
```

### Running the Example

```bash
cd Examples
dotnet run
```

### What the Example Does

The example creates two test repositories:
1. A private repository with a timestamped name
2. A public repository with a timestamped name

Both repositories are created with descriptions and initialized with a README.

### Expected Output

```
=== AiAgent GitHub Repository Creation Example ===

Assistant initialized with CreateRepositoryTool.

Example 1: Creating a private test repository...
Response: Successfully created repository: https://github.com/username/test-private-repo-123456
Name: test-private-repo-123456
Owner: username
Private: True
Description: A test private repository created by AiAgent

Example 2: Creating a public test repository...
Response: Successfully created repository: https://github.com/username/test-public-repo-789012
Name: test-public-repo-789012
Owner: username
Private: False
Description: A test public repository created by AiAgent

Examples completed successfully!
```

### Cleanup

Remember to delete the test repositories created by this example from your GitHub account when you're done testing.

## Creating Your Own Examples

To create your own example:

1. Reference the AiAgent project in your example
2. Create instances of tools you want to use
3. Register the tools with a `ChatAssistant`
4. Initialize the assistant with your API keys
5. Use `ChatAsync` to interact with the assistant

See `Program.cs` for a complete working example.
