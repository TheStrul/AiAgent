namespace AiAgent
{
    using LangChain.Chains.StackableChains.Agents.Tools;
    using LangChain.Providers;
    using LangChain.Schema;

    public class CustomTool
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Func<string, Task<string>> Function { get; set; } = _ => Task.FromResult("No function defined");
    }

    public class AiAgentBase
    {
        private readonly Dictionary<string, CustomTool> tools = new Dictionary<string, CustomTool>();
        private readonly List<Message> conversationHistory = new List<Message>();
        private IChatModel? chatModel;

        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string Author { get; set; }
        
        public IReadOnlyDictionary<string, CustomTool> Tools => tools;
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

        public void AddTool(string name, CustomTool tool)
        {
            tools[name] = tool;
        }

        public void AddTool(string name, string description, Func<string, Task<string>> function)
        {
            var tool = new CustomTool
            {
                Name = name,
                Description = description,
                Function = function
            };
            AddTool(name, tool);
        }

        public bool RemoveTool(string name)
        {
            return tools.Remove(name);
        }

        public CustomTool? GetTool(string name)
        {
            return tools.TryGetValue(name, out var tool) ? tool : null;
        }

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
                var responseMessage = lastMessage?.Content ?? "No response generated.";

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
                        var result = await tool.Function(userMessage);
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

        protected virtual bool ShouldUseTool(string userMessage, CustomTool tool)
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
