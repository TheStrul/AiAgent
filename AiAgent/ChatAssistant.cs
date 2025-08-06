namespace AiAgent
{
    using AiAgent.Tools;
    using LangChain.Providers.OpenAI;

    /// <summary>
    /// High level chat assistant that hides the underlying chat model and tool wiring.
    /// </summary>
    public class ChatAssistant : AiAgentBase
    {
        public ChatAssistant(
            string name = "Assistant",
            string description = "General purpose assistant",
            string version = "1.0",
            string author = "unknown")
            : base(name, description, version, author)
        {
        }

        /// <summary>
        /// Initializes the assistant with the OpenAI chat model.
        /// </summary>
        public void Init(string apiKey, string modelId = "gpt-4o-mini")
        {
            var model = new OpenAiChatModel(modelId, apiKey);
            Initialize(model);
        }

        /// <summary>
        /// Registers a single tool with the assistant.
        /// </summary>
        public void RegisterTool(IAiTool tool) => AddTool(tool);

        /// <summary>
        /// Registers multiple tools at once.
        /// </summary>
        public void RegisterTools(IEnumerable<IAiTool> toolSet)
        {
            foreach (var tool in toolSet)
            {
                RegisterTool(tool);
            }
        }
    }
}

