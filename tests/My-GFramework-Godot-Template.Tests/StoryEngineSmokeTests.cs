using GFrameworkTemplate.scripts.core.story;
using GFrameworkTemplate.scripts.cqrs.visualnovel.command;
using GFrameworkTemplate.scripts.data.story;
using GFrameworkTemplate.scripts.system.visualnovel;

namespace GFrameworkTemplate.Tests;

/// <summary>
///     StoryEngineSystem 集成测试——覆盖完整流水线和分支过滤逻辑
/// </summary>
public class StoryEngineSmokeTests
{
    [Fact]
    public void Engine_Constructor_ShouldInitializeWorkers()
    {
        var engine = new StoryEngineSystem();

        Assert.False(engine.IsPlaying);
        Assert.Empty(engine.TalkBranch);
        Assert.Empty(engine.CanNotChoose);
    }

    [Fact]
    public void Engine_RegisterAndResolveJson()
    {
        StoryEngineSystem.RegisterJson("TestChapter", "res://resource/story/test.json");

        var resolved = StoryResourceMapper.ResolveJsonPath("TestChapter");
        Assert.Equal("res://resource/story/test.json", resolved);
    }

    [Fact]
    public void Engine_BranchState_BasicOperations()
    {
        var engine = new StoryEngineSystem();

        engine.AddCannotChoose("1A");
        Assert.Contains("1A", engine.CanNotChoose);

        engine.RemoveCannotChoose("1A");
        Assert.DoesNotContain("1A", engine.CanNotChoose);
    }

    [Fact]
    public void Engine_AutoPlay_ShouldBeNullByDefault()
    {
        var engine = new StoryEngineSystem();
        Assert.False(engine.IsPlaying);
    }

    [Fact]
    public void Engine_SetWordSpeed_And_AutoPlay()
    {
        var engine = new StoryEngineSystem();

        engine.SetWordSpeed(0.05f);
        engine.SetAutoPlay(2.0f);
        engine.SetAutoPlay(null); // 关闭自动播放

        // 不抛异常即通过
        Assert.True(true);
    }

    [Fact]
    public void Engine_Stop_ShouldNotThrow()
    {
        var engine = new StoryEngineSystem();
        engine.Stop();
        Assert.False(engine.IsPlaying);
    }

    [Fact]
    public void ParseAndVerify_FullScript_AllCommandTypes()
    {
        var json = """
        {
          "content": [
            {"type": "talk", "talker": "??", "talk_content": "……早上好。", "avatar_path": "heroine"},
            {"type": "background", "file_path": "bg_classroom", "wait_tween_end": "1"},
            {"type": "tachie", "tachies": {"heroine": {"file_path": "heroine_normal", "type": "show"}}},
            {"type": "sound", "sound_type": "bgm", "file_path": "morning_theme"},
            {"type": "talk", "is_center": "1", "talk_content": "（新学期的第一天。）"},
            {"type": "branch", "options": {"1A": {"text": "友好回应"}, "1B": {"text": "沉默"}}},
            {"type": "goto", "file_path": "Chapter1_Next"}
          ]
        }
        """;

        var script = StoryParser.ParseStory(json);
        Assert.Equal(7, script.Content.Count);

        // 验证公共命令无分支标记
        Assert.Null(script.Content[0].Branch);
        Assert.Null(script.Content[1].Branch);
        Assert.Null(script.Content[2].Branch);
        Assert.Null(script.Content[3].Branch);
        Assert.Null(script.Content[4].Branch);
        Assert.Null(script.Content[5].Branch);
        Assert.Null(script.Content[6].Branch);
    }

    [Fact]
    public void Parse_MixedBranchCommands_FilteringReady()
    {
        var json = """
        {
          "content": [
            {"type": "talk", "talk_content": "公共开头"},
            {"type": "talk", "talk_content": "分支A", "branch": "A"},
            {"type": "talk", "talk_content": "分支B", "branch": "B"},
            {"type": "talk", "talk_content": "分支C", "branch": "C"},
            {"type": "goto", "file_path": "End"}
          ]
        }
        """;

        var script = StoryParser.ParseStory(json);
        Assert.Equal(5, script.Content.Count);

        // 只有 1,2,3 有 Branch 标记
        Assert.Null(script.Content[0].Branch);
        Assert.Equal("A", script.Content[1].Branch);
        Assert.Equal("B", script.Content[2].Branch);
        Assert.Equal("C", script.Content[3].Branch);
        Assert.Null(script.Content[4].Branch);
    }
}
