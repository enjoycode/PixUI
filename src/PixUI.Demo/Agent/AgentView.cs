using System;
using System.Collections.Generic;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OllamaSharp;
using PixUI.AIChatBox;
using PixUI.Demo.Agent.Tools;

namespace PixUI.Demo.Agent;

internal sealed class AgentView : View
{
    public AgentView()
    {
        _chatController = new ChatBoxController();
        _chatController.OnSendMessage = OnSendMessage;

        Child = new ChatBox(_chatController);

        BuildAgent();
    }

    private readonly ChatBoxController _chatController;
    private AIAgent _agent = null!;
    private AgentSession? _session = null;

    private const string INSTRUCTIONS = """
                                        你是C#和PixUI专家
                                        """;

    private void BuildAgent()
    {
        IChatClient ollamaClient = new OllamaApiClient(AIOptions.Url, AIOptions.Model);
        // var loggingClient = new ChatClientBuilder(ollamaClient)
        //     .UseLogging(loggerFactory)
        //     //.UseAIContextProviders()
        //     .Build();

        var clientOptions = new ChatClientAgentOptions()
        {
            Name = "MyFirstAgent",
            ChatOptions = new ChatOptions()
            {
                Instructions = INSTRUCTIONS,
                Tools = BuildTools(),
                Temperature = 1,
                TopP = 0.95f,
                TopK = 64,
                AllowMultipleToolCalls = false,
                // ResponseFormat = ChatResponseFormat.Json,
                // Reasoning = new ReasoningOptions() { Effort = ReasoningEffort.Medium, Output = ReasoningOutput.Full },
                // AdditionalProperties = new AdditionalPropertiesDictionary(new Dictionary<string, object?>
                // {
                //     { "think", true }
                // })
            },
            ChatHistoryProvider = new InMemoryChatHistoryProvider(), // 默认
        };
        _agent = new ChatClientAgent(ollamaClient, clientOptions);
    }

    private static IList<AITool> BuildTools() =>
    [
        AIFunctionFactory.Create(CodeTool.ReadCurrentDemoCode)
    ];

    private async void OnSendMessage(string message)
    {
        _session ??= await _agent.CreateSessionAsync();

        await foreach (var update in _agent.RunStreamingAsync(message, _session))
        {
            foreach (var content in update.Contents)
            {
                if (content is TextContent textContent)
                {
                    _chatController.AppendAIContent(textContent.Text);
                }
                else if (content is TextReasoningContent reasoningContent)
                {
                    _chatController.AppendAIContent(reasoningContent.Text, true);
                }
                else if (content is UsageContent usageContent)
                {
                    //TODO:
                }
                else if (content is FunctionCallContent functionCallContent)
                {
                    _chatController.AppendAIFunctionCall(functionCallContent.Name, functionCallContent.Arguments);
                }
                else if (content is FunctionResultContent functionResultContent)
                {
                    
                }
                else
                {
                    Console.WriteLine(content.ToString());
                }
            }
        }
    }
}