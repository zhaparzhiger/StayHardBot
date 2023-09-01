using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StayHardBot.Slash_Commands
{
    public class FunSl : ApplicationCommandModule
    {
        [SlashCommand("test","This is our first slash command")]
        public async Task TestSlashCommand(InteractionContext ctx, [Choice("Pre-Defined Text","Hello nigga")]
                                                                    [Option("string","Type in anything you want")] string text)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                .WithContent("starting slash command"));
            var embedMessage = new DiscordEmbedBuilder()
            {
                Title = text,
            };
            await ctx.Channel.SendMessageAsync(embed:embedMessage);
        }
        [SlashCommand("poll","Create your own poll!")]
        public async Task PollCommand(InteractionContext ctx, [Option("question","The main poll subject/question")] string Question,
                                                              [Option("timelimit","The time set on this poll")] long TimeLimit,
                                                              [Option("option1","Option 1")] string Option1,
                                                              [Option("option2", "Option 2")] string Option2,
                                                              [Option("option3", "Option 3")] string Option3,
                                                              [Option("option4", "Option 4")] string Option4)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
            .WithContent("starting slash command"));
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
            catch (Exception ex)
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

        [SlashCommand("caption","Give any image a caption")]
        public async Task CaptionCommand(InteractionContext ctx, [Option("Caption","The caption you want the image to have")] string caption,
                                                                 [Option("image","the image you want to upload")] DiscordAttachment picture)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
            .WithContent("starting slash command"));

            var captionMessage = new DiscordMessageBuilder().
                AddEmbed(new DiscordEmbedBuilder()
                .WithFooter(caption)
                .WithColor(DiscordColor.Azure)
                .WithImageUrl(picture.Url));
            await ctx.Channel.SendMessageAsync(captionMessage);
        }

        [SlashCommand("ban","bans a user")]
        [RequireRoles(RoleCheckMode.MatchNames, "Administrator")]
        public async Task Ban(InteractionContext ctx, [Option("user","User to ban")] DiscordUser user,
                                                      [Choice("None",0)]
                                                      [Choice("1 day",1)]
                                                      [Choice("1 week",0)]
                                                      [Option("deletedays", "Number of days of message history to delete")] long deleteDays = 0)
        {
            await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
           .WithContent("starting slash command"));
            await ctx.Guild.BanMemberAsync(user.Id,(int)deleteDays);
        }
    }
}
