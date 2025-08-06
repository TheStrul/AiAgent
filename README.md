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

## Project Structure

```
AiAgent/
├── AiAgent.csproj        # Project file with dependencies
├── ChatAssistant.cs      # High-level assistant wrapper
├── AiAgentBase.cs        # Core agent implementation
├── Tools/
│   └── IAiTool.cs       # Tool interface for user-defined extensions
├── README.md            # This file
├── LICENSE              # License information
└── .gitignore          # Git ignore rules
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