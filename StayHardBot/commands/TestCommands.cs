using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using StayHardBot.other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayHardBot.commands
{
   public class TestCommands : BaseCommandModule
    {
        [Command("test")]
        [Cooldown(5,10,CooldownBucketType.User)]
        public async Task MyFirstCommand(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"Hello {ctx.User.Username}");
        }
        [Command("add")]
        public async Task Add(CommandContext ctx,int number1 ,int number2 )
        {
            int result = number1 + number2;
            await ctx.Channel.SendMessageAsync(result.ToString());
        }

        [Command("subtract")]
        public async Task Subtract(CommandContext ctx, int number1, int number2)
        {
            int result = number1 - number2;
            await ctx.Channel.SendMessageAsync(result.ToString());
        }
        [Command("count")]
        public async Task Count(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"{ctx.Channel.Users.Count}");
        }
        [Command("field")]
        public async Task Fields(CommandContext ctx)
        {
            var fieldEmbet = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .AddField("Field1","this is a field",false)
                .AddField("Field1", "this is a field", false)
                .AddField("Field1", "this is a field", false)
                .AddField("Field1", "this is a field", false));
            await ctx.Channel.SendMessageAsync(fieldEmbet);
        }
        [Command("embed")]
        [RequireRoles(RoleCheckMode.MatchNames,"Test Role")]
        public async Task Embed(CommandContext ctx)
        {
            if (ctx.Channel.Id == 1145976281096261652)
            {
                var message = new DiscordEmbedBuilder()
                {
                    Title = "Wassup bitch!",
                    Description = "My first embet!",
                    Color = DiscordColor.Azure

                };
                await ctx.Channel.SendMessageAsync(embed: message);
            }
            else
            {
                await ctx.Channel.SendMessageAsync("That command cannot be executed here");
            }
        }
        [Command("cardgame")]
        public async Task CardGame(CommandContext ctx)
        {
            var userCard = new CardSystem();
            var userCardEmbet = new DiscordEmbedBuilder()
            {
                Title = $"Your card is {userCard.SelectedCard}",
                Color = DiscordColor.Cyan,
            };
            await ctx.Channel.SendMessageAsync(embed:userCardEmbet);
            var botCard = new CardSystem();
            var botCardEmbet = new DiscordEmbedBuilder()
            {
                Title = $"The bot drew a card {botCard.SelectedCard}",
                Color = DiscordColor.Orange,
            };
            await ctx.Channel.SendMessageAsync(embed: botCardEmbet);

            if(userCard.SelectedNumber > botCard.SelectedNumber)
            {
                //user wins
                var winMessage = new DiscordEmbedBuilder()
                {
                    Title = "Congratulations, you win!!",
                    Color = DiscordColor.Green,
                };
                await ctx.Channel.SendMessageAsync(embed:winMessage);
            }
            else
            {
                // bot wins
                var loseMessage = new DiscordEmbedBuilder()
                {
                    Title = "Pupsik you lost the game!!! ",
                    Color = DiscordColor.Red,
                };
                await ctx.Channel.SendMessageAsync(embed: loseMessage);
            }
        }

        [Command("poll")]
        public async Task PollCommand(CommandContext ctx,int TimeLimit,string Option1, string Option2, string Option3, string Option4,params string[] Question)
        {
            try
            {


                var interactivity = ctx.Client.GetInteractivity();
                TimeSpan timer = TimeSpan.FromSeconds(TimeLimit);
                DiscordEmoji[] optionEmojies = { DiscordEmoji.FromName(ctx.Client, ":man_in_manual_wheelchair:",false),
                                             DiscordEmoji.FromName(ctx.Client, ":pregnant_man:",false),
                                             DiscordEmoji.FromName(ctx.Client, ":rainbow:",false),
                                             DiscordEmoji.FromName(ctx.Client, ":banana:",false)
                                            };

                string optionsString = optionEmojies[0] + "| " + Option1 + "\n" +
                                       optionEmojies[1] + "| " + Option2 + "\n" +
                                       optionEmojies[2] + "| " + Option3 + "\n" +
                                       optionEmojies[3] + "| " + Option4 + "\n";

                var pollMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .WithTitle(string.Join(" ", Question))
                .WithColor(DiscordColor.Azure)
                .WithDescription(optionsString)
                );
                var putReactOn = await ctx.Channel.SendMessageAsync(pollMessage);
                foreach (var emoji in optionEmojies)
                {
                    await putReactOn.CreateReactionAsync(emoji);
                }
                var result = await interactivity.CollectReactionsAsync(putReactOn, timer);

                int count1 = 0;
                int count2 = 0;
                int count3 = 0;
                int count4 = 0;

                foreach (var emoji in result)
                {
                    if (emoji.Emoji == optionEmojies[0])
                    {
                        count1++;
                    }
                    if (emoji.Emoji == optionEmojies[1])
                    {
                        count2++;
                    }
                    if (emoji.Emoji == optionEmojies[2])
                    {
                        count3++;
                    }
                    if (emoji.Emoji == optionEmojies[3])
                    {
                        count4++;
                    }
                }

                int totalVotes = count1 + count2 + count3 + count4;
                string resultString = optionEmojies[0] + " : " + count1 + " Votes \n" +
                                  optionEmojies[1] + " : " + count2 + " Votes \n" +
                                  optionEmojies[2] + " : " + count3 + " Votes \n" +
                                  optionEmojies[3] + " : " + count4 + " Votes \n\n" +
                                  "The total number of votes is " + totalVotes;

                var resultMessage = new DiscordEmbedBuilder()
                {
                    Color = DiscordColor.Green,
                    Title = "Results of poll",
                    Description = resultString
                };
                await ctx.Channel.SendMessageAsync(resultMessage);
            }
            catch(Exception ex)
            {
                var errorMsg = new DiscordEmbedBuilder()
                {
                    Title = "Something Went Wrong!!!",
                    Description = ex.Message,
                    Color = DiscordColor.Red,
                };
                await ctx.Channel.SendMessageAsync(embed: errorMsg);
            }
          
        }

        [Command("button")]
        public async Task ButtonExample(CommandContext ctx)
        {
            DiscordButtonComponent button1 = new DiscordButtonComponent(ButtonStyle.Primary,"1","Button 1");
            DiscordButtonComponent button2 = new DiscordButtonComponent(ButtonStyle.Primary, "2", "Button 2");
            var message = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Cyan)
                .WithTitle("This is a message with buttons")
                .WithDescription("Please,pick a button"))
                .AddComponents(button1)
                .AddComponents(button2);

            await ctx.Channel.SendMessageAsync(message);
        }

        [Command("help")]
        public async Task HelpCommand(CommandContext ctx)
        {
            var funButton = new DiscordButtonComponent(ButtonStyle.Success, "funButton", "Fun");
            var gameButton = new DiscordButtonComponent(ButtonStyle.Success, "gameButton", "Games");
            var helpMessage = new DiscordMessageBuilder()
                .AddEmbed(new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Azure)
                .WithTitle("Help menu")
                .WithDescription("Please pick a button for more information on the commands"))
                .AddComponents(funButton)
                .AddComponents(gameButton);
            await ctx.Channel.SendMessageAsync(helpMessage);
        }
    }
}
