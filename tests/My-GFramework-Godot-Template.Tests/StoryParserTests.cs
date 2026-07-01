using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;
using GFrameworkTemplate.scripts.enums.visualnovel;

namespace GFrameworkTemplate.Tests;

/// <summary>
///     StoryParser 完整流水线测试——验证 7 种命令类型的解析和分支过滤
/// </summary>
public class StoryParserTests
{
    [Fact]
    public void Parse_AllSevenTypes_ShouldSucceed()
    {
        var json = """
        {
          "content": [
            {"type": "talk", "talker": "小夜", "talk_content": "你好", "avatar_path": "heroine"},
            {"type": "background", "file_path": "bg_classroom", "wait_tween_end": "1", "delay": "0.5"},
            {"type": "tachie", "tachies": {"nunu": {"file_path": "nunu_normal", "type": "show"}}},
            {"type": "sound", "sound_type": "bgm", "file_path": "morning_theme"},
            {"type": "branch", "options": {"1A": {"text": "选项A"}, "1B": {"text": "选项B", "wait": "1.5"}}},
            {"type": "event", "event_name": "bell_ring"},
            {"type": "goto", "file_path": "Chapter2"}
          ]
        }
        """;

        var script = StoryParser.ParseStory(json);

        Assert.Equal(7, script.Content.Count);
        Assert.IsType<TalkCommand>(script.Content[0]);
        Assert.IsType<BackgroundCommand>(script.Content[1]);
        Assert.IsType<TachieCommand>(script.Content[2]);
        Assert.IsType<SoundCommand>(script.Content[3]);
        Assert.IsType<BranchCommand>(script.Content[4]);
        Assert.IsType<EventCommand>(script.Content[5]);
        Assert.IsType<GotoCommand>(script.Content[6]);
    }

    [Fact]
    public void Parse_TalkCommand_ShouldExtractAllFields()
    {
        var json = """{"content": [{"type": "talk", "talker": "主角", "talk_content": "今天天气真好。", "is_center": "0", "avatar_path": "hero", "branch": "1A", "wait": "0.5"}]}""";

        var script = StoryParser.ParseStory(json);
        var cmd = Assert.IsType<TalkCommand>(script.Content[0]);

        Assert.Equal("主角", cmd.Talker);
        Assert.Equal("今天天气真好。", cmd.TalkContent);
        Assert.False(cmd.IsCenter);
        Assert.Equal("hero", cmd.AvatarPath);
        Assert.Equal("1A", cmd.Branch);
        Assert.Equal(0.5f, cmd.Wait);
    }

    [Fact]
    public void Parse_CenterTalk_ShouldBeNarration()
    {
        var json = """{"content": [{"type": "talk", "is_center": "1", "talk_content": "（窗外传来雨声。）"}]}""";

        var script = StoryParser.ParseStory(json);
        var cmd = Assert.IsType<TalkCommand>(script.Content[0]);

        Assert.True(cmd.IsCenter);
        Assert.Null(cmd.Talker);
        Assert.Equal("（窗外传来雨声。）", cmd.TalkContent);
    }

    [Fact]
    public void Parse_TachieShowAndChange()
    {
        var json = """{"content": [{"type": "tachie", "tachies": {"nunu": {"file_path": "nunu_smile", "type": "change"}, "lili": {"file_path": "lili_normal", "type": "show"}}}]}""";

        var script = StoryParser.ParseStory(json);
        var cmd = Assert.IsType<TachieCommand>(script.Content[0]);

        Assert.Equal(2, cmd.Tachies.Count);
        Assert.Equal("nunu_smile", cmd.Tachies["nunu"].FilePath);
        Assert.Equal(TachieOperation.Change, cmd.Tachies["nunu"].Type);
        Assert.Equal("lili_normal", cmd.Tachies["lili"].FilePath);
        Assert.Equal(TachieOperation.Show, cmd.Tachies["lili"].Type);
    }

    [Fact]
    public void Parse_BranchWithOptions()
    {
        var json = """{"content": [{"type": "branch", "options": {"1A": {"text": "友好回应"}, "1B": {"text": "保持沉默", "wait": "2.0"}}}]}""";

        var script = StoryParser.ParseStory(json);
        var cmd = Assert.IsType<BranchCommand>(script.Content[0]);

        Assert.Equal(2, cmd.Options.Count);
        Assert.Equal("友好回应", cmd.Options["1A"].Text);
        Assert.Null(cmd.Options["1A"].Wait);
        Assert.Equal("保持沉默", cmd.Options["1B"].Text);
        Assert.Equal(2.0f, cmd.Options["1B"].Wait);
    }

    [Fact]
    public void Parse_EmptyContent_ShouldReturnEmpty()
    {
        var json = """{"content": []}""";

        var script = StoryParser.ParseStory(json);

        Assert.Empty(script.Content);
    }

    [Fact]
    public void Parse_UnknownType_ShouldSkip()
    {
        var json = """{"content": [{"type": "talk", "talk_content": "有效"}, {"type": "unknown_type"}, {"type": "goto", "file_path": "Next"}]}""";

        var script = StoryParser.ParseStory(json);

        Assert.Equal(2, script.Content.Count);
        Assert.IsType<TalkCommand>(script.Content[0]);
        Assert.IsType<GotoCommand>(script.Content[1]);
    }

    [Fact]
    public void Parse_BranchFiltering_OnlyPublicCommands()
    {
        var json = """
        {
          "content": [
            {"type": "talk", "talk_content": "公共对话"},
            {"type": "talk", "talk_content": "分支1A对话", "branch": "1A"},
            {"type": "talk", "talk_content": "分支1B对话", "branch": "1B"},
            {"type": "talk", "talk_content": "结尾对话"}
          ]
        }
        """;

        var script = StoryParser.ParseStory(json);

        // 无 branch 字段的公共命令
        Assert.Null(script.Content[0].Branch);
        Assert.Null(script.Content[3].Branch);
        // 有 branch 字段的分支命令
        Assert.Equal("1A", script.Content[1].Branch);
        Assert.Equal("1B", script.Content[2].Branch);
    }

    [Fact]
    public void Parse_CommonFields_ShouldSetOnAllTypes()
    {
        var json = """{"content": [{"type": "goto", "file_path": "NextFile", "wait": "1.0", "hide_labels": "1", "branch": "2A"}]}""";

        var script = StoryParser.ParseStory(json);
        var cmd = Assert.IsType<GotoCommand>(script.Content[0]);

        Assert.Equal("NextFile", cmd.FilePath);
        Assert.Equal(1.0f, cmd.Wait);
        Assert.True(cmd.HideLabels);
        Assert.Equal("2A", cmd.Branch);
    }

    [Fact]
    public void ParseSound_DefaultsToOneSound()
    {
        var json = """{"content": [{"type": "sound", "file_path": "explosion"}]}""";

        var script = StoryParser.ParseStory(json);
        var cmd = Assert.IsType<SoundCommand>(script.Content[0]);

        Assert.Equal("oneSound", cmd.SoundType);
        Assert.Equal("explosion", cmd.FilePath);
    }
}
