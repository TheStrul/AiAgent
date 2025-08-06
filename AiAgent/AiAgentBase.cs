namespace AiAgent
{
    using AiAgent.Tools;
    using LangChain.Providers;
    using LangChain.Schema;

    internal sealed class DelegateTool : IAiTool
    {
        private readonly Func<string, Task<string>> func;

        public DelegateTool(string name, string description, Func<string, Task<string>> func)
        {
            Name = name;
            Description = description;
            this.func = func;
        }

        public string Name { get; }
        public string Description { get; }
        public Task<string> ExecuteAsync(string input) => func(input);
    }

    public class AiAgentBase
    {
        private readonly Dictionary<string, IAiTool> tools = new();
        private readonly List<Message> conversationHistory = new();
        private IChatModel? chatModel;

        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }

        public IReadOnlyDictionary<string, IAiTool> Tools => tools;
        public IReadOnlyList<Message> ConversationHistory => conversationHistory;

        public AiAgentBase(string name, string description, string version, string author)
        {
            Name = name;
            Description = description;
            Version = version;
            Author = author;
        }

        public virtual void Initialize()
        {
            // Initialization logic for the AI agent
            InitializeDefaultTools();
        }

        public virtual void Initialize(IChatModel model)
        {
            chatModel = model;
            Initialize();
        }

        protected virtual void InitializeDefaultTools()
        {
            // Override in derived classes to add specific tools
        }

        public void AddTool(IAiTool tool) => tools[tool.Name] = tool;

        public void AddTool(string name, string description, Func<string, Task<string>> function)
        {
            var tool = new DelegateTool(name, description, function);
            AddTool(tool);
        }

        public bool RemoveTool(string name) => tools.Remove(name);

        public IAiTool? GetTool(string name) => tools.TryGetValue(name, out var tool) ? tool : null;

        public async Task<string> ChatAsync(string userMessage)
        {
            if (chatModel == null)
            {
                throw new InvalidOperationException("Chat model not initialized. Call Initialize(IChatModel) first.");
            }

            // Add user message to conversation history
            var userMsg = Message.Human(userMessage);
            conversationHistory.Add(userMsg);

            try
            {
                // Check if the message requires tool usage
                var toolResponse = await ProcessWithToolsAsync(userMessage);
                if (toolResponse != null)
                {
                    var toolMsg = Message.Ai(toolResponse);
                    conversationHistory.Add(toolMsg);
                    return toolResponse;
                }

                // Generate response using the chat model
                var response = await chatModel.GenerateAsync(conversationHistory.ToArray());
                var lastMessage = response.Messages.LastOrDefault();
                var responseMessage = lastMessage.Content ?? "No response generated.";

                // Add AI response to conversation history
                var aiMsg = Message.Ai(responseMessage);
                conversationHistory.Add(aiMsg);

                return responseMessage;
            }
            catch (Exception ex)
            {
                var errorMsg = $"Error processing message: {ex.Message}";
                var errorMessage = Message.Ai(errorMsg);
                conversationHistory.Add(errorMessage);
                return errorMsg;
            }
        }

        protected virtual async Task<string?> ProcessWithToolsAsync(string userMessage)
        {
            // Simple tool detection - override for more sophisticated tool selection
            foreach (var tool in tools.Values)
            {
                if (ShouldUseTool(userMessage, tool))
                {
                    try
                    {
                        var result = await tool.ExecuteAsync(userMessage);
                        return $"Tool '{tool.Name}' executed: {result}";
                    }
                    catch (Exception ex)
                    {
                        return $"Error executing tool '{tool.Name}': {ex.Message}";
                    }
                }
            }
            return null;
        }

        protected virtual bool ShouldUseTool(string userMessage, IAiTool tool)
        {
            // Simple keyword-based tool detection
            // Override for more sophisticated tool selection logic
            return userMessage.ToLower().Contains(tool.Name.ToLower());
        }

        public void ClearConversationHistory()
        {
            conversationHistory.Clear();
        }

        public string GetConversationSummary()
        {
            if (!conversationHistory.Any())
                return "No conversation history.";

            var summary = $"Conversation with {Name} ({conversationHistory.Count} messages):\n";
            foreach (var message in conversationHistory.TakeLast(5)) // Show last 5 messages
            {
                var role = message.Role == MessageRole.Human ? "User" : "AI";
                var content = message.Content.Length > 100
                    ? message.Content.Substring(0, 97) + "..."
                    : message.Content;
                summary += $"{role}: {content}\n";
            }
            return summary;
        }

        public virtual void Execute()
        {
            // Execution logic for the AI agent
            Console.WriteLine($"Executing {Name} agent...");
            Console.WriteLine($"Description: {Description}");
            Console.WriteLine($"Available tools: {string.Join(", ", tools.Keys)}");
        }

        public string GetToolsDescription()
        {
            if (!tools.Any())
                return "No tools available.";

            var description = "Available tools:\n";
            foreach (var tool in tools.Values)
            {
                description += $"- {tool.Name}: {tool.Description}\n";
            }
            return description;
        }
    }
}

