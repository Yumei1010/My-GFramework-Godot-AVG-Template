using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;

namespace GFrameworkTemplate.Tests;

/// <summary>
///     StoryParser 高级测试——边缘情形、链式解析、字段继承
/// </summary>
public class StoryParserAdvancedTests
{
    [Fact]
    public void Parse_BranchFiltering_SkipsUnmatchedBranches()
    {
        var json = """
        {
          "content": [
            {"type": "talk", "talk_content": "公共", "talker": "A"},
            {"type": "talk", "talk_content": "分支A", "branch": "A"},
            {"type": "talk", "talk_content": "分支B", "branch": "B"},
            {"type": "talk", "talk_content": "结尾"}
          ]
        }
        """;

        var script = StoryParser.ParseStory(json);

        // 公共命令 = null branch
        Assert.Null(script.Content[0].Branch);
        Assert.Null(script.Content[3].Branch);
        // 分支命令
        Assert.Equal("A", script.Content[1].Branch);
        Assert.Equal("B", script.Content[2].Branch);
    }

    [Fact]
    public void Parse_GotoChaining_PreservesTargetPath()
    {
        var json = """{"content": [{"type": "goto", "file_path": "NextChapter"}]}""";
        var script = StoryParser.ParseStory(json);
        var cmd = Assert.IsType<GotoCommand>(script.Content[0]);
        Assert.Equal("NextChapter", cmd.FilePath);
    }

    [Fact]
    public void Parse_WaitField_AllTypes()
    {
        var json = """{"content": [{"type": "talk", "talk_content": "a", "wait": "0.5"}]}""";
        var script = StoryParser.ParseStory(json);
        Assert.Equal(0.5f, script.Content[0].Wait);
    }

    [Fact]
    public void Parse_HideLabelsField()
    {
        var json = """{"content": [{"type": "talk", "talk_content": "x", "hide_labels": "1"}]}""";
        var script = StoryParser.ParseStory(json);
        Assert.True(script.Content[0].HideLabels);
    }

    [Fact]
    public void Parse_MultipleCommands_KeepsOrder()
    {
        var json = """
        {
          "content": [
            {"type": "talk", "talk_content": "1"},
            {"type": "talk", "talk_content": "2"},
            {"type": "background", "file_path": "bg"},
            {"type": "talk", "talk_content": "3"},
            {"type": "goto", "file_path": "End"}
          ]
        }
        """;

        var script = StoryParser.ParseStory(json);
        Assert.Equal(5, script.Content.Count);
        Assert.IsType<TalkCommand>(script.Content[0]);
        Assert.IsType<TalkCommand>(script.Content[1]);
        Assert.IsType<BackgroundCommand>(script.Content[2]);
        Assert.IsType<TalkCommand>(script.Content[3]);
        Assert.IsType<GotoCommand>(script.Content[4]);
    }

    [Fact]
    public void Parse_BackgroundWithDelay()
    {
        var json = """{"content": [{"type": "background", "file_path": "bg_room", "wait_tween_end": "1", "delay": "0.8"}]}""";
        var script = StoryParser.ParseStory(json);
        var cmd = Assert.IsType<BackgroundCommand>(script.Content[0]);
        Assert.Equal(0.8f, cmd.Delay);
        Assert.True(cmd.WaitTweenEnd);
    }

    [Fact]
    public void Parse_BranchOptions_WithWait()
    {
        var json = """{"content": [{"type": "branch", "options": {"1A": {"text": "A", "wait": "1.5"}, "1B": {"text": "B"}}}]}""";
        var script = StoryParser.ParseStory(json);
        var cmd = Assert.IsType<BranchCommand>(script.Content[0]);
        Assert.Equal(1.5f, cmd.Options["1A"].Wait);
        Assert.Null(cmd.Options["1B"].Wait);
    }

    [Fact]
    public void Parse_EventCommand()
    {
        var json = """{"content": [{"type": "event", "event_name": "chapter_end"}]}""";
        var script = StoryParser.ParseStory(json);
        var cmd = Assert.IsType<EventCommand>(script.Content[0]);
        Assert.Equal("chapter_end", cmd.EventName);
    }

    [Fact]
    public void ParseCommand_SingleCommand_Works()
    {
        var cmd = StoryParser.ParseCommand(
            System.Text.Json.JsonDocument.Parse("""{"type": "talk", "talk_content": "hi"}""").RootElement);

        Assert.NotNull(cmd);
        var talk = Assert.IsType<TalkCommand>(cmd);
        Assert.Equal("hi", talk.TalkContent);
    }

    [Fact]
    public void ParseCommand_NullOnUnknownType()
    {
        var cmd = StoryParser.ParseCommand(
            System.Text.Json.JsonDocument.Parse("""{"type": "unknown_type"}""").RootElement);

        Assert.Null(cmd);
    }
}
