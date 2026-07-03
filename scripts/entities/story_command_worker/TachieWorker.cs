using GFrameworkTemplate.scripts.cqrs.visualnovel.command;

namespace GFrameworkTemplate.scripts.entities.story_command_worker;

public sealed class TachieWorker : FireAndForgetWorker<TachieCommand> { }
