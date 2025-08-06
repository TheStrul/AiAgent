# AiAgent

A flexible and extensible AI agent framework built on .NET 9 using the LangChain library.

## Features

- **Modular Tool System**: Add custom tools to extend agent capabilities
- **Conversation History**: Automatic tracking of chat interactions
- **Async Support**: Full async/await pattern implementation
- **LangChain Integration**: Built on top of LangChain.Core for robust AI functionality
- **Extensible Architecture**: Base class design for creating specialized agents

## Getting Started

### Prerequisites

- .NET 9.0 or later
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

### Basic Agent Setup

```csharp
using AiAgent;
using LangChain.Providers.OpenAI;

// Create an AI agent
var agent = new AiAgentBase(
    name: "MyAgent",
    description: "A helpful AI assistant",
    version: "1.0.0",
    author: "Your Name"
);

// Initialize with a chat model
var chatModel = new OpenAiChatModel(
    apiKey: "your-openai-api-key",
    modelId: "gpt-3.5-turbo"
);

agent.Initialize(chatModel);

// Chat with the agent
var response = await agent.ChatAsync("Hello, how can you help me?");
Console.WriteLine(response);
```

### Adding Custom Tools

```csharp
// Add a simple tool
agent.AddTool(
    "calculator",
    "Performs basic arithmetic calculations",
    async (input) => {
        // Your tool logic here
        return $"Calculation result: {input}";
    }
);

// Use the tool
var toolResponse = await agent.ChatAsync("calculator: 2+2");
```

### Creating Custom Agents

```csharp
public class SpecializedAgent : AiAgentBase
{
    public SpecializedAgent() : base("Specialized", "A specialized agent", "1.0", "Author")
    {
    }

    protected override void InitializeDefaultTools()
    {
        base.InitializeDefaultTools();
        
        // Add specialized tools
        AddTool("special_function", "Does something special", SpecialFunction);
    }

    private async Task<string> SpecialFunction(string input)
    {
        // Implement your specialized functionality
        return "Special result";
    }
}
```

## Project Structure

```
AiAgent/
??? AiAgent.csproj          # Project file with dependencies
??? AiAgentBase.cs          # Main agent base class
??? Tools/                  # Directory for custom tools (planned)
??? README.md              # This file
??? LICENSE                # License information
??? .gitignore            # Git ignore rules
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
- [LangChain.Chains.StackableChains](https://www.nuget.org/packages/LangChain.Chains.StackableChains/) - Agent tools support

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