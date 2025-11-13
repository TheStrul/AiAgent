using AiAgent;
using AiAgent.Tools;

Console.WriteLine("=== AiAgent GitHub Repository Creation Example ===\n");

// This example demonstrates how to use the CreateRepositoryTool
// To run this example, you need:
// 1. A GitHub Personal Access Token with 'repo' scope
// 2. An OpenAI API key

// Get credentials from environment variables
var githubToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
var openAiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

if (string.IsNullOrEmpty(githubToken))
{
    Console.WriteLine("ERROR: GITHUB_TOKEN environment variable not set.");
    Console.WriteLine("Please set your GitHub Personal Access Token:");
    Console.WriteLine("  export GITHUB_TOKEN=your_github_token");
    return;
}

if (string.IsNullOrEmpty(openAiKey))
{
    Console.WriteLine("ERROR: OPENAI_API_KEY environment variable not set.");
    Console.WriteLine("Please set your OpenAI API key:");
    Console.WriteLine("  export OPENAI_API_KEY=your_openai_key");
    return;
}

try
{
    // Create the assistant and register the repository creation tool
    var assistant = new ChatAssistant();
    var repoTool = new CreateRepositoryTool(githubToken);
    assistant.RegisterTool(repoTool);
    assistant.Init(openAiKey);

    Console.WriteLine("Assistant initialized with CreateRepositoryTool.\n");

    // Example 1: Create a private repository
    Console.WriteLine("Example 1: Creating a private test repository...");
    var response1 = await assistant.ChatAsync(
        "create_repository: name:test-private-repo-" + DateTime.Now.Ticks + 
        ",description:A test private repository created by AiAgent,private:true"
    );
    Console.WriteLine($"Response: {response1}\n");

    // Example 2: Create a public repository
    Console.WriteLine("Example 2: Creating a public test repository...");
    var response2 = await assistant.ChatAsync(
        "create_repository: name:test-public-repo-" + DateTime.Now.Ticks + 
        ",description:A test public repository created by AiAgent,private:false"
    );
    Console.WriteLine($"Response: {response2}\n");

    Console.WriteLine("Examples completed successfully!");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
}
