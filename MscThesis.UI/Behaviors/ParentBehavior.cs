using MscThesis.Runner.Factories.Expression;
using MscThesis.UI.ViewModels;

namespace MscThesis.UI.Behaviors
{
    public class ParentBehavior : Behavior<View>
    {
        public static SetupVM VM { get; set; }

        protected override void OnAttachedTo(View view)
        {
            view.DescendantAdded += ChildAdded;
            view.DescendantRemoved += ChildRemoved;
            base.OnAttachedTo(view);
        }

        public void ChildAdded(object sender, ElementEventArgs args)
        {
            ;
        }

        public void ChildRemoved(object sender, ElementEventArgs args)
        {
            ;
        }

    }
}
