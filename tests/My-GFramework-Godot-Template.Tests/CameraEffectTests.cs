using GFrameworkTemplate.scripts.component.camera;
using Godot;

namespace GFrameworkTemplate.Tests;

/// <summary>
///     CameraEffect 基类和效果策略测试
/// </summary>
public class CameraEffectTests
{
    [Fact]
    public void WalkBobEffect_HasLowPriority()
    {
        var fx = new WalkBobEffect();
        Assert.Equal(10, fx.Priority);
        Assert.False(fx.IsExpired); // Duration = -1, never expires
    }

    [Fact]
    public void EarthquakeEffect_HasHighPriority()
    {
        var fx = new EarthquakeEffect();
        Assert.Equal(90, fx.Priority);
    }

    [Fact]
    public void EarthquakeEffect_ExpiresAfterDuration()
    {
        var fx = new EarthquakeEffect { Duration = 1.5f, Intensity = 20f };
        Assert.False(fx.IsExpired);
        fx.Elapsed = 1.5f;
        Assert.True(fx.IsExpired);
    }

    [Fact]
    public void CloseUpEffect_HasDefaultZoom()
    {
        var fx = new CloseUpEffect { TargetPosition = Vector2.Zero };
        Assert.Equal(70, fx.Priority);
        Assert.Equal(1.5f, fx.TargetZoom);
    }

    [Fact]
    public void BreatheEffect_NeverExpires()
    {
        var fx = new BreatheEffect();
        fx.Elapsed = 999f;
        Assert.False(fx.IsExpired);
    }

    [Fact]
    public void PanEffect_LinearMovement()
    {
        var fx = new PanEffect { Direction = Vector2.Right, Speed = 100f, Duration = 2f };
        fx.Elapsed = 1f;
        var offset = fx.GetOffset(fx.Progress);
        Assert.Equal(100f, offset.X, 1f);
        Assert.Equal(0f, offset.Y, 1f);
    }

    [Fact]
    public void Progress_AtStart_IsZero()
    {
        var fx = new EarthquakeEffect { Duration = 2f };
        Assert.Equal(0f, fx.Progress);
    }

    [Fact]
    public void Progress_AtHalf_IsHalf()
    {
        var fx = new EarthquakeEffect { Duration = 2f };
        fx.Elapsed = 1f;
        Assert.Equal(0.5f, fx.Progress);
    }

    [Fact]
    public void Progress_AtEnd_IsOne()
    {
        var fx = new EarthquakeEffect { Duration = 2f };
        fx.Elapsed = 3f;
        Assert.Equal(1f, fx.Progress);
    }

    [Fact]
    public void PrioritySorting_HigherOverrides()
    {
        var effects = new List<CameraEffect>
        {
            new WalkBobEffect { Priority = 10 },
            new EarthquakeEffect { Priority = 90 },
            new PanEffect { Priority = 20 }
        };

        effects.Sort((a, b) => b.Priority.CompareTo(a.Priority));

        Assert.Equal(90, effects[0].Priority);
        Assert.Equal(20, effects[1].Priority);
        Assert.Equal(10, effects[2].Priority);
    }

    [Fact]
    public void GetZoom_Default_ReturnsOne()
    {
        var fx = new WalkBobEffect();
        Assert.Equal(1f, fx.GetZoom(0.5f));
    }

    [Fact]
    public void GetRotation_Default_ReturnsZero()
    {
        var fx = new WalkBobEffect();
        Assert.Equal(0f, fx.GetRotation(0.5f));
    }

    [Fact]
    public void CloseUpEffect_Zoom_Increases()
    {
        var fx = new CloseUpEffect { TargetZoom = 2f };
        var zoomMid = fx.GetZoom(0.5f);
        Assert.True(zoomMid > 1f && zoomMid < 2f);
        Assert.Equal(2f, fx.GetZoom(1f));
    }
}
