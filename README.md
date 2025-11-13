# AiAgent

A flexible and extensible AI agent framework built on .NET 8 using the LangChain library.

## Features

- **Ready-to-use Chat Assistant**: `ChatAssistant` hides OpenAI chat logic behind a simple API
- **Modular Tool System**: Add custom tools to extend agent capabilities
- **Conversation History**: Automatic tracking of chat interactions
- **Async Support**: Full async/await pattern implementation
- **LangChain Integration**: Built on top of LangChain.Core for robust AI functionality
- **Extensible Architecture**: Base class design for creating specialized agents

## Getting Started

### Prerequisites

- .NET 8.0 or later
- An OpenAI API key (or other compatible LLM provider)

### Installation

1. Clone the repository:
```bash
git clone https://github.com/TheStrul/AiAgent.git
cd AiAgent
```

2. Restore NuGet packages:
```bash
dotnet restore
```

3. Build the project:
```bash
dotnet build
```

4. (Optional) Run the examples:
```bash
cd Examples
dotnet run
```
See the [Examples README](Examples/README.md) for more details on running the example applications.

## Usage

### Quick Start

Implement a tool by following the `IAiTool` interface:

```csharp
using AiAgent.Tools;

public class CalculatorTool : IAiTool
{
    public string Name => "calculator";
    public string Description => "Performs basic arithmetic calculations";

    public Task<string> ExecuteAsync(string input)
    {
        // tool implementation here
        return Task.FromResult("42");
    }
}
```

Create and use the assistant:

```csharp
using AiAgent;

var assistant = new ChatAssistant();
assistant.RegisterTool(new CalculatorTool());
assistant.Init("your-openai-api-key");

var response = await assistant.ChatAsync("calculator: 2+2");
Console.WriteLine(response);
```

### Using the GitHub Repository Creation Tool

The `CreateRepositoryTool` allows you to create GitHub repositories programmatically:

```csharp
using AiAgent;
using AiAgent.Tools;

// Create the assistant and register the repository creation tool
var assistant = new ChatAssistant();
var repoTool = new CreateRepositoryTool("your-github-token", "your-username");
assistant.RegisterTool(repoTool);
assistant.Init("your-openai-api-key");

// Create a private repository
var response = await assistant.ChatAsync(
    "create_repository: name:my-new-repo,description:A test repository,private:true"
);
Console.WriteLine(response);
```

**Input Format:**
- `name:<repository-name>` (required) - The name of the repository to create
- `description:<description>` (optional) - Repository description
- `private:<true|false>` (optional, defaults to true) - Whether the repository should be private
- `owner:<username-or-org>` (optional) - Owner for the repository (uses authenticated user if not specified)

**Requirements:**
- A GitHub personal access token with `repo` scope (for creating repositories)
- The token must have appropriate permissions for the target owner/organization

## Project Structure

```
AiAgent/
├── AiAgent/                     # Main library project
│   ├── AiAgent.csproj           # Project file with dependencies
│   ├── ChatAssistant.cs         # High-level assistant wrapper
│   ├── AiAgentBase.cs           # Core agent implementation
│   └── Tools/
│       ├── IAiTool.cs          # Tool interface for user-defined extensions
│       └── CreateRepositoryTool.cs  # GitHub repository creation tool
├── Examples/                    # Example applications
│   ├── Program.cs               # Example code
│   └── README.md                # Examples documentation
├── README.md                    # This file
├── LICENSE                      # License information
└── .gitignore                   # Git ignore rules
```

## API Reference

### AiAgentBase Class

The main base class for creating AI agents.

#### Properties
- `Name`: Agent name
- `Description`: Agent description
- `Version`: Agent version
- `Author`: Agent author
- `Tools`: Read-only collection of available tools
- `ConversationHistory`: Read-only conversation history

#### Methods
- `Initialize()`: Initialize the agent
- `Initialize(IChatModel)`: Initialize with a specific chat model
- `AddTool(name, tool)`: Add a custom tool
- `RemoveTool(name)`: Remove a tool
- `ChatAsync(message)`: Send a message and get response
- `ClearConversationHistory()`: Clear conversation history
- `GetConversationSummary()`: Get a summary of recent conversations

## Dependencies

 - [LangChain.Core](https://www.nuget.org/packages/LangChain.Core/) - Core LangChain functionality
 - [LangChain.Providers.OpenAI](https://www.nuget.org/packages/LangChain.Providers.OpenAI/) - OpenAI provider
 - [Octokit](https://www.nuget.org/packages/Octokit/) - GitHub API client (for CreateRepositoryTool)

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Author

**TheStrul**

## Acknowledgments

- Built with [LangChain](https://github.com/tryAGI/LangChain) for .NET
- Inspired by the growing need for flexible AI agent frameworks