using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using StayHardBot.commands;
using StayHardBot.config;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using StayHardBot.Slash_Commands;

namespace StayHardBot
{
        internal class Program
    {
        
        private static DiscordClient Client { get; set; }
        private static CommandsNextExtension Commands { get; set; }
        private static InteractivityExtension Interactivity { get; set; }


        static async Task Main(string[] args)
        {
         
    

            var jsonReader = new JSONreader();
            await jsonReader.ReadJSON();

            var discordConfig = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = jsonReader.token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };
            Client = new DiscordClient(discordConfig);

            Client.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(2)
            });

            Client.Ready += OnClientReady;
            Client.ComponentInteractionCreated += ButtonPressResponse;
            //6. Set up the Commands Configuration
            var commandsConfig = new CommandsNextConfiguration()
            {
                StringPrefixes = new string[] { jsonReader.prefix },
                EnableMentionPrefix = true,
                EnableDms = true,
                EnableDefaultHelp = false,
            };
                      Commands = Client.UseCommandsNext(commandsConfig);

            var slashCommandsConfig = Client.UseSlashCommands();


            Commands.RegisterCommands<TestCommands>();

            // SLASH COMMANDS
            slashCommandsConfig.RegisterCommands<FunSl>(1144276720418050078);

            Commands.CommandErrored += OnCommandError;


            await Client.ConnectAsync();
            await Task.Delay(-1);
        }

        private static async Task ButtonPressResponse(DiscordClient sender, ComponentInteractionCreateEventArgs e)
        {
            if (e.Interaction.Data.CustomId == "1")
            {
                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,new DiscordInteractionResponseBuilder().WithContent("You pressed the first button!"));
            }
            else if(e.Interaction.Data.CustomId == "2")
            {
                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("You pressed the second button!"));
            }
            else if (e.Interaction.Data.CustomId == "funButton")
            {
                string funCommandsList = "!add -> Adds two numbers \n" +
                                         "!embed -> Sends an embed message with Test Role \n" +
                                         "!poll -> Starts a poll";
                await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder().WithContent(funCommandsList));

            }
            else if (e.Interaction.Data.CustomId == "gameButton")                                                                                     
            {
                string gameList = "!cardgame -> Play a simple card game.Whoever draws the highest wins the game";
                var gamesCommandList = new DiscordInteractionResponseBuilder()
                {
                    Title = "Game Command List",
                    Content = gameList
                };
                await e.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,gamesCommandList);
            }
        }


        private static async Task OnCommandError(CommandsNextExtension sender, CommandErrorEventArgs e)
        {
            if (e.Exception is ChecksFailedException)
            {
                var castedException = (ChecksFailedException)e.Exception;
                string cooldownTimer = string.Empty;

                foreach (var check in castedException.FailedChecks)
                {
                    var coolDown = (CooldownAttribute)check;
                    TimeSpan timeLeft = coolDown.GetRemainingCooldown(e.Context);
                    cooldownTimer = timeLeft.ToString(@"hh\:mm\:ss");
                }
                var cooldownMessage = new DiscordEmbedBuilder()
                {
                    Title = "Wait for the cooldown to end",
                    Description = "Remaining time: "  + cooldownTimer,
                    Color = DiscordColor.Red,
                };
                await e.Context.Channel.SendMessageAsync(embed:cooldownMessage);
            }
        }

        private static Task OnClientReady(DiscordClient sender, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
