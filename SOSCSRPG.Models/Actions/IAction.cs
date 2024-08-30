namespace SOSCSRPG.Models.Actions
{
    public interface IAction
    {
        event EventHandler<string> OnActionPerformed;
        void Execute(LivingEntity actor, LivingEntity target);
    }
}
