namespace AiAgent.Tools
{
    /// <summary>
    /// Represents a tool that can be invoked by the chat assistant.
    /// </summary>
    public interface IAiTool
    {
        /// <summary>Unique name of the tool.</summary>
        string Name { get; }

        /// <summary>Description of what the tool does.</summary>
        string Description { get; }

        /// <summary>
        /// Executes the tool logic using the provided input and returns the result.
        /// </summary>
        /// <param name="input">User message or arguments for the tool.</param>
        /// <returns>Result of the tool execution.</returns>
        Task<string> ExecuteAsync(string input);
    }
}

