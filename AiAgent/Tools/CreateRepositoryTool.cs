namespace AiAgent.Tools
{
    using System;
    using System.Threading.Tasks;
    using Octokit;

    /// <summary>
    /// Tool for creating GitHub repositories programmatically.
    /// </summary>
    public class CreateRepositoryTool : IAiTool
    {
        private readonly GitHubClient _client;
        private readonly string _defaultOwner;

        /// <summary>
        /// Initializes a new instance of the CreateRepositoryTool.
        /// </summary>
        /// <param name="githubToken">GitHub personal access token with repo creation permissions.</param>
        /// <param name="defaultOwner">Default owner for repositories (username or organization).</param>
        public CreateRepositoryTool(string githubToken, string defaultOwner = "")
        {
            if (string.IsNullOrWhiteSpace(githubToken))
            {
                throw new ArgumentException("GitHub token cannot be null or empty.", nameof(githubToken));
            }

            _client = new GitHubClient(new ProductHeaderValue("AiAgent-RepoCreator"))
            {
                Credentials = new Credentials(githubToken)
            };
            _defaultOwner = defaultOwner;
        }

        public string Name => "create_repository";

        public string Description => "Creates a new GitHub repository. Input format: 'name:<repo-name>,description:<description>,private:<true|false>'";

        /// <summary>
        /// Creates a new GitHub repository based on the input parameters.
        /// </summary>
        /// <param name="input">Repository creation parameters in format 'name:<repo-name>,description:<description>,private:<true|false>'</param>
        /// <returns>Result message with repository URL or error details.</returns>
        public async Task<string> ExecuteAsync(string input)
        {
            try
            {
                var parameters = ParseInput(input);

                if (!parameters.ContainsKey("name") || string.IsNullOrWhiteSpace(parameters["name"]))
                {
                    return "Error: Repository name is required. Use format: 'name:<repo-name>,description:<description>,private:<true|false>'";
                }

                var newRepo = new NewRepository(parameters["name"])
                {
                    Description = parameters.ContainsKey("description") ? parameters["description"] : "",
                    Private = parameters.ContainsKey("private") && bool.TryParse(parameters["private"], out var isPrivate) ? isPrivate : true,
                    AutoInit = true // Initialize with README
                };

                Repository? repository;

                // If owner is specified in parameters, create for that owner (could be org)
                if (parameters.ContainsKey("owner") && !string.IsNullOrWhiteSpace(parameters["owner"]))
                {
                    repository = await _client.Repository.Create(parameters["owner"], newRepo);
                }
                // Otherwise use default owner if set
                else if (!string.IsNullOrWhiteSpace(_defaultOwner))
                {
                    repository = await _client.Repository.Create(_defaultOwner, newRepo);
                }
                // Otherwise create for authenticated user
                else
                {
                    repository = await _client.Repository.Create(newRepo);
                }

                return $"Successfully created repository: {repository.HtmlUrl}\n" +
                       $"Name: {repository.Name}\n" +
                       $"Owner: {repository.Owner.Login}\n" +
                       $"Private: {repository.Private}\n" +
                       $"Description: {repository.Description ?? "None"}";
            }
            catch (Exception ex)
            {
                return $"Error creating repository: {ex.Message}";
            }
        }

        private Dictionary<string, string> ParseInput(string input)
        {
            var parameters = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            // Split by comma to get key-value pairs
            var pairs = input.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in pairs)
            {
                var keyValue = pair.Split(new[] { ':' }, 2);
                if (keyValue.Length == 2)
                {
                    parameters[keyValue[0].Trim()] = keyValue[1].Trim();
                }
            }

            return parameters;
        }
    }
}
