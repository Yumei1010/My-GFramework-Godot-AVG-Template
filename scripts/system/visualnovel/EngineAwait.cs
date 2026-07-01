namespace GFrameworkTemplate.scripts.system.visualnovel;

public static class EngineAwait
{
    public static async Task Advance(float minDuration, EngineContext ctx)
    {
        if (ctx.AutoPlayDelay.HasValue)
        {
            var waitTime = Math.Max(minDuration, ctx.AutoPlayDelay.Value);
            await Task.Delay(TimeSpan.FromSeconds(waitTime));
        }
        else
        {
            ctx.WaitSource = new TaskCompletionSource<bool>();
            await ctx.WaitSource.Task;
            ctx.WaitSource = null;
        }
    }
}
